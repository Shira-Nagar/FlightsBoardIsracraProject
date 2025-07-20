# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln .
COPY FlightsApi/*.csproj ./FlightsApi/
RUN dotnet restore
COPY . .
WORKDIR /src/FlightsApi
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "FlightsApi.dll"] 