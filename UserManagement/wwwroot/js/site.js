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
        clientHive.subscribe("Brightness/light-180591/Sensor");


        $("#hell").css("border-color", "green");
        $("#hell").css("border-style", "dotted");
        $("#hell").css("border-width", "5px");

    }

    // called when the client loses its connection
    function onConnectionLost(responseObject) {
        if (responseObject.errorCode !== 0) {
            console.log("onConnectionLost:" + responseObject.errorMessage);
            $("#hell").css("border-color", "red");
            clientHive.connect({ onSuccess: onConnect });
           
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
            case "DISC:Success": DISC: Success
                console.log("DISC:Success Hive");
                $("#hellConnect").css("color", "red");
                break; 
            default:
                //Helligkeit Verarbeitung
                //{"id":"light-180591","helligkeit":12345,"timestamp":12345}
                try {
                    var helligkeit = JSON.parse(message.payloadString);
                    if (helligkeit.helligkeit !== null) {
                        var nextDataPoint = new Object();
                        nextDataPoint.time = helligkeit.timestamp;
                        nextDataPoint.y = helligkeit.helligkeit;

                        var newBarChartData = [{ time: getTimeValue(), y: helligkeit.helligkeit }];

                        //hell ändern
                        //id hellWert       hellZeitWert
                        $("#hellWert").html(helligkeit.helligkeit);
                        $("#hellConnect").css("color", "green");
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
                        //Helligkeit Verarbeitung
                        //{"id":"light-180591","temperatur":12345,"timestamp":12345}
                        //eher null prüfen für attribut????
                    } if (helligkeit.temperatur !== null) {

                        var nextDataPoint = new Object();
                        nextDataPoint.time = helligkeit.timestamp;
                        nextDataPoint.y = helligkeit.temperatur;

                        var newBarChartData = [{ time: getTimeValue(), y: helligkeit.helligkeit }];

                        //hell ändern
                        //id hellWert       hellZeitWert
                        $("#tempWert").html(helligkeit.temperatur);



                    }
                } catch (e) {
                    console.log('invalid json');
                }

        }

        }


        $("#hellConnect").click(function () {

            var hellTempConn = $(this).css("color");
            //Send Registration
            if (hellTempConn !== "rgb(0, 128, 0)") {
                console.log("css color:" + hellTempConn);
                message = new Paho.MQTT.Message("CON:light-180591");
                message.destinationName = "Brightness/Register";
                clientHive.send(message);

                $(this).css("color", "green");
            } else {
                //Send Disconnect
                console.log("Disconnect css color:" + hellTempConn);
                message = new Paho.MQTT.Message("DISC:light-180591");
                message.destinationName = "Brightness/light-180591/Server";
                clientHive.send(message);

            }

    })

        $("#sendBtnHell").click(function () {
            $('#newDiv1').hide("slow");

            var neuerWert = $("#inputHell").val();
            var re = new RegExp("\\d+");
            if (re.test(neuerWert)) {
                message = new Paho.MQTT.Message("SYST:PER:" + neuerWert);
                console.log("neuerWert:" + neuerWert);
                message.destinationName = "Brightness/light-180591/Server";
                clientHive.send(message);
                $("#hellZeitWert").html(neuerWert);
            }
        })

        $("#hellÄndern").click(function () {
            var dispalay = $("#newDiv1").css("display");
            if (dispalay == "none") {
                $("#newDiv1").show("slow");
            } else {
                $("#newDiv1").hide("slow");
            }
            
        })

        
       

        

        $("#tempConnect").click(function () {
            var colorTempConn = $(this).css("color");
            //Send Registration
            if (colorTempConn !== "green") {
                console.log("css color:" + colorTempConn);
            message = new Paho.MQTT.Message("CON:light-180591");
            message.destinationName = "Brightness/Register";
            clientHive.send(message);

            $(this).css("color", "green");
            } else {
                //Send Disconnect
                console.log("Disconnect css color:" + color);
                message = new Paho.MQTT.Message("DISC:light-180591");
                message.destinationName = "Brightness/light-180591/Server";
                clientHive.send(message);

            }
               

        })

      

        $("#tempÄndern").click(function () {

            var dispalay = $("#newDiv2").css("display");
            if (dispalay == "none") {
                $("#newDiv2").show("slow");
            } else {
                $("#newDiv2").hide("slow");
            }
           
        })
        $("#sendBtnTemp").click(function () {
            $('#newDiv2').hide("slow");

            var neuerWert = $("#inputTemp").val();
            var re = new RegExp("\\d+");
            if (re.test(neuerWert)) {
                message = new Paho.MQTT.Message("SYST:PER:" + neuerWert);
                console.log("neuerWert:" + neuerWert);
                message.destinationName = "Brightness/light-180591/Server";
                clientHive.send(message);
                $("#tempZeitWert").html(neuerWert);
            }
        })

       





    });


