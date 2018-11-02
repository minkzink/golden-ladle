FROM microsoft/dotnet:2.1.403-sdk AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1.403-sdk AS build
WORKDIR /src
COPY GoldenLadle.sln ./
COPY GoldenLadle.Web/GoldenLadle.Web.csproj GoldenLadle.Web/
COPY GoldenLadle.Web/NuGet.config GoldenLadle.Web/
COPY . .
WORKDIR /src/GoldenLadle.Web
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY golden_ladle.pfx .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet GoldenLadle.dll
