FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

COPY /PlaneFX .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "PlaneFX.dll"]