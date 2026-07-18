FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fightcore.FeedbackHub/Fightcore.FeedbackHub.csproj", "Fightcore.FeedbackHub/"]
RUN dotnet restore "Fightcore.FeedbackHub/Fightcore.FeedbackHub.csproj"
COPY . .
WORKDIR "/src/Fightcore.FeedbackHub"
RUN dotnet build "./Fightcore.FeedbackHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fightcore.FeedbackHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fightcore.FeedbackHub.dll"]
