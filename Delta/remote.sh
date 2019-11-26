#!/usr/bin/env bash
sudo docker load --input docker-image.tar && \
(sudo docker stop delta-app-server || true) && \
(sudo docker rm delta-app-server || true) && \
sudo docker run -d --restart=unless-stopped \
--name delta-app-server delta-app-server && \
rm docker-image.tar
