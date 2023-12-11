#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./green-craze-be-v1.API/green-craze-be-v1.API.csproj", "green-craze-be-v1.API/"]
COPY ["./green-craze-be-v1.Application/green-craze-be-v1.Application.csproj", "green-craze-be-v1.Application/"]
COPY ["./green-craze-be-v1.Domain/green-craze-be-v1.Domain.csproj", "green-craze-be-v1.Domain/"]
COPY ["./green-craze-be-v1.Infrastructure/green-craze-be-v1.Infrastructure.csproj", "green-craze-be-v1.Infrastructure/"]

RUN dotnet restore "/src/green-craze-be-v1.API/green-craze-be-v1.API.csproj"
COPY . .
WORKDIR "/src/green-craze-be-v1.API"
RUN dotnet build "green-craze-be-v1.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "green-craze-be-v1.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "green-craze-be-v1.API.dll", "--environment=Docker"]