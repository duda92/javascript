/// <reference path="references.js" />


function clear_orders_list() {
    orders_list_div.empty();
}

function show_orders_list() {
    orders_list_div.css('display', 'inline-block');
}

function hide_orders_list() {
    orders_list_div.css('display', 'none');
}

function get_order(order_id) {
    var order;
    $.ajax('/api/Orders/Get', {
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { orderId: order_id },
        success: function (Order, textStatus, jqXHR) {
            order = JSON.parse(Order);
        }
    });
    return order;
}

function create_orders_list(date) {
    
    var url = '/api/Orders/GetAllOrdersForDate';
    var day_orders_list;
    $.ajax(url, {
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: { date: date },
        success: function (day_orders_list_) {
            day_orders_list = JSON.parse(day_orders_list_);
        }
    });

    $.each(day_orders_list, function (index) {
        var order = day_orders_list[index];
        draw_order_with_user(order);
    });
}

function draw_order_with_user(order) {
    var div = $('<div>').addClass('order_item_div ui-widget-content').attr('order_id', order.Id).appendTo(orders_list_div);
    var user_div = $('<div>').addClass('user_div').appendTo(div);
    var order_div = $('<div>').addClass('order_div').attr('order_id', order.Id).appendTo(div);

    create_user_content(user_div, order);
    create_order_content(order_div, order);
}

function create_user_content(div, order) {    
    //Todo: make it better
    div.append(order.Person.FullName).append('<br/>').append('Balance: ').append(order.Person.Balance);
}

function create_order_content(order_div, order) {
    draw_order(order_div, order);
}

function draw_order(order_div, order) {
    
    order_div.attr('date', order.Date);
    order_div.attr('order_id', order.Id);

    var ul = $('<ul id="order_id_' + order.Id + '">').appendTo(order_div);

    $.each(order.OrderDetail, function (key, detail) {
        add_product_to_order_dom(order, detail.Product, detail.Quantity, detail.Id, false);
    });

    var change_acion_div = $('<div>').addClass('change_order_panel').appendTo(order_div);
    $('<a>', {
        href: "#",
        text: "Изменить",
        title: 'Изменить',
        click: function () { edit_order(order); }
    }).addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only edit_link').appendTo(change_acion_div);
    $('<a>', {
        href: "#",
        text: "Сохранить",
        title: 'Сохранить',
        click: function () { commit_order(order); }
    }).css('display', 'none').addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only save_link').appendTo(change_acion_div);
    $('<a>', {
        href: "#",
        text: "Отмена",
        title: 'Отмена',
        click: function () { cancel_edit_order(order); }
    }).css('display', 'none').addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only cancel_link').appendTo(change_acion_div);
    $('<a>', {
        href: "#",
        text: "Удалить",
        title: 'Удалить',
        click: function () { delete_order(order); }
    }).addClass('link_button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only delete_link').appendTo(change_acion_div);

}

function edit_order(order) {

    edited_order = order;

    enable_removing_from_order(true, order.Id);
    enable_accordion_change(false);


    var proposition_for_order;
    var url = '/api/Propositions/PropositionForOrder';
    $.ajax(url, {
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: { date: order.Date },
        success: function (proposition_for_order_) {
            proposition_for_order = JSON.parse(proposition_for_order_);
        }
    });
    draw_proposition(proposition_for_order);
    enable_proposition_div(true);
    enable_orders(false);
    enable_save_changes(order.Id);
}

function cancel_edit_order(order) {
    enable_orders(true);
    enable_accordion_change(true);
    enable_removing_from_order(false, order.Id);
    enable_proposition_div(false);

    remove_order_from_dom(order);
    order = get_order(order.Id);
    draw_order_with_user(order);

    edited_order = null;
}

function delete_order(order) {
    var id = order.Id;
    var url = '/api/Orders/Delete';
    $.ajax(url, {
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: { orderId: id },
        success: function () {
            remove_order_from_dom(order);
        }
    });

    enable_orders(true);
    enable_accordion_change(true);
    enable_removing_from_order(false, order.Id);
    enable_proposition_div(false);

    edited_order = null; 
}

function commit_order(order) {
    var order = get_order_from_dom();

    if (order.Id == 0) {

        $.ajax('/api/Orders/CreateFromAdmin',
        {
            type: 'POST',
            async:false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(order),
            success: function (Order, textStatus, jqXHR) {
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    else {
        $.ajax('/api/Orders/CreateFromAdmin',
        {
            type: 'POST',
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(order),
            success: function (Order, textStatus, jqXHR) {
                var deleted_detail_list = get_deleted_detail_list();
                deleted_detail_list.push(order.Id);
                $.ajax('/api/Orders/RemovePreviousDetails',
                    {
                        type: 'POST',
                        async: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(deleted_detail_list),
                        success: function (Order, textStatus, jqXHR) {
                            
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                        }
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    
    enable_orders(true);
    enable_accordion_change(true);
    enable_removing_from_order(false, order.Id);
    enable_proposition_div(false);

    edited_order = null;
}
