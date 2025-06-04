# kill-port-19888.ps1
$port = 19888
$processes = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess -Unique
foreach ($pid in $processes) {
    if ($pid -ne $null) {
        Write-Host "Killing process $pid using port $port"
        Stop-Process -Id $pid -Force
    }
}
