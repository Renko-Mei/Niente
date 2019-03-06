#!/bin/sh

# Deploy script for Niente to AWS EC2

if [ -d "//home/ubuntu/Niente/" ]
then
  echo "Niente repository exists"
else
  echo "Niente repository does not exist"
  git clone https://github.com/xhw994/Niente.git
fi

cd //home/ubuntu/Niente/
git pull origin master
cd //home/ubuntu/Niente/Niente/
dotnet ef database update
sudo systemctl stop kestrel-niente.service
sudo dotnet publish -c Release -o //var/www/niente/
cd //var/www/niente/
sudo pkill dotnet
sudo systemctl start kestrel-niente.service

