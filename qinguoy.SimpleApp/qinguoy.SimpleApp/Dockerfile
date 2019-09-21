FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["qinguoy.SimpleApp/qinguoy.SimpleApp.csproj", "qinguoy.SimpleApp/"]
RUN dotnet restore "qinguoy.SimpleApp/qinguoy.SimpleApp.csproj"
COPY . .
WORKDIR "/src/qinguoy.SimpleApp"
RUN dotnet build "qinguoy.SimpleApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "qinguoy.SimpleApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "qinguoy.SimpleApp.dll"]