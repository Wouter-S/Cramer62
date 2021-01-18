// 
// 
// 

#include "Light.h"

Light::Light(int pin)
{
	_pin = pin;
	pinMode(_pin, OUTPUT);
}

void Light::dim(int value)
{
	_dimmable = true;
	_targetValue = value;
	this->_working = true;
	_lastUpdate = millis();
}

void Light::switchOn()
{
	digitalWrite(_pin, HIGH);

}

void Light::switchOff()
{
	digitalWrite(_pin, LOW);
}

void Light::update() {

	if ((millis() - _lastUpdate) > _delay)  // time to update
	{
		_lastUpdate = millis();

		if (!this->_working) {
			return;
		}

		if (_targetValue > _value)
		{
			_value++;
		}
		else if (_targetValue < _value)
		{
			_value--;
		}
		else
		{
			this->_working = false;
			return;
		}

		analogWrite(_pin, _value);
		if (_targetValue == _value)
		{
			this->_working = false;
		}
	}
}
