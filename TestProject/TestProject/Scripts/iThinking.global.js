/*
* JS全局服务（依赖jQuery和jQueryUI）
* Copyright 2010-2015, thinking_shi@qq.com
* 2014-2-5
*/

(function (window) {

    // 基本属性
    var _global = function () {
        /// <summary>
        /// 全局服务类
        /// </summary>

        this.systemId = "iThinking";
        this.document = window.document;
        this.firstSeparator = "|";
        this.tipElementId = this.systemId + "_Tip";
        this.messageElementId = this.systemId + "_Message";
        this.dialogElementId = this.systemId + "_Dialog";

        //样式
        this.requiredClass = "iThinking_required";//必填
        this.readonlyClass = "iThinking_readonly";//只读
        this.integerClass = "iThinking_int";//纯数字
        this.floatClass = "iThinking_float";//浮点数
        this.dateClass = "iThinking_date";//日历
        this.mobileClass = "iThinking_mobile";//手机
        this.telClass = "iThinking_tel";//座机
        this.emailClass = "iThinking_email";//邮箱
        this.idcardClass = "iThinking_idcard";//身份证
        this.ipClass = "iThinking_ip";//ip
        this.urlClass = "iThinking_url";//网址
        this.focusedClass = "iThinking_focused";
        this.searchButtonClass = "iThinking_btnSearch";
        this.imgPreviewClass = "iThinking_imgPreview";
        this.addFavoriteClass = "iThinking_addFavorite";
        this.addHomepageClass = "iThinking_addHomepage";
        this.checkAllClass = "iThinking_checkAll";
        this.checkOneClass = "iThinking_checkOne";
        this.panelClass = "iThinking_panel";
        this.imageButtonClass = "iThinking_btnImg";
        this.plusButtonClass = "btnImgPlus";
        this.minusButtonClass = "btnImgMinus";
        this.stateErrorClass = "ui-state-error";
        this.cornerAllClass = "ui-corner-all";
        this.hiddenClass = "ui-helper-hidden";
        this.tipIconClass = "ui-icon";
        this.dialogIconClass = "dialogIcon";
        this.globalLoadingClass = "globalLoading";

        //默认值
        this.defaultTipIconClass = "ui-icon-info";
        this.defaultDialogIconClass = "dialogIconInfo";
        this.defaultPanelButtonClass = this.plusButtonClass;
        this.defaultDuration = 3000;
        this.defaultDateFormat = "yy/mm/dd";
    };

    //基础方法
    _global.prototype.trimLeft = function (s, trimming) {
        /// <summary>
        /// 首截去
        /// </summary>
        if (!trimming) {
            trimming = "\\s|\\u00A0";
        }
        var pattern = new RegExp("^(" + trimming + ")+", "g");
        return s.replace(pattern, "");
    };
    _global.prototype.trimRight = function (s, trimming) {
        /// <summary>
        /// 尾截去
        /// </summary>
        if (!trimming) {
            trimming = "\\s|\\u00A0";
        }
        var pattern = new RegExp("(" + trimming + ")+$", "g");
        return s.replace(pattern, "");
    };
    _global.prototype.trim = function (s, trimming) {
        /// <summary>
        /// 首尾截去
        /// </summary>
        var _this = this;
        return _this.trimRight(_this.trimLeft(s, trimming), trimming);
    };
    _global.prototype.padLeft = function (s, width, padding) {
        /// <summary>
        /// 左填充
        /// </summary>
        var result = s;
        if (!padding) {
            padding = " ";
        }
        while (result.length < width) {
            result = padding + result;
        }
        return result;
    };
    _global.prototype.padRight = function (s, width, padding) {
        /// <summary>
        /// 右填充
        /// </summary>
        var result = s;
        if (!padding) {
            padding = " ";
        }
        while (result.length < width) {
            result = result + padding;
        }
        return result;
    };
    _global.prototype.leftWith = function (s, matching) {
        /// <summary>
        /// 起始于
        /// </summary>
        if (!matching) {
            matching = "\\s|\\u00A0";
        }
        var pattern = new RegExp("^(" + matching + ")", "g");
        return pattern.test(s);
    };
    _global.prototype.rightWith = function (s, matching) {
        /// <summary>
        /// 结束于
        /// </summary>
        if (!matching) {
            matching = "\\s|\\u00A0";
        }
        var pattern = new RegExp("(" + matching + ")$", "g");
        return pattern.test(s);
    };
    _global.prototype.format = function (s) {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        var result = s;
        for (var i = 1; i < arguments.length; i++) {
            var pattern = new RegExp("(\\{" + (i - 1) + "\\})", "g");
            result = result.replace(pattern, arguments[i].toString());
        }
        return result;
    };
    _global.prototype.round = function (d, precision) {
        /// <summary>
        /// 按精度四舍五入
        /// </summary>
        if (!precision) {
            precision = 0;
        }
        var multiple = Math.pow(10, precision);
        return Math.round(d * multiple) / multiple;
    };

    //高级方法
    _global.prototype.checkRequired = function (parent) {
        /// <summary>
        /// 验证必填栏位
        /// </summary>
        var _this = this;
        if (typeof (parent) != "object") {
            parent = _this.document;
        }
        var result = true;
        $(parent).find("." + _this.requiredClass).each(function () {
            if ($(this).val().length < 1) {
                $(this).addClass(_this.stateErrorClass).focus();
                //iThinking.showTip(this, "Required");
                result = false;
            }
            else {
                $(this).removeClass(_this.stateErrorClass);
            }
        });
        return result;
    };
    _global.prototype.goBack = function () {
        /// <summary>
        /// 返回上一页
        /// </summary>
        history.back();
        return false;
    };
    _global.prototype.addFavorite = function () {
        /// <summary>
        /// 添加到收藏夹
        /// </summary>
        var title = document.title;
        var url = document.location.href;
        var ctrl = (navigator.userAgent.toLowerCase()).indexOf('mac') != -1 ? 'Command/Cmd' : 'CTRL';
        if (document.all) {//IE
            window.external.addFavorite(url, title);
        } else if (window.sidebar) {
            window.sidebar.addPanel(title, url, "");
        } else {
            alert("添加失败\n您可以尝试通过快捷键 " + ctrl + " + D 加入到收藏夹");
        }
    };
    _global.prototype.addHomepage = function () {
        /// <summary>
        /// 设为浏览器主页
        /// </summary>
        if (document.all) {//IE 
            document.body.style.behavior = 'url(#default#homepage)';
            document.body.setHomePage(document.location.href);
        } else {
            alert("设置首页失败，请手动设置");
        }
    };
    _global.prototype.openLoading = function () {
        /// <summary>
        /// 全局Loading-打开
        /// </summary>
        $("." + iThinking.globalLoadingClass).parent().children().first().attr("style", "display:none;");
        $("." + iThinking.globalLoadingClass).dialog("open");
    };
    _global.prototype.closeLoading = function () {
        /// <summary>
        /// 全局Loading-关闭
        /// </summary>
        $("." + iThinking.globalLoadingClass).dialog("close");
    };

    //消息框、弹窗
    _global.prototype.showTip = function (sender, message, iconClass, duration) {
        /// <summary>
        /// 显示提示
        /// </summary>
        var _this = this;
        if (typeof (sender) != "object") {
            return;
        }
        if (!iconClass) {
            iconClass = _this.defaultTipIconClass;
        }
        if (!duration) {
            duration = _this.defaultDuration;
        }
        //容器
        var id = _this.tipElementId;
        var container = $("#" + id);
        if (container.size() == 0) { //不存在则创建
            container = $("<div style='padding: 5px; position: absolute' />")
                .attr("id", id)
                .addClass(_this.stateErrorClass).addClass(_this.cornerAllClass).addClass(_this.hiddenClass)
                .append("<span style='float: left; margin: 2px 0px 0px 0px;'></span>") //图标容器
                .append("<span style='display: block; margin: 0px 0px 0px 20px;'></span>") //消息容器
                .appendTo($(_this.document.body));
        }
        //图标
        container.children("span:first")
            .removeClass()
            .addClass(_this.tipIconClass).addClass(iconClass);
        //消息
        container.children("span:last").html(message);
        //呈现
        var senderPosition = $(sender).position();
        container
            .stop(true, true) //初始化动画
            .css("left", senderPosition.left + 185 + $(sender).outerWidth() + 1).css("top", senderPosition.top + 48) //位置
            .fadeIn() //显示
            .delay(duration)
            .fadeOut("slow"); //隐藏
    };
    _global.prototype.showMesage = function (message, title, iconClass) {
        /// <summary>
        /// 显示消息
        /// </summary>
        var _this = this;
        if (!title) {
            title = "系统消息";
        }
        if (!iconClass) {
            iconClass = _this.defaultDialogIconClass;
        }
        //容器
        var id = _this.messageElementId;
        var container = $("#" + id);
        if (container.size() == 0) { //不存在则创建
            container = $("<div />")
                .attr("id", id)
                .addClass(_this.hiddenClass)
                .append("<span style='float: left;'></span>") //图标容器
                .append("<span style='display: block; margin: 0;'></span>") //消息容器
                .appendTo($(_this.document.body))
                .dialog({
                    autoOpen: false,
                    modal: true,
                    buttons: [
                        {
                            text: "确定",
                            click: function () { $(this).dialog("close"); }
                        }
                    ],
                    minWidth: 300,
                    minHeight: 160,
                    show: "fade",
                    hide: "fade"
                });
        }
        //标题
        container.dialog("option", "title", title);
        //图标
        container.children("span:first")
            .removeClass()
            .addClass(_this.dialogIconClass).addClass(iconClass);
        //消息
        container.children("span:last").html(message);
        //呈现
        container.dialog("open");
    };
    _global.prototype.showDialog = function (windowId, url, width, height, title) {
        /// <summary>
        /// 显示弹窗
        /// </summary>
        var _this = this;
        if (!title) {
            title = "系统弹窗";
        }
        //容器
        var id = _this.dialogElementId + "_" + windowId;
        var container = $("#" + id);
        if (container.size() == 0) { //不存在则创建
            container = $("<div />")
                .attr("id", id)
                .addClass(_this.hiddenClass)
                .append("<iframe style='width: 99%; height: 98%; border:0;'></iframe>") //内容容器
                .appendTo($(_this.document.body))
                .dialog({
                    autoOpen: false,
                    modal: true,
                    minWidth: 300,
                    minHeight: 160,
                    show: "fade",
                    hide: "fade",
                    close: function (event, ui) {
                        $(this).children("iframe:first").removeAttr("src"); //清除内容
                    }
                });
        }
        //标题
        container.dialog("option", "title", title);
        //尺寸
        container.dialog("option", "width", width).dialog("option", "height", height);
        //内容
        container.children("iframe:first").attr("src", url);
        //呈现
        container.dialog("open");
    };
    _global.prototype.closeDialog = function (windowId) {
        /// <summary>
        /// 关闭对话框
        /// </summary>
        var _this = this;
        var container = $("#" + _this.dialogElementId + "_" + windowId);
        if (container.size() > 0 && container.dialog("isOpen")) {
            container.dialog("close");
        }
    };

    //发布
    window.iThinking = new _global();
})(window);
