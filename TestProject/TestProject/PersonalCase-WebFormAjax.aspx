<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-WebFormAjax.aspx.cs" Inherits="TestProject.WebFormTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <style type="text/css">
        .blur
        {
            border: 1px solid Red;
        }
    </style>
    <table>
        <tr>
            <td style="height: 50px;">姓名：
            </td>
            <td>
                <input type="text" disabled="disabled" id="txtName"
                    style="border-color: #CCCCCC" runat="server" />
            </td>
            <td>序列：
            </td>
            <td>
                <input type="text" disabled="disabled" id="txtArray"
                    style="border-color: #CCCCCC" runat="server" />
            </td>
        </tr>
        <tr>
            <td>英文名：
            </td>
            <td>
                <input type="text" id="txtEngName" style="border-color: #CCCCCC" /><label
                    style='color: Red; padding-left: 5px;'>*</label>
            </td>
            <td>直线电话：
            </td>
            <td>
                <input type="text" id="txtTelephone" style="border-color: #CCCCCC" /><label
                    style='color: Red; padding-left: 5px;'>*</label>
            </td>
        </tr>
    </table>
    <input type="button" id="btnTest" value="Confirm" onclick="onSubmit()" />
</body>
</html>

<script src="Scripts/jquery-1.9.1.js"></script>
<script src="Scripts/AjaxFunc.js"></script>

