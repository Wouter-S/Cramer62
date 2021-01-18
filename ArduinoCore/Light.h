#ifndef _LIGHT_h
#define _LIGHT_h
#include "Arduino.h"

class Light
{
private:
	int _pin;
	int _value;
	int _targetValue;
	bool _dimmable = false;
	int _delay = 10;
	int _lastUpdate = 0;

	bool _working = false;
protected:


public:
	Light(int pin);
	void dim(int value);
	void switchOn();
	void switchOff();
	void update();
};

#endif

