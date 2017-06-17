// Write your Javascript code.
$(document).ready(function () {
    var lineChartData = [{ time: 1370044800, y: 100 }, { time: 1370044801, y: 1000 }, { time: 1370044803, y: 100 },
    { time: 1370044805, y: 1000 }, { time: 1370044808, y: 100 }, { time: 1370044810, y: 1000 }];

    lineChartData.forEach(function (element) {
        console.log(element.time);
    });


    var myChart=$('#lineChart').epoch({
        type: 'time.line',
        data: lineChartData
    });

   


    // Config
    var port = 39401;
    var host = "m13.cloudmqtt.com";



    // Create a client instance
    client = new Paho.MQTT.Client(host, port, "1234");

    //Example client = new Paho.MQTT.Client("m11.cloudmqtt.com", 32903, "web_" + parseInt(Math.random() * 100, 10));

    // set callback handlers
    client.onConnectionLost = onConnectionLost2;
    client.onMessageArrived = onMessageArrived2;

    var options = {
        useSSL: true,
        userName: "yvsytira",
        password: "ro0wLZInhaQo",
        onSuccess: onConnect2,
        onFailure: doFail
    }

    // connect the client
    client.connect(options);

    // called when the client connects
    function onConnect2() {
        // Once a connection has been made, make a subscription and send a message.
        console.log("onConnect Cloud mqtt");
        client.subscribe("/cloudmqtt/#");
        message = new Paho.MQTT.Message("Hello CloudMQTT");
        message.destinationName = "/cloudmqtt";
        client.send(message);
        $("#temp").css("border-color", "green");
        $("#temp").css("border-style", "dotted");
        $("#temp").css("border-width", "5px");

    }

    function doFail(e) {
        console.log(e);
    }

    // called when the client loses its connection
    function onConnectionLost2(responseObject) {
        if (responseObject.errorCode !== 0) {
            console.log("onConnectionLost:" + responseObject.errorMessage);
            $("#temp").css("border-color", "red");


        }
    }

    // called when a message arrives
    function onMessageArrived2(message) {
        console.log("onMessageArrived:" + message.payloadString);
    }



    //HiveMQ
    var portHive = 8000;
    var hostHive = "broker.hivemq.com";

    clientHive = new Paho.MQTT.Client(hostHive, portHive, "1234");

    // set callback handlers
    clientHive.onConnectionLost = onConnectionLost;
    clientHive.onMessageArrived = onMessageArrived;

    // connect the client
    clientHive.connect({ onSuccess: onConnect });

    // called when the client connects
    function onConnect() {
        // Once a connection has been made, make a subscription and send a message.
        console.log("onConnect Hive");
        clientHive.subscribe("Brightness/Register");
        clientHive.subscribe("BaWue/77743/Haus1/light-180591/Sensor");


        $("#hell").css("border-color", "green");
        $("#hell").css("border-style", "dotted");
        $("#hell").css("border-width", "5px");

    }

    // called when the client loses its connection
    function onConnectionLost(responseObject) {
        if (responseObject.errorCode !== 0) {
            console.log("onConnectionLost:" + responseObject.errorMessage);
            $("#hell").css("border-color", "red");

        }
    }

    // called when a message arrives
    function onMessageArrived(message) {
        console.log("onMessageArrived:" + message.payloadString);
        //überprüfen 
        switch (message.payloadString) {
            case "CON:Success":
                console.log("CON:Success Hive");
                $("#hellConnect").css("color", "green");
                break;
            case "DISC:Success":
                console.log("DISC:Success Hive");
                $("#hellConnect").css("color", "red");
                break;
            default:
                //Helligkeit Verarbeitung
                //{"id":"light-180591","helligkeit":12345,"timestamp":12345}
                var helligkeit = JSON.parse(message.payloadString); 
                var nextDataPoint = new Object();
                nextDataPoint.time = helligkeit.timestamp;
                nextDataPoint.y = helligkeit.helligkeit;

                myChart.push(nextDataPoint);

        }

        }


        $("#hellConnect").click(function () {

            message = new Paho.MQTT.Message("CON:light-180591");
            message.destinationName = "Brightness/Register";
            clientHive.send(message);

    })

        $("#hellÄndern").click(function () {

            message = new Paho.MQTT.Message("SYST:PER:1000");
            message.destinationName = "BaWue/77743/Haus1/light-180591/Server";
            clientHive.send(message);

        })

        $("#tempConnect").click(function () {
            message = new Paho.MQTT.Message("CON:light-180591");
            message.destinationName = "Brightness/Register";
            clientHive.send(message);

            $(this).css("color", "green");

        })

        $("#tempÄndern").click(function () {

            message = new Paho.MQTT.Message("SYST:PER:1000");
            message.destinationName = "BaWue/77743/Haus1/light-180591/Server";
            clientHive.send(message);

        })

       





    });


