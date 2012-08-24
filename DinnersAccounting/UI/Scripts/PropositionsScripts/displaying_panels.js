function hide_panels() {

    ////////////Animated hide : 
    
    //var selectedEffect = 'drop';
    //var options = {};
    //$("#edit_create_product").hide(selectedEffect, options, 0);

    ///////////Fast hide :
    $("#edit_product").css('display', 'none');
    $("#create_product").css('display', 'none');

    hasUnsavedData = false;
}

function hide_validation_errors() {
    $('#edit_product_validation_title').css('display', 'none');
    $('#edit_product_validation_price').css('display', 'none');
    $('#create_product_validation_price').css('display', 'none');
    $('#create_product_validation_title').css('display', 'none');

}

function AlertHasUnsavedData()
{
    $('#unsaved_data_alert').dialog('open');
}

function add_continuous_proposition() {
    $('#create_proposition_dialog').dialog('open');
}