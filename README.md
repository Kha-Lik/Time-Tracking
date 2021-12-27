# LILO [![CI](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/ci.yml/badge.svg)](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/ci.yml) [![Deploy](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/deploy.yml/badge.svg)](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/deploy.yml)
Time tracking solution

Achitecture and technologies

![image](https://user-images.githubusercontent.com/46414904/147396810-38e5ce76-eb8f-40d7-a813-5bd351a5df3c.png)

# __________________________________      
# How to run locally almost without docker
1. Start elasticsearch container
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up  elasticsearch
```
2.Start kibana container
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up  kibana
```
3. Start consul container
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up consul
```
4. Start rabbitmq container
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up rabbitmq
```
5. Change connection string in TimeTracking and TimeTracking.Identity projects

6. Create databases and run migrations 

7. Start projects locally 
# __________________________________
# How to run locally with docker
in project directory run command
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d```

# Local links

##Rabbit MQ management UI
** http://localhost:15672/**
![image](https://user-images.githubusercontent.com/46414904/147396476-d29dc228-2662-4f60-833e-65ab7e603c6c.png)

## Elastic 

http://localhost:9200/

```
{
  "name" : "03814a4258e7",
  "cluster_name" : "docker-cluster",
  "cluster_uuid" : "-DroBZiMRsie1XhnTXpkog",
  "version" : {
    "number" : "7.9.2",
    "build_flavor" : "default",
    "build_type" : "docker",
    "build_hash" : "d34da0ea4a966c4e49417f2da2f244e3e97b4e6e",
    "build_date" : "2020-09-23T00:45:33.626720Z",
    "build_snapshot" : false,
    "lucene_version" : "8.6.2",
    "minimum_wire_compatibility_version" : "6.8.0",
    "minimum_index_compatibility_version" : "6.0.0-beta1"
  },
  "tagline" : "You Know, for Search"
}
```

## Kibana

http://localhost:5601/app/home

logs: http://localhost:5601/app/discover#/?_g=(filters:!(),refreshInterval:(pause:!t,value:0),time:(from:now-15m,to:now))&_a=(columns:!(_source),filters:!(),index:fe4a6960-6120-11ec-8174-c703e222662f,interval:auto,query:(language:kuery,query:''),sort:!())

![image](https://user-images.githubusercontent.com/46414904/147396736-63e337c3-d097-4bbd-981e-1ffc995632f3.png)

## HealtchChecks UI

http://localhost:8600/healthchecks-ui#/healthchecks

![image](https://user-images.githubusercontent.com/46414904/147396610-4f0504e0-1cb6-4623-beb6-0d7c1cc8d222.png)


## Consul

http://localhost:8500/ui/dc1/services

![image](https://user-images.githubusercontent.com/46414904/147396694-7957433c-d4c9-4ba5-af59-1749ea0c59d8.png)

## Report generator swagger

http://localhost:44593/swagger/index.html

## Time Tracking swagger

http://localhost:57732/swagger/index.html

## Time Tracking identity

http://localhost:43396/swagger/index.html

## Ocelot API Gateway

http://localhost:6500



