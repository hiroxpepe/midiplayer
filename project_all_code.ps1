# powershell -NoProfile -ExecutionPolicy Bypass -File .\project_all_code.ps1 project_all_code.md -ExcludeProjects 'MidiPlayer.Droid','MidiPlayer.Win64'

Param(
    [string]$OutFile = "project_all_code.md",
    [string[]]$ExcludeProjects = @()
)

# Repository root (current directory)
$repo = (Get-Location).ProviderPath
$fullOut = Join-Path -Path $repo -ChildPath $OutFile

if (Test-Path $fullOut) { Remove-Item $fullOut -Force }

# Header
Out-File -FilePath $fullOut -Encoding utf8 -InputObject '# Aggregated C# sources'
Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ('Repository: {0}' -f $repo)
Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ('Date: {0}' -f (Get-Date -Format u))
Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ''

# Exclude common build/metadata directories by checking relative path segments
$excludeDirs = @('bin','obj','.git','.vs','packages','TestResults') + $ExcludeProjects

$files = Get-ChildItem -Path $repo -Recurse -Filter *.cs -File | Sort-Object FullName
foreach ($f in $files) {
    $rel = $f.FullName.Substring($repo.Length + 1)
    $skip = $false
    # If specific projects were requested to be excluded, skip files whose relative path starts with those project names
    if ($ExcludeProjects -and $ExcludeProjects.Length -gt 0) {
        foreach ($p in $ExcludeProjects) {
            $pn = $p.ToString().Trim()
            if ($pn -eq '') { continue }
            if ($rel.StartsWith($pn + '\\')) { $skip = $true; break }
        }
        if ($skip) { continue }
    }

    # split relative path into segments and check for excluded directory names (like bin/obj/.git)
    $segments = $rel -split '\\'
    foreach ($d in $excludeDirs) {
        if ($segments -contains $d) { $skip = $true; break }
    }
    if ($skip) { continue }

    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ('## {0}' -f $rel)
    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ''
    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject '```csharp'
    $content = Get-Content -LiteralPath $f.FullName -Raw
    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject $content
    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject '```'
    Out-File -FilePath $fullOut -Encoding utf8 -Append -InputObject ''
}

# Completed aggregation
