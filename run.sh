#!/bin/bash

set -e

if [ "$#" -eq 0 ]; then
	docker compose up --build
else
	export MongoDb__ConnectionString="$1"
    docker build -t oppkanban-app .
	docker run -e MongoDb__ConnectionString="$MongoDb__ConnectionString" oppkanban-app
fi