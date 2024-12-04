using PMIU.WRMIS.BLL.IrrigationNetwork.Reach;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach
{
    public partial class LSectionParametersHistory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();

                    string ReachID = Request.QueryString["ReachID"];
                    string channelID = Request.QueryString["ChannelID"];
                    string ReachRD = Request.QueryString["ReachRD"];
                    string ReachNo = Request.QueryString["ReachNo"];
                    string L = Request.QueryString["L"];

                    if (!string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(ReachID) && !string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(ReachID) && !string.IsNullOrEmpty(L))
                    {
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ChannelID = Convert.ToInt64(channelID);
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ReachRD = Convert.ToInt64(ReachRD);
                        PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.LSectionParameters.ReachNo = Convert.ToInt64(ReachNo);

                        hdnChannelID.Value = channelID;
                        hdnReachID.Value = ReachID;
                        hdnL.Value = L;

                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Reach/DefineChannelReach.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value);
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.LSectionParametersHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindLSectionParametersHistoryGrid()
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                List<object> lstLSectionParametersHistory = new ReachBLL().GetLSectionParametersHistory(Convert.ToInt64(hdnReachID.Value), hdnL.Value, fromDate, toDate);
                gvLSectionHistory.DataSource = lstLSectionParametersHistory;
                gvLSectionHistory.DataBind();
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
                BindLSectionParametersHistoryGrid();
                content.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvLSectionHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLSectionHistory.PageIndex = e.NewPageIndex;
                gvLSectionHistory.EditIndex = -1;
                BindLSectionParametersHistoryGrid();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLSectionHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string AttachmentPath = Convert.ToString(gvLSectionHistory.DataKeys[e.Row.RowIndex].Value);
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

        //protected void gvLSectionHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow gv = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
        //        DataKey key = gvLSectionHistory.DataKeys[gv.RowIndex];
        //        if (e.CommandName.ToLower().Equals("ViewLSectionImage".ToLower()))
        //        {
        //            imgLSectioinImage.ImageUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, Convert.ToString(key[0]));
        //            System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //            sb.Append(@"<script type='text/javascript'>");
        //            sb.Append("$('#viewimage').modal('show');");
        //            sb.Append(@"</script>");
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScript", sb.ToString(), false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

    }
}