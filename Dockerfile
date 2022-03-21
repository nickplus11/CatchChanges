FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /source

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet

WORKDIR /app

COPY --from=build-env /source/out .

ENTRYPOINT ["dotnet", "CatchChangesREST.dll"]