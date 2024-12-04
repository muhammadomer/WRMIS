using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
    public partial class InfrastructureParent : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string ParentInfrastructureID = Utility.GetStringValueFromQueryString("InfrastructureID", "");
                    hdnProtectioninfrastructure.Value = ParentInfrastructureID;
                    InfrastructureDetail.InfrastructureID = Convert.ToInt64(ParentInfrastructureID);
                    hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID={0}", ParentInfrastructureID);
                    BindParentTypeDropDown();
                    CheckParentName(Convert.ToInt64(ParentInfrastructureID));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelParentFeeder);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region Bind DropDown

        private void BindParentTypeDropDown()
        {
            try
            {
                Dropdownlist.DDLParentTypes(ddlParentType);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindParentNameDropdown()
        {
            if (ddlParentType.SelectedItem.Value != "")
            {
                if (Convert.ToInt64(ddlParentType.SelectedItem.Value) == (long)Constants.ParentType.Barrage || Convert.ToInt64(ddlParentType.SelectedItem.Value) == (long)Constants.ParentType.Headwork)   //When selected item is "Barrage/Headwork"
                {
                    Dropdownlist.DDLBarrages(ddlParentName);
                }
                else if (Convert.ToInt64(ddlParentType.SelectedItem.Value) == (long)Constants.ParentType.Channel)   //When selected item is "Channels"
                {
                    Dropdownlist.DDLParentChannels(ddlParentName);
                }
                else if (Convert.ToInt64(ddlParentType.SelectedItem.Value) == (long)Constants.ParentType.River)   //When selected item is "River"
                {
                    Dropdownlist.DDLRiver(ddlParentName);
                }
            }
        }

        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID);
        }

        #endregion Bind DropDown

        #region Dropdown Events

        protected void ddlParentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(ddlParentType.SelectedItem.Value == string.Empty) && ddlParentType.SelectedItem.Text != "River")
            {
                ddlZone.Enabled = true;
                BindZoneDropdown();
                ddlZone.CssClass = "form-control required";
                ddlZone.Attributes.Add("required", "required");
            }
            else
            {
                ddlZone.Enabled = false;
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select);
                ddlCircle.Enabled = false;
                Dropdownlist.DDLCircles(ddlCircle, false, (int)Constants.DropDownFirstOption.Select);
                ddlDivision.Enabled = false;
                Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.Select);
            }
            BindParentNameDropdown();
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(ddlZone.SelectedItem.Value == string.Empty))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                BindCircleDropdown(ZoneID);
                ddlCircle.CssClass = "form-control required";
                ddlCircle.Attributes.Add("required", "required");
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(ddlCircle.SelectedItem.Value == string.Empty))
            {
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = true;
                long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                BindDivisionDropdown(CircleID);

                ddlDivision.CssClass = "form-control required";
                ddlDivision.Attributes.Add("required", "required");
            }
        }

        #endregion Dropdown Events

        private void CheckParentName(long _InfrastructureID)
        {
            bool isParent = new InfrastructureBLL().IsInfrastructureParent(_InfrastructureID);
            if (isParent)
            {
                Tuple<string, string> tupleRD = null;
                object lstinfrastructureParent = new InfrastructureBLL().GetInfrastructureParentNameByID(_InfrastructureID);
                hdnParentid.Value = Convert.ToString(lstinfrastructureParent.GetType().GetProperty("ID").GetValue(lstinfrastructureParent));
                var ParentType = lstinfrastructureParent.GetType().GetProperty("ParentType").GetValue(lstinfrastructureParent);
                string ParentName = Convert.ToString(lstinfrastructureParent.GetType().GetProperty("ParentName").GetValue(lstinfrastructureParent));
                long ParentID = Convert.ToInt64(lstinfrastructureParent.GetType().GetProperty("ParentID").GetValue(lstinfrastructureParent));
                string Zone = Convert.ToString(lstinfrastructureParent.GetType().GetProperty("ZoneName").GetValue(lstinfrastructureParent));
                var ZoneID = lstinfrastructureParent.GetType().GetProperty("ZoneID").GetValue(lstinfrastructureParent);
                var Circle = lstinfrastructureParent.GetType().GetProperty("CircleName").GetValue(lstinfrastructureParent);
                var CircleID = lstinfrastructureParent.GetType().GetProperty("CircleID").GetValue(lstinfrastructureParent);
                var Division = lstinfrastructureParent.GetType().GetProperty("DivisionName").GetValue(lstinfrastructureParent);
                var DivisionID = lstinfrastructureParent.GetType().GetProperty("DivisionID").GetValue(lstinfrastructureParent);
                var OffTakingRD = lstinfrastructureParent.GetType().GetProperty("OffTakingRD").GetValue(lstinfrastructureParent);
                if (ParentID == (long)Constants.ParentType.Barrage || ParentID == (long)Constants.ParentType.Barrage)
                {
                    ddlZone.SelectedIndex = 0;
                    ddlCircle.SelectedIndex = 0;
                    ddlDivision.SelectedIndex = 0;
                    Dropdownlist.DDLBarrages(ddlParentName);
                    ddlParentName.Items.FindByText(ParentName).Selected = true;
                }
                else if (ParentID == (long)Constants.ParentType.Channel)
                {
                    Dropdownlist.DDLParentChannels(ddlParentName);
                    ddlParentName.Items.FindByText(ParentName).Selected = true;
                }
                else if (ParentID == (long)Constants.ParentType.River)
                {
                    Dropdownlist.DDLRiver(ddlParentName);
                    ddlParentName.Items.FindByText(ParentName).Selected = true;
                }
                ddlZone.Enabled = true;
                ddlCircle.Enabled = true;
                ddlDivision.Enabled = true;
                BindZoneDropdown();
                ddlZone.SelectedItem.Text = Convert.ToString(Zone);
                BindCircleDropdown(Convert.ToInt64(ZoneID));
                ddlCircle.SelectedItem.Text = Convert.ToString(Circle);
                BindDivisionDropdown(Convert.ToInt64(CircleID));
                ddlDivision.SelectedItem.Text = Convert.ToString(Division);
                ddlParentType.Items.FindByValue(Convert.ToString(ParentID)).Selected = true;
                tupleRD = Calculations.GetRDValues(Convert.ToInt64(OffTakingRD));
                txtTotalOfftakingLeftRD.Text = tupleRD.Item1;
                txtTotalOfftakingRightRD.Text = tupleRD.Item2;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ParentChannel = null;
                long ParentChannelID = -1;
                FO_InfrastructureParent infrastructureParent = new FO_InfrastructureParent();
                InfrastructureBLL bllInfrastructure = new InfrastructureBLL();
                bool isParent = bllInfrastructure.IsInfrastructureParent(Convert.ToInt64(hdnProtectioninfrastructure.Value));
                infrastructureParent.ID = Convert.ToInt64(hdnParentid.Value);
                infrastructureParent.ProtectionInfrastructureID = Convert.ToInt64(hdnProtectioninfrastructure.Value);
                infrastructureParent.StructureTypeID = Convert.ToInt64(ddlParentType.SelectedItem.Value);

                ParentChannel = ddlParentName.SelectedItem.Value.Split(';');
                ParentChannelID = Convert.ToInt64(ParentChannel[0]);

                //infrastructureParent.StructureID = ddlParentName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlParentName.SelectedItem.Value);
                infrastructureParent.StructureID = ddlParentName.SelectedItem.Value == string.Empty ? -1 : ParentChannelID;
                infrastructureParent.DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                infrastructureParent.OfftakeRD = Calculations.CalculateTotalRDs(txtTotalOfftakingLeftRD.Text, txtTotalOfftakingRightRD.Text); ;
                infrastructureParent.CreatedDate = DateTime.Now;
                infrastructureParent.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                infrastructureParent.ModifiedDate = DateTime.Now;
                infrastructureParent.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                if (!isParent)
                {
                    bool isSaved = bllInfrastructure.SaveInfrastructureParent(infrastructureParent);
                    if (isSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID=" + infrastructureParent.ProtectionInfrastructureID, false);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    bool IsUpdated = bllInfrastructure.UpdateInfrastructureParent(infrastructureParent);
                    if (IsUpdated)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID=" + infrastructureParent.ProtectionInfrastructureID, false);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }
    }
}