/// <reference path="references.js" />
function hide_order() {
    $('.order').css('visibility', 'hidden');
}

function show_order() {
    $('.order').css('visibility', 'visible');
}

function draw_empty_order(date) {

    clear_order();

    $('#order_date').val(date);

    $('#order_title').append('Заказ на ' + date.split("T")[0]);

    $('#order_id').val(0);

    $('#order_status').html('Создание');
}

function draw_existing_order(order) {

    order = JSON.parse(order);

    clear_order();

    $('#order_template').html(tmpl("order_template", JSON.stringify(order)));

//    $('#order_date').val(order.Date);
//    $('#order_id').val(order.Id);

//    $('#order_title').append('Заказ на ' + order.Date.split("T")[0]);

//    if (order.CurrentStatus == 1)
//        $('#order_status').append('Редактирование');
//    else if (order.CurrentStatus == 2)
//        $('#order_status').append('Отправленый');
//    else if (order.CurrentStatus == 3)
//        $('#order_status').append('Доставленый '); 
//    else if (order.CurrentStatus == 4)
//        $('#order_status').append('Не доставлен');
//    else if (order.CurrentStatus == 5)
//        $('#order_status').append('Оплачен');

//    $.each(order.OrderDetail, function (key, detail) {
//        add_product_item_to_dom(detail.Product, detail.Quantity, detail.Id);
//    });
}

function clear_order() {
    $('#order_title').empty();
    $('#order_status').empty();
    $('#order_list').empty()
    $('#order_date').empty()
    $('#order_id').val(0)
}

function add_product_item_to_dom(product, quantity, detail_id) {

    var item_li = $('#li_of_' + product.Id);    
    var div_product_title = $('<div class="product_title_in_order_div"/>');
    var div_product_price = $('<div class="product_price_in_order_div"/>');
    var div_product_quantity = $('<div class="product_quantity_in_order_div"/>');

    if ((item_li.length != 0)) {

        if (item_li.attr('deleted')) {
            item_li.removeAttr('deleted');
            $('#li_of_' + product.Id).css('display', 'block');
            item_li.val(0);
        }
        
        item_li.empty();        
        var prev_value = parseInt(item_li.val());
        item_li.val(prev_value + 1);            
    }
    else {
        item_li = $('<li id="li_of_' + product.Id + '"/>');
        item_li.css('margin', '1em');
        if(typeof detail_id == 'undefined')
            item_li.attr('order_detail_id', 0);
        else
            item_li.attr('order_detail_id', detail_id);
        if(typeof quantity == 'undefined')
            item_li.val(1);
        else
            item_li.val(quantity);
    }

    div_product_title.appendTo(item_li);
    div_product_title.empty();
    div_product_title.append(product.Title);

    div_product_quantity.appendTo(item_li);
    div_product_quantity.empty();
    div_product_quantity.append(item_li.val());

    div_product_price.appendTo(item_li);
    div_product_price.empty();
    div_product_price.append('<b>' + product.Price + '</b>');

    var delete_a_div = $('<div class="delete_link_in_order_div"/>').appendTo(item_li);

    var delete_a = $('<a>', {
        text: '',
        title: 'Delete',
        href: '#',
        click: function () { delete_from_order(product); return false; }
    }).append('<img src ="../Scripts/images/delete.png" style="width:20px;"/>').appendTo(delete_a_div);

    item_li.appendTo($('#order_list'));
}


function get_order_from_dom() {

    var Id = parseInt($('#order_id').val());
    var date = $('#order_date').val();

    var orderDetail = new Array();

    var li_s = $('#order_list').children().each(function () {
        var product_id = $(this).attr('id');
        product_id = product_id.substring(6);
        product_id = parseInt(product_id);
        var single_detail = { Quantity: $(this).val(), Product: { Id: product_id }, Id: $(this).attr('order_detail_id') };
        orderDetail.push(single_detail);
    });

    var order = { Id: Id, Date: date, OrderDetail: orderDetail };

    return order;
}

function remove_product_from_order_dom(product) {

    $('#li_of_' + product.Id).css('display', 'none');
    $('#li_of_' + product.Id).attr('deleted', 'true');
}

function get_deleted_detail_list() {
    var deleted_detail_list = new Array();
    var deleted = $('[deleted="true"]').each(function (index) {
        deleted_detail_list.push($(this).attr('order_detail_id'));
    });
    return deleted_detail_list;
}