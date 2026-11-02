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
