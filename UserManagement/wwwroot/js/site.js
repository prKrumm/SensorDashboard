// Write your Javascript code.
$(document).ready(function () {
    

  










    

    var sinLayer = { label: 'sin', values: [] },
        cosLayer = { label: 'cos', values: [] }

    for (var x = 0; x <= 2 * Math.PI; x += Math.PI / 64) {
        sinLayer.values.push({ x: x, y: Math.sin(x) + 1 });
        cosLayer.values.push({ x: x, y: Math.cos(x) + 1 });
    }

    var chart = $('#chart').epoch({
        type: 'area',
        data: [sinLayer, cosLayer],
        axes: ['left', 'right']
    });

    Date.now = function () { return new Date().getTime(); }

    


    ///////////////this function generates the date and time in milliseconds//////////
    function getTimeValue() {
        var dateBuffer = new Date();
        var Time = dateBuffer.getTime();
        return Time;
    }

    ////////////// this function generates a random value ////////////////////////////
    function getRandomValue() {
        var randomValue = Math.random() * 100;
        return randomValue;
    }

    ////////////// this function is used to update the chart values ///////////////	
    $("#updateMessage").click(function () {

       

    })
   

    ////////////// real time graph generation////////////////////////////////////////	  
    var barChartData = [{
        label: "Series 1",
        values: [{
            time: getTimeValue(),
            y: getRandomValue()
        }]
    },];

    var barChartInstance = $('#barChart').epoch({
        type: 'time.bar',
        axes: ['right', 'bottom', 'left'],
        data: barChartData
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

                var newBarChartData = [{ time: getTimeValue(), y: helligkeit.helligkeit }];

                /* Wrong: don't use the full configuration for an update.
                var newBarChartData = [{
                  label: "Series 1",
                  values: [{
                    time: getTimeValue(),
                    y: getRandomValue()
                  }]
                }, ];
                */
                barChartInstance.push(newBarChartData);

                

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


