
using Microsoft.EntityFrameworkCore;
using Sensor.Context;
using Sensor.Repository;

AirQualityService service = new AirQualityService();
service.InsertDataToDb();
service.ReadFromDb();

