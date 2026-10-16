#!/bin/bash

set -e

if [ "$#" -ge 1 ] && [ "$1" = "ai" ]; then
	reranker_size="${2:-0.6B}"
	case "$reranker_size" in
		0.6b|0.6B) export RERANKER_SIZE=0.6B ;;
		4b|4B) export RERANKER_SIZE=4B ;;
		*)
			echo "Usage: $0 ai [0.6b|4b]" >&2
			exit 2
			;;
	esac
	if [ "$#" -gt 2 ]; then
		echo "Usage: $0 ai [0.6b|4b]" >&2
		exit 2
	fi
	docker compose --profile ai up --build
elif [ "$#" -eq 0 ]; then
	docker compose up --build
else
	export MongoDb__ConnectionString="$1"
    docker build -t oppkanban-app .
	docker run -e MongoDb__ConnectionString="$MongoDb__ConnectionString" oppkanban-app
fi