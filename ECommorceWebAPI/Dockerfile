# ==========================================
# Stage 1: Runtime Base
# ==========================================
# Standard .NET 10 ASP.NET runtime for running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ==========================================
# Stage 2: Build and Publish
# ==========================================
# Use the full .NET 10 SDK to restore and build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ECommorceWebAPI/ECommorceWebAPI.csproj", "ECommorceWebAPI/"]
RUN dotnet restore "ECommorceWebAPI/ECommorceWebAPI.csproj"

# Copy the rest of the source code and build
COPY . .
WORKDIR "/src/ECommorceWebAPI"
RUN dotnet build "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application into a clean folder
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ==========================================
# Stage 3: Final Image
# ==========================================
# Copy the compiled output to the runtime base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommorceWebAPI.dll"]