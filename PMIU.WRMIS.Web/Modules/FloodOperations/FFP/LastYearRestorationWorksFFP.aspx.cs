using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class LastYearRestorationWorksFFP : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    Session["FFPID"] = "";
                    long _FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    if (_FFPID > 0)
                    {
                        // DepartmentalInspectionDetail1.FloodInspectionIDProp = _FFPID;
                        hdnFFPID.Value = Convert.ToString(_FFPID);
                        hdnFFPStatus.Value = new FloodFightingPlanBLL().GetFFPStatus(_FFPID).ToString();
                        Session["status"] = "";
                        Session["status"] = hdnFFPStatus.Value;
                        Session["FFPID"] = _FFPID;
                        FFPDetail._FFPID = _FFPID;
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/SearchFFP.aspx?FFPID={0}", _FFPID);
                        BindLastRestorationWorksGrid(_FFPID);
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindLastRestorationWorksGrid(long _FFPID) 
        {
            try
            {
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);
                int _FFPDivisionID = Utility.GetNumericValueFromQueryString("FFPDivisionID", 0);
                IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFFPLastYearRestorationWork(_FFPDivisionID, _FFPYear);
                var LstFFP = IeFFPCam.Select(dataRow => new
                {
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    WorkType = dataRow.Field<string>("WorkType"),
                    WorkName = dataRow.Field<string>("WorkName"),
                    WorkStatus = dataRow.Field<string>("WorkStatus"),
                    Description = dataRow.Field<string>("Description"),
                }).ToList();
                gvLastRestorationWorks.DataSource = LstFFP;
                gvLastRestorationWorks.DataBind();
                gvLastRestorationWorks.Visible = true;

                //Button btn = (Button)gvLastRestorationWorks.HeaderRow.FindControl("btnAddInfrastructures");
                //if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                //{
                //    btn.Enabled = false;
                //    DisableEditDeleteColumn(gvLastRestorationWorks);
                //}
                //if (Convert.ToString(hdnFFPStatus.Value) == "Draft")
                //{
                //    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                //    if (CanAddEditFFP)
                //    {
                //        btn.Enabled = CanAddEditFFP;
                //        EnableEditDeleteColumn(gvLastRestorationWorks);
                //    }
                //    else
                //    {
                //        btn.Enabled = false;
                //        DisableEditDeleteColumn(gvLastRestorationWorks);

                //    }
                //}
                //else if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                //{
                //    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                //    if (CanAddEditFFP)
                //    {
                //        btn.Enabled = CanAddEditFFP;
                //        EnableEditDeleteColumn(gvLastRestorationWorks);
                //    }
                //}


                //if (Convert.ToString(hdnFFPStatus.Value) == "Published" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                //{
                //    Button btn = (Button)gvInfrastructures.HeaderRow.FindControl("btnAddInfrastructures");
                //    btn.Enabled = true;
                //    EnableEditDeleteColumn(gvInfrastructures);
                //}
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Old code Of LastYearRestorationWork
        //private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        //{
        //    Button btnDelete = (Button)_e.Row.FindControl("btnDeleteInfrastructures");
        //    if (btnDelete != null)
        //    {
        //        btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
        //    }
        //}

        //private void DisableEditDeleteColumn(GridView grid)
        //{
        //    foreach (GridViewRow r in grid.Rows)
        //    {
        //        Button btnEditInfrastructures = (Button)r.FindControl("btnEditInfrastructures");
        //        btnEditInfrastructures.Enabled = false;

        //        Button btnDeleteInfrastructures = (Button)r.FindControl("btnDeleteInfrastructures");
        //        btnDeleteInfrastructures.Enabled = false;
        //    }
        //}

        //private void EnableEditDeleteColumn(GridView grid)
        //{
        //    foreach (GridViewRow r in grid.Rows)
        //    {
        //        Button btnEditFFPStonePosition = (Button)r.FindControl("btnEditInfrastructures");
        //        btnEditFFPStonePosition.Enabled = true;

        //        Button btnDeleteFFPStonePosition = (Button)r.FindControl("btnDeleteInfrastructures");
        //        btnDeleteFFPStonePosition.Enabled = true;
        //    }
        //}

        //protected void gvLastRestorationWorks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvLastRestorationWorks.EditIndex = -1;
        //        BindLastRestorationWorksGrid(Convert.ToInt32(hdnFFPID.Value));
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvLastRestorationWorks_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "Add")
        //        {
        //            //List<object> lstInfrastructures = new FloodFightingPlanBLL().GetInfrastructures_CampSiteBy_FFPID(Convert.ToInt64(hdnFFPID.Value));
        //            //lstInfrastructures.Add(new
        //            //{
        //            //    FFPCampSitesID = 0,
        //            //    FFPID=0,
        //            //    InfrastructureTypeID = string.Empty,
        //            //    InfrastructureType=string.Empty,
        //            //    InfrastructureName = string.Empty,
        //            //    StructureID = string.Empty,
        //            //    Description = "",
        //            //    RDtotal = string.Empty,
        //            //    CreatedBy = 0,
        //            //    CreatedDate = DateTime.Now
        //            //});

        //            IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFO_FFPCampSitesByIDs(null, Convert.ToInt64(hdnFFPID.Value));
        //            List<object> LstFFP = IeFFPCam.Select(dataRow => new
        //            {
        //                FFPCampSitesID = dataRow.Field<long>("FFPCampSitesID"),
        //                InfrastructureType = dataRow.Field<string>("InfrastructureType"),
        //                InfrastructureName = dataRow.Field<string>("InfrastructureName"),
        //                InfrastructureTypeID = dataRow.Field<Int32>("InfrastructureTypeID"),
        //                RDtotal = dataRow.Field<Int32>("RDtotal"),
        //                Description = dataRow.Field<string>("Description"),
        //                StructureID = dataRow.Field<long>("StructureID"),
        //                CreatedBy = dataRow.Field<Int32>("CreatedBy"),
        //                CreatedDate = dataRow.Field<DateTime>("CreatedDate"),
        //            }).ToList<object>();
        //            LstFFP.Add(new
        //            {
        //                FFPCampSitesID = 0,
        //                FFPID = 0,
        //                InfrastructureTypeID = string.Empty,
        //                InfrastructureType = string.Empty,
        //                InfrastructureName = string.Empty,
        //                StructureID = string.Empty,
        //                Description = "",
        //                RDtotal = string.Empty,
        //                CreatedBy = 0,
        //                CreatedDate = DateTime.Now
        //            });

        //            gvLastRestorationWorks.PageIndex = gvLastRestorationWorks.PageCount;
        //            gvLastRestorationWorks.DataSource = LstFFP;
        //            gvLastRestorationWorks.DataBind();

        //            gvLastRestorationWorks.EditIndex = gvLastRestorationWorks.Rows.Count - 1;
        //            gvLastRestorationWorks.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvLastRestorationWorks_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        UA_Users _Users = SessionManagerFacade.UserInformation;

        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            AddDeletionConfirmMessage(e);

        //            DataKey key = gvLastRestorationWorks.DataKeys[e.Row.RowIndex];
        //            string InfrastructuresTypeID = Convert.ToString(key.Values["InfrastructureTypeID"]);
        //            string InfrastructuresNameID = Convert.ToString(key.Values["StructureID"]);


        //            Label lblInfrastructuresType = (Label)e.Row.FindControl("lblInfrastructuresType");
        //            Label lblInfrastructuresName = (Label)e.Row.FindControl("lblInfrastructuresName");


        //            if (gvLastRestorationWorks.EditIndex == e.Row.RowIndex)
        //            {
        //                string ID = Convert.ToString(key.Values["ID"]);
        //                string Description = Convert.ToString(key.Values["Description"]);
        //                string RD = Convert.ToString(key.Values["RDtotal"]);
        //                string struct_type = Convert.ToString(key.Values["InfrastructureType"]);
        //                string structureName = Convert.ToString(key.Values["InfrastructureName"]);

        //                #region "Controls"

        //                DropDownList ddlInfrastructuresType = (DropDownList)e.Row.FindControl("ddlInfrastructuresType");
        //                DropDownList ddlInfrastructuresName = (DropDownList)e.Row.FindControl("ddlInfrastructuresName");

        //                TextBox txtDesc = (TextBox)e.Row.FindControl("txtDesc");

        //                //     HyperLink hlItems = (HyperLink)e.Row.FindControl("hlItems");

        //                //     hlItems.Enabled = false;

        //                #endregion "Controls"

        //                Dropdownlist.DDLInfrastructureType(ddlInfrastructuresType);
        //                if (struct_type == "Protection Infrastructure")
        //                {
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Protection Infrastructure");
        //                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
        //                }
        //                else if (struct_type == "Control Structure1")
        //                {
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Barrage/Headwork");
        //                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
        //                }
        //                else if (struct_type == "Drain")
        //                {
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Drain");
        //                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
        //                    Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
        //                }

        //                if (Description != null)
        //                {
        //                    txtDesc.Text = Description;
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvLastRestorationWorks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        string ID = Convert.ToString(gvLastRestorationWorks.DataKeys[e.RowIndex].Values[0]);
        //        if (!IsValidDelete(Convert.ToInt64(ID)))
        //        {
        //            return;
        //        }

        //        bool IsDeleted = new FloodFightingPlanBLL().DeleteFFPCampSites(Convert.ToInt64(ID));
        //        if (IsDeleted)
        //        {
        //            BindLastRestorationWorksGrid(Convert.ToInt32(hdnFFPID.Value));
        //            Master.ShowMessage(Message.RecordDeleted.Description);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvLastRestorationWorks_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {
        //        gvLastRestorationWorks.EditIndex = e.NewEditIndex;
        //        BindLastRestorationWorksGrid(Convert.ToInt32(hdnFFPID.Value));
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvLastRestorationWorks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        object bllGetStructureTypeIDByvalue = null;

        //        string InfrastructureTypeName = string.Empty;

        //        UA_Users mdlUser = SessionManagerFacade.UserInformation;

        //        #region "Data Keys"

        //        DataKey key = gvLastRestorationWorks.DataKeys[e.RowIndex];
        //        string ID = Convert.ToString(key.Values["FFPCampSitesID"]);
        //        string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
        //        string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

        //        #endregion "Data Keys"

        //        #region "Controls"

        //        GridViewRow row = gvLastRestorationWorks.Rows[e.RowIndex];
        //        DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
        //        DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");

        //        TextBox txtDesc = (TextBox)row.FindControl("txtDesc");

        //        #endregion "Controls"

        //        FO_FFPCampSites dInfrastructures = new FO_FFPCampSites();

        //        dInfrastructures.ID = Convert.ToInt64(ID);
        //        dInfrastructures.FFPID = Convert.ToInt64(hdnFFPID.Value);

        //        if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 1)
        //        {
        //            bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
        //            long ProtectionInfrastructureID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("ProtectionInfrastructureID").GetValue(bllGetStructureTypeIDByvalue));
        //            long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
        //            InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

        //            dInfrastructures.StructureTypeID = InfrastructureTypeID;
        //            dInfrastructures.StructureID = ProtectionInfrastructureID;
        //        }
        //        else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 2)
        //        {
        //            bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
        //            long StationID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("StationID").GetValue(bllGetStructureTypeIDByvalue));
        //            long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
        //            InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

        //            dInfrastructures.StructureTypeID = InfrastructureTypeID;
        //            dInfrastructures.StructureID = StationID;
        //        }
        //        else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 3)
        //        {
        //            bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
        //            long DrainID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("DrainID").GetValue(bllGetStructureTypeIDByvalue));
        //            long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
        //            InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

        //            dInfrastructures.StructureTypeID = InfrastructureTypeID;
        //            dInfrastructures.StructureID = DrainID;
        //        }

        //        dInfrastructures.Description = txtDesc.Text;

        //        if (dInfrastructures.ID == 0)
        //        {
        //            dInfrastructures.CreatedBy = Convert.ToInt32(mdlUser.ID);
        //            dInfrastructures.CreatedDate = DateTime.Now;
        //        }
        //        else
        //        {
        //            dInfrastructures.CreatedBy = Convert.ToInt32(CreatedBy);
        //            dInfrastructures.CreatedDate = Convert.ToDateTime(CreatedDate);
        //            dInfrastructures.ModifiedBy = Convert.ToInt32(mdlUser.ID);
        //            dInfrastructures.ModifiedDate = DateTime.Now;
        //        }

        //        if (dInfrastructures.ID == 0)
        //        {
        //            if (InfrastructureTypeName != "Control Structure1")
        //            {
        //                if (new FloodFightingPlanBLL().IsFFPCampSits_Unique(dInfrastructures))
        //                {
        //                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                if (new FloodFightingPlanBLL().IsFFPCampSitsWithoutRD_Unique(dInfrastructures))
        //                {
        //                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (InfrastructureTypeName != "Control Structure1")
        //            {
        //                if (new FloodFightingPlanBLL().IsFFPCampSits_UniqueByID(dInfrastructures))
        //                {
        //                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //                    return;
        //                }

        //            }
        //            else
        //            {
        //                if (new FloodFightingPlanBLL().IsFFPCampSitsWithoutRD_UniqueByID(dInfrastructures))
        //                {
        //                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
        //                    return;
        //                }
        //            }
        //        }
        //        bool IsSave = new FloodFightingPlanBLL().SaveFFPCampSites(dInfrastructures);

        //        if (IsSave)
        //        {
        //            // Redirect user to first page.
        //            if (Convert.ToInt64(dInfrastructures.ID) == 0)
        //                gvLastRestorationWorks.PageIndex = 0;

        //            gvLastRestorationWorks.EditIndex = -1;
        //            BindLastRestorationWorksGrid(Convert.ToInt32(hdnFFPID.Value));
        //            Master.ShowMessage(Message.RecordSaved.Description);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //private bool IsValidDelete(long ID)
        //{
        //    FloodFightingPlanBLL bl = new FloodFightingPlanBLL();
        //    bool IsExist = bl.IsFo_FFPCampSite_IDExists(ID);

        //    if (IsExist)
        //    {
        //        Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

        //        return false;
        //    }

        //    return true;
        //}
        //protected void ddlInfrastructuresType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddl.NamingContainer;
        //    int index = row.RowIndex;

        //    DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
        //    DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");
        //    TextBox txtRDLeft = (TextBox)row.FindControl("txtRDLeft");
        //    TextBox txtRDRight = (TextBox)row.FindControl("txtRDRight");

        //    if (ddlInfrastructuresType.SelectedItem.Value != "")
        //    {
        //        UA_Users _Users = SessionManagerFacade.UserInformation;
        //        long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
        //        if (InfrastructureTypeSelectedValue == 1)
        //        {
        //            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
        //            txtRDLeft.Enabled = true;
        //            txtRDRight.Enabled = true;
        //        }
        //        else if (InfrastructureTypeSelectedValue == 2)
        //        {
        //            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
        //            //rd disable
        //            txtRDLeft.Text = "";
        //            txtRDRight.Text = "";
        //            txtRDLeft.Enabled = false;
        //            txtRDRight.Enabled = false;
        //        }
        //        else if (InfrastructureTypeSelectedValue == 3)
        //        {
        //            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
        //            txtRDLeft.Enabled = true;
        //            txtRDRight.Enabled = true;
        //        }
        //    }
        //    else
        //    {
        //        ddlInfrastructuresName.Items.Clear();
        //    }
        //}
        #endregion Old code Of LastYearRestorationWork
        protected void gvLastRestorationWorks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLastRestorationWorks.PageIndex = e.NewPageIndex;
                gvLastRestorationWorks.EditIndex = -1;
                BindLastRestorationWorksGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}