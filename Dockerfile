FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Presentation/WebAPI/WebAPI.csproj", "src/Presentation/WebAPI/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Presentation/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR /src/src/Presentation/WebAPI
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "WebAPI.dll"]