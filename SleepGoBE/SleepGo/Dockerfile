FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-dev
WORKDIR /app

COPY . ./
RUN dotnet restore SleepGo.sln
RUN dotnet publish SleepGo.Api/SleepGo.Api.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-dev /app/out .

ENTRYPOINT ["dotnet", "SleepGo.Api.dll", "--migrate"]