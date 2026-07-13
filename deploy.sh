#!/bin/bash
set -e

echo "=== Toplusms .NET Deploy ==="

dotnet publish src/Toplusms.Web -c Release -r linux-x64 --self-contained -o publish

rsync -avz --delete publish/ root@37.27.134.97:/opt/toplusms/

ssh root@37.27.134.97 "systemctl restart toplusms"
echo "=== Deploy complete ==="
