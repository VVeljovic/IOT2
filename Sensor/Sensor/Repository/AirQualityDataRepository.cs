
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
            var connectionString = "Server=localhost ; port=5432 ; user id=postgres; password=Veljko22!!!; database=Internet of things ; ";

            var queryString = "SELECT * FROM air_quality WHERE \"Id\" = @Id;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("Id", 1);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            var airQualityData = new AirQualityData
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                CO_GT = reader.GetFloat(reader.GetOrdinal("co_gt")),
                                PT08_S1_CO = reader.GetInt32(reader.GetOrdinal("PT08_S1_CO"))
                            };
                            this.sendToTopic("topic", airQualityData);
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
