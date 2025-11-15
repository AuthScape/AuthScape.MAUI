# Register authscape:// protocol for unpackaged Windows app
# Run this script as Administrator

$exePath = "$PSScriptRoot\bin\Debug\net9.0-windows10.0.19041.0\win10-x64\AuthScapeMAUI.exe"

# Create registry entries for the protocol
$regPath = "HKCU:\Software\Classes\authscape"

# Remove existing registration if present
if (Test-Path $regPath) {
    Remove-Item -Path $regPath -Recurse -Force
}

# Create protocol registration
New-Item -Path $regPath -Force | Out-Null
Set-ItemProperty -Path $regPath -Name "(Default)" -Value "URL:AuthScape Protocol"
Set-ItemProperty -Path $regPath -Name "URL Protocol" -Value ""

# Create shell\open\command
New-Item -Path "$regPath\shell\open\command" -Force | Out-Null
Set-ItemProperty -Path "$regPath\shell\open\command" -Name "(Default)" -Value "`"$exePath`" `"%1`""

Write-Host "Protocol 'authscape://' registered successfully!" -ForegroundColor Green
Write-Host "App path: $exePath" -ForegroundColor Cyan
Write-Host ""
Write-Host "Test the protocol by running: start authscape://test" -ForegroundColor Yellow
