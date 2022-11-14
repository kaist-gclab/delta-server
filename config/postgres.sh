#!/usr/bin/env bash
: "${DELTA_POSTGRES_PORT?DELTA_POSTGRES_PORT}"
: "${DELTA_POSTGRES_PASSWORD?DELTA_POSTGRES_PASSWORD}"
: "${DELTA_POSTGRES_USER?DELTA_POSTGRES_USER}"
: "${DELTA_POSTGRES_DB?DELTA_POSTGRES_DB}"
: "${DELTA_POSTGRES_DATA_DIRECTORY?DELTA_POSTGRES_DATA_DIRECTORY}"

INITDB=""
if [ "$DELTA_POSTGRES_INITDB" ]
then
INITDB="-v $DELTA_POSTGRES_INITDB:/docker-entrypoint-initdb.d/ "
fi

docker run -d \
-p $DELTA_POSTGRES_PORT:5432 \
-e "POSTGRES_PASSWORD=$DELTA_POSTGRES_PASSWORD" \
-e "POSTGRES_USER=$DELTA_POSTGRES_USER" \
-e "POSTGRES_DB=$DELTA_POSTGRES_DB" \
-e "POSTGRES_INITDB_ARGS=--encoding=UTF-8 --no-locale" \
--name delta-postgres \
--restart=unless-stopped \
-v "$DELTA_POSTGRES_DATA_DIRECTORY:/var/lib/postgresql/data" \
$INITDB \
postgres:14
