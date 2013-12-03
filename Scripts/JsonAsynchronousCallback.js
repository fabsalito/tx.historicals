var CallBacks = new function () {
    this.loadJsonView = function (cadurl, method, fn, params) {

        $.ajax({
            url: cadurl,
            data: JSON.stringify(params),
            dataType: 'json',
            contentType: 'application/json',
            cache: false,
            type: method,
            success: function (data) {

                fn(data, params);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var strmsg = "Status: " + textStatus + ". Error: " + errorThrown;

                alert(strmsg);
            }
        });
    };
    this.loadPartialView = function (cadurl, method, fn, params) {
        $.ajax({
            url: cadurl,
            data: JSON.stringify(params),
            dataType: 'html',
            contentType: 'application/json',
            cache: false,
            type: method,
            success: function (data) {

                fn(data, params);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var strmsg = "Status: " + textStatus + ". Error: " + errorThrown;

                alert(strmsg);
            }
        });
    };

    this.loadPartialViewWithoutParams = function (cadurl, method, fn) {
        $.ajax({
            url: cadurl,
            dataType: 'html',
            cache: false,
            type: method,
            success: function (data) {
                fn(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var strmsg = "Status: " + textStatus + ". Error: " + errorThrown;

                alert(strmsg);
            }
        });
    };

    this.loadJsonViewWithoutParams = function (cadurl, method, fn) {
        $.ajax({
            url: cadurl,
            dataType: 'json',
            contentType: 'application/json',
            cache: false,
            type: method,
            success: function (data) {
                fn(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var strmsg = "Status: " + textStatus + ". Error: " + errorThrown;

                alert(strmsg);
            }
        });
    };

}