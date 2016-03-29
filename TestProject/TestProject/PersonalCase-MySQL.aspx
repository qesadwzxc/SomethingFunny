<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-MySQL.aspx.cs" Inherits="TestProject.PersonalCase_MySQL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lblResult"></asp:Label>
        <asp:Textbox runat="server" ID="txtLalala"></asp:Textbox>
        <asp:Button runat="server" ID="btnClick" OnClick="btnClick_Click2" Text="Run"/>
    </div>
    </form>
</body>
</html>
