package internet.of.things.RestApi.config;

import org.eclipse.paho.client.mqttv3.*;
import org.eclipse.paho.client.mqttv3.persist.MemoryPersistence;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.ArrayList;
import java.util.List;

@Configuration
public class MqttConfig implements MqttCallback {
    @Value("${mqtt.broker}")
    private String broker;

    @Value("${mqtt.clientId}")
    private String clientId;

    @Value("${mqtt.topic}")
    private String topic;

    private List<String> messagesFromTopic =new ArrayList<>();
    @Bean
    public MqttClient mqttClient() throws  MqttException {
        MqttClient mqttClient = new MqttClient(broker, clientId, new MemoryPersistence());
        MqttConnectOptions mqttConnectOptions = new MqttConnectOptions();
        mqttConnectOptions.setCleanSession(true);
        mqttClient.setCallback(this);
        mqttClient.connect(mqttConnectOptions);


        mqttClient.subscribe(topic);
        return mqttClient;
    }
    @Override
    public void deliveryComplete(IMqttDeliveryToken iMqttDeliveryToken) {

    }

    @Override
    public void connectionLost(Throwable throwable) {

    }

    @Override
    public void messageArrived(String s, MqttMessage mqttMessage) throws Exception {
        String payload = new String(mqttMessage.getPayload());
        messagesFromTopic.add(payload);
        System.out.println("Poruka primljena sa topica: " + topic + ", sadržaj: " + payload);
    }

    public List<String> getMessageList() {
        return messagesFromTopic;
    }
}
