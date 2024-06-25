FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app

COPY . . 
RUN dotnet restore PerformanceTestApi/
RUN dotnet publish PerformanceTestApi/ -o /app/published-app

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

EXPOSE 8080
WORKDIR /app
COPY --from=build /app/published-app /app
CMD [ "dotnet", "test" ]