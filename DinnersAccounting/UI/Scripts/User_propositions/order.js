/// <reference path="references.js" />

function add_product_to_order(product, date) {

    isUnsaved = true;
    $('.accordion_header').addClass("ui-state-disabled"); 
    add_product_item_to_dom(product);
    
}

function delete_from_order(product) {
    remove_product_from_order_dom(product);
    isUnsaved = true;
}

function get_order(date) {

    $.ajax('/api/Orders/GetByDateForCurrentUser', {
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { date_: date },
        success: function (Order, textStatus, jqXHR) {
            show_order();
            if (Order == null)
                draw_empty_order(date);
            else
                draw_existing_order(Order);

            isUnsaved = false;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('error');
            $('.accordion_header').removeClass("ui-state-disabled"); 
    
        }
    });
}

function commit_order() {

    var order = get_order_from_dom();

    if (order.Id == 0) {

        $.ajax('/api/Orders/Create',
        {
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(order),
            success: function (Order, textStatus, jqXHR) {
                get_order(composing_date);
                isUnsaved = false;
                $('.accordion_header').removeClass("ui-state-disabled"); 
    
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    else {
        $.ajax('/api/Orders/Create',
        {
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(order),
            success: function (Order, textStatus, jqXHR) {

                var deleted_detail_list = get_deleted_detail_list();
                deleted_detail_list.push(order.Id);
                $.ajax('/api/Orders/RemovePreviousDetails',
                    {
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(deleted_detail_list),
                        success: function (Order, textStatus, jqXHR) {

                            get_order(composing_date);
                            isUnsaved = false;
                            $('.accordion_header').removeClass("ui-state-disabled");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                        }
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });  
    }
}

function cancel_order_changes() { 
    isUnsaved = false;
    $('.accordion_header').removeClass("ui-state-disabled"); 
    get_order(composing_date);
}

