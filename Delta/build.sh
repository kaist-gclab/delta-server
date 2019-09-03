cd Delta.AppServer && \
rm -rf ./bin/Release/ && \
dotnet publish --self-contained --configuration Release --runtime linux-musl-x64 && \
cd bin/Release && \
docker build -t delta-app-server -f ../../../Dockerfile .
