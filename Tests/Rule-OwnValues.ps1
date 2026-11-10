param([Parameter(Mandatory)][string]$ModRoot, [string]$SharedDir)

# Own values of this mod: the hidden settings shortcut (worker, world view), the About identity and source link, no mandatory
# dependency, MayRequire only on the Anomaly DLC. One consumer, nothing shareable; Tests/Test-Mod.ps1 (generated) runs it after
# the shared checks. The shortcut shape (buttonVisible, worker class) is Check-SettingsContract; image sizes are Check-ModBasics.
# Prints findings, exit 1 on any.
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ModRoot).Path
$mod = Join-Path $root 'Mod'
$script:findings = @()

function It([string]$name, [scriptblock]$body) {
    try { $problems = @(& $body | Where-Object { $_ }) } catch { $problems = @("threw: $($_.Exception.GetBaseException().Message)") }
    foreach ($p in $problems) { $script:findings += "[$name] $p" }
}
function Read-Xml([string]$path) { [xml](Get-Content -LiteralPath $path -Raw -Encoding UTF8) }

$defs = @{}
foreach ($file in Get-ChildItem -LiteralPath (Join-Path $mod 'Defs') -Recurse -Filter *.xml) {
    foreach ($def in (Read-Xml $file.FullName).Defs.SelectNodes('*')) { $defs[$def.defName] = $def }
}

It 'Settings shortcut: own worker, also on the world view' {
    $button = $defs['Nelim_ManyHappyReturnsSettings']
    if (!$button) { 'Nelim_ManyHappyReturnsSettings is missing'; return }
    if ($button.workerClass -ne 'ManyHappyReturns.MainButtonWorker_Settings') { 'Wrong shortcut worker' }
    if ($button.validWithoutMap -ne 'true') { 'Settings should also be available on the world view' }
}
It 'About identity, source link, no mandatory dependency' {
    $about = (Read-Xml (Join-Path $mod 'About/About.xml')).ModMetaData
    $url = 'https://github.com/vbardales/Rimworld-Many-Happy-Returns'
    if ($about.packageId -ne 'nelim.manyhappyreturns') { 'Stable packageId changed' }
    if ($about.url -ne $url) { 'Incorrect source URL' }
    if (!$about.description.TrimEnd().EndsWith("Source code on GitHub ($url)")) { 'Final description source link missing' }
    if ($about.modDependencies) { 'Unexpected mandatory dependency' }
}
It 'Only the Anomaly DLC is a conditional dependency' {
    foreach ($def in $defs.Values) {
        foreach ($node in $def.SelectNodes('.//*[@MayRequire]')) {
            if ($node.MayRequire -ne 'Ludeon.RimWorld.Anomaly') { "Unexpected conditional dependency: $($node.MayRequire)" }
        }
    }
}

if ($script:findings.Count) { $script:findings | ForEach-Object { Write-Output $_ }; exit 1 }
Write-Output 'PASS: shortcut worker, metadata, optional DLC guard'
