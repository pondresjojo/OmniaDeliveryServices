FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Omnia.Consumer.Service/OmniaDelivery.ConsumerService.csproj", "Omnia.Consumer.Service/"]
COPY ["OmniaDelivery.Infra/OmniaDelivery.Infra.csproj", "OmniaDelivery.Infra/"]
RUN dotnet restore "Omnia.Consumer.Service/OmniaDelivery.ConsumerService.csproj"
COPY . .
WORKDIR "/src/Omnia.Consumer.Service"
RUN dotnet build "OmniaDelivery.ConsumerService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OmniaDelivery.ConsumerService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OmniaDelivery.ConsumerService.dll"]