FROM microsoft/dotnet:3.0-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 1337

FROM microsoft/dotnet:3.0-sdk AS build
WORKDIR /src
COPY ["TheMachine/TheMachine.csproj", "TheMachine/"]
RUN dotnet restore "TheMachine/TheMachine.csproj"
COPY . .
WORKDIR "/src/TheMachine"
RUN dotnet build "TheMachine.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TheMachine.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TheMachine.dll"]