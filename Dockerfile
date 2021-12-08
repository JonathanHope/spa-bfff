FROM node:15 AS frontend
WORKDIR /src
COPY src/ClientApp/package.json .
COPY src/ClientApp/package-lock.json .
RUN npm ci
COPY src/ClientApp/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend
WORKDIR /src
COPY src/ .
COPY --from=frontend /src/dist ./wwwroot
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
EXPOSE 80
WORKDIR /app
COPY --from=backend /app .

ENTRYPOINT ["dotnet", "ViteBff.dll"]
