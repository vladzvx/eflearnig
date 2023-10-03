FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EF.Learning/EF.Learning.csproj", "EF.Learning/"]
RUN dotnet restore "EF.Learning/EF.Learning.csproj"
COPY . .
WORKDIR "/src/EF.Learning"
RUN dotnet build "EF.Learning.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EF.Learning.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EF.Learning.dll"]