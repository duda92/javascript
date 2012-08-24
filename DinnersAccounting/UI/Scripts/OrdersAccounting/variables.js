/// <reference path="references.js" />
var start_date = Date.today().previous().monday();
var end_date = Date.today().next().monday();
var accordion_div;
var orders_list_div;
var proposition_div;
var proposition_content_div;
var edited_order;
var selected_date;