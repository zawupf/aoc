Get-ChildItem -Name $PSScriptRoot -Include Day*.fsx -Exclude Day00.fsx | ForEach-Object {
    $file = Join-Path $PSScriptRoot $_ | Get-Item

    $day = $file.BaseName
    Write-Output "`n$day`:"

    $filepath = $file.FullName
    & dotnet fsi $filepath
}
