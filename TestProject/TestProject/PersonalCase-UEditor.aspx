<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-UEditor.aspx.cs" Inherits="TestProject.WebFormTest12" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="Stylesheet" type="text/css" href="Styles/default.css" />
    <link type="text/css" href="Styles/ForumPosts.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form runat="server">    
        <div>
         <script id="editor" type="text/plain" style="width:100%; min-height: 240px; z-index: 999;"></script>
         <asp:HiddenField runat="server" ID="content"/>
        </div>
    </form>
        
    <script type="text/javascript" src="http://libs.baidu.com/jquery/2.1.4/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="ueditor/ueditor.all.js"> </script>
    <script type="text/javascript" charset="utf-8" src="ueditor/lang/zh-cn/zh-cn.js"></script>
    <script type="text/javascript">
        var ue = UE.getEditor('editor');
        ue.ready(function () {
            var conment = this.document.getElementById("content");
            ue.ready(ue.setContent(document.getElementById("content").value.toString()));
        })
        function bind2() {
            document.getElementById('content').value = 'ss';
        }
        window.onload = bind2();
        </script>
</body>
</html>
