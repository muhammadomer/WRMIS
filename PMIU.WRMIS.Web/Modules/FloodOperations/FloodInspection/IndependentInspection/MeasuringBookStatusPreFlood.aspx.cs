using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class MeasuringBookStatusPreFlood : BasePage
    {
        #region Initialize

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdown();
                    int floodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);

                    if (floodInspectionID > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = floodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID)).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                        //  LoadIGCProtectionInfrastructure(floodInspectionID);
                        UserRoleCanEdit();
                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Initialize

        #region Functions

        private void UserRoleCanEdit()
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
            {
                btnSave.Enabled = false;
                gvMeasuringBookStatus.Enabled = false;
            }
            if (_InspectionTypeID == 1)
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                if (CanEdit)
                {
                    btnSave.Enabled = CanEdit;
                    gvMeasuringBookStatus.Enabled = CanEdit;
                }
                else
                {
                    btnSave.Enabled = false;
                    gvMeasuringBookStatus.Enabled = false;
                }
            }
            else
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                if (CanEdit)
                {
                    btnSave.Enabled = CanEdit;
                    gvMeasuringBookStatus.Enabled = CanEdit;
                }
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdown()
        {
            Dropdownlist.DDLItemCategoryWithOutAsset(ddlCategory, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindMeasuringBookStatusPreFloodGrid(long _CategoryID, long floodInspectionID)
        {
            try
            {
                object Obj = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                if (Obj != null)
                {
                    int _pYear = Convert.ToInt32(Utility.GetDynamicPropertyValue(Obj, "year"));
                    long _pDivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "DivisionID"));
                    long _pStructureTypeID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "StructureTypeID"));
                    long _pStructureID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "StructureID"));

                    //List<object> lstPreMBStatusSearch = new FloodInspectionsBLL().GetMBStatusPreItemList(_CategoryID, floodInspectionID);

                    IEnumerable<DataRow> lstPreMBStatusSearch = new FloodInspectionsBLL().GetPreMBStatus(_pDivisionID, _pYear, Convert.ToInt16(ddlCategory.SelectedValue), _pStructureTypeID, _pStructureID);

                    List<object> lstGetPreMBLoad = lstPreMBStatusSearch.Select(dataRow => new
                    {
                        PreMBStatusID = dataRow.Field<long?>("PreMBStatusID") == null ? 0 : dataRow.Field<long?>("PreMBStatusID"),
                        ItemID = dataRow.Field<long>("ItemID"),
                        ItemName = dataRow.Field<string>("ItemName"),
                        LYQty = dataRow.Field<int?>("LYQty") == null ? 0 : dataRow.Field<int?>("LYQty"),
                        DIVIssueQty = dataRow.Field<int?>("DIVIssueQty") == null ? 0 : dataRow.Field<int?>("DIVIssueQty"),
                        AvaQty = dataRow.Field<int?>("AvaQty") == null ? 0 : dataRow.Field<int?>("AvaQty"),
                        BalanceQty = dataRow.Field<int?>("BalanceQty") == null ? 0 : dataRow.Field<int?>("BalanceQty"),
                    }).ToList<object>();

                    gvMeasuringBookStatus.DataSource = lstGetPreMBLoad;
                    gvMeasuringBookStatus.DataBind();
                    gvMeasuringBookStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadIGCProtectionInfrastructure(long _FloodInspectionID)
        {
            FO_IGCProtectionInfrastructure iGCProtectionInfrastructure = new FloodInspectionsBLL().GetIGCProtectionInfrastructureByInspectionID(_FloodInspectionID);
        }

        #endregion Functions

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsSave = false;
            for (int m = 0; m < gvMeasuringBookStatus.Rows.Count; m++)
            {
                try
                {
                    string PreMBStatusID = Convert.ToString(gvMeasuringBookStatus.DataKeys[m].Values["PreMBStatusID"]);
                    string ItemsID = Convert.ToString(gvMeasuringBookStatus.DataKeys[m].Values["ItemID"]);
                    string LYQty = Convert.ToString(gvMeasuringBookStatus.DataKeys[m].Values["LYQty"]);
                    string DIVIssueQty = Convert.ToString(gvMeasuringBookStatus.DataKeys[m].Values["DIVIssueQty"]);
                    string AvaQty = Convert.ToString(gvMeasuringBookStatus.DataKeys[m].Values["AvaQty"]);

                    TextBox txtQuantityAvalible = (TextBox)gvMeasuringBookStatus.Rows[m].FindControl("txtQuantityAvalible");
                    if (Convert.ToInt64(PreMBStatusID) != 0)
                    {
                        if (txtQuantityAvalible.Text != "" && AvaQty != txtQuantityAvalible.Text)
                        {
                            new FloodInspectionsBLL().PreMBStatusInsertion(Convert.ToInt64(PreMBStatusID), Convert.ToInt64(hdnFloodInspectionsID.Value), Convert.ToInt64(ItemsID), Convert.ToInt32(LYQty), Convert.ToInt32(DIVIssueQty),
                                Convert.ToInt32(txtQuantityAvalible.Text), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                    else
                    {
                        if (txtQuantityAvalible.Text != "")// --&& AvaQty != txtQuantityAvalible.Text)
                        {
                            new FloodInspectionsBLL().PreMBStatusInsertion(0, Convert.ToInt64(hdnFloodInspectionsID.Value), Convert.ToInt64(ItemsID), Convert.ToInt32(LYQty), Convert.ToInt32(DIVIssueQty),
                                Convert.ToInt32(txtQuantityAvalible.Text), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
            try
            {
                if (IsSave == true)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindMeasuringBookStatusPreFloodGrid(Convert.ToInt16(ddlCategory.SelectedValue), Convert.ToInt64(hdnFloodInspectionsID.Value));
                    // gvMeasuringBookStatus.Visible = false;
                    //BindDropdown();
                    //btnSave.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategory.SelectedIndex == 0)
                {
                    gvMeasuringBookStatus.Visible = false;
                }
                else
                {
                    BindMeasuringBookStatusPreFloodGrid(Convert.ToInt64(ddlCategory.SelectedItem.Value), Convert.ToInt64(hdnFloodInspectionsID.Value));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //protected void TextChangedEvent(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //        int index = gvRow.RowIndex;
        //        string DSQty = (gvMeasuringBookStatus.Rows[index].FindControl("lblQuantityIssuedFromDivisionStore") as Label).Text;
        //        string Ava = (gvMeasuringBookStatus.Rows[index].FindControl("txtQuantityAvalible") as TextBox).Text;

        //        if (Ava != "")
        //        {
        //            (gvMeasuringBookStatus.Rows[index].FindControl("lblBalanceQuantity") as Label).Text = (Convert.ToInt32(DSQty) - Convert.ToInt32(Ava)).ToString();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void gvMeasuringBookStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtQuantityAvalible");
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    //if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                    //{
                    //    btnSave.Visible = true;
                    //}

                    txt.Enabled = false;
                }
                if (_InspectionTypeID == 1)
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                    if (CanEdit)
                    {
                        txt.Enabled = CanEdit;
                    }
                    else
                    {
                        txt.Enabled = false;
                    }
                }
                else
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                    if (CanEdit)
                    {
                        txt.Enabled = CanEdit;
                    }
                }
            }
        }

        #endregion Events
    }
}