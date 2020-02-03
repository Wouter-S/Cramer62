INSERT INTO Room (roomId,name,groupLights,codeName) VALUES 
(1,'Woonkamer',0,'living1'),
 (2,'Keuken',1,'kitchen'),
 (3,'Gang',1,'hallway'),
 (4,'Hal',1,'hallway2'),
 (5,'Badkamer',1,'bathroom'),
 (6,'WC',0,'toilet'),
 (7,'Slaapkamer',1,'bedroom');

INSERT INTO Light (name,percentage,mode,isDimmable,friendlyName,mqttAddress,roomId) VALUES 
 ('Keuken',0,1,1,'Kitchen','cramer62/lights/core/4',2),
 ('Hal',5,0,1,'Hallway','cramer62/lights/core/8',4),
 ('Gang',20,0,1,'Front door','cramer62/lights/core/6',3),
 ('Badkamer',100,0,0,'Bathroom','cramer62/lights/core/2',5),
 ('Slaapkamer',100,0,0,'Bedroom','cramer62/lights/core/12',7),
 
 ('Balk',100,0,1,'Ceiling','cramer62/lights/core/10',1),
 ('Zithoek',100,0,0,'Living','cramer62/lights/core/5',1),
 ('Eettafel',100,0,0,'Dining','cramer62/lights/core/11',1),
 
 ('TV meubel',100,0,0,'TV','cramer62/lights/100',1),
 ('Ziggy',100,0,0,'Ziggy','cramer62/lights/101',1);

 INSERT INTO Scene (friendlyName, mqttAddress) VALUES 
 ('ifttt', 'cramer62/scenes/ifttt');