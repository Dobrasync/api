#region Build src
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish Dobrasync.Api.sln --configuration Release --output /app
#endregion

#region Run app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Expose the port your application runs on (default is 80)
EXPOSE 80

# Set the entry point for the application
COPY --from=build /app /app
WORKDIR /app
RUN ls -lha /app
ENTRYPOINT ["dotnet", "Dobrasync.Api.ApiControllers.dll"]
#endregion
