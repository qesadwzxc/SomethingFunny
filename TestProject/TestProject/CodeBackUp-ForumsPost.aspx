<%@ Page Title="" Language="C#" ValidateRequest="false" EnableEventValidation="false"
    MasterPageFile="~/TCBBS.Master" AutoEventWireup="true" CodeBehind="ForumPosts.aspx.cs"
    Inherits="TCBBS.Web.TCLife.ForumPosts" %>

<%@ Register Src="~/Controls/PageSplitControl.ascx" TagPrefix="uc" TagName="Page" %>
<%@ Register Src="~/Controls/LoginControl.ascx" TagPrefix="uc" TagName="login" %>
<%@ Register src="../Controls/BreadcrumbTrailControl.ascx" tagname="BreadcrumbTrailControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title id="title" runat="server"></title>
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="Stylesheet" type="text/css" href="../Styles/default.css" />
    
    <link type="text/css" href="../Styles/TCLife/ForumPosts.css" rel="stylesheet" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <uc:login ID="login" runat="server" />
<uc1:BreadcrumbTrailControl ID="BreadcrumbTrailControl1" runat="server" />

    <div class="marginauto w1200 ha">
        <div class="postInfo wP100 mb10 bgcolorf">
            <%if (dtNewsByPosts != null && dtNewsByPosts.Rows.Count > 0)
              {%>
            <div class="wP100 h50 lh50 colorfff fwb bgcolor8ab" style="margin-top:10px;">
                <em style="font-size: 16px; font-weight: 700; margin: 0px 20px;">标题：<%=dtNewsByPosts.Rows[0]["NewsTitle"].ToString()%></em>
            </div>
            <div class="postCon" style="position: relative;">
                <div class="color999 fontsize12" style="position: absolute; bottom: 20px;">
                    <div class="fl readcount">
                        <%=dtNewsByPosts.Rows[0]["NewsReadSize"].ToString()%></div>
                    <div class="fl replycount">
                        <%=dtNewsByPosts.Rows[0]["ReplyCount"].ToString()%></div>
                    <%if (dtNewsByPosts.Rows[0]["NewsPublishID"].ToString() == EmployeeModel.EmployeeID.ToString())
                      {%>
                    <div class="fl">
                        <a href="<%=TCSmartFramework.Utility.Web.WebHelper.RootURI%>TCLife/ForumEdit.aspx?newsid=<%=TCBBS.Common.EncryptHelper.Encrypt(dtNewsByPosts.Rows[0]["NewsID"].ToString())%>"
                            title="编辑内容" class="editInfo color999" style="text-decoration: none">编辑 </a>
                    </div>
                    <%} %>
                    <div class=" clear">
                    </div>
                </div>
                <div class="fl lh40 fontsize14 minH200" style="width: 15%;">
                    <p style="font-size: 16px; font-weight: 500;">
                        <%=dtNewsByPosts.Rows[0]["MemberEmpName"].ToString()%></p>
                    <p class="color999 fontsize12">
                        <%=dtNewsByPosts.Rows[0]["MemberDeptName"].ToString()%></p>
                    <p class="color999 fontsize12">
                        发表于&nbsp;<%=dtNewsByPosts.Rows[0]["NewsPublishTime"].ToString()%></p>
                    <%if (dtNewsByPosts.Rows[0]["NewsModifierTime"].ToString() != "1900.01.01 00:00")
                      {%>
                    <p class="color999 fontsize12">
                        编辑于&nbsp;<%=dtNewsByPosts.Rows[0]["NewsModifierTime"].ToString()%></p>
                    <%} %>
                </div>
                <div class="fl pl20 borderleft minH200 overflowH word contentTable"  style="width:83%;">
                    <%=dtNewsByPosts.Rows[0]["NewsContent"].ToString()%>
                </div>
                <div class="clear">
                </div>
            </div>
            <% }%>
        </div>
        <div id="pagelistId" class="wP100 mb10 bgcolorf">
            <ul class="ulinfo sendInfo">
                <%if (totalCount == 0)
                  {%><li style="">暂时还没有人回复</li>
                <%} %>
                <asp:Repeater ID="rptReply" runat="server" onitemcommand="rptReply_ItemCommand">
                    <ItemTemplate>
                        <li>
                            <div class="replytitle">
                                <div class="mt10 mb10">
                                    <p class="fl color555"><%#Eval("MemberEmpName").ToString()%></p>
                                    <p class="fl ml20"><%#Eval("MemberDeptName").ToString()%></p>
                                    <%#FloorNumber(Eval("rowNumber").ToString())%>
                                    <p class="fr"><%#"【回复于" + Eval("ReplyTime").ToString() + "】"%></p>
                                    <p class="clear h0">
                                    </p>
                                </div>
                            </div>
<%--<div class="overflowH word contentTable" style="padding: 0px 10px;"></div>  --%>
                                <%#ReplyContentTransfer(Convert.ToInt32(Eval("ReplyToReplyID")), Eval("ReplyContent").ToString())%>
                            <div class="del">
                                <%#JudgeDelete(Eval("ReplyID").ToString())%>
                                <a href="javascript:showhide('reply_<%#Eval("rowNumber").ToString()%>')">回复</a></div>
                                <div id="reply_<%#Eval("rowNumber").ToString()%>" class="reply" style="text-align:right;display:none">
                                <asp:TextBox runat="server" ID="tbComment" Columns="20" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:Button ID="btnSubmit" runat="server" CommandName="Submit" CommandArgument='<%#Eval("MemberEmpID") %>' Text="确定"/>
                        </li>
                    </ItemTemplate>                   
                </asp:Repeater>
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
            </ul>
            <div class="wP100 h40" style="display: block;">
                <div class="b_page" style="margin-right: 20px; margin-bottom: 20px">
                    <uc:Page ID="PageControl" runat="server" /> 
                </div>
            </div>
        </div>
        <div class="wP100 bgcolorf" style="padding-top: 20px;">
            <div style="padding: 0px 20px 0px 20px;">
                <script id="editor" type="text/plain" style="width:100%; min-height: 240px; z-index: 999;"></script>
            </div>
            <div class="wP100 txtcenter">
                <input id="Save" type="button" value="回复" onclick="onSubmitReply();" class="btn" style="margin: 20px 0px;
                    width: 110px" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <input type="hidden" runat="server" id="txtformid" />
    <input type="button" runat="server" id="btnCancel" onserverclick="DeleteForumPost" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="script" runat="server">
    <script src="../Jquery/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/source/tcoa.js"></script>
    <script type="text/javascript" src="../Scripts/source/tcoaMessage.js"></script>
    <script type="text/javascript" src="../Scripts/source/touchMessage.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/lang/zh-cn/zh-cn.js"></script>
    <script type="text/javascript" src="../Scripts/TCLife/ForumPosts.js"></script>
</asp:Content>
