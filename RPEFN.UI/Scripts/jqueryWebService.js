



$(document).ready(function () {
    $("#grpResponse").hide();

    $("#method").change(function() {
        if ($("#method").val().toUpperCase() === "GET") {
            $("#grpRequest").hide();
        } else {
            $("#grpRequest").show();
        }
    });


    $("#btnSubmit").click(function () {

        switch ($("#method").val().toUpperCase()) {
            case 'POST':
                $.ajax({
                    type: 'POST',
                    url: $("#url").val(),
                    crossDomain: true,
                    contentType: "application/json; charset=utf-8",
                    data:  $("#requestBody").val(),
                    dataType: 'json',
                    headers: {
                        'Authorization': $("#token").val()
                    },
                    success: function (responseData, textStatus) {
                        $("#grpResponse").show();
                        $("#responseBody").val(JSON.stringify(responseData, null, 2));
                        $("#statusCode").val("Status Code: " + textStatus);
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        $("#grpResponse").show();
                        $("#statusCode").text("Status Code: " + jqXhr.status);
                        $("#responseBody").val(JSON.stringify(errorThrown));
                    }
                });
                break;
            case 'GET':
                $.ajax({
                    url: $("#url").val(),
                    type: 'GET',
                    cache: false,
                    crossDomain: true,
                    headers: {
                        'Authorization': $("#token").val()
                    },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        $("#grpResponse").show();
                        $("#responseBody").val(JSON.stringify(response, null, 2));
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        $("#grpResponse").show();
                        $("#statusCode").text("Status Code: " + jqXhr.status);
                        $("#responseBody").val(JSON.stringify(errorThrown));
                    }
                });
                break;
            case 'PUT':
                $.ajax({
                    url: $("#url").val(),
                    type: 'PUT',
                    cache: false,
                    crossDomain: true,
                    data: $("#requestBody").val(),
                    headers: {
                        'Authorization': $("#token").val()
                    },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    xhrFields: {
                        //withCredentials: true
                    },
                    success: function (response) {
                        $("#grpResponse").show();
                        $("#responseBody").val(JSON.stringify(response, null, 2));
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        $("#grpResponse").show();
                        $("#statusCode").text("Status Code: " + jqXhr.status);
                        $("#responseBody").val(JSON.stringify(errorThrown));
                    }
                });
                break;
            case 'DELETE':
                $.ajax({
                    url: $("#url").val(),
                    type: 'DELETE',
                    cache: false,
                    crossDomain: true,
                    data: $("#requestBody").val(),
                    headers: {
                        'Authorization': $("#token").val()
                    },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        $("#grpResponse").show();
                        $("#responseBody").val(JSON.stringify(response, null, 2));
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        $("#grpResponse").show();
                        $("#statusCode").text("Status Code: " + jqXhr.status);
                        $("#responseBody").val(JSON.stringify(errorThrown));
                    }
                });
                break;
        }
    });
});