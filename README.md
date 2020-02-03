# Cramer62

Small home automation setup, uses a set of services to control lights and read temperature and power usage

The system runs on an Odroid C2 on which docker is installed

- **CramerGui** - 
  Responsible for displaying the GUI, supports two views, regular "desktop" view, optimized for tablet, and mobile view. Also acts as broker between NodeRed, Alexa and Core.
- **CramerAlexa** - 
  Acts like an Hue hub, makes local communication between the Amazon Echo and CramerGui possible
- **CramerCore** -
  Recieves Mqtt commands, and sends them to the Arduino connected to the comm port to switch the relays.

Requires:

- **Mosquito**
- **Node-Red**
- **InfluxDb**
- **Grafana**
- **Zigbee2Mqtt**
- **SmartMeterToMqtt**





Setup Odroid

```
sudo apt-get update

sudo apt-get install \
    apt-transport-https \
    ca-certificates \
    curl \
    software-properties-common
	
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

sudo add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
   $(lsb_release -cs) \
   stable"

sudo apt-get update

sudo apt-get install docker-ce

sudo docker run hello-world

sudo docker volume create portainer_data && sudo docker run -d -p 8000:8000 -p 9000:9000 -v /var/run/docker.sock:/var/run/docker.sock --name portainer -v portainer_data:/data portainer/portainer 

open http://192.168.1.4:9000/
	enter password
	select local

Create stack, paste docker-compose.yml
```