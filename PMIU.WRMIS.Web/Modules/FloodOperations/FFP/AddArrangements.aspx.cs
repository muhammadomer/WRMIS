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
    public partial class AddArrangements : BasePage
    {
        private List<FO_FFPArrangements> lstArrangements = new List<FO_FFPArrangements>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long _FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);

                    hdnFFPID.Value = Convert.ToString(_FFPID);
                    SetPageTitle();
                    hdnFFPStatus.Value = new FloodFightingPlanBLL().GetFFPStatus(_FFPID).ToString();
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    btnSave.Visible = true;
                    //}
                    //else
                    //{
                    //    btnSave.Visible = false;
                    //}
                    BindGrid();
                    FFPDetail._FFPID = _FFPID;
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/SearchFFP.aspx?FFPID={0}", _FFPID);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstArrangements = new FloodFightingPlanBLL().GetAllArangementsByFFPID(Convert.ToInt64(hdnFFPID.Value));
                    FO_FFPArrangements mdlFundingSource = new FO_FFPArrangements();

                    mdlFundingSource.ID = 0;
                    mdlFundingSource.FFPID = 0;
                    mdlFundingSource.FFPArrangementTypeID = 0;
                    mdlFundingSource.Description = "";
                    lstArrangements.Add(mdlFundingSource);

                    gvArrangements.PageIndex = gvArrangements.PageCount;
                    gvArrangements.DataSource = lstArrangements;
                    gvArrangements.DataBind();

                    gvArrangements.EditIndex = gvArrangements.Rows.Count - 1;
                    gvArrangements.DataBind();
                    gvArrangements.Rows[gvArrangements.Rows.Count - 1].FindControl("ddlArrangementType").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);
                //if (e.Row.RowType == DataControlRowType.Header)
                //{
                //    if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED")
                //    {
                //        Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                //        btnAdd.Visible = false;
                //        if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                //        {
                //            btnAdd.Visible = true;
                //        }
                //    }
                //}

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED")
                    //{
                    //    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    //    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    //    btnEdit.Visible = false;
                    //    btnDelete.Visible = false;
                    //    if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                    //    {
                    //        btnEdit.Visible = true;
                    //        btnDelete.Visible = true;
                    //    }
                    //}

                    if (gvArrangements.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlArrangementType = (DropDownList)e.Row.FindControl("ddlArrangementType");
                        Dropdownlist.DDLArrangementTypes(ddlArrangementType);

                        Label lblHdnLevel = (Label)e.Row.FindControl("lblHdnLevel");
                        ddlArrangementType.ClearSelection();
                        if (lblHdnLevel != null)
                        {
                            Dropdownlist.SetSelectedValue(ddlArrangementType, lblHdnLevel.Text);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvArrangements.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                //FFPID
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long ArrangementID = Convert.ToInt32(((Label)gvArrangements.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                short ddlArrangementType = ((DropDownList)gvArrangements.Rows[RowIndex].Cells[1].FindControl("ddlArrangementType")).SelectedValue == string.Empty ? (short)-1 : Convert.ToInt16(((DropDownList)gvArrangements.Rows[RowIndex].Cells[1].FindControl("ddlArrangementType")).SelectedValue);
                string Description = ((TextBox)gvArrangements.Rows[RowIndex].Cells[2].FindControl("txtDescription")).Text.Trim();
                long FFPID = 0;
                FloodFightingPlanBLL bllFloodFightingPlan = new FloodFightingPlanBLL();
                if (!string.IsNullOrEmpty(Request.QueryString["FFPID"]))
                {
                    FFPID = Convert.ToInt64(Request.QueryString["FFPID"]);
                }
                //FO_FFPArrangements mdlSearchedFundingSource = bllFloodFightingPlan;

                if (ArrangementID == 0)
                {
                    if (new FloodFightingPlanBLL().IsFFPArrangementsExist(FFPID, ddlArrangementType))
                    {
                        Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                else
                {
                    if (new FloodFightingPlanBLL().IsFFPArrangementsExistOnUpdate(FFPID, ddlArrangementType, ArrangementID))
                    {
                        Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                FO_FFPArrangements mdlArrangements = new FO_FFPArrangements();

                mdlArrangements.ID = ArrangementID;
                mdlArrangements.FFPID = FFPID;
                mdlArrangements.FFPArrangementTypeID = ddlArrangementType;
                mdlArrangements.Description = Description;

                bool IsRecordSaved = false;

                if (ArrangementID == 0)
                {
                    mdlArrangements.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlArrangements.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllFloodFightingPlan.AddArrangements(mdlArrangements);
                }
                else
                {
                    mdlArrangements.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlArrangements.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllFloodFightingPlan.UpdateArrangements(mdlArrangements);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (ArrangementID == 0)
                    {
                        gvArrangements.PageIndex = 0;
                    }
                    gvArrangements.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvArrangements.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvArrangements_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvArrangements.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            try
            {
                bool CanAddEditFFP = false;
                UA_SystemParameters systemParameters = null;
                int _FFPYear = Utility.GetNumericValueFromQueryString("FFPYear", 0);
                IEnumerable<DataRow> IeArrangementType = new FloodFightingPlanBLL().GetFFPArrangement(Convert.ToInt32(hdnFFPID.Value));
                var LstItem = IeArrangementType.Select(dataRow => new
                {
                    FFPArrangeTypeName = dataRow.Field<string>("FFPArrangeTypeName"),
                    FFPArrangeTypeID = dataRow.Field<Int16>("FFPArrangeTypeID"),
                    Description = dataRow.Field<string>("Description"),
                    FFPArrangeID = dataRow.Field<long?>("FFPArrangeID"),
                }).ToList();
                gvArrangements.DataSource = LstItem;
                gvArrangements.DataBind();
                gvArrangements.Enabled = true;
                if (gvArrangements.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
                if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED")
                {
                    gvArrangements.Enabled = false;
                    btnSave.Enabled = false;
                    btnSave.Visible = false;

                    //if (hdnFFPStatus.Value.Trim().ToUpper() == "PUBLISHED" && SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DF) && DateTime.Now.Year == _FFPYear)
                    //{
                    //    gvArrangements.Enabled = true;
                    //    btnSave.Enabled = true;
                    //    btnSave.Visible = true;
                    //}
                }
                if (Convert.ToString(hdnFFPStatus.Value).ToUpper() == "DRAFT")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Draft");
                    if (CanAddEditFFP)
                    {
                        gvArrangements.Enabled = CanAddEditFFP;
                        btnSave.Enabled = CanAddEditFFP;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        gvArrangements.Enabled = false;
                        btnSave.Enabled = false;
                        btnSave.Visible = false;

                    }
                }
                else if (Convert.ToString(hdnFFPStatus.Value).ToUpper() == "PUBLISHED")
                {
                    CanAddEditFFP = new FloodFightingPlanBLL().CanAddEditFFP(_FFPYear, SessionManagerFacade.UserInformation.UA_Designations.ID, "Published");
                    if (CanAddEditFFP)
                    {
                        gvArrangements.Enabled = CanAddEditFFP;
                        btnSave.Enabled = CanAddEditFFP;
                        btnSave.Visible = true;
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
            Master.PageTitle = pageTitle.Item1 + " - Arrangements For Flood Fighting Plan";
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                FloodFightingPlanBLL bllFloodFightingPlan = new FloodFightingPlanBLL();
                bool IsSave = false;
                for (int m = 0; m < gvArrangements.Rows.Count; m++)
                {
                    try
                    {
                        //   long ArrangementID = Convert.ToInt32(((Label)gvArrangements.Rows[m].Cells[0].FindControl("lblID")).Text);
                        TextBox txtDescription = (TextBox)gvArrangements.Rows[m].FindControl("txtDescription");
                        string FFPArrangementTypeID = gvArrangements.DataKeys[m].Values[0].ToString();
                        long ArrangementID = Convert.ToInt32(gvArrangements.DataKeys[m].Values[3].ToString());

                        if (txtDescription.Text != "")
                        {
                            FO_FFPArrangements mdlArrangements = new FO_FFPArrangements();

                            mdlArrangements.ID = ArrangementID;
                            mdlArrangements.FFPID = Convert.ToInt64(hdnFFPID.Value);
                            mdlArrangements.FFPArrangementTypeID = Convert.ToInt16(FFPArrangementTypeID);
                            mdlArrangements.Description = txtDescription.Text;

                            if (ArrangementID == 0)
                            {
                                mdlArrangements.CreatedBy = Convert.ToInt32(mdlUser.ID);
                                mdlArrangements.CreatedDate = DateTime.Today;
                                IsSave = bllFloodFightingPlan.AddArrangements(mdlArrangements);
                            }
                            else
                            {
                                mdlArrangements.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                                mdlArrangements.ModifiedDate = DateTime.Today;
                                IsSave = bllFloodFightingPlan.UpdateArrangements(mdlArrangements);
                            }
                        }
                        else
                        {
                            if (ArrangementID != 0)
                            {
                                bool IsDeleted = bllFloodFightingPlan.DeleteArrangements(ArrangementID);
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
                        BindGrid();
                    }
                    else
                    {
                        Master.ShowMessage("Please Enter Description", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

            ////////////////////////////////////////////////////////////////////
        }
    }
}