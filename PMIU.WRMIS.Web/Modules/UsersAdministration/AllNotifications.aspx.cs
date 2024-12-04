using Newtonsoft.Json;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class AllNotifications : BasePage
    {
        #region Hash Table Keys

        public const string StatusKey = "Status";
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string PageIndexKey = "PageIndex";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {

                    BindDropDown();
                    DateTime CurrentDate = DateTime.Now;
                    txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-5)));
                    ddlStatus.SelectedIndex = 2;
                    BindSearchGrid();
                    SetTitle();

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Notifications);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropDown()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlStatus, CommonLists.GetAlertStatus());
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindSearchGrid()
        {
            try
            {
                UserBLL bllUser = new UserBLL();

                Hashtable SearchCriteria = new Hashtable();
                long SelectedStausID = -1;
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (ddlStatus.SelectedItem.Value != String.Empty)
                {
                    SelectedStausID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                }
                SearchCriteria.Add(StatusKey, ddlStatus.SelectedItem.Value);

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }
                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());
                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }
                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long UserId = mdlUser.ID;

                DataTable lstAllNotification = bllUser.GetAllNotificationBySearchCriteria(SelectedStausID, FromDate, ToDate, UserId);
                gvSearchResult.DataSource = lstAllNotification;
                gvSearchResult.DataBind();

                SearchCriteria.Add(PageIndexKey, gvSearchResult.PageIndex);

                Session[SessionValues.SearchBreachCriteria] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchResult.PageIndex = e.NewPageIndex;
                gvSearchResult.EditIndex = -1;
                BindSearchGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchBreachCriteria];

            string StatusID = (string)SearchCriteria[StatusKey];
            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }
            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];
            gvSearchResult.PageIndex = (int)SearchCriteria[PageIndexKey];
            BindSearchGrid();
        }

        protected void gvSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblDate = (Label)e.Row.FindControl("lblDate");
                    DateTime Date = DateTime.Parse(lblDate.Text);
                    lblDate.Text = Utility.GetFormattedDate(Date) + " " + Utility.GetFormattedTime(Date);
                    Label lbl = (Label)e.Row.FindControl("Status");
                    HyperLink lblAlertText = (HyperLink)e.Row.FindControl("lblAlertText");
                    if (lbl.Text == "1")
                    {
                        //  e.Row.BackColor = System.Drawing.Color.LightGray;
                        e.Row.Font.Bold = true;

                    }
                    else
                    {
                        lblAlertText.ForeColor = System.Drawing.Color.Black;
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnUnreadSearch_Click(object sender, EventArgs e)
        {
            try
            {
                UserBLL bllUser = new UserBLL();
                List<dynamic> lstofID = new List<dynamic>();
                string strname = string.Empty;
                Int16 _StatusID = 1;
                foreach (GridViewRow gvrow in gvSearchResult.Rows)
                {
                    Label id = (Label)(gvrow.FindControl("ID"));
                    if (id.Text == "-1")
                    {
                        continue;
                    }
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    if (chk != null & chk.Checked)
                    {
                        //lstofID += Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("ID")).Text) + ", ";
                        lstofID.Add(Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("ID")).Text));

                    }
                }
                bool Status = bllUser.UnreadAllAlertNotification(lstofID, _StatusID);

                BindSearchGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnreadSearch_Click(object sender, EventArgs e)
        {
            try
            {
                UserBLL bllUser = new UserBLL();
                List<dynamic> lstofID = new List<dynamic>();
                string strname = string.Empty;
                Int16 _StatusID = 2;
                foreach (GridViewRow gvrow in gvSearchResult.Rows)
                {
                    Label id = (Label)(gvrow.FindControl("ID"));
                    if (id.Text == "-1")
                    {
                        continue;
                    }
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    if (chk != null & chk.Checked)
                    {
                        //lstofID += Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("ID")).Text) + ", ";
                        lstofID.Add(Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("ID")).Text));

                    }
                }
                bool Status = bllUser.UnreadAllAlertNotification(lstofID, _StatusID);

                BindSearchGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        [WebMethod]
        public static void OnSubmit()
        {
            UserBLL bllUser = new UserBLL();
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long Status = bllUser.UpdateAlertCount(mdlUser.ID);
            }
            catch (Exception ex)
            {
                //new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static void ConvertToAsRead(long rowid)
        {
            bool IsSaved = false;
            UserBLL bllUser = new UserBLL();
            try
            {
                IsSaved = bllUser.ConvertToAsRead(rowid);

            }
            catch (Exception ex)
            {
                //new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            //return IsSaved;
        }


        [WebMethod]
        public static long GetAlertsCounts()
        {
            UserBLL bllUser = new UserBLL();
            long Count = 0;
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                Count = bllUser.GetAlertCount(mdlUser.ID);
            }
            catch (Exception ex)
            {
                //  new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return Count;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<dynamic> GetAlertsNotification()
        {
            UserBLL bllUser = new UserBLL();
            List<dynamic> lstOfNotication = new List<dynamic>();
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                lstOfNotication = bllUser.GetNotificationAlertList(mdlUser.ID);

                if (false)
                    lstOfNotication = new List<dynamic>();


            }
            catch (Exception ex)
            {
                //  new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstOfNotication;
        }

        protected void MarkAsRead(object sender, EventArgs e)
        {
            string filename = (sender as LinkButton).CommandArgument;
        }

    }
}