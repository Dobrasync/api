# Use the official .NET Runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Copy the built binaries and DLLs into the image
COPY ./temp/publish/ ./

# Expose the port your application runs on (default is 80)
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "YourProjectName.dll"]

