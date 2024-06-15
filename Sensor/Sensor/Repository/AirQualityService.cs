
using Npgsql;

using System.Text;


using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using  Sensor.Models;
using Sensor.Context;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Sensor.Repository
{
    internal class AirQualityDataRepository
    {
        List<AirQualityData> airQualityarrayFromCsv = new List<AirQualityData>();
        public AirQualityDataRepository()
        {
           


        }
        public void InsertDataToDb()
        {
            using (var context = new SensorDbContext())
            {
                
              
                if (context.Database.EnsureCreated())
                {
                    loadFromCsv();
                    foreach (var airQuality in airQualityarrayFromCsv)
                    {
                        var date = DateOnly.ParseExact(airQuality.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var time = TimeSpan.ParseExact(airQuality.Time, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);


                        var airQualityDataDbModel = new AirQualityDataDbModel
                        {
                            Date = date,
                            Time = time,
                            CO_GT = airQuality.CO_GT,
                            PT08_S1_CO = airQuality.PT08_S1_CO,
                            NMHC_GT = airQuality.NMHC_GT,
                            C6H6_GT = airQuality.C6H6_GT,
                            PT08_S2_NMHC = airQuality.PT08_S2_NMHC,
                            NOx_GT = airQuality.NOx_GT,
                            PT08_S3_NOx = airQuality.PT08_S3_NOx,
                            NO2_GT = airQuality.NO2_GT,
                            PT08_S4_NO2 = airQuality.PT08_S4_NO2,
                            PT08_S5_O3 = airQuality.PT08_S5_O3,
                            T = airQuality.T,
                            RH = airQuality.RH,
                            AH = airQuality.AH
                        };

                        context.AirQualityData.Add(airQualityDataDbModel);
                    }

                    context.SaveChanges();
                }
            }
        }
        public void SetupNotification()
        {
            var connectionString = "Server=postgres;Port=5432;Database=IOT;Username=postgres;password=Veljko22!!!";

            var queryString = "SELECT * FROM air_quality_iot WHERE \"Id\" <= @Id;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("Id", 500);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var airQualityData = new AirQualityDataDbModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Date = DateOnly.ParseExact(reader.GetString(reader.GetOrdinal("Date")), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                Time = reader.GetTimeSpan(reader.GetOrdinal("Time")),
                                CO_GT = reader.GetFloat(reader.GetOrdinal("co_gt")),
                                PT08_S1_CO = reader.GetFloat(reader.GetOrdinal("PT08_S1_CO")),
                                NMHC_GT = reader.GetFloat(reader.GetOrdinal("NMHC_GT")),
                                C6H6_GT = reader.GetFloat(reader.GetOrdinal("C6H6_GT")),
                                PT08_S2_NMHC = reader.GetFloat(reader.GetOrdinal("PT08_S2_NMHC")),
                                NOx_GT = reader.GetFloat(reader.GetOrdinal("NOx_GT")),
                                PT08_S3_NOx = reader.GetFloat(reader.GetOrdinal("PT08_S3_NOx")),
                                NO2_GT = reader.GetFloat(reader.GetOrdinal("NO2_GT")),
                                PT08_S4_NO2 = reader.GetFloat(reader.GetOrdinal("PT08_S4_NO2")),
                                PT08_S5_O3 = reader.GetFloat(reader.GetOrdinal("PT08_S5_O3")),
                                T = 100,
                                RH = reader.GetFloat(reader.GetOrdinal("RH")),
                                AH = reader.GetFloat(reader.GetOrdinal("AH"))
                            };
                            
                            Thread.Sleep(1000);
                            SendToTopic("air_topic", airQualityData);
                        }
                        
                    }

                }
            }

        }

        public  void SendToTopic(string topic, AirQualityDataDbModel airQualityData)
        {

            string brokerAddress = "mosquitto";
            int brokerPort = 1883;

            string jsonData = JsonConvert.SerializeObject(airQualityData);

            string clientId = Guid.NewGuid().ToString();


            MqttClient mqttClient = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);



            try
            {

                if (!mqttClient.IsConnected)
                {
                    mqttClient.Connect(clientId);
                }
                mqttClient.Publish(topic, Encoding.UTF8.GetBytes(jsonData), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                Console.WriteLine($"Message '{jsonData}' published to topic '{topic}'");


               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void loadFromCsv()
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            { HasHeaderRecord = false};

            //string csvFilePath = "C:\\Users\\veljk\\OneDrive\\Desktop\\Cetvrta godina\\IOT2\\AirQuality.csv";
            string csvFilePath = "/sensor/config/AirQuality.csv";
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<AirQualityData>();
                        airQualityarrayFromCsv.Add(record);
                       // Console.WriteLine($"Date: {record.Date}, Time: {record.Time},AH:{record.AH}, CO(GT): {record.CO_GT}, PT08.S1(CO): {record.PT08_S1_CO}");
                    }
                   
                }
        }
    }
 
}
