using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.Reach;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach
{
    // Enum used for pages links
    public enum PageNames
    {
        LSectionParameters,
        LSectionHistory
    }
    public partial class DefineChannelReach : BasePage
    {
        private List<object> ChannelReach
        {
            get
            {
                if (this.ViewState["lstChannelReach"] == null)
                {
                    this.ViewState["lstChannelReach"] = new List<object>();
                }
                return (List<object>)(this.ViewState["lstChannelReach"]);
            }

            set { this.ViewState["lstChannelReach"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    string channelID = Request.QueryString["ChannelID"];

                    if (!string.IsNullOrEmpty(channelID))
                    {
                        hdnChannelID.Value = Convert.ToString(channelID);

                        LoadChannelReach(Convert.ToInt64(channelID));

                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}", channelID);
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DefineChannelReach);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadChannelReach(long _ChannelID)
        {
            try
            {
                LoadChannelDetail(_ChannelID);
                BindChannelReachGrid(_ChannelID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadChannelDetail(long _ChannelID)
        {
            try
            {
                CO_Channel channel = new ChannelBLL().GetChannelByID(_ChannelID);
                lblChannelName.Text = channel.NAME;
                lblIMISCode.Text = channel.IMISCode;
                lblTotalRDs.Text = Calculations.GetRDText(channel.TotalRDs);
                hdnChannelTotalRDs.Value = Convert.ToString(channel.TotalRDs);

                IN_OutletBLL outLetBLL = new IN_OutletBLL();
                lblChannelType.Text = outLetBLL.GetChannelTypeByID(channel.ChannelTypeID);
                lblFlowType.Text = outLetBLL.GetFlowType(channel.FlowTypeID);
                lblCommandName.Text = outLetBLL.GetCommandNameByID(channel.ComndTypeID);
                hdnChannelTotalRDs.Value = Convert.ToString(channel.TotalRDs);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindChannelReachGrid(long _ChannelID)
        {
            try
            {
                List<object> lstChannelReach = new ReachBLL().GetChannelReach(_ChannelID);
                gvChannelReach.DataSource = lstChannelReach;
                gvChannelReach.DataBind();

                //this.ChannelReach = lstChannelReach;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddChannelReach")
                {
                    List<object> lstChannelReach = new ReachBLL().GetChannelReach(Convert.ToInt64(hdnChannelID.Value));
                    lstChannelReach.Add(new
                    {
                        ID = 0,
                        StartingRDTotal = string.Empty,
                        EndingRDTotal = string.Empty,
                        Remarks = string.Empty,
                        StartingRD = string.Empty,
                        EndingRD = string.Empty,
                        IsActive = true
                    });


                    gvChannelReach.PageIndex = gvChannelReach.PageCount;
                    gvChannelReach.DataSource = lstChannelReach;
                    gvChannelReach.DataBind();

                    gvChannelReach.EditIndex = gvChannelReach.Rows.Count - 1;
                    gvChannelReach.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvChannelReach.EditIndex = -1;
                BindChannelReachGrid(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvChannelReach.EditIndex = e.NewEditIndex;
                BindChannelReachGrid(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string StartingRDLeft = string.Empty;
            string StartingRDRight = string.Empty;
            string EndingRDLeft = string.Empty;
            string EndingRDRight = string.Empty;

            try
            {
                //int LastRow = gvChannelReach.Rows.Count - 1;
                //string Balance = gvChannelReach.Rows[LastRow].Cells[1].Text;


                if (e.Row.RowType == DataControlRowType.DataRow && gvChannelReach.EditIndex == e.Row.RowIndex)
                {
                    #region "Data Keys"
                    DataKey key = gvChannelReach.DataKeys[e.Row.RowIndex];
                    string ID = Convert.ToString(key.Values[0]);
                    string StartingRDTotal = Convert.ToString(key.Values[1]);
                    string EndingRDTotal = Convert.ToString(key.Values[2]);
                    string Remarks = Convert.ToString(key.Values[3]);
                    #endregion

                    #region "Controls"
                    TextBox txtStartingRDLeft = (TextBox)e.Row.FindControl("txtStartingRDLeft");
                    TextBox txtStartingRDRight = (TextBox)e.Row.FindControl("txtStartingRDRight");

                    TextBox txtEndingRDLeft = (TextBox)e.Row.FindControl("txtEndingRDLeft");
                    TextBox txtEndingRDRight = (TextBox)e.Row.FindControl("txtEndingRDRight");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                    Label lblEditStartingRD = (Label)e.Row.FindControl("lblEditStartingRD");
                    Panel pnlStartingRD = (Panel)e.Row.FindControl("pnlStartingRD");
                    #endregion

                    if (e.Row.RowIndex != 0)
                    {
                        lblEditStartingRD.CssClass = "block";
                        pnlStartingRD.CssClass = "hidden";
                        GridViewRow row = gvChannelReach.Rows[gvChannelReach.Rows.Count - 1];
                        string[] lastEndingRD = (((Label)row.FindControl("lblEndingRD")).Text).Split('+');
                        StartingRDTotal = Calculations.CalculateTotalRDs(lastEndingRD[0], lastEndingRD[1]).ToString();
                    }
                    else
                    {
                        lblEditStartingRD.CssClass = "hidden";
                        pnlStartingRD.CssClass = "block";
                    }

                    if (!string.IsNullOrEmpty(StartingRDTotal) && lblEditStartingRD != null)
                        lblEditStartingRD.Text = Calculations.GetRDText(Convert.ToInt64(StartingRDTotal));


                    if (!string.IsNullOrEmpty(StartingRDTotal))
                    {
                        Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(StartingRDTotal));
                        StartingRDLeft = tupleRD.Item1;
                        StartingRDRight = tupleRD.Item2;
                    }
                    if (txtStartingRDLeft != null)
                        txtStartingRDLeft.Text = StartingRDLeft;
                    if (txtStartingRDRight != null)
                        txtStartingRDRight.Text = StartingRDRight;


                    if (!string.IsNullOrEmpty(EndingRDTotal))
                    {
                        Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(EndingRDTotal));
                        EndingRDLeft = tupleRD.Item1;
                        EndingRDRight = tupleRD.Item2;
                    }
                    if (txtEndingRDLeft != null)
                        txtEndingRDLeft.Text = EndingRDLeft;
                    if (txtEndingRDRight != null)
                        txtEndingRDRight.Text = EndingRDRight;

                    if (txtRemarks != null)
                        txtRemarks.Text = Remarks;

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string reachID = Convert.ToString(gvChannelReach.DataKeys[e.RowIndex].Values[0]);

                //if (new ReachBLL().IsChannelReachChildExists(Convert.ToInt64(reachID)))
                //{
                //    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                //    return;
                //}

              bool isAdjusted = new ReachBLL().AdjustReach(Convert.ToInt64(reachID), Convert.ToInt64(hdnChannelID.Value));

               // bool isDeleted = new ReachBLL().DeleteChannelReach(Convert.ToInt64(reachID));
              if (isAdjusted)
                {
                    BindChannelReachGrid(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReach_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                #region "Controls"
                GridViewRow row = gvChannelReach.Rows[e.RowIndex];
                TextBox txtStartingRDLeft = (TextBox)row.FindControl("txtStartingRDLeft");
                TextBox txtStartingRDRight = (TextBox)row.FindControl("txtStartingRDRight");
                TextBox txtEndingRDLeft = (TextBox)row.FindControl("txtEndingRDLeft");
                TextBox txtEndingRDRight = (TextBox)row.FindControl("txtEndingRDRight");
                TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
                #endregion

                string ReachID = Convert.ToString(gvChannelReach.DataKeys[e.RowIndex].Values[0]);
                string EndingRDTotal = Convert.ToString(gvChannelReach.DataKeys[e.RowIndex].Values[2]);


                CO_ChannelReach channelReach = new CO_ChannelReach();
                channelReach.ID = Convert.ToInt64(ReachID);
                channelReach.ChannelID = Convert.ToInt64(hdnChannelID.Value);
                if (txtStartingRDLeft != null && txtStartingRDRight != null)
                    channelReach.FromRD = Calculations.CalculateTotalRDs(txtStartingRDLeft.Text, txtStartingRDRight.Text);

                if (txtEndingRDLeft != null && txtEndingRDRight != null)
                    channelReach.ToRD = Calculations.CalculateTotalRDs(txtEndingRDLeft.Text, txtEndingRDRight.Text);

                if (txtRemarks != null)
                    channelReach.Remarks = txtRemarks.Text;

                channelReach.UpdateDate = DateTime.Now.Date;
                channelReach.IsActive = true;

                Tuple<int, int> tupleRDs = new ChannelBLL().GetIrrigationBoundariesRDs(Convert.ToInt64(hdnChannelID.Value));

                if (!(tupleRDs.Item1 <= channelReach.FromRD && tupleRDs.Item2 >= channelReach.FromRD))
                {
                    Master.ShowMessage("Starting R.D is out of range of the starting and ending R.Ds of the channel", SiteMaster.MessageType.Error);
                    return;
                }
                if (!(tupleRDs.Item1 <= channelReach.ToRD && tupleRDs.Item2 >= channelReach.ToRD))
                {
                    Master.ShowMessage("Ending R.D is out of range of the starting and ending R.Ds of the channel", SiteMaster.MessageType.Error);
                    return;
                }
                if (channelReach.FromRD >= channelReach.ToRD)
                {
                    Master.ShowMessage("Reach Starting R.D must be smaller than Reach Ending R.D", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSaved = false;
                // 
                if (channelReach.ID > 0 && channelReach.ToRD != Convert.ToDouble(EndingRDTotal)) // No change in Ending RD
                    IsSaved = new ReachBLL().EditChannelReach(channelReach);
                else
                    IsSaved = new ReachBLL().SaveChannelReach(channelReach);

                if (IsSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ReachID) == 0)
                        gvChannelReach.PageIndex = 0;

                    gvChannelReach.EditIndex = -1;
                    BindChannelReachGrid(channelReach.ChannelID.Value);
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public string GetPageURL(PageNames _PageName, string _ReachID, string _ReachRD, string _ReachNo, string _RDLocation)
        {
            string URL = "~/Modules/IrrigationNetwork/Reach/";
            try
            {
                switch (_PageName)
                {
                    case PageNames.LSectionParameters:
                        URL = URL + "LSectionParameters.aspx?ChannelID=" + hdnChannelID.Value + "&ReachID=" + _ReachID + "&ReachRD=" + _ReachRD + "&ReachNo=" + _ReachNo + "&L=" + _RDLocation;
                        break;
                    case PageNames.LSectionHistory:
                        URL = URL + "LSectionParametersHistory.aspx?ChannelID=" + hdnChannelID.Value + "&ReachID=" + _ReachID + "&ReachRD=" + _ReachRD + "&ReachNo=" + _ReachNo + "&L=" + _RDLocation;
                        break;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return URL;
        }
        protected void gvChannelReach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannelReach.PageIndex = e.NewPageIndex;
                gvChannelReach.EditIndex = -1;
                BindChannelReachGrid(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}