#!/bin/bash
set -e

dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

echo "🔁 Running EF Core migrations..."
dotnet ef database update