FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as builder
WORKDIR /app

COPY *.sln .
COPY Delta/*.csproj ./Delta/
COPY Delta.AppServer/*.csproj ./Delta.AppServer/
COPY Delta.AppServer.Test/*.csproj ./Delta.AppServer.Test/
RUN dotnet restore --runtime linux-musl-x64

COPY . .
WORKDIR /app/Delta.AppServer
RUN dotnet publish --self-contained --configuration Release --runtime linux-musl-x64 -o out --no-restore

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine
WORKDIR /app
COPY --from=builder /app/Delta.AppServer/out .
EXPOSE 80
ENTRYPOINT ["./Delta.AppServer"]
