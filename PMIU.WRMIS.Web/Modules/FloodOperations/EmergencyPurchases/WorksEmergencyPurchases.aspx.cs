using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.FloodOperations;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class WorksEmergencyPurchases : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    Session["EmergencyPurchaseID"] = "";
                    Session["EmergencyPurchaseID"] = Utility.GetNumericValueFromQueryString("EmergencyPurchaseID", 0);
                    int EmergencyPurchaseID = Utility.GetNumericValueFromQueryString("EmergencyPurchaseID", 0);
                    if (EmergencyPurchaseID > 0)
                    {

                        hdnEmergencyPurchaseID.Value = Convert.ToString(EmergencyPurchaseID);

                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/EmergencyPurchases/SearchEmergencyPurchases.aspx?EmergencyPurchaseID={0}", EmergencyPurchaseID);
                        HeaderDivision_Show(EmergencyPurchaseID);
                        GridFloodFighting_works(EmergencyPurchaseID);
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
        private void GridFloodFighting_works(int EmergencyPurchaseID)
        {
            try
            {

                List<object> lstFo_Epwork = new FloodOperationsBLL().GetFo_EPWorkBy_ID(EmergencyPurchaseID);

                gv_FloodFighting.DataSource = lstFo_Epwork;
                gv_FloodFighting.DataBind();
                gv_FloodFighting.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void HeaderDivision_Show(int EmergencyPurchaseID)
        {
            try
            {

                object lstF_Division = new FloodOperationsBLL().GetFloodFightingDivision_CampSite_By_ID(EmergencyPurchaseID);
                int Struct_type_id = 0;
                int StructureID = 0;
                string StructypeName = string.Empty;

                if (lstF_Division != null)
                {

                    string Divsion = Convert.ToString(lstF_Division.GetType().GetProperty("Name").GetValue(lstF_Division));
                    string Campsite = Convert.ToString(lstF_Division.GetType().GetProperty("Campsite").GetValue(lstF_Division));
                    Struct_type_id = Convert.ToInt32(lstF_Division.GetType().GetProperty("Struct_type_id").GetValue(lstF_Division));
                    StructureID = Convert.ToInt32(lstF_Division.GetType().GetProperty("StructureID").GetValue(lstF_Division));
                    StructypeName = Convert.ToString(lstF_Division.GetType().GetProperty("StructypeName").GetValue(lstF_Division));
                    lbl_division.Text = Divsion;
                    if (Campsite == "True")
                    {
                        lbl_camp_site.Text = "Yes";
                    }
                    else
                    {
                        lbl_camp_site.Text = "No";

                    }


                }

                if (StructypeName.Equals("Protection Infrastructure"))
                {

                    object lstF_InfrastructureName = new FloodOperationsBLL().GetFloodFightingInsfrastructureName(1, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    hdnTotalRD.Value = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("TotalLength").GetValue(lstF_InfrastructureName));


                }
                else if (StructypeName.Equals("Control Structure1"))
                {

                    object lstF_InfrastructureName = new FloodOperationsBLL().GetFloodFightingInsfrastructureName(2, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    hdnStructureType.Value = "Control Structure1";


                }
                else if (StructypeName.Equals("Drain"))
                {
                    object lstF_InfrastructureName = new FloodOperationsBLL().GetFloodFightingInsfrastructureName(3, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    hdnTotalRD.Value = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("TotalLength").GetValue(lstF_InfrastructureName));
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gv_FloodFighting_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv_FloodFighting.EditIndex = -1;
                GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {

                    List<object> lstFo_Epwork = new FloodOperationsBLL().GetFo_EPWorkBy_ID(Convert.ToInt32(hdnEmergencyPurchaseID.Value));
                    lstFo_Epwork.Add(new
                    {
                        ID = 0,
                        EmergencyPurchaseID = 0,
                        NatureOfWorkID = 0,
                        NatureOfWorkName = string.Empty,
                        Description = "",
                        RDtotal = string.Empty,
                        RD = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });


                    gv_FloodFighting.PageIndex = gv_FloodFighting.PageCount;
                    gv_FloodFighting.DataSource = lstFo_Epwork;
                    gv_FloodFighting.DataBind();

                    gv_FloodFighting.EditIndex = gv_FloodFighting.Rows.Count - 1;
                    gv_FloodFighting.DataBind();

                    //   gv_FloodFighting.Rows[gv_FloodFighting.Rows.Count - 1].FindControl("ddlNatureOfWork").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;

                long FO_EpWork_ID = Convert.ToInt64(((Label)gv_FloodFighting.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                TextBox txtRDLeft = ((TextBox)gv_FloodFighting.Rows[RowIndex].Cells[2].FindControl("txtRDLeft"));
                TextBox txtRDRight = ((TextBox)gv_FloodFighting.Rows[RowIndex].Cells[2].FindControl("txtRDRight"));
                string Description = ((TextBox)gv_FloodFighting.Rows[RowIndex].Cells[3].FindControl("txtDesc")).Text.Trim().Replace("'", "");

                DataKey key = gv_FloodFighting.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);


                DropDownList ddlNatureOfWork = (DropDownList)gv_FloodFighting.Rows[RowIndex].Cells[1].FindControl("ddlNatureOfWork");


                FO_EPWork mdlFO_EPWork = new FO_EPWork();

                mdlFO_EPWork.ID = FO_EpWork_ID;
                mdlFO_EPWork.EmergencyPurchaseID = Convert.ToInt64(hdnEmergencyPurchaseID.Value);
                mdlFO_EPWork.NatureOfWorkID = Convert.ToInt64(ddlNatureOfWork.SelectedValue);


                if (txtRDLeft != null & txtRDRight != null)
                {
                    mdlFO_EPWork.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);
                }
                mdlFO_EPWork.Description = Description;

                if (mdlFO_EPWork.ID == 0)
                {
                    mdlFO_EPWork.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFO_EPWork.CreatedDate = DateTime.Now;
                }
                else
                {


                    mdlFO_EPWork.CreatedBy = Convert.ToInt32(CreatedBy);
                    mdlFO_EPWork.CreatedDate = Convert.ToDateTime(CreatedDate);
                    mdlFO_EPWork.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFO_EPWork.ModifiedDate = DateTime.Now;
                }

                /*         if (hdnStructureType.Value != "Control Structure1")
                         {
                             Tuple<string, string> tupleTotalRD = Calculations.GetRDValues(Convert.ToInt64(hdnTotalRD.Value));

                             if (!(Convert.ToInt64(tupleTotalRD.Item1) <= mdlFO_EPWork.RD && Convert.ToInt64(tupleTotalRD.Item2) >= mdlFO_EPWork.RD))
                             {
                                 Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                                 return;
                             }
                         }*/

                //object DivisionObjct = new FloodOperationsBLL().GetEmergency_DivisionStructtypeID(Convert.ToInt64(hdnEmergencyPurchaseID.Value));
                //DataSet DS = new FloodOperationsBLL().FO_irrigationRDs(Convert.ToInt32(DivisionObjct.GetType().GetProperty("DivisionID").GetValue(DivisionObjct)), Convert.ToInt32(DivisionObjct.GetType().GetProperty("StructureID").GetValue(DivisionObjct)), Convert.ToInt32(DivisionObjct.GetType().GetProperty("StructureTypeID").GetValue(DivisionObjct)));
                //if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //{
                //    DataRow DR = DS.Tables[0].Rows[0];
                //    Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //    if (!(mdlFO_EPWork.RD >= Convert.ToInt64(tupleRDs.Item1) && mdlFO_EPWork.RD <= Convert.ToInt64(tupleRDs.Item2)))
                //    {
                //        Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //        return;
                //    }
                //}

                bool IsSave = new FloodOperationsBLL().SaveFloodFightingWorks(mdlFO_EPWork);

                if (IsSave)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (FO_EpWork_ID == 0)
                    {
                        gv_FloodFighting.PageIndex = 0;
                    }

                    gv_FloodFighting.EditIndex = -1;
                    GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));
                }


            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_FloodFighting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string LeftRD = string.Empty;
                string RightRD = string.Empty;
                UA_SystemParameters systemParameters = null;
                bool CanAddEditEP = false;
                int EmergencyPurchaseYear = Utility.GetNumericValueFromQueryString("EmergencyPurchaseYear", 0);
                DisposalEmergencyPurchases.EPYear = EmergencyPurchaseYear;
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                //string StartDate = systemParameters.ParameterValue + "-" + EmergencyPurchaseYear; // 01-Jan
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                //string EndDate = systemParameters.ParameterValue + "-" + EmergencyPurchaseYear;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Button AddWorks = (Button)e.Row.FindControl("AddWorks");
                    AddWorks.Enabled = false;
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        AddWorks.Enabled = true;
                    //    }
                    //}
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EmergencyPurchaseYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        AddWorks.Enabled = CanAddEditEP;
                    }

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (gv_FloodFighting.EditIndex == e.Row.RowIndex)
                    {
                        DataKey key = gv_FloodFighting.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string EmergencyPurchaseID = Convert.ToString(key.Values["EmergencyPurchaseID"]);
                        string RD = Convert.ToString(key.Values["RDtotal"]);
                        string Description = Convert.ToString(key.Values["Description"]);
                        string NatureOfWorkID = Convert.ToString(key.Values["NatureOfWorkID"]);


                        DropDownList ddlNatureOfWork = (DropDownList)e.Row.FindControl("ddlNatureOfWork");
                        TextBox txtRDLeft = (TextBox)e.Row.FindControl("txtRDLeft");
                        TextBox txtRDRight = (TextBox)e.Row.FindControl("txtRDRight");
                        TextBox txtDesc = (TextBox)e.Row.FindControl("txtDesc");

                        Label lblNature_work = (Label)e.Row.FindControl("lblNature_work");

                        //  HyperLink hlDisposalDetails = (HyperLink)e.Row.FindControl("hlDisposalDetails");

                        //hlDisposalDetails.NavigateUrl = "~/Modules/FloodOperations/EmergencyPurchases/DisposalEmergencyPurchases.aspx?EPWorkID=" + ID.ToString();
                        //  hlDisposalDetails.Enabled = false;


                        if (ddlNatureOfWork != null)
                        {
                            Dropdownlist.DDLFo_NatureWork(ddlNatureOfWork);
                            if (!string.IsNullOrEmpty(NatureOfWorkID))
                                Dropdownlist.SetSelectedValue(ddlNatureOfWork, NatureOfWorkID);
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
                    }

                    #region User Role

                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EmergencyPurchaseYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnEdit.Enabled = CanAddEditEP;
                        btnDelete.Enabled = CanAddEditEP;
                    }
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnEdit.Enabled = true;
                    //        btnDelete.Enabled = true;
                    //    }
                    //}

                    #endregion User Role
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    CanAdd = true;
                    //    CanEdit = true;
                    //    CanDelete = true;
                    //}
                    //else
                    //{
                    //    CanAdd = false;
                    //    CanEdit = false;
                    //    CanDelete = false;
                    //}

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_FloodFighting.EditIndex = e.NewEditIndex;

                GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));

                //  gv_FloodFighting.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gv_FloodFighting.DataKeys[e.RowIndex].Values[0]);
                if (!IsValidDelete(Convert.ToInt64(ID)))
                {
                    return;
                }

                bool IsDeleted = new FloodOperationsBLL().DeleteFo_EPWork(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_FloodFighting.PageIndex = e.NewPageIndex;

                GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_FloodFighting_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv_FloodFighting.EditIndex = -1;

                GridFloodFighting_works(Convert.ToInt32(hdnEmergencyPurchaseID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private bool IsValidDelete(long ID)
        {
            FloodOperationsBLL bl = new FloodOperationsBLL();
            bool IsExist = bl.IsFo_EpworkIDExists(ID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }
    }
}