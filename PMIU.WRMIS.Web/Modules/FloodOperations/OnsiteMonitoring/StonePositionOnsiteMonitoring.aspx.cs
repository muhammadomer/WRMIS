using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring
{
    public partial class StonePositionOnsiteMonitoring : BasePage
    {
        //long _StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
        //long _StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
        private int FFPID, StructureID, StructureTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    ShowHeader();
                    SetPageTitle();
                    if (FFPID > 0)
                    {
                        hdnFFPID.Value = Convert.ToString(FFPID);

                        //OMDetail._FFPSPID = _FFPID;
                        BindInfrastructuresGrid(FFPID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/OnsiteMonitoring/SearchOnsiteMonitoring.aspx?FFPID={0}", FFPID);
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
                StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
                StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
                IEnumerable<DataRow> IeFFPCam = new OnsiteMonitoringBLL().FO_OMStonePosition(_FFPID, StructureTypeID, StructureID);
                var LstFFP = IeFFPCam.Select(dataRow => new
                {
                    SDID = dataRow.Field<long>("SDID"),
                    RD = dataRow.Field<Int32>("RD"),
                    QtyOfStone = dataRow.Field<Int32>("QtyOfStone"),
                    OnSiteQty = dataRow.Field<Int32>("OnSiteQty"),
                }).ToList();
                gvStonePosition.DataSource = LstFFP;
                gvStonePosition.DataBind();
                gvStonePosition.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStonePosition.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                UA_SystemParameters systemParameters = null;
                int FFPYear = Utility.GetNumericValueFromQueryString("Year", 0);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvStonePosition.DataKeys[e.Row.RowIndex];
                    string RD = Convert.ToString(key.Values["RD"]);

                    Label lblRD = (Label)e.Row.FindControl("lblRD");
                    Label lbldep = (Label)e.Row.FindControl("lbldep");
                    Label lbl_DeployQty = (Label)e.Row.FindControl("lbl_DeployQty");
                    Label lbl_BalnceQty = (Label)e.Row.FindControl("lbl_BalnceQty");
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");

                    if (lblRD.Text != "")
                    {
                        lblRD.Text = Calculations.GetRDText(Convert.ToInt64(RD));
                    }
                    if (Utility.GetStringValueFromQueryString("InfrastructureType", "") == "Control Structure1")
                    {
                        lblRD.Visible = false;
                    }

                    if (lbldep.Text == "")
                    {
                        lbldep.Text = "0";
                    }
                    if (lbl_DeployQty.Text == "")
                    {
                        lbl_DeployQty.Text = "0";
                    }
                    lbl_BalnceQty.Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(lbl_DeployQty.Text) - Convert.ToDouble(lbldep.Text)));

                    #region User Role

                    btnEdit.Enabled = false;

                    if (new FloodOperationsBLL().CanAddEditOnSiteMonitoring(FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID))
                    {
                        btnEdit.Enabled = true;
                    }

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                    //string StartDate = systemParameters.ParameterValue + "-" + FFPYear; // 15-Jun

                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 15-Oct
                    //string EndDate = systemParameters.ParameterValue + "-" + FFPYear;

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.MA))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnEdit.Enabled = true;
                    //    }
                    //}

                    #endregion User Role
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //protected void gvStonePosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        string ID = Convert.ToString(gvStonePosition.DataKeys[e.RowIndex].Values[0]);
        //        if (!IsValidDelete(Convert.ToInt64(ID)))
        //        {
        //            return;
        //        }

        //        bool IsDeleted = new OnsiteMonitoringBLL().DeleteFFPCampSites(Convert.ToInt64(ID));
        //        if (IsDeleted)
        //        {
        //            BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
        //            Master.ShowMessage(Message.RecordDeleted.Description);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        protected void gvStonePosition_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStonePosition.EditIndex = e.NewEditIndex;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

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
        protected void gvStonePosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStonePosition.PageIndex = e.NewPageIndex;
                gvStonePosition.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStonePosition_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                bool IsSave = false;
                string totalRD = string.Empty;
                string CreatedBy = string.Empty;
                string CreatedDate = string.Empty;
                string MonitoringDate = string.Empty;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"

                DataKey key = gvStonePosition.DataKeys[e.RowIndex];
                string SDID = Convert.ToString(key.Values["SDID"]);
                string RD = Convert.ToString(key.Values["RD"]);

                #endregion "Data Keys"

                #region "Controls"

                GridViewRow row = gvStonePosition.Rows[e.RowIndex];
                TextBox txtonsiteQty = (TextBox)row.FindControl("txtonsiteQty");

                #endregion "Controls"

                object lstF_OMSPID = new OnsiteMonitoringBLL().GETFO_OMStonePositionID(Convert.ToInt64(SDID));
                int OMSPID = 0;
                string StructypeName = string.Empty;

                if (lstF_OMSPID != null)
                {
                    OMSPID = Convert.ToInt32(lstF_OMSPID.GetType().GetProperty("ID").GetValue(lstF_OMSPID));
                    CreatedBy = Convert.ToString(lstF_OMSPID.GetType().GetProperty("CreatedBy").GetValue(lstF_OMSPID));
                    CreatedDate = Convert.ToString(lstF_OMSPID.GetType().GetProperty("CreatedDate").GetValue(lstF_OMSPID));
                    MonitoringDate = Convert.ToString(lstF_OMSPID.GetType().GetProperty("MonitoringDate").GetValue(lstF_OMSPID));
                }

                FO_OMStonePosition objmodel = new FO_OMStonePosition();

                objmodel.ID = Convert.ToInt64(OMSPID);
                objmodel.StoneDeploymentID = Convert.ToInt64(SDID);

                objmodel.OnSiteQty = Convert.ToInt32(txtonsiteQty.Text);

                if (objmodel.ID == 0)
                {
                    objmodel.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    objmodel.CreatedDate = DateTime.Now;
                    objmodel.MonitoringDate = DateTime.Now;
                }
                else
                {
                    objmodel.MonitoringDate = Convert.ToDateTime(MonitoringDate);
                    objmodel.CreatedBy = Convert.ToInt32(CreatedBy);
                    objmodel.CreatedDate = Convert.ToDateTime(CreatedDate);
                    objmodel.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    objmodel.ModifiedDate = DateTime.Now;
                }

                IsSave = new OnsiteMonitoringBLL().SaveFO_OMStonePosition(objmodel);

                if (IsSave == true)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(objmodel.ID) == 0)
                        gvStonePosition.PageIndex = 0;

                    gvStonePosition.EditIndex = -1;
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void ShowHeader()
        {
            FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
            StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
            StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);

            lblYear.Text = Utility.GetStringValueFromQueryString("Year", "");
            lblZone.Text = Utility.GetStringValueFromQueryString("Zone", "");
            lblCircle.Text = Utility.GetStringValueFromQueryString("Circle", "");
            lblDivision.Text = Utility.GetStringValueFromQueryString("Division", "");
            lblinfraname.Text = Utility.GetStringValueFromQueryString("InfrastructureName", "");
        }
    }
}