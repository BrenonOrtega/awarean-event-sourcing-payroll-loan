FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY ./src/Awarean.EventSourcing.PayrollLoans.Api/Awarean.EventSourcing.PayrollLoans.Api.csproj Awarean.EventSourcing.PayrollLoans.Api/

FROM base AS build
WORKDIR /app
RUN dotnet restore Awarean.EventSourcing.PayrollLoans.Api
COPY ./src/Awarean.EventSourcing.PayrollLoans.Api/ Awarean.EventSourcing.PayrollLoans.Api
RUN dotnet build Awarean.EventSourcing.PayrollLoans.Api

FROM build AS publish
WORKDIR /app
RUN dotnet publish Awarean.EventSourcing.PayrollLoans.Api -c Release -o publish  --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
WORKDIR /publish
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT [ "dotnet", "Awarean.EventSourcing.PayrollLoans.Api.dll" ]
