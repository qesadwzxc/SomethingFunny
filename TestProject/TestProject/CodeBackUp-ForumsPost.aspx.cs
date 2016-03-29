///////////////////////////////
///TCBBS工程ForumsPost文件备份
///////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using TCBBS.Model.TableModel;
using TCBBS.IBusiness.TableBusiness;
using TCBBS.Injector;
using TCSmartFramework.Utility;
using TCBBS.Model.ViewModel;
using TCBBS.Web.Controls;
using System.Text.RegularExpressions;
using TCBBS.Common;
using TCBBS.Model.Enum;

namespace TCBBS.Web.TCLife
{
    public partial class ForumPosts : BasePage
    {
        private long newsid = 0;
        private int pageSize = 10;
        private int pageIndex = 1;
        public int totalCount = 0;
        public DataTable dtNewsByPosts;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 传值

            if (Request["page"] != null)
            {
                int.TryParse(Request["page"], out pageIndex);
            }

            if (Request.QueryString["newsid"] == null)
            {
                return;
            }
            long.TryParse(Common.EncryptHelper.Decrypt(Request.QueryString["newsid"].ToString()), out newsid);
            #endregion

            if (!IsPostBack)
            {
                List<BBS_NewsWhereFields> wherenews = new List<BBS_NewsWhereFields>();
                wherenews.Add(new BBS_NewsWhereFields(BBS_NewsFields.NewsID, newsid));
                List<BBS_NewsModel> listnews = DependencyInjector.GetInstance<IBBS_NewsServices>().GetBBS_NewsList(wherenews);
                if (listnews != null && listnews.Count > 0)
                {
                    #region 更新阅读次数（1分钟内同一个IP只允许递增一次）
                    string clientIp = TCSmartFramework.Utility.IpHelper.GetClient() + "ForumPosts" + newsid.ToString();
                    if (CacheHelper.Get(clientIp) == null)
                    {
                        CacheHelper.Save(clientIp, DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                        int readsize = listnews[0].NewsReadSize + 1;
                        List<BBS_NewsFieldValuePair> updatenews = new List<BBS_NewsFieldValuePair>();
                        updatenews.Add(new BBS_NewsFieldValuePair(BBS_NewsFields.NewsReadSize, readsize));
                        DependencyInjector.GetInstance<IBBS_NewsServices>().Update(updatenews, wherenews);
                    }
                    #endregion
                }
                InitPage();
                login.LoginHtmlInner("同程生活");
            }
        }

        public void InitPage()
        {
            //单个帖子信息
            dtNewsByPosts = DependencyInjector.GetInstance<IBBS_NewsServices>().GetNewsByPostsInfo(newsid);

            title.InnerText = dtNewsByPosts.Rows[0]["NewsTitle"].ToString();

            totalCount = DependencyInjector.GetInstance<IBBS_NewsServices>().GetReplyByCount(newsid);
            DataTable dtreply = DependencyInjector.GetInstance<IBBS_NewsServices>().GetReplyByInfo(newsid, pageSize, pageIndex);
            if (dtreply == null || dtreply.Rows.Count <= 0)
            {
                rptReply.DataSource = null;
                rptReply.DataBind();
            }
            else
            {
                rptReply.DataSource = dtreply;
                rptReply.DataBind();
                PageControl.InitPageFoot(totalCount, pageSize, pageIndex);
            }

        }

        [AjaxMethod]
        public void OnSubmitReply()
        {
            try
            {
                //每个IP只能1分钟发送一次
                string key = TCSmartFramework.Utility.IpHelper.GetClient() + "ForumPosts" + EmployeeModel.EmployeeID + newsid;

                if (CacheHelper.Get(key) == null)
                {
                    CacheHelper.Save(key, DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                }
                else
                {
                    AjaxResponseHeader.Add("errorMessage", "亲，请不要频繁回复，系统累了，请一分钟之后再回复哦！");
                    return;
                }


                if (AjaxRequest["ReplyContent"] == null)
                {
                    AjaxResponseHeader.Add("errorMessage", "对不起，您的请求缺少参数！");
                    return;
                }

                string replyContent = ValueProcess.ReplaceCSS(AjaxRequest["ReplyContent"].ToString());

                //if (ValueProcess.FilterSqlKeyword(replyContent))
                //{
                //    AjaxResponseHeader.Add("errorMessage", "对不起，您的内容有不合法字符！");
                //    return;
                //}
                replyContent = ValueProcess.FilterKeyword(replyContent);
                if (HtmlHelper.RemoveHtml(replyContent).Length > GlobalVariable.LimtLength)
                {
                    AjaxResponseHeader.Add("errorMessage", "对不起，字数长度已超过" + GlobalVariable.LimtLength + "字！");
                    return;
                }
                else
                {
                    BBS_ReplyModel replyModel = new BBS_ReplyModel();
                    DataTable dtreply = DependencyInjector.GetInstance<IBBS_NewsServices>().GetReplyByInfo(newsid, pageSize, pageIndex);
                    //RepeaterItem rpi = rptReply.Items[0];
                    replyModel.M_MemberID = EmployeeModel.EmployeeID;
                    replyModel.N_NewsID = newsid;
                    replyModel.ReplyContent = replyContent;
                    replyModel.ReplyStates = "1";//
                    replyModel.ReplyTime = DateTime.Now;
                    replyModel.ReplyHasReaded = 1;
                    replyModel.ReplyToReplyID = 0;

                    if (DependencyInjector.GetInstance<IBBS_ReplyServices>().Add(replyModel))
                    {
                        AjaxResponseHeader.Add("result", "ok");
                        AjaxResponseHeader.Add("message", "回帖成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                Guid guid = TCErrorHelper.AddSys_Error(ex, Guid.Empty);
                TCErrorHelper.SendErrorMessage(ex, guid, "同程生活回复页面--回复 异常");
                AjaxResponseHeader.Add("errorMessage", GlobalVariable.errorMessage + guid);
            }
        }

        /// <summary>
        /// 设置一级和二级回复内容的显示方式
        /// </summary>
        /// <param name="num">一级或二级回复的判断参数</param>
        /// <returns>返回回复内容</returns>
        public string ReplyContentTransfer(int num, string content)
        {
            StringBuilder strbuild = new StringBuilder();
            strbuild.Append("<div class='overflowH word contentTable' style='padding: 0px 10px;'>");
            if (num == 0)
            {
                strbuild.Append(content);
                strbuild.Append("</div>");
                return strbuild.ToString();
            }
            else
            {
                string strMemberName = DependencyInjector.GetInstance<IBBS_MemberServices>().GetBBS_GetMemberName(num);
                strbuild.Append("回复 " + strMemberName + "：" + content);
                strbuild.Append("</div>");
                return strbuild.ToString();
            }
        }

        /// <summary>
        /// 楼层
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string FloorNumber(string number)
        {
            StringBuilder strbuild = new StringBuilder();
            strbuild.Append("<p class='fl ml20 colorfff borderradius5 ' style='padding:1px 10px; ");
            switch (number)
            {
                case "1":
                    strbuild.Append("background-color:#f60;'>" + "沙发");
                    break;
                case "2":
                    strbuild.Append("background-color:#ffa63c;'>" + "板凳");
                    break;
                case "3":
                    strbuild.Append("background-color:#64c4fe;'>" + "地板");
                    break;
                default:
                    strbuild.Append("background-color:#ccc;'>" + number + "#");
                    break;
            }
            strbuild.Append("</p>");
            return strbuild.ToString();
        }

        public string JudgeDelete(string formid)
        {
            string judge = string.Empty;
            int id = 0;
            int.TryParse(formid, out id);

            List<BBS_ReplyWhereFields> where = new List<BBS_ReplyWhereFields>();
            where.Add(new BBS_ReplyWhereFields(BBS_ReplyFields.ReplyID, id));
            where.Add(new BBS_ReplyWhereFields(BBS_ReplyFields.ReplyStates, 0, QueryCondition.NotEqual));
            List<BBS_ReplyModel> list = DependencyInjector.GetInstance<IBBS_ReplyServices>().GetBBS_ReplyList(where);

            if (list.Count > 0)
            {
                if (list[0].M_MemberID == EmployeeModel.EmployeeID)
                {
                    judge = "<a class=\"delete\" href=\"javascript:deleteformID('" + Common.EncryptHelper.Encrypt(id.ToString()) + "')\">删除</a>";
                }
            }

            return judge;
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="id"></param>
        public void DeleteForumPost(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                int.TryParse(Common.EncryptHelper.Decrypt(txtformid.Value), out id);

                BBS_ReplyModel model = DependencyInjector.GetInstance<IBBS_ReplyServices>().GetBBS_ReplyList(id);

                if (model == null)
                {
                    return;
                }

                if (model.M_MemberID != EmployeeModel.EmployeeID)
                {
                    return;
                }

                List<BBS_ReplyWhereFields> where = new List<BBS_ReplyWhereFields>();
                where.Add(new BBS_ReplyWhereFields(BBS_ReplyFields.ReplyID, id));
                List<BBS_ReplyFieldValuePair> update = new List<BBS_ReplyFieldValuePair>();
                update.Add(new BBS_ReplyFieldValuePair(BBS_ReplyFields.ReplyStates, 0));

                DependencyInjector.GetInstance<IBBS_ReplyServices>().Update(update, where);
            }
            catch (Exception ex)
            {
                Guid guid = TCErrorHelper.AddSys_Error(ex, Guid.Empty);
                TCErrorHelper.SendErrorMessage(ex, guid, "同程生活回复页面--删除回复内容 异常");
                AjaxResponseHeader.Add("errorMessage", GlobalVariable.errorMessage + guid);
            }

            InitPage();
        }

        protected void rptReply_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string strCommentPersonID = e.CommandArgument.ToString();
                if (e.CommandName == "Submit")
                {
                    string key = TCSmartFramework.Utility.IpHelper.GetClient() + "ForumPosts" + EmployeeModel.EmployeeID + newsid;
                    string strComment = ((TextBox)e.Item.FindControl("tbComment")).Text;

                    if (!string.IsNullOrEmpty(strComment))
                    {
                        BBS_ReplyModel replyModel = new BBS_ReplyModel();
                        DataTable dtreply = DependencyInjector.GetInstance<IBBS_NewsServices>().GetReplyByInfo(newsid, pageSize, pageIndex);
                        //RepeaterItem rpi = rptReply.Items[0];
                        replyModel.M_MemberID = EmployeeModel.EmployeeID;
                        replyModel.N_NewsID = newsid;
                        replyModel.ReplyContent = strComment;
                        replyModel.ReplyStates = "1";//
                        replyModel.ReplyTime = DateTime.Now;
                        replyModel.ReplyHasReaded = 1;
                        replyModel.ReplyToReplyID = Convert.ToInt32(strCommentPersonID);

                        DependencyInjector.GetInstance<IBBS_ReplyServices>().Add(replyModel);
                        Response.Redirect("~/TCLife/ForumPosts.aspx?newsid=" + Common.EncryptHelper.Encrypt(newsid.ToString()));

                    }
                    else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex + "????");
            }

        }
    }
}