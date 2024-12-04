using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.Notifications;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class CrestLevelDTParameters : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelGaugeID"]))
                    {
                        long ChannelGaugeID = Convert.ToInt64(Request.QueryString["ChannelGaugeID"]);
                        BindChannelData(ChannelGaugeID);
                    }

                    txtReadingDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    txtObservationdate.Text = Utility.GetFormattedDate(DateTime.Now);

                    txtFallBreadth.Focus();

                    btnSave.Enabled = true;

                    btnSave.Visible = base.CanAdd;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 18-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.CrestLevelDTParameters);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function sets the data related to Channel in the top grid.
        /// Created on 18-11-2015
        /// </summary>
        /// <param name="_ChannelGaugeID"></param>
        private void BindChannelData(long _ChannelGaugeID)
        {
            ChannelBLL bllChannel = new ChannelBLL();
            CO_ChannelGauge mdlChannelGauge = bllChannel.GetChannelGaugeByID(_ChannelGaugeID);

            lblChannelName.Text = mdlChannelGauge.CO_Channel.NAME;
            lblChannelType.Text = mdlChannelGauge.CO_Channel.CO_ChannelType.Name;
            lblTotalRDs.Text = Calculations.GetRDText(mdlChannelGauge.CO_Channel.TotalRDs);
            lblFlowType.Text = mdlChannelGauge.CO_Channel.CO_ChannelFlowType.Name;
            lblCommandName.Text = mdlChannelGauge.CO_Channel.CO_ChannelComndType.Name;
            lblCategoryOfGauge.Text = mdlChannelGauge.CO_GaugeCategory.Name;
            lblGaugeID.Text = Convert.ToString(_ChannelGaugeID);
            lblChannelID.Text = Convert.ToString(mdlChannelGauge.ChannelID);
        }

        /// <summary>
        /// This function validates the date and returns model object with validated date.
        /// Created on 18-11-2015
        /// </summary>
        /// <returns>CO_ChannelGaugeDTPFall</returns>
        private CO_ChannelGaugeDTPFall GetValidatedDate()
        {
            CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = new CO_ChannelGaugeDTPFall();

            DateTime ReadingDate = Utility.GetParsedDate(txtReadingDate.Text.Trim());
            DateTime ObservationDate = Utility.GetParsedDate(txtObservationdate.Text.Trim());

            ReadingDate = ReadingDate.Add(DateTime.Now.TimeOfDay);

            mdlChannelGaugeDTFall.ReadingDate = ReadingDate;
            mdlChannelGaugeDTFall.ObservationDate = ObservationDate;

            if (ObservationDate > ReadingDate)
            {
                Master.ShowMessage(Message.ObservationDateCannotBeGreater.Description, SiteMaster.MessageType.Error);

                return null;
            }
            if (mdlChannelGaugeDTFall.ReadingDate < DateTime.Now.Date)
            {
                Master.ShowMessage(Message.OldDatesNotAllowed.Description, SiteMaster.MessageType.Error);

                return null;
            }

            return mdlChannelGaugeDTFall;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = GetValidatedDate();

                if (mdlChannelGaugeDTFall == null)
                {
                    return;
                }

                mdlChannelGaugeDTFall.BreadthFall = Convert.ToDouble(txtFallBreadth.Text.Trim());
                mdlChannelGaugeDTFall.HeadAboveCrest = Convert.ToDouble(txtHeadAboveCrest.Text.Trim());
                mdlChannelGaugeDTFall.DischargeObserved = Convert.ToDouble(txtObservedDischarge.Text.Trim());
                mdlChannelGaugeDTFall.GaugeID = Convert.ToInt32(lblGaugeID.Text);
                mdlChannelGaugeDTFall.DischargeCoefficient = Calculations.GetCrestCoefficientOfDischarge((double)mdlChannelGaugeDTFall.BreadthFall, mdlChannelGaugeDTFall.HeadAboveCrest,
                    mdlChannelGaugeDTFall.DischargeObserved);

                txtDischargeCoefficient.Text = Convert.ToString(mdlChannelGaugeDTFall.DischargeCoefficient);

                ChannelBLL bllChannel = new ChannelBLL();

                bool IsRecordSaved = bllChannel.AddCrestLevelDTParameters(mdlChannelGaugeDTFall);

                if (IsRecordSaved)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("GaugeID", mdlChannelGaugeDTFall.GaugeID);
                    _event.Parameters.Add("IsCrestParameters", "true");
                    _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditCrestLevelParameters, SessionManagerFacade.UserInformation.ID);

                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    btnSave.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                btnSave.Enabled = true;

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                string Url = "~/Modules/IrrigationNetwork/Channel/GaugeInformation.aspx?ChannelID=" + lblChannelID.Text;
                Response.Redirect(Url);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}