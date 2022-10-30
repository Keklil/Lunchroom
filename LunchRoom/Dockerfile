#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LunchRoom/LunchRoom.csproj", "LunchRoom/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["LoggerService/LoggerService.csproj", "LoggerService/"]
COPY ["Services/Services.csproj", "Services/"]

RUN dotnet restore "LunchRoom/LunchRoom.csproj"
COPY . .
WORKDIR "/src/LunchRoom"
RUN dotnet build "LunchRoom.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LunchRoom.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/Html
COPY ["LunchRoom/Html/email.html", "Html/"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LunchRoom.dll"]