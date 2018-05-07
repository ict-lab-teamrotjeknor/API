#!/bin/bash
systemctl stop kestrel-api-ictlab.service

rm -r API/obj
git pull

rm -r ../Publish

cd API
dotnet restore
dotnet ef migrations add ictlabV6
dotnet ef database update

dotnet publish -o ../../Publish

sudo systemctl start kestrel-api-ictlab.service