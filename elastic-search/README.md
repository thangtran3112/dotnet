# Fullstack app with C #

## Setup framework ##

```zsh
  dotnet --list-sdks
  dotnet new globaljson
  # Enter the version from the --list-sdks
  dotnet new webapi --use-controllers -o API
```

## Setup API ##

```zsh
  dotnet tool install --global dotnet-ef
  dotnet ef migrations add Initial
  dotnet ef database update
```

## EF core commands ##

* To apply a specific migration: `dotnet ef database update <MigrationName>`
* To rollback to a previous migration: `dotnet ef database update <PreviousMigrationName>`
* To undo all migrations: `dotnet ef database update 0`

## Cleanup nuget ##

```zsh
  cd backend
  dotnet clean
  dotnet restore
  dotnet build
```

## Run the API with hot reloading ##

```zsh
  dotnet watch run
```

## Start ElasticSearch and Kibana ##

```zsh
  docker-compose up -d
```
