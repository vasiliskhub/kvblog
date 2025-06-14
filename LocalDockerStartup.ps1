#!/usr/bin/env pwsh

$composeFile = "docker-compose.dev.yml"

Write-Host "🚀 Starting PostgreSQL container..." -ForegroundColor Cyan
docker compose -f $composeFile up -d db

Write-Host "⏳ Waiting for PostgreSQL to be ready..." -ForegroundColor Yellow

$maxRetries = 15
$retry = 0
$ready = $false

while (-not $ready -and $retry -lt $maxRetries) {
    try {
        $containerId = docker compose -f $composeFile ps -q db
        $result = docker exec $containerId pg_isready -U kvblog
        if ($result -like "*accepting connections*") {
            $ready = $true
            break
        }
    } catch {
        # Ignore errors while waiting
    }

    $retry++
    Start-Sleep -Seconds 2
    Write-Host "Waiting for DB... ($retry/$maxRetries)"
}

if (-not $ready) {
    Write-Error "❌ Postgres did not become ready in time. Aborting."
    exit 1
}

Write-Host "✅ DB is ready. Running EF Core migrations..." -ForegroundColor Green
docker compose -f $composeFile run --rm migrator

Write-Host "⬆️ Starting API and Client containers..." -ForegroundColor Cyan
docker compose -f $composeFile up -d api client
