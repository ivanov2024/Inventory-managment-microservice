FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InventoryManagment.csproj", "."]
RUN dotnet restore "./InventoryManagment.csproj"
COPY . .
RUN dotnet publish "./InventoryManagment.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "InventoryManagment.dll"]