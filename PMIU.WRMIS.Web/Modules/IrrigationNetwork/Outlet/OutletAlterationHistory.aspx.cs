using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using System.Globalization;
namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet
{
    public partial class OutletAlterationHistory : BasePage
    {
        public static string ActionType;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();

                    string outletID = Request.QueryString["OutletID"];
                    string channelID = Request.QueryString["ChannelID"];
                    //ActionType = Request.QueryString["ActionType"];
                    //string alterationID = Request.QueryString["AlterationID"];

                    if (!string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(outletID))// && !string.IsNullOrEmpty(alterationID))
                    {
                        hdnChannelID.Value = Convert.ToString(channelID);
                        hdnOutletID.Value = Convert.ToString(outletID);
                        //hdnAlternateID.Value = Convert.ToString(alterationID);
                        ChannelOutletAlterationDetails.ChannelID = Convert.ToInt64(channelID);
                        ChannelOutletAlterationDetails.OutletID = Convert.ToInt64(outletID);

                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value);
                    }
                    txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                    txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OutletAlterationHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        protected void gvAlterationHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAlterationHistory.PageIndex = e.NewPageIndex;
                gvAlterationHistory.EditIndex = -1;
                BindOuletAlterationHistoryGrid();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindOuletAlterationHistoryGrid()
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                List<object> lstOuletAlterationHistory = new IN_OutletBLL().GetOutletAlterationHistory(Convert.ToInt64(hdnOutletID.Value), fromDate, toDate);
                gvAlterationHistory.DataSource = lstOuletAlterationHistory;
                gvAlterationHistory.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnShowHistory_Click(object sender, EventArgs e)
        {
            try
            {
                BindOuletAlterationHistoryGrid();
                content.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAlterationHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string AttachmentPath = Convert.ToString(gvAlterationHistory.DataKeys[e.Row.RowIndex].Value);
                    List<string> lstName = new List<string>();
                    lstName.Add(AttachmentPath);
                    WebFormsTest.FileUploadControl FileUploadControl = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl");
                    FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    FileUploadControl.Size = 1;
                    FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.IrrigationNetwork, lstName);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}