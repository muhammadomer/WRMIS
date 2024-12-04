using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.WaterLosses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.Web.Modules.WaterLosses
{
    public partial class ReachLagTime : BasePage
    {
        List<object> lstData = new List<object>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            lstData = new WaterLossesBLL().GetReachLagTime();
            gvLagTime.DataSource = lstData;
            gvLagTime.DataBind();
        }


        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.LagTime);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvLagTime_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvLagTime.EditIndex = e.NewEditIndex;
                BindGrid();
                gvLagTime.Rows[e.NewEditIndex].FindControl("txtNovApr").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvLagTime_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvLagTime.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvLagTime_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvLagTime.Rows[e.RowIndex];
                long userID = (long)Session[SessionValues.UserID];
                long id = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                int novApr = Convert.ToInt32(((TextBox)row.FindControl("txtNovApr")).Text);
                int mayJun = Convert.ToInt32(((TextBox)row.FindControl("txtMayJun")).Text);
                int julAug = Convert.ToInt32(((TextBox)row.FindControl("txtJulAug")).Text);
                int sepOct = Convert.ToInt32(((TextBox)row.FindControl("txtSepOct")).Text); 

                bool result = new WaterLossesBLL().UpdateLagTime(userID, id, novApr, mayJun, julAug, sepOct);
                if (result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    gvLagTime.EditIndex = -1;
                    BindGrid();
                }
                else
                {
                    Master.ShowMessage(Message.GeneralError.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvLagTime_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("History"))
                {
                    GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    long id = Convert.ToInt64(((Label)gvrow.FindControl("lblID")).Text);
                    List<object> lstUpdateHistory = new WaterLossesBLL().GetReachLagTimeUpdateHistoy(id);
                    gvHistory.DataSource = lstUpdateHistory;
                    gvHistory.DataBind();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#history').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "lagTimeUpdateHistoryScript", sb.ToString(), false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}