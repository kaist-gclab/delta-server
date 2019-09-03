FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine

WORKDIR /app
COPY ./netcoreapp2.2/linux-musl-x64/publish .

EXPOSE 80

ENTRYPOINT ["./Delta.AppServer"]
