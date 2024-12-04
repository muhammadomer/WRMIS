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
    public partial class AddFFPStonePosition : BasePage
    {
        private long _FFPID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    // Session["FFPID"] = "";
                    _FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);

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
                        BindFFPStonePositionGrid(Convert.ToInt64(hdnFFPID.Value));
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

        private void BindFFPStonePositionGrid(long _FFPID)
        {
            try
            {
                bool CanAddEditFFP = false;
                UA_SystemParameters systemParameters = null;
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);
                IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFFPGetStonePositionByID(_FFPID);
                var LstFFP = IeFFPCam.Select(dataRow => new
                {
                    FFPStonePositionID = dataRow.Field<Int64?>("FFPStonePositionID"),
                    FloodInspectionDetailID = dataRow.Field<Int64?>("FloodInspectionDetailID"),
                    IStonePositonID = dataRow.Field<Int64?>("IStonePositonID"),
                    RD = dataRow.Field<Int32>("RD"),
                    RequiredQty = dataRow.Field<Int32?>("RequiredQty") == null ? 0 : dataRow.Field<Int32?>("RequiredQty"),
                    CreatedBy = dataRow.Field<Int32?>("CreatedBy"),
                    CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                    InfrastructureTypeID = dataRow.Field<Int64>("InfrastructureTypeID"),
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    AvailableQty = dataRow.Field<Int32?>("AvailableQty") == null ? 0 : dataRow.Field<Int32?>("AvailableQty"),
                    TotalQty = dataRow.Field<Int32?>("TotalQty") == null ? 0 : dataRow.Field<Int32?>("TotalQty")
                }).ToList();
                gvFFPStonePosition.DataSource = LstFFP;
                gvFFPStonePosition.DataBind();
                gvFFPStonePosition.Visible = true;

                Button btnAddFFPStonePosition = (Button)gvFFPStonePosition.HeaderRow.FindControl("btnAddFFPStonePosition");
                if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                {
                    btnAddFFPStonePosition.Enabled = false;
                    DisableEditDeleteColumn(gvFFPStonePosition);
                }

                if (Convert.ToString(hdnFFPStatus.Value) == "Draft")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                    if (CanAddEditFFP)
                    {
                        btnAddFFPStonePosition.Enabled = CanAddEditFFP;
                        EnableEditDeleteColumn(gvFFPStonePosition);
                    }
                    else
                    {
                        btnAddFFPStonePosition.Enabled = false;
                        DisableEditDeleteColumn(gvFFPStonePosition);
                    }
                }
                else if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                    if (CanAddEditFFP)
                    {
                        btnAddFFPStonePosition.Enabled = CanAddEditFFP;
                        EnableEditDeleteColumn(gvFFPStonePosition);
                    }
                }



                //if (Convert.ToString(hdnFFPStatus.Value) == "Published")
                //{
                //    Button btn = (Button)gvFFPStonePosition.HeaderRow.FindControl("btnAddFFPStonePosition");
                //    btn.Enabled = false;
                //    DisableEditDeleteColumn(gvFFPStonePosition);
                //}
                //if (Convert.ToString(hdnFFPStatus.Value) == "Published" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                //{
                //    Button btn = (Button)gvFFPStonePosition.HeaderRow.FindControl("btnAddFFPStonePosition");
                //    btn.Enabled = true;
                //    EnableEditDeleteColumn(gvFFPStonePosition);
                //}
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteFFPStonePosition");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditFFPStonePosition = (Button)r.FindControl("btnEditFFPStonePosition");
                btnEditFFPStonePosition.Enabled = false;

                Button btnDeleteFFPStonePosition = (Button)r.FindControl("btnDeleteFFPStonePosition");
                btnDeleteFFPStonePosition.Enabled = false;
            }
        }
        private void EnableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditFFPStonePosition = (Button)r.FindControl("btnEditFFPStonePosition");
                btnEditFFPStonePosition.Enabled = true;

                Button btnDeleteFFPStonePosition = (Button)r.FindControl("btnDeleteFFPStonePosition");
                btnDeleteFFPStonePosition.Enabled = true;
            }
        }


        protected void gvFFPStonePosition_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvFFPStonePosition.EditIndex = -1;
                BindFFPStonePositionGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFPStonePosition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    IEnumerable<DataRow> IeFFPCam = new FloodFightingPlanBLL().GetFFPGetStonePositionByID(Convert.ToInt64(hdnFFPID.Value));
                    var LstFFP = IeFFPCam.Select(dataRow => new
                    {
                        FFPStonePositionID = dataRow.Field<Int64?>("FFPStonePositionID"),
                        FloodInspectionDetailID = dataRow.Field<Int64?>("FloodInspectionDetailID"),
                        IStonePositonID = dataRow.Field<Int64?>("IStonePositonID"),
                        RD = dataRow.Field<Int32>("RD"),
                        RequiredQty = dataRow.Field<Int32?>("RequiredQty") == null ? 0 : dataRow.Field<Int32?>("RequiredQty"),
                        CreatedBy = dataRow.Field<Int32?>("CreatedBy"),
                        CreatedDate = dataRow.Field<DateTime?>("CreatedDate"),
                        InfrastructureTypeID = dataRow.Field<Int64>("InfrastructureTypeID"),
                        InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                        InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                        AvailableQty = dataRow.Field<Int32?>("AvailableQty") == null ? 0 : dataRow.Field<Int32?>("AvailableQty"),
                        TotalQty = dataRow.Field<Int32?>("TotalQty") == null ? 0 : dataRow.Field<Int32?>("TotalQty")
                    }).ToList<object>();

                    LstFFP.Add(new
                    {
                        FFPStonePositionID = 0,
                        FloodInspectionDetailID = 0,
                        IStonePositonID = 0,
                        RD = 0,
                        RequiredQty = 0,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now,
                        InfrastructureTypeID = 0,
                        InfrastructureType = string.Empty,
                        InfrastructureName = string.Empty,
                        Infrastructure = string.Empty,
                        AvailableQty = 0,
                        TotalQty = 0
                    });

                    gvFFPStonePosition.PageIndex = gvFFPStonePosition.PageCount;
                    gvFFPStonePosition.DataSource = LstFFP;
                    gvFFPStonePosition.DataBind();

                    gvFFPStonePosition.EditIndex = gvFFPStonePosition.Rows.Count - 1;
                    gvFFPStonePosition.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFPStonePosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                string LeftRD = string.Empty;
                string RightRD = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    DataKey key = gvFFPStonePosition.DataKeys[e.Row.RowIndex];
                    string InfrastructuresTypeID = Convert.ToString(key.Values["InfrastructureTypeID"]);
                    string InfrastructuresName = Convert.ToString(key.Values["InfrastructureName"]);
                    string RDedit = Convert.ToString(key.Values["RD"]);
                    string RequiredQty = Convert.ToString(key.Values["RequiredQty"]);

                    Label lblInfrastructuresType = (Label)e.Row.FindControl("lblInfrastructuresType");
                    Label lblInfrastructuresName = (Label)e.Row.FindControl("lblInfrastructuresName");
                    Label lblRD = (Label)e.Row.FindControl("lblRD");

                    if (gvFFPStonePosition.EditIndex == e.Row.RowIndex)
                    {
                        string FFPStonePositionID = Convert.ToString(key.Values["FFPStonePositionID"]);
                        string RD = Convert.ToString(key.Values["RD"]);
                        string struct_type = Convert.ToString(key.Values["InfrastructureType"]);
                        string structureName = Convert.ToString(key.Values["InfrastructureName"]);

                        #region "Controls"

                        DropDownList ddlInfrastructuresType = (DropDownList)e.Row.FindControl("ddlInfrastructuresType");
                        DropDownList ddlInfrastructuresName = (DropDownList)e.Row.FindControl("ddlInfrastructuresName");
                        TextBox txtRDLeft = (TextBox)e.Row.FindControl("txtRDLeft");
                        TextBox txtRDRight = (TextBox)e.Row.FindControl("txtRDRight");
                        TextBox txtRequiredQty = (TextBox)e.Row.FindControl("txtRequiredQty");

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

                        if (txtRequiredQty != null)
                            txtRequiredQty.Text = RequiredQty;
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

        protected void gvFFPStonePosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvFFPStonePosition.DataKeys[e.RowIndex].Values[0]);
                //if (!IsValidDelete(Convert.ToInt64(ID)))
                //{
                //    return;
                //}

                bool IsDeleted = new FloodFightingPlanBLL().DeleteFFPStonePosition(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindFFPStonePositionGrid(Convert.ToInt32(hdnFFPID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFPStonePosition_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvFFPStonePosition.EditIndex = e.NewEditIndex;
                BindFFPStonePositionGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFPStonePosition_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                object bllGetStructureTypeIDByvalue = null;
                string totalRD = string.Empty;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvFFPStonePosition.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["FFPStonePositionID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                long? FloodInspectionDetailID = Convert.ToInt64(key.Values["FloodInspectionDetailID"]);
                long? IStonePositonID = Convert.ToInt64(key.Values["IStonePositonID"]);

                #endregion "Data Keys"

                #region "Controls"

                GridViewRow row = gvFFPStonePosition.Rows[e.RowIndex];
                DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
                DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");

                TextBox txtRDLeft = (TextBox)row.FindControl("txtRDLeft");
                TextBox txtRDRight = (TextBox)row.FindControl("txtRDRight");
                TextBox txtRequiredQty = (TextBox)row.FindControl("txtRequiredQty");

                #endregion "Controls"

                FO_FFPStonePosition dStonePosition = new FO_FFPStonePosition();

                if (ID != "")
                {
                    dStonePosition.ID = Convert.ToInt64(ID);
                }


                dStonePosition.FFPID = Convert.ToInt64(hdnFFPID.Value);
                if (FloodInspectionDetailID != 0)
                {
                    dStonePosition.FloodInspectionDetailID = FloodInspectionDetailID;
                }

                if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 1)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long ProtectionInfrastructureID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("ProtectionInfrastructureID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                    dStonePosition.StructureTypeID = InfrastructureTypeID;
                    dStonePosition.StructureID = ProtectionInfrastructureID;
                }
                else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 2)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long StationID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("StationID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                    dStonePosition.StructureTypeID = InfrastructureTypeID;
                    dStonePosition.StructureID = StationID;
                }
                else if (Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value) == 3)
                {
                    bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value));
                    long DrainID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("DrainID").GetValue(bllGetStructureTypeIDByvalue));
                    long InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                    dStonePosition.StructureTypeID = InfrastructureTypeID;
                    dStonePosition.StructureID = DrainID;
                }

                if (txtRDLeft.Text != "" && txtRDRight.Text != "")
                {
                    dStonePosition.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);
                }
                if (IStonePositonID != 0)
                {
                    dStonePosition.IStonePositionID = IStonePositonID;
                }

                dStonePosition.RequiredQty = Convert.ToInt32(txtRequiredQty.Text);

                if (dStonePosition.ID == 0)
                {
                    dStonePosition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dStonePosition.CreatedDate = DateTime.Now;
                }
                else
                {
                    dStonePosition.CreatedBy = Convert.ToInt32(CreatedBy);
                    dStonePosition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dStonePosition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dStonePosition.ModifiedDate = DateTime.Now;
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

                if (new FloodFightingPlanBLL().IsFFPStonePosition_Unique(dStonePosition))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new FloodFightingPlanBLL().SaveFFPStonePosition(dStonePosition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dStonePosition.ID) == 0)
                        gvFFPStonePosition.PageIndex = 0;

                    gvFFPStonePosition.EditIndex = -1;
                    BindFFPStonePositionGrid(Convert.ToInt32(hdnFFPID.Value));
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

        protected void gvFFPStonePosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFFPStonePosition.PageIndex = e.NewPageIndex;
                gvFFPStonePosition.EditIndex = -1;
                BindFFPStonePositionGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}