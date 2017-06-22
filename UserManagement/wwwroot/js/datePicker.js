
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

    var barChartDataDetails = [{
        label: "Series 1",
        values: [{
            time: getTimeValue(),
            y: getRandomValue()
        }]
    },];

    var barChartInstance = $('#barChartDetails').epoch({
        type: 'time.bar',
        axes: ['right', 'bottom', 'left'],
        data: barChartDataDetails
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
            var jsonArray = data;
            
            alert(data);
        });


        //$.get("/api/Measurement", { time: Time, id: deviceName })
            //.done(function (data) {
            //    var ergebnis = JSON.parse(data);
            //    alert(data);               
            //});
    });



});

