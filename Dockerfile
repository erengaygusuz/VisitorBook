FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./VisitorBook.BL/VisitorBook.BL.csproj", "VisitorBook.BL/"]
COPY ["./VisitorBook.Core/VisitorBook.Core.csproj", "VisitorBook.Core/"]
COPY ["./VisitorBook.DAL/VisitorBook.DAL.csproj", "VisitorBook.DAL/"]
COPY ["./VisitorBook.Test/VisitorBook.Test.csproj", "VisitorBook.Test/"]
COPY ["./VisitorBook.UI/VisitorBook.UI.csproj", "VisitorBook.UI/"]
RUN dotnet restore "VisitorBook.UI/VisitorBook.UI.csproj"
COPY . .
WORKDIR "/src/VisitorBook.UI"
RUN dotnet build "VisitorBook.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VisitorBook.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir /app/StaticFiles
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VisitorBook.UI.dll"]