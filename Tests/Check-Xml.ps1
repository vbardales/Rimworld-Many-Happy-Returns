param([string]$Root = (Split-Path $PSScriptRoot -Parent))
$ErrorActionPreference = 'Stop'
function Assert($condition, [string]$message) { if (!$condition) { throw $message } }
function Read-Xml([string]$path) { return [xml](Get-Content -LiteralPath $path -Raw) }
$mod = Join-Path $Root 'Mod'
$files = @(Get-ChildItem -LiteralPath $mod -Recurse -Filter *.xml)
foreach ($file in $files) { $null = Read-Xml $file.FullName }
Write-Output "PASS: $($files.Count) distributed XML files parse"

$defs = @{}
foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod 'Defs') -Recurse -Filter *.xml) {
    foreach ($def in (Read-Xml $file.FullName).Defs.SelectNodes('*')) {
        Assert (!$defs.ContainsKey($def.defName)) "Duplicate defName $($def.defName)"
        $defs[$def.defName] = $def
    }
}
$keyed = @{}
foreach ($language in 'English','French') {
    $entries = @{}
    foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod "Languages/$language/Keyed") -Recurse -Filter *.xml) {
        foreach ($node in (Read-Xml $file.FullName).LanguageData.SelectNodes('*')) {
            Assert (!$entries.ContainsKey($node.Name)) "Duplicate $language key $($node.Name)"
            Assert (![string]::IsNullOrWhiteSpace($node.InnerText)) "Empty $language key $($node.Name)"
            Assert ($node.InnerText -notmatch 'TODO|TODO_TRANSLATE') "Untranslated $language key $($node.Name)"
            $entries[$node.Name] = $node.InnerText
        }
    }
    $keyed[$language] = $entries
}
$source = (Get-ChildItem -LiteralPath (Join-Path $Root 'Source') -Filter *.cs | ForEach-Object { Get-Content -LiteralPath $_.FullName -Raw }) -join "`n"
$used = @([regex]::Matches($source, '"(ManyHappyReturns\.[^"]+)"\s*\.Translate\(') | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique)
foreach ($key in $used) {
    foreach ($language in 'English','French') { Assert ($keyed[$language].ContainsKey($key)) "Missing $language source key $key" }
    $enTokens = @([regex]::Matches($keyed.English[$key], '\{[^}]+\}').Value | Sort-Object -Unique)
    $frTokens = @([regex]::Matches($keyed.French[$key], '\{[^}]+\}').Value | Sort-Object -Unique)
    Assert (!(Compare-Object $enTokens $frTokens)) "Parameter mismatch for $key"
}
foreach ($language in 'English','French') {
    Assert ($keyed[$language].Count -eq $used.Count) "Unused or unmatched keys in $language"
}
Write-Output "PASS: $($used.Count) source keys covered in EN/FR, nonempty, unique, with matching parameters"

# This mod owns only ThoughtDef stage text, InteractionDef grammar and MainButtonDef text.
# Require every owned source string and resolve every French injection back to source XML.
$expected = @{}
foreach ($def in $defs.Values) {
    foreach ($node in $def.SelectNodes('label|description|logRulesInitiator/rulesStrings')) {
        $field = if ($node.Name -eq 'rulesStrings') { 'logRulesInitiator.rulesStrings' } else { $node.Name }
        $expected["$($def.defName).$field"] = $node
    }
    foreach ($stage in $def.SelectNodes('stages/li')) {
        $handle = ([string]$stage.label) -replace '[^a-zA-Z0-9_]', '_'
        foreach ($node in $stage.SelectNodes('label|description')) { $expected["$($def.defName).stages.$handle.$($node.Name)"] = $node }
    }
}
$seen = @{}
foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod 'Languages/French/DefInjected') -Recurse -Filter *.xml) {
    foreach ($node in (Read-Xml $file.FullName).LanguageData.SelectNodes('*')) {
        Assert ($expected.ContainsKey($node.Name)) "Unresolved injection $($node.Name)"
        Assert (!$seen.ContainsKey($node.Name)) "Duplicate injection $($node.Name)"
        Assert (![string]::IsNullOrWhiteSpace($node.InnerText)) "Empty injection $($node.Name)"
        $defName = $node.Name.Split('.')[0]
        Assert ($file.Directory.Name -eq $defs[$defName].Name) "Wrong DefInjected type for $($node.Name)"
        if ($node.Name.EndsWith('.rulesStrings')) {
            Assert ($node.SelectNodes('li').Count -eq $expected[$node.Name].SelectNodes('li').Count) 'Grammar alternative count differs'
            $enTokens = @([regex]::Matches($expected[$node.Name].InnerText, '\[[^\]]+\]').Value | Sort-Object -Unique)
            $frTokens = @([regex]::Matches($node.InnerText, '\[[^\]]+\]').Value | Sort-Object -Unique)
            Assert (!(Compare-Object $enTokens $frTokens)) 'Grammar tokens differ'
        }
        $seen[$node.Name] = $true
    }
}
Assert ($seen.Count -eq $expected.Count) 'French DefInjected coverage is incomplete'
Write-Output "PASS: $($seen.Count) French injection paths and native English source strings covered"

$button = $defs['Nelim_ManyHappyReturnsSettings']
Assert ($button.buttonVisible -eq 'false') 'Settings shortcut must be hidden by default'
Assert ($button.workerClass -eq 'ManyHappyReturns.MainButtonWorker_Settings') 'Wrong shortcut worker'
Assert ($button.validWithoutMap -eq 'true') 'Settings should also be available on the world view'
$about = (Read-Xml (Join-Path $mod 'About/About.xml')).ModMetaData
$url = 'https://github.com/vbardales/Rimworld-Many-Happy-Returns'
Assert ($about.packageId -eq 'nelim.manyhappyreturns') 'Stable packageId changed'
Assert ($about.url -eq $url) 'Incorrect source URL'
Assert ($about.description.TrimEnd().EndsWith("[url=$url]Source code on GitHub[/url]")) 'Final description source link missing'
Assert (!$about.modDependencies) 'Unexpected mandatory dependency'
foreach ($node in $defs.Values.SelectNodes('.//*[@MayRequire]')) { Assert ($node.MayRequire -eq 'Ludeon.RimWorld.Anomaly') 'Unexpected conditional dependency' }
foreach ($name in 'LICENSE','ATTRIBUTION.md') {
    Assert ((Get-FileHash (Join-Path $Root $name)).Hash -eq (Get-FileHash (Join-Path $mod $name)).Hash) "Distribution copy differs: $name"
}
Add-Type -AssemblyName System.Drawing
foreach ($spec in @(@('ModIcon.png',128,128),@('Preview.png',896,504))) {
    $path = Join-Path $mod "About/$($spec[0])"
    $image = [Drawing.Image]::FromFile($path)
    try {
        Assert ($image.Width -eq $spec[1] -and $image.Height -eq $spec[2]) "Wrong dimensions: $($spec[0])"
        Assert ($image.RawFormat.Guid -eq [Drawing.Imaging.ImageFormat]::Png.Guid) "Not a PNG: $($spec[0])"
    } finally { $image.Dispose() }
}
Assert ((Get-Item (Join-Path $mod 'About/Preview.png')).Length -lt 1000000) 'Preview exceeds 1 MB'
Write-Output 'PASS: shortcut defaults, metadata, optional DLC guard, distribution notices and image formats'
