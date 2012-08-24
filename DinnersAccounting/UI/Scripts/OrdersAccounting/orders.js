/// <reference path="references.js" />

function get_day_order_models(start, end) {
    var url = '/api/Orders/GetAllOrderModelsForDates';

    var start_date_day = start.getDate();
    var start_date_month = start.getMonth() + 1;
    var start_date_year = start.getFullYear();
    
    var end_date_day = end.getDate();
    var end_date_month = end.getMonth() + 1;
    var end_date_year = end.getFullYear();

    var day_order_models;
    $.ajax(url, {
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: { start_date_day: start_date_day, start_date_month: start_date_month, start_date_year: start_date_year, end_date_day: end_date_day, end_date_month: end_date_month, end_date_year: end_date_year },
        success: function (day_order_models_) {
            day_order_models = JSON.parse(day_order_models_);
        }
    });

    return day_order_models;
}

