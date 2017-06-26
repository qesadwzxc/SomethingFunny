// 拖拽插件
(function ($) {
    $.fn.dragSort = function (_aoOptions) {
        var _oSelf$ = this;

        // 覆盖参数默认设置(这部分为不可配置)
        _oSelf$.config = $.extend({
            container$: _oSelf$,
            selectEle: null,
            dashed: "#js_dashed"
        }, $.fn.dragSort.defaults, _aoOptions);

        _oSelf$.children(_oSelf$.config.canDragEle).off("mousedown").mousedown(function (event) {
            // 排除非左击
            if (event.which != 1) return;
            //阻止选中文本
            event.preventDefault();

            var _oTarget$ = $(this);
            // 全局属性
            _oTarget$.extend({
                downMouseX: 0,
                downMouseY: 0,
                downLeft: 0,
                downTop: 0,
                downWidth: 0,
                downHeight: 0,
                scrollTimer: null,
                scrollDirection: 0 //自动滚动方向，0-不滚动，-1-向上，1-向下
            });

            // mousedown
            $.fn.dragSort._mousedown.call(_oSelf$, event, _oTarget$);

            // mousemove
            $(document).mousemove(function (event) {
                $.fn.dragSort._mousemove.call(_oSelf$, event, _oTarget$);
            });

            // mouseup
            $(document).mouseup(function (event) {
                $.fn.dragSort._mouseup.call(_oSelf$, event, _oTarget$);
            });

            return false;
        });

        // 拖拽回位，并删除虚线框
        // 说明：回调函数请根据是不是分类有高亮Class来判断是不是要回位
        this.sortEnd = function () {
            var position = $(_oSelf$.config.dashed).offset();
            _oSelf$.config.selectEle.animate({ "left": position.left, "top": position.top }, 100, function () {
                _oSelf$.config.selectEle.removeAttr("style");
                $(_oSelf$.config.dashed).replaceWith(_oSelf$.config.selectEle);
            });
            return this;
        }

        // 添加到分类-移除虚线框与结点
        this.addToCategory = function () {
            $(_oSelf$.config.dashed).remove();
            _oSelf$.config.selectEle.remove();
            return this;
        }
        return this;
    }


    // 默认参数设置(可配置)
    $.fn.dragSort.defaults = {
        canDragEle: "li",				//可以拖拽元素
        highLightParent: ".sidebar_navigation_block",	//分类的父级
        highLightEle: "li",				//分类的元素
        highLightClass: "js_highlight",	//分类的class
        hasNotHover: true, 				//是否有不可排序元素
        notHoverClass: ".icon_star",	//指定排序元素中2个分类之中的一个
        dragUpCallback: null,			//回调函数
        sensitive: 80, 					//自动滚动敏感度，默认80px
        scrollSpeed: 4,					//自动滚动速度，值越小，滚动的越快
        dashedStyle: { "margin-bottom": "4px" },	//虚线框的附加样式
        dragEleStyle: { "opacity": "0.7" }		//拖拽元素的附加样式
    }

    $.fn.dragSort._mousedown = function (event, _oTarget$) {
        var _oSelf$ = _oTarget$,
			this$ = this,
			offset = _oSelf$.offset();

        this$.config.selectEle = _oTarget$;				//被拖拽元素
        _oSelf$.downMouseX = event.pageX;			//鼠标x坐标
        _oSelf$.downMouseY = event.pageY;			//鼠标y坐标		
        _oSelf$.downLeft = offset.left;			//li左顶点坐标x
        _oSelf$.downTop = offset.top;			//li左顶点坐标y
        _oSelf$.downWidth = _oSelf$.width();		//li宽
        _oSelf$.downHeight = _oSelf$.height();		//li宽

        // 定义样式
        var dashedStyle = $.extend({
            "border": "2px dashed #ccc",
            "height": _oSelf$.downHeight - 2
        }, this$.config.dashedStyle),
			dragEleStyle = $.extend({
			    "left": _oSelf$.downLeft,
			    "top": _oSelf$.downTop,
			    "width": _oSelf$.downWidth,
			    "height": _oSelf$.downHeight,
			    "position": "absolute",
			    "opacity": "0.7",
			    "z-index": 999
			}, this$.config.dragEleStyle);

        // 添加虚线框
        _oSelf$.before('<div id="js_dashed"></div>');
        $(this$.config.dashed).css(dashedStyle);

        // 保持原来的宽高，可移动
        _oSelf$.css(dragEleStyle);


        // 上下自动滚动
        _oSelf$.scrollTimer = setInterval(function () {
            if (_oSelf$.scrollDirection) {
                window.scrollBy(0, _oSelf$.scrollDirection);
                if (parseInt($(document).scrollTop()) == 0 || $(document).scrollTop() + $(window).height() >= $(document).height()) {
                    _oSelf$.scrollDirection = 0;
                }
                _oSelf$.css({ "top": _oSelf$.offset().top + _oSelf$.scrollDirection });
            }
        }, this$.config.scrollSpeed);
    }

    $.fn.dragSort._mousemove = function (event, _oTarget$) {
        event.preventDefault();

        var _oSelf$ = _oTarget$,
			this$ = this;

        // 移动选中块（顶点+移动距离）
        var moveLeft = _oSelf$.downLeft + event.pageX - _oSelf$.downMouseX,
			moveTop = _oSelf$.downTop + event.pageY - _oSelf$.downMouseY,
			winWidth = $(window).width(),
			winHeight = $(window).height(),
			scrollTop = $(document).scrollTop();

        // 移动排序
        var _moveToSort = function () {
            // 判断是否有不可移动位置,开始排序
            var notHoverClass = this$.config.hasNotHover ? this$.config.notHoverClass : "",
				nowClass = _oSelf$.attr("class"),
				notHoverClassReg = new RegExp(notHoverClass),
				sortLi$;

            // 判断是星标还是非星标
            if (notHoverClassReg.test(nowClass)) {
                sortLi$ = $(this$.config.container$).children(notHoverClass).not(_oSelf$);
            } else {
                sortLi$ = $(this$.config.container$).children().not(_oSelf$).not(notHoverClass)
            }

            // 移动排序
            sortLi$.each(function (i) {
                var _oTarget$ = $(this),
					offset = _oTarget$.offset(),
					minLeft = offset.left,
					maxLeft = offset.left + _oSelf$.downWidth,
					minTop = offset.top,
					maxTop = offset.top + _oSelf$.downHeight;

                if (minLeft < event.pageX && event.pageX < maxLeft && minTop < event.pageY && event.pageY < maxTop) {
                    $("." + this$.config.highLightClass).removeClass(this$.config.highLightClass);
                    if (!_oTarget$.next(this$.config.dashed).length) {
                        $(this$.config.dashed).insertAfter(_oTarget$);
                    } else {
                        $(this$.config.dashed).insertBefore(_oTarget$);
                    }
                }
            });
        }

        // 移动到分类-高亮
        var _moveToCategory = function () {
            if (this$.config.highLightParent != "") {
                $(this$.config.highLightParent).find(this$.config.highLightEle).each(function (i) {
                    var _oTarget$ = $(this),
						offset = _oTarget$.offset(),
						minLeft = offset.left,
						maxLeft = offset.left + _oTarget$.width(),
						minTop = offset.top,
						maxTop = offset.top + _oTarget$.height();

                    if (minLeft < event.pageX && event.pageX < maxLeft && minTop < event.pageY && event.pageY < maxTop) {
                        $("." + this$.config.highLightClass).removeClass(this$.config.highLightClass);
                        $(this).addClass(this$.config.highLightClass);
                    }
                });
            }
        }

        // 边缘检测
        var _edgeJudge = function () {
            // 移出左侧边界
            if (moveLeft <= 0) {
                moveLeft = 0;
            }

            // 移出右侧边界
            if (moveLeft >= winWidth - _oSelf$.downWidth - 1) {
                moveLeft = winWidth - _oSelf$.downWidth - 1;
            }

            // 移出顶部
            if (moveTop <= scrollTop + this$.config.sensitive) {
                _oSelf$.scrollDirection = -1;
            }

            // 不触发自动滚动
            if (scrollTop + this$.config.sensitive < moveTop && moveTop < winHeight + scrollTop - this$.config.sensitive) {
                _oSelf$.scrollDirection = 0;
            }

            //移出底部
            if (moveTop >= winHeight + scrollTop - this$.config.sensitive) {
                _oSelf$.scrollDirection = 1;
            }
        }


        _moveToSort();
        _moveToCategory();
        _edgeJudge();
        // 最后设置选中块样式
        _oSelf$.css({ "left": moveLeft, "top": moveTop });
    }

    $.fn.dragSort._mouseup = function (event, _oTarget$) {
        var _oSelf$ = _oTarget$,
			this$ = this;

        // 解除绑定
        $(document).off('mouseup').off('mousemove');
        clearInterval(_oSelf$.scrollTimer);

        // 回调函数
        this$.config.dragUpCallback(_oSelf$, event);
    }
})(jQuery);


$(function () {
    var test = $(".list_con").dragSort({
        canDragEle: "li",				//可以拖拽元素
        highLightParent: ".sidebar_navigation_block",	//分类的父级
        highLightEle: "li",				//分类的元素
        highLightClass: "js_highlight",	//分类的class
        hasNotHover: true, 				//是否有不可排序元素
        notHoverClass: ".icon_star",	//指定排序元素中2个分类之中的一个
        dragUpCallback: callback,		//回调函数
        sensitive: 80, 					//自动滚动敏感度，默认80px
        scrollSpeed: 1,					//自动滚动速度
        dashedStyle: { "margin-bottom": "4px" },	//虚线框的附加样式
        dragEleStyle: { "opacity": "0.7" }		//拖拽元素的附加样式
    });

    function callback(obj, event) {
        var current = currentView.call(obj, event);
        var selectEle$ = obj;
        var category$ = $(".sidebar_navigation_block").find("li.js_highlight");
        if (current) {
            // 添加到分组
            test.addToCategory();
            category$.removeClass("js_highlight");
            console.log("选中：" + $.trim(selectEle$.text()) + " 添加到：" + $.trim(category$.text()));
        } else {
            // 排序
            test.sortEnd();
            console.log("选中：" + $.trim(selectEle$.text()));
        }
    }
    // 判断在分类区域还是排序区域
    function currentView(event) {
        var _oTarget$ = $(".sidebar_navigation_block"),
			offset = _oTarget$.offset(),
			minLeft = offset.left,
			maxLeft = offset.left + _oTarget$.width(),
			minTop = offset.top,
			maxTop = offset.top + _oTarget$.height();

        if (minLeft < event.pageX && event.pageX < maxLeft && minTop < event.pageY && event.pageY < maxTop) {
            return 1;
        } else {
            return 0;
        }
    }

});
