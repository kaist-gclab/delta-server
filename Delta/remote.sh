#!/usr/bin/env bash
: "${DELTA_JWT_SECRET?DELTA_JWT_SECRET}"
: "${DELTA_AUTH_ADMIN_PASSWORD?DELTA_AUTH_ADMIN_PASSWORD}"
: "${DELTA_CONNECTION_STRING_DATABASE?DELTA_CONNECTION_STRING_DATABASE}"
: "${DELTA_OBJECT_STORAGE_ENDPOINT?DELTA_OBJECT_STORAGE_ENDPOINT}"
: "${DELTA_OBJECT_STORAGE_ACCESS_KEY?DELTA_OBJECT_STORAGE_ACCESS_KEY}"
: "${DELTA_OBJECT_STORAGE_SECRET_KEY?DELTA_OBJECT_STORAGE_SECRET_KEY}"

sudo docker load --input docker-image.tar && \
(sudo docker stop delta-app-server || true) && \
(sudo docker rm delta-app-server || true) && \
sudo docker run -d --restart=unless-stopped \
$DELTA_SERVER_RUN_OPTIONS \
-e "Jwt__Secret=$DELTA_JWT_SECRET" \
-e "Auth__AdminPassword=$DELTA_AUTH_ADMIN_PASSWORD" \
-e "ConnectionStrings__DeltaDatabase=$DELTA_CONNECTION_STRING_DATABASE" \
-e "ObjectStorage__Endpoint=$DELTA_OBJECT_STORAGE_ENDPOINT" \
-e "ObjectStorage__AccessKey=$DELTA_OBJECT_STORAGE_ACCESS_KEY" \
-e "ObjectStorage__SecretKey=$DELTA_OBJECT_STORAGE_SECRET_KEY" \
--name delta-app-server delta-app-server && \
rm docker-image.tar
