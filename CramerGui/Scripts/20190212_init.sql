BEGIN TRANSACTION;
CREATE TABLE SensorType (
	[sensorTypeId]	integer PRIMARY KEY AUTOINCREMENT NOT NULL,
	[name]	varchar(50) NOT NULL COLLATE NOCASE

);

CREATE TABLE ScheduleType (
	[scheduleTypeId]	integer NOT NULL,
	[name]	varchar(50) NOT NULL COLLATE NOCASE,
    PRIMARY KEY ([scheduleTypeId])

);

CREATE TABLE Scene (
	sceneId	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	friendlyName	varchar(50) NOT NULL,
	mqttAddress	varchar(255)
);

CREATE TABLE Room (
	roomId	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	[name]	varchar(50) NOT NULL,
	groupLights	bit NOT NULL DEFAULT 0,
	codeName	TEXT DEFAULT ''
);
CREATE TABLE Light (
	lightId	integer NOT NULL,
	name	varchar(50) NOT NULL,
	percentage	integer,
	mode	integer NOT NULL,
	isDimmable	bit NOT NULL DEFAULT 0,
	friendlyName	varchar(50),
	roomId	INTEGER,
	mqttAddress	varchar(255),
	PRIMARY KEY(lightId)
);


CREATE TABLE Room_Light (
	[roomId]	integer NOT NULL,
	[lightId]	integer NOT NULL,
    PRIMARY KEY ([roomId], [lightId])
,
    FOREIGN KEY ([lightId])
        REFERENCES [Light]([lightId]),
    FOREIGN KEY ([roomId])
        REFERENCES [Room]([roomId])
);

COMMIT;
