package internet.of.things.RestApi;

//import internet.of.things.RestApi.config.MqttConfig;
import org.eclipse.paho.client.mqttv3.MqttClient;
import org.eclipse.paho.client.mqttv3.MqttException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class RestApiApplication {

	public static void main(String[] args) throws MqttException, InterruptedException {

		SpringApplication.run(RestApiApplication.class, args);



	}

}
