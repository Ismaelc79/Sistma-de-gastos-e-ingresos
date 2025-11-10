## Multi-stage Dockerfile for backend-dotnet WebApi on Render

# -------- build --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files for better restore caching
COPY backend-dotnet/src/Application/Application.csproj backend-dotnet/src/Application/
COPY backend-dotnet/src/Domain/Domain.csproj backend-dotnet/src/Domain/
COPY backend-dotnet/src/Infrastructure/Infrastructure.csproj backend-dotnet/src/Infrastructure/
COPY backend-dotnet/src/Presentation/WebApi/WebApi.csproj backend-dotnet/src/Presentation/WebApi/

RUN dotnet restore backend-dotnet/src/Presentation/WebApi/WebApi.csproj

# Copy the rest of the source
COPY backend-dotnet ./backend-dotnet

# Publish
RUN dotnet publish backend-dotnet/src/Presentation/WebApi/WebApi.csproj -c Release -o /app


# -------- runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render will pass PORT=5000, so we just bind to 0.0.0.0:5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "WebApi.dll"]