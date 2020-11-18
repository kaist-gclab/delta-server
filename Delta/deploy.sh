#!/usr/bin/env bash
IMAGE="delta-app-server"
REMOTE="delta-test"

docker save $IMAGE | lz4 | ssh $REMOTE "lz4 -dc | docker load" && \
cat remote.sh | ssh $REMOTE
