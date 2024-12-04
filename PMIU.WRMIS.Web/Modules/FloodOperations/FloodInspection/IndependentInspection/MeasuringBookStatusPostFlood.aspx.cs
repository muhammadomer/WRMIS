using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class MeasuringBookStatusPostFlood : BasePage
    {
        public static long _DivisionID;
        public static long _StructureTypeID;
        public static long _StructureID;
        public static int _Year;

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
                    // hdnProtectioninfrastructure.Value = floodInspectionsID;

                    if (floodInspectionID > 0)
                    {
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        FloodInspectionDetail1.FloodInspectionIDProp = Convert.ToInt64(hdnFloodInspectionsID.Value);
                        hlBack.NavigateUrl =
                            string.Format(
                                "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}",
                                floodInspectionID);
                        //   BindMeasuringBookStatusPostFloodGrid(Convert.ToInt64(ddlCategory.SelectedValue), floodInspectionID);
                        hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID)).ToString();
                        btnSave.Enabled = false;
                        gvMeasuringBookStatusPost.Enabled = false;
                        UserRoleCanEdit();


                        //if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        //{
                        //    btnSave.Enabled = false;
                        //    gvMeasuringBookStatusPost.Enabled = false;
                        //}

                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //BindParentTypeDropDown();
                    //CheckParentName(Convert.ToInt64(ParentInfrastructureID));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Initialize

        #region Functions

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void UserRoleCanEdit()
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
            {
                btnSave.Enabled = false;
                gvMeasuringBookStatusPost.Enabled = false;
            }
            if (_InspectionTypeID == 1)
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                if (CanEdit)
                {
                    btnSave.Enabled = CanEdit;
                    gvMeasuringBookStatusPost.Enabled = CanEdit;
                }
                else
                {
                    btnSave.Enabled = false;
                    gvMeasuringBookStatusPost.Enabled = false;
                }
            }
            else
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                if (CanEdit)
                {
                    btnSave.Enabled = CanEdit;
                    gvMeasuringBookStatusPost.Enabled = CanEdit;
                }
            }

        }

        private void BindDropdown()
        {
            Dropdownlist.DDLItemCategoryWithOutAsset(ddlCategory, false, (int)Constants.DropDownFirstOption.Select);
        }

        private void BindMeasuringBookStatusPostFloodGrid(Int16 _ItemCategoryID, long _FloodinspectioID)
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

                    DataSet ieGetPostMBLoad = new FloodInspectionsBLL().GetPostMBStatus(_pDivisionID, _pYear, Convert.ToInt16(ddlCategory.SelectedValue), _pStructureTypeID, _pStructureID);
                    //var lstGetPostMBLoad = ieGetPostMBLoad.Select(dataRow => new
                    //{
                    //    OverallDivItemsID = dataRow.Field<long>("OverallDivItemsID"),
                    //    ItemsID = dataRow.Field<long>("ItemsID"),
                    //    ItemName = dataRow.Field<string>("ItemName"),
                    //    QtyPreFloodInspection =
                    //    dataRow.Field<int?>("QtyPreFloodInspection") == null
                    //        ? 0
                    //        : dataRow.Field<int?>("QtyPreFloodInspection"),
                    //    QuantityAvailable =
                    //    dataRow.Field<int?>("QuantityAvailable") == null ? 0 : dataRow.Field<int?>("QuantityAvailable"),
                    //    QuantityRequired =
                    //    dataRow.Field<int?>("QuantityRequired") == null ? 0 : dataRow.Field<int?>("QuantityRequired"),
                    //}).ToList();

                    gvMeasuringBookStatusPost.DataSource = ieGetPostMBLoad;
                    gvMeasuringBookStatusPost.DataBind();
                    gvMeasuringBookStatusPost.Visible = true;
                }

                //List<object> lstPostMBStatusSearch = new FloodInspectionsBLL().GetMBStatusPostItemList(_CategoryID, floodInspectionID);

                //gvMeasuringBookStatusPost.DataSource = lstPostMBStatusSearch;
                //gvMeasuringBookStatusPost.DataBind();
                //gvMeasuringBookStatusPost.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //private void LoadIGCProtectionInfrastructure(long _FloodInspectionID)
        //{
        //    FO_IGCProtectionInfrastructure iGCProtectionInfrastructure = new FloodInspectionsBLL().GetIGCProtectionInfrastructureByInspectionID(_FloodInspectionID);
        //}

        #endregion Functions

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ////--To DO FO Team
            ////FO_PostMBStatus _ObjModel = new FO_PostMBStatus();

            //int count = 0;
            //bool NewUpdate = false;
            //UA_Users mdlUser = SessionManagerFacade.UserInformation;
            //foreach (GridViewRow row in gvMeasuringBookStatusPost.Rows)
            //{
            //    _ObjModel.ID = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
            //    if (Obj != null)
            //    {
            //        _ObjModel.FloodInspectionDetailID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "FloodInspectionDetailID"));
            //        DataSet DS = new FloodInspectionsBLL().GetPreMBStatusID(_Year, _DivisionID, _StructureTypeID, _StrctureID);
            //        if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow DR = DS.Tables[0].Rows[0];
            //            _ObjModel.PreMBStatusID = Convert.ToInt64(DR["PreMBStatusID"]);
            //        }
            //    }
            //    _ObjModel.ItemSubcategoryID = Convert.ToInt64(((Label)row.FindControl("lblItemID")).Text);
            //    //   _ObjModel.PreFloodQty = Convert.ToInt32(((Label)row.FindControl("lblQuantityPreFloodInspections")).Text);
            //    _ObjModel.CreatedBy = Convert.ToInt32(mdlUser.ID);
            //    _ObjModel.CreatedDate = System.DateTime.Now;
            //    if (_ObjModel.ID != 0)
            //    {
            //        NewUpdate = true;
            //    }
            //    if (NewUpdate)
            //    {
            //        _ObjModel.ModifiedDate = System.DateTime.Now;
            //        _ObjModel.ModifiedBy = Convert.ToInt32(mdlUser.ID);
            //    }
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        if (((TextBox)row.FindControl("txtQuantityAvalible")) != null)
            //        {
            //            _ObjModel.PostAvailableQty = Convert.ToInt32(((TextBox)row.FindControl("txtQuantityAvalible")).Text);
            //        }
            //        if (((TextBox)row.FindControl("txtQuantityRequired")) != null)
            //        {
            //            _ObjModel.PostRequiredQty = Convert.ToInt32(((TextBox)row.FindControl("txtQuantityRequired")).Text);
            //        }

            //    }

            //    FloodInspectionsBLL ObjBL = new FloodInspectionsBLL();

            //    bool res = ObjBL.SavePostMBStatus(_ObjModel);
            //    if (res)
            //    {
            //        count++;
            //    }
            //    else
            //    {
            //        count--;
            //    }

            //}

            //if (gvMeasuringBookStatusPost.Rows.Count == count)
            //{
            //    if (NewUpdate)
            //    {
            //        Master.ShowMessage("Measuring Book Status Post Flood updated successfully. ", SiteMaster.MessageType.Success);
            //    }
            //    else
            //    {
            //        Master.ShowMessage("Measuring Book Status Post Flood saved successfully. ", SiteMaster.MessageType.Success);
            //    }

            //    ddlCategory.SelectedIndex = 0;

            //    //gvMeasuringBookStatusPost.DataSource = null;
            //    //gvMeasuringBookStatusPost.DataBind();
            //    //gvMeasuringBookStatusPost.Visible = false;

            //    BindMeasuringBookStatusPostFloodGrid(Convert.ToInt16(ddlCategory.SelectedValue), Convert.ToInt64(hdnFloodInspectionsID.Value));
            //    gvMeasuringBookStatusPost.Visible = false;
            //    btnSave.Enabled = false;
            //}
            //else
            //{
            //    Master.ShowMessage("Measuring Book Status Pre Flood not save. ", SiteMaster.MessageType.Error);
            //}

            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            FloodFightingPlanBLL bllFFPBLL = new FloodFightingPlanBLL();
            long? FloodInspectionDetailID = 0;
            long? PreMBStatusID = null;
            object Obj =
                new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
            bool IsSave = false;
            for (int m = 0; m < gvMeasuringBookStatusPost.Rows.Count; m++)
            {
                try
                {
                    string OverallDivItemsID = Convert.ToString(gvMeasuringBookStatusPost.DataKeys[m].Values["OverallDivItemsID"]);
                    string ItemsID = Convert.ToString(gvMeasuringBookStatusPost.DataKeys[m].Values["ItemsID"]);
                    string QuantityAvailable = Convert.ToString(gvMeasuringBookStatusPost.DataKeys[m].Values["QuantityAvailable"]);
                    string QuantityRequired = Convert.ToString(gvMeasuringBookStatusPost.DataKeys[m].Values["QuantityRequired"]);

                    if (Obj != null)
                    {
                        _Year = Convert.ToInt32(Utility.GetDynamicPropertyValue(Obj, "year"));
                        _DivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "DivisionID"));
                        _StructureTypeID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "StructureTypeID"));
                        _StructureID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "StructureID"));

                        FloodInspectionDetailID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "FloodInspectionDetailID"));
                        DataSet DS = new FloodInspectionsBLL().GetPreMBStatusID(_Year, _DivisionID, _StructureTypeID, _StructureID, Convert.ToInt64(ItemsID));
                        if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                        {
                            DataRow DR = DS.Tables[0].Rows[0];
                            PreMBStatusID = Convert.ToInt64(DR["PreMBStatusID"]) == 0 ? (long?)null : Convert.ToInt64(DR["PreMBStatusID"]);
                        }
                    }

                    TextBox txtQuantityAvalible = (TextBox)gvMeasuringBookStatusPost.Rows[m].FindControl("txtQuantityAvalible");
                    TextBox txtQuantityRequired = (TextBox)gvMeasuringBookStatusPost.Rows[m].FindControl("txtQuantityRequired");
                    if (Convert.ToInt64(OverallDivItemsID) != 0)
                    {
                        //if (txtQuantityAvalible.Text != "" && txtQuantityRequired.Text != "" && (QuantityAvailable != txtQuantityAvalible.Text || QuantityRequired != txtQuantityRequired.Text))
                        if (txtQuantityRequired.Text != "" && (QuantityAvailable != txtQuantityAvalible.Text || QuantityRequired != txtQuantityRequired.Text))
                        {
                            bllFFPBLL.OverallDivItemsInsertion(Convert.ToInt64(OverallDivItemsID), _Year, _DivisionID,
                                Convert.ToInt16(ddlCategory.SelectedValue),
                                Convert.ToInt64(ItemsID), _StructureTypeID, _StructureID, PreMBStatusID,
                                FloodInspectionDetailID, txtQuantityAvalible.Text == "" ? 0 : Convert.ToInt32(txtQuantityAvalible.Text), Convert.ToInt32(txtQuantityRequired.Text),
                                null, null, null,
                                Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                    else
                    {
                        if (txtQuantityAvalible.Text != "" && txtQuantityRequired.Text != "" && (QuantityAvailable != txtQuantityAvalible.Text || QuantityRequired != txtQuantityRequired.Text))
                        {
                            bllFFPBLL.OverallDivItemsInsertion(0, _Year, _DivisionID,
                                Convert.ToInt16(ddlCategory.SelectedValue),
                                Convert.ToInt64(ItemsID), _StructureTypeID, _StructureID, PreMBStatusID,
                                FloodInspectionDetailID,
                                Convert.ToInt32(txtQuantityAvalible.Text), Convert.ToInt32(txtQuantityRequired.Text),
                                null, null, null,
                                Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(
                        Constants.MessageCategory.WebApp);
                }
            }
            try
            {
                if (IsSave == true)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindMeasuringBookStatusPostFloodGrid(Convert.ToInt16(ddlCategory.SelectedValue), Convert.ToInt64(hdnFloodInspectionsID.Value));
                    // gvMeasuringBookStatusPost.Visible = false;
                    // BindDropdown();
                    //btnSave.Enabled = false;
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(
                    Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategory.SelectedItem.Value == "")
                {
                    gvMeasuringBookStatusPost.Visible = false;
                    //  btnSave.Enabled = false;
                }
                else
                {
                    gvMeasuringBookStatusPost.Visible = true;
                    BindMeasuringBookStatusPostFloodGrid(Convert.ToInt16(ddlCategory.SelectedItem.Value), Convert.ToInt64(hdnFloodInspectionsID.Value));
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
        //        string PFIns = (gvMeasuringBookStatusPost.Rows[index].FindControl("lblQuantityPreFloodInspections") as Label).Text;
        //        string QtyAva = (gvMeasuringBookStatusPost.Rows[index].FindControl("txtQuantityAvalible") as TextBox).Text;

        //        if (QtyAva != "")
        //        {
        //            (gvMeasuringBookStatusPost.Rows[index].FindControl("lblQuantityConsumed") as Label).Text = (Convert.ToInt32(PFIns) - Convert.ToInt32(QtyAva)).ToString();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void gvMeasuringBookStatusPost_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtQuantityAvalible");
                TextBox Reqtxt = (TextBox)e.Row.FindControl("txtQuantityRequired");
                if (txt.Text == "0")
                {
                    txt.Text = "";
                }
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    //if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                    //{
                    //    btnSave.Visible = true;
                    //}

                    txt.Enabled = false;
                    Reqtxt.Enabled = false;
                }
                if (_InspectionTypeID == 1)
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                    if (CanEdit)
                    {
                        txt.Enabled = CanEdit;
                        Reqtxt.Enabled = CanEdit;
                    }
                    else
                    {
                        txt.Enabled = false;
                        Reqtxt.Enabled = false;
                    }
                }
                else
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                    if (CanEdit)
                    {
                        txt.Enabled = CanEdit;
                        Reqtxt.Enabled = CanEdit;
                    }
                }





            }
        }

        #endregion Events

        protected void gvMeasuringBookStatusPost_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMeasuringBookStatusPost.PageIndex = e.NewPageIndex;
                BindMeasuringBookStatusPostFloodGrid(Convert.ToInt16(ddlCategory.SelectedItem.Value), Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}