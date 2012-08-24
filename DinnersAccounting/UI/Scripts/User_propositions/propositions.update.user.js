/// <reference path="references.js" />


function UpdateDatesListAndDisplayingProposition(proposition_id) {
    $.ajax('/api/Propositions/GetPropositionsDates', {
        success: function (dates_dictionary) {
            $('#date_select').empty();
            $.each(dates_dictionary, function (key, value) {
                var id = key;
                var val = value;
                var option = $('<option />').append(value);
                option.val(key);
                option.appendTo($('#date_select'));
            });

            if (proposition_id == null)
                proposition_id = $('#date_select').val();

            if (proposition_id != null)
                UpdateContinuousProposition(proposition_id);
            else
                ClearContinuousProposition();
            
            

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

function UpdateContinuousProposition(id) {
    var address = null;
    if (typeof (id) !== 'undefined' && id != null)
        address = "/api/Propositions/Get/" + id;
    else
        address = "/api/Propositions/Current";

    $.ajax(address, {
        success: function (ContinuousProposition, textStatus, jqXHR) {

            $('#continuous_proposition_parent').empty();

            $('<div "class="accordion" id="continuous_proposition"/>').appendTo($('#continuous_proposition_parent'));

            var DayPropositions = ContinuousProposition.DayPropositions;

            $.each(DayPropositions, function (key_dp, DayProposition) {

                $('#continuous_proposition').append('<h3 class="accordion_header" date="' + DayProposition.Date + '"><a href="#" id="date">' + DayProposition.Date.split("T")[0] + '</a></h3>');

                var productsHtml = $('<div/>');

                var sorted_day_proposition_products = sort_products(DayProposition.Products);

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
                            var date = new Date(DayProposition.Date);
                            add_product_to_order(Product, DayProposition.Date); return false;
                        }
                    }).append('<div style="display:inline-block;margin-left:50px;"><img src ="../Scripts/images/add_to_order.png" style="width:60px;vertical-align: middle;"/></div>').appendTo(item_li);

                });

                $.each(ContinuousProposition.Products, function (key_dp_pr, Product) {

                    if (typeof (Product.Summary) == 'undefined')
                        Product.Summary = '';

                    var item_li = $('<li>').append('<b>' + Product.Title + '</b><br/>' + Product.Summary + '<br/><br/><b>Цена:  </b>' + Product.Price); ;
                    item_li.appendTo(every_day_in_ul);

                    $('<a>', {
                        text: '',
                        title: 'Добавить в заказ',
                        href: '#',
                        click: function () {
                            var date = new Date(DayProposition.Date);
                            add_product_to_order(Product, DayProposition.Date); return false;
                        }
                    }).append('<div style="display:inline-block;margin-left:50px;"><img src ="../Scripts/images/add_to_order.png" style="width:60px;vertical-align: middle;"/></div>').appendTo(item_li);

                });

                proposition_ul.treeview({
                    collapsed: true
                });
                $('#continuous_proposition').append(productsHtml);
            });

            //-----------------------------------
            $('#continuous_proposition').accordion('destroy').accordion({ clearStyle: true, autoHeight: false, collapsible: true, active: false,
                change: function (event, ui) {
                    if (ui.newHeader.length == 0) {
                        isUnsaved = false;
                        hide_order();
                        return false;
                    }
                    var date = ui.newHeader.attr('date');
                    composing_date = date;
                    get_order(date);
                    active_accordion_index = $('#continuous_proposition').accordion("option", "active");
                    isUnsaved == false;
                }
            });
            activate_disabling();
            $(".collapsed_first").click();

        },
        error: function (jqXHR, textStatus, errorThrown) {

        } //on error
    });

}

function activate_disabling() {
    var accordion = $('#continuous_proposition').data("accordion");
    accordion._std_clickHandler = accordion._clickHandler;
    accordion._clickHandler = function (event, target) {
        var clicked = $(event.currentTarget || target);
        if (!clicked.hasClass("ui-state-disabled")) {
            this._std_clickHandler(event, target);
        }
    };
}
