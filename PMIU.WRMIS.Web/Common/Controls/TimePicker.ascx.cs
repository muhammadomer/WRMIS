using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Common.Controls
{
    public partial class TimePicker : System.Web.UI.UserControl
    {
        protected string _Hour;
        protected string _Minute;
        protected string _AMPM;
        protected string _Width;
        protected bool _CrossIcon;
        protected bool _ClockIcon;
        public string Hour
        {
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

        public string Width 
        {
            get { return _Width; }
            set { _Width = value; } 
        }

        public bool CrossIcon 
        {
            get { return _CrossIcon; }
            set { _CrossIcon = value; } 
        }

        public bool ClockIcon 
        {
            get { return _ClockIcon; }
            set { _ClockIcon = value; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (_Hour == null || _Minute == null || _AMPM == null)
                {
                    int sHours = System.DateTime.Now.Hour;
                    string sHour = (((System.DateTime.Now.Hour + 11) % 12) + 1).ToString("00");
                    string sMinute = System.DateTime.Now.Minute.ToString("00");
                    string sAMPM = "";
                    if (sHours > 11 && sHours < 25)
                        sAMPM = "PM";
                    else
                        sAMPM = "AM";
                   

                    SetTime(sHour, sMinute, sAMPM);
                }
            }

            tpDiv.Style.Add(HtmlTextWriterStyle.Width, _Width);

            if (_ClockIcon == null)
                _ClockIcon = true;
            if (_CrossIcon == null)
                _CrossIcon = true;

            dvCrossIcon.Visible = CrossIcon;
            dvClockIcon.Visible = ClockIcon;
        }

        public void SetTime(string _pHour, string _pMinute, string _pAMPM)
        {
            _Hour = _pHour;
            _Minute = _pMinute;
            _AMPM = _pAMPM;

            //SetTime();


            string Time = _Hour + ":" + _Minute + " " + _AMPM;
            txtTimePicker.Text = Time;
            


        }


        public void SetTime(string _pTime)
        {
            DateTime TimeOnly = Convert.ToDateTime(_pTime);

            string Time = TimeOnly.ToString("hh:mm tt");

            txtTimePicker.Text = Time;
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

        public void DisbaleTimePicker()
        {
            //ddlHour.Enabled = false;
            //ddlMinute.Enabled = false;
            //ddlAMPM.Enabled = false;

            txtTimePicker.Enabled = false;
            dvCrossIcon.Visible = false;
            txtTimePicker.CssClass = "aspNetDisabled form-control";
        }
        public void EnableTimePicker()
        {
            //ddlHour.Enabled = true;
            //ddlMinute.Enabled = true;
            //ddlAMPM.Enabled = true;

            dvCrossIcon.Visible = true;
            txtTimePicker.Enabled = true;
        }
        public void RemoveTimePickerValidations()
        {
            txtTimePicker.CssClass = "form-control timepicker-default";
            txtTimePicker.Attributes.Remove("required");
        }
        public string GetTime()
        {
            //string sHour = ddlHour.SelectedValue;
            //string sMinute = ddlMinute.SelectedValue;
            //string sAMPM = ddlAMPM.SelectedValue;

            //return sHour + ":" + sMinute + " " + sAMPM;


            return txtTimePicker.Text;
        }
    }
}