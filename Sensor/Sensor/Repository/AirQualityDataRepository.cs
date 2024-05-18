
using Npgsql;
using Sensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json;

namespace Sensor.Repository
{
    internal class AirQualityDataRepository
    {
        public AirQualityDataRepository()
        {


        }//172.18.0.3
        public void SetupNotification()
        {
            //promeni na port 5433 i database = iot
            var connectionString = "Server=localhost ; port=5432 ; user id=postgres; password=Veljko22!!!; database=Internet of things ; ";

            var queryString = "SELECT * FROM air_quality WHERE \"Id\" = @Id;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("Id", 34);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            var airQualityData = new AirQualityData
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                Time = reader.GetTimeSpan(reader.GetOrdinal("time")),
                                CO_GT = reader.GetFloat(reader.GetOrdinal("co_gt")),
                                PT08_S1_CO = reader.GetInt32(reader.GetOrdinal("PT08_S1_CO")),
                                NMHC_GT = reader.GetFloat(reader.GetOrdinal("NMHC_GT")),
                                C6H6_GT = reader.GetFloat(reader.GetOrdinal("C6H6_GT")),
                                PT08_S2_NMHC = reader.GetFloat(reader.GetOrdinal("PT08_S2_NMHC")),
                                NOx_GT = reader.GetFloat(reader.GetOrdinal("NOx_GT")),
                                PT08_S3_NOx = reader.GetFloat(reader.GetOrdinal("PT08_S3_NOx")),
                                NO2_GT = reader.GetFloat(reader.GetOrdinal("NO2_GT")),
                                PT08_S4_NO2 = reader.GetFloat(reader.GetOrdinal("PT08_S4_NO2")),
                                PT08_S5_O3 = reader.GetFloat(reader.GetOrdinal("PT08_S5_O3")),
                                T = reader.GetFloat(reader.GetOrdinal("T")),
                                RH = reader.GetFloat(reader.GetOrdinal("RH")),
                                AH = reader.GetFloat(reader.GetOrdinal("AH"))
                            };
                            this.sendToTopic("topic",airQualityData);
                        }
                    }

                }
            }

        }
        public async void sendToTopic(string topic, AirQualityData airQualityData)
        {
            
            string brokerAddress = "localhost"; 
            int brokerPort = 1883;

            string jsonData = JsonConvert.SerializeObject(airQualityData);
           
            string clientId = Guid.NewGuid().ToString();

            
            MqttClient mqttClient = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);



            try
            {
                
                mqttClient.Connect(clientId);
                mqttClient.Publish(topic, Encoding.UTF8.GetBytes(jsonData), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                Console.WriteLine($"Message '{jsonData}' published to topic '{topic}'");

                
                mqttClient.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
