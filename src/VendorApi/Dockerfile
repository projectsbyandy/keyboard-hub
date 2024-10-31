FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VendorApi/VendorApi.csproj", "VendorApi/"]
RUN dotnet restore "VendorApi/VendorApi.csproj"
COPY . .
WORKDIR "/src/VendorApi"
RUN dotnet build "VendorApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VendorApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VendorApi.dll"]
