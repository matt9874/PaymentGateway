FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 80

COPY PaymentGateway.API PaymentGateway.API
COPY PaymentGateway.Application PaymentGateway.Application
COPY PaymentGateway.Domain PaymentGateway.Domain
COPY PaymentGateway.Persistence PaymentGateway.Persistence
RUN dotnet restore PaymentGateway.API/PaymentGateway.API.csproj

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PaymentGateway.API.dll"]
