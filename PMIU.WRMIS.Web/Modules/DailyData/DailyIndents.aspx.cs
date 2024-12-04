using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class DailyIndents : BasePage
    {
        double IndentsTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    btnSearchChannel.Visible = base.CanView;
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.DailyIndents] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                    else
                    {
                        txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        BindDropdowns();
                    }

                    hlPlaceIndent.NavigateUrl = "~/Modules/DailyData/PlacingIndents.aspx";
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDropdowns()
        {
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            //{
            //    List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);
            //    BindChannelsDropdown(lstSubDivision.FirstOrDefault().ID, -1);

            //    ddlSubDivision.DataSource = lstSubDivision;
            //    ddlSubDivision.DataTextField = "NAME";
            //    ddlSubDivision.DataValueField = "ID";
            //    ddlSubDivision.DataBind();
            //    ddlSubDivision.Enabled = false;
            //    ddlChannel.AutoPostBack = false;

            //    ddlSubDivision.CssClass = "form-control";
            //    ddlChannel.CssClass = "form-control";
            //    ddlChannel.Attributes.Remove("required");
            //}
            //else if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
            //{
            //    List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionsOFXENByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);
            //    BindChannelsDropdown(-1, lstSubDivision.FirstOrDefault().DivisionID);

            //    ddlSubDivision.DataSource = lstSubDivision;
            //    ddlSubDivision.DataTextField = "NAME";
            //    ddlSubDivision.DataValueField = "ID";
            //    ddlSubDivision.DataBind();
            //    ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
            //}

            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);
                BindChannelsDropdown(lstSubDivision.FirstOrDefault().ID, -1);

                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
                ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
                //ddlSubDivision.Enabled = false;
                //ddlChannel.AutoPostBack = false;

                //ddlSubDivision.CssClass = "form-control";
                //ddlChannel.CssClass = "form-control";
                //ddlChannel.Attributes.Remove("required");
            }
            else if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionsOFXENByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);
                //BindChannelsDropdown(-1, lstSubDivision.FirstOrDefault().DivisionID);
                //BindChannelsDropdown(lstSubDivision.FirstOrDefault().ID, -1);
                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
                ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
                ddlChannel.Items.Insert(0, new ListItem("Select", ""));
                ddlChannel.Enabled = false;
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlSubDivision.SelectedItem.Value == string.Empty)
                {
                    ddlChannel.SelectedIndex = 0;
                    ddlChannel.Enabled = false;
                }
                else
                {
                    BindChannelsDropdown(Convert.ToInt64(ddlSubDivision.SelectedItem.Value), 0);
                }

                //if (ddlSubDivision.SelectedItem.Value == string.Empty)
                //{
                //    ddlChannel.Enabled = true;
                //    ddlChannel.CssClass = "form-control required";
                //}
                //else
                //{
                //    ddlChannel.Enabled = false;
                //    ddlChannel.CssClass = "form-control";
                //}
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //if (ddlChannel.SelectedItem.Value == string.Empty)
                //{
                //    ddlSubDivision.Enabled = true;
                //    ddlSubDivision.CssClass = "form-control required";
                //}
                //else
                //{
                //    ddlSubDivision.Enabled = false;
                //    ddlSubDivision.CssClass = "form-control";
                //}

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindChannelsDropdown(long? _SubDivisionID, long? _DivisionID)
        {
            //Dropdownlist.DDLChannelsNameBySubDivisionIDOrDivisionID(ddlChannel, _SubDivisionID, _DivisionID);
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            List<dynamic> lstChannelsBySubDivIDAndUserID = bllIndents.GetChannelsByUserIDAndSubDivID(mdlUser.ID, Convert.ToInt64(mdlUser.UA_Designations.IrrigationLevelID), Convert.ToInt64(_SubDivisionID), "P");


            ddlChannel.DataSource = lstChannelsBySubDivIDAndUserID;
            ddlChannel.DataTextField = "ChannelName";
            ddlChannel.DataValueField = "ChannelID";
            ddlChannel.DataBind();
            ddlChannel.Items.Insert(0, new ListItem("Select", ""));
            ddlChannel.Enabled = true;
        }

        private void BindGrid()
        {
            IndentsBLL bllIndents = new IndentsBLL();
            long ChannelID = -1;
            long SubDivisionID = -1;
            DateTime IndentPlacementDate;

            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            {
                ddlChannel.Attributes.Remove("required");

                if (ddlChannel.SelectedItem.Value != string.Empty)
                {
                    ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                    SubDivisionID = -1;
                }
                else
                {
                    ChannelID = -1;
                    SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                }
            }
            else
            {
                if (ddlChannel.SelectedItem.Value != string.Empty)
                {
                    ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                }

                if (ddlSubDivision.SelectedItem.Value != string.Empty)
                {
                    SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                }
            }

            IndentPlacementDate = Convert.ToDateTime(txtDate.Text);

            List<DD_GetDailyIndentsData_Result> lstChannelIndnet = bllIndents.GetIndentsBySubDivisionIDOrChannelID(SubDivisionID, ChannelID, IndentPlacementDate);

            if (SubDivisionID == -1)
            {
                gvByChannel.DataSource = lstChannelIndnet;
                gvByChannel.DataBind();
                gvByChannel.Visible = true;
                gvBySubDivision.Visible = false;
            }
            if (ChannelID == -1)
            {
                gvBySubDivision.DataSource = lstChannelIndnet;
                gvBySubDivision.DataBind();
                gvBySubDivision.Visible = true;
                gvByChannel.Visible = false;
            }
            if (ChannelID != -1 && SubDivisionID != -1)
            {
                gvByChannel.DataSource = lstChannelIndnet;
                gvByChannel.DataBind();
                gvByChannel.Visible = true;
                gvBySubDivision.Visible = false;
            }
        }

        protected void btnSearchChannel_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();

                List<dynamic> SearchCriteria = new List<dynamic>();
                SearchCriteria.Add(ddlSubDivision.SelectedValue);
                SearchCriteria.Add(ddlChannel.SelectedValue);
                SearchCriteria.Add(txtDate.Text);
                Session[SessionValues.DailyIndents] = SearchCriteria;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvBySubDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    double IndentValue = Convert.ToDouble(((Label)e.Row.FindControl("lblIndent")).Text);
                    Label lblIndentDate = (Label)e.Row.FindControl("lblIndentDate");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");

                    if (lblIndentDate.Text != "")
                    {
                        lblIndentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblIndentDate.Text));
                    }

                    if (lblIndent.Text != null)
                    {
                        lblIndent.Text = String.Format("{0:0.00}", (IndentValue));
                    }

                    IndentsTotal = IndentValue + IndentsTotal;

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblSubDivisionalTotal = (Label)e.Row.FindControl("lblSubDivisionalTotal");
                    lblSubDivisionalTotal.Text = String.Format("{0:0.00}", (IndentsTotal));
                    //lblSubDivisionalTotal.Text = Convert.ToString(IndentsTotal);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvByChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblIndentDate = (Label)e.Row.FindControl("lblIndentDate");
                    double IndentValue = Convert.ToDouble(((Label)e.Row.FindControl("lblIndents")).Text);
                    Label lblIndent = (Label)e.Row.FindControl("lblIndents");

                    if (lblIndentDate.Text != "")
                    {
                        lblIndentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblIndentDate.Text));
                    }

                    if (lblIndent.Text != null)
                    {
                        lblIndent.Text = String.Format("{0:0.00}", (IndentValue));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //if (SessionManagerFacade.UserInformation.UA_Designations.ID != Convert.ToInt64(Constants.Designation.SDO))
                //{
                    //ddlSubDivision.SelectedIndex = 0;
                    //ddlSubDivision.Enabled = true;
                    //ddlChannel.Enabled = true;
                    //ddlSubDivision.CssClass = "form-control required";
                    //ddlChannel.CssClass = "form-control required";
                //}
                //else
                //{
                //    ddlChannel.Attributes.Remove("required");
                //}
                ddlSubDivision.SelectedIndex = 0;
                ddlChannel.SelectedIndex = 0;
                ddlChannel.Enabled = false;
                txtDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                gvByChannel.Visible = false;
                gvBySubDivision.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 16/08/2016 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyIndents);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindHistoryData()
        {
            BindDropdowns();

            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.DailyIndents];

            string SubDivID = Convert.ToString(SearchCriteria[0]);
            ddlSubDivision.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlSubDivision, SubDivID.ToString());

            string ChannelID = Convert.ToString(SearchCriteria[1]);
            ddlChannel.ClearSelection();
            Dropdownlist.SetSelectedValue(ddlChannel, ChannelID.ToString());

            //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            //{
            //    ddlSubDivision.Enabled = false;
            //    ddlChannel.Enabled = true;
            //}
            //else
            //{
            //    if (ddlSubDivision.SelectedItem.Value == string.Empty)
            //    {
            //        ddlChannel.Enabled = true;
            //        ddlSubDivision.Enabled = false;
            //        ddlChannel.CssClass = "form-control required";
            //    }
            //    else
            //    {
            //        ddlChannel.Enabled = false;
            //        ddlSubDivision.Enabled = true;
            //        ddlChannel.CssClass = "form-control";
            //    }
            //}


            txtDate.Text = SearchCriteria[2];
            BindGrid();
        }
    }
}