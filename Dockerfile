FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App
ENV TZ="America/New_York"
ENV DOTNET_EnableWriteXorExecute=0

# Copy everything
COPY ./OppKanban/ ./
# Restore as distinct layers
# RUN dotnet restore
RUN rm -rf ./bin
RUN rm -rf ./obj
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build-env /App/out .
RUN mkdir /var/keys
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT ["dotnet", "OppKanban.dll"]
