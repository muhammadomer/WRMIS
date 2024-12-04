using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class ExcludeDivision : BasePage
    {
        #region Grid Data Key Index

        public const int ExcludeDivisionIDIndex = 0;
        public const int IsExcludedIndex = 1;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindScreenDropdowns();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 23-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 26-09-2016
        /// </summary>
        private void BindScreenDropdowns()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);

        }

        /// <summary>
        /// This function binds the channel type dropdown.
        /// Created On 26-09-2016
        /// </summary>


        /// <summary>
        /// This function binds the parent channel dropdown.
        /// Created On 26-09-2016
        /// </summary>


        /// <summary>
        /// This function binds the grid
        /// Created By 29-06-2016
        /// </summary>
        private void BindGrid()
        {
            long ZoneID = -1;
            long CircleID = -1;
            bool IsExcludedDivisions = chkExclude.Checked;
            if (ddlZone.SelectedItem.Value != String.Empty)
            {
                ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
            }
            if (ddlCircle.SelectedItem.Value != String.Empty)
            {
                CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
            }
            List<dynamic> lstExcludedChannel = new PerformanceEvaluationBLL().GetDivisionsReadyToExclude(ZoneID, CircleID, IsExcludedDivisions);
            if (lstExcludedChannel.Count > 0)
            {
                btnSave.Visible = base.CanEdit;
            }
            else
            {
                btnSave.Visible = false;

                if (IsExcludedDivisions)
                {
                    Master.ShowMessage(Message.ExcludedDivisionsNotExists.Description, SiteMaster.MessageType.Error);
                }
            }

            gvExcludeDivision.DataSource = lstExcludedChannel;
            gvExcludeDivision.DataBind();
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == string.Empty)
                {
                    Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    Dropdownlist.DDLCircles(ddlCircle, false, ZoneID, (int)Constants.DropDownFirstOption.All);
                }
                gvExcludeDivision.Visible = false;
                btnSave.Visible = false;

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }





        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvExcludeDivision.PageIndex = 0;
                gvExcludeDivision.Visible = true;
                btnSave.Visible = true;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExcludeChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvExcludeDivision.PageIndex = e.NewPageIndex;
                gvExcludeDivision.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExcludeChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTotalRDs = (Label)e.Row.FindControl("lblTotalRDs");
                    double TotalRDs = Convert.ToDouble(lblTotalRDs.Text);

                    lblTotalRDs.Text = Calculations.GetRDText(TotalRDs);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsRecordSaved = false;

                foreach (GridViewRow Row in gvExcludeDivision.Rows)
                {
                    long DivisionID = Convert.ToInt64(gvExcludeDivision.DataKeys[Row.RowIndex].Values[ExcludeDivisionIDIndex]);
                    bool IsExcludedCurrent = Convert.ToBoolean(gvExcludeDivision.DataKeys[Row.RowIndex].Values[IsExcludedIndex]);
                    bool IsExcluded = ((CheckBox)Row.FindControl("chkIsExcluded")).Checked;
                    PerformanceEvaluationBLL bllPerformanceEvaluation = new PerformanceEvaluationBLL();
                    PE_DivisionExcluded mdlDivisionExcluded = bllPerformanceEvaluation.GetChannelExcludedByDivisionID(DivisionID);
                    UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                    DateTime Now = DateTime.Now;
                    if (mdlDivisionExcluded == null)
                    {
                        if (IsExcluded)
                        {
                            mdlDivisionExcluded = new PE_DivisionExcluded
                            {
                                DivisionID = DivisionID,
                                IsExcluded = IsExcluded,
                                CreatedBy = mdlUsers.ID,
                                ModifiedBy = mdlUsers.ID,
                                CreatedDate = Now,
                                ModifiedDate = Now
                            };

                            IsRecordSaved = bllPerformanceEvaluation.AddDivisionExcluded(mdlDivisionExcluded);
                        }
                    }
                    else
                    {

                        mdlDivisionExcluded.IsExcluded = IsExcluded;
                        mdlDivisionExcluded.ModifiedBy = mdlUsers.ID;
                        mdlDivisionExcluded.ModifiedDate = Now;
                        IsRecordSaved = bllPerformanceEvaluation.UpdateDivisionExcluded(mdlDivisionExcluded);

                    }
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvExcludeDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvExcludeDivision.PageIndex = e.NewPageIndex;
            gvExcludeDivision.EditIndex = -1;

            BindGrid();
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvExcludeDivision.Visible = false;
            btnSave.Visible = false;
        }
    }
}