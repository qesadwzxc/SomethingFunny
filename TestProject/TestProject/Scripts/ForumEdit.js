//实例化编辑器
var ue = UE.getEditor('editor');

function onClose() {
    window.location.href = "/TCLife/ForumPosts.aspx?newsid=" + document.getElementById("newsidHidden").value;
}

window.onload = function() {
    onPageLoad();
}

//初始化编辑器内容
function onPageLoad() {
    var request = new tcOA.request();
    request.method("OnLoad");

    request.callBack(function(text) {
        var obj = request.toJson(text);

        if (obj && obj.responseHeader) {
            if (obj.responseHeader.result == 'ok') {
                var column = obj.responseHeader.columnIDTypejsonString;
                var titleType = obj.responseHeader.titleTypejsonString;
                var newsTitle = obj.responseHeader.newsTitleString;
                var columnID = obj.responseHeader.columnIDString;
                var newsType = obj.responseHeader.newsTypeString;
                var newsContent = obj.responseHeader.newsContentString;
                var newsId = obj.responseHeader.newsId;

                document.getElementById("newsidHidden").value = newsId;

                if (column != null && column != "undefined") {
                    var selColumnIDType = document.getElementById("selColumnIDType");
                    onCreateElement(column, selColumnIDType, columnID);
                }
                if (titleType != null && titleType != "undefined") {
                    var selTitleType = document.getElementById("selTitleType");
                    onCreateElement(titleType, selTitleType, newsType);
                }

                document.getElementById("txtTitle").value = newsTitle;
                setNewsContent(newsContent);


            } else {
                if (obj.responseHeader.errorMessage) {
                    touch.alert(obj.responseHeader.errorMessage);
                    return;
                }
            }
        }

    });
    request.post("ForumEdit.aspx");
}

function setNewsContent(content) {
    if (navigator.userAgent.indexOf('Firefox') >= 0) {
        UE.getEditor('editor').setContent(content);
    } else {
        UE.getEditor('editor').addListener("ready", function() { //准备好之后才可以使用
            UE.getEditor('editor').setContent(content);
        });
    }
}

//创建select
function onCreateElement(jsonstrintg, selectid, selectedValue) {
    eval("jsonstr = " + jsonstrintg);

    for (i = selectid.length; i >= 0; i--) {
        selectid.remove(i);
    }

    selectid.options.add(new Option("---请选择---", "0"));
    for (i = 0; i < jsonstr.length; i++) {
        var newOption = document.createElement("OPTION");
        if (jsonstr[i].Key == selectedValue) {
            newOption.selected = true;
        }
        newOption.text = jsonstr[i].Value;
        newOption.value = jsonstr[i].Key;
        selectid.options.add(newOption);
    }
}

//提交更新
function onSubmitUpdateEdit() {
    $("#Save").attr("disabled", "disabled");
    var selTitleType = document.getElementById("selTitleType").value;
    var txtTitle = document.getElementById("txtTitle").value;
    var selColumnIDType = document.getElementById("selColumnIDType").value;

    if (selTitleType == 0) {
        touch.alert("对不起，请选择标题类型！");
        $("#Save").removeAttr("disabled");
        return;
    }
    if (tcOA.isNullOrEmpty(txtTitle)) {
        touch.alert("对不起，标题不能为空！", function() { $("#Save").removeAttr("disabled"); });
        return;
    }
    if (selColumnIDType == 0) {
        touch.alert("对不起，请选择板块类型！", function() { $("#Save").removeAttr("disabled"); });
        return;
    }
    if (txtTitle.length > 100) {
        touch.alert("对不起，标题不能超过100个字符！", function() { $("#Save").removeAttr("disabled"); });
        return;
    }
    if (!UE.getEditor('editor').hasContents()) {
        touch.alert("对不起，请填写回帖内容！", function() { $("#Save").removeAttr("disabled"); });
        return;
    }
    var content = UE.getEditor('editor').getContent();

    var request = new tcOA.request();
    request.method("OnSubmitUpdateEdit");
    request.param("newsContent", content);
    request.param("newsType", selTitleType);
    request.param("newsTitle", txtTitle);
    request.param("columnID", selColumnIDType);

    request.callBack(function(text) {
        var obj = request.toJson(text);

        if (obj && obj.responseHeader) {
            if (obj.responseHeader.result == 'ok') {
                var newsid = obj.responseHeader.newsid;
                touch.alert(obj.responseHeader.message, function() {
                    window.location.href = "/TCLife/ForumPosts.aspx?newsid=" + newsid;
                });
            } else {
                if (obj.responseHeader.errorMessage) {
                    touch.alert(obj.responseHeader.errorMessage, function() { $("#Save").removeAttr("disabled"); });
                }
            }
        }

    });
    request.post("ForumEdit.aspx");
}