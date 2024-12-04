using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public partial class ChannelWaterTheftIncident : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!IsPostBack)
                //{                
                LoadWaterTheftIncidentInformation();

                if (!string.IsNullOrEmpty(Request.QueryString["ET"]))
                    SBESDOWorking.Visible = false;
                else
                    SBESDOWorking.Visible = true;
                // }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadWaterTheftIncidentInformation()
        {
            try
            {
                string ChannelSide = string.Empty;
                dynamic bllWaterTheftIncidentInformation = new WaterTheftBLL().GetChannelWaterTheftIncidentWorking(WaterTheftCase.WaterTheftID);
                if (bllWaterTheftIncidentInformation == null)
                    return;

                ChannelSide = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "OffenceSide");

                lblChannelName.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "ChannelName");
                lblOffencesType.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "OffenceType");
                lblCutCondition.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "CutCondition");
                lblOffenceSide.Text = Utility.GetDynamicPropertyValue(CommonLists.GetWTChannelSides(ChannelSide)[0], "Name");
                lblDateTimeOfChecking.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "DateTimeOfChecking");
                lblTheftSiteRD.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "TheftSiteRD");
                //  lblRemarks.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Remarks");
                //   lblSMSDate.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "SMSDate");
                lblCaseNo.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "CaseNo");
                lblCaseStatus.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Name");
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void lnkViewRemarks_Click(object sender, EventArgs e)
        {
            try
            {
                BindViewRemarksGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#viewRemarks').modal();", true);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void lnkViewAttachments_Click(object sender, EventArgs e)
        {
            try
            {
                BindViewAttachmentsGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Attachments", "$('#viewAttachment').modal();", true);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindViewAttachmentsGrid()
        {
            try
            {
                List<dynamic> lstWTCaseWorking = new WaterTheftBLL().GetWaterTheftCaseAttachment(WaterTheftCase.WaterTheftID);
                gvViewAttachment.DataSource = lstWTCaseWorking;
                gvViewAttachment.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindViewRemarksGrid()
        {
            try
            {
                List<dynamic> lstWTCaseWorking = new WaterTheftBLL().GetWaterTheftCaseWorkingRemarks(WaterTheftCase.WaterTheftID);
                gvViewRemarks.DataSource = lstWTCaseWorking;
                gvViewRemarks.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvViewRemarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvViewRemarks.PageIndex = e.NewPageIndex;
                BindViewRemarksGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvViewAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvViewAttachment.PageIndex = e.NewPageIndex;
                BindViewAttachmentsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string AttachmentPath = Convert.ToString(gvViewAttachment.DataKeys[e.Row.RowIndex].Value);
                List<string> lstName = new List<string>();
                lstName.Add(AttachmentPath);
                WebFormsTest.FileUploadControl FileUploadControl4 = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl4");
                FileUploadControl4.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl4.Size = lstName.Count;
                FileUploadControl4.ViewUploadedFilesAsThumbnail(Configuration.WaterTheft, lstName);


            }
        }
    }
}