using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement
{
    public partial class AddComplaint : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (lblValueForTab.Text != string.Empty)
                {
                    if (lblValueForTab.Text == "Village")
                    {
                        liVillage.Attributes.Add("class", "active");
                        liDivision.Attributes.Remove("class");
                        Village.Attributes.Add("class", "tab-pane active");
                        Division.Attributes.Add("class", "tab-pane");
                    }
                    else if (lblValueForTab.Text == "Division")
                    {
                        liDivision.Attributes.Add("class", "active");
                        liVillage.Attributes.Remove("class");
                        Division.Attributes.Add("class", "tab-pane active");
                        Village.Attributes.Add("class", "tab-pane");
                    }
                }
                if (!IsPostBack)
                {
                    SetTitle();
                    txtAddress.Attributes.Add("maxlength", txtAddress.MaxLength.ToString());
                    txtComplaintDetails.Attributes.Add("maxlength", txtComplaintDetails.MaxLength.ToString());
                    txtComplaintDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    Dropdownlist.DDLDomainsForAddComplaint(ddlDomain);
                    Dropdownlist.DDLComplaintSourceForAddComplaint(ddlComplaintSource);
                    Dropdownlist.GetComplaintType(ddlComplaintType);
                    BindNotificationListCheckList();
                    BackNavigationFunction();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divPanel.Visible = false;
                divWithoutTab.Visible = false;

                ddlStructure.Enabled = true;
                if (ddlDomain.SelectedItem.Value == string.Empty)
                {
                    ddlStructure.Enabled = false;
                    ddlStructure.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlStructure, string.Empty);

                    divPanel.Visible = false;
                    ddlStructure.Enabled = false;
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Development)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = false;

                    lblDivisionByDomain.Visible = true;
                    ddlDivisionByDomain.Visible = true;

                    lblStructure.Visible = false;
                    ddlStructure.Visible = false;

                    Dropdownlist.DDLDivisionsByDomainID(ddlDivisionByDomain, Convert.ToInt64(ddlDomain.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation)
                {
                    lblDivisionByDomain.Visible = false;
                    ddlDivisionByDomain.Visible = false;

                    lblStructure.Visible = true;
                    ddlStructure.Visible = true;

                    Dropdownlist.DDLStructureByDomainIrrigation(ddlStructure);
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf)
                {
                    lblDivisionByDomain.Visible = false;
                    ddlDivisionByDomain.Visible = false;

                    lblStructure.Visible = true;
                    ddlStructure.Visible = true;

                    Dropdownlist.DDLStructureByDomainDandF(ddlStructure);
                }
                else
                {
                    lblDivisionByDomain.Visible = false;
                    ddlDivisionByDomain.Visible = false;

                    lblStructure.Visible = true;
                    ddlStructure.Visible = true;

                    Dropdownlist.DDLStructuresByDomainID(ddlStructure, Convert.ToInt64(ddlDomain.SelectedItem.Value));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlStructure.SelectedItem.Value == string.Empty)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = false;
                }

                #region Reset Division Tab
                ddlDivisionTabCircle.SelectedIndex = 0;
                ddlDivisionTabCircle.Enabled = false;
                ddlDivisionTabDivision.SelectedIndex = 0;
                ddlDivisionTabDivision.Enabled = false;
                ddlDivisionTabSubDivision.SelectedIndex = 0;
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabProtectionStructure.SelectedIndex = 0;
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabChannel.SelectedIndex = 0;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabDrain.SelectedIndex = 0;
                ddlDivisionTabDrain.Enabled = false;
                ddlDivisionTabOutlet.SelectedIndex = 0;
                ddlDivisionTabOutlet.Enabled = false;
                #endregion

                #region Reset Village Tab
                ddlVillageTabTehsil.SelectedIndex = 0;
                ddlVillageTabTehsil.Enabled = false;
                ddlVillageTabVillage.SelectedIndex = 0;
                ddlVillageTabVillage.Enabled = false;
                ddlVillageTabChannel.SelectedIndex = 0;
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabProtectionStructure.SelectedIndex = 0;
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabDrain.SelectedIndex = 0;
                ddlVillageTabDrain.Enabled = false;
                ddlVillageTabOutlet.SelectedIndex = 0;
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabDivision.SelectedIndex = 0;
                ddlVillageTabDivision.Enabled = false;
                #endregion

                #region Irrigation Domain

                if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    Dropdownlist.DDLDistricts(ddlVillageTabDistrict);
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;

                    lblVillageTabChannel.Visible = true;
                    ddlVillageTabChannel.Visible = true;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    lblVillageTabOutlet.Visible = false;
                    ddlVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = true;
                    txtVillageTabTotalRDRight.Visible = true;
                    lblVillageTabTotalRDs.Visible = true;
                    lblVillageTabPlusSign.Visible = true;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                    #region DivisionTab
                    Dropdownlist.DDLZones(ddlDivisionTabZone, false, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;
                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = true;
                    ddlDivisionTabChannel.Visible = true;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = true;
                    txtDivisionTabTotalRDLeft.Visible = true;
                    txtDivisionTabTotalRDRight.Visible = true;
                    lblDivisionTabPlusSign.Visible = true;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    Dropdownlist.DDLDistricts(ddlVillageTabDistrict);
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = true;
                    ddlVillageTabChannel.Visible = true;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = true;
                    lblVillageTabOutlet.Visible = true;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                    #region DivisionTab
                    Dropdownlist.DDLZones(ddlDivisionTabZone, false, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;
                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = true;
                    ddlDivisionTabChannel.Visible = true;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = true;
                    ddlDivisionTabOutlet.Visible = true;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    Dropdownlist.DDLDistricts(ddlVillageTabDistrict);
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = true;
                    ddlVillageTabProtectionStructure.Visible = true;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = false;
                    lblVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                    #region DivisionTab
                    Dropdownlist.DDLZones(ddlDivisionTabZone, false, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = false;
                    ddlDivisionTabSubDivision.Visible = false;
                    lblDivisionTabProtectionStructure.Visible = true;
                    ddlDivisionTabProtectionStructure.Visible = true;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = true;
                    ddlBarrage.Visible = true;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
                }
                #endregion

                #region D&F Domain

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    Dropdownlist.DDLDistricts(ddlVillageTabDistrict);
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = true;
                    ddlVillageTabProtectionStructure.Visible = true;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = false;
                    lblVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                    #region DivisionTab
                    Dropdownlist.DDLZones(ddlDivisionTabZone, false, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = false;
                    ddlDivisionTabSubDivision.Visible = false;
                    lblDivisionTabProtectionStructure.Visible = true;
                    ddlDivisionTabProtectionStructure.Visible = true;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    Dropdownlist.DDLDistricts(ddlVillageTabDistrict);
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = true;
                    ddlVillageTabDrain.Visible = true;

                    lblVillageTabOutlet.Visible = false;
                    ddlVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = true;
                    txtVillageTabTotalRDRight.Visible = true;
                    lblVillageTabTotalRDs.Visible = true;
                    lblVillageTabPlusSign.Visible = true;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                    #region DivisionTab
                    Dropdownlist.DDLZones(ddlDivisionTabZone, false, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;
                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = true;
                    ddlDivisionTabDrain.Visible = true;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = true;
                    txtDivisionTabTotalRDLeft.Visible = true;
                    txtDivisionTabTotalRDRight.Visible = true;
                    lblDivisionTabPlusSign.Visible = true;
                    #endregion
                }

                #endregion

                #region Small Dams

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDam)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = true;
                    ddlSmallDams.Visible = true;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = true;
                    ddlChannel.Visible = true;

                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
                }

                #endregion

                if (lblValueForTab.Text == "Village")
                {
                    anchVillage_ServerClick(null, null);
                }
                else if (lblValueForTab.Text == "Division")
                {
                    anchDivision_ServerClick(null, null);
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Div With Tabs Pannel (Division)
        protected void ddlDivisionTabZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionTabZone.SelectedItem.Value == String.Empty)
                {
                    ddlDivisionTabCircle.SelectedIndex = 0;
                    ddlDivisionTabCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);

                    Dropdownlist.DDLCircles(ddlDivisionTabCircle, false, ZoneID);
                    ddlDivisionTabCircle.Enabled = true;
                }

                ddlDivisionTabDivision.SelectedIndex = 0;
                ddlDivisionTabDivision.Enabled = false;
                ddlDivisionTabSubDivision.SelectedIndex = 0;
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabChannel.SelectedIndex = 0;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabProtectionStructure.SelectedIndex = 0;
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabOutlet.SelectedIndex = 0;
                ddlDivisionTabOutlet.Enabled = false;
                ddlDivisionTabDrain.SelectedIndex = 0;
                ddlDivisionTabDrain.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionTabCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionTabCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivisionTabDivision.SelectedIndex = 0;
                    ddlDivisionTabDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    Dropdownlist.DDLDivisions(ddlDivisionTabDivision, false, CircleID);
                    ddlDivisionTabDivision.Enabled = true;
                }
                ddlDivisionTabSubDivision.SelectedIndex = 0;
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabChannel.SelectedIndex = 0;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabProtectionStructure.SelectedIndex = 0;
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabOutlet.SelectedIndex = 0;
                ddlDivisionTabOutlet.Enabled = false;
                ddlDivisionTabDrain.SelectedIndex = 0;
                ddlDivisionTabDrain.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionTabDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionTabDivision.SelectedItem.Value == String.Empty)
                {
                    ddlDivisionTabSubDivision.SelectedIndex = 0;
                    ddlDivisionTabSubDivision.Enabled = false;
                    ddlDivisionTabProtectionStructure.SelectedIndex = 0;
                    ddlDivisionTabProtectionStructure.Enabled = false;
                }
                else if (ddlDivisionTabDivision.SelectedItem.Value != String.Empty && (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation || ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf) && (ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel || ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet || ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain))
                {
                    long DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    Dropdownlist.DDLSubDivisions(ddlDivisionTabSubDivision, false, DivisionID);
                    ddlDivisionTabSubDivision.Enabled = true;
                }
                else if (ddlDivisionTabDivision.SelectedItem.Value != String.Empty && (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation || ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf) && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    long DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    long DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
                    Dropdownlist.DDLProtectionInfrastructureByDivisionID(ddlDivisionTabProtectionStructure, DivisionID, DomainID, "Protection Infrastructure");
                    ddlDivisionTabProtectionStructure.Enabled = true;
                }
                ddlDivisionTabChannel.SelectedIndex = 0;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabOutlet.SelectedIndex = 0;
                ddlDivisionTabOutlet.Enabled = false;
                ddlDivisionTabDrain.SelectedIndex = 0;
                ddlDivisionTabDrain.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionTabSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionTabSubDivision.SelectedItem.Value == String.Empty)
                {
                    ddlDivisionTabChannel.SelectedIndex = 0;
                    ddlDivisionTabChannel.Enabled = false;
                    ddlDivisionTabDrain.SelectedIndex = 0;
                    ddlDivisionTabDrain.Enabled = false;

                }
                else if (ddlDivisionTabSubDivision.SelectedItem.Value != String.Empty && ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && (ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel || ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet))
                {
                    long SubDivisionID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    Dropdownlist.DDLChannelsBySubDivID(ddlDivisionTabChannel, SubDivisionID);
                    ddlDivisionTabChannel.Enabled = true;
                }
                else if (ddlDivisionTabSubDivision.SelectedItem.Value != String.Empty && ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
                {
                    long SubDivisionID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    long DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
                    Dropdownlist.DDLDrainsBySubDivisionID(ddlDivisionTabDrain, SubDivisionID, DomainID, "Drain");
                    ddlDivisionTabDrain.Enabled = true;
                }
                ddlDivisionTabOutlet.SelectedIndex = 0;
                ddlDivisionTabOutlet.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionTabChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionTabChannel.SelectedItem.Value == string.Empty)
                {
                    ddlDivisionTabOutlet.SelectedIndex = 0;
                    ddlDivisionTabOutlet.Enabled = false;
                }
                else if (ddlDivisionTabChannel.SelectedItem.Value != String.Empty && ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                {
                    long SubDivisionID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    long ChannelID = Convert.ToInt64(ddlDivisionTabChannel.SelectedItem.Value);
                    Dropdownlist.DDLOutletsBySubDivIDAndChannelID(ddlDivisionTabOutlet, SubDivisionID, ChannelID);
                    ddlDivisionTabOutlet.Enabled = true;

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Div With Tabs Pannel (Village)
        protected void ddlVillageTabDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long DistrictID = ddlVillageTabDistrict.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                if (DistrictID == -1)
                {
                    ddlVillageTabTehsil.SelectedIndex = -1;
                    ddlVillageTabTehsil.Enabled = false;
                }
                else
                {
                    Dropdownlist.DDLTehsils(ddlVillageTabTehsil, false, DistrictID);
                    ddlVillageTabTehsil.Enabled = true;
                }
                ddlVillageTabVillage.SelectedIndex = 0;
                ddlVillageTabVillage.Enabled = false;
                ddlVillageTabDivision.SelectedIndex = 0;
                ddlVillageTabDivision.Enabled = false;
                ddlVillageTabChannel.SelectedIndex = 0;
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabOutlet.SelectedIndex = 0;
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabProtectionStructure.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlVillageTabTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long TehsilID = ddlVillageTabTehsil.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);

                if (TehsilID == -1)
                {
                    ddlVillageTabVillage.SelectedIndex = 0;
                    ddlVillageTabVillage.Enabled = false;
                }
                else
                {
                    Dropdownlist.DDLVillagesByTehsilID(ddlVillageTabVillage, TehsilID);
                    ddlVillageTabVillage.Enabled = true;
                }
                ddlVillageTabDivision.SelectedIndex = 0;
                ddlVillageTabDivision.Enabled = false;
                ddlVillageTabChannel.SelectedIndex = 0;
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabOutlet.SelectedIndex = 0;
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabProtectionStructure.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlVillageTabVillage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long VillageID = ddlVillageTabVillage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                long DomainID = ddlDomain.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDomain.SelectedItem.Value);

                if (VillageID == -1)
                {
                    ddlVillageTabDivision.SelectedIndex = 0;
                    ddlVillageTabDivision.Enabled = false;
                    ddlVillageTabChannel.SelectedIndex = 0;
                    ddlVillageTabChannel.Enabled = false;
                    ddlVillageTabProtectionStructure.SelectedIndex = 0;
                    ddlVillageTabProtectionStructure.Enabled = false;
                    ddlVillageTabDrain.SelectedIndex = 0;
                    ddlVillageTabDrain.Enabled = false;
                    ddlVillageTabOutlet.SelectedIndex = 0;
                    ddlVillageTabOutlet.Enabled = false;
                }
                else
                {
                    if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                    {
                        Dropdownlist.DDLDivisionsByVillageID(ddlVillageTabDivision, VillageID);
                        Dropdownlist.DDLChannelsByVillageID(ddlVillageTabChannel, VillageID);
                        ddlVillageTabDivision.Enabled = true;
                        ddlVillageTabChannel.Enabled = true;
                    }
                    else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                    {
                        Dropdownlist.DDLChannelsByVillageID(ddlVillageTabChannel, VillageID);
                        ddlVillageTabChannel.Enabled = true;
                        ddlVillageTabOutlet.SelectedIndex = 0;
                        ddlVillageTabOutlet.Enabled = false;
                        ddlVillageTabDivision.SelectedIndex = 0;
                        ddlVillageTabDivision.Enabled = false;
                    }
                    else if ((ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation || ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf) && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                    {
                        Dropdownlist.DDLProtectionStructureByVillageIDAndDomainID(ddlVillageTabProtectionStructure, VillageID, DomainID);
                        Dropdownlist.DDLDivisionsByVillageID(ddlVillageTabDivision, VillageID);
                        ddlVillageTabProtectionStructure.Enabled = true;
                        ddlVillageTabDivision.Enabled = true;
                    }
                    else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
                    {
                        Dropdownlist.DDLProtectionStructureByVillageIDAndDomainID(ddlVillageTabDrain, VillageID, DomainID);
                        Dropdownlist.DDLDivisionsByVillageID(ddlVillageTabDivision, VillageID);
                        ddlVillageTabDrain.Enabled = true;
                        ddlVillageTabDivision.Enabled = true;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlVillageTabChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ChannelID = ddlVillageTabChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabChannel.SelectedItem.Value);
                long VillageID = ddlVillageTabVillage.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);

                if (ChannelID == -1)
                {
                    ddlVillageTabOutlet.SelectedIndex = 0;
                    ddlVillageTabOutlet.Enabled = false;
                }
                else
                {
                    if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                    {
                        Dropdownlist.DDLOutletsByVillageIDAndChannelID(ddlVillageTabOutlet, VillageID, ChannelID);
                        ddlVillageTabOutlet.Enabled = true;
                    }
                }

                if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                {

                    ddlVillageTabDivision.SelectedIndex = 0;
                    ddlVillageTabDivision.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlVillageTabOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long OutletID = ddlVillageTabOutlet.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlVillageTabOutlet.SelectedItem.Value);

                if (OutletID == -1)
                {
                    ddlVillageTabDivision.SelectedIndex = 0;
                    ddlVillageTabDivision.Enabled = false;
                }
                else
                {
                    Dropdownlist.DDLDivisionByOutletID(ddlVillageTabDivision, OutletID);
                    ddlVillageTabDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Div Without Pannel Dropdown Events
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
                    ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    Dropdownlist.DDLDivisions(ddlDivision, false, CircleID);
                    ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
                long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value == String.Empty)
                {

                }
                else if (ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
                {
                    Dropdownlist.DDLProtectionInfrastructureByDivisionID(ddlBarrage, DivisionID, DomainID, "Protection Infrastructure");
                    ddlBarrage.Enabled = true;
                }
                else if (ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDam)
                {

                }
                else if (ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        /// <summary>
        /// this function binds checkbox list
        /// Created On: 22/09/2016
        /// </summary>
        private void BindNotificationListCheckList()
        {
            ComplaintsManagementBLL bllComplaintsManagement = new ComplaintsManagementBLL();
            List<dynamic> lst = bllComplaintsManagement.GetAllNotifications();
            chkAdditionalAccessibility.DataValueField = "ID";
            chkAdditionalAccessibility.DataTextField = "Name";
            chkAdditionalAccessibility.DataSource = lst;
            chkAdditionalAccessibility.DataBind();

            if (string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
            {
                foreach (ListItem li in chkAdditionalAccessibility.Items)
                {
                    if ((Convert.ToInt64(li.Value) == (long)Constants.Designation.ChiefMonitoring) ||
                        (Convert.ToInt64(li.Value) == (long)Constants.Designation.DirectorGauges))
                    {
                        li.Selected = true;
                    }
                }
            }
        }

        #region Insert Data Tables

        private CM_Complaint AddComplaints()
        {
            CM_Complaint mdlComplaint = new CM_Complaint();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
            UA_AssociatedLocation mdlAssociatedLocation = null;
            if (ddlDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
            }
            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
            }
            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
            }
            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
            }

            string FileName = string.Empty;

            if (string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
            {
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ComplaintDashboardID"]))
            {
                long CompID = Convert.ToInt64(Request.QueryString["ComplaintDashboardID"]);
                mdlComplaint.ID = CompID;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
            {
                long CompID = Convert.ToInt64(Request.QueryString["ComplaintSearchID"]);
                mdlComplaint.ID = CompID;
            }


            mdlComplaint.ComplaintSourceID = Convert.ToInt64(ddlComplaintSource.SelectedItem.Value);


            if (!string.IsNullOrEmpty(Request.QueryString["ComplaintDashboardID"]) || !string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
            {
                mdlComplaint.ComplaintNumber = txtComplaintID.Text;
            }
            else
            {
                long ComplaintSourceID = Convert.ToInt64(ddlComplaintSource.SelectedItem.Value);
                // if (ComplaintSourceID == 1)
                mdlComplaint.ComplaintNumber = txtComplaintID.Text;
                //  else
                //    mdlComplaint.ComplaintNumber = GetComplaintID(ComplaintSourceID);
            }

            mdlComplaint.ComplainantName = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
            mdlComplaint.MobilePhone = txtMobileNo.Text;
            mdlComplaint.Phone = txtPhoneNo.Text;
            mdlComplaint.ComplaintDate = Convert.ToDateTime(txtComplaintDate.Text);
            mdlComplaint.Address = txtAddress.Text;

            mdlComplaint.DomainID = Convert.ToInt64(ddlDomain.SelectedItem.Value);
            mdlComplaint.ComplaintStatusID = 1;
            mdlComplaint.ComplaintTypeID = Convert.ToInt64(ddlComplaintType.SelectedItem.Value);

            if (FileName != string.Empty)
            {
                mdlComplaint.Attachment = FileName;
            }

            if (txtResponseDuration.Text != string.Empty)
                mdlComplaint.ResponseDuration = Convert.ToInt16(txtResponseDuration.Text);
            mdlComplaint.PMIUFileNo = txtPmiuFileNo.Text;
            mdlComplaint.ComplaintDetails = txtComplaintDetails.Text;

            if (mdlAssociatedLocation != null)
            {
                mdlComplaint.AssignedToUser = mdlAssociatedLocation.UserID;
                mdlComplaint.AssignedToDesig = mdlAssociatedLocation.UA_Users.DesignationID;
                mdlComplaint.AssignedDate = DateTime.Now;
            }

            mdlComplaint.UserID = mdlUser.ID;
            mdlComplaint.UserDesigID = mdlUser.DesignationID;
            mdlComplaint.CreatedBy = Convert.ToInt32(mdlUser.ID);
            mdlComplaint.CreatedDate = DateTime.Now;


            if (ddlDomain.SelectedItem.Text.Trim().ToUpper() != Configuration.Complaint.Development)
            {
                mdlComplaint.StructureTypeID = Convert.ToInt64(ddlStructure.SelectedItem.Value);
            }


            #region Domain Irrigation
            if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
            {
                if (lblValueForTab.Text.ToUpper() == "DIVISION")
                {
                    mdlComplaint.ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);
                    mdlComplaint.CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    mdlComplaint.SubDivID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    mdlComplaint.ChannelID = Convert.ToInt64(ddlDivisionTabChannel.SelectedItem.Value);
                    if (txtDivisionTabTotalRDLeft.Text != string.Empty && txtDivisionTabTotalRDRight.Text != string.Empty)
                    {
                        mdlComplaint.RD = Calculations.CalculateTotalRDs(txtDivisionTabTotalRDLeft.Text, txtDivisionTabTotalRDRight.Text);
                    }

                }
                else if (lblValueForTab.Text.ToUpper() == "VILLAGE")
                {
                    mdlComplaint.DistrictID = Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                    mdlComplaint.TehsilID = Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);
                    mdlComplaint.VillageID = Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                    mdlComplaint.ChannelID = Convert.ToInt64(ddlVillageTabChannel.SelectedItem.Value);
                    if (txtVillageTabTotalRDLeft.Text != string.Empty && txtVillageTabTotalRDRight.Text != string.Empty)
                    {
                        mdlComplaint.RD = Calculations.CalculateTotalRDs(txtVillageTabTotalRDLeft.Text, txtVillageTabTotalRDRight.Text);
                    }

                    mdlComplaint.DivisionID = Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value);
                }
            }
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
            {
                if (lblValueForTab.Text.ToUpper() == "DIVISION")
                {
                    mdlComplaint.ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);
                    mdlComplaint.CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    mdlComplaint.SubDivID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    mdlComplaint.ChannelID = Convert.ToInt64(ddlDivisionTabChannel.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlDivisionTabOutlet.SelectedItem.Value);
                }
                else if (lblValueForTab.Text.ToUpper() == "VILLAGE")
                {
                    mdlComplaint.DistrictID = Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                    mdlComplaint.TehsilID = Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);
                    mdlComplaint.VillageID = Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                    mdlComplaint.ChannelID = Convert.ToInt64(ddlVillageTabChannel.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlVillageTabOutlet.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value);
                }

            }
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
            {
                if (lblValueForTab.Text.ToUpper() == "DIVISION")
                {
                    mdlComplaint.ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);
                    mdlComplaint.CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlDivisionTabProtectionStructure.SelectedItem.Value);
                }
                else if (lblValueForTab.Text.ToUpper() == "VILLAGE")
                {
                    mdlComplaint.DistrictID = Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                    mdlComplaint.TehsilID = Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);
                    mdlComplaint.VillageID = Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlVillageTabProtectionStructure.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value);
                }

            }
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
            {
                mdlComplaint.ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                mdlComplaint.CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                mdlComplaint.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                mdlComplaint.StructureID = Convert.ToInt64(ddlBarrage.SelectedItem.Value);
            }
            #endregion

            #region Domain D&F
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
            {
                if (lblValueForTab.Text.ToUpper() == "DIVISION")
                {
                    mdlComplaint.ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);
                    mdlComplaint.CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlDivisionTabProtectionStructure.SelectedItem.Value);
                }
                else if (lblValueForTab.Text.ToUpper() == "VILLAGE")
                {
                    mdlComplaint.DistrictID = Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                    mdlComplaint.TehsilID = Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);
                    mdlComplaint.VillageID = Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlVillageTabProtectionStructure.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value);
                }
            }
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
            {
                if (lblValueForTab.Text.ToUpper() == "DIVISION")
                {
                    mdlComplaint.ZoneID = Convert.ToInt64(ddlDivisionTabZone.SelectedItem.Value);
                    mdlComplaint.CircleID = Convert.ToInt64(ddlDivisionTabCircle.SelectedItem.Value);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value);
                    mdlComplaint.SubDivID = Convert.ToInt64(ddlDivisionTabSubDivision.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlDivisionTabDrain.SelectedItem.Value);
                    if (txtDivisionTabTotalRDLeft.Text != string.Empty && txtDivisionTabTotalRDRight.Text != string.Empty)
                    {
                        mdlComplaint.RD = Calculations.CalculateTotalRDs(txtDivisionTabTotalRDLeft.Text, txtDivisionTabTotalRDRight.Text);
                    }
                }
                else if (lblValueForTab.Text.ToUpper() == "VILLAGE")
                {
                    mdlComplaint.DistrictID = Convert.ToInt64(ddlVillageTabDistrict.SelectedItem.Value);
                    mdlComplaint.TehsilID = Convert.ToInt64(ddlVillageTabTehsil.SelectedItem.Value);
                    mdlComplaint.VillageID = Convert.ToInt64(ddlVillageTabVillage.SelectedItem.Value);
                    mdlComplaint.StructureID = Convert.ToInt64(ddlVillageTabDrain.SelectedItem.Value);
                    mdlComplaint.RD = Calculations.CalculateTotalRDs(txtVillageTabTotalRDLeft.Text, txtVillageTabTotalRDRight.Text);
                    mdlComplaint.DivisionID = Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value);
                }
            }
            #endregion

            #region Domain Small Dams
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDam)
            {
                mdlComplaint.ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                mdlComplaint.CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                mdlComplaint.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                mdlComplaint.StructureID = Convert.ToInt64(ddlSmallDams.SelectedItem.Value);
            }
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
            {
                mdlComplaint.ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                mdlComplaint.CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                mdlComplaint.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                mdlComplaint.ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
            }
            #endregion

            #region Domain Development
            else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Development)
            {
                mdlComplaint.DivisionID = Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value);
            }
            #endregion

            return mdlComplaint;

        }

        private CM_ComplaintAssignmentHistory AddComplaintAssignemntHistory()
        {
            CM_ComplaintAssignmentHistory mdlComplaintAssignmentHistory = new CM_ComplaintAssignmentHistory();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
            UA_AssociatedLocation mdlAssociatedLocation = null;
            if (ddlDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
            }
            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
            }
            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
            }
            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
            {
                mdlAssociatedLocation = bllComplaintsManagementBLL.GetUserByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
            }



            mdlComplaintAssignmentHistory.ComplaintStatusID = 1;
            mdlComplaintAssignmentHistory.AssignedByUser = mdlUser.ID;
            mdlComplaintAssignmentHistory.AssignedByDesig = mdlUser.DesignationID;
            if (mdlAssociatedLocation != null)
            {
                mdlComplaintAssignmentHistory.AssignedToUser = mdlAssociatedLocation.UserID;
                mdlComplaintAssignmentHistory.AssignedToDesig = mdlAssociatedLocation.UA_Users.DesignationID;
                mdlComplaintAssignmentHistory.AssignedDate = DateTime.Now;
            }
            mdlComplaintAssignmentHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
            mdlComplaintAssignmentHistory.CreatedDate = DateTime.Now;

            return mdlComplaintAssignmentHistory;
        }

        private List<CM_ComplaintNotification> AddComplaintNotificationAndComplaintMarked(ref List<CM_ComplaintMarked> lstComplaintMarked)
        {
            List<CM_ComplaintNotification> lstComplaintNotification = new List<CM_ComplaintNotification>();
            CM_ComplaintNotification mdlComplaintNotification = null;
            CM_ComplaintMarked mdlComplaintMarked = null;

            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            dynamic Location = null;
            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
            List<long> lstUsers = null;



            foreach (ListItem li in chkAdditionalAccessibility.Items)
            {

                if (li.Text.Trim().ToUpper() == "DEPUTYDIRECTORHELPLINE" || li.Text.Trim().ToUpper() == "DEPUTY DIRECTOR HELPLINE")
                {
                    li.Selected = false;
                }
                if (li.Selected)
                {
                    CM_NotificationList mdlNotificationList = bllComplaintsManagementBLL.GetOrganizationByDesignationID(Convert.ToInt64(li.Value));
                    if (mdlNotificationList.OrgID == (long)Constants.Organization.PMIU)
                    {

                        if (li.Value == Convert.ToString((int)Constants.Designation.ADM)) //if (li.Text.Trim().ToUpper() == Convert.ToString(Constants.Designation.ADM).Trim().ToUpper())
                        {
                            if (ddlDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
                            }
                            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
                            }

                            lstUsers = bllComplaintsManagementBLL.GetUsersForPmiuAndIrrigation(Convert.ToInt64(li.Value), (long)Constants.IrrigationLevelID.Division, Location.GetType().GetProperty("Division").GetValue(Location, null), (long)Constants.Organization.PMIU);
                        }
                        else if (li.Value == Convert.ToString((int)Constants.Designation.MA))
                        {
                            if (ddlDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
                            }
                            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
                            }
                            lstUsers = bllComplaintsManagementBLL.GetUsersForPmiuAndIrrigation(Convert.ToInt64(li.Value), (long)Constants.IrrigationLevelID.Division, Location.GetType().GetProperty("Division").GetValue(Location, null), (long)Constants.Organization.PMIU);
                        }
                        else
                        {
                            lstUsers = bllComplaintsManagementBLL.GetUsersForPidaAndSecretariat(Convert.ToInt64(li.Value), (long)Constants.Organization.PMIU);
                        }
                    }
                    else if (mdlNotificationList.OrgID == (long)Constants.Organization.PIDA)
                    {
                        lstUsers = bllComplaintsManagementBLL.GetUsersForPidaAndSecretariat(Convert.ToInt64(li.Value), (long)Constants.Organization.PIDA);
                    }
                    else if (mdlNotificationList.OrgID == (long)Constants.Organization.Secretariat)
                    {
                        lstUsers = bllComplaintsManagementBLL.GetUsersForPidaAndSecretariat(Convert.ToInt64(li.Value), (long)Constants.Organization.Secretariat);
                    }
                    else if (mdlNotificationList.OrgID == (long)Constants.Organization.Irrigation)
                    {
                        if (li.Value == Convert.ToString((int)Constants.Designation.ChiefIrrigation))
                        {
                            if (ddlDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
                            }
                            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
                            }
                            lstUsers = bllComplaintsManagementBLL.GetUsersForPmiuAndIrrigation(Convert.ToInt64(li.Value), (long)Constants.IrrigationLevelID.Zone, Location.GetType().GetProperty("Zone").GetValue(Location, null), (long)Constants.Organization.Irrigation);
                        }
                        else if (li.Value == Convert.ToString((int)Constants.Designation.SE))
                        {
                            if (ddlDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
                            }
                            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
                            }
                            lstUsers = bllComplaintsManagementBLL.GetUsersForPmiuAndIrrigation(Convert.ToInt64(li.Value), (long)Constants.IrrigationLevelID.Circle, Location.GetType().GetProperty("Circle").GetValue(Location, null), (long)Constants.Organization.Irrigation);
                        }
                        else if (li.Value == Convert.ToString((int)Constants.Designation.XEN))
                        {
                            if (ddlDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionTabDivision.SelectedItem.Value));
                            }
                            else if (ddlVillageTabDivision.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlVillageTabDivision.SelectedItem.Value));
                            }
                            else if (ddlDivisionByDomain.SelectedItem.Value != string.Empty)
                            {
                                Location = bllComplaintsManagementBLL.GetIrrigtionBoundaryByDivisionID(Convert.ToInt64(ddlDivisionByDomain.SelectedItem.Value));
                            }
                            lstUsers = bllComplaintsManagementBLL.GetUsersForPmiuAndIrrigation(Convert.ToInt64(li.Value), (long)Constants.IrrigationLevelID.Division, Location.GetType().GetProperty("Division").GetValue(Location, null), (long)Constants.Organization.Irrigation);
                        }
                    }
                    else
                    {
                        lstUsers = bllComplaintsManagementBLL.GetUsersByDesignationID(Convert.ToInt64(li.Value));
                    }
                    if (lstUsers != null)
                    {
                        for (int i = 0; i < lstUsers.Count; i++)
                        {
                            mdlComplaintNotification = new CM_ComplaintNotification
                            {
                                UserID = lstUsers.ElementAt(i),
                                UserDesigID = Convert.ToInt64(li.Value),
                                CreatedBy = Convert.ToInt32(mdlUser.ID),
                                CreatedDate = DateTime.Now
                            };

                            mdlComplaintMarked = new CM_ComplaintMarked
                            {
                                UserID = lstUsers.ElementAt(i),
                                UserDesigID = Convert.ToInt64(li.Value),
                                Starred = false,
                                MarkRead = false,
                                CreatedBy = Convert.ToInt32(mdlUser.ID),
                                CreatedDate = DateTime.Now
                            };
                            lstComplaintNotification.Add(mdlComplaintNotification);
                            lstComplaintMarked.Add(mdlComplaintMarked);
                        }
                    }
                    lstUsers = null;
                }
            }
            return lstComplaintNotification;
        }

        private string GetComplaintID(long ComplaintSourceID)
        {
            ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
            CM_ComplaintSource mdlComplaintSource = bllComplaints.GetComplaintSourceForComplaintID(ComplaintSourceID);
            string CID = string.Empty;

            if (ComplaintSourceID == 1)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 2)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 3)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 4)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 5)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 6)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 7)
            {
                CID = mdlComplaintSource.ShortCode + "" + bllComplaints.GetNewComplaintID();
            }
            return CID;
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ComplaintsManagementBLL bllComplaintsManagement = new ComplaintsManagementBLL();
                List<CM_ComplaintMarked> lstComplaintMarked = new List<CM_ComplaintMarked>();
                CM_ComplaintNotification CompNotification = new CM_ComplaintNotification();
                CM_ComplaintMarked CompMarked = new CM_ComplaintMarked();
                CM_ComplaintMarked mdlCompMarked = new CM_ComplaintMarked();
                List<CM_ComplaintNotification> lstAddComplaintNotification = AddComplaintNotificationAndComplaintMarked(ref lstComplaintMarked);
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long CompSourceID = 0;

                if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSource"]))
                {
                    CompSourceID = Convert.ToInt64(Request.QueryString["ComplaintSource"]);
                }




                if (CompSourceID == 6) // auto generated complaints 
                {
                    long CompID = Convert.ToInt64(Request.QueryString["ComplaintSearchID"]);
                    CompSourceID = Convert.ToInt64(Request.QueryString["ComplaintSource"]);

                    //DDH Entry
                    CM_ComplaintMarked mdlCompMarkedForDDH = new CM_ComplaintMarked();
                    CM_ComplaintNotification mdlCompNotificationForDDH = new CM_ComplaintNotification();
                    List<long> DDH = bllComplaintsManagement.GetUsersForPidaAndSecretariat((long)Constants.Designation.DeputyDirectorHelpline, (long)Constants.Organization.PMIU);
                    for (int i = 0; i < DDH.Count(); i++)
                    {
                        mdlCompMarkedForDDH.UserID = DDH.ElementAt(i);
                        mdlCompMarkedForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
                        mdlCompMarkedForDDH.Starred = false;
                        mdlCompMarkedForDDH.MarkRead = false;
                        mdlCompMarkedForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
                        mdlCompMarkedForDDH.CreatedDate = DateTime.Now;
                    }
                    mdlCompNotificationForDDH.UserID = mdlCompMarkedForDDH.UserID;
                    mdlCompNotificationForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
                    mdlCompNotificationForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCompNotificationForDDH.CreatedDate = DateTime.Now;
                    mdlCompMarkedForDDH.ComplaintID = CompID;
                    mdlCompNotificationForDDH.ComplaintID = CompID;

                    //////////////////////////
                    List<CM_ComplaintMarked> lstComplaintsMarked = bllComplaintsManagement.GetComplaintMarkedByComplaintID(CompID);

                    for (int i = 0; i < lstComplaintsMarked.Count(); i++)
                    {
                        bllComplaintsManagement.DeleteComplaintMarked(lstComplaintsMarked.ElementAt(i).ID);
                    }

                    List<CM_ComplaintNotification> lstComplaintsNotification = bllComplaintsManagement.GetComplaintNotificationByComplaintID(CompID);

                    for (int i = 0; i < lstComplaintsNotification.Count(); i++)
                    {
                        bllComplaintsManagement.DeleteComplaintNotification(lstComplaintsNotification.ElementAt(i).ID);
                    }
                    //////////////////////////

                    for (int i = 0; i < lstAddComplaintNotification.Count; i++)
                    {
                        lstAddComplaintNotification.ElementAt(i).ComplaintID = CompID;
                        bllComplaintsManagement.AddComplaintNotification(lstAddComplaintNotification.ElementAt(i));
                    }

                    for (int i = 0; i < lstComplaintMarked.Count; i++)
                    {
                        lstComplaintMarked.ElementAt(i).ComplaintID = CompID;
                        bllComplaintsManagement.AddComplaintMarked(lstComplaintMarked.ElementAt(i));
                    }

                    bllComplaintsManagement.AddComplaintMarked(mdlCompMarkedForDDH);
                    bllComplaintsManagement.AddComplaintNotification(mdlCompNotificationForDDH);

                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    //btnSave.Enabled = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtComplaintID.Text))
                    {
                        CM_Complaint mdlAddComplaint = AddComplaints();
                        bool Exist = false;
                        bool BInitial = false;
                        string Number = "";
                        //if (mdlAddComplaint.ComplaintSourceID == 1) // Default Helpline 
                        //{
                        string ID = Convert.ToString(mdlAddComplaint.ComplaintNumber);
                        string Initial = ID.Substring(0, 2);

                        if (mdlAddComplaint.ComplaintSourceID == 1 && Initial == "DH")
                            BInitial = true;
                        else if (mdlAddComplaint.ComplaintSourceID == 2 && Initial == "CM")
                            BInitial = true;
                        else if (mdlAddComplaint.ComplaintSourceID == 3 && Initial == "CS")
                            BInitial = true;
                        else if (mdlAddComplaint.ComplaintSourceID == 4 && Initial == "SI")
                            BInitial = true;
                        else if (mdlAddComplaint.ComplaintSourceID == 5 && Initial == "TP")
                            BInitial = true;
                        else if (mdlAddComplaint.ComplaintSourceID == 7 && Initial == "MI")
                            BInitial = true;

                        if (BInitial) //(Initial == "DH" || Initial == "CM" || Initial == "CS" || Initial == "SI" || Initial == "TP" || Initial == "MI")
                        {
                            Number = ID.Substring(2);
                            try
                            {
                                int temp = Convert.ToInt32(Number);
                                Number = temp.ToString();

                                if (Number.Length == 5)
                                    Number = "0" + Number;
                                else if (Number.Length == 4)
                                    Number = "00" + Number;
                                else if (Number.Length == 3)
                                    Number = "000" + Number;
                                else if (Number.Length == 2)
                                    Number = "0000" + Number;
                                else if (Number.Length == 1)
                                    Number = "00000" + Number;
                                else if (Number.Length == 0)
                                    Number = "000000";

                                Exist = new ComplaintsManagementBLL().ComplaintNumberExist(Number);
                                if (Exist)
                                    Master.ShowMessage("Complaint number already exist", SiteMaster.MessageType.Error);
                            }
                            catch (Exception h)
                            {
                                Exist = true;

                                if (mdlAddComplaint.ComplaintSourceID == 1)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. DHXXXXXX", SiteMaster.MessageType.Error);
                                else if (mdlAddComplaint.ComplaintSourceID == 2)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. CMXXXXXX", SiteMaster.MessageType.Error);
                                else if (mdlAddComplaint.ComplaintSourceID == 3)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. CSXXXXXX", SiteMaster.MessageType.Error);
                                else if (mdlAddComplaint.ComplaintSourceID == 4)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. SIXXXXXX", SiteMaster.MessageType.Error);
                                else if (mdlAddComplaint.ComplaintSourceID == 5)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. TPXXXXXX", SiteMaster.MessageType.Error);
                                else if (mdlAddComplaint.ComplaintSourceID == 7)
                                    Master.ShowMessage("Follow pattern for Complaint ID e.g. MIXXXXXX", SiteMaster.MessageType.Error);
                                else
                                    Master.ShowMessage("Follow pattern for Complaint ID", SiteMaster.MessageType.Error);
                            }
                        }
                        else
                        {
                            Exist = true;

                            if (mdlAddComplaint.ComplaintSourceID == 1)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. DHXXXXXX", SiteMaster.MessageType.Error);
                            else if (mdlAddComplaint.ComplaintSourceID == 2)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. CMXXXXXX", SiteMaster.MessageType.Error);
                            else if (mdlAddComplaint.ComplaintSourceID == 3)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. CSXXXXXX", SiteMaster.MessageType.Error);
                            else if (mdlAddComplaint.ComplaintSourceID == 4)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. SIXXXXXX", SiteMaster.MessageType.Error);
                            else if (mdlAddComplaint.ComplaintSourceID == 5)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. TPXXXXXX", SiteMaster.MessageType.Error);
                            else if (mdlAddComplaint.ComplaintSourceID == 7)
                                Master.ShowMessage("Follow pattern for Complaint ID e.g. MIXXXXXX", SiteMaster.MessageType.Error);
                            else
                                Master.ShowMessage("Follow pattern for Complaint ID", SiteMaster.MessageType.Error);
                        }
                        //}

                        if (!Exist)
                        {
                            if (mdlAddComplaint.ComplaintSourceID == 1)
                                mdlAddComplaint.ComplaintNumber = "DH" + Number;
                            else if (mdlAddComplaint.ComplaintSourceID == 2)
                                mdlAddComplaint.ComplaintNumber = "CM" + Number;
                            else if (mdlAddComplaint.ComplaintSourceID == 3)
                                mdlAddComplaint.ComplaintNumber = "CS" + Number;
                            else if (mdlAddComplaint.ComplaintSourceID == 4)
                                mdlAddComplaint.ComplaintNumber = "SI" + Number;
                            else if (mdlAddComplaint.ComplaintSourceID == 5)
                                mdlAddComplaint.ComplaintNumber = "TP" + Number;
                            else if (mdlAddComplaint.ComplaintSourceID == 7)
                                mdlAddComplaint.ComplaintNumber = "MI" + Number;

                            CM_ComplaintAssignmentHistory mdlAddComplaintAssignemntHistory = AddComplaintAssignemntHistory();
                            mdlCompMarked.UserID = mdlAddComplaintAssignemntHistory.AssignedToUser;
                            mdlCompMarked.UserDesigID = mdlAddComplaintAssignemntHistory.AssignedToDesig;
                            mdlCompMarked.MarkRead = false;
                            mdlCompMarked.Starred = false;
                            mdlCompMarked.CreatedBy = mdlAddComplaintAssignemntHistory.CreatedBy;
                            mdlCompMarked.CreatedDate = DateTime.Now;

                            //DDH Entry
                            CM_ComplaintMarked mdlCompMarkedForDDH = new CM_ComplaintMarked();
                            CM_ComplaintNotification mdlCompNotificationForDDH = new CM_ComplaintNotification();
                            List<long> DDH = bllComplaintsManagement.GetUsersForPidaAndSecretariat((long)Constants.Designation.DeputyDirectorHelpline, (long)Constants.Organization.PMIU);
                            for (int i = 0; i < DDH.Count(); i++)
                            {
                                mdlCompMarkedForDDH.UserID = DDH.ElementAt(i);
                                mdlCompMarkedForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
                                mdlCompMarkedForDDH.Starred = false;
                                mdlCompMarkedForDDH.MarkRead = false;
                                mdlCompMarkedForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
                                mdlCompMarkedForDDH.CreatedDate = DateTime.Now;
                            }
                            mdlCompNotificationForDDH.UserID = mdlCompMarkedForDDH.UserID;
                            mdlCompNotificationForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
                            mdlCompNotificationForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            mdlCompNotificationForDDH.CreatedDate = DateTime.Now;

                            using (TransactionScope transaction = new TransactionScope())
                            {
                                if (!string.IsNullOrEmpty(Request.QueryString["ComplaintDashboardID"]) || !string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
                                {
                                    bllComplaintsManagement.UpdateComplaint(mdlAddComplaint);

                                    List<CM_ComplaintAssignmentHistory> lstComplaintAssignmentHistory = bllComplaintsManagement.GetComplaintAssignmentHistoryByComplaintID(mdlAddComplaint.ID);

                                    List<CM_ComplaintMarked> lstComplaintsMarked = bllComplaintsManagement.GetComplaintMarkedByComplaintID(mdlAddComplaint.ID);

                                    for (int i = 0; i < lstComplaintsMarked.Count(); i++)
                                    {
                                        bllComplaintsManagement.DeleteComplaintMarked(lstComplaintsMarked.ElementAt(i).ID);
                                    }

                                    List<CM_ComplaintNotification> lstComplaintsNotification = bllComplaintsManagement.GetComplaintNotificationByComplaintID(mdlAddComplaint.ID);

                                    for (int i = 0; i < lstComplaintsNotification.Count(); i++)
                                    {
                                        bllComplaintsManagement.DeleteComplaintNotification(lstComplaintsNotification.ElementAt(i).ID);
                                    }

                                    for (int i = 0; i < lstComplaintAssignmentHistory.Count(); i++)
                                    {
                                        CompNotification = new CM_ComplaintNotification
                                        {
                                            UserID = lstComplaintAssignmentHistory.ElementAt(i).AssignedToUser,
                                            ComplaintID = lstComplaintAssignmentHistory.ElementAt(i).ComplaintID,
                                            UserDesigID = lstComplaintAssignmentHistory.ElementAt(i).AssignedToDesig,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = Convert.ToInt32(mdlUser.ID),
                                        };

                                        CompMarked = new CM_ComplaintMarked
                                        {
                                            UserID = lstComplaintAssignmentHistory.ElementAt(i).AssignedToUser,
                                            UserDesigID = lstComplaintAssignmentHistory.ElementAt(i).AssignedToDesig,
                                            Starred = false,
                                            MarkRead = false,
                                            CreatedBy = Convert.ToInt32(mdlUser.ID),
                                            CreatedDate = DateTime.Now
                                        };
                                        lstAddComplaintNotification.Add(CompNotification);
                                        lstComplaintMarked.Add(CompMarked);

                                    }


                                    for (int i = 0; i < lstAddComplaintNotification.Count; i++)
                                    {
                                        lstAddComplaintNotification.ElementAt(i).ComplaintID = mdlAddComplaint.ID;
                                        bllComplaintsManagement.AddComplaintNotification(lstAddComplaintNotification.ElementAt(i));
                                    }

                                    for (int i = 0; i < lstComplaintMarked.Count; i++)
                                    {
                                        lstComplaintMarked.ElementAt(i).ComplaintID = mdlAddComplaint.ID;
                                        bllComplaintsManagement.AddComplaintMarked(lstComplaintMarked.ElementAt(i));
                                    }
                                    mdlCompMarkedForDDH.ComplaintID = mdlAddComplaint.ID;
                                    mdlCompNotificationForDDH.ComplaintID = mdlAddComplaint.ID;

                                    bllComplaintsManagement.AddComplaintMarked(mdlCompMarkedForDDH);
                                    bllComplaintsManagement.AddComplaintNotification(mdlCompNotificationForDDH);


                                }
                                else
                                {
                                    long ComplaintID = bllComplaintsManagement.AddComplaint(mdlAddComplaint);
                                    mdlAddComplaintAssignemntHistory.ComplaintID = ComplaintID;
                                    bllComplaintsManagement.AddComplaintAssignmentHistory(mdlAddComplaintAssignemntHistory);
                                    mdlCompMarkedForDDH.ComplaintID = ComplaintID;
                                    mdlCompNotificationForDDH.ComplaintID = ComplaintID;



                                    for (int i = 0; i < lstAddComplaintNotification.Count; i++)
                                    {
                                        lstAddComplaintNotification.ElementAt(i).ComplaintID = ComplaintID;
                                        bllComplaintsManagement.AddComplaintNotification(lstAddComplaintNotification.ElementAt(i));
                                    }

                                    for (int i = 0; i < lstComplaintMarked.Count; i++)
                                    {
                                        lstComplaintMarked.ElementAt(i).ComplaintID = ComplaintID;
                                        bllComplaintsManagement.AddComplaintMarked(lstComplaintMarked.ElementAt(i));
                                    }
                                    mdlCompMarked.ComplaintID = ComplaintID;
                                    bllComplaintsManagement.AddComplaintMarked(mdlCompMarked);
                                    bllComplaintsManagement.AddComplaintMarked(mdlCompMarkedForDDH);
                                    bllComplaintsManagement.AddComplaintNotification(mdlCompNotificationForDDH);
                                }

                                transaction.Complete();
                            }


                            if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
                            {
                                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                                btnSave.Enabled = false;
                            }
                            else
                            {
                                NotifyEvent _event = new NotifyEvent();
                                _event.Parameters.Add("ComplaintID", mdlAddComplaint.ID);
                                _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.Complaintisloggedintothesystem, SessionManagerFacade.UserInformation.ID);

                                UA_SMSNotification mdlSMS = new UA_SMSNotification();
                                mdlSMS.NotificationEventID = 20;
                                mdlSMS.UserID = SessionManagerFacade.UserInformation.ID;
                                mdlSMS.MobileNumber = mdlAddComplaint.MobilePhone;
                                mdlSMS.SMSText = "Complaint of Type:" + ddlComplaintType.SelectedItem.Text + " has been logged with Complaint Id:" + mdlAddComplaint.ComplaintNumber;
                                mdlSMS.Status = 0;
                                mdlSMS.TryCount = 0;
                                mdlSMS.CreatedDate = DateTime.Now;
                                mdlSMS.CreatedBy = SessionManagerFacade.UserInformation.ID;
                                bool IsSaved = bllComplaintsManagement.InsertComplanintEntryInSMSTable(mdlSMS);
                                ClearSelections();
                                Master.ShowMessage("New Complaint " + mdlAddComplaint.ComplaintNumber + " has been created in the system successfully", SiteMaster.MessageType.Success);
                            }
                        }
                    }
                    else
                        Master.ShowMessage("Please enter Complaint ID", SiteMaster.MessageType.Error);

                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchDivision_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ddlVillageTabDistrict.ClearSelection();
                ddlVillageTabTehsil.Enabled = false;
                ddlVillageTabTehsil.ClearSelection();
                ddlVillageTabVillage.Enabled = false;
                ddlVillageTabVillage.ClearSelection();
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabChannel.ClearSelection();
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabProtectionStructure.ClearSelection();
                ddlVillageTabDrain.Enabled = false;
                ddlVillageTabDrain.ClearSelection();
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabOutlet.ClearSelection();
                ddlVillageTabDivision.Enabled = false;
                ddlVillageTabDivision.ClearSelection();

                ddlVillageTabDistrict.Visible = false;
                ddlVillageTabTehsil.Visible = false;
                ddlVillageTabVillage.Visible = false;
                ddlVillageTabChannel.Visible = false;
                ddlVillageTabProtectionStructure.Visible = false;
                ddlVillageTabDrain.Visible = false;
                ddlVillageTabOutlet.Visible = false;
                ddlVillageTabDivision.Visible = false;

                #region Irrigation Domain

                if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region DivisionTab
                    //Dropdownlist.DDLZones(ddlDivisionTabZone);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;

                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = true;
                    ddlDivisionTabChannel.Visible = true;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = true;
                    txtDivisionTabTotalRDLeft.Visible = true;
                    txtDivisionTabTotalRDRight.Visible = true;
                    lblDivisionTabPlusSign.Visible = true;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region DivisionTab
                    //Dropdownlist.DDLZones(ddlDivisionTabZone);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;
                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = true;
                    ddlDivisionTabChannel.Visible = true;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = true;
                    ddlDivisionTabOutlet.Visible = true;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region DivisionTab
                    //Dropdownlist.DDLZones(ddlDivisionTabZone);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = false;
                    ddlDivisionTabSubDivision.Visible = false;
                    lblDivisionTabProtectionStructure.Visible = true;
                    ddlDivisionTabProtectionStructure.Visible = true;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = true;
                    ddlBarrage.Visible = true;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    //Dropdownlist.DDLZones(ddlZone);
                }
                #endregion

                #region D&F Domain

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region DivisionTab
                    //Dropdownlist.DDLZones(ddlDivisionTabZone);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = false;
                    ddlDivisionTabSubDivision.Visible = false;
                    lblDivisionTabProtectionStructure.Visible = true;
                    ddlDivisionTabProtectionStructure.Visible = true;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = false;
                    ddlDivisionTabDrain.Visible = false;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = false;
                    txtDivisionTabTotalRDLeft.Visible = false;
                    txtDivisionTabTotalRDRight.Visible = false;
                    lblDivisionTabPlusSign.Visible = false;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region DivisionTab
                    //Dropdownlist.DDLZones(ddlDivisionTabZone);
                    ddlDivisionTabZone.Visible = true;
                    ddlDivisionTabCircle.Visible = true;
                    ddlDivisionTabDivision.Visible = true;
                    lblDivisionTabSubDivision.Visible = true;
                    ddlDivisionTabSubDivision.Visible = true;
                    lblDivisionTabProtectionStructure.Visible = false;
                    ddlDivisionTabProtectionStructure.Visible = false;

                    lblDivisionTabChannel.Visible = false;
                    ddlDivisionTabChannel.Visible = false;
                    lblDivisionTabDrain.Visible = true;
                    ddlDivisionTabDrain.Visible = true;

                    lblDivisionTabOutlet.Visible = false;
                    ddlDivisionTabOutlet.Visible = false;

                    lblDivisionTabTotalRDs.Visible = true;
                    txtDivisionTabTotalRDLeft.Visible = true;
                    txtDivisionTabTotalRDRight.Visible = true;
                    lblDivisionTabPlusSign.Visible = true;
                    #endregion
                }

                #endregion

                #region Small Dams

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDam)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = true;
                    ddlSmallDams.Visible = true;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    //Dropdownlist.DDLZones(ddlZone);
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = true;
                    ddlChannel.Visible = true;

                    // Dropdownlist.DDLZones(ddlZone);
                }

                #endregion

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchVillage_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ddlDivisionTabZone.ClearSelection();
                ddlDivisionTabCircle.Enabled = false;
                ddlDivisionTabCircle.ClearSelection();
                ddlDivisionTabDivision.Enabled = false;
                ddlDivisionTabDivision.ClearSelection();
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabSubDivision.ClearSelection();
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabProtectionStructure.ClearSelection();
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabChannel.ClearSelection();
                ddlDivisionTabDrain.Enabled = false;
                ddlDivisionTabDrain.ClearSelection();
                ddlDivisionTabOutlet.Enabled = false;
                ddlDivisionTabOutlet.ClearSelection();

                ddlDivisionTabZone.Visible = false;
                ddlDivisionTabCircle.Visible = false;
                ddlDivisionTabDivision.Visible = false;
                ddlDivisionTabSubDivision.Visible = false;
                ddlDivisionTabProtectionStructure.Visible = false;
                ddlDivisionTabChannel.Visible = false;
                ddlDivisionTabDrain.Visible = false;
                ddlDivisionTabOutlet.Visible = false;

                #region Irrigation Domain

                if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = true;
                    ddlVillageTabChannel.Visible = true;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    lblVillageTabOutlet.Visible = false;
                    ddlVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = true;
                    txtVillageTabTotalRDRight.Visible = true;
                    lblVillageTabTotalRDs.Visible = true;
                    lblVillageTabPlusSign.Visible = true;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Outlet)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = true;
                    ddlVillageTabChannel.Visible = true;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = true;
                    lblVillageTabOutlet.Visible = true;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = true;
                    ddlVillageTabProtectionStructure.Visible = true;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = false;
                    lblVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = true;
                    ddlBarrage.Visible = true;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    //Dropdownlist.DDLZones(ddlZone);
                }
                #endregion

                #region D&F Domain

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.ProtectionStructure)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = true;
                    ddlVillageTabProtectionStructure.Visible = true;
                    lblVillageTabDrain.Visible = false;
                    ddlVillageTabDrain.Visible = false;

                    ddlVillageTabOutlet.Visible = false;
                    lblVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = false;
                    txtVillageTabTotalRDRight.Visible = false;
                    lblVillageTabTotalRDs.Visible = false;
                    lblVillageTabPlusSign.Visible = false;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion

                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.dandf && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Drain)
                {
                    divPanel.Visible = true;
                    divWithoutTab.Visible = false;

                    #region VillageTab
                    ddlVillageTabDistrict.Visible = true;
                    ddlVillageTabTehsil.Visible = true;
                    ddlVillageTabVillage.Visible = true;
                    lblVillageTabChannel.Visible = false;
                    ddlVillageTabChannel.Visible = false;
                    lblVillageTabProtectionStructure.Visible = false;
                    ddlVillageTabProtectionStructure.Visible = false;
                    lblVillageTabDrain.Visible = true;
                    ddlVillageTabDrain.Visible = true;

                    lblVillageTabOutlet.Visible = false;
                    ddlVillageTabOutlet.Visible = false;

                    txtVillageTabTotalRDLeft.Visible = true;
                    txtVillageTabTotalRDRight.Visible = true;
                    lblVillageTabTotalRDs.Visible = true;
                    lblVillageTabPlusSign.Visible = true;

                    lblVillageTabDivision.Visible = true;
                    ddlVillageTabDivision.Visible = true;
                    #endregion
                }

                #endregion

                #region Small Dams

                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDam)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = true;
                    ddlSmallDams.Visible = true;

                    lblChannel.Visible = false;
                    ddlChannel.Visible = false;

                    //Dropdownlist.DDLZones(ddlZone);
                }
                else if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.SmallDamsDivision && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Channel)
                {
                    divPanel.Visible = false;
                    divWithoutTab.Visible = true;

                    lblBarrage.Visible = false;
                    ddlBarrage.Visible = false;
                    lblSmallDams.Visible = false;
                    ddlSmallDams.Visible = false;

                    lblChannel.Visible = true;
                    ddlChannel.Visible = true;

                    //Dropdownlist.DDLZones(ddlZone);
                }

                #endregion

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlComplaintSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlComplaintSource.SelectedItem.Value != "")
                {
                    long ComplaintSourceID = Convert.ToInt64(ddlComplaintSource.SelectedItem.Value);
                    txtComplaintID.Text = GetComplaintID(ComplaintSourceID);

                    //if (ComplaintSourceID == 1) // for Default Helpline
                    txtComplaintID.Enabled = true;
                    //  else
                    //   txtComplaintID.Enabled = false;
                }
                else
                {
                    txtComplaintID.Text = "";
                    // txtComplaintID.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetComplaintByID(long _ComplaintID)
        {
            ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
            CM_Complaint mdlComplaint = bllComplaints.GetComplaintByID(_ComplaintID);

            string[] strFullName = mdlComplaint.ComplainantName.Split(' ');
            string FirstName = strFullName[0];
            string LastName = string.Empty;
            if (FirstName != "N/A")
            {
                LastName = strFullName[1];
            }
            Dropdownlist.SetSelectedValue(ddlComplaintSource, mdlComplaint.ComplaintSourceID.ToString());
            txtComplaintID.Text = mdlComplaint.ComplaintNumber;
            txtFirstName.Text = FirstName;
            txtLastName.Text = LastName;
            txtAddress.Text = mdlComplaint.Address;
            txtPhoneNo.Text = mdlComplaint.Phone;
            txtMobileNo.Text = mdlComplaint.MobilePhone;
            txtComplaintDate.Text = Utility.GetFormattedDate(mdlComplaint.ComplaintDate);
            Dropdownlist.SetSelectedValue(ddlDomain, mdlComplaint.DomainID.ToString());
            ddlDomain_SelectedIndexChanged(null, null);
            Dropdownlist.SetSelectedValue(ddlStructure, mdlComplaint.StructureTypeID.ToString());
            ddlStructure_SelectedIndexChanged(null, null);

            if (mdlComplaint.ZoneID != null)
            {
                liDivision.Attributes.Add("class", "active");
                liVillage.Attributes.Remove("class");
                Division.Attributes.Add("class", "tab-pane active");
                Village.Attributes.Add("class", "tab-pane");

                if (ddlDomain.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.Irrigation && ddlStructure.SelectedItem.Text.Trim().ToUpper() == Configuration.Complaint.BarrageHeadwork)
                {
                    Dropdownlist.SetSelectedValue(ddlZone, mdlComplaint.ZoneID.ToString());

                    ddlZone_SelectedIndexChanged(null, null);
                    ddlCircle.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlCircle, mdlComplaint.CircleID.ToString());

                    ddlCircle_SelectedIndexChanged(null, null);
                    ddlDivision.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlDivision, mdlComplaint.DivisionID.ToString());

                    ddlDivision_SelectedIndexChanged(null, null);
                    ddlBarrage.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlBarrage, mdlComplaint.StructureID.ToString());
                }

                Dropdownlist.SetSelectedValue(ddlDivisionTabZone, mdlComplaint.ZoneID.ToString());

                ddlDivisionTabZone_SelectedIndexChanged(null, null);
                ddlDivisionTabCircle.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabCircle, mdlComplaint.CircleID.ToString());

                ddlDivisionTabCircle_SelectedIndexChanged(null, null);
                ddlDivisionTabDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabDivision, mdlComplaint.DivisionID.ToString());

                ddlDivisionTabDivision_SelectedIndexChanged(null, null);
                ddlDivisionTabSubDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabSubDivision, mdlComplaint.SubDivID.ToString());

                ddlDivisionTabSubDivision_SelectedIndexChanged(null, null);
                ddlDivisionTabChannel.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabChannel, mdlComplaint.ChannelID.ToString());

                ddlDivisionTabChannel_SelectedIndexChanged(null, null);
                ddlDivisionTabOutlet.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabOutlet, mdlComplaint.StructureID.ToString());

                ddlDivisionTabProtectionStructure.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionTabProtectionStructure, mdlComplaint.StructureID.ToString());

                Tuple<string, string> tuple = Calculations.GetRDValues(mdlComplaint.RD);
                txtDivisionTabTotalRDLeft.Text = tuple.Item1;
                txtDivisionTabTotalRDRight.Text = tuple.Item2;

            }

            if (mdlComplaint.VillageID != null)
            {
                liVillage.Attributes.Add("class", "active");
                liDivision.Attributes.Remove("class");
                Village.Attributes.Add("class", "tab-pane active");
                Division.Attributes.Add("class", "tab-pane");

                Dropdownlist.SetSelectedValue(ddlVillageTabDistrict, mdlComplaint.DistrictID.ToString());

                ddlVillageTabDistrict_SelectedIndexChanged(null, null);
                ddlVillageTabTehsil.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabTehsil, mdlComplaint.TehsilID.ToString());

                ddlVillageTabTehsil_SelectedIndexChanged(null, null);
                ddlVillageTabVillage.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabVillage, mdlComplaint.VillageID.ToString());

                ddlVillageTabVillage_SelectedIndexChanged(null, null);
                ddlVillageTabChannel.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabChannel, mdlComplaint.ChannelID.ToString());

                ddlVillageTabChannel_SelectedIndexChanged(null, null);
                ddlVillageTabOutlet.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabOutlet, mdlComplaint.StructureID.ToString());

                ddlVillageTabOutlet_SelectedIndexChanged(null, null);
                ddlVillageTabProtectionStructure.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabProtectionStructure, mdlComplaint.StructureID.ToString());

                ddlVillageTabDrain.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabDrain, mdlComplaint.StructureID.ToString());

                ddlVillageTabDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlVillageTabDivision, mdlComplaint.DivisionID.ToString());

                Tuple<string, string> tuple = Calculations.GetRDValues(mdlComplaint.RD);
                txtVillageTabTotalRDLeft.Text = tuple.Item1;
                txtVillageTabTotalRDRight.Text = tuple.Item2;
            }

            if (mdlComplaint.ZoneID == null && mdlComplaint.VillageID == null)
            {
                ddlDivisionByDomain.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivisionByDomain, mdlComplaint.DivisionID.ToString());
            }

            ddlComplaintType.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlComplaintType, mdlComplaint.ComplaintTypeID.ToString());
            txtResponseDuration.Text = mdlComplaint.ResponseDuration.ToString();
            txtPmiuFileNo.Text = mdlComplaint.PMIUFileNo;
            txtComplaintDetails.Text = mdlComplaint.ComplaintDetails;

            if (!string.IsNullOrEmpty(mdlComplaint.Attachment))
            {
                //hlImage.NavigateUrl = Utility.GetImageURL(Configuration.Complaints, Convert.ToString(mdlComplaint.Attachment));
                //hlImage.Visible = true;
                string AttachmentPath = Convert.ToString(mdlComplaint.Attachment);
                List<string> lstName = new List<string>();
                lstName.Add(AttachmentPath);
                FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl1.Size = lstName.Count;
                FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.Complaints, lstName);
                FileUploadControl.Size = 0;

            }
            if (mdlComplaint.RefCode != null && mdlComplaint.RefCodeID != null)
            {
                lnkViewAttachments.Visible = true;
                hdnRefCode.Value = Convert.ToString(mdlComplaint.RefCodeID);
            }
            List<long?> lstDesignations = bllComplaints.GetDesignationsForListByComplaintID(mdlComplaint.ID);
            for (int i = 0; i < lstDesignations.Count; i++)
            {
                foreach (ListItem li in chkAdditionalAccessibility.Items)
                {
                    if (lstDesignations.ElementAt(i) == Convert.ToInt64(li.Value))
                    {
                        li.Selected = true;
                        //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
                        if (SessionManagerFacade.UserInformation.UA_Designations.ID != Convert.ToInt64(Constants.Designation.DeputyDirectorHelpline))
                        {
                            li.Enabled = false;
                        }
                    }
                }
            }

            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.HelplineOperator))
            {
                chkAdditionalAccessibility.Visible = false;
                lblAdditionalAccessibility.Visible = false;
                btnSave.Visible = false;
            }

            bool IsUpdated = bllComplaints.UpdateComplaintMarkedAndComplaint(_ComplaintID, SessionManagerFacade.UserInformation.ID, SessionManagerFacade.UserInformation.UA_Designations.ID);
            DisableTabControls(_ComplaintID);
        }

        //public long AddAutoGeneratedComplaint(string RefCode, long DivisionID = -1, long RefCodeID = -1)
        //{
        //CM_Complaint complaint = new CM_Complaint();
        //UA_Users mdlUser = SessionManagerFacade.UserInformation;
        //ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
        //CM_ComplaintAssignmentHistory mdlComplaintAssignmentHistory = new CM_ComplaintAssignmentHistory();
        //CM_ComplaintMarked mdlCompMarked = new CM_ComplaintMarked();
        ////  List<CM_ComplaintMarked> lstComplaintMarked = new List<CM_ComplaintMarked>();
        //dynamic lst = null;
        //long ComplaintType = 0;
        //complaint.ComplaintSourceID = (long)Constants.ComplaintSource.Automatic;
        //complaint.ComplainantName = mdlUser.FirstName + " " + mdlUser.LastName;
        //complaint.MobilePhone = mdlUser.MobilePhone;
        //complaint.Phone = mdlUser.LandLineNo;
        //complaint.ComplaintDate = Convert.ToDateTime(DateTime.Now);
        //complaint.RefCode = RefCode;
        //complaint.RefCodeID = RefCodeID;
        //if (DivisionID != -1)
        //{
        //    lst = bllComplaintsManagementBLL.GetSubDivisionCircleZoneAndUserIDByDivisionID(DivisionID, (long)Constants.IrrigationLevelID.Division);

        //}
        //if (RefCode.Trim().ToUpper() == "WT_C" || RefCode.Trim().ToUpper() == "WT_O" || RefCode.Trim().ToUpper() == "IF_WT")
        //{
        //    ComplaintType = 2;
        //}
        //else if (RefCode.Trim().ToUpper() == "IF_ST" || RefCode.Trim().ToUpper() == "SI_ST")
        //{
        //    ComplaintType = 4;
        //}
        //else if (RefCode.Trim().ToUpper() == "IF_DT" || RefCode.Trim().ToUpper() == "SI_DT")
        //{
        //    ComplaintType = 3;
        //}
        //else if (RefCode.Trim().ToUpper() == "IF_RV")
        //{
        //    ComplaintType = 5;
        //}
        //else if (RefCode.Trim().ToUpper() == "SI_HGNF")
        //{
        //    ComplaintType = 6;
        //}
        //else if (RefCode.Trim().ToUpper() == "SI_HGNP")
        //{
        //    ComplaintType = 7;
        //}
        //else if (RefCode.Trim().ToUpper() == "SI_TGNF")
        //{
        //    ComplaintType = 8;
        //}
        //else if (RefCode.Trim().ToUpper() == "SI_TGNP")
        //{
        //    ComplaintType = 9;
        //}
        //complaint.ComplaintTypeID = ComplaintType;
        //CM_ComplaintType mdlComplaintType = bllComplaintsManagementBLL.GetComplaintTypeByID(ComplaintType);
        //complaint.ResponseDuration = mdlComplaintType.ResponseTime;
        //complaint.DomainID = lst.GetType().GetProperty("DomainID").GetValue(lst, null);
        //complaint.ZoneID = lst.GetType().GetProperty("ZoneID").GetValue(lst, null);
        //complaint.CircleID = lst.GetType().GetProperty("CircleID").GetValue(lst, null);
        //complaint.DivisionID = lst.GetType().GetProperty("DivisionID").GetValue(lst, null);
        //complaint.SubDivID = lst.GetType().GetProperty("SubDivisionID").GetValue(lst, null);
        ////   complaint.ResponseDuration = 7;
        //complaint.CreatedDate = DateTime.Now;
        //complaint.ComplaintNumber = GetComplaintID(6);
        //complaint.UserID = mdlUser.ID;
        //complaint.UserDesigID = mdlUser.DesignationID;
        //complaint.ComplaintStatusID = 1;
        //complaint.AssignedToUser = lst.GetType().GetProperty("UserID").GetValue(lst, null);
        //complaint.AssignedToDesig = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
        //// Complaint Assignment History
        //mdlComplaintAssignmentHistory.ComplaintStatusID = 1;
        //mdlComplaintAssignmentHistory.AssignedByUser = mdlUser.ID;
        //mdlComplaintAssignmentHistory.AssignedByDesig = mdlUser.DesignationID;
        //mdlComplaintAssignmentHistory.AssignedToUser = lst.GetType().GetProperty("UserID").GetValue(lst, null);
        //mdlComplaintAssignmentHistory.AssignedToDesig = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
        //mdlComplaintAssignmentHistory.AssignedDate = DateTime.Now;
        //mdlComplaintAssignmentHistory.Remarks = "Autogentrated Complaint";
        //mdlComplaintAssignmentHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
        //mdlComplaintAssignmentHistory.CreatedDate = DateTime.Now;
        //// Complaint Marked
        //mdlCompMarked.UserID = lst.GetType().GetProperty("UserID").GetValue(lst, null);
        //mdlCompMarked.UserDesigID = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
        //mdlCompMarked.Starred = false;
        //mdlCompMarked.MarkRead = false;
        //mdlCompMarked.CreatedBy = Convert.ToInt32(mdlUser.ID);
        //mdlCompMarked.CreatedDate = DateTime.Now;



        //long ComplaintID;
        //using (TransactionScope transaction = new TransactionScope())
        //{
        //    ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaintData(complaint);
        //    mdlComplaintAssignmentHistory.ComplaintID = ComplaintID;
        //    mdlCompMarked.ComplaintID = ComplaintID;
        //    bool IsSaved = bllComplaintsManagementBLL.AddAutoGeneratedAssignmentHistoryData(mdlComplaintAssignmentHistory);
        //    bool IsSave = bllComplaintsManagementBLL.AddAutoGeneratedComplaintMarked(mdlCompMarked);
        //    transaction.Complete();
        //}
        ////DDH Entry
        //CM_ComplaintMarked mdlCompMarkedForDDH = new CM_ComplaintMarked();
        //List<long> DDH = bllComplaintsManagementBLL.GetUsersForPidaAndSecretariat((long)Constants.Designation.DeputyDirectorHelpline, (long)Constants.Organization.PMIU);
        //for (int i = 0; i < DDH.Count(); i++)
        //{
        //    mdlCompMarkedForDDH.UserID = DDH.ElementAt(i);
        //    mdlCompMarkedForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
        //    mdlCompMarkedForDDH.Starred = false;
        //    mdlCompMarkedForDDH.MarkRead = false;
        //    mdlCompMarkedForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
        //    mdlCompMarkedForDDH.CreatedDate = DateTime.Now;
        //    mdlCompMarkedForDDH.ComplaintID = ComplaintID;
        //    bllComplaintsManagementBLL.AddAutoGeneratedComplaintMarked(mdlCompMarkedForDDH);
        //}



        //  ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();

        //long ComplaintID;

        // ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplant(SessionManagerFacade.UserInformation.ID, RefCode, DivisionID, RefCodeID);

        //NotifyEvent _event = new NotifyEvent();
        //_event.Parameters.Add("ComplaintID", ComplaintID);
        //_event.AddNotifyEvent((long)NotificationEventConstants.Complaints.Complaintisloggedintothesystem, SessionManagerFacade.UserInformation.ID);

        //    return null;//ComplaintID;
        //}

        /// <summary>
        /// this function sets URL to Back Button and call GetComplaintByID function
        /// Created On: 19/10/2016
        /// </summary>
        private void BackNavigationFunction()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ComplaintDashboardID"]))
            {
                hlBack.NavigateUrl = "~/Modules/ComplaintsManagement/ComplaintsDashboard.aspx?ShowHistory=true";
                long CompID = Convert.ToInt64(Request.QueryString["ComplaintDashboardID"]);

                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline)
                {
                    GetComplaintByID(CompID);
                    DivForOthers.Attributes.Add("style", "display:none;");
                    divForDDH.Attributes.Add("style", "display:block;");


                    lblAddComplaint.Visible = false;
                    lblEditComplaint.Visible = true;
                    lblViewComplaint.Visible = false;
                }
                else
                {
                    GetComplaintDataByComplaintID(CompID);
                    GetComplaintByID(CompID);
                    DivForOthers.Attributes.Add("style", "display:block;");
                    divForDDH.Attributes.Add("style", "display:none;");


                    lblAddComplaint.Visible = false;
                    lblEditComplaint.Visible = false;
                    lblViewComplaint.Visible = true;
                }


            }
            else if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
            {
                hlBack.NavigateUrl = "~/Modules/ComplaintsManagement/SearchComplaints.aspx?ShowHistory=true";
                long CompID = Convert.ToInt64(Request.QueryString["ComplaintSearchID"]);
                long CompSourceID = Convert.ToInt64(Request.QueryString["ComplaintSource"]);

                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline && CompSourceID != 6)
                {
                    GetComplaintByID(CompID);
                    DivForOthers.Attributes.Add("style", "display:none;");
                    divForDDH.Attributes.Add("style", "display:block;");


                    lblAddComplaint.Visible = false;
                    lblEditComplaint.Visible = true;
                    lblViewComplaint.Visible = false;
                }
                else
                {
                    if (CompSourceID == 6)
                    {
                        GetComplaintDataByComplaintID(CompID);
                        GetComplaintByID(CompID);
                        DivForOthers.Attributes.Add("style", "display:block;");
                        divForDDH.Visible = false;


                        lblAddComplaint.Visible = false;
                        lblEditComplaint.Visible = false;
                        lblViewComplaint.Visible = true;
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.HelplineOperator
                            || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DataEntryOperator)
                        {
                            btnSave.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = base.CanEdit;
                        }
                    }
                    else
                    {
                        GetComplaintDataByComplaintID(CompID);
                        GetComplaintByID(CompID);
                        DivForOthers.Attributes.Add("style", "display:block;");
                        divForDDH.Attributes.Add("style", "display:none;");


                        lblAddComplaint.Visible = false;
                        lblEditComplaint.Visible = false;
                        lblViewComplaint.Visible = true;
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.HelplineOperator
                             || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DataEntryOperator)
                        {
                            btnSave.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = base.CanEdit;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["FromSearchComplaint"]))
            {
                hlBack.NavigateUrl = "~/Modules/ComplaintsManagement/SearchComplaints.aspx?ShowHistory=true";

                lblAddComplaint.Visible = true;
                lblEditComplaint.Visible = false;
                lblViewComplaint.Visible = false;

                btnSave.Visible = base.CanAdd;
            }
            else
            {
                hlBack.Visible = false;

                lblAddComplaint.Visible = true;
                lblEditComplaint.Visible = false;
                lblViewComplaint.Visible = false;

                btnSave.Visible = base.CanAdd;
            }
        }

        private void DisableFields()
        {
            ddlComplaintSource.Enabled = false;
            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.HelplineOperator))
            {
                // txtComplaintID.Enabled = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtMobileNo.Enabled = false;
                txtPhoneNo.Enabled = false;
                txtComplaintDate.Enabled = false;
                txtAddress.Enabled = false;
                ddlDomain.Enabled = false;
                ddlDivisionByDomain.Enabled = false;
                ddlStructure.Enabled = false;

                ddlDivisionTabZone.Enabled = false;
                ddlDivisionTabCircle.Enabled = false;
                ddlDivisionTabDivision.Enabled = false;
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabDrain.Enabled = false;
                ddlDivisionTabOutlet.Enabled = false;

                ddlVillageTabDistrict.Enabled = false;
                ddlVillageTabTehsil.Enabled = false;
                ddlVillageTabVillage.Enabled = false;
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabDrain.Enabled = false;
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabDivision.Enabled = false;

                ddlZone.Enabled = false;
                ddlCircle.Enabled = false;
                ddlDivision.Enabled = false;
                ddlBarrage.Enabled = false;
                ddlSmallDams.Enabled = false;
                ddlChannel.Enabled = false;

                ddlComplaintType.Enabled = false;
                txtResponseDuration.Enabled = false;
                txtPmiuFileNo.Enabled = false;
                txtComplaintDetails.Enabled = false;

                chkAdditionalAccessibility.Visible = false;

                lblAdditionalAccessibility.Visible = false;
            }
            else if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
            {
                // txtComplaintID.Enabled = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtMobileNo.Enabled = false;
                txtPhoneNo.Enabled = false;
                txtComplaintDate.Enabled = false;
                txtAddress.Enabled = false;
                ddlDomain.Enabled = false;
                ddlDivisionByDomain.Enabled = false;
                ddlStructure.Enabled = false;

                ddlDivisionTabZone.Enabled = false;
                ddlDivisionTabCircle.Enabled = false;
                ddlDivisionTabDivision.Enabled = false;
                ddlDivisionTabSubDivision.Enabled = false;
                ddlDivisionTabProtectionStructure.Enabled = false;
                ddlDivisionTabChannel.Enabled = false;
                ddlDivisionTabDrain.Enabled = false;
                ddlDivisionTabOutlet.Enabled = false;

                ddlVillageTabDistrict.Enabled = false;
                ddlVillageTabTehsil.Enabled = false;
                ddlVillageTabVillage.Enabled = false;
                ddlVillageTabChannel.Enabled = false;
                ddlVillageTabProtectionStructure.Enabled = false;
                ddlVillageTabDrain.Enabled = false;
                ddlVillageTabOutlet.Enabled = false;
                ddlVillageTabDivision.Enabled = false;

                ddlZone.Enabled = false;
                ddlCircle.Enabled = false;
                ddlDivision.Enabled = false;
                ddlBarrage.Enabled = false;
                ddlSmallDams.Enabled = false;
                ddlChannel.Enabled = false;

                ddlComplaintType.Enabled = false;
                txtResponseDuration.Enabled = false;
                txtPmiuFileNo.Enabled = false;
                txtComplaintDetails.Enabled = false;
            }
        }

        protected void ddlComplaintType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
                long ComplaintTypeID = Convert.ToInt64(ddlComplaintType.SelectedItem.Value);
                CM_ComplaintType mdlComplaintType = bllComplaints.GetComplaintTypeByID(ComplaintTypeID);
                txtResponseDuration.Text = Convert.ToString(mdlComplaintType.ResponseTime);
                txtComplaintDetails.Text = mdlComplaintType.Description;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddComplaint);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void DisableTabControls(long _ComplaintID)
        {
            ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
            CM_Complaint mdlComplaint = bllComplaints.GetComplaintByID(_ComplaintID);

            if (mdlComplaint.ZoneID != null)
            {
                lblValueForTab.Text = "Division";

                ddlVillageTabDistrict.Attributes.Remove("required");
                ddlVillageTabTehsil.Attributes.Remove("required");
                ddlVillageTabVillage.Attributes.Remove("required");
                ddlVillageTabChannel.Attributes.Remove("required");
                ddlVillageTabProtectionStructure.Attributes.Remove("required");
                ddlVillageTabDrain.Attributes.Remove("required");
                ddlVillageTabOutlet.Attributes.Remove("required");
                ddlVillageTabDivision.Attributes.Remove("required");
            }
            else if (mdlComplaint.DistrictID != null)
            {
                lblValueForTab.Text = "Village";

                ddlDivisionTabZone.Attributes.Remove("required");
                ddlDivisionTabCircle.Attributes.Remove("required");
                ddlDivisionTabDivision.Attributes.Remove("required");
                ddlDivisionTabSubDivision.Attributes.Remove("required");
                ddlDivisionTabProtectionStructure.Attributes.Remove("required");
                ddlDivisionTabChannel.Attributes.Remove("required");
                ddlDivisionTabDrain.Attributes.Remove("required");
                ddlDivisionTabOutlet.Attributes.Remove("required");
            }
            ddlComplaintSource.Enabled = false;
            ddlComplaintSource.CssClass = "form-control";
        }

        protected void GetComplaintDataByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            dynamic ComplaintInformation = bllComplaint.GetComplaintInformationByComplaintID(_ComplaintID);
            lblDivForOthersComplaintNumber.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintNumber");
            lblDivForOthersComplaintSource.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintSource");
            lblDivForOthersDomain.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "Domain");

            lblDivForOthersDivision.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "Division");
            DateTime date = Convert.ToDateTime(Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintDate"));
            lblDivForOthersComplaintDate.Text = Convert.ToString(Utility.GetFormattedDate(date));
            lblDivForOthersComplaintName.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplainantName");
            lblDivForOthersComplaintType.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintType");
            lblDivForOthersCell.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaiantCell");
            lblDivForOthersAddress.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplainantAddress");
            lblDivForOthersComplaintDetails.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintDetails");
            lblDivForOthersPMIUFileNo.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "PMIUFileNo");
            lblDivForOthersResponseDuration.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ResponseDuration");
            lblDivForOthersComplaintStatus.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "Status");
            string Attachment = Utility.GetDynamicPropertyValue(ComplaintInformation, "Attachment");
            if (!string.IsNullOrEmpty(Attachment))
            {
                //hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.Complaints, Attachment);
                //hlAttachment.Visible = true;
                string AttachmentPath = Convert.ToString(Attachment);
                List<string> lstName = new List<string>();
                lstName.Add(AttachmentPath);
                FileUploadControl2.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl2.Size = lstName.Count;
                FileUploadControl2.ViewUploadedFilesAsThumbnail(Configuration.Complaints, lstName);
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
                List<dynamic> lstWTCaseWorking = new WaterTheftBLL().GetWaterTheftCaseAttachment(Convert.ToInt64(hdnRefCode.Value));
                gvViewAttachment.DataSource = lstWTCaseWorking;
                gvViewAttachment.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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
                WebFormsTest.FileUploadControl FileUploadControl = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl4");
                FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl.Size = lstName.Count;
                FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.WaterTheft, lstName);


            }
        }
        private void ClearSelections()
        {
            ddlComplaintSource.ClearSelection();
            txtComplaintID.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtMobileNo.Text = String.Empty;
            txtPhoneNo.Text = String.Empty;
            txtComplaintDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
            txtAddress.Text = String.Empty;

            ddlDomain.ClearSelection();
            ddlDivisionByDomain.ClearSelection();
            ddlStructure.ClearSelection();

            ddlDivisionTabZone.ClearSelection();
            ddlDivisionTabCircle.ClearSelection();
            ddlDivisionTabDivision.ClearSelection();
            ddlDivisionTabSubDivision.ClearSelection();
            ddlDivisionTabProtectionStructure.ClearSelection();
            ddlDivisionTabChannel.ClearSelection();
            ddlDivisionTabDrain.ClearSelection();
            ddlDivisionTabOutlet.ClearSelection();
            txtDivisionTabTotalRDLeft.Text = string.Empty;
            txtDivisionTabTotalRDRight.Text = string.Empty;

            ddlVillageTabDistrict.ClearSelection();
            ddlVillageTabTehsil.ClearSelection();
            ddlVillageTabVillage.ClearSelection();
            ddlVillageTabChannel.ClearSelection();
            ddlVillageTabProtectionStructure.ClearSelection();
            ddlVillageTabDrain.ClearSelection();
            ddlVillageTabOutlet.ClearSelection();
            ddlVillageTabDivision.ClearSelection();
            txtVillageTabTotalRDLeft.Text = string.Empty;
            txtVillageTabTotalRDRight.Text = string.Empty;

            ddlZone.ClearSelection();
            ddlCircle.ClearSelection();
            ddlDivision.ClearSelection();
            ddlBarrage.ClearSelection();
            ddlSmallDams.ClearSelection();
            ddlChannel.ClearSelection();

            ddlComplaintType.ClearSelection();
            txtResponseDuration.Text = String.Empty;
            txtPmiuFileNo.Text = String.Empty;
            txtComplaintDetails.Text = String.Empty;
            FileUploadControl.Dispose();

            foreach (ListItem li in chkAdditionalAccessibility.Items)
            {
                li.Selected = false;
            }

            divWithoutTab.Visible = false;
            divPanel.Visible = false;
        }
    }
}