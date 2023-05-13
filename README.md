LunchRoom

## Запуск
```bash
dotnet run --project ./LunchRoom/LunchRoom.csproj --configuration Development
dotnet run --project ./Client/ClientV2.csproj --configuration Development
```

## Требуемое окружение

1. Postgres с PostGIS (postgis/postgis:14-3.3-alpine)
2. Traefik v2 (traefik:v2.9.9)