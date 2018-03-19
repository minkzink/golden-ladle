FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY GoldenLadle.sln ./
COPY GoldenLadle.Web/GoldenLadle.Web.csproj GoldenLadle.Web/
COPY C:\Users\brinehart\Documents\Projects\GoldenLadle\GoldenLadle.Web\nuget.config GoldenLadle.Web/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/GoldenLadle.Web
RUN dotnet build -c Release -o /app
RUN dotnet ef database update

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GoldenLadle.Web.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet GoldenLadel.Web.dll
