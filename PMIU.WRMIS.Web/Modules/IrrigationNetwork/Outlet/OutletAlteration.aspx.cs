using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
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
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using System.Globalization;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet
{
    public partial class OutletAlteration : BasePage
    {
        CO_OutletAlterationHistroy OutletRefData = new CO_OutletAlterationHistroy();
        IN_OutletBLL OutletBLL = new IN_OutletBLL();
        public static int? _OutletRD;
        public static string _ChannelSide;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();

                    string outletID = Request.QueryString["OutletID"];
                    string channelID = Request.QueryString["ChannelID"];
                    string alterationID = Request.QueryString["AlterationID"];

                    if (!string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(outletID) && !string.IsNullOrEmpty(alterationID))
                    {
                        //if (Convert.ToInt64(outletID) == 0 && Convert.ToInt64(alterationID) == 0)
                        //    txtAlterationDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

                        hdnChannelID.Value = Convert.ToString(channelID);
                        hdnOutletID.Value = Convert.ToString(outletID);
                        hdnAlternateID.Value = Convert.ToString(alterationID);

                        LoadOutletAlterationData(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value), Convert.ToInt64(hdnAlternateID.Value));

                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value);

                        if (Convert.ToInt64(alterationID) == 0 && base.CanAdd == false)
                            btnSave.Visible = base.CanAdd;
                        else if (Convert.ToInt64(alterationID) > 0 && base.CanEdit == false)
                            btnSave.Visible = base.CanEdit;

                        //txtAlterationDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    }
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }

            }

        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AlterationOutlet);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadOutletAlterationData(long _ChannelID, long _OutletID, long _OutletAlterationID)
        {
            try
            {
                ChannelOutletAlterationDetails.ChannelID = _ChannelID;
                ChannelOutletAlterationDetails.OutletID = _OutletID;

                Dropdownlist.DDLStatus(ddlOutletStatus);

                List<CO_OutletType> LstOutletTypes = new IN_OutletBLL().GetOutletTypes();
                Dropdownlist.BindDropdownlist<List<CO_OutletType>>(ddlOutletType, LstOutletTypes, (int)Constants.DropDownFirstOption.Select, "Name", "Description");

                DateTime latestAlterationDate = new IN_OutletBLL().GetLatestAlterationDate(_ChannelID, _OutletID, _OutletAlterationID);
                hdnAlterationDate.Value = latestAlterationDate.ToString("MM/dd/yyyy");
                //    txtAlterationDate.Text = Convert.ToString(Utility.GetFormattedDate(latestAlterationDate));

                BindOutletData(_OutletID, _OutletAlterationID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindOutletData(long _OutletID, long _AlterationID)
        {
            try
            {
                // Outlet Data from Channel-Outlet Table
                OutletRefData = new IN_OutletBLL().GetOutletRefData(_AlterationID);

                // txtAlterationDate.Text = OutletRefData.AlterationDate.Value.ToString("MMM dd, yyyy");
                txtGrossCommandArea.Text = Convert.ToString(OutletRefData.OutletGCA);
                txtCultureableCommandArea.Text = Convert.ToString(OutletRefData.OutletCCA);

                txtDesignDischarge.Text = Convert.ToString(OutletRefData.DesignDischarge);

                Dropdownlist.SetSelectedValue(ddlOutletStatus, Convert.ToString(OutletRefData.OutletStatus));

                string OutletDesc = OutletBLL.GetOLTypeAbbrvByID(OutletRefData.OutletTypeID);
                Dropdownlist.SetSelectedValue(ddlOutletType, OutletDesc);

                if (OutletRefData.OutletHeight != null)
                    txtOutletHeight.Text = Convert.ToString(OutletRefData.OutletHeight.Value);
                if (OutletRefData.OutletCrest != null)
                    txtOutletCrestHead.Text = Convert.ToString(OutletRefData.OutletCrest.Value);
                if (OutletRefData.OutletSubmergence != null)
                    txtSubmergence.Text = Convert.ToString(OutletRefData.OutletSubmergence.Value);
                if (OutletRefData.OutletWidth != null)
                    txtDBWidth.Text = Convert.ToString(OutletRefData.OutletWidth.Value);
                if (OutletRefData.OutletCrestRL != null)
                    txtCrestRL.Text = Convert.ToString(OutletRefData.OutletCrestRL.Value);
                if (OutletRefData.OutletMMH != null)
                    txtMMHead.Text = Convert.ToString(OutletRefData.OutletMMH.Value);
                if (OutletRefData.OutletWorkingHead != null)
                    txtWorkingHead.Text = Convert.ToString(OutletRefData.OutletWorkingHead.Value);

                if (!string.IsNullOrEmpty(OutletRefData.OutletAttachment))
                    PreviewImage(OutletRefData.OutletAttachment);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveOutlet();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void SaveOutlet()
        {
            try
            {
                CO_OutletAlterationHistroy _GetOutletHistory = PrepareOutletHistoryEntity(Convert.ToInt64(hdnOutletID.Value));
                _GetOutletHistory.OutletRD = _OutletRD;
                _GetOutletHistory.ChannelSide = _ChannelSide;
                _GetOutletHistory.ActionType = "A";

                bool isSaveHistory = new IN_OutletBLL().SaveOutletHistory(_GetOutletHistory);

                if (isSaveHistory)
                {
                    OutletView.IsSaved = true;
                    // in case of successful save date- redirect to outletview screen
                    Response.Redirect("~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private CO_OutletAlterationHistroy PrepareOutletHistoryEntity(long _OutletID)
        {
            CO_OutletAlterationHistroy outletHistory = new CO_OutletAlterationHistroy();

            DateTime alterationDate = Utility.GetParsedDate(txtAlterationDate.Text.Trim());
            alterationDate = alterationDate.Add(DateTime.Now.TimeOfDay);
            outletHistory.AlterationDate = alterationDate;
            outletHistory.OutletID = _OutletID;
            outletHistory.DesignDischarge = Convert.ToDouble(txtDesignDischarge.Text);
            outletHistory.OutletStatus = Convert.ToString(ddlOutletStatus.SelectedItem.Value);

            string FileName = string.Empty;
            List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.IrrigationNetwork);
            foreach (var item in lstNameofFiles)
            {
                FileName = item.Item3.ToString();
            }

            outletHistory.OutletAttachment = FileName;

            long GetOutletTypeID = OutletBLL.GetOutletTypeID(ddlOutletType.SelectedItem.Value);
            if (GetOutletTypeID != 0)
            {
                outletHistory.OutletTypeID = GetOutletTypeID;
            }

            if (!string.IsNullOrEmpty(txtGrossCommandArea.Text))
            {
                outletHistory.OutletGCA = Convert.ToDouble(txtGrossCommandArea.Text);
            }
            if (!string.IsNullOrEmpty(txtCultureableCommandArea.Text))
            {
                outletHistory.OutletCCA = Convert.ToDouble(txtCultureableCommandArea.Text);
            }

            // Design Parameters values
            if (!string.IsNullOrEmpty(txtOutletHeight.Text))
            {
                outletHistory.OutletHeight = Convert.ToDouble(txtOutletHeight.Text);
            }

            if (!string.IsNullOrEmpty(txtWorkingHead.Text))
            {
                outletHistory.OutletWorkingHead = Convert.ToDouble(txtWorkingHead.Text);
            }

            if (!string.IsNullOrEmpty(txtOutletHeight.Text) && !string.IsNullOrEmpty(txtOutletCrestHead.Text))
            {
                outletHistory.OutletSubmergence = Convert.ToDouble(txtOutletCrestHead.Text) - Convert.ToDouble(txtOutletHeight.Text);
            }
            if (!string.IsNullOrEmpty(txtOutletCrestHead.Text))
            {
                outletHistory.OutletCrest = Convert.ToDouble(txtOutletCrestHead.Text);
            }

            if (!string.IsNullOrEmpty(txtDBWidth.Text))
            {
                outletHistory.OutletWidth = Convert.ToDouble(txtDBWidth.Text);
            }

            if (!string.IsNullOrEmpty(txtCrestRL.Text))
            {
                outletHistory.OutletCrestRL = Convert.ToDouble(txtCrestRL.Text);
            }

            if (!string.IsNullOrEmpty(txtMMHead.Text))
            {
                outletHistory.OutletMMH = Convert.ToDouble(txtMMHead.Text);
            }

            return outletHistory;
        }
        private void PreviewImage(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            //lnkFile.Text = "File: " + filename;
            //lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = 1;
            FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.IrrigationNetwork, lstName);

        }
    }
}