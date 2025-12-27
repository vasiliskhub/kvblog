#!/bin/bash
set -e

echo "📦 Installing dotnet-ef tool..."
dotnet tool install --global dotnet-ef --version 10.* || dotnet tool update --global dotnet-ef --version 10.*
export PATH="$PATH:/root/.dotnet/tools"

echo "📥 Restoring NuGet packages for entire solution..."
cd /app
dotnet restore

echo "🔁 Running EF Core migrations..."
cd /app/kvblog.api.db
dotnet ef database update