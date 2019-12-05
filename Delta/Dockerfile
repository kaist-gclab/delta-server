FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
COPY . /app
WORKDIR /app/Delta.AppServer
RUN dotnet publish --self-contained --configuration Release --runtime linux-musl-x64 -o out

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=builder /app/Delta.AppServer/out .
EXPOSE 80
ENTRYPOINT ["./Delta.AppServer"]
