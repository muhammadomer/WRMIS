using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet
{
    public partial class OutletAdd : BasePage
    {
        IN_OutletBLL OutletBLL = new IN_OutletBLL();
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
                        OutletChannelDetails.ChannelID = Convert.ToInt64(channelID);
                        hdnChannelID.Value = Convert.ToString(channelID);
                        hdnOutletID.Value = Convert.ToString(outletID);
                        hdnAlternateID.Value = Convert.ToString(alterationID);

                        LoadOutletData(Convert.ToInt64(hdnOutletID.Value), Convert.ToInt64(hdnAlternateID.Value));
                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value);
                    }

                    txtComments.Attributes.Add("maxlength", "1000");
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            string outletID = Request.QueryString["OutletID"];

            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddOutlet);
            Master.ModuleTitle = pageTitle.Item1;

            if (!string.IsNullOrEmpty(outletID) && Convert.ToInt64(outletID) == 0)
            {
                Master.NavigationBar = pageTitle.Item3;
                Master.PageTitle = pageTitle.Item2;
                pageTitleID.InnerText = pageTitle.Item2;
            }
            else
            {
                Master.NavigationBar = "Edit Outlet";
                Master.PageTitle = "Edit Outlet";
                pageTitleID.InnerText = "Edit Outlet";
            }
        }
        private void LoadOutletData(long _OutletID, long _AlterationID)
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlOutletSide, CommonLists.GetOutletSides(null));

                List<CO_OutletType> LstOutletTypes = new IN_OutletBLL().GetOutletTypes();
                Dropdownlist.BindDropdownlist<List<CO_OutletType>>(ddlOutletType, LstOutletTypes, (int)Constants.DropDownFirstOption.Select, "Name", "Description");

                if (_OutletID != 0 && _AlterationID != 0)
                {
                    AttachFile.Visible = true;
                    LoadOutletDataInEditMode(_OutletID, _AlterationID);
                    // DisableFieldsinEditMode();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadOutletDataInEditMode(long _OutletID, long _AlterationID)
        {
            // Channel Outlet Data to bind RDs and Outlet Side 
            CO_ChannelOutlets OutletData = OutletBLL.GetOutletByOutletID(_OutletID);
            Tuple<string, string> tuple = Calculations.GetRDValues(OutletData.OutletRD);
            txtOutletRDLeft.Text = tuple.Item1;
            txtOutletRDRight.Text = tuple.Item2;
            txtComments.Text = OutletData.AdditionalInformation == null ? "" : OutletData.AdditionalInformation.Trim();

            if (OutletData.ChannelSide != null)
            {
                Dropdownlist.SetSelectedValue(ddlOutletSide, Convert.ToString(OutletData.ChannelSide));
            }
            // Outlet alteration data to bind GCA, CCA, DD, outlet Type
            CO_OutletAlterationHistroy OutletHistoryData = new IN_OutletBLL().GetOutletRefData(_AlterationID);

            txtGrossCommandArea.Text = Convert.ToString(OutletHistoryData.OutletGCA);
            txtCultureableCommandArea.Text = Convert.ToString(OutletHistoryData.OutletCCA);
            txtDesignDischarge.Text = Convert.ToString(OutletHistoryData.DesignDischarge);

            string OutletDesc = OutletBLL.GetOLTypeAbbrvByID(OutletHistoryData.OutletTypeID);
            Dropdownlist.SetSelectedValue(ddlOutletType, OutletDesc);

            txtOutletHeight.Text = Convert.ToString(OutletHistoryData.OutletHeight);

            txtOutletCrestHead.Text = Convert.ToString(OutletHistoryData.OutletCrest);

            txtSubmergence.Text = Convert.ToString(OutletHistoryData.OutletSubmergence);

            txtDBWidth.Text = Convert.ToString(OutletHistoryData.OutletWidth);

            txtCrestRL.Text = Convert.ToString(OutletHistoryData.OutletCrestRL);
            txtMMHead.Text = Convert.ToString(OutletHistoryData.OutletMMH);

            txtWorkingHead.Text = Convert.ToString(OutletHistoryData.OutletWorkingHead);

            if (!string.IsNullOrEmpty(OutletHistoryData.OutletAttachment))
                PreviewImage(OutletHistoryData.OutletAttachment);

        }
        private CO_ChannelOutlets PrepareOutletEntity()
        {
            CO_ChannelOutlets outlet = new CO_ChannelOutlets();
            outlet.OutletRD = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
            outlet.Name = Convert.ToString(outlet.OutletRD) + " / " + Convert.ToString(ddlOutletSide.SelectedItem.Text);
            outlet.ChannelID = Convert.ToInt64(hdnChannelID.Value);
            outlet.ChannelSide = Convert.ToString(ddlOutletSide.SelectedItem.Value);
            outlet.AdditionalInformation = txtComments.Text.Trim();
            return outlet;
        }
        private CO_OutletAlterationHistroy PrepareOutletHistoryEntity(long _OutletID)
        {
            CO_OutletAlterationHistroy outletHistory = new CO_OutletAlterationHistroy();

            outletHistory.AlterationDate = DateTime.Now;
            outletHistory.OutletID = _OutletID;
            outletHistory.DesignDischarge = Convert.ToDouble(txtDesignDischarge.Text);
            outletHistory.OutletStatus = "1"; // By default active.




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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CO_ChannelOutlets _objOutlet = PrepareOutletEntity();
                if (OutletChannelDetails.ChannelTotalRDs < _objOutlet.OutletRD)
                {
                    Master.ShowMessage("Outlet RD can not be greater than Channel Total RDs.", SiteMaster.MessageType.Error);
                    return;
                }

                if (new IN_OutletBLL().IsChannelOutletExists(_objOutlet, Convert.ToInt64(hdnOutletID.Value)))
                {
                    Master.ShowMessage("Unique value is required.", SiteMaster.MessageType.Error);
                    return;

                }

                if (Convert.ToInt64(hdnAlternateID.Value) != 0)
                {
                    // Edit scenario-only channel outlet table will be updated
                    _objOutlet.ID = Convert.ToInt64(hdnOutletID.Value);
                    long _ID = OutletBLL.UpdateOutlet(_objOutlet);
                    //if (_objOutlet.ID != 0)
                    //{
                    //    OutletView.IsSaved = true;
                    //    Response.Redirect("~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value));
                    //}
                    #region New CR 002 Implemented
                    if (_ID != 0)
                    {
                        // Save Outlet History
                        CO_OutletAlterationHistroy _GetOutletHistory = PrepareOutletHistoryEntity(_objOutlet.ID);
                        //  _GetOutletHistory.ID = Convert.ToInt64(hdnAlternateID.Value);
                        _GetOutletHistory.OutletRD = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
                        _GetOutletHistory.ChannelSide = Convert.ToString(ddlOutletSide.SelectedItem.Value);
                        _GetOutletHistory.ActionType = "E";
                        string FileName = string.Empty;
                        List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.IrrigationNetwork);
                        foreach (var item in lstNameofFiles)
                        {
                            FileName = item.Item3.ToString();
                        }

                        _GetOutletHistory.OutletAttachment = FileName;

                        bool isSavedHistory = OutletBLL.SaveOutletHistory(_GetOutletHistory);

                        if (isSavedHistory)
                        {
                            OutletView.IsSaved = true;
                            // in case of successful save date- redirect to outletview screen
                            Response.Redirect("~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value), false);
                        }
                    }

                    #endregion New CR 002 Implemented




                }
                else if (Convert.ToInt64(hdnAlternateID.Value) == 0)
                {
                    // add scenario- channel Outlet Basic Data will be inserted
                    long _OutletID = OutletBLL.AddOutlet(_objOutlet);

                    if (_OutletID != 0)
                    {
                        // Save Outlet History
                        CO_OutletAlterationHistroy _GetOutletHistory = PrepareOutletHistoryEntity(_OutletID);
                        _GetOutletHistory.OutletRD = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
                        _GetOutletHistory.ChannelSide = Convert.ToString(ddlOutletSide.SelectedItem.Value);
                        _GetOutletHistory.ActionType = "D";
                        bool isSavedHistory = OutletBLL.SaveOutletHistory(_GetOutletHistory);

                        if (isSavedHistory)
                        {
                            OutletView.IsSaved = true;
                            // in case of successful save date- redirect to outletview screen
                            Response.Redirect("~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value), false);
                        }
                        else
                        {
                            // in case outlet history fails to save data
                            OutletBLL.DeleteOutlet(_OutletID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

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
        private void DisableFieldsinEditMode()
        {
            txtCultureableCommandArea.Enabled = false;
            txtGrossCommandArea.Enabled = false;
            txtDesignDischarge.Enabled = false;
            ddlOutletType.Enabled = false;
            txtSubmergence.Enabled = false;
            txtOutletHeight.Enabled = false;
            txtOutletCrestHead.Enabled = false;
            txtDBWidth.Enabled = false;
            txtCrestRL.Enabled = false;
            txtMMHead.Enabled = false;
            txtWorkingHead.Enabled = false;
        }
    }
}