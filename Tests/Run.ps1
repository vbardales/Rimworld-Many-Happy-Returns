param([string]$GameManaged = 'C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed')
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$results = Join-Path $PSScriptRoot 'Results'
New-Item -ItemType Directory -Force -Path $results | Out-Null
Push-Location $root
try {
    # Build and run of the test project: the shared script (scripts/ of the protocols repo), not repeated here.
    & powershell -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root '../scripts/Run-UnitTests.ps1') -ModRoot $root -ManagedPath $GameManaged -PassManagedToExe *> (Join-Path $results 'automated.txt')
    if ($LASTEXITCODE -ne 0) { throw 'Build or behavior tests failed; see Tests/Results/automated.txt' }
    & ./Tests/Check-Xml.ps1 *> (Join-Path $results 'xml.txt')
    $testedHash = (Get-FileHash build/tests/bin/ManyHappyReturns.dll).Hash
    $deliveredHash = (Get-FileHash Mod/Assemblies/ManyHappyReturns.dll).Hash
    if ($testedHash -ne $deliveredHash) { throw 'Tested and distributed DLLs differ' }
    $files = @(Get-ChildItem Source,Mod -Recurse -File) + @(Get-ChildItem Tests -File | Where-Object Extension -In '.cs','.csproj','.props','.ps1')
    $hashes = foreach ($file in $files | Sort-Object FullName) {
        [ordered]@{path=$file.FullName.Substring($root.Length+1).Replace('\','/'); sha256=(Get-FileHash $file.FullName).Hash}
    }
    [ordered]@{
        executed_at=(Get-Date -Format o)
        base_revision=(& git rev-parse HEAD)
        scope='Working tree; behavior and XML checks, not in-game validation'
        game_assembly=$GameManaged
        game_assembly_sha256=(Get-FileHash (Join-Path $GameManaged 'Assembly-CSharp.dll')).Hash
        distributed_dll_sha256=$deliveredHash
        files=$hashes
    } | ConvertTo-Json -Depth 6 | Set-Content (Join-Path $results 'manifest.json') -Encoding utf8
    Get-Content (Join-Path $results 'automated.txt')
    Get-Content (Join-Path $results 'xml.txt')
    Write-Output 'PASS: tested DLL matches distributed DLL; file hashes recorded in Tests/Results/manifest.json'
} finally { Pop-Location }
