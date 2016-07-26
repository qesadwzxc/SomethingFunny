/*标题栏闪烁提示*/
var titleInit = document.title, isShine = true;

setInterval(function () {
    var title = document.title;
    if (isShine == true) {
        if (/新/.test(title) == false) {
            document.title = '【你有新的订单！】';
        } else {
            document.title = '☏☏☏';
        }
    } else {
        document.title = titleInit;
    }
}, 500);

window.onfocus = function () {
    isShine = false;
};
window.onblur = function () {
    isShine = true;
};

// for IE
document.onfocusin = function () {
    isShine = false;
};
document.onfocusout = function () {
    isShine = true;
};

/*浏览器弹出窗口（页面上需要定义一个button和一个label）*/
if (window.Notification) {
    var button = document.getElementById('button'), text = document.getElementById('text');

    var popNotice = function () {
        if (Notification.permission == "granted") {
            var notification = new Notification("Hi，帅哥：", {
                body: '可以加你为好友吗？',
                icon: 'http://image.zhangxinxu.com/image/study/s/s128/mm1.jpg'
            });

            notification.onclick = function () {
                text.innerHTML = '张小姐已于' + new Date().toTimeString().split(' ')[0] + '加你为好友！';
                notification.close();
            };
        }
    };

    button.onclick = function () {
        if (Notification.permission == "granted") {
            popNotice();
        } else if (Notification.permission != "denied") {
            Notification.requestPermission(function (permission) {
                popNotice();
            });
        }
    };
} else {
    alert('浏览器不支持Notification');
}