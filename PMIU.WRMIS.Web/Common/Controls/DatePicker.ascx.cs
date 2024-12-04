using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Common.Controls
{
    public partial class DatePicker : System.Web.UI.UserControl
    {
        protected string _Hour;
        protected string _Minute;
        protected string _AMPM;
        public string Hour {
            get { return _Hour; }
            set { _Hour = value; }
        }

        public string Minute
        {
            get { return _Minute; }
            set { _Minute = value; }
        }

        public string AMPM
        {
            get { return _AMPM; }
            set { _AMPM = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetTime(string _pHour, string _pMinute, string _pAMPM)
        {
            _Hour = _pHour;
            _Minute = _pMinute;
            _AMPM = _pAMPM;

            SetTime();
        }

        public void SetTime()
        {
            ListItem iHour = ddlHour.Items.FindByValue(_Hour);
            if (iHour != null)    
                iHour.Selected = true;

            ListItem iMinute = ddlMinute.Items.FindByValue(_Minute);
            if (iMinute != null)
                iMinute.Selected = true;

            ListItem iAMPM = ddlAMPM.Items.FindByValue(_AMPM);
            if (iAMPM != null)
                iAMPM.Selected = true;

        }

        public string GetTime()
        {
            string sHour = ddlHour.SelectedValue;
            string sMinute = ddlMinute.SelectedValue;
            string sAMPM = ddlAMPM.SelectedValue;

            return sHour + ":" + sMinute + " " + sAMPM;
        }
    }
}