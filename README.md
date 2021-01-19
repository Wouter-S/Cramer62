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





**Setup Odroid**

Initial setup, follow tutorial here: https://wiki.odroid.com/odroid-c2/getting_started/os_installation_guide?redirect=2#tab__odroid-c2
- Download image "Ubuntu Minimal 18.04.3", copy to EMMC via sd card adapter with Etcher

After that, restore the files from backup to the odroid, should be in /data folder in line with the compose file (see below). This contains all configuration for zigbee2mqtt, node red, etc.

```

# Update key for odroid packages, see here: https://forum.odroid.com/viewtopic.php?t=37301
 sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys AB19BAC9
 sudo apt-get update

sudo apt-get update
```

Install docker dependencies, see here: https://docs.docker.com/engine/install/debian/
```
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

sudo docker run -d -p 8000:8000 -p 9000:9000 -v /var/run/docker.sock:/var/run/docker.sock --name portainer -v portainer_data:/data portainer/portainer 

open http://192.168.1.4:9000/
	enter password
	select local

Create stack, paste docker-compose.yml
```

**Configuration**
Go to http://{ipaddress}:9000, stacks > stack > editor

In the portainer stack (which is docker compose) the following changes need to be configured: <br/>
- On line 145/146, the arduino device has to be configured, this should be something like "/dev/ttyUSB0:/dev/lights" where "/dev/ttyUSB0" should be the port where the arduino is connected to. (can be found by executing "lsusb" command in ubuntu)<br/>
- On line 108/109, the smart meter devices has to be configured, just like the arduino 
- The device for zigbee2mqtt is configured on /etc/ttyACM0, make sure that that is still correct.

 
**How it works** 

The wall switches communicate to the Zigbee2Mqtt container, where the devices are configured to send a message to the MQTT broker, on example topic like: "cramer62/switches/switch_01". This message is picked up by the node-red container, which translates left/right/double/long clicks to a specific action and sends a message which is picked up by the CramerGui, which saves the state in a database and sends a message over MQTT which is picked up by the arduino.
 
The arduino communicates with a baudrate of 1000000, it listens for 2 types of commands,
- Switch light on/off, in the format "l{lightId}{mode}", where lightId is a number between 0-20 and mode is 0 for off and 1 for on
- Dimming lights through PWM, format "d{lightId}{brightness}" where lightId is a number between 0-20 and brightness a number between 0 and 255
