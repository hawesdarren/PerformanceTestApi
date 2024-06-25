FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app

COPY . . 
RUN dotnet restore PerformanceTestApi/
RUN dotnet publish PerformanceTestApi/ -o /app/published-app

EXPOSE 5235

CMD [ "dotnet", "test" ]