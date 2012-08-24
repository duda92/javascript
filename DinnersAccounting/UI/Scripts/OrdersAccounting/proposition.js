/// <reference path="references.js" />


function draw_proposition(proposition) {

    proposition_div.empty();

    var productsHtml = proposition_div;

    var sorted_day_proposition_products = sort_products(proposition.day_products);

    var proposition_ul = $('<ul id="day_proposition"/>'); proposition_ul.appendTo(productsHtml);

    var complexes_li = $('<li />').append('<span STYLE="font-size: 20pt; font-family:sans-serif;" class="collapsed_first">Комплексы</span>').appendTo(proposition_ul); var complexes_ul = $('<ul />').appendTo(complexes_li);

    var separated_li = $('<li />').append('<span STYLE="font-size: 20pt">Также сегодня в меню</span>').appendTo(proposition_ul); var separated_ul = $('<ul />').appendTo(separated_li);

    var every_day_in_li = $('<li />').append('<span STYLE="font-size: 20pt">Ежедневно в меню</span>').appendTo(proposition_ul); var every_day_in_ul = $('<ul />').appendTo(every_day_in_li);

    //complexes and separated:
    $.each(sorted_day_proposition_products, function (key_dp_pr, Product) {

        if (typeof (Product.Summary) == 'undefined')
            Product.Summary = '';

        var item_li = $('<li>').append('<b>' + Product.Title + '</b><br/>' + Product.Summary + '<br/><br/><b style="margin-left:50px">Цена:  </b>' + Product.Price);

        if (Product.isComplex == true)
            item_li.appendTo(complexes_ul);
        if (Product.isComplex == false)
            item_li.appendTo(separated_ul);

        $('<a>', {
            text: '',
            title: 'Добавить в заказ',
            href: '#',
            click: function () {
                add_product_to_order_dom(edited_order, Product, 1, 0, true);
            }
        }).append('<div style="display:inline-block;margin-left:50px;"><img src ="../Scripts/images/add_to_order.png" style="width:60px;vertical-align: middle;"/></div>').appendTo(item_li);

    });

}

function enable_removing_from_order(enable, order_id){
    var remove_divs = $('div.order_div[order_id=' + order_id + ']').find('.delete_link_in_order_div');
    if (enable == true) {
        remove_divs.css('display', 'inline-block');
    }
    else if (enable == false){
        remove_divs.css('display', 'none');
    }
}
