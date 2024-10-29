FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KeyboardApi/KeyboardApi.csproj", "KeyboardApi/"]
RUN dotnet restore "KeyboardApi/KeyboardApi.csproj"
COPY . .
WORKDIR "/src/KeyboardApi"
RUN dotnet build "KeyboardApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeyboardApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyboardApi.dll"]
