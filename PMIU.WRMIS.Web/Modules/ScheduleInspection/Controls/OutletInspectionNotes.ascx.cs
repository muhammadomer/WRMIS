using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls
{
    public partial class OutletInspectionNotes : System.Web.UI.UserControl
    {
        private static bool _IsVisible = false;
        public static bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
            }
        }
        public string ScheduleTitle
        {
            get
            {
                return lblScheduleTitle.Text;
            }
            set
            {
                lblScheduleTitle.Text = value;
            }
        }
        public string ScheduleStatus
        {
            get
            {
                return lblStatus.Text;
            }
            set
            {
                lblStatus.Text = value;
            }
        }
        public string PreparedBy
        {
            get
            {
                return lblPreparedBy.Text;
            }
            set
            {
                lblPreparedBy.Text = value;
            }
        }
        public string FromDate
        {
            get
            {
                return lblFromDate.Text;
            }
            set
            {
                lblFromDate.Text = value;
            }
        }
        public string ToDate
        {
            get
            {
                return lblToDate.Text;
            }
            set
            {
                lblToDate.Text = value;
            }
        }
        public string ChannelName
        {
            get
            {
                return lblChannelName.Text;
            }
            set
            {
                lblChannelName.Text = value;
            }
        }
        public string OutletName
        {
            get
            {
                return lblOutletName.Text;
            }
            set
            {
                lblOutletName.Text = value;
            }
        }
        public string OutletTypeName
        {
            get
            {
                return lblOutletType.Text;
            }
            set
            {
                lblOutletType.Text = value;
            }
        }
        public string InpectedBy
        {
            get
            {
                return lblInspectedBy.Text;
            }
            set
            {
                lblInspectedBy.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (_IsVisible)
                    {
                        InspectedByLabel.Visible = true;
                        InspectedByValue.Visible = true;
                        _IsVisible = false; //Reset Flag
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetOutletInspection()
        {
            try
            {
                dynamic bllOutletInspection = new ScheduleInspectionBLL().GetOutletInspection(Convert.ToInt64(hdnScheduleID.Value),null);

                lblScheduleTitle.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleTitle");
                lblStatus.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleStatus");
                lblPreparedBy.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "PreparedBy");
                lblFromDate.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "FromDate");
                lblToDate.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "ToDate");
                lblChannelName.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelName");
                lblOutletName.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletRDChannelSide");
                lblOutletType.Text = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeName");
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}