/*
* JS公共扩展（依赖jQuery和jQueryUI）
* Copyright 2010-2015, thinking_shi@qq.com
* 2014-2-5
*/

$(function () {
    //鼠标放到tabel上，自动改变当前行颜色
    //    $("table tbody tr").hover(function () {
    //        $(this).find("td").addClass("highlight");
    //    }, function () {
    //        $(this).find("td").removeClass("highlight");
    //    });

    //日历
    $("." + iThinking.dateClass).attr("readonly", "readonly").datepicker({ dateFormat: 'yy/mm/dd' });

    //只读
    $("." + iThinking.readonlyClass).attr("readonly", "readonly").css("backgroundColor", "#eee").css("color", "#777");

    //手机
    $("." + iThinking.mobileClass).blur(function () {
        if ($(this).val().match(/^0{0,1}(13|14|15|18)[0-9]{9}$/) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect phone");
        }
    });

    //座机
    $("." + iThinking.telClass).blur(function () {
        if ($(this).val().match(/^(([0\+]?\d{2,3}([-]|[ ]?))?([0\+]?\d{2,3})([-]|[ ]?))(\d{7,8})(-(\d{3,}))?$/) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect telephone");
        }
    });

    //邮箱
    $("." + iThinking.emailClass).blur(function () {
        if ($(this).val().match(/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect email");
        }
    });

    //身份证
    $("." + iThinking.idcardClass).blur(function () {
        //15位
        var arg1 = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/;
        //18位
        var arg2 = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/;
        if ($(this).val().match(arg1) == null && $(this).val().match(arg2) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect ID card");
        }
    });

    //url
    $("." + iThinking.urlClass).blur(function () {
        if ($(this).val().match(/((http[s]?|ftp):\/\/)?[^\/\.]+?\..+\w$/i) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect url");
        }
    });

    //ip
    $("." + iThinking.ipClass).blur(function () {
        if ($(this).val().match(/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/) == null) {
            $(this).val("");
            iThinking.showTip(this, "Incorrect ip");
        }
    });

    //整数
    $("." + iThinking.integerClass).blur(function () {
        $(this).val($(this).val().replace(/\D+/g, ""));
    }).keypress(function () {
        return (/\d|\r/.test(String.fromCharCode(event.keyCode)));
    });

    //浮点数
    $("." + iThinking.floatClass).blur(function () {
        var value = $(this).val().replace(/[^\d\.]+/g, "");
        //保留第一个小数点
        var radixPointIndex = value.indexOf(".");
        if (radixPointIndex >= 0) {
            value = value.substring(0, radixPointIndex + 1) + value.substr(radixPointIndex + 1).replace(/\.+/g, "");
        }
        $(this).val(value);
    }).keypress(function () {
        return (/\d|\.|\r/.test(String.fromCharCode(event.keyCode)));
    });

    //全选
    //$("." + iThinking.checkAllClass).click(function () {
    //    if ($(this).attr("checked")) {
    //        $("." + iThinking.checkOneClass).attr("checked", "checked");
    //    }
    //    else {
    //        $("." + iThinking.checkOneClass).removeAttr("checked");
    //    }
    //});
    $("." + iThinking.checkAllClass + ":checkbox").each(function () { //input(checkbox)初始化
        var checkAll = $(this);
        var groupStamp = checkAll.attr("title"); //用title作为组标记
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        if (checks.size() > 0) {
            checkAll.attr("checked", checks.size() == checks.filter(":checked").size());
        }
    });
    $("." + iThinking.checkAllClass + ">:checkbox").each(function () { //asp:CheckBox初始化
        var checkAll = $(this);
        var groupStamp = checkAll.parent().attr("title"); //用title作为组标记
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        if (checks.size() > 0) {
            checkAll.attr("checked", checks.size() == checks.filter(":checked").size());
        }
    });
    $("." + iThinking.checkAllClass + ":checkbox").click(function () {
        var checkAll = $(this);
        var groupStamp = checkAll.attr("title"); //用title作为组标记
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        checks.attr("checked", !!checkAll.attr("checked"));
    });
    $("." + iThinking.checkAllClass + ">:checkbox").click(function () {
        var checkAll = $(this);
        var groupStamp = checkAll.parent().attr("title"); //用title作为组标记
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        checks.attr("checked", !!checkAll.attr("checked"));
    });
    $("." + iThinking.checkOneClass + ":checkbox").click(function () {
        var groupStamp = $(this).attr("title"); //用title作为组标记
        var checkAlls = $("." + iThinking.checkAllClass + "[title=" + groupStamp + "]:checkbox, ." + iThinking.checkAllClass + "[title=" + groupStamp + "]>:checkbox");
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        checkAlls.attr("checked", checks.size() == checks.filter(":checked").size());
    });
    $("." + iThinking.checkOneClass + ">:checkbox").click(function () {
        var groupStamp = $(this).parent().attr("title"); //用title作为组标记
        var checkAlls = $("." + iThinking.checkAllClass + "[title=" + groupStamp + "]:checkbox, ." + iThinking.checkAllClass + "[title=" + groupStamp + "]>:checkbox");
        var checks = $("." + iThinking.checkOneClass + "[title=" + groupStamp + "]:checkbox:visible:enabled, ." + iThinking.checkOneClass + "[title=" + groupStamp + "]>:checkbox:visible:enabled");
        checkAlls.attr("checked", checks.size() == checks.filter(":checked").size());
    });

    //下拉框
    $("select").mouseover(function () {
        $(this).attr("title", $(this).children(":selected").text());
    });

    //折叠面板
    $("." + iThinking.panelClass).each(function () {
        var panel = $(this).hover(
            function () { $(this).addClass(iThinking.focusedClass).focus(); },
            function () { $(this).removeClass(iThinking.focusedClass); }
            );
        if (panel.children().size() > 1) {
            panel.wrapInner("<div />") //确保完整的高度值
        }
        var panelHeight = panel.height();
        var contentHeight = panel.children().outerHeight() + parseInt(panel.css("border-top-width")) + parseInt(panel.css("border-bottom-width")) + parseInt(panel.css("padding-top")) + parseInt(panel.css("padding-bottom"));
        panel.data("CollapsedHeight_" + iThinking.systemId, panelHeight);
        panel.data("ExpandedHeight_" + iThinking.systemId, Math.max(panelHeight, contentHeight));

        //面板按钮
        $("<a href='#' style='float: right;' />")
            .addClass(iThinking.imageButtonClass).addClass(iThinking.defaultPanelButtonClass) //初始化
            .prependTo(panel)
            .click(function () {
                var button = $(this);
                var panel = button.parent();
                if (!panel.is(":animated")) {
                    var expectedHeight;
                    if (button.hasClass(iThinking.plusButtonClass)) {
                        button.removeClass(iThinking.plusButtonClass).addClass(iThinking.minusButtonClass);
                        expectedHeight = panel.data("ExpandedHeight_" + iThinking.systemId);
                    }
                    else {
                        button.removeClass(iThinking.minusButtonClass).addClass(iThinking.plusButtonClass);
                        expectedHeight = panel.data("CollapsedHeight_" + iThinking.systemId);
                    }
                    panel.animate({ height: expectedHeight }, "fast");
                }
                return false;
            });
    });

    //焦点特效
    $("input[type=text], input[type=password], textarea, input[type=file], select").focus(function () {
        $(this).addClass(iThinking.focusedClass).select(); //友好性全选
    }).blur(function () {
        $(this).removeClass(iThinking.focusedClass);
    });

    //特殊字符过滤
    $("input[type=text], input[type=password]").blur(function () {
        $(this).val($(this).val().replace(/['<>]/g, "").replace(/\n/g, " "));
    }).keypress(function () {
        if (event.keyCode == 13) { //不影响可能触发的提交
            $(this).triggerHandler("blur");
        }
    });
    $("textarea").blur(function () {
        $(this).val($(this).val().replace(/['<>]/g, ""));
    });

    //将图片在新页面最大展示
    $("." + iThinking.imgPreviewClass).css("cursor", "pointer").click(function () {
        var url = $(this).attr("src");
        if (url) {
            window.open(url, "_blank");
        }
        return false;
    });

    //收藏
    $("." + iThinking.addFavoriteClass).click(function () {
        iThinking.addFavorite();
        return false;
    });

    //设为首页
    $("." + iThinking.addHomepageClass).click(function () {
        iThinking.addHomepage();
        return false;
    });

    //全局Loading
    $("." + iThinking.globalLoadingClass).dialog({
        autoOpen: false,
        height: 100,
        width: 100,
        dialogClass: "my-dialog",
        modal: true
    });
    $(document).ajaxStart(function () {
        $("." + iThinking.globalLoadingClass).dialog("open");
        $("." + iThinking.globalLoadingClass).parent().children().first().attr("style", "display:none;");
    });
    $(document).ajaxStop(function () {
        $("." + iThinking.globalLoadingClass).dialog("close");
    });
});
