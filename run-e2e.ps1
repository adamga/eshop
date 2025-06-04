# PowerShell script to orchestrate e2e testing for eShop
# 1. Load .env variables and set them in the current process
# 2. Kill any process on port 19888
# 3. Kill any existing dotnet processes for a clean start
# 4. Wait 20 seconds
# 5. Start eShop.AppHost, capturing all output
# 6. Wait 60 seconds for services to be ready
# 7. Run Playwright tests
# 8. Optionally, stop services (not implemented here)

$ErrorActionPreference = 'Stop'

# --- Load .env variables ---
Write-Host "[0/8] Loading .env variables..."
$envFile = Join-Path $PSScriptRoot ".env"
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match '^(\w+)=(.*)$') {
            $name = $matches[1]
            $value = $matches[2]
            [System.Environment]::SetEnvironmentVariable($name, $value, 'Process')
        }
    }
    Write-Host "Loaded environment variables from .env."
} else {
    Write-Host ".env file not found. Skipping environment variable loading."
}

Write-Host "[1/8] Killing any process on port 19888..."
.\kill-port-19888.ps1

# --- Kill any existing dotnet processes for a clean start ---
Write-Host "[2/8] Killing any existing dotnet processes..."
Get-Process dotnet -ErrorAction SilentlyContinue | ForEach-Object { try { $_ | Stop-Process -Force } catch {} }

# --- Wait 20 seconds ---
Write-Host "[3/8] Waiting 20 seconds before starting AppHost..."
Start-Sleep -Seconds 20

# --- Start eShop.AppHost and capture output ---
Write-Host "[4/8] Starting eShop.AppHost and capturing output..."
$appHostLog = "apphost-startup.log"
$appHostErr = "apphost-startup.err.log"
if (Test-Path $appHostLog) { Remove-Item $appHostLog }
if (Test-Path $appHostErr) { Remove-Item $appHostErr }
$appHostProcess = Start-Process 'dotnet' -ArgumentList 'run --project src/eShop.AppHost/eShop.AppHost.csproj' -RedirectStandardOutput $appHostLog -RedirectStandardError $appHostErr -PassThru

# --- Wait 60 seconds for services to be ready ---
Write-Host "[5/8] Waiting 60 seconds for services to start..."
Start-Sleep -Seconds 60

# --- Check if AppHost process is still running ---
if ($appHostProcess.HasExited) {
    Write-Host "[ERROR] AppHost process exited early. Dumping logs:"
    Write-Host "--- STDOUT ---"
    Get-Content $appHostLog | Write-Host
    Write-Host "--- STDERR ---"
    Get-Content $appHostErr | Write-Host
    throw "AppHost failed to start. See apphost-startup.log and apphost-startup.err.log for details."
}

# --- Run Playwright tests ---
Write-Host "[6/8] Running Playwright tests..."
$npx = if ($IsWindows) { 'npx.cmd' } else { 'npx' }
& $npx playwright test
$playwrightExitCode = $LASTEXITCODE

# --- Optionally, stop AppHost and dependencies ---
Write-Host "[7/8] Stopping AppHost..."
Stop-Process -Id $appHostProcess.Id -Force

if ($playwrightExitCode -ne 0) {
    Write-Host "[ERROR] Playwright tests failed."
    exit $playwrightExitCode
}
