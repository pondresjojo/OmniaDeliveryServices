FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["OmniaDelivery.ValidationService/OmniaDelivery.ValidationService.csproj", "OmniaDelivery.ValidationService/"]
COPY ["OmniaDelivery.Infra/OmniaDelivery.Infra.csproj", "OmniaDelivery.Infra/"]
RUN dotnet restore "OmniaDelivery.ValidationService/OmniaDelivery.ValidationService.csproj"
COPY . .
WORKDIR "/src/OmniaDelivery.ValidationService"
RUN dotnet build "OmniaDelivery.ValidationService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OmniaDelivery.ValidationService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OmniaDelivery.ValidationService.dll"]