FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as builder
COPY . /app
WORKDIR /app/Delta.AppServer
RUN dotnet publish --self-contained --configuration Release --runtime linux-musl-x64

FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine
WORKDIR /app
COPY --from=builder /app/Delta.AppServer/bin/Release/netcoreapp2.2/linux-musl-x64/publish .
EXPOSE 80
ENTRYPOINT ["./Delta.AppServer"]
