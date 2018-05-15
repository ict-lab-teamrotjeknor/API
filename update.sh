#!/bin/bash

uid=$(id -u)
systemctl stop kestrel-api-ictlab.service

sudo -u $uid rm -r API/obj
sudo -u $uid git pull

sudo -u $uid rm -r ../Publish

sudo -u $uid cd API
sudo -u $uid dotnet restore
sudo -u $uid dotnet ef migrations add ictlabV6
sudo -u $uid dotnet ef database update

sudo -u $uid dotnet publish -o ../../Publish

systemctl start kestrel-api-ictlab.service
