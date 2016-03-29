<%--个人实例-Repeater控件综合应用
一、定义Repeater控件并用ItemTemple定义控件中内容。用Eval方法读取绑定的数据。
二、定义了每行中div的名称，并用一个链接绑定一个简单的JS实现了div的显示和隐藏。
三、后台通过ItemCommand绑定了每个div中的Button（通过CommandName）和TextBox（FindControl），通过CommandArgument进行传参。--%>

<%@ Page Language="C#" AutoEventWireup="true" 

CodeBehind="PersonalCase-Repeater.aspx.cs" Inherits="TestProject.TestWebForm" 

EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../Styles/default.css" />
    <link type="text/css" href="../Styles/ForumPosts.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
                    <asp:Repeater ID="rptReply" runat="server" onitemcommand="rptReply_ItemCommand">
                    <ItemTemplate>
                        <li>
                            <div class="replytitle">
                                <div class="mt10 mb10">
                                    <p class="fl color555"><%#Eval("MemberEmpName").ToString()%></p>
                                    <p class="fl ml20"><%#Eval("MemberDeptName").ToString()%></p>
                                    <%#Eval("rowNumber").ToString()%>
                                    <p class="fr"><%#"【回复于" + Eval("ReplyTime").ToString() + "】"%></p>
                                    <p class="clear h0">
                                    </p>
                                </div>
                            </div>
                            <div class="overflowH word contentTable" style="padding: 0px 10px;">
                                <%#Eval("ReplyContent").ToString()%></div>
                            <div class="del">
                                <%#Eval("ReplyID").ToString()%>
                                <a href="javascript:showhide('reply_<%#Eval("rowNumber").ToString()%>')">回复</a></div>
                    <div id="reply_<%#Eval("rowNumber").ToString()%>" class="reply" style="right:-95px;display:none">
                                <asp:TextBox runat="server" ID="TextBox3" Columns="20" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:Button ID="btnSubmit" runat="server" CommandName="Insert" CommandArgument='<%#Eval("MemberEmpName") %>' Text="Submit"/>
                    height:18px;width: 30px" />
                            </div>
                        </li>
                    </ItemTemplate>                  
                </asp:Repeater>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                <script type="text/javascript">
                    function showhide(id) {
                        var o = this.document.getElementById(id);
                        if (o.style.display == 'block') {
                            o.style.display = 'none';
                        }
                        else {
                            divs = this.document.getElementsByClassName("reply");
                            for (i = 0; i < divs.length; i++) {
                                divs[i].style.display = "none";
                            }
                            o.style.display = 'block';
                        }
                    }
                </script>
    </form>
</body>
</html>