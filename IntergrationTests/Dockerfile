FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app

COPY . . 

EXPOSE 5235

CMD [ "dotnet", "test" ]