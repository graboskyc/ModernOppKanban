# ModernOppKanban

## Background

Basic tracking tool that allows exporting a view from SFDC to CSV, importing it here, then managing custom metadata for SAs to administer and run their business.

SFDC remains the source of truth and you can re-import over top without losing SA information.

## Running

`docker compose up`

Visit `http://localhost:80`

### Running the llama.cpp reranker

The llama.cpp reranker is an optional Docker Compose service and runs on CPU, including on Apple Silicon Macs. Start the app and reranker together with the default 0.6B model:

`docker compose --profile ai up --build`

Or use `./run.sh ai`. To select the 4B model, run `./run.sh ai 4b` or set `RERANKER_SIZE=4B` before `docker compose --profile ai up --build`. Docker Desktop containers on macOS do not use Apple's Metal GPU, so reranking runs on CPU. The app reaches llama.cpp over the Compose network at `http://llama-reranker:8000/`; the server port is not published to the host. Both sizes use Q4_K_M quantization, are downloaded from Hugging Face on first start, and are cached in the `llama-cache` volume. The 4B model is larger and will use more memory and run more slowly on CPU. To use an external compatible reranker server instead, set `Reranker__BaseUrl` before starting the app; from Docker Compose, a host server is reachable at `http://host.docker.internal:8000/`.

Without the reranker, start the regular app with `docker compose up`. To use an external compatible reranker server, set `Reranker__BaseUrl` to its URL before starting the app.

### Connecting to MongoDB from the host

The MongoDB container publishes port `27017` on the host. Since the replica set advertises the Docker-only hostname `mongodb`, host-side clients must use a direct connection:

`mongodb://admin:admin@localhost:27017/OppKanban?authSource=admin&directConnection=true`

The URI uses the default credentials from `docker-compose.yml`; if you set `ADMIN_PASSWORD`, replace `admin` with that password. For an app running directly on the host, set this as `MongoDb__ConnectionString` before starting the app. Apps running in Docker Compose should continue to use `mongodb:27017` without `directConnection=true`.

## Techstack

* Docker
* MongoDB Community with Atlas Search
* Blazor
* Apex Charts

## Screenshots

![](Screenshots/ss01.png)

![](Screenshots/ss02.png)

![](Screenshots/ss03.png)

![](Screenshots/ss04.png)

![](Screenshots/ss05.png)

![](Screenshots/ss06.png)

![](Screenshots/ss07.png)