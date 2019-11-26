#!/usr/bin/env bash
docker save --output docker-image.tar delta-app-server && \
scp ./docker-image.tar gclab-beta:~ && \
cat remote.sh | ssh gclab-beta && \
rm docker-image.tar
