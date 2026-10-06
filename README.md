# ModernOppKanban

## Background

Basic tracking tool that allows exporting a view from SFDC to CSV, importing it here, then managing custom metadata for SAs to administer and run their business.

SFDC remains the source of truth and you can re-import over top without losing SA information.

## Running

`docker compose up`

Visit `http://localhost:80`

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