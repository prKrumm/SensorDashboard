
$(document).ready(function () {

    ///////////////this function generates the date and time in milliseconds//////////
    function getTimeValue() {
        var dateBuffer = new Date();
        var Time = dateBuffer.getTime() / 1000;
        return Time;
    }

    ////////////// this function generates a random value ////////////////////////////
    function getRandomValue() {
        var randomValue = Math.random() * 100;
        return randomValue;
    }

    $(function () {
        $("#datepicker").datepicker();
    });

    //var barChartDataDetails = [{
    //    label: "Series 1",
    //    values: [{
    //        time: 0,
    //        y: 0
    //    }]
    //},];

    //var barChartInstance = $('#barChartDetails').epoch({
    //    type: 'time.bar',
    //    axes: ['right', 'bottom', 'left'],
    //    data: barChartDataDetails
    //});
    $(".close").click(function () {

        window.location.reload();

    });

    $("#showDetails").click(function () {
       
        var date = $("#datepicker").datepicker("getDate");
        var Time = date.getTime() / 1000;
        var deviceName = ($("#device").first().text());
        deviceName = deviceName.replace(/\s/g, '');
        $.ajax({
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            "type": "GET",
            "url": "/api/Measurement",
            "data": { time: Time, id: deviceName },
            "dataType": "json",
           
        }).done(function (data) {
          
           
            var jsonArray = JSON.parse(data);
            console.log(jsonArray);
           
            
            var sinLayer = { label: 'sin', values: [] };
            var newBarChartData = [{ time: 0, y: 0 }];

            if (jsonArray.length === 0) {
                $("#myAlert").addClass("in")
 
            }
            var sinLayer = { label: 'sin', values: [] }
            for (var i = 0; i < jsonArray.length; i++) {
                var nextDataPoint = new Object();
                var obj = jsonArray[i];
                nextDataPoint.time = obj.timestamp;                   
                        nextDataPoint.y = obj.temperatur;
                        newBarChartData.push(nextDataPoint); 
                        sinLayer.values.push({ x: i, y: obj.temperatur });
            }   

           
                

            

            var chart = $('#chart').epoch({
                type: 'line',
                data: [sinLayer],
                axes: ['left', 'bottom','right']
            });
                    
            
            var barChartDataDetails = [{
                label: "Series 1",
                values: newBarChartData
            },];

            var barChartInstance = $('#barChartDetails').epoch({
                type: 'time.bar',
                axes: ['right', 'bottom', 'left'],
                data: barChartDataDetails
            });
            
            
        });


        //$.get("/api/Measurement", { time: Time, id: deviceName })
            //.done(function (data) {
            //    var ergebnis = JSON.parse(data);
            //    alert(data);               
            //});
    });



});

