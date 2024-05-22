﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json.Linq;

namespace Analytics
{
    internal class MsAnalytics
    {
        private MqttClient mqttClient;
        private List<string> messages;
        private string[] topics = { "topic", "resultTopic" };
        private float PT08_S1_CO = 1300;
        private float PT08_S2_NMHC = 1100;
        private float PT08_S3_NOx = 1500;
        private float PT08_S4_NO2 = 1600;
        private float PT08_S5_O3 = 1700;
        public MsAnalytics()
        {
            this.getDataFromTopic("topic");
            messages = new List<string>(); 
        }
        public void getDataFromTopic(string topic)
        {
            string brokerAddress = "mosquitto";
            int brokerPort = 1883;
            string clientId = Guid.NewGuid().ToString();
            this.mqttClient = new MqttClient(brokerAddress, brokerPort,false,null,null,MqttSslProtocols.None);
            mqttClient.MqttMsgPublishReceived += SendDataToeKuiperTopic;

            try
            {
                mqttClient.Connect(clientId);
                mqttClient.Subscribe(topics, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private  void SendDataToeKuiperTopic(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            string message = Encoding.UTF8.GetString(e.Message);
            if (topic == topics[0])
            {
                
                messages.Add(message);
                this.mqttClient.Publish("airtopic", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            }
            else if (topic == topics[1])
            {
                Console.WriteLine($"From '{topic}' i got message {message}");
                JObject airDataQuality = JObject.Parse(message);
                float currentPT08_S1_CO = (float)airDataQuality["PT08_S1_CO"];
                float currentPT08_S2_NMHC = (float)airDataQuality["PT08_S2_NMHC"];
                float currentPT08_S3_NOx = (float)airDataQuality["PT08_S3_NOx"];
                float currentPT08_S4_NO2 = (float)airDataQuality["PT08_S4_NO2"];
                float currentPT08_S5_O3 = (float)airDataQuality["PT08_S5_O3"];
                string dateTime = (string)airDataQuality["Date"];
                string time = (string)airDataQuality["Time"];

                string date = dateTime.Split('T')[0];
                string formattedDate = DateTime.Parse(date).ToString("MM/dd/yyyy");
                string messageToPublic = $"On {formattedDate} at {time}, the following air quality measurements were taken:";

               
                messageToPublic += FormMeasurementMessage("carbon monoxide (CO)", currentPT08_S1_CO, this.PT08_S1_CO);
                messageToPublic += FormMeasurementMessage("non-methane hydrocarbons (NMHC)", currentPT08_S2_NMHC, this.PT08_S2_NMHC);
                messageToPublic += FormMeasurementMessage("nitrogen oxides (NOx)", currentPT08_S3_NOx, this.PT08_S3_NOx);
                messageToPublic += FormMeasurementMessage("nitrogen dioxide (NO2)", currentPT08_S4_NO2, this.PT08_S4_NO2);
                messageToPublic += FormMeasurementMessage("ozone (O3)", currentPT08_S5_O3, this.PT08_S5_O3);

                Console.WriteLine(messageToPublic);

             

                this.mqttClient.Publish("eventInfoTopic", Encoding.UTF8.GetBytes(messageToPublic), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            }
        }
        private string FormMeasurementMessage(string pollutant, float currentValue, float previousValue)
        {
            float change = currentValue - previousValue;
            float percentageChange = (change / previousValue) * 100;
            string direction = change > 0 ? "increase" : "decrease";
            string measurementMessage = $" Measured {pollutant} value is {currentValue} which is a {Math.Abs(percentageChange):0.00}% {direction} from the previous value of {previousValue}.";
            return measurementMessage;
        }

    }
}
