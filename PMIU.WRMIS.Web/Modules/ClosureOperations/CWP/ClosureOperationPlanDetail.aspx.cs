using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
    public partial class ClosureOperationPlanDetail : BasePage
    {
        long cwpID = 0;
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["isRecordSaved"]))
                    {
                        if (Convert.ToBoolean(Request.QueryString["isRecordSaved"].ToString()))
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        else
                            Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["CWPID"]))
                    {
                        cwpID = Convert.ToInt64(Request.QueryString["CWPID"]);

                        Session["CWPDetail_PlanID"] = cwpID;
                        Session["CWPDetail_IsPublished"] = bllCO.isCWP_Published(cwpID);

                        LoadInfoControl(cwpID);
                        PublishButton(cwpID);
                    }
                    ShowUnPublishButton();
                }
                LoadClosureWorks(Convert.ToInt64(Session["CWPDetail_PlanID"]));
              
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void ShowUnPublishButton()
        {
            if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ChiefIrrigation)
            {
                if (bllCO.isCWP_Published(Convert.ToInt64(Request.QueryString["CWPID"])))
                {
                    btnUnPublish.Visible = true;
                }
            }
        }
        private void LoadClosureWorks(long _CWPID)
        {
            try
            {
                List<object> lstData = new ClosureOperationsBLL().GetClosureWorksByCWPID(_CWPID);
                gvCW.DataSource = lstData;
                gvCW.DataBind();
                if ((bool)Session["CWPDetail_IsPublished"])// Published  
                    gvCW.Columns[4].Visible = true;
                else // drafted
                    gvCW.Columns[4].Visible = false;
                 
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadInfoControl(long _CWPID)
        {
            ucInfo._ID = _CWPID;
            ucInfo.isCWP = true;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ClosureWorkPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvCW_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if ((bool)Session["CWPDetail_IsPublished"])// Published 
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    ((HyperLink)e.Row.FindControl("hlAddCW")).Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((HyperLink)e.Row.FindControl("hlEditCW")).Visible = false;
                    ((Button)e.Row.FindControl("btnDelete")).Visible = false;

                    ((HyperLink)e.Row.FindControl("hlCWDetail")).Visible = true;
                    ((HyperLink)e.Row.FindControl("hlAddPrgrs")).Visible = true;
                    ((HyperLink)e.Row.FindControl("hlPrgrsHstry")).Visible = true;
                }
            }
            else // Drafted
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    HyperLink add = (HyperLink)e.Row.FindControl("hlAddCW");

                    if (add != null)
                    {
                        add.Visible = true;
                        add.Enabled = (bool)mdlRoleRights.BAdd;
                        add.NavigateUrl = "~/Modules/ClosureOperations/CWP/AddClosureWork.aspx?CWPID=" + Session["CWPDetail_PlanID"];
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((HyperLink)e.Row.FindControl("hlCWDetail")).Visible = false;
                    ((HyperLink)e.Row.FindControl("hlAddPrgrs")).Visible = false;
                    ((HyperLink)e.Row.FindControl("hlPrgrsHstry")).Visible = false;

                    ((HyperLink)e.Row.FindControl("hlEditCW")).Visible = true;
                    ((Button)e.Row.FindControl("btnDelete")).Visible = true;

                    HyperLink btnEdit = (HyperLink)e.Row.FindControl("hlEditCW");
                    if (btnEdit != null)
                        btnEdit.Enabled = (bool)mdlRoleRights.BEdit;

                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    if (btnDelete != null)
                        btnDelete.Enabled = (bool)mdlRoleRights.BDelete;
                }
            }
            // Get the current ROw and c its and edit able record or not 

        }
        protected void gvCW_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblID = (Label)e.Row.FindControl("lblID");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (((bool)Session["CWPDetail_IsPublished"]))// Published 
                {
                    ((HyperLink)e.Row.FindControl("hlAddCW")).Visible = false;
                }
                else
                {
                    ((HyperLink)e.Row.FindControl("hlAddCW")).NavigateUrl
                        = "~/Modules/ClosureOperations/CWP/AddClosureWork.aspx?CWPID=" + Session["CWPDetail_PlanID"];
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                ((HyperLink)e.Row.FindControl("hlCWItems")).
                    NavigateUrl = "~/Modules/ClosureOperations/CWP/ClosureWorkItem.aspx?CWID=" + lblID.Text
                    + "&CWPID=" + Request.QueryString["CWPID"] + "&ViewMode=" + Session["CWPDetail_IsPublished"];

                if (((bool)Session["CWPDetail_IsPublished"]))// Published 
                {
                    ((HyperLink)e.Row.FindControl("hlCWDetail")).NavigateUrl =
                        "~/Modules/ClosureOperations/CWP/AddClosureWork.aspx?CWID=" + lblID.Text +
                        "&CWPID=" + Session["CWPDetail_PlanID"] +
                        "&ViewMode=true";

                    long cwID = Convert.ToInt64(((Label)e.Row.FindControl("lblID")).Text);

                    bool tenderAwarded = bllCO.IsTenderAwarded(cwID);
                    ((HyperLink)e.Row.FindControl("hlAddPrgrs")).Enabled = tenderAwarded;
                    ((HyperLink)e.Row.FindControl("hlAddPrgrs")).NavigateUrl
                        = "~/Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + lblID.Text + "&CWPID=" + Request.QueryString["CWPID"];

                    ((HyperLink)e.Row.FindControl("hlPrgrsHstry")).Enabled = tenderAwarded;
                    ((HyperLink)e.Row.FindControl("hlPrgrsHstry")).NavigateUrl
                        = "~/Modules/ClosureOperations/CWP/WorkProgessHistory.aspx?CWID="
                        + lblID.Text + "&CWPID=" + Request.QueryString["CWPID"];


                    //Show contractor amount 
                    string amount = Utility.GetRoundOffValue(bllCO.ContractorAmount(cwID));
                    ((Label)e.Row.FindControl("lblAmnt")).Text = amount;

                }
                else // Drafted
                {
                    HyperLink btnEdit = (HyperLink)e.Row.FindControl("hlEditCW");
                    btnEdit.NavigateUrl = "~/Modules/ClosureOperations/CWP/AddClosureWork.aspx?CWPID=" + Session["CWPDetail_PlanID"] + "&CWID=" + lblID.Text;
                }
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            cwpID = Convert.ToInt64(Session["CWPDetail_PlanID"]);

            if (bllCO.IsCWPComplete(cwpID))
            {
                string status = bllCO.IsClosureWorkPlanCostEstimationCorrect(cwpID);
                if (!string.IsNullOrEmpty(status))
                {
                    Master.ShowMessage("Plan Cannot be published.Estimation cost of Work and its Items should be same.", SiteMaster.MessageType.Error);
                    return;
                }
                bllCO.PublishCWP(cwpID, SessionManagerFacade.UserInformation.ID);
                Session["CWPDetail_IsPublished"] = bllCO.isCWP_Published(cwpID);

                PublishButton(cwpID);
                LoadClosureWorks(cwpID);
                Master.ShowMessage(Message.CWP_Published.Description, SiteMaster.MessageType.Success);
                ShowUnPublishButton();
            }
            else
            {
                Master.ShowMessage("Plan is not complete.Cannot be published", SiteMaster.MessageType.Error);
            }



        }

        private void PublishButton(long _CWPID)
        {
            if (base.CanAdd)
            {
                btnPublish.Visible = true;
                btnPublish.Enabled = !bllCO.isCWP_Published(cwpID);
            }
            else
                btnPublish.Visible = false;
        }

        protected void gvCW_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long unit = Convert.ToInt64(((Label)gvCW.Rows[e.RowIndex].FindControl("lblID")).Text);
                CW_ClosureWork mdl = new CW_ClosureWork(); mdl.ID = unit;

                if ((bool)bllCO.ClosureWork_Operations(Constants.CHECK_ASSOCIATION, mdl))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if ((bool)bllCO.ClosureWork_Operations(Constants.CRUD_DELETE, mdl))
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    LoadClosureWorks(Convert.ToInt64(Session["CWPDetail_PlanID"]));
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                CW_ClosureWorkPlan mdlCWP = bllCO.GetCWPByID(Convert.ToInt64(Request.QueryString["CWPID"]));

                // Create Object of "ReportData"
                PMIU.WRMIS.Web.Common.ReportData mdlReportData = new ReportData();
                // Set the Report Name (planced in Constants)
                mdlReportData.Name = Constants.ClosureWorkPlanReport;

                // Set the report parameters
                ReportParameter ReportParameter = new ReportParameter("DivisionID", "" + mdlCWP.DivisionID);
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("Year", mdlCWP.Year);
                mdlReportData.Parameters.Add(ReportParameter);

                // Set the ReportData in Session with specific Key
                Session[SessionValues.ReportData] = mdlReportData;
                string ReportViwerurl = "../" + Constants.ReportsUrl;
                // Open the report printable in new tab
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnUnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                long id = Convert.ToInt64(Request.QueryString["CWPID"]);
                bool isInTender = bllCO.IsCWPInTender(id);
                if (isInTender)
                {
                    Master.ShowMessage("This plan cannot be un-published, one or more of it's associated works are in tender process.", SiteMaster.MessageType.Error);
                    return;
                }
                else
                {
                    bllCO.UnPublishCWPlan(id);
                    Master.ShowMessage("This plan has been successfully un-published.", SiteMaster.MessageType.Success);
                    btnUnPublish.Visible = false;
                    btnPublish.Enabled = true;
                    Session["CWPDetail_IsPublished"] = bllCO.isCWP_Published(id);
                    LoadClosureWorks(id);
                }
                    
            }
             catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}