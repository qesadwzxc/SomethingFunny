<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-Regex.aspx.cs" Inherits="TestProject.PersonalCase_Regex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label>选择要校验的方式：</label>
        <asp:DropDownList ID="ddlRegexType" runat="server"></asp:DropDownList>
        <br/>
        <label>输入要校验的内容：</label>
        <asp:Textbox runat="server" ID="txtRegex"></asp:Textbox>
        <asp:Button runat="server" ID="btnRegex" Text="确定" OnClick="btnRegex_Click" />
        <br/>
        <label>校验的结果：</label>
        <asp:Label runat="server" ID="lblRegex"></asp:Label>
    </div>
    </form>
</body>
</html>
