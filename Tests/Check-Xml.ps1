# Mod-specific expected values only. Generic XML, key and injection coverage is done by the shared
# Check-ModBasics.ps1, Check-Translations.ps1 and Check-DefInjected.ps1 (scripts/ of the protocols repo).
param([string]$Root = (Split-Path $PSScriptRoot -Parent))
$ErrorActionPreference = 'Stop'
function Assert($condition, [string]$message) { if (!$condition) { throw $message } }
function Read-Xml([string]$path) { return [xml](Get-Content -LiteralPath $path -Raw) }
$mod = Join-Path $Root 'Mod'
$defs = @{}
foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod 'Defs') -Recurse -Filter *.xml) {
    foreach ($def in (Read-Xml $file.FullName).Defs.SelectNodes('*')) { $defs[$def.defName] = $def }
}

# French agreement: the English pronoun tokens become {PAWN_gender ? m : f : n} switches, so compare what is left
# (the shared Check-Translations counts the English tokens and flags a correct French agreement).
function Get-Keyed([string]$language) {
    $entries = @{}
    foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod "Languages/$language/Keyed") -Recurse -Filter *.xml) {
        foreach ($node in (Read-Xml $file.FullName).LanguageData.SelectNodes('*')) { $entries[$node.Name] = $node.InnerText }
    }
    return $entries
}
$en = Get-Keyed 'English'; $fr = Get-Keyed 'French'
foreach ($key in $en.Keys) {
    $enTokens = @([regex]::Matches($en[$key], '\{[^}]+\}').Value | Where-Object { $_ -notmatch '^\{PAWN_(pronoun|objective|possessive)\}$' } | Sort-Object -Unique)
    $frText = [regex]::Replace($fr[$key], '\{[^{}]*\?[^{}]*\}', '')
    $frTokens = @([regex]::Matches($frText, '\{[^}]+\}').Value | Sort-Object -Unique)
    Assert (!(Compare-Object $enTokens $frTokens)) "Parameter mismatch for $key"
}
Write-Output "PASS: $($en.Count) keyed entries keep their parameters in French"

# The five social-log rules keep their grammar tokens in French.
foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod 'Languages/French/DefInjected') -Recurse -Filter *.xml) {
    foreach ($node in (Read-Xml $file.FullName).LanguageData.SelectNodes('*')) {
        if ($node.Name -notmatch '^(?<def>[^.]+)\.logRulesInitiator\.rulesStrings\.(?<i>\d+)$') { continue }
        $source = @($defs[$Matches.def].SelectNodes('logRulesInitiator/rulesStrings/li'))[[int]$Matches.i].InnerText
        $enTokens = @([regex]::Matches($source, '\[[^\]]+\]').Value | Sort-Object -Unique)
        $frTokens = @([regex]::Matches($node.InnerText, '\[[^\]]+\]').Value | Sort-Object -Unique)
        Assert (!(Compare-Object $enTokens $frTokens)) "Grammar tokens differ: $($node.Name)"
    }
}
Write-Output 'PASS: French log rules keep their grammar tokens'

$button = $defs['Nelim_ManyHappyReturnsSettings']
Assert ($button.buttonVisible -eq 'false') 'Settings shortcut must be hidden by default'
Assert ($button.workerClass -eq 'ManyHappyReturns.MainButtonWorker_Settings') 'Wrong shortcut worker'
Assert ($button.validWithoutMap -eq 'true') 'Settings should also be available on the world view'
$about = (Read-Xml (Join-Path $mod 'About/About.xml')).ModMetaData
$url = 'https://github.com/vbardales/Rimworld-Many-Happy-Returns'
Assert ($about.packageId -eq 'nelim.manyhappyreturns') 'Stable packageId changed'
Assert ($about.url -eq $url) 'Incorrect source URL'
Assert ($about.description.TrimEnd().EndsWith("Source code on GitHub ($url)")) 'Final description source link missing'
Assert (!$about.modDependencies) 'Unexpected mandatory dependency'
foreach ($node in $defs.Values.SelectNodes('.//*[@MayRequire]')) { Assert ($node.MayRequire -eq 'Ludeon.RimWorld.Anomaly') 'Unexpected conditional dependency' }
Add-Type -AssemblyName System.Drawing
foreach ($spec in @(@('ModIcon.png',128,128),@('Preview.png',896,504))) {
    $image = [Drawing.Image]::FromFile((Join-Path $mod "About/$($spec[0])"))
    try { Assert ($image.Width -eq $spec[1] -and $image.Height -eq $spec[2]) "Wrong dimensions: $($spec[0])" } finally { $image.Dispose() }
}
Assert ((Get-Item (Join-Path $mod 'About/Preview.png')).Length -lt 1000000) 'Preview exceeds 1 MB'
Write-Output 'PASS: shortcut defaults, metadata, optional DLC guard and image sizes'
