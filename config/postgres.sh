docker run -d \
-p 17361:5432 \
-e "POSTGRES_PASSWORD=PASSWORD_HERE" \
-e "POSTGRES_USER=delta" \
-e "POSTGRES_DB=delta" \
-e "POSTGRES_INITDB_ARGS=--encoding=UTF-8 --no-locale" \
--name delta-postgres \
--restart=unless-stopped \
-v /var/delta/postgres:/var/lib/postgresql/data \
postgres:11
