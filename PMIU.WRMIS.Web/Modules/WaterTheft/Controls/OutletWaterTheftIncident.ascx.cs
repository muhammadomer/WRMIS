using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public partial class OutletWaterTheftIncident : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!IsPostBack)
                //{
                //    LoadWaterTheftIncidentInformation();
                //}
                LoadWaterTheftIncidentInformation();

                if (!string.IsNullOrEmpty(Request.QueryString["ET"]))
                    SBESDOWorking.Visible = false;
                else
                    SBESDOWorking.Visible = true;
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
                object bllWaterTheftIncidentInformation = new WaterTheftBLL().GetZiladaarWorkingParentInformation(WaterTheftCase.WaterTheftID);   // WaterTheftID to be provided from search screen 

                if (bllWaterTheftIncidentInformation != null)
                {
                    lblChnlName.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "ChannelName");
                    lblOutlet.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Outlet");
                    lblType.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "OutletType");
                    lblRD.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "RD");
                    lblSide.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Side");
                    lblDate.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "IncidentDate");
                    lblTime.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "IncidentTime");
                    //lblSMSDate.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "SMSDate");
                    lblTheftType.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "TheftType");
                    lblConditonOutlet.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "ConditionOfOutlet");
                    lblH.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "H");
                    lblDefectiveType.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Defectivetype");
                    lblB.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "B");
                    lblY.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Y");
                    lblDIA.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "DIA");
                    // lblRemarks.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Remarks");
                    lblCaseNo.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "CaseNo");
                    lblCaseStatus.Text = Utility.GetDynamicPropertyValue(bllWaterTheftIncidentInformation, "Name");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
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
                WebFormsTest.FileUploadControl FileUploadControl3 = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl3");
                FileUploadControl3.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl3.Size = lstName.Count;
                FileUploadControl3.ViewUploadedFilesAsThumbnail(Configuration.WaterTheft, lstName);


            }
        }
    }
}