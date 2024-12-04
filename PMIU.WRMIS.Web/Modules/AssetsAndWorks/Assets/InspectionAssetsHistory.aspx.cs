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
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;
using PMIU.WRMIS.BLL.UserAdministration;
using System.Data;
using PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls;
using System.Reflection;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets
{
    public partial class InspectionAssetsHistory : BasePage
    {
        AssetsWorkBLL BLLAsset = new AssetsWorkBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdown();
                    int AssetsID = Utility.GetNumericValueFromQueryString("AssetsID", 0);
                    if (AssetsID > 0)
                    {
                        AssetsDetail._AssetsID = AssetsID;
                        hdnAssetsID.Value = Convert.ToString(AssetsID);


                        if (Utility.GetStringValueFromQueryString("AssetType", "") == "Lot")
                        {
                            DivStatus.Visible = false;
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                        {
                            SetControlsValues();
                            if (Utility.GetStringValueFromQueryString("AssetType", "") == "Individual Item")
                            {
                                BindGrid();
                                DivInd.Visible = true;
                                DivLot.Visible = false;
                            }
                            else
                            {
                                BindGridLot();
                                DivInd.Visible = false;
                                DivLot.Visible = true;
                            }
                        }
                        else
                        {
                            txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-30));
                            txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            if (Utility.GetStringValueFromQueryString("AssetType", "") == "Individual Item")
                            {
                                BindGrid();
                                DivInd.Visible = true;
                                DivLot.Visible = false;
                            }
                            else
                            {
                                BindGridLot();
                                DivInd.Visible = false;
                                DivLot.Visible = true;
                            }
                            SaveSearchCriteriaInSession();
                        }
                        btnShow.Enabled = base.CanView;
                        hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Assets/SearchAssets.aspx?RestoreState=1");
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region Set Page Title
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title
        #region Dropdown Lists Binding
        private void BindDropdown()
        {
            try
            {
                //if (Utility.GetStringValueFromQueryString("AssetType", "") == "")
                //    Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
                //else
                if ((long)SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.AccountOfficer)
                    Dropdownlist.DDLAssetInspectionStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
                else
                    Dropdownlist.DDLAssetInspectionAllStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);

                LoadDropdowns();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void LoadDropdowns()
        {
            long designationID = (long)SessionManagerFacade.UserInformation.DesignationID;
            List<string> lstNames = new List<string>();
            //if (SessionManagerFacade.UserAssociatedLocations.UserID == 0)
            //{
            //    ddlInspectedBy.Items.Clear();
            //    ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
            //    ddlInspectedBy.Items.Insert(2, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
            //    ddlInspectedBy.Items.Insert(3, new ListItem("SE", "" + (long)Constants.Designation.SE));
            //    ddlInspectedBy.Items.Insert(4, new ListItem("CE", "" + (long)Constants.Designation.ChiefIrrigation));
            //}
            //else
            //{
            if (designationID == (long)Constants.Designation.XEN)
            {
                ddlInspectedBy.Items.Clear();
                ddlInspectedBy.Items.Insert(0, new ListItem(BLLAsset.GetDesignationByID(designationID), "" + designationID));
            }
            else if (designationID == (long)Constants.Designation.SE)
            {
                ddlInspectedBy.Items.Clear();

               // ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                ddlInspectedBy.Items.Insert(0, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
               // ddlInspectedBy.Items.Insert(2, new ListItem("SE", "" + (long)Constants.Designation.SE));

            }
            else if (designationID == (long)Constants.Designation.ChiefIrrigation)
            {
                ddlInspectedBy.Items.Clear();

               // ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                ddlInspectedBy.Items.Insert(0, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
              //  ddlInspectedBy.Items.Insert(2, new ListItem("SE", "" + (long)Constants.Designation.SE));
              //  ddlInspectedBy.Items.Insert(3, new ListItem("CE", "" + (long)Constants.Designation.ChiefIrrigation));
            }
            //forPMIU
            else if (designationID == (long)Constants.Designation.ADM || designationID == (long)Constants.Designation.MA)
            {
                ddlInspectedBy.Items.Clear();

                ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                ddlInspectedBy.Items.Insert(1, new ListItem("ADM", "" + (long)Constants.Designation.ADM));
                ddlInspectedBy.Items.Insert(2, new ListItem("MA", "" + (long)Constants.Designation.MA));
            }
            else if (designationID == (long)Constants.Designation.AccountOfficer)
            {
                ddlInspectedBy.Items.Clear();

              //  ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                ddlInspectedBy.Items.Insert(0, new ListItem("AccountOfficer", "" + (long)Constants.Designation.AccountOfficer));

            }
            else if (designationID == (long)Constants.Designation.DataAnalyst)
            {
                ddlInspectedBy.Items.Clear();

               // ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                ddlInspectedBy.Items.Insert(0, new ListItem("DataAnalyst", "" + (long)Constants.Designation.DataAnalyst));
            }
            //}
        }
        #endregion Dropdown Lists Binding
        #region GridviewPopulateInd
        private void BindGrid()
        {
            try
            {
                DateTime? fromDate = null;
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    fromDate = Utility.GetParsedDate(txtFromDate.Text);

                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(txtToDate.Text))
                    toDate = Utility.GetParsedDate(txtToDate.Text);

                string StatusID = "All";
                if (!string.IsNullOrEmpty(ddlStatus.SelectedItem.Value))
                {

                    StatusID = ddlStatus.SelectedItem.Text;

                }


                List<long> inspectedBy = new List<long>();
                if (string.IsNullOrEmpty(ddlInspectedBy.SelectedItem.Value))
                {
                    foreach (ListItem i in ddlInspectedBy.Items)
                    {
                        if (!string.IsNullOrEmpty(i.Value))
                        {
                            inspectedBy.Add(Convert.ToInt64(i.Value));
                        }
                    }

                }
                else
                    inspectedBy.Add(Convert.ToInt64(ddlInspectedBy.SelectedItem.Value));
                List<object> lstData = BLLAsset.GetIndividualHistoryInd(StatusID, inspectedBy, Convert.ToInt64(hdnAssetsID.Value), fromDate, toDate);
                gvInspectionHistoryIndividual.DataSource = lstData;
                gvInspectionHistoryIndividual.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInspectionHistoryIndividual_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvInspectionHistoryIndividual.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInspectionHistoryIndividual_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInspectionHistoryIndividual.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvInspectionHistoryIndividual_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("InspectionInd"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int id = Convert.ToInt32(gvInspectionHistoryIndividual.DataKeys[row.RowIndex].Values[0]);
                    string URL = "~/Modules/AssetsAndWorks/Assets/InspectionAssetsIndividual.aspx?AssetsID=" + hdnAssetsID.Value + "&InspectionIndID=" + id + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", "");
                    Response.Redirect(URL, false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion GridviewPopulateInd

        #region GridviewPopulateLot
        private void BindGridLot()
        {
            try
            {
                DateTime? fromDate = null;
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    fromDate = Utility.GetParsedDate(txtFromDate.Text);

                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(txtToDate.Text))
                    toDate = Utility.GetParsedDate(txtToDate.Text);

                List<long> inspectedBy = new List<long>();
                if (string.IsNullOrEmpty(ddlInspectedBy.SelectedItem.Value))
                {
                    foreach (ListItem i in ddlInspectedBy.Items)
                    {
                        if (!string.IsNullOrEmpty(i.Value))
                        {
                            inspectedBy.Add(Convert.ToInt64(i.Value));
                        }
                    }

                }
                else
                    inspectedBy.Add(Convert.ToInt64(ddlInspectedBy.SelectedItem.Value));
                List<object> lstData = BLLAsset.GetIndividualHistoryLot(inspectedBy, Convert.ToInt64(hdnAssetsID.Value), fromDate, toDate);
                gvInspectionHistoryLot.DataSource = lstData;
                gvInspectionHistoryLot.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInspectionHistoryLot_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvInspectionHistoryLot.EditIndex = -1;
                BindGridLot();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvInspectionHistoryLot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInspectionHistoryLot.PageIndex = e.NewPageIndex;
                BindGridLot();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvInspectionHistoryLot_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("InspectionLot"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int id = Convert.ToInt32(gvInspectionHistoryLot.DataKeys[row.RowIndex].Values[0]);
                    string URL = "~/Modules/AssetsAndWorks/Assets/InspectionAssetsLot.aspx?AssetsID=" + hdnAssetsID.Value + "&InspectionLotID=" + id + "&AssetType=" + Utility.GetStringValueFromQueryString("AssetType", "");
                    Response.Redirect(URL, false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion GridviewPopulateLot
        private bool DateRangesCheck()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
                    return true;
                DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    fromDate = Utility.GetParsedDate(txtFromDate.Text);
                    if (fromDate > DateTime.Now)
                    {
                        Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    toDate = Utility.GetParsedDate(txtToDate.Text);
                    if (toDate > DateTime.Now)
                    {
                        Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                {
                    if (fromDate > toDate)
                    {
                        Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                        return false;
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            return true;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (Utility.GetStringValueFromQueryString("AssetType", "") == "Individual Item")
            {
                if (!DateRangesCheck())
                {
                    DivInd.Visible = false;
                    DivLot.Visible = false;
                    return;
                }
                else
                {
                    BindGrid();
                    DivInd.Visible = true;
                    DivLot.Visible = false;
                }
            }
            else
            {
                if (!DateRangesCheck())
                {
                    DivInd.Visible = false;
                    DivLot.Visible = false;
                    return;
                }
                else
                {
                    BindGridLot();
                    DivInd.Visible = false;
                    DivLot.Visible = true;
                }
            }
            SaveSearchCriteriaInSession();
        }
        #region SearchCriteria
        protected void SaveSearchCriteriaInSession()
        {
            object obj = new
            {
                InspectedBy = ddlInspectedBy.SelectedItem.Value,
                FromDate = txtFromDate.Text,
                ToDate = txtToDate.Text,
                StatusID = ddlStatus.SelectedItem.Value
            };
            Session["IAH_SC_SearchCriteria"] = obj;
        }

        protected void SetControlsValues()
        {
            object currentObj = Session["IAH_SC_SearchCriteria"] as object;
            if (currentObj != null)
            {
                if (Convert.ToString(currentObj.GetType().GetProperty("FromDate").GetValue(currentObj)) != "")
                    txtFromDate.Text = Convert.ToString(currentObj.GetType().GetProperty("FromDate").GetValue(currentObj));

                if (Convert.ToString(currentObj.GetType().GetProperty("ToDate").GetValue(currentObj)) != "")
                    txtToDate.Text = Convert.ToString(currentObj.GetType().GetProperty("ToDate").GetValue(currentObj));

                if (Convert.ToString(currentObj.GetType().GetProperty("InspectedBy").GetValue(currentObj)) != "")
                    ddlInspectedBy.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("InspectedBy").GetValue(currentObj));

                if (Convert.ToString(currentObj.GetType().GetProperty("StatusID").GetValue(currentObj)) != "")
                    ddlStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("StatusID").GetValue(currentObj));
            }

        }
        #endregion SearchCriteria
    }
}