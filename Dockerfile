# ==========================================
# Stage 1: Build and Publish
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy csproj
COPY ["ECommorceWebAPI/ECommorceWebAPI.csproj", "ECommorceWebAPI/"]

# Restore packages
RUN dotnet restore "ECommorceWebAPI/ECommorceWebAPI.csproj"

# Copy all files
COPY . .

# Move into project folder
WORKDIR "/src/ECommorceWebAPI"

# Build
RUN dotnet build "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
RUN dotnet publish "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# ==========================================
# Stage 2: Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommorceWebAPI.dll"]