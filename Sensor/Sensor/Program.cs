
using Microsoft.EntityFrameworkCore;
using Sensor.Context;
using Sensor.Repository;

AirQualityDataRepository repo = new AirQualityDataRepository();
repo.InsertDataToDb();
//repo.loadFromCsv();
repo.SetupNotification();
//for (int i = 0; i < 10000; i++)
//{
//    var airQualityData = new AirQualityData
//    {

//        Date = DateTime.Now.Date,
//        Time = DateTime.Now.TimeOfDay,
//        CO_GT = (float)random.NextDouble() * 10,
//        PT08_S1_CO = random.Next(0, 1000),
//        NMHC_GT = (float)random.NextDouble() * 10,
//        C6H6_GT = (float)random.NextDouble() * 10,
//        PT08_S2_NMHC = (float)random.NextDouble() * 10,
//        NOx_GT = (float)random.NextDouble() * 10,
//        PT08_S3_NOx = (float)random.NextDouble() * 10,
//        NO2_GT = (float)random.NextDouble() * 10,
//        PT08_S4_NO2 = (float)random.NextDouble() * 10,
//        PT08_S5_O3 = (float)random.NextDouble() * 10,
//        T = (float)random.NextDouble() * 50,
//        RH = (float)random.NextDouble() * 100,
//        AH = (float)random.NextDouble() * 100 
//    };
//    Thread.Sleep(1000);
//    repo.sendToTopic("najnoviji2", airQualityData);
//}
