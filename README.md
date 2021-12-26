# Time-Tracking  [![CI](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/ci.yml/badge.svg)](https://github.com/FairyFox5700/Time-Tracking/actions/workflows/ci.yml)
Time tracking solution
# __________________________________      
# How to run locally without docker
1. Start elasticsearch container
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up  elasticsearch```
2.Start kibana container
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up  kibana```
3. Start consul container
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up consul```
4. Start rabbitmq container
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up rabbitmq```
5.Change connection string in TimeTracking and TimeTracking.Identuty projects
6. Create databases and run migrations 
7. Start projects locally 
# __________________________________
# How to run locally with docker
in project directoy run command
```docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d```
