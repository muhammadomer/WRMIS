using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddOutletAlterationInspection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        btnSave.Visible = base.CanAdd;
                        hdnScheduleDetailChannelID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        GetOutletInspection();
                        hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + hdnScheduleID.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddOutletAlterationInspectionNotes);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void GetOutletInspection()
        {
            try
            {
                dynamic bllOutletInspection = new ScheduleInspectionBLL().GetOutletInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value), 4);

                OutletAlterationInspection.ScheduleTitle = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleTitle");
                OutletAlterationInspection.ScheduleStatus = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleStatus");
                OutletAlterationInspection.PreparedBy = Utility.GetDynamicPropertyValue(bllOutletInspection, "PreparedBy");
                OutletAlterationInspection.FromDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "FromDate");
                OutletAlterationInspection.ToDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "ToDate");
                OutletAlterationInspection.ChannelName = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelName");
                OutletAlterationInspection.OutletName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletRDChannelSide");
                OutletAlterationInspection.OutletTypeName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeName");
                hdnChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelID");
                hdnOutletID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletID");
                hdnOutletTypeID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeID");
                hdnScheduleID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleID");
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void Check_InspectionDates(object sender, EventArgs e)
        {
            try
            {
                if (txtInspectionDate.Text != "")
                {
                    bool IsInspectionDateInValid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtInspectionDate.Text));
                    if (IsInspectionDateInValid)
                    {
                        Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtInspectionDate.Text = Now;
                    }
                }

            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtInspectionDate.Text != "")
                {
                    bool IsInspectionDateInvalid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtInspectionDate.Text));
                    if (IsInspectionDateInvalid)
                    {
                        Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                long? outletID = null;
                long? OutletTypeID = null;
                if (!string.IsNullOrEmpty(hdnOutletID.Value))
                    outletID = Convert.ToInt64(hdnOutletID.Value);
                if (!string.IsNullOrEmpty(hdnOutletTypeID.Value))
                    OutletTypeID = Convert.ToInt64(hdnOutletTypeID.Value);

                SI_OutletAlterationHistroy inspectionNotes = new SI_OutletAlterationHistroy();
                inspectionNotes.AlterationDate = Utility.GetParsedDate(txtInspectionDate.Text).Add(DateTime.Now.TimeOfDay);
                inspectionNotes.OutletID = outletID;
                inspectionNotes.OutletTypeID = OutletTypeID;
                inspectionNotes.OutletHeight = Convert.ToDouble(txtOutletHeight.Text);
                inspectionNotes.OutletCrest = Convert.ToDouble(txtOutletCrestHead.Text);
                inspectionNotes.OutletWidth = Convert.ToDouble(txtDBWidth.Text);
                inspectionNotes.OutletWorkingHead = Convert.ToDouble(txtWorkingHead.Text);
                inspectionNotes.UserID = SessionManagerFacade.UserInformation.ID;
                inspectionNotes.CreatedBy = Convert.ToInt64(SessionManagerFacade.UserInformation.ID);
                inspectionNotes.Discharge = Convert.ToDouble(txtDischarge.Text);
                inspectionNotes.Remarks = txtComments.Text;
                inspectionNotes.ScheduleDetailChannelID = Convert.ToInt64(hdnScheduleDetailChannelID.Value);
                inspectionNotes.Source = Configuration.RequestSource.RequestFromWeb;
                inspectionNotes.CreatedDate = DateTime.Now;

                bool isSaved = new ScheduleInspectionBLL().AddOutletAlterationInspectionNotes(inspectionNotes);
                if (isSaved)
                {
                    ScheduleInspectionNotes.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + hdnScheduleID.Value, false);
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    return;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}