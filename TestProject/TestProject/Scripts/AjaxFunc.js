$(onSub())

function onSub() {
    jQuery(jQuery('td>label').hide())
    $(":input").blur(function () { //注册blur的事件  
        $(this).each(function () { //遍历input元素对象   
            if ("" == $(this).val()) { //判断元素对象的value值  
                //$(this).addClass("blur"); //添加css样式  
                $(this).next("label").show();
            } else {
                $(this).next("label").hide();
                //$(this).removeClass("blur");
            }
        });
    });

    //var td_label = jQuery('td');
    //td_label.each(function () {
    //    var item = jQuery(this).children('input');
    //    var value = $(item).val();
    //    if (item || value == "" || value == null) {
    //        jQuery('label').show();
    //        return false;
    //    }
    //    else { jQuery('label').hide() }
    //});
    //if (jQuery(this).find('input') || jQuery(this).find('input').val() == "" || jQuery(this).find('input').val() == null) {
    //    jQuery(this).find('label').show();
    //    return false;
    //}

}

function onSubmit() {
    $.ajax({
        url: "WebFormTest1.aspx/Submit",
        type: "post",
        dataType: "json",
        data: "{ model:{ EngName:" + $("#txtEngName").val() + ", Telephone: " + $("#txtTelephone").val() + ",Name: " + $("#txtName").val() + ",Array:"+$("#txtArray").val()+"}",
            success: function (msg) { alert(msg.d.id); }
        });
}


