using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class ChannelAddition : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindChannelDropdownlists();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        channelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        hdnChannelID.Value = Convert.ToString(channelID);
                        LoadChannelDetail(channelID);
                    }
                    txtRemarks.Attributes.Add("maxlength", "300");
                }
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
                CO_Channel bllChannel = new ChannelBLL().GetChannelByID(_ChannelID);
                if (bllChannel != null)
                {
                    Tuple<string, string> tuple = Calculations.GetRDValues(bllChannel.TotalRDs);
                    txtChannelName.Text = bllChannel.NAME;
                    txtTotalRDLeft.Text = tuple.Item1;
                    txtTotalRDRight.Text = tuple.Item2;
                    Dropdownlist.SetSelectedValue(ddlChannelType, Convert.ToString(bllChannel.ChannelTypeID));
                    Dropdownlist.SetSelectedValue(ddlChannelFlowType, Convert.ToString(bllChannel.FlowTypeID));
                    Dropdownlist.SetSelectedValue(ddlChannelCommandType, Convert.ToString(bllChannel.ComndTypeID));
                    txtAuthorizedTailGauge.Text = Convert.ToString(bllChannel.AuthorizedTailGauge);
                    txtGrossCommandArea.Text = Convert.ToString(bllChannel.GCAAcre);
                    txtCultureableCommandArea.Text = Convert.ToString(bllChannel.CCAAcre);
                    txtChannelNameAbbreviation.Text = bllChannel.ChannelAbbreviation;

                    hdnCurrentTotalRD.Value = Convert.ToString(bllChannel.TotalRDs);
                    hdnIsGaugeCalculated.Value = Convert.ToString(bllChannel.IsCalculated);
                    txtRemarks.Text = bllChannel.Remarks;
                    txtIMISCode.Text = bllChannel.IMISCode;

                    txtIMISCode.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelAddition);

            Master.ModuleTitle = pageTitle.Item1;
            if (string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
            {
                Master.NavigationBar = pageTitle.Item3;
                Master.PageTitle = pageTitle.Item2;
                pageTitleID.InnerText = pageTitle.Item2;
            }
            else
            {
                Master.NavigationBar = "Edit Channel";
                Master.PageTitle = "Edit Channel";
                pageTitleID.InnerText = "Edit Channel";
            }
        }
        private void BindChannelDropdownlists()
        {
            try
            {
                // Bind Channel type dropdownlist
                Dropdownlist.DDLChannelTypes(ddlChannelType, (int)Constants.DropDownFirstOption.Select, true);
                // Bind Flow type dropdownlist
                Dropdownlist.DDLFlowTypes(ddlChannelFlowType, (int)Constants.DropDownFirstOption.Select, true);
                // Bind Common name dropdownlist
                Dropdownlist.DDLCommandNames(ddlChannelCommandType, (int)Constants.DropDownFirstOption.Select, true);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CO_Channel channelEntity = PrepareChannelEntity();
            try
            {
                if (Convert.ToInt64(hdnChannelID.Value) != 0 && Convert.ToInt64(hdnCurrentTotalRD.Value) > 0 && channelEntity.TotalRDs != Convert.ToInt64(hdnCurrentTotalRD.Value))
                {
                    Master.ShowMessage("Channel Total RD can not be changed.", SiteMaster.MessageType.Error);
                }
                else if (new ChannelBLL().IsChannelExists(channelEntity))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                }
                //else if (new ChannelBLL().IsChannelIMISExists(channelEntity))
                //{
                //    Master.ShowMessage(Message.UniqueIMISValueRequired.Description, SiteMaster.MessageType.Error);
                //}
                else
                {
                    bool isSaved = new ChannelBLL().SaveChannel(channelEntity);
                    if (isSaved)
                    {
                        ChannelSearch.IsSaved = true;
                        HttpContext.Current.Session.Add("ChannelName", channelEntity.NAME);
                        Response.Redirect("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID=" + channelEntity.ID, false);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                lblMessage.Text = ex.Message;
            }
        }
        private CO_Channel PrepareChannelEntity()
        {
            CO_Channel channel = new CO_Channel();


            channel.ID = Convert.ToInt64(hdnChannelID.Value);
            channel.NAME = txtChannelName.Text;
            channel.ChannelTypeID = Convert.ToInt64(ddlChannelType.SelectedValue);
            channel.TotalRDs = Calculations.CalculateTotalRDs(txtTotalRDLeft.Text, txtTotalRDRight.Text);
            channel.FlowTypeID = Convert.ToInt64(ddlChannelFlowType.SelectedValue);
            channel.ComndTypeID = Convert.ToInt64(ddlChannelCommandType.SelectedValue);
            channel.AuthorizedTailGauge = Convert.ToDouble(txtAuthorizedTailGauge.Text);
            channel.IMISCode = txtIMISCode.Text;
            if (!string.IsNullOrEmpty(txtGrossCommandArea.Text))
                channel.GCAAcre = Cast.ToDouble(txtGrossCommandArea.Text);
            if (!string.IsNullOrEmpty(txtCultureableCommandArea.Text))
                channel.CCAAcre = Cast.ToDouble(txtCultureableCommandArea.Text);
            channel.ChannelAbbreviation = txtChannelNameAbbreviation.Text;
            channel.IsCalculated = Convert.ToBoolean(hdnIsGaugeCalculated.Value);// Default is false(0)
            channel.Remarks = txtRemarks.Text.Trim();
            channel.IMISCode = txtIMISCode.Text;

            return channel;
        }


    }
}