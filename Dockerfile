# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["smartTaskTracker.API.csproj", "./"]
RUN dotnet restore "smartTaskTracker.API.csproj"

# Copy everything else
COPY . .

# Build
RUN dotnet build "smartTaskTracker.API.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "smartTaskTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 10000

# Set environment
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# Run the app
ENTRYPOINT ["dotnet", "smartTaskTracker.API.dll"]