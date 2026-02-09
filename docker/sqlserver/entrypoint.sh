#!/bin/bash

set -e

/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."
sleep 25s

/opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "$SA_PASSWORD" \
    -C \
    -i /docker-entrypoint-initdb.d/init.sql

wait
