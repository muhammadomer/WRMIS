using System;
using System.Collections.Generic;
using System.Linq;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class IMISCode : BasePage
    {
        public static long _ChannelTypeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        channelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        ChannelDetails.ChannelID = channelID;
                        hdnChannelID.Value = Convert.ToString(channelID);
                        BndDropdown();
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}", channelID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BndDropdown()
        {
            Dropdownlist.DDLGetChannelParentFeedersNameByChannelID(ddlParentFeeder, Convert.ToInt64(hdnChannelID.Value));
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.IMISCode);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string _UpdateIMISCode = string.Empty;
                _UpdateIMISCode = Convert.ToString(lblIMISFirstThree.Text + txtIMISCode.Text + txtEscapeChannel.Text + lblIMISLast.Text);
                if (_ChannelTypeID == (int)Constants.ChannelType.DistributarySubMinor || _ChannelTypeID == (int)Constants.ChannelType.EscapeChannel)
                {
                    _UpdateIMISCode = Convert.ToString(lblIMISFirstThree.Text + txtEscapeChannel.Text);
                }
                if (new ChannelBLL().IsIMISCodeExists(Convert.ToInt64(hdnChannelID.Value), _UpdateIMISCode))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsRecordUpdated = new ChannelBLL().UpdateIMISCodeChannelID(Convert.ToInt64(hdnChannelID.Value), _UpdateIMISCode);
                if (IsRecordUpdated)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindGrid()
        {
            try
            {
                List<object> lstParentFeederChannel = new ChannelBLL().BindIMISCode(Convert.ToInt64(ViewState["PFId"]));
                gvIMIS.DataSource = lstParentFeederChannel;
                gvIMIS.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlParentFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlParentFeeder.SelectedItem.Value != "")
                {
                    ViewState["PFId"] = Convert.ToString(ddlParentFeeder.SelectedItem.Value);
                    List<object> lstParentFeederChannel = new ChannelBLL().BindIMISCode(Convert.ToInt64(ddlParentFeeder.SelectedItem.Value));
                    gvIMIS.DataSource = lstParentFeederChannel;
                    gvIMIS.DataBind();
                    gvIMIS.Visible = true;
                    lblIMISFirstThree.Visible = true;
                    lblIMISLast.Visible = true;
                    lblIMISCode.Visible = true;
                    txtIMISCode.Visible = true;

                    string IMIS = Convert.ToString(new ChannelBLL().GetStructureTypeID(Convert.ToInt64(ddlParentFeeder.SelectedItem.Value)));
                    if (_ChannelTypeID == (int)Constants.ChannelType.MainCanal || _ChannelTypeID == (int)Constants.ChannelType.LinkCanal)
                    {
                        txtEscapeChannel.Visible = false;
                        lblIMISFirstThree.Text = IMIS.Substring(0, 3);
                        txtIMISCode.Text = IMIS.Substring(3, 3);
                        lblIMISLast.Text = IMIS.Substring(6);
                    }
                    else if (_ChannelTypeID == (int)Constants.ChannelType.BranchCanal)
                    {
                        txtEscapeChannel.Visible = false;
                        lblIMISFirstThree.Text = IMIS.Substring(0, 6);
                        txtIMISCode.Text = IMIS.Substring(6, 3);
                        lblIMISLast.Text = IMIS.Substring(9);
                    }
                    else if (_ChannelTypeID == (int)Constants.ChannelType.DistributaryMajor)
                    {
                        txtEscapeChannel.Visible = false;
                        lblIMISFirstThree.Text = IMIS.Substring(0, 9);
                        txtIMISCode.Text = IMIS.Substring(9, 3);
                        lblIMISLast.Text = IMIS.Substring(12);
                    }
                    else if (_ChannelTypeID == (int)Constants.ChannelType.DistributaryMinor)
                    {
                        txtEscapeChannel.Visible = false;
                        lblIMISFirstThree.Text = IMIS.Substring(0, 12);
                        txtIMISCode.Text = IMIS.Substring(12, 3);
                        lblIMISLast.Text = IMIS.Substring(15);
                    }
                    else if (_ChannelTypeID == (int)Constants.ChannelType.DistributarySubMinor || _ChannelTypeID == (int)Constants.ChannelType.EscapeChannel)
                    {
                        lblIMISFirstThree.Text = IMIS.Substring(0, 15);
                        // txtIMISCode.Text = IMIS.Substring(15);
                        txtIMISCode.Visible = false;
                        txtEscapeChannel.Visible = true;
                        txtEscapeChannel.Text = IMIS.Substring(15);
                        lblIMISLast.Visible = false;

                    }
                }
                else
                {
                    gvIMIS.Visible = false;
                    lblIMISFirstThree.Visible = false;
                    lblIMISLast.Visible = false;
                    lblIMISCode.Visible = false;
                    txtIMISCode.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void gvIMIS_PageIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvIMIS.EditIndex = -1;
        //        BindGrid();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //protected void gvIMIS_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        gvIMIS.PageIndex = e.NewPageIndex;
        //        BindGrid();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

    }
}