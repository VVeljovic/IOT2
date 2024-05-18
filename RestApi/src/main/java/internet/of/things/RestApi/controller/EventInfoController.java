package internet.of.things.RestApi.controller;

import internet.of.things.RestApi.config.MqttConfig;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping(path = "eventInfo")
public class EventInfoController {

    private final MqttConfig config;
    @Autowired
    public EventInfoController(MqttConfig config) {this.config = config;}
    @GetMapping("")
    public String getData()
    {
        return config.messageFromTopic;
    }
}
