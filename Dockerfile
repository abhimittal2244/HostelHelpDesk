# Use official .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and build
COPY . . 
RUN dotnet publish ./HostelHelpDesk.csproj -c Release -o out

# Use runtime-only image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port (match your launch profile)
EXPOSE 5000

ENTRYPOINT ["dotnet", "HostelHelpDesk.dll"]
