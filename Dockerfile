FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["LoanDecisioning.API/LoanDecisioning.API.csproj", "LoanDecisioning.API/"]
COPY ["LoanDecisioning.Data/LoanDecisioning.Data.csproj", "LoanDecisioning.Data/"]
COPY ["LoanDecisioning.Core/LoanDecisioning.Core.csproj", "LoanDecisioning.Core/"]
RUN dotnet restore "LoanDecisioning.API/LoanDecisioning.API.csproj"
COPY . .
WORKDIR "/src/LoanDecisioning.API"
RUN dotnet build "LoanDecisioning.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoanDecisioning.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanDecisioning.API.dll"]