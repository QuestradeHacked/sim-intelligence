FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443

# Copy project resources
COPY src src
COPY nuget.config ./
COPY .editorconfig ./
COPY Questrade.FinCrime.Identity.Intelligence.sln Questrade.FinCrime.Identity.Intelligence.sln
RUN dotnet restore --locked-mode ./src/WebApi/WebApi.csproj --configfile nuget.config

# Test steps
FROM build as test
RUN dotnet restore --locked-mode src/UnitTests/UnitTests.csproj --configfile nuget.config
RUN dotnet restore --locked-mode src/IntegrationTests/IntegrationTests.csproj --configfile nuget.config
RUN dotnet restore --locked-mode src/ContractTests/ContractTests.csproj --configfile nuget.config
ENTRYPOINT ["dotnet", "test" ]

# Publishing the application
FROM build AS publish
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o /app/Questrade.FinCrime.Identity.Intelligence --no-restore

# Final image wrap-up
FROM gcr.io/qt-shared-services-3w/dotnet:6.0 as runtime
WORKDIR /app
COPY --from=publish /app/Questrade.FinCrime.Identity.Intelligence .
USER dotnet
CMD [ "dotnet", "WebApi.dll" ]

