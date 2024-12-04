using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class ProblemForFI : BasePage
    {
        public static string InfrastructureType;

        #region Initialize

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);

                    if (_FloodInspectionID > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = _FloodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
                        hdnInfrastructureType.Value = new FloodInspectionsBLL().GetInfrastructureType(_FloodInspectionID).ToString();
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(_FloodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", _FloodInspectionID);
                        LoadIGCProtectionInfrastructure(_FloodInspectionID);
                        BindProblemFOGrid(_FloodInspectionID);
                    }
                    // hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", _FloodInspectionID);
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

        private void BindProblemFOGrid(long _FloodInspectionID)
        {
            try
            {
                List<object> lstProblemForFI = new FloodInspectionsBLL().GetProblemForFIByFloodInspectionID(_FloodInspectionID);

                gvProblemForFI.DataSource = lstProblemForFI;
                gvProblemForFI.DataBind();
                gvProblemForFI.Visible = true;
                //if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                //{
                //    Button btn = (Button)gvProblemForFI.HeaderRow.FindControl("btnAddProblemFI");
                //    btn.Enabled = false;

                //    DisableEditDeleteColumn(gvProblemForFI);
                //}
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

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteProblemFI");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditProblemFI = (Button)r.FindControl("btnEditProblemFI");
                Button btnDeleteProblemFI = (Button)r.FindControl("btnDeleteProblemFI");
                btnDeleteProblemFI.Enabled = false;
                btnEditProblemFI.Enabled = false;
            }
        }

        #endregion Functions

        #region Events

        protected void gvProblemForFI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProblemForFI.PageIndex = e.NewPageIndex;
                gvProblemForFI.EditIndex = -1;
                BindProblemFOGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvProblemForFI.EditIndex = -1;
                BindProblemFOGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddProblemFI")
                {
                    List<object> lstProblemForFI = new FloodInspectionsBLL().GetProblemForFIByFloodInspectionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    lstProblemForFI.Add(new
                    {
                        ID = 0,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        NatureofProblemID = string.Empty,
                        NatureofProblem = string.Empty,
                        RecommendedSolution = string.Empty,
                        TentativeCostofRestoration = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvProblemForFI.PageIndex = gvProblemForFI.PageCount;
                    gvProblemForFI.DataSource = lstProblemForFI;
                    gvProblemForFI.DataBind();

                    gvProblemForFI.EditIndex = gvProblemForFI.Rows.Count - 1;
                    gvProblemForFI.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvProblemForFI.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new FloodInspectionsBLL().DeleteProblemFI(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindProblemFOGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvProblemForFI.EditIndex = e.NewEditIndex;
                BindProblemFOGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvProblemForFI.Rows[e.RowIndex];

                #region "Data Keys"

                DataKey key = gvProblemForFI.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion "Data Keys"

                #region "Controls"

                GridViewRow Row = gvProblemForFI.Rows[e.RowIndex];
                DropDownList ddlNatureofProblem = (DropDownList)Row.FindControl("ddlNatureofProblem");

                TextBox txtFromRDLeft = (TextBox)Row.FindControl("txtFromRDLeft");
                TextBox txtFromRDRight = (TextBox)Row.FindControl("txtFromRDRight");
                TextBox txtToRDLeft = (TextBox)Row.FindControl("txtToRDLeft");
                TextBox txtToRDRight = (TextBox)Row.FindControl("txtToRDRight");
                TextBox txtSolutions = (TextBox)Row.FindControl("txtRecommendedSolution");
                TextBox txtTentativeCostofRestoration = (TextBox)Row.FindControl("txtTentativeCostofRestoration");

                #endregion "Controls"

                FO_IProblems ObjProFI = new FO_IProblems();

                ObjProFI.ID = Convert.ToInt64(ID);
                ObjProFI.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (ddlNatureofProblem != null)
                    ObjProFI.ProblemID = Convert.ToInt16(ddlNatureofProblem.SelectedItem.Value);

                if (txtFromRDLeft != null && txtFromRDRight != null && txtFromRDLeft.Text != "" && txtFromRDRight.Text != "")
                    ObjProFI.FromRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

                if (txtToRDLeft != null && txtToRDRight != null && txtToRDLeft.Text != "" && txtToRDRight.Text != "")
                    ObjProFI.ToRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);

                if (txtSolutions.Text != "")
                    ObjProFI.Solution = Convert.ToString(txtSolutions.Text);

                if (txtTentativeCostofRestoration.Text != "")
                    ObjProFI.RestorationCost = Convert.ToInt64(txtTentativeCostofRestoration.Text);

                if (ObjProFI.ID == 0)
                {
                    ObjProFI.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjProFI.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjProFI.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjProFI.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjProFI.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjProFI.ModifiedDate = DateTime.Now;
                }
                //infrastructurecheck = Utility.GetStringValueFromQueryString("InfrastructureType", "");
                if (Utility.GetStringValueFromQueryString("InfrastructureType", "") == "Drain")
                {
                    if (ObjProFI.ToRD >= ObjProFI.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjProFI.FromRD >= ObjProFI.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS = new FloodOperationsBLL().FO_irrigationRDs(Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")), Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")), Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));
                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (!((ObjProFI.FromRD >= Convert.ToInt64(tupleRDs.Item1) && ObjProFI.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //            && (ObjProFI.ToRD >= Convert.ToInt64(tupleRDs.Item1) && ObjProFI.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                bool IsSave = new FloodInspectionsBLL().SaveProblemForFI(ObjProFI);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjProFI.ID) == 0)
                        gvProblemForFI.PageIndex = 0;

                    gvProblemForFI.EditIndex = -1;
                    BindProblemFOGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemForFI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string FromLeftRD = string.Empty;
            string FromRightRD = string.Empty;
            string ToLeftRD = string.Empty;
            string ToRightRD = string.Empty;
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btn = (Button)e.Row.FindControl("btnAddProblemFI");
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btn.Enabled = false;

                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btn.Enabled = CanEdit;
                        else
                            btn.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btn.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblIrrigationFromRDs = (Label)e.Row.FindControl("lblIrrigationFromRDs");
                    Label lblIrrigationToRD = (Label)e.Row.FindControl("lblIrrigationToRD");
                    Button btnEditProblemFI = (Button)e.Row.FindControl("btnEditProblemFI");
                    Button btnDeleteProblemFI = (Button)e.Row.FindControl("btnDeleteProblemFI");
                    AddDeletionConfirmMessage(e);

                   
                    if (gvProblemForFI.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvProblemForFI.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRD"]);
                        string ToRD = Convert.ToString(key.Values["ToRD"]);
                        string FromRDTotal = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRDTotal = Convert.ToString(key.Values["ToRDTotal"]);
                        string NatureofProblemID = Convert.ToString(key.Values["NatureofProblemID"]);
                        string RecommendedSolution = Convert.ToString(key.Values["RecommendedSolution"]);
                        string TentativeCostofRestoration = Convert.ToString(key.Values["TentativeCostofRestoration"]);

                        #endregion "Data Keys"

                        #region "Controls"

                        DropDownList ddlNatureofProblem = (DropDownList)e.Row.FindControl("ddlNatureofProblem");

                        TextBox txtFromRDLeft = (TextBox)e.Row.FindControl("txtFromRDLeft");
                        TextBox txtFromRDRight = (TextBox)e.Row.FindControl("txtFromRDRight");
                        TextBox txtToRDLeft = (TextBox)e.Row.FindControl("txtToRDLeft");
                        TextBox txtToRDRight = (TextBox)e.Row.FindControl("txtToRDRight");
                        TextBox txtRecommendedSolution = (TextBox)e.Row.FindControl("txtRecommendedSolution");
                        TextBox txtTentativeCostofRestoration = (TextBox)e.Row.FindControl("txtTentativeCostofRestoration");

                        #endregion "Controls"

                        if (ddlNatureofProblem != null)
                        {
                            Dropdownlist.DDLProblemNature(ddlNatureofProblem, false);
                            if (!string.IsNullOrEmpty(NatureofProblemID))
                                Dropdownlist.SetSelectedValue(ddlNatureofProblem, NatureofProblemID);
                        }

                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRDTotal))
                        {
                            if (!string.IsNullOrEmpty(FromRD))
                            {
                                Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRDTotal));
                                FromLeftRD = tupleFromRD.Item1;
                                FromRightRD = tupleFromRD.Item2;
                            }
                        }
                        if (txtFromRDLeft != null)
                            txtFromRDLeft.Text = FromLeftRD;
                        if (txtFromRDRight != null)
                            txtFromRDRight.Text = FromRightRD;

                        // Check To RD is not null

                        if (!string.IsNullOrEmpty(ToRDTotal))
                        {
                            if (!string.IsNullOrEmpty(ToRD))
                            {
                                Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRDTotal));
                                ToLeftRD = tupleToRD.Item1;
                                ToRightRD = tupleToRD.Item2;
                            }
                        }
                        if (txtToRDLeft != null)
                            txtToRDLeft.Text = ToLeftRD;
                        if (txtToRDRight != null)
                            txtToRDRight.Text = ToRightRD;

                        if (txtRecommendedSolution != null)
                            txtRecommendedSolution.Text = RecommendedSolution;

                        if (txtTentativeCostofRestoration != null)
                            txtTentativeCostofRestoration.Text = TentativeCostofRestoration;

                        if (hdnInfrastructureType.Value == "1" || hdnInfrastructureType.Value == "2")
                        {
                            txtFromRDLeft.Enabled = false;
                            txtFromRDRight.Enabled = false;
                            txtToRDLeft.Enabled = false;
                            txtToRDRight.Enabled = false;
                        }
                        string infrastructurecheck = "";
                        infrastructurecheck = Utility.GetStringValueFromQueryString("InfrastructureType", "");
                        if (infrastructurecheck.Contains("Control Structure1"))
                        {
                            lblIrrigationFromRDs.Text = "";
                            lblIrrigationToRD.Text = "";
                        }
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditProblemFI.Enabled = false;
                        btnDeleteProblemFI.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditProblemFI.Enabled = CanEdit;
                            btnDeleteProblemFI.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditProblemFI.Enabled = false;
                            btnDeleteProblemFI.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditProblemFI.Enabled = CanEdit;
                            btnDeleteProblemFI.Enabled = CanEdit;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Events
    }
}