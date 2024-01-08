#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VisitorBook.UI/VisitorBook.UI.csproj", "VisitorBook.UI/"]
COPY ["VisitorBook.BL/VisitorBook.BL.csproj", "VisitorBook.BL/"]
COPY ["VisitorBook.Core/VisitorBook.Core.csproj", "VisitorBook.Core/"]
COPY ["VisitorBook.DAL/VisitorBook.DAL.csproj", "VisitorBook.DAL/"]
RUN dotnet restore "./VisitorBook.UI/./VisitorBook.UI.csproj"
COPY . .
WORKDIR "/src/VisitorBook.UI"
RUN dotnet build "./VisitorBook.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VisitorBook.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VisitorBook.UI.dll"]