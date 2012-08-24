/// <reference path="references.js" />

function apply_ui() {

    $('#start_date_input').datepicker();
    $('#end_date_input').datepicker();

    init_date_pickers();
}

function init_date_pickers() {
    $('#start_date_input').val(start_date.getMonth() + 1 + '/' + start_date.getDate() + '/' + start_date.getFullYear());
    $('#end_date_input').val(end_date.getMonth() + 1 + '/' + end_date.getDate() + '/' + end_date.getFullYear());
}

function build_days_accordion() {
    
    var day_order_models = get_day_order_models(start_date, end_date);

    $.each(day_order_models, function (index) {
        var model = day_order_models[index];

        var title = $('<h3>').append(get_humans_date(model.Date)).append($('<a>', { href: "#" }));
        var content = $('<div>').attr('date', model.Date);

        accordion_div.append(title);
        accordion_div.append(content);

        draw_orders_model(content, model);
    });

    accordion_div.accordion('destroy').accordion({ clearStyle: true, autoHeight: false, collapsible: true, active: false });
    activate_disabling();
    accordion_div.bind('accordionchangestart', function (event, ui) {
        if (ui.newContent.lenght == 0) {
            selected_date = null;
            return false;
        }
        selected_date = ui.newContent.attr('date');
        clear_orders_list();
        create_orders_list(selected_date);
        show_orders_list();
    });
}

function draw_orders_model(content, model) {

    var table = $('<table>').appendTo(content);
    var row_1 = $('<tr>').appendTo(table);
    var cur_status_title = $('<td>').width(200).append('Текущий статус').appendTo(row_1);
    var cur_status_value = $('<td>').width(200).append('<b>' + model.current_state.Text + '</b>').appendTo(row_1);

    if (model.current_state.Id == 0) {
    }
    else if (model.current_state.Id == 1) {
        var buttons_div = $('<div>').appendTo(content);
        var apply_button = $('<a>', { text: 'Отправить', title: 'Отправить', href: '#', click: function () { send_orders(); }
        }).addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only edit_link').appendTo(buttons_div);
    }
    else {
        var row_2 = $('<tr>').appendTo(table);
        var choose_status_title = $('<td>').append('Текущий статус').appendTo(row_1);
        var choose_status_value = $('<td>').appendTo(row_1);
        var drop_down_status = $('<select>').appendTo(choose_status_value);
        $.each(model.possible_states, function (index) {
            var state = model.possible_states[index];
            var option = $('<option>').attr('state_id', state.Id).append(state.Text);
            drop_down_status.append(option);
        });

        var buttons_div = $('<div>').appendTo(content);
        var apply_button = $('<a>', { text: 'Применить', title: 'Применить', href: '#', click: function () { apply_orders(); }
        }).addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only edit_link').appendTo(buttons_div);
    }
}

function enable_proposition_div(enable) {
    if (enable == true) {
        proposition_content_div.css('display', 'inline-block');
    }
    else if (enable == false) {
        proposition_content_div.css('display', 'none');
    }
}

function enable_accordion_change(enable) {
    if (enable == true) {
        accordion_div.find('.ui-accordion-header').removeClass("ui-state-disabled");
    }
    else if (enable == false) {
        accordion_div.find('.ui-accordion-header').addClass("ui-state-disabled");
    }
}

function enable_save_changes(order_id) {

    var target_panel = orders_list_div.find('.order_div[order_id=' + order_id + ']').find('.change_order_panel');

    target_panel.css('display', 'inline-block');

    target_panel.find('.save_link').css('display', 'inline-block');
    target_panel.find('.cancel_link').css('display', 'inline-block');
    target_panel.find('.edit_link').css('display', 'none');
    target_panel.find('.delete_link').css('display', 'none');
}

function enable_orders(enable) {

    if (enable == false) {
        orders_list_div.find('.order_div').find('.change_order_panel').css('display', 'none');
    }
    else if (enable == true) {
        var panels = orders_list_div.find('.order_div').find('.change_order_panel');

        panels.css('display', 'inline-block');

        panels.find('.save_link').css('display', 'none');
        panels.find('.cancel_link').css('display', 'none');
        panels.find('.edit_link').css('display', 'inline-block');
        panels.find('.delete_link').css('display', 'inline-block');

    }
}

function activate_disabling() {
    var accordion = accordion_div.data("accordion");
    accordion._std_clickHandler = accordion._clickHandler;
    accordion._clickHandler = function (event, target) {
        var clicked = $(event.currentTarget || target);
        if (!clicked.hasClass("ui-state-disabled")) {
            this._std_clickHandler(event, target);
        }
    };
}


function remove_order_from_dom(order) {
    var target_element = orders_list_div.find('.order_div[order_id=' + order.Id + ']').parent();
    target_element.remove();
}

function add_product_to_order_dom(order, product, quantity, detail_id, isEditing) {

    var ul = $('ul#order_id_' + order.Id);
    var item_li = $('ul#order_id_' + order.Id).find('#li_of_' + product.Id);
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
        if (typeof detail_id == 'undefined')
            item_li.attr('order_detail_id', 0);
        else
            item_li.attr('order_detail_id', detail_id);
        if (typeof quantity == 'undefined')
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

    var delete_a_div = $('<div/>').addClass('delete_link_in_order_div').appendTo(item_li);

    if (isEditing == false) {
        delete_a_div.css('display', 'none');
    }
    else if (isEditing == true) {
        delete_a_div.css('display', 'inline-block');
    }

    var delete_a = $('<a>', {
        text: '',
        title: 'Delete',
        href: '#',
        click: function () { remove_product_from_order_dom(product, order); }
    }).append('<img src ="../Scripts/images/delete.png" style="width:20px;"/>').appendTo(delete_a_div);

    item_li.appendTo(ul);
}


function remove_product_from_order_dom(product, order) 
{
    var target_panel = orders_list_div.find('.order_div[order_id=' + order.Id + ']').find('#li_of_' + product.Id);
    target_panel.css('display', 'none');
    target_panel.attr('deleted', 'true');
}

function get_order_from_dom() {

    var Id = edited_order.Id;
    var date = edited_order.Date;

    var orderDetail = new Array();
    orders_list_div.find('.order_div[order_id=' + edited_order.Id + '] ul').children().each(function () {
        var product_id = $(this).attr('id'); product_id = product_id.substring(6); product_id = parseInt(product_id);
        var single_detail = { Quantity: $(this).val(), Product: { Id: product_id }, Id: $(this).attr('order_detail_id') };
        orderDetail.push(single_detail);
    });

    var order = { Id: Id, Date: date, OrderDetail: orderDetail, Person: edited_order.Person };
    return order;
}

function get_deleted_detail_list() {
    var deleted_detail_list = new Array();
    $('[deleted="true"]').each(function (index) {
        deleted_detail_list.push($(this).attr('order_detail_id'));
    });
    return deleted_detail_list;
}

function send_orders() {
    var url = '/api/Orders/SendDayOrders/';
    $.ajax(url, {
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: { date: selected_date },
        success: function () {
            
        }
    });
}