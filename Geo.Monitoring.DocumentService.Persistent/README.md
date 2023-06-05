# Run in Docker

## 1. Create new docker network (bridge type)
docker network create mysqlnet

## 2. Run mySql server
docker run -p 3306:3306 --network mysqlnet --name geodbtest -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=geotest -e MYSQL_USER=geotest -e MYSQL_PASSWORD=geotest -d mysql:8.0.33

## 3. Убедитесь что контеёнер запущен
docker container ls

## 4.1. Run mySql admin tool
docker run -it --rm --network mysqlnet mysql:latest mysql -hgeodbtest -ugeotest -pgeotest -P3306

## 4.2. Run Adminer
docker run -p 18080:8080 --network mysqlnet --name adminer1 adminer:latest


# Migration

## For Visual Studio 'Package Manager Console'
### Add migration
Add-Migration -Name [MigrationName] -OutputDir Migrations -StartupProject Geo.Monitoring.DocumentService.Persistent -Project Geo.Monitoring.DocumentService.Persistent -Context DocumentDbContext

