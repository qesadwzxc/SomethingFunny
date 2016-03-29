<%--首先定义一个Button按钮，在后台文件中将click事件绑定在前台的js方法上，通过js方法的返回值判断是否执行button1_Click事件。--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-ButtonANDJavascript.aspx.cs" Inherits="TestProject.WebFormTest11" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="button1" runat="server" Text="提交"  onclick="button1_Click"/>

        <script type="text/javascript">
            function jsFunction() {
                if (confirm("确定添加员工吗?")) {
                    return true;
                }
                return false;
            }
</script>
    </div>
    </form>
</body>
</html>
