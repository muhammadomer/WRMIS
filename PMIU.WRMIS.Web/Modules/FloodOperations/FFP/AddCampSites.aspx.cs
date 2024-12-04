using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class AddCampSites : BasePage
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
                        BindInfrastructuresGrid(_FFPID);
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

        private void BindInfrastructuresGrid(long _FFPID)
        {
            try
            {
                bool CanAddEditFFP = false;
                UA_SystemParameters systemParameters = null;
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);
                IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFO_FFPCampSitesByIDs(null, _FFPID);
                var LstFFP = IeFFPCam.Select(dataRow => new
                {
                    FFPCampSitesID = dataRow.Field<long>("FFPCampSitesID"),
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    InfrastructureTypeID = dataRow.Field<Int32>("InfrastructureTypeID"),
                    RDtotal = dataRow.Field<Int32>("RDtotal"),
                    Description = dataRow.Field<string>("Description"),
                    StructureID = dataRow.Field<long>("StructureID"),
                    CreatedBy = dataRow.Field<Int32>("CreatedBy"),
                    CreatedDate = dataRow.Field<DateTime>("CreatedDate"),
                }).ToList();
                gvInfrastructures.DataSource = LstFFP;
                gvInfrastructures.DataBind();
                gvInfrastructures.Visible = true;

                Button btn = (Button)gvInfrastructures.HeaderRow.FindControl("btnAddInfrastructures");
                if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                {
                    btn.Enabled = false;
                    DisableEditDeleteColumn(gvInfrastructures);
                }
                if (Convert.ToString(hdnFFPStatus.Value) == "Draft")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                    if (CanAddEditFFP)
                    {
                        btn.Enabled = CanAddEditFFP;
                        EnableEditDeleteColumn(gvInfrastructures);
                    }
                    else
                    {
                        btn.Enabled = false;
                        DisableEditDeleteColumn(gvInfrastructures);

                    }
                }
                else if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                    if (CanAddEditFFP)
                    {
                        btn.Enabled = CanAddEditFFP;
                        EnableEditDeleteColumn(gvInfrastructures);
                    }
                }


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

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteInfrastructures");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditInfrastructures = (Button)r.FindControl("btnEditInfrastructures");
                btnEditInfrastructures.Enabled = false;

                Button btnDeleteInfrastructures = (Button)r.FindControl("btnDeleteInfrastructures");
                btnDeleteInfrastructures.Enabled = false;
            }
        }

        private void EnableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditFFPStonePosition = (Button)r.FindControl("btnEditInfrastructures");
                btnEditFFPStonePosition.Enabled = true;

                Button btnDeleteFFPStonePosition = (Button)r.FindControl("btnDeleteInfrastructures");
                btnDeleteFFPStonePosition.Enabled = true;
            }
        }

        protected void gvInfrastructures_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvInfrastructures.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    //List<object> lstInfrastructures = new FloodFightingPlanBLL().GetInfrastructures_CampSiteBy_FFPID(Convert.ToInt64(hdnFFPID.Value));
                    //lstInfrastructures.Add(new
                    //{
                    //    FFPCampSitesID = 0,
                    //    FFPID=0,
                    //    InfrastructureTypeID = string.Empty,
                    //    InfrastructureType=string.Empty,
                    //    InfrastructureName = string.Empty,
                    //    StructureID = string.Empty,
                    //    Description = "",
                    //    RDtotal = string.Empty,
                    //    CreatedBy = 0,
                    //    CreatedDate = DateTime.Now
                    //});

                    IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFO_FFPCampSitesByIDs(null, Convert.ToInt64(hdnFFPID.Value));
                    List<object> LstFFP = IeFFPCam.Select(dataRow => new
                    {
                        FFPCampSitesID = dataRow.Field<long>("FFPCampSitesID"),
                        InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                        InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                        InfrastructureTypeID = dataRow.Field<Int32>("InfrastructureTypeID"),
                        RDtotal = dataRow.Field<Int32>("RDtotal"),
                        Description = dataRow.Field<string>("Description"),
                        StructureID = dataRow.Field<long>("StructureID"),
                        CreatedBy = dataRow.Field<Int32>("CreatedBy"),
                        CreatedDate = dataRow.Field<DateTime>("CreatedDate"),
                    }).ToList<object>();
                    LstFFP.Add(new
                    {
                        FFPCampSitesID = 0,
                        FFPID = 0,
                        InfrastructureTypeID = string.Empty,
                        InfrastructureType = string.Empty,
                        InfrastructureName = string.Empty,
                        StructureID = string.Empty,
                        Description = "",
                        RDtotal = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvInfrastructures.PageIndex = gvInfrastructures.PageCount;
                    gvInfrastructures.DataSource = LstFFP;
                    gvInfrastructures.DataBind();

                    gvInfrastructures.EditIndex = gvInfrastructures.Rows.Count - 1;
                    gvInfrastructures.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                string LeftRD = string.Empty;
                string RightRD = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    DataKey key = gvInfrastructures.DataKeys[e.Row.RowIndex];
                    string InfrastructuresTypeID = Convert.ToString(key.Values["InfrastructureTypeID"]);
                    string InfrastructuresNameID = Convert.ToString(key.Values["StructureID"]);
                    string RDedit = Convert.ToString(key.Values["RDtotal"]);

                    Label lblInfrastructuresType = (Label)e.Row.FindControl("lblInfrastructuresType");
                    Label lblInfrastructuresName = (Label)e.Row.FindControl("lblInfrastructuresName");
                    Label lblRD = (Label)e.Row.FindControl("lblRD");

                    if (gvInfrastructures.EditIndex == e.Row.RowIndex)
                    {
                        string ID = Convert.ToString(key.Values["ID"]);
                        string Description = Convert.ToString(key.Values["Description"]);
                        string RD = Convert.ToString(key.Values["RDtotal"]);
                        string struct_type = Convert.ToString(key.Values["InfrastructureType"]);
                        string structureName = Convert.ToString(key.Values["InfrastructureName"]);

                        #region "Controls"

                        DropDownList ddlInfrastructuresType = (DropDownList)e.Row.FindControl("ddlInfrastructuresType");
                        DropDownList ddlInfrastructuresName = (DropDownList)e.Row.FindControl("ddlInfrastructuresName");
                        TextBox txtRDLeft = (TextBox)e.Row.FindControl("txtRDLeft");
                        TextBox txtRDRight = (TextBox)e.Row.FindControl("txtRDRight");
                        TextBox txtDesc = (TextBox)e.Row.FindControl("txtDesc");

                        //     HyperLink hlItems = (HyperLink)e.Row.FindControl("hlItems");

                        //     hlItems.Enabled = false;

                        #endregion "Controls"

                        Dropdownlist.DDLInfrastructureType(ddlInfrastructuresType);
                        if (struct_type == "Protection Infrastructure")
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Protection Infrastructure");
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
                            Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
                        }
                        else if (struct_type == "Control Structure1")
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Barrage/Headwork");
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
                            Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
                        }
                        else if (struct_type == "Drain")
                        {
                            Dropdownlist.SetSelectedText(ddlInfrastructuresType, "Drain");
                            Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
                            Dropdownlist.SetSelectedText(ddlInfrastructuresName, structureName);
                        }

                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(RD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(RD));
                            LeftRD = tupleFromRD.Item1;
                            RightRD = tupleFromRD.Item2;
                        }

                        if (txtRDLeft != null)
                            txtRDLeft.Text = LeftRD;
                        if (txtRDRight != null)
                            txtRDRight.Text = RightRD;

                        if (Description != null)
                        {
                            txtDesc.Text = Description;
                        }
                        if (struct_type == "Control Structure1")
                        {
                            //rd disable
                            txtRDLeft.Text = "";
                            txtRDRight.Text = "";
                            txtRDLeft.Enabled = false;
                            txtRDRight.Enabled = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string struct_type = Convert.ToString(key.Values["InfrastructureType"]);

                        if (struct_type == "Control Structure1")
                        {
                            lblRD.Visible = false;
                        }
                    }

                    if (lblRD.Text != "")
                    {
                        lblRD.Text = Calculations.GetRDText(Convert.ToInt64(RDedit));
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvInfrastructures.DataKeys[e.RowIndex].Values[0]);
                if (!IsValidDelete(Convert.ToInt64(ID)))
                {
                    return;
                }

                bool IsDeleted = new FloodFightingPlanBLL().DeleteFFPCampSites(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvInfrastructures.EditIndex = e.NewEditIndex;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                object bllGetStructureTypeIDByvalue = null;
                string totalRD = string.Empty;
                string InfrastructureTypeName = string.Empty;
                bool check_RDStation = false;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvInfrastructures.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["FFPCampSitesID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion "Data Keys"

                #region "Controls"

                GridViewRow row = gvInfrastructures.Rows[e.RowIndex];
                DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
                DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");

                TextBox txtRDLeft = (TextBox)row.FindControl("txtRDLeft");
                TextBox txtRDRight = (TextBox)row.FindControl("txtRDRight");
                TextBox txtDesc = (TextBox)row.FindControl("txtDesc");

                #endregion "Controls"

                FO_FFPCampSites dInfrastructures = new FO_FFPCampSites();

                dInfrastructures.ID = Convert.ToInt64(ID);
                dInfrastructures.FFPID = Convert.ToInt64(hdnFFPID.Value);

                if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 1)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long ProtectionInfrastructureID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("ProtectionInfrastructureID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

                    dInfrastructures.StructureTypeID = InfrastructureTypeID;
                    dInfrastructures.StructureID = ProtectionInfrastructureID;
                }
                else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 2)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long StationID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("StationID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

                    dInfrastructures.StructureTypeID = InfrastructureTypeID;
                    dInfrastructures.StructureID = StationID;
                }
                else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 3)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long DrainID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("DrainID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    InfrastructureTypeName = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeName").GetValue(bllGetStructureTypeIDByvalue));

                    dInfrastructures.StructureTypeID = InfrastructureTypeID;
                    dInfrastructures.StructureID = DrainID;
                }

                if (txtRDLeft.Text != "" & txtRDRight.Text != "")
                {
                    dInfrastructures.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);
                }
                dInfrastructures.Description = txtDesc.Text;

                if (dInfrastructures.ID == 0)
                {
                    dInfrastructures.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dInfrastructures.CreatedDate = DateTime.Now;
                }
                else
                {
                    dInfrastructures.CreatedBy = Convert.ToInt32(CreatedBy);
                    dInfrastructures.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dInfrastructures.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dInfrastructures.ModifiedDate = DateTime.Now;
                }

                //object RDCheck = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value), Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                //if (RDCheck != null)
                //{
                //    string InfraType = RDCheck.GetType().GetProperty("InfrastructureTypeName").GetValue(RDCheck).ToString();
                //    if (InfraType=="Protection Infrastructure")
                //    {
                //        totalRD = RDCheck.GetType().GetProperty("TotalLength").GetValue(RDCheck).ToString();
                //        check_RDStation = true;
                //    }
                //    else if (InfraType == "Drain")
                //    {
                //        totalRD = RDCheck.GetType().GetProperty("TotalLength").GetValue(RDCheck).ToString();
                //        check_RDStation = true;
                //    }
                //    if (check_RDStation == true)
                //    {
                //        Tuple<string, string> tupleTotalRD = Calculations.GetRDValues(Convert.ToInt64(totalRD));

                //        if (!(Convert.ToInt64(tupleTotalRD.Item1) <= dInfrastructures.RD && Convert.ToInt64(tupleTotalRD.Item2) >= dInfrastructures.RD))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                //object DivisionObjct = new FloodFightingPlanBLL().GetFFPDivisionID(Convert.ToInt64(hdnFFPID.Value));
                //DataSet DS = new FloodOperationsBLL().FO_irrigationRDs(Convert.ToInt32(DivisionObjct.GetType().GetProperty("DivisionID").GetValue(DivisionObjct)), dInfrastructures.StructureID, dInfrastructures.StructureTypeID);
                //if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //{
                //    DataRow DR = DS.Tables[0].Rows[0];
                //    Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //    if (!(dInfrastructures.RD >= Convert.ToInt64(tupleRDs.Item1) && dInfrastructures.RD <= Convert.ToInt64(tupleRDs.Item2)))
                //    {
                //        Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //        return;
                //    }
                //}

                if (dInfrastructures.ID == 0)
                {
                    if (InfrastructureTypeName != "Control Structure1")
                    {
                        if (new FloodFightingPlanBLL().IsFFPCampSits_Unique(dInfrastructures))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        //if (txtRDLeft.Text == "" && txtRDRight.Text == "")
                        //{
                        //    Master.ShowMessage("RD is required", SiteMaster.MessageType.Error);
                        //    return;
                        //}
                    }
                    else
                    {
                        if (new FloodFightingPlanBLL().IsFFPCampSitsWithoutRD_Unique(dInfrastructures))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                }
                else
                {
                    if (InfrastructureTypeName != "Control Structure1")
                    {
                        if (new FloodFightingPlanBLL().IsFFPCampSits_UniqueByID(dInfrastructures))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if (txtRDLeft.Text == "" && txtRDRight.Text == "")
                        {
                            Master.ShowMessage("RD is required", SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        if (new FloodFightingPlanBLL().IsFFPCampSitsWithoutRD_UniqueByID(dInfrastructures))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                }
                bool IsSave = new FloodFightingPlanBLL().SaveFFPCampSites(dInfrastructures);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dInfrastructures.ID) == 0)
                        gvInfrastructures.PageIndex = 0;

                    gvInfrastructures.EditIndex = -1;
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private bool IsValidDelete(long ID)
        {
            FloodFightingPlanBLL bl = new FloodFightingPlanBLL();
            bool IsExist = bl.IsFo_FFPCampSite_IDExists(ID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void ddlInfrastructuresType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int index = row.RowIndex;

            DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
            DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");
            TextBox txtRDLeft = (TextBox)row.FindControl("txtRDLeft");
            TextBox txtRDRight = (TextBox)row.FindControl("txtRDRight");

            if (ddlInfrastructuresType.SelectedItem.Value != "")
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
                    txtRDLeft.Enabled = true;
                    txtRDRight.Enabled = true;
                }
                else if (InfrastructureTypeSelectedValue == 2)
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
                    //rd disable
                    txtRDLeft.Text = "";
                    txtRDRight.Text = "";
                    txtRDLeft.Enabled = false;
                    txtRDRight.Enabled = false;
                }
                else if (InfrastructureTypeSelectedValue == 3)
                {
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
                    txtRDLeft.Enabled = true;
                    txtRDRight.Enabled = true;
                }
            }
            else
            {
                ddlInfrastructuresName.Items.Clear();
            }
        }

        protected void gvInfrastructures_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInfrastructures.PageIndex = e.NewPageIndex;
                gvInfrastructures.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}