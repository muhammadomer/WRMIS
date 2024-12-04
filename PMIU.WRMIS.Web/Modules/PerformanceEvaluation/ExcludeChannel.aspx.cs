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
    public partial class ExcludeChannel : BasePage
    {
        #region Grid Data Key Index

        public const int ExcludeChannelIDIndex = 0;
        public const int ChannelIDIndex = 1;
        public const int IsExcludedIndex = 2;

        #endregion

        public bool? AllChecked = null;

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
            Dropdownlist.DDLCommandNames(ddlCommandName, (int)Constants.DropDownFirstOption.All);

            BindChannelTypeDropdown();

            Dropdownlist.DDLFlowTypes(ddlFlowType, (int)Constants.DropDownFirstOption.All);

            BindParentChannelDropdown();
        }

        /// <summary>
        /// This function binds the channel type dropdown.
        /// Created On 26-09-2016
        /// </summary>
        private void BindChannelTypeDropdown()
        {
            ddlChannelType.DataSource = new PerformanceEvaluationBLL().GetChannelTypesForPE();
            ddlChannelType.DataValueField = "ID";
            ddlChannelType.DataTextField = "Name";
            ddlChannelType.DataBind();
            ddlChannelType.Items.Insert(0, new ListItem("All", ""));
        }

        /// <summary>
        /// This function binds the parent channel dropdown.
        /// Created On 26-09-2016
        /// </summary>
        private void BindParentChannelDropdown()
        {
            ddlParentChannel.DataSource = new PerformanceEvaluationBLL().GetParentChannels();
            ddlParentChannel.DataValueField = "ID";
            ddlParentChannel.DataTextField = "NAME";
            ddlParentChannel.DataBind();
            ddlParentChannel.Items.Insert(0, new ListItem("All", ""));
        }

        /// <summary>
        /// This function binds the grid
        /// Created By 29-06-2016
        /// </summary>
        private void BindGrid()
        {
            long ZoneID = -1;
            long CircleID = -1;
            long DivisionID = -1;
            long SubDivisionID = -1;
            long CommandNameID = -1;
            long ChannelTypeID = -1;
            long FlowTypeID = -1;
            long ParentChannelID = -1;

            if (ddlZone.SelectedItem.Value != String.Empty)
            {
                ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
            }

            if (ddlCircle.SelectedItem.Value != String.Empty)
            {
                CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
            }

            if (ddlDivision.SelectedItem.Value != String.Empty)
            {
                DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            }

            if (ddlSubDivision.SelectedItem.Value != String.Empty)
            {
                SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
            }

            if (ddlCommandName.SelectedItem.Value != String.Empty)
            {
                CommandNameID = Convert.ToInt64(ddlCommandName.SelectedItem.Value);
            }

            if (ddlChannelType.SelectedItem.Value != String.Empty)
            {
                ChannelTypeID = Convert.ToInt64(ddlChannelType.SelectedItem.Value);
            }

            if (ddlFlowType.SelectedItem.Value != String.Empty)
            {
                FlowTypeID = Convert.ToInt64(ddlFlowType.SelectedItem.Value);
            }

            if (ddlParentChannel.SelectedItem.Value != String.Empty)
            {
                ParentChannelID = Convert.ToInt64(ddlParentChannel.SelectedItem.Value);
            }

            string ChannelName = txtChannelName.Text.Trim();
            string IMISCode = txtIMISCode.Text.Trim();
            bool GetExcludedChannels = chkExclude.Checked;
            bool GetZeroAuthorizedTail = chkAuthorizedTail.Checked;

            List<dynamic> lstExcludedChannel = new PerformanceEvaluationBLL().GetExcludedChannels(ZoneID, CircleID, DivisionID, SubDivisionID,
                CommandNameID, ChannelTypeID, FlowTypeID, ParentChannelID, ChannelName, IMISCode, GetExcludedChannels, GetZeroAuthorizedTail);

            if (lstExcludedChannel.Count > 0)
            {
                btnSave.Visible = base.CanEdit;
            }
            else
            {
                btnSave.Visible = false;

                if (GetExcludedChannels)
                {
                    Master.ShowMessage(Message.ExcludedChannelsNotExists.Description, SiteMaster.MessageType.Error);
                }
            }

            gvExcludeChannel.DataSource = lstExcludedChannel;
            gvExcludeChannel.DataBind();

            CheckBox chkAll = (CheckBox)gvExcludeChannel.HeaderRow.FindControl("chkAll");
            chkAll.Checked = (AllChecked == null ? false : AllChecked.Value);
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

                Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == string.Empty)
                {
                    Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    Dropdownlist.DDLDivisions(ddlDivision, false, CircleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
                }

                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == string.Empty)
                {
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.All);
                }
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
                gvExcludeChannel.PageIndex = 0;

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
                gvExcludeChannel.PageIndex = e.NewPageIndex;
                gvExcludeChannel.EditIndex = -1;

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
                    #region "Data Keys"
                    DataKey key = gvExcludeChannel.DataKeys[e.Row.RowIndex];
                    long ChannelID = Convert.ToInt64(key.Values["ChannelID"]);
                    long ChannelTotalRD = Convert.ToInt64(key.Values["TotalRDs"]);
                    #endregion "Data Keys"
                    Label lblTotalRDs = (Label)e.Row.FindControl("lblTotalRDs");
                    Label lblTailAnalysis = (Label)e.Row.FindControl("lblTailAnalysis");
                    double TotalRDs = Convert.ToDouble(lblTotalRDs.Text);

                    lblTotalRDs.Text = Calculations.GetRDText(TotalRDs);
                    if (new PerformanceEvaluationBLL().IsTailAnalysisExists(ChannelID, Convert.ToInt64(ChannelTotalRD)))
                    {
                        lblTailAnalysis.Text = "No";
                    }
                    else
                    {
                        lblTailAnalysis.Text = "Yes";
                    }

                    CheckBox chkIsExcluded = (CheckBox)e.Row.FindControl("chkIsExcluded");

                    if (AllChecked == null)
                    {
                        AllChecked = chkIsExcluded.Checked;
                    }
                    else
                    {
                        if (AllChecked == true)
                        {
                            AllChecked = chkIsExcluded.Checked;
                        }
                    }

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

                foreach (GridViewRow Row in gvExcludeChannel.Rows)
                {
                    long ID = Convert.ToInt64(gvExcludeChannel.DataKeys[Row.RowIndex].Values[ExcludeChannelIDIndex]);
                    long ChannelID = Convert.ToInt64(gvExcludeChannel.DataKeys[Row.RowIndex].Values[ChannelIDIndex]);
                    bool IsExcludedCurrent = Convert.ToBoolean(gvExcludeChannel.DataKeys[Row.RowIndex].Values[IsExcludedIndex]);
                    bool IsExcluded = ((CheckBox)Row.FindControl("chkIsExcluded")).Checked;

                    PerformanceEvaluationBLL bllPerformanceEvaluation = new PerformanceEvaluationBLL();

                    PE_ChannelExcluded mdlChannelExcluded = bllPerformanceEvaluation.GetChannelExcludedByChannelID(ChannelID); ;

                    UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                    DateTime Now = DateTime.Now;

                    if (mdlChannelExcluded == null)
                    {
                        if (IsExcluded)
                        {
                            mdlChannelExcluded = new PE_ChannelExcluded
                            {
                                ChannelID = ChannelID,
                                IsExcluded = IsExcluded,
                                CreatedBy = mdlUsers.ID,
                                ModifiedBy = mdlUsers.ID,
                                CreatedDate = Now,
                                ModifiedDate = Now
                            };

                            IsRecordSaved = bllPerformanceEvaluation.AddChannelExcluded(mdlChannelExcluded);
                        }
                    }
                    else
                    {
                        if (IsExcluded != IsExcludedCurrent)
                        {
                            mdlChannelExcluded.IsExcluded = IsExcluded;
                            mdlChannelExcluded.ModifiedBy = mdlUsers.ID;
                            mdlChannelExcluded.ModifiedDate = Now;

                            IsRecordSaved = bllPerformanceEvaluation.UpdateChannelExcluded(mdlChannelExcluded);
                        }
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

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkAll = (CheckBox)sender;

                foreach (GridViewRow Row in gvExcludeChannel.Rows)
                {
                    ((CheckBox)Row.FindControl("chkIsExcluded")).Checked = chkAll.Checked;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void chkIsExcluded_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkAll = (CheckBox)gvExcludeChannel.HeaderRow.FindControl("chkAll");

                bool checkAll = false;

                foreach (GridViewRow Row in gvExcludeChannel.Rows)
                {
                    bool IsCurrentChecked = ((CheckBox)Row.FindControl("chkIsExcluded")).Checked;

                    checkAll = IsCurrentChecked;

                    if (!IsCurrentChecked)
                    {
                        break;
                    }
                }

                chkAll.Checked = checkAll;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}