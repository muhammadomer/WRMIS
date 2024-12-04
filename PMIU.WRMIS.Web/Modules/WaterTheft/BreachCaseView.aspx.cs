using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.WaterTheft;
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

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class BreachCaseView : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long breachCaseId = 0;
            try
            {

                if (!IsPostBack)
                {
                    SetTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindDropDown(mdlUser);

                    if (!string.IsNullOrEmpty(Request.QueryString["BreachCaseID"]))
                    {
                        breachCaseId = Convert.ToInt64(Request.QueryString["BreachCaseID"]);
                        BreachCaseId.Value = Convert.ToString(breachCaseId);
                        LoadBreachCaseData(breachCaseId);
                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }

        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.BreachCaseView);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropDown(UA_Users mdlUser)
        {
            // long  UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            long UserId = mdlUser.ID;
            long IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;
            Dropdownlist.BindDropdownlist<List<object>>(ddlChannel, new WaterTheftBLL().GetChannelByUserId(UserId, IrrigationLevelID));
            Dropdownlist.BindDropdownlist<List<object>>(ddlSide, CommonLists.GetWTChannelSides());
        }
        private void LoadBreachCaseData(long _BreachCaseId)
        {
            WT_Breach ObjBreach = new WT_Breach();
            string LeftRD = string.Empty;
            string RightRD = string.Empty;
            ObjBreach = new WaterTheftBLL().GetBreachCaseDatebyID(_BreachCaseId);
            Dropdownlist.SetSelectedValue(ddlChannel, ObjBreach.ChannelID.ToString());

            string RDs = ObjBreach.BreachSiteRD.ToString();
            if (!string.IsNullOrEmpty(RDs))
            {
                Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(RDs));
                LeftRD = tupleRD.Item1;
                RightRD = tupleRD.Item2;
            }

            txtOutletRDLeft.Text = LeftRD;
            txtOutletRDRight.Text = RightRD;
            Dropdownlist.SetSelectedValue(ddlSide, ObjBreach.BreachSide.ToString());
            DateTime datetime = Convert.ToDateTime(ObjBreach.DateTime);
            string BreachDate = datetime.ToString("yyyy-MM-dd");
            string IncidentTime = datetime.ToString("HH:mm:ss tt");
            txtIncidentDate.Text = BreachDate;
            txtIncidentTime.Text = IncidentTime;
            txtHeadDischarge.Text = ObjBreach.HeadDischarge.ToString();
            txtLengthofbreach.Text = ObjBreach.BreachLength.ToString();
            txtRemarks.Text = ObjBreach.Remarks.ToString();

        }



    }
}