using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.FloodOperations;



namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class RDWiseConditions : BasePage
    {
        public static string _infrastructureType;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int floodInspectionId = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (floodInspectionId > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = floodInspectionId;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionId);
                        hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(floodInspectionId)).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionId);
                        LoadAllGridViewInformation();
                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    // hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionId);

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Set PageTitle

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set PageTitle

        #region LoadGrid
        private void LoadAllGridViewInformation()
        {
            try
            {
                LoadStonePitchingInformation();
                LoadPitchStoneApronInformation();
                LoadRainCutsInformation();
                LoadErodingAnimalHolesInformation();
                LoadRDMarksInformation();
                LoadEncroachmentInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }


        }
        private void LoadStonePitchingInformation()
        {
            try
            {
                List<object> lstStonePitching = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.StonePitching);
                gvStonePitching.DataSource = lstStonePitching;
                gvStonePitching.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadPitchStoneApronInformation()
        {
            try
            {
                List<object> lstPitchStoneApron = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.PitchStoneAppron);
                gvPitchStoneApron.DataSource = lstPitchStoneApron;
                gvPitchStoneApron.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadRainCutsInformation()
        {
            try
            {
                List<object> lstRainCuts = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.RainCuts);
                gvRainCuts.DataSource = lstRainCuts;
                gvRainCuts.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadErodingAnimalHolesInformation()
        {
            try
            {
                List<object> lstgvErodingAnimalHoles = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.ErodingAnimalHoles);
                gvErodingAnimalHoles.DataSource = lstgvErodingAnimalHoles;
                gvErodingAnimalHoles.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadRDMarksInformation()
        {
            try
            {
                List<object> lstRDMarks = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.RDMarks);
                gvRDMarks.DataSource = lstRDMarks;
                gvRDMarks.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadEncroachmentInformation()
        {
            try
            {
                List<object> lstEncroachment = new FloodInspectionsBLL().GetEncroachmentByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.Enchrochment);
                gvEncroachment.DataSource = lstEncroachment;
                gvEncroachment.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Stone Pitching Gridview Method

        protected void gvStonePitching_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStonePitching.PageIndex = e.NewPageIndex;
                gvStonePitching.EditIndex = -1;
                LoadStonePitchingInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvStonePitching_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStonePitching.EditIndex = -1;
                LoadStonePitchingInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePitching_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddStonePitching")
                {
                    List<object> lstGaugesInformation = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.StonePitching);
                    lstGaugesInformation.Add(new
                    {
                        ID = 0,
                        SideID = string.Empty,
                        SideName = string.Empty,
                        ConditionID = string.Empty,
                        ConditionName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvStonePitching.PageIndex = gvStonePitching.PageCount;
                    gvStonePitching.DataSource = lstGaugesInformation;
                    gvStonePitching.DataBind();


                    gvStonePitching.EditIndex = gvStonePitching.Rows.Count - 1;
                    gvStonePitching.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePitching_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadStonePitchingInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePitching_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStonePitching.EditIndex = e.NewEditIndex;
                LoadStonePitchingInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePitching_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvStonePitching.Rows[e.RowIndex];

                #region "Data Keys"

                DataKey key = gvStonePitching.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvStonePitching.Rows[e.RowIndex];
                DropDownList ddlStonePitchingSide = (DropDownList)Row.FindControl("ddlStonePitchingSide");
                DropDownList ddlStonePitchingCondition = (DropDownList)Row.FindControl("ddlStonePitchingCondition");

                TextBox txtStonePitchingFromRDLeft = (TextBox)Row.FindControl("txtStonePitchingFromRDLeft");
                TextBox txtStonePitchingFromRDRight = (TextBox)Row.FindControl("txtStonePitchingFromRDRight");
                TextBox txtStonePitchingToRDLeft = (TextBox)Row.FindControl("txtStonePitchingToRDLeft");
                TextBox txtStonePitchingToRDRight = (TextBox)Row.FindControl("txtStonePitchingToRDRight");
                TextBox txtStonePitchingRemarks = (TextBox)Row.FindControl("txtStonePitchingRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(1);

                if (ddlStonePitchingSide != null)
                    ObjRDWiseCondition.StonePitchSideID = Convert.ToInt16(ddlStonePitchingSide.SelectedItem.Value);

                if (ddlStonePitchingCondition != null)
                    ObjRDWiseCondition.ConditionID = Convert.ToInt16(ddlStonePitchingCondition.SelectedItem.Value);

                if (txtStonePitchingFromRDLeft != null & txtStonePitchingFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtStonePitchingFromRDLeft.Text, txtStonePitchingFromRDRight.Text);

                if (txtStonePitchingToRDLeft != null & txtStonePitchingToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtStonePitchingToRDLeft.Text, txtStonePitchingToRDRight.Text);

                if (txtStonePitchingRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtStonePitchingRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }


                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvStonePitching.PageIndex = 0;

                    gvStonePitching.EditIndex = -1;
                    LoadStonePitchingInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePitching_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string FromLeftRD = string.Empty;
            string FromRightRD = string.Empty;
            string ToLeftRD = string.Empty;
            string ToRightRD = string.Empty;
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEditInspection = false;

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button btnAddStonePitching = (Button)e.Row.FindControl("btnAddStonePitching");
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddStonePitching.Enabled = false;

                    if (_InspectionTypeID == 1)
                    {
                        CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEditInspection)
                            btnAddStonePitching.Enabled = CanEditInspection;
                        else
                            btnAddStonePitching.Enabled = false;
                    }
                    else
                    {
                        CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEditInspection)
                            btnAddStonePitching.Enabled = CanEditInspection;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditStonePitching = (Button)e.Row.FindControl("btnEditStonePitching");
                    Button btnDeleteStonePitching = (Button)e.Row.FindControl("btnDeleteStonePitching");


                    if (gvStonePitching.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvStonePitching.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string SideID = Convert.ToString(key.Values["SideID"]);
                        string ConditionID = Convert.ToString(key.Values["ConditionID"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlStonePitchingSide = (DropDownList)e.Row.FindControl("ddlStonePitchingSide");
                        DropDownList ddlStonePitchingCondition = (DropDownList)e.Row.FindControl("ddlStonePitchingCondition");

                        TextBox txtStonePitchingFromRDLeft = (TextBox)e.Row.FindControl("txtStonePitchingFromRDLeft");
                        TextBox txtStonePitchingFromRDRight = (TextBox)e.Row.FindControl("txtStonePitchingFromRDRight");
                        TextBox txtStonePitchingToRDLeft = (TextBox)e.Row.FindControl("txtStonePitchingToRDLeft");
                        TextBox txtStonePitchingToRDRight = (TextBox)e.Row.FindControl("txtStonePitchingToRDRight");
                        TextBox txtStonePitchingRemarks = (TextBox)e.Row.FindControl("txtStonePitchingRemarks");


                        #endregion


                        if (ddlStonePitchingSide != null)
                        {
                            Dropdownlist.DDLActiveStonePitchSide(ddlStonePitchingSide, false);
                            if (!string.IsNullOrEmpty(SideID))
                                Dropdownlist.SetSelectedValue(ddlStonePitchingSide, SideID);
                        }
                        if (ddlStonePitchingCondition != null)
                        {
                            Dropdownlist.DDLInspectionConditionsByGroup(ddlStonePitchingCondition, "Stone Pitching", false, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(ConditionID))
                                Dropdownlist.SetSelectedValue(ddlStonePitchingCondition, ConditionID);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtStonePitchingFromRDLeft != null)
                            txtStonePitchingFromRDLeft.Text = FromLeftRD;
                        if (txtStonePitchingFromRDRight != null)
                            txtStonePitchingFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtStonePitchingToRDLeft != null)
                            txtStonePitchingToRDLeft.Text = ToLeftRD;
                        if (txtStonePitchingToRDRight != null)
                            txtStonePitchingToRDRight.Text = ToRightRD;

                        if (txtStonePitchingRemarks != null)
                            txtStonePitchingRemarks.Text = Remarks;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditStonePitching.Enabled = false;
                        btnDeleteStonePitching.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEditInspection)
                        {
                            btnEditStonePitching.Enabled = CanEditInspection;
                            btnDeleteStonePitching.Enabled = CanEditInspection;
                        }
                        else
                        {
                            btnEditStonePitching.Enabled = false;
                            btnDeleteStonePitching.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEditInspection = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEditInspection)
                        {
                            btnEditStonePitching.Enabled = CanEditInspection;
                            btnDeleteStonePitching.Enabled = CanEditInspection;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion Stone Pitching Gridview Method

        #region Pitch Stone Apron Gridview Method
        protected void gvPitchStoneApron_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPitchStoneApron.PageIndex = e.NewPageIndex;
                gvPitchStoneApron.EditIndex = -1;
                LoadPitchStoneApronInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPitchStoneApron.EditIndex = -1;
                LoadPitchStoneApronInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddPitchStoneApron")
                {
                    List<object> lstPitchStoneApronformation = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.PitchStoneAppron);
                    lstPitchStoneApronformation.Add(new
                    {
                        ID = 0,
                        ConditionID = string.Empty,
                        ConditionName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvPitchStoneApron.PageIndex = gvPitchStoneApron.PageCount;
                    gvPitchStoneApron.DataSource = lstPitchStoneApronformation;
                    gvPitchStoneApron.DataBind();

                    gvPitchStoneApron.EditIndex = gvPitchStoneApron.Rows.Count - 1;
                    gvPitchStoneApron.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvPitchStoneApron.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadPitchStoneApronInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvPitchStoneApron.EditIndex = e.NewEditIndex;
                LoadPitchStoneApronInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvPitchStoneApron.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvPitchStoneApron.Rows[e.RowIndex];
                DropDownList ddlPitchStoneApronCondition = (DropDownList)Row.FindControl("ddlPitchStoneApronCondition");

                TextBox txtPitchStoneApronFromRDLeft = (TextBox)Row.FindControl("txtPitchStoneApronFromRDLeft");
                TextBox txtPitchStoneApronFromRDRight = (TextBox)Row.FindControl("txtPitchStoneApronFromRDRight");
                TextBox txtPitchStoneApronToRDLeft = (TextBox)Row.FindControl("txtPitchStoneApronToRDLeft");
                TextBox txtPitchStoneApronToRDRight = (TextBox)Row.FindControl("txtPitchStoneApronToRDRight");
                TextBox txtPitchStoneApronRemarks = (TextBox)Row.FindControl("txtPitchStoneApronRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(3);


                if (ddlPitchStoneApronCondition != null)
                    ObjRDWiseCondition.ConditionID = Convert.ToInt16(ddlPitchStoneApronCondition.SelectedItem.Value);

                if (txtPitchStoneApronFromRDLeft != null & txtPitchStoneApronFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtPitchStoneApronFromRDLeft.Text, txtPitchStoneApronFromRDRight.Text);

                if (txtPitchStoneApronToRDLeft != null & txtPitchStoneApronToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtPitchStoneApronToRDLeft.Text, txtPitchStoneApronToRDRight.Text);

                if (txtPitchStoneApronRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtPitchStoneApronRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }

                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvPitchStoneApron.PageIndex = 0;

                    gvPitchStoneApron.EditIndex = -1;
                    LoadPitchStoneApronInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPitchStoneApron_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    Button btnAddPitchStoneApron = (Button)e.Row.FindControl("btnAddPitchStoneApron");
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddPitchStoneApron.Enabled = false;
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnAddPitchStoneApron.Enabled = CanEdit;
                        else
                            btnAddPitchStoneApron.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnAddPitchStoneApron.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditPitchStoneApron = (Button)e.Row.FindControl("btnEditPitchStoneApron");
                    Button btnDeletePitchStoneApron = (Button)e.Row.FindControl("btnDeletePitchStoneApron");


                    if (gvPitchStoneApron.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvPitchStoneApron.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string ConditionID = Convert.ToString(key.Values["ConditionID"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlPitchStoneApronCondition = (DropDownList)e.Row.FindControl("ddlPitchStoneApronCondition");

                        TextBox txtPitchStoneApronFromRDLeft = (TextBox)e.Row.FindControl("txtPitchStoneApronFromRDLeft");
                        TextBox txtPitchStoneApronFromRDRight = (TextBox)e.Row.FindControl("txtPitchStoneApronFromRDRight");
                        TextBox txtPitchStoneApronToRDLeft = (TextBox)e.Row.FindControl("txtPitchStoneApronToRDLeft");
                        TextBox txtPitchStoneApronToRDRight = (TextBox)e.Row.FindControl("txtPitchStoneApronToRDRight");
                        TextBox txtStonePitchingRemarks = (TextBox)e.Row.FindControl("txtPitchStoneApronRemarks");
                        #endregion


                        if (ddlPitchStoneApronCondition != null)
                        {
                            Dropdownlist.DDLInspectionConditionsByGroup(ddlPitchStoneApronCondition, "Stone Pitching", false, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(ConditionID))
                                Dropdownlist.SetSelectedValue(ddlPitchStoneApronCondition, ConditionID);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtPitchStoneApronFromRDLeft != null)
                            txtPitchStoneApronFromRDLeft.Text = FromLeftRD;
                        if (txtPitchStoneApronFromRDRight != null)
                            txtPitchStoneApronFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtPitchStoneApronToRDLeft != null)
                            txtPitchStoneApronToRDLeft.Text = ToLeftRD;
                        if (txtPitchStoneApronToRDRight != null)
                            txtPitchStoneApronToRDRight.Text = ToRightRD;

                        if (Remarks != null)
                            txtStonePitchingRemarks.Text = Remarks;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditPitchStoneApron.Enabled = false;
                        btnDeletePitchStoneApron.Enabled = false;
                    }

                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditPitchStoneApron.Enabled = CanEdit;
                            btnDeletePitchStoneApron.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditPitchStoneApron.Enabled = false;
                            btnDeletePitchStoneApron.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditPitchStoneApron.Enabled = CanEdit;
                            btnDeletePitchStoneApron.Enabled = CanEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion Pitch Stone Apron Gridview Method

        #region Rain Cuts Gridview Method
        protected void gvRainCuts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRainCuts.PageIndex = e.NewPageIndex;
                gvRainCuts.EditIndex = -1;
                LoadRainCutsInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRainCuts.EditIndex = -1;
                LoadRainCutsInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddRainCuts")
                {
                    List<object> lstRainCutsformation = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.RainCuts);
                    lstRainCutsformation.Add(new
                    {
                        ID = 0,
                        ConditionID = string.Empty,
                        ConditionName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvRainCuts.PageIndex = gvRainCuts.PageCount;
                    gvRainCuts.DataSource = lstRainCutsformation;
                    gvRainCuts.DataBind();

                    gvRainCuts.EditIndex = gvRainCuts.Rows.Count - 1;
                    gvRainCuts.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvRainCuts.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadRainCutsInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvRainCuts.EditIndex = e.NewEditIndex;
                LoadRainCutsInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvRainCuts.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvRainCuts.Rows[e.RowIndex];
                DropDownList ddlRainCutsCondition = (DropDownList)Row.FindControl("ddlRainCutsCondition");

                TextBox txtRainCutsFromRDLeft = (TextBox)Row.FindControl("txtRainCutsFromRDLeft");
                TextBox txtRainCutsFromRDRight = (TextBox)Row.FindControl("txtRainCutsFromRDRight");
                TextBox txtRainCutsToRDLeft = (TextBox)Row.FindControl("txtRainCutsToRDLeft");
                TextBox txtRainCutsToRDRight = (TextBox)Row.FindControl("txtRainCutsToRDRight");
                TextBox txtRainCutsRemarks = (TextBox)Row.FindControl("txtRainCutsRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(4);


                if (ddlRainCutsCondition != null)
                    ObjRDWiseCondition.ConditionID = Convert.ToInt16(ddlRainCutsCondition.SelectedItem.Value);

                if (txtRainCutsFromRDLeft != null & txtRainCutsFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtRainCutsFromRDLeft.Text, txtRainCutsFromRDRight.Text);

                if (txtRainCutsToRDLeft != null & txtRainCutsToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtRainCutsToRDLeft.Text, txtRainCutsToRDRight.Text);

                if (txtRainCutsRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtRainCutsRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }

                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }


                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}
                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvRainCuts.PageIndex = 0;

                    gvRainCuts.EditIndex = -1;
                    LoadRainCutsInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRainCuts_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    Button btnAddRainCuts = (Button)e.Row.FindControl("btnAddRainCuts");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddRainCuts.Enabled = false;

                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnAddRainCuts.Enabled = CanEdit;
                        else
                            btnAddRainCuts.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnAddRainCuts.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditRainCuts = (Button)e.Row.FindControl("btnEditRainCuts");
                    Button btnDeleteRainCuts = (Button)e.Row.FindControl("btnDeleteRainCuts");

                    if (gvRainCuts.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvRainCuts.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string ConditionID = Convert.ToString(key.Values["ConditionID"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlRainCutsCondition = (DropDownList)e.Row.FindControl("ddlRainCutsCondition");

                        TextBox txtRainCutsFromRDLeft = (TextBox)e.Row.FindControl("txtRainCutsFromRDLeft");
                        TextBox txtRainCutsFromRDRight = (TextBox)e.Row.FindControl("txtRainCutsFromRDRight");
                        TextBox txtRainCutsToRDLeft = (TextBox)e.Row.FindControl("txtRainCutsToRDLeft");
                        TextBox txtRainCutsToRDRight = (TextBox)e.Row.FindControl("txtRainCutsToRDRight");
                        TextBox txtRainCutsRemarks = (TextBox)e.Row.FindControl("txtRainCutsRemarks");
                        #endregion


                        if (ddlRainCutsCondition != null)
                        {
                            Dropdownlist.DDLInspectionConditionsByGroup(ddlRainCutsCondition, "Rain Cuts", false, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(ConditionID))
                                Dropdownlist.SetSelectedValue(ddlRainCutsCondition, ConditionID);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtRainCutsFromRDLeft != null)
                            txtRainCutsFromRDLeft.Text = FromLeftRD;
                        if (txtRainCutsFromRDRight != null)
                            txtRainCutsFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtRainCutsToRDLeft != null)
                            txtRainCutsToRDLeft.Text = ToLeftRD;
                        if (txtRainCutsToRDRight != null)
                            txtRainCutsToRDRight.Text = ToRightRD;

                        if (txtRainCutsRemarks != null)
                            txtRainCutsRemarks.Text = Remarks;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditRainCuts.Enabled = false;
                        btnDeleteRainCuts.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditRainCuts.Enabled = CanEdit;
                            btnDeleteRainCuts.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditRainCuts.Enabled = false;
                            btnDeleteRainCuts.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditRainCuts.Enabled = CanEdit;
                            btnDeleteRainCuts.Enabled = CanEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Rain Cuts Gridview Method

        #region Eroding Animal Holes Gridview Method
        protected void gvErodingAnimalHoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvErodingAnimalHoles.PageIndex = e.NewPageIndex;
                gvErodingAnimalHoles.EditIndex = -1;
                LoadErodingAnimalHolesInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvErodingAnimalHoles.EditIndex = -1;
                LoadErodingAnimalHolesInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddErodingAnimalHoles")
                {
                    List<object> lstErodingAnimalHoles = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.ErodingAnimalHoles);
                    lstErodingAnimalHoles.Add(new
                    {
                        ID = 0,
                        ConditionID = string.Empty,
                        ConditionName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvErodingAnimalHoles.PageIndex = gvErodingAnimalHoles.PageCount;
                    gvErodingAnimalHoles.DataSource = lstErodingAnimalHoles;
                    gvErodingAnimalHoles.DataBind();

                    gvErodingAnimalHoles.EditIndex = gvErodingAnimalHoles.Rows.Count - 1;
                    gvErodingAnimalHoles.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvErodingAnimalHoles.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadErodingAnimalHolesInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvErodingAnimalHoles.EditIndex = e.NewEditIndex;
                LoadErodingAnimalHolesInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvErodingAnimalHoles.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvErodingAnimalHoles.Rows[e.RowIndex];
                DropDownList ddlErodingAnimalHolesCondition = (DropDownList)Row.FindControl("ddlErodingAnimalHolesCondition");

                TextBox txtErodingAnimalHolesFromRDLeft = (TextBox)Row.FindControl("txtErodingAnimalHolesFromRDLeft");
                TextBox txtErodingAnimalHolesFromRDRight = (TextBox)Row.FindControl("txtErodingAnimalHolesFromRDRight");
                TextBox txtErodingAnimalHolesToRDLeft = (TextBox)Row.FindControl("txtErodingAnimalHolesToRDLeft");
                TextBox txtErodingAnimalHolesToRDRight = (TextBox)Row.FindControl("txtErodingAnimalHolesToRDRight");
                TextBox txtErodingAnimalHolesRemarks = (TextBox)Row.FindControl("txtErodingAnimalHolesRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(2);


                if (ddlErodingAnimalHolesCondition != null)
                    ObjRDWiseCondition.ConditionID = Convert.ToInt16(ddlErodingAnimalHolesCondition.SelectedItem.Value);

                if (txtErodingAnimalHolesFromRDLeft != null & txtErodingAnimalHolesFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtErodingAnimalHolesFromRDLeft.Text, txtErodingAnimalHolesFromRDRight.Text);

                if (txtErodingAnimalHolesToRDLeft != null & txtErodingAnimalHolesToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtErodingAnimalHolesToRDLeft.Text, txtErodingAnimalHolesToRDRight.Text);

                if (txtErodingAnimalHolesRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtErodingAnimalHolesRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }

                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }


                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}
                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvErodingAnimalHoles.PageIndex = 0;

                    gvErodingAnimalHoles.EditIndex = -1;
                    LoadErodingAnimalHolesInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvErodingAnimalHoles_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    Button btnAddErodingAnimalHoles = (Button)e.Row.FindControl("btnAddErodingAnimalHoles");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddErodingAnimalHoles.Enabled = false;
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnAddErodingAnimalHoles.Enabled = CanEdit;
                        else
                            btnAddErodingAnimalHoles.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnAddErodingAnimalHoles.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditErodingAnimalHoles = (Button)e.Row.FindControl("btnEditErodingAnimalHoles");
                    Button btnDeleteErodingAnimalHoles = (Button)e.Row.FindControl("btnDeleteErodingAnimalHoles");




                    if (gvErodingAnimalHoles.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvErodingAnimalHoles.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string ConditionID = Convert.ToString(key.Values["ConditionID"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlErodingAnimalHolesCondition = (DropDownList)e.Row.FindControl("ddlErodingAnimalHolesCondition");

                        TextBox txtErodingAnimalHolesFromRDLeft = (TextBox)e.Row.FindControl("txtErodingAnimalHolesFromRDLeft");
                        TextBox txtErodingAnimalHolesFromRDRight = (TextBox)e.Row.FindControl("txtErodingAnimalHolesFromRDRight");
                        TextBox txtErodingAnimalHolesToRDLeft = (TextBox)e.Row.FindControl("txtErodingAnimalHolesToRDLeft");
                        TextBox txtErodingAnimalHolesToRDRight = (TextBox)e.Row.FindControl("txtErodingAnimalHolesToRDRight");
                        TextBox txtErodingAnimalHolesRemarks = (TextBox)e.Row.FindControl("txtErodingAnimalHolesRemarks");
                        #endregion


                        if (ddlErodingAnimalHolesCondition != null)
                        {
                            Dropdownlist.DDLInspectionConditionsByGroup(ddlErodingAnimalHolesCondition, "Rain Cuts", false, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(ConditionID))
                                Dropdownlist.SetSelectedValue(ddlErodingAnimalHolesCondition, ConditionID);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtErodingAnimalHolesFromRDLeft != null)
                            txtErodingAnimalHolesFromRDLeft.Text = FromLeftRD;
                        if (txtErodingAnimalHolesFromRDRight != null)
                            txtErodingAnimalHolesFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtErodingAnimalHolesToRDLeft != null)
                            txtErodingAnimalHolesToRDLeft.Text = ToLeftRD;
                        if (txtErodingAnimalHolesToRDRight != null)
                            txtErodingAnimalHolesToRDRight.Text = ToRightRD;

                        if (txtErodingAnimalHolesRemarks != null)
                            txtErodingAnimalHolesRemarks.Text = Remarks;
                    }

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditErodingAnimalHoles.Enabled = false;
                        btnDeleteErodingAnimalHoles.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditErodingAnimalHoles.Enabled = CanEdit;
                            btnDeleteErodingAnimalHoles.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditErodingAnimalHoles.Enabled = false;
                            btnDeleteErodingAnimalHoles.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditErodingAnimalHoles.Enabled = CanEdit;
                            btnDeleteErodingAnimalHoles.Enabled = CanEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Eroding Animal Holes Gridview Method

        #region RD Marks Gridview Method
        protected void gvRDMarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRDMarks.PageIndex = e.NewPageIndex;
                gvRDMarks.EditIndex = -1;
                LoadRDMarksInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRDMarks.EditIndex = -1;
                LoadRDMarksInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddRDMarks")
                {
                    List<object> lstRDMarksformation = new FloodInspectionsBLL().GetRDWiseConditionForStonePitchingApronByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.RDMarks);
                    lstRDMarksformation.Add(new
                    {
                        ID = 0,
                        ConditionID = string.Empty,
                        ConditionName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvRDMarks.PageIndex = gvRDMarks.PageCount;
                    gvRDMarks.DataSource = lstRDMarksformation;
                    gvRDMarks.DataBind();

                    gvRDMarks.EditIndex = gvRDMarks.Rows.Count - 1;
                    gvRDMarks.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvRDMarks.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadRDMarksInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvRDMarks.EditIndex = e.NewEditIndex;
                LoadRDMarksInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvRDMarks.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvRDMarks.Rows[e.RowIndex];
                DropDownList ddlRDMarksCondition = (DropDownList)Row.FindControl("ddlRDMarksCondition");

                TextBox txtRDMarksFromRDLeft = (TextBox)Row.FindControl("txtRDMarksFromRDLeft");
                TextBox txtRDMarksFromRDRight = (TextBox)Row.FindControl("txtRDMarksFromRDRight");
                TextBox txtRDMarksToRDLeft = (TextBox)Row.FindControl("txtRDMarksToRDLeft");
                TextBox txtRDMarksToRDRight = (TextBox)Row.FindControl("txtRDMarksToRDRight");
                TextBox txtRDMarksRemarks = (TextBox)Row.FindControl("txtRDMarksRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(5);


                if (ddlRDMarksCondition != null)
                    ObjRDWiseCondition.ConditionID = Convert.ToInt16(ddlRDMarksCondition.SelectedItem.Value);

                if (txtRDMarksFromRDLeft != null & txtRDMarksFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtRDMarksFromRDLeft.Text, txtRDMarksFromRDRight.Text);

                if (txtRDMarksToRDLeft != null & txtRDMarksToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtRDMarksToRDLeft.Text, txtRDMarksToRDRight.Text);

                if (txtRDMarksRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtRDMarksRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }

                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }


                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}
                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvRDMarks.PageIndex = 0;

                    gvRDMarks.EditIndex = -1;
                    LoadRDMarksInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRDMarks_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    Button btnAddRDMarks = (Button)e.Row.FindControl("btnAddRDMarks");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddRDMarks.Enabled = false;
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnAddRDMarks.Enabled = CanEdit;
                        else
                            btnAddRDMarks.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnAddRDMarks.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditRDMarks = (Button)e.Row.FindControl("btnEditRDMarks");
                    Button btnDeleteRDMarks = (Button)e.Row.FindControl("btnDeleteRDMarks");
                   

                    if (gvRDMarks.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvRDMarks.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string ConditionID = Convert.ToString(key.Values["ConditionID"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlRDMarksCondition = (DropDownList)e.Row.FindControl("ddlRDMarksCondition");

                        TextBox txtRDMarksFromRDLeft = (TextBox)e.Row.FindControl("txtRDMarksFromRDLeft");
                        TextBox txtRDMarksFromRDRight = (TextBox)e.Row.FindControl("txtRDMarksFromRDRight");
                        TextBox txtRDMarksToRDLeft = (TextBox)e.Row.FindControl("txtRDMarksToRDLeft");
                        TextBox txtRDMarksToRDRight = (TextBox)e.Row.FindControl("txtRDMarksToRDRight");
                        TextBox txtRDMarksRemarks = (TextBox)e.Row.FindControl("txtRDMarksRemarks");
                        #endregion


                        if (ddlRDMarksCondition != null)
                        {
                            Dropdownlist.DDLInspectionConditionsByGroup(ddlRDMarksCondition, "Stone Pitching", false, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(ConditionID))
                                Dropdownlist.SetSelectedValue(ddlRDMarksCondition, ConditionID);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtRDMarksFromRDLeft != null)
                            txtRDMarksFromRDLeft.Text = FromLeftRD;
                        if (txtRDMarksFromRDRight != null)
                            txtRDMarksFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtRDMarksToRDLeft != null)
                            txtRDMarksToRDLeft.Text = ToLeftRD;
                        if (txtRDMarksToRDRight != null)
                            txtRDMarksToRDRight.Text = ToRightRD;

                        if (txtRDMarksRemarks != null)
                            txtRDMarksRemarks.Text = Remarks;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditRDMarks.Enabled = false;
                        btnDeleteRDMarks.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditRDMarks.Enabled = CanEdit;
                            btnDeleteRDMarks.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditRDMarks.Enabled = false;
                            btnDeleteRDMarks.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditRDMarks.Enabled = CanEdit;
                            btnDeleteRDMarks.Enabled = CanEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion R D Marks Gridview Method

        #region Encroachment Gridview Method
        protected void gvEncroachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvEncroachment.PageIndex = e.NewPageIndex;
                gvEncroachment.EditIndex = -1;
                LoadEncroachmentInformation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvEncroachment.EditIndex = -1;
                LoadEncroachmentInformation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddEncroachment")
                {
                    List<object> lstEncroachment = new FloodInspectionsBLL().GetEncroachmentByID(Convert.ToInt64(hdnFloodInspectionsID.Value), (int)Constants.RDWiseType.Enchrochment);
                    lstEncroachment.Add(new
                    {
                        ID = 0,
                        EncroachmentTypeId = string.Empty,
                        EncroachmentTypeName = string.Empty,
                        Remarks = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now

                    });


                    gvEncroachment.PageIndex = gvEncroachment.PageCount;
                    gvEncroachment.DataSource = lstEncroachment;
                    gvEncroachment.DataBind();

                    gvEncroachment.EditIndex = gvEncroachment.Rows.Count - 1;
                    gvEncroachment.DataBind();
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RDWiseConditionID = Convert.ToString(gvEncroachment.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new FloodInspectionsBLL().DeleteRDWiseCondition(Convert.ToInt64(RDWiseConditionID));
                if (IsDeleted)
                {
                    LoadEncroachmentInformation();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvEncroachment.EditIndex = e.NewEditIndex;
                LoadEncroachmentInformation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvEncroachment.DataKeys[e.RowIndex];
                string RDWiseConditionID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);

                #endregion

                #region "Controls"
                GridViewRow Row = gvEncroachment.Rows[e.RowIndex];
                DropDownList ddlEncroachmentType = (DropDownList)Row.FindControl("ddlEncroachmentType");

                TextBox txtEncroachmentFromRDLeft = (TextBox)Row.FindControl("txtEncroachmentFromRDLeft");
                TextBox txtEncroachmentFromRDRight = (TextBox)Row.FindControl("txtEncroachmentFromRDRight");
                TextBox txtEncroachmentToRDLeft = (TextBox)Row.FindControl("txtEncroachmentToRDLeft");
                TextBox txtEncroachmentToRDRight = (TextBox)Row.FindControl("txtEncroachmentToRDRight");
                TextBox txtEncroachmentRemarks = (TextBox)Row.FindControl("txtEncroachmentRemarks");
                #endregion

                // string irrigationBoundariesID = Convert.ToString(gvStonePitching.DataKeys[e.RowIndex].Values[0]);

                FO_IRDWiseCondition ObjRDWiseCondition = new FO_IRDWiseCondition();

                ObjRDWiseCondition.ID = Convert.ToInt64(RDWiseConditionID);
                ObjRDWiseCondition.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                ObjRDWiseCondition.RDWiseTypeID = Convert.ToInt16(6);


                if (ddlEncroachmentType != null)
                    ObjRDWiseCondition.EncroachmentTypeID = Convert.ToInt16(ddlEncroachmentType.SelectedItem.Value);

                if (txtEncroachmentFromRDLeft != null & txtEncroachmentFromRDRight != null)
                    ObjRDWiseCondition.FromRD = Calculations.CalculateTotalRDs(txtEncroachmentFromRDLeft.Text, txtEncroachmentFromRDRight.Text);

                if (txtEncroachmentToRDLeft != null & txtEncroachmentToRDRight != null)
                    ObjRDWiseCondition.ToRD = Calculations.CalculateTotalRDs(txtEncroachmentToRDLeft.Text, txtEncroachmentToRDRight.Text);

                if (txtEncroachmentRemarks != null)
                    ObjRDWiseCondition.Remarks = Convert.ToString(txtEncroachmentRemarks.Text);

                if (ObjRDWiseCondition.ID == 0)
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.CreatedDate = DateTime.Now;
                }
                else
                {
                    ObjRDWiseCondition.CreatedBy = Convert.ToInt32(CreatedBy);
                    ObjRDWiseCondition.CreatedDate = Convert.ToDateTime(CreatedDate);
                    ObjRDWiseCondition.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    ObjRDWiseCondition.ModifiedDate = DateTime.Now;
                }

                if (_infrastructureType == "Drain")
                {
                    if (ObjRDWiseCondition.ToRD >= ObjRDWiseCondition.FromRD)
                    {
                        Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else if (ObjRDWiseCondition.FromRD >= ObjRDWiseCondition.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS =
                //        new FloodOperationsBLL().FO_irrigationRDs(
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //            Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));

                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (
                //            !((ObjRDWiseCondition.FromRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //              &&
                //              (ObjRDWiseCondition.ToRD >= Convert.ToInt64(tupleRDs.Item1) &&
                //               ObjRDWiseCondition.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                bool IsSave = new FloodInspectionsBLL().SaveIRDWiseConditionForStonePitching(ObjRDWiseCondition);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ObjRDWiseCondition.ID) == 0)
                        gvEncroachment.PageIndex = 0;

                    gvEncroachment.EditIndex = -1;
                    LoadEncroachmentInformation();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEncroachment_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    Button btnAddEncroachment = (Button)e.Row.FindControl("btnAddEncroachment");

                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                        btnAddEncroachment.Enabled = false;
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                            btnAddEncroachment.Enabled = CanEdit;
                        else
                            btnAddEncroachment.Enabled = false;
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                            btnAddEncroachment.Enabled = CanEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditEncroachment = (Button)e.Row.FindControl("btnEditEncroachment");
                    Button btnDeleteEncroachment = (Button)e.Row.FindControl("btnDeleteEncroachment");
                   
                    if (gvEncroachment.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"

                        DataKey key = gvEncroachment.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string FromRD = Convert.ToString(key.Values["FromRDTotal"]);
                        string ToRD = Convert.ToString(key.Values["ToRDTotal"]);
                        string EncroachmentTypeId = Convert.ToString(key.Values["EncroachmentTypeId"]);
                        string Remarks = Convert.ToString(key.Values["Remarks"]);

                        #endregion

                        #region "Controls"
                        DropDownList ddlEncroachmentType = (DropDownList)e.Row.FindControl("ddlEncroachmentType");

                        TextBox txtEncroachmentFromRDLeft = (TextBox)e.Row.FindControl("txtEncroachmentFromRDLeft");
                        TextBox txtEncroachmentFromRDRight = (TextBox)e.Row.FindControl("txtEncroachmentFromRDRight");
                        TextBox txtEncroachmentToRDLeft = (TextBox)e.Row.FindControl("txtEncroachmentToRDLeft");
                        TextBox txtEncroachmentToRDRight = (TextBox)e.Row.FindControl("txtEncroachmentToRDRight");
                        TextBox txtEncroachmentRemarks = (TextBox)e.Row.FindControl("txtEncroachmentRemarks");
                        #endregion


                        if (ddlEncroachmentType != null)
                        {
                            Dropdownlist.DDLActiveEncroachmentType(ddlEncroachmentType, false);
                            if (!string.IsNullOrEmpty(EncroachmentTypeId))
                                Dropdownlist.SetSelectedValue(ddlEncroachmentType, EncroachmentTypeId);
                        }


                        // Check From RD is not null
                        if (!string.IsNullOrEmpty(FromRD))
                        {
                            Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(FromRD));
                            FromLeftRD = tupleFromRD.Item1;
                            FromRightRD = tupleFromRD.Item2;
                        }

                        if (txtEncroachmentFromRDLeft != null)
                            txtEncroachmentFromRDLeft.Text = FromLeftRD;
                        if (txtEncroachmentFromRDRight != null)
                            txtEncroachmentFromRDRight.Text = FromRightRD;

                        // Check To RD is not null
                        if (!string.IsNullOrEmpty(ToRD))
                        {
                            Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(ToRD));
                            ToLeftRD = tupleToRD.Item1;
                            ToRightRD = tupleToRD.Item2;
                        }
                        if (txtEncroachmentToRDLeft != null)
                            txtEncroachmentToRDLeft.Text = ToLeftRD;
                        if (txtEncroachmentToRDRight != null)
                            txtEncroachmentToRDRight.Text = ToRightRD;

                        if (txtEncroachmentRemarks != null)
                            txtEncroachmentRemarks.Text = Remarks;
                    }
                    if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                    {
                        btnEditEncroachment.Enabled = false;
                        btnDeleteEncroachment.Enabled = false;
                    }
                    if (_InspectionTypeID == 1)
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                        if (CanEdit)
                        {
                            btnEditEncroachment.Enabled = CanEdit;
                            btnDeleteEncroachment.Enabled = CanEdit;
                        }
                        else
                        {
                            btnEditEncroachment.Enabled = false;
                            btnDeleteEncroachment.Enabled = false;
                        }
                    }
                    else
                    {
                        CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                        if (CanEdit)
                        {
                            btnEditEncroachment.Enabled = CanEdit;
                            btnDeleteEncroachment.Enabled = CanEdit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Encroachment Gridview Method


    }
}