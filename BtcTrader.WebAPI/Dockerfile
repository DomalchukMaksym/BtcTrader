#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BtcTrader.WebAPI/BtcTrader.WebAPI.csproj", "BtcTrader.WebAPI/"]
COPY ["BtcTrader.ExchangeServices/BtcTrader.ExchangeServices.csproj", "BtcTrader.ExchangeServices/"]
COPY ["BtcTrader.Models/BtcTrader.Models.csproj", "BtcTrader.Models/"]
RUN dotnet restore "BtcTrader.WebAPI/BtcTrader.WebAPI.csproj"
COPY . .
WORKDIR "/src/BtcTrader.WebAPI"
RUN dotnet build "BtcTrader.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BtcTrader.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BtcTrader.WebAPI.dll"]