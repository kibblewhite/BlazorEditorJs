function Purge-ProjectFiles {
    param (
        [Parameter(Mandatory=$true, Position=0)]
        [string]$RelativePath,
        [switch]$Silent
    )

    if (-not $Silent) {
        Write-Host "Scanning for items..."
    }

    $itemsToRemove = Get-ChildItem "$RelativePath" -Include bin,obj,bld,Backup,_UpgradeReport_Files,Debug,Release,ipch -Recurse
    $totalItems = $itemsToRemove.Count
    $currentItem = 0

    if (-not $Silent) {
        Write-Host "Items found: $($totalItems)"
    }

    foreach ($item in $itemsToRemove) {
        $currentItem++
        $percentComplete = $currentItem / $totalItems * 100
        $status = "Removing $($item.FullName)"
    
        if (-not (Test-Path $item.FullName)) {
            continue
        }
        
        try {
            Remove-Item $item.FullName -Force -Recurse -ErrorAction SilentlyContinue
        } finally {
            if (-not $Silent) {
                Write-Progress -Activity "Removing project files" -Status $status -PercentComplete $percentComplete
                Start-Sleep -Milliseconds 20
            }
        }
    }

    if (-not $Silent) {
        Write-Progress -Activity "Purge completed" -Completed
        Start-Sleep -Milliseconds 200
    }
}

$path = "../"
$full_path = Convert-Path $path

do {
    $confirm = Read-Host "Are you sure you want to recursively purge all project folders [$($full_path)]? (Y/N/?)"
    if ($confirm -eq '?') {
        Write-Host "This will recursively remove all files and folders that match the following names: bin, obj, bld, Backup, _UpgradeReport_Files, Debug, Release, ipch"
    }
} while ($confirm -ne 'Y' -and $confirm -ne 'N')

if ($confirm -ne 'Y') {
    Write-Host "Operation cancelled."
    return
}

Purge-ProjectFiles $full_path -Silent:$false
Write-Host "Please wait..."
Start-Sleep -Seconds 4
Purge-ProjectFiles $full_path -Silent:$true
Write-Host "Purge completed."

if (-not $psISE) {
    Write-Host -NoNewLine "`nPress any key to continue...";
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}
