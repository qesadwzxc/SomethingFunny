//var input = $("#lblAt")
//input.bind('input propertychange focus', OnDown(input));


//function OnDown(input) {
//    alert(input.val());
//    //var text = $("#lblAt").val();
//    //$.ajax({
//    //    //url: 'AtFunc.aspx/OnTextChange',
//    //    //data: { text: text },
//    //    //dataType: "json",
//    //    success: function a(msg) { alert('b');}
//    //});
//}

//function change(msg) {
//    alert(msg);
//}

//定义全局变量用于储存提示层 
var liketips;

//监听改动或得到焦点事件 

$('.getsearchjson').attr("autocomplete", "off");
$('.getsearchjson').bind("propertychange input focus", function (event) {
    $this = $(this);

    //获取input框位置并计算提示层应出现的位置 
    var top = 1 * $this.offset().top + 25;
    var left = 1 * $this.offset().left;
    var width = 1 * $this.width() + 12;

    $(liketips).remove();
    liketips = document.createElement('div');
    $liketips = $(liketips);
    $liketips.addClass("liketips");
    $liketips.css({ top: top + 'px', left: left + 'px', width: width + 'px' });


    $liketips.append('<label>' + $this.val() + '<label>');
    $liketips.appendTo($this.parent());
    $liketips.show();
})

$('.getsearchjson').keyup(function (event) {
    //console.log(event.keyCode); 
    var text = $(".getsearchjson").val();
    $this = $(this);
    if (event.shiftKey && event.which == 50) {
//        while(event.which!=20)
        $.ajax({
            url: "OnTextChange",
            type: "post",
            data: { text: text },
            success: function (msg) {
                alert(msg);
            }
        });
    }
})
//    })
//按键是向下 

//    if (event.type != 'focus') {
//        //如果有改变，则状态定为必须重新选择，因为纯人手输入会导致value无法插入 
//        $this.data('ok', false);
//    }
//    //获取input框位置并计算提示层应出现的位置 
//    var top = 1 * $this.offset().top + 25;
//    var left = 1 * $this.offset().left;
//    var width = 1 * $this.width() + 12;

//    //重建储存提示层并让其在适当位置显示 
//    $(liketips).remove();
//    liketips = document.createElement('div');
//    $liketips = $(liketips);
//    //class样式这里不提供，最主要是position:absolute 
//    $liketips.addClass("liketips");
//    $liketips.css({ top: top + 'px', left: left + 'px', width: width + 'px' });

//    //加载前先显示loading动态图 
//    $liketips.append('<img src="/images/loading.jpg" border="0" />');
//    $liketips.appendTo($this.parent());
//    $liketips.show();

//    //定义ajax去获取json，type参数通过data-type设置，keyword则是目前已输入的值 
//    //返回值以table形式展示 
//    $.post('/data/search.do', { type: $this.data('type'), keyword: $this.val() }, function (json) {
//        $liketips.empty();
//        var htmlcode = "<table cellspacing='0' cellpadding='2'><tbody>";
//        for (var i = 0; i < json.datas.length; i++) {
//            //这里我需要用到value和title两项，所以用data-value传递多一个参数，在回车或鼠标点击后赋值到相应的地方，以此完美地替代select 
//            htmlcode += '<tr data-value="' + json.datas[i][0] + '"><td>' + json.datas[i][1] + '</td></tr>';
//        }
//        htmlcode += "</tbody></table><span>请务必在此下拉框中选择</span>";
//        //把loading动态图替换成内容 
//        $liketips.html(htmlcode);
//    }, "json");
//});

////焦点消失时确保数据来自选项，隐藏提示框体 
//$('.getsearchjson').blur(function () {
//    //因为鼠标点击时blur动作结算在click之前，setTimeout是为了解决这个问题 
//    $oldthis = $(this);
//    setTimeout(function () {
//        if ($oldthis.data('ok'))
//            $(liketips).fadeOut('fast');
//        else {
//            alert('为保证数据准确性，请务必在下拉提示中选择一项，谢谢合作');
//            $oldthis.focus();
//        }
//    }, 200);
//});

////监听键盘动作 
//$('.getsearchjson').keydown(function (event) {
//    //console.log(event.keyCode); 
//    $this = $(this);
//    if (event.keyCode == 40) {
//        //按键是向下 
//        $nowtr = $('tr.selectedtr');
//        //如果已存在选中，则向下，否则，选中选单中第一个 
//        if ($nowtr.length > 0) {
//            $nexttr = $nowtr.next('tr')
//            //如果不是最后选项，向下个tr移动，否则跳到第一个tr 
//            if ($nexttr.length > 0) {
//                $('tr.selectedtr').removeClass();
//                $nexttr.addClass('selectedtr');
//            }
//            else {
//                $('tr.selectedtr').removeClass();
//                $nowtr.parent().find('tr:first').addClass('selectedtr');
//            }
//        }
//        else {
//            $('.liketips').find('tr:first').addClass('selectedtr');
//        }
//    }
//    else if (event.keyCode == 38) {
//        //按键是向上 
//        $nowtr = $('tr.selectedtr');
//        //如果已存在选中，则向下，否则，选中选单中第一个 
//        if ($nowtr.length > 0) {
//            $prevtr = $nowtr.prev('tr')
//            //如果不是最后选项，向下个tr移动，否则跳到第一个tr 
//            if ($prevtr.length > 0) {
//                $('tr.selectedtr').removeClass();
//                $prevtr.addClass('selectedtr');
//            }
//            else {
//                $('tr.selectedtr').removeClass();
//                $nowtr.parent().find('tr:last').addClass('selectedtr');
//            }
//        }
//        else {
//            $('.liketips').find('tr:last').addClass('selectedtr');
//        }
//    }
//    else if (event.keyCode == 13) {
//        //按键是回车，则确定返回并隐藏选框 
//        $nowtr = $('tr.selectedtr');
//        if ($nowtr.length == 1) {
//            //设置value值到input框通过参数data-valueto配置的value值存储项中去，一般是hidden项 
//            $valuefield = $('input[name=' + $this.data('valueto') + ']');
//            $valuefield.val($nowtr.data('value'));
//            $this.val($nowtr.text());
//            //设置状态是从选项中选择，允许blur 
//            $this.data('ok', true);
//        }
//        $(liketips).fadeOut('fast');
//        return false;
//    }
//    //console.log(event.keyCode); 
//    return true;
//});

////监听鼠标动作，mouseover改变选中项 
//$(document).delegate('.liketips td', 'mouseover', function () {
//    $nowtr = $(this).parent();
//    $nowtr.siblings('tr').removeClass();
//    $nowtr.addClass('selectedtr');
//});

////监听鼠标动作，click选定 
//$(document).delegate('.liketips td', 'click', function () {
//    $nowtr = $(this).parent();
//    if ($nowtr.length == 1) {
//        //取得该提示层对应的input框 
//        $inputfield = $nowtr.parent().parent().parent().siblings('input.getsearchjson');

//        //设置value值到input框通过参数data-valueto配置的value值存储项中去，一般是hidden项 
//        $valuefield = $('input[name=' + $inputfield.data('valueto') + ']');
//        $valuefield.val($nowtr.data('value'));
//        $inputfield.val($nowtr.text());
//        //设置状态是从选项中选择，允许blur 
//        $inputfield.data('ok', true);
//    }
//    $(liketips).fadeOut('fast');
//});