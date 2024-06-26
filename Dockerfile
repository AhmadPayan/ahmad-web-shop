# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["src/WebShop.Api/WebShop.Api.csproj", "WebShop.Api/"]
COPY ["src/WebShop.Application/WebShop.Application.csproj", "WebShop.Application/"]
COPY ["src/WebShop.Domain/WebShop.Domain.csproj", "WebShop.Domain/"]
COPY ["src/WebShop.Infrastructure/WebShop.Infrastructure.csproj", "WebShop.Infrastructure/"]
COPY ["src/WebShop.Lambda/WebShop.Lambda.csproj", "WebShop.Lambda/"]

RUN dotnet restore "WebShop.Api/WebShop.Api.csproj"
RUN dotnet restore "WebShop.Lambda/WebShop.Lambda.csproj"

# Build the project
WORKDIR "/src/WebShop.Api"

# Copy the entire source code
COPY . .

RUN dotnet build "WebShop.Api.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "WebShop.Api.csproj" -c Release -o /app/publish

# Use the ASP.NET Core runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/WebShop.Api/appsettings.json .
ENTRYPOINT ["dotnet", "WebShop.Api.dll"]