using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Analytics
{
    internal class MsAnalytics
    {
        private MqttClient mqttClient;
        private List<string> messages;
        private string[] topics = { "topic", "resultTopic" };
        public MsAnalytics()
        {
            this.getDataFromTopic("topic");
            messages = new List<string>(); 
        }
        public void getDataFromTopic(string topic)
        {
            string brokerAddress = "localhost";
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
                this.mqttClient.Publish("eventInfoTopic", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            }
        }
       
        
    }
}
