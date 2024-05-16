using Sensor.Repository;

AirQualityDataRepository repo = new AirQualityDataRepository();
repo.sendToTopic("topic","message");