#include "Light.h"
#include <SPI.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>


int nrOfLights = 20;
Light lights[] = {
  Light(2), Light(3), Light(4), Light(5), Light(6), Light(7), Light(8), Light(9), Light(10), Light(11),
  Light(12), Light(13), Light(14), Light(15), Light(16), Light(17), Light(18), Light(19), Light(20), Light(21)
};

void setup() {
  Serial.begin(1000000);
  Serial.setTimeout(3);
  Serial.println("starting");
}

void loop() {

  for (int i = 0; i < nrOfLights; i++)
  {
    lights[i].update();
  }

  String command = Serial.readString();
  Serial.flush();

  if (command == "") {
    return;
  }
  
  int lightId;
  int mode;
  switch (command[0])
  {
    case 'l':
      {
        lightId = (String(command[1]) + String(command[2])).toInt();
        mode = String(command[3]).toInt();

        if (mode == 0) {
          lights[lightId].switchOff();
        }
        if (mode == 1) {
          lights[lightId].switchOn();
        }

        break;
      }
    case 'd':
      {

        lightId = (String(command[1]) + String(command[2])).toInt();
        mode = (String(command[3]) + String(command[4]) + String(command[5])).toInt();
        lights[lightId].dim(mode);
        break;
      }
  }
}
