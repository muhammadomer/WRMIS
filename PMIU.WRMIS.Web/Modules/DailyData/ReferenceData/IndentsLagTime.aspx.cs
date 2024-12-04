using PMIU.WRMIS.AppBlocks;
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

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.ReferenceData
{
    public partial class Indents : BasePage
    {
        List<CO_GaugeLag> lstGaugeLag = new List<CO_GaugeLag>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    IndentsBLL bllIndents = new IndentsBLL();
                    ddlChannel.Enabled = false;
                    ddlChannel.Items.Insert(0, new ListItem("Select", ""));
                    //List<CO_SubDivision> lstSubDiv = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, (mdlUser.DesignationID == null ? null : mdlUser.UA_Designations.IrrigationLevelID));
                    //BindIndentsDropdown(mdlUser.ID, (mdlUser.DesignationID == null ? null : mdlUser.UA_Designations.IrrigationLevelID), lstSubDiv.FirstOrDefault().ID);
                    BindSubDivisionDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIndents.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                IndentsBLL bllIndents = new IndentsBLL();
                int RowIndex = e.RowIndex;
                long GaugeLagID = Convert.ToInt32(((Label)gvIndents.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                double Velocity = Convert.ToDouble(((TextBox)gvIndents.Rows[RowIndex].Cells[5].FindControl("txtVelocity")).Text.Trim());
                double Distance = Convert.ToInt32(((Label)gvIndents.Rows[RowIndex].Cells[4].FindControl("lblDistance")).Text);
                double LagTime;
                //double LagTimeInHours;

                if (Velocity >= 0.1 && Velocity <= 15)
                {
                    LagTime = Math.Ceiling((Distance / Velocity) / 3600);
                    //LagTimeInHours = LagTimeInSeconds / 3600;
                }
                else
                {
                    Master.ShowMessage(Message.VelocitySpeed.Description, SiteMaster.MessageType.Success);
                    return;
                }

                CO_GaugeLag mdlGaugeLag = new CO_GaugeLag();
                mdlGaugeLag.ID = GaugeLagID;
                mdlGaugeLag.Velocity = Velocity;
                mdlGaugeLag.LagTime = LagTime;

                bllIndents.UpdateGaugeLag(mdlGaugeLag);
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                gvIndents.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
            }
        }

        protected void gvIndents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvIndents.EditIndex = e.NewEditIndex;
                BindGrid();
                gvIndents.Rows[e.NewEditIndex].FindControl("txtVelocity").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIndents.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvIndents.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindSubDivisionDropdown()
        {
            //IndentsBLL bllIndents = new IndentsBLL();
            //UA_Users mdlUser = SessionManagerFacade.UserInformation;
            //List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


            //ddlSubDivision.DataSource = lstSubDivision;
            //ddlSubDivision.DataTextField = "NAME";
            //ddlSubDivision.DataValueField = "ID";
            //ddlSubDivision.DataBind();
            //ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
            IndentsBLL bllIndents = new IndentsBLL();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SDO))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
                ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
            }
            else if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
            {
                List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionsOFXENByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);


                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataTextField = "NAME";
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataBind();
                ddlSubDivision.Items.Insert(0, new ListItem("Select", ""));
            }
        }

        private void BindGrid()
        {
            long ChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);

            lstGaugeLag = new IndentsBLL().GetGaugeLagByChannelID(ChannelID);
            gvIndents.DataSource = lstGaugeLag;
            gvIndents.DataBind();
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannel.SelectedItem.Value);

                if (ChannelID == -1)
                {
                    gvIndents.Visible = false;
                }
                else
                {
                    gvIndents.EditIndex = -1;
                    BindGrid();
                    gvIndents.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblGaugeAtRD = (Label)e.Row.FindControl("lblGaugeAtRD");
                    Label lblUpperRD = (Label)e.Row.FindControl("lblUpperRD");
                    Label lblLagTime = (Label)e.Row.FindControl("lblLagTime");
                    double LagTime;

                    if (lblGaugeAtRD.Text != String.Empty)
                    {
                        double GaugeAtRD = Convert.ToDouble(lblGaugeAtRD.Text);
                        lblGaugeAtRD.Text = Calculations.GetRDText(GaugeAtRD);
                    }

                    if (lblUpperRD.Text != string.Empty)
                    {
                        double UpperRD = Convert.ToDouble(lblUpperRD.Text);
                        lblUpperRD.Text = Calculations.GetRDText(UpperRD);

                    }
                    LagTime = Convert.ToDouble(lblLagTime.Text);
                    LagTime = Math.Round(LagTime, 2);
                    lblLagTime.Text = Convert.ToString(LagTime);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubDivision.SelectedItem.Value == String.Empty)
                {
                    ddlChannel.SelectedIndex = 0;
                    ddlChannel.Enabled = false;
                }
                else
                {
                    IndentsBLL bllIndents = new IndentsBLL();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    List<CO_SubDivision> lstSubDivision = bllIndents.GetSubDivisionOFSDOByUserID(mdlUser.ID, mdlUser.UA_Designations.IrrigationLevelID);

                    List<dynamic> lstChannelsBySubDivIDAndUserID = bllIndents.GetChannelsByUserIDAndSubDivID(mdlUser.ID, Convert.ToInt64(mdlUser.UA_Designations.IrrigationLevelID), Convert.ToInt64(ddlSubDivision.SelectedItem.Value), null);

                    //lstChannelsBySubDivIDAndUserID = lstChannelsBySubDivIDAndUserID.OrderBy(x => x.ChannelName).ToList();

                    ddlChannel.DataSource = lstChannelsBySubDivIDAndUserID;
                    ddlChannel.DataTextField = "ChannelName";
                    ddlChannel.DataValueField = "ChannelID";
                    ddlChannel.DataBind();
                    ddlChannel.Items.Insert(0, new ListItem("Select", ""));
                    ddlChannel.Enabled = true;
                }
                gvIndents.Visible = false;

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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.IndentsLagTime);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}