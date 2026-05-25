# ==========================================
# Stage 2: Build and Publish
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Point this to the subfolder!
COPY ["ECommorceWebAPI/ECommorceWebAPI.csproj", "ECommorceWebAPI/"]
RUN dotnet restore "ECommorceWebAPI/ECommorceWebAPI.csproj"

# 2. Copy the rest of the source code
COPY . .

# 3. Move into the subfolder inside the container before building
WORKDIR "/src/ECommorceWebAPI"
RUN dotnet build "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 4. Update the publish step path as well
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommorceWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false