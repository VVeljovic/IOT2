
using Npgsql;
using Sensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

namespace Sensor.Repository
{
    internal class AirQualityDataRepository
    {
        public AirQualityDataRepository()
        {


        }//172.18.0.3
        public void SetupNotification()
        {
            var connectionString = "Server=localhost ; port=5433 ; user id=postgres; password=Veljko22!!!; database=iot ; ";

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
                            Console.WriteLine(airQualityData);
                        }
                    }

                }
            }

        }
        public async void sendToTopic(string topic, string message)
        {
            // Adresa Mosquitto brokera
            string brokerAddress = "localhost"; // Ako se izvršava lokalno
            int brokerPort = 1883; // Port na kojem Mosquitto broker sluša

            // Korisničko ime i lozinka (ako je konfigurisano)
            string username = "";
            string password = "";

            // Klijent ID
            string clientId = Guid.NewGuid().ToString();

            // Kreiranje instance MQTT klijenta
            MqttClient mqttClient = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);



            try
            {
                // Povezivanje na broker
                mqttClient.Connect(clientId);
                mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                Console.WriteLine($"Message '{message}' published to topic '{topic}'");

                // Prekidamo vezu
                mqttClient.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
