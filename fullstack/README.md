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
  cd API
  dotnet tool install --global dotnet-ef
  dotnet ef migrations add Initial
```

## Run the API with hot reloading ##

```zsh
  dotnet watch run
```