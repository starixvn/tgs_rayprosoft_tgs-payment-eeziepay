#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Tgs.Payment.EeziePay/Tgs.Payment.EeziePay.csproj", "Tgs.Payment.EeziePay/"]
COPY ["EeziePay.Repositories/EeziePay.Repositories.csproj", "EeziePay.Repositories/"]
COPY ["EeziePay.Core/EeziePay.Core.csproj", "EeziePay.Core/"]
COPY ["Tgs.Payment.EeziePay.Client/Tgs.Payment.EeziePay.Client.csproj", "Tgs.Payment.EeziePay.Client/"]
RUN dotnet restore "Tgs.Payment.EeziePay/Tgs.Payment.EeziePay.csproj"
COPY . .
WORKDIR "/src/Tgs.Payment.EeziePay"
RUN dotnet build "Tgs.Payment.EeziePay.csproj" -c ${BuildConfiguration} -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Payment.EeziePay.csproj" -c ${BuildConfiguration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Payment.EeziePay.dll"]