#/bin/sh

GREEN='\033[0;32m'
NC='\033[0m' 

docker compose up -d --build
ngrok http --url=snail-deciding-stingray.ngrok-free.app 8080
echo -e "=======================${GREEN}FINISHED!${NC}==========================="