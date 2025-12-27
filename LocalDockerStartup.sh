#!/bin/bash

set -e  # Exit on any error

composeFile="docker-compose.dev.yml"

echo -e "\033[0;36m🚀 Starting PostgreSQL container...\033[0m"
docker compose -f "$composeFile" up -d db

echo -e "\033[0;33m⏳ Waiting for PostgreSQL to be ready...\033[0m"

maxRetries=15
retry=0
ready=false

while [ "$ready" = false ] && [ $retry -lt $maxRetries ]; do
    containerId=$(docker compose -f "$composeFile" ps -q db)
    if [ -n "$containerId" ]; then
        result=$(docker exec "$containerId" pg_isready -U kvblog 2>/dev/null)
        if echo "$result" | grep -q "accepting connections"; then
            ready=true
            break
        fi
    fi

    retry=$((retry + 1))
    sleep 2
    echo "Waiting for DB... ($retry/$maxRetries)"
done

if [ "$ready" = false ]; then
    echo -e "\033[0;31m❌ Postgres did not become ready in time. Aborting.\033[0m"
    exit 1
fi

echo -e "\033[0;32m✅ DB is ready. Running EF Core migrations...\033[0m"
if ! docker compose -f "$composeFile" run --rm migrator; then
    echo -e "\033[0;31m❌ Migrations failed. Aborting.\033[0m"
    exit 1
fi

echo -e "\033[0;36m⬆️ Starting API and Client containers...\033[0m"
docker compose -f "$composeFile" up -d api client

echo ""
echo -e "\033[0;32m✅ All services started successfully!\033[0m"
echo ""
echo "API: http://localhost:8080"
echo "Client: http://localhost:8081"
echo ""
echo "View logs: docker compose -f $composeFile logs -f"
echo "Stop all: docker compose -f $composeFile down"
