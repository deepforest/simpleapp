FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["SimpleApp.csproj", "."]
RUN dotnet restore "SimpleApp.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "SimpleApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SimpleApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimpleApp.dll"]
