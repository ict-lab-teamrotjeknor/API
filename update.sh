#!/bin/bash

user=oscar
systemctl stop kestrel-api-ictlab.service

rm -rf API/obj
sudo -u $user git pull

rm -rf ../Publish

cd API
sudo -u $user dotnet restore
sudo -u $user dotnet ef migrations add ictlabV6
sudo -u $user dotnet ef database update

sudo -u $user dotnet publish -o ../../Publish

systemctl start kestrel-api-ictlab.service
