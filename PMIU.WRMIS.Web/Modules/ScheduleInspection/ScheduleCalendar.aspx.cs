using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ScheduleCalendar : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (_IsSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        _IsSaved = false; // Reset flag after displaying message.
                    }
                    SetPageTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    hdnUserID.Value = Convert.ToString(SessionManagerFacade.UserInformation.ID);
                    hdnMonth.Value = Convert.ToString(DateTime.Now.Month);
                    hdnYear.Value = Convert.ToString(DateTime.Now.Year);
                    //if (!string.IsNullOrEmpty(Request.QueryString["OutletCheckingDate"]))
                    //{
                    //    DateTime OutletCheckingDate = Convert.ToDateTime(Request.QueryString["OutletCheckingDate"]);
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "LoadScheduleInspections("+mdlUser.ID+","+OutletCheckingDate+");", true);

                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ScheduleCalendar);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetUserInspectionDates(string _UserID, long _Month, long _Year)
        {
            return new ScheduleInspectionBLL().GetUserInspectionDates(Convert.ToInt64(_UserID), _Month, _Year);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetUserInspectionsByDate(string _UserID, DateTime _Date)
        {
            List<dynamic> lstUserInspection = new ScheduleInspectionBLL().GetUserInspectionsByDate(Convert.ToInt64(_UserID), _Date);
            List<dynamic> lstTenderInspection = new ScheduleInspectionBLL().GetTenderInspectionsByDate(Convert.ToInt64(_UserID), _Date);
            List<dynamic> lstClosureInspection = new ScheduleInspectionBLL().GetClosureInspectionsByDate(Convert.ToInt64(_UserID), _Date);
            List<dynamic> lstgeneralInspection = new ScheduleInspectionBLL().GetGeneralInspectionsByDate(Convert.ToInt64(_UserID), _Date);
            lstUserInspection.AddRange(lstTenderInspection);
            lstUserInspection.AddRange(lstClosureInspection);
            lstUserInspection.AddRange(lstgeneralInspection);
           
            Tuple<string, string> status = null;
            Tuple<string, bool, string> URL = null;
            string disabled = string.Empty;
            string href = string.Empty;
            string InspectionArea = string.Empty;
            StringBuilder inspection = new StringBuilder();
            foreach (dynamic item in lstUserInspection)
            {
                if (IsPropertyExist(item, "IsTenderSold"))
                {
                    disabled = string.Empty;
                    href = string.Empty;
                    InspectionArea = string.Empty;
                    string WorkTypeName = "";
                    bool IsSold = Convert.ToBoolean(Utility.GetDynamicPropertyValue(item, "IsTenderSold"));
                    WorkTypeName = Utility.GetDynamicPropertyValue(item, "WorkTypeName");
                    long ScheduleStatusID = Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID"));

                    string IconT = "btn btn-primary btn_24 edit";
                    bool IsExists = new ScheduleInspectionBLL().IsADMRecordExists(Convert.ToInt64(Utility.GetDynamicPropertyValue(item,"TenderWorkID")));
                    //if (IsExists)
                    //{
                    //    IconT = "btn btn-success btn_24 view green";
                    //}
                    //else
                    //{
                    //     IconT = "btn btn-primary btn_24 edit";
                    //}
                    string InspectionType = "Tender Monitoring";
                    string URLTender = "";

                    status = GetInspectionLegendValue(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID")));
                    if (!IsSold)
                    {
                        disabled = "disabled ";
                        href = "href=" + URLTender;
                    }
                    else if (IsSold)
                    {
                        if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM)
                        {
                            URLTender = string.Format("/../../Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "TenderWorkID")) + "&WorkSourceID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "WorkSourceID")));
                        }
                        else if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                        {
                            URLTender = string.Format("/../../Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "TenderWorkID")) + "&WorkSourceID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "WorkSourceID")));
                        }

                        href = "href=" + URLTender;
                    }

                    if (ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared || ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval || ScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                    {
                        disabled = "disabled ";
                    } 

                   

                    inspection.Append("<tr>");
                    inspection.Append("<td>" + InspectionType + "<br><b>" + Utility.GetDynamicPropertyValue(item, "TenderWorkName") + "</b><br><span class='label label-small " + status.Item1 + "'>" + status.Item2 + "</span>&nbsp;&nbsp;&nbsp;&nbsp;" + WorkTypeName + "</td>");
                    inspection.Append("<td><a  id='hlInspections-" + Utility.GetDynamicPropertyValue(item, "ScheduleID") + "' title='Inspection' class='" + disabled + "" + IconT + "'" + href + "></a></td>");
                    inspection.Append("</tr>"); 
                }
                else if (IsPropertyExist(item, "CWPID"))
                {
                    disabled = string.Empty;
                    href = string.Empty;
                    InspectionArea = string.Empty;
                    string Type = string.Empty;
                    string IconT = string.Empty;
                    //string Edit = "";
                    string EditMode = Utility.GetDynamicPropertyValue(item, "RefMonitoringID");
                    if (string.IsNullOrEmpty(EditMode))
                    {
                        IconT = "btn btn-primary btn_24 edit";
                        
                    }
                    else
                    {
                        IconT = "btn btn-success btn_24 view green";
                        
                    }
                    Type = Utility.GetDynamicPropertyValue(item, "WorkType");
                    
                    string InspectionType = "Works Inspection";
                    string URLClosure = "";
                    long CStatusID = Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID"));
                    status = GetInspectionLegendValue(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID")));
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                    {
                        if (string.IsNullOrEmpty(EditMode))
                        {
                            URLClosure = string.Format("/../../Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ClosureWorkID")) + "&CWPID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "CWPID")) + "&ScheduleDetailID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleDetailID")));

                        }
                        else if (!string.IsNullOrEmpty(EditMode))
                        {
                            URLClosure = string.Format("/../../Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ClosureWorkID")) + "&CWPID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "CWPID")) + "&RefMonitoringID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "RefMonitoringID")));
                        }
                    }
                    href = "href=" + URLClosure;
                    if (CStatusID == (long)Constants.SIScheduleStatus.Prepared || CStatusID == (long)Constants.SIScheduleStatus.PendingForApproval || CStatusID == (long)Constants.SIScheduleStatus.Rejected)
                    {
                        disabled = "disabled ";
                    }



                    inspection.Append("<tr>");
                    inspection.Append("<td>" + InspectionType + "<br><b>" + Utility.GetDynamicPropertyValue(item, "CLosureWorkName") + "</b><br><span class='label label-small " + status.Item1 + "'>" + status.Item2 + "</span>&nbsp;&nbsp;&nbsp;&nbsp;" + Type + "</td>");
                    inspection.Append("<td><a  id='hlInspections-" + Utility.GetDynamicPropertyValue(item, "ScheduleID") + "' title='Inspection' class='" + disabled + "" + IconT + "'" + href + "></a></td>");
                    inspection.Append("</tr>"); 
                }
                else if (IsPropertyExist(item, "GIViewMode"))
                {
                   disabled = string.Empty;
                    href = string.Empty;
                    InspectionArea = string.Empty;
                    string Location = string.Empty;
                    string IconT = string.Empty;
                    //string Edit = "";
                    bool ViewMode = Convert.ToBoolean(Utility.GetDynamicPropertyValue(item, "GIViewMode"));
                    if (!ViewMode)
                    {
                        IconT = "btn btn-primary btn_24 edit";
                       
                    }
                    else
                    {
                        IconT = "btn btn-success btn_24 view green";
                        
                    }
                    Location = Utility.GetDynamicPropertyValue(item, "Location");
                    
                    string InspectionType = "General Inspection";
                    string URLGeneral = "";
                    long GStatusID = Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID"));
                    status = GetInspectionLegendValue(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID")));
                    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                    {
                     URLGeneral = string.Format("/../../Modules/ScheduleInspection/AddGeneralInspections.aspx?ScheduleDetailID=" + Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleDetailID")) + "&IsViewMode=" + ViewMode + "&Source=C");
                    }
                    href = "href=" + URLGeneral;
                    if (GStatusID == (long)Constants.SIScheduleStatus.Prepared || GStatusID == (long)Constants.SIScheduleStatus.PendingForApproval || GStatusID == (long)Constants.SIScheduleStatus.Rejected)
                    {
                        disabled = "disabled ";
                    }



                    inspection.Append("<tr>");
                    inspection.Append("<td>" + InspectionType + "<br><b>" + Location + "</b><br><span class='label label-small " + status.Item1 + "'>" + status.Item2 + "</span>&nbsp;&nbsp;&nbsp;&nbsp;" + Utility.GetDynamicPropertyValue(item, "InspectionType") + "</td>");
                    inspection.Append("<td><a  id='hlInspections-" + Utility.GetDynamicPropertyValue(item, "ScheduleID") + "' title='Inspection' class='" + disabled + "" + IconT + "'" + href + "></a></td>");
                    inspection.Append("</tr>");  
                }
                else
                {
                    disabled = string.Empty;
                    href = string.Empty;
                    InspectionArea = string.Empty;
                    bool IsExists = false;
                    string Icon = "btn btn-primary btn_24 edit";

                    status = GetInspectionLegendValue(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ScheduleStatusID")));
                    URL = GetInspectionsURL(item, ref IsExists, _Date);
                    if (URL.Item2)
                        disabled = "disabled ";
                    if (!string.IsNullOrEmpty(URL.Item1))
                        href = "href=" + URL.Item1;

                    if (IsExists)
                    {
                        Icon = "btn btn-success btn_24 view green";
                    }

                    inspection.Append("<tr>");
                    inspection.Append("<td>" + Utility.GetDynamicPropertyValue(item, "InspectionType") + "<br><b>" + Utility.GetDynamicPropertyValue(item, "ChannelName") + "</b><br><span class='label label-small " + status.Item1 + "'>" + status.Item2 + "</span>&nbsp;&nbsp;&nbsp;&nbsp;" + URL.Item3 + "</td>");
                    inspection.Append("<td><a  id='hlInspections-" + Utility.GetDynamicPropertyValue(item, "ScheduleID") + "' title='Inspection' class='" + disabled + "" + Icon + "'" + href + "></a></td>");
                    inspection.Append("</tr>");
                }
             
            }
            return inspection.ToString();
        }
        private static Tuple<string, string> GetInspectionLegendValue(long _ScheduleStatusID)
        {
            string value = string.Empty;
            string text = string.Empty;
            switch (_ScheduleStatusID)
            {
                case (long)Constants.SIScheduleStatus.Prepared:
                    value = "label-info";
                    text = Constants.Prepared;
                    break;
                case (long)Constants.SIScheduleStatus.PendingForApproval:
                    value = "label-warning";
                    text = Constants.Pending;
                    break;
                case (long)Constants.SIScheduleStatus.Approved:
                    value = "label-success";
                    text = Constants.Approved;
                    break;
                case (long)Constants.SIScheduleStatus.Rejected:
                    value = "label-warning";
                    text = Constants.Rejected;
                    break;
                default:
                    break;
            }
            return Tuple.Create(value, text);
        }
        private static Tuple<string, bool, string> GetInspectionsURL(dynamic _Inspection, ref bool IsExists, DateTime _CheckingDateTime)
        {
            string url = string.Empty;
            bool IsDisabled = true;
            string InspectionArea = string.Empty;
            long InspectionTypeID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "InspectionTypeID"));

            if ((long)Constants.SIScheduleStatus.Approved == Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "ScheduleStatusID")))
            {
                switch (InspectionTypeID)
                {
                    case (long)Constants.SIInspectionType.GaugeReading:
                        IsDisabled = false;
                        IsExists = new ScheduleInspectionBLL().IsGaugeInspectionAlreadyExists(Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID")));
                        url = "AddGaugeInspection.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID");
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtGauge");
                        break;
                    case (long)Constants.SIInspectionType.OutletAlteration:
                        if (Convert.ToBoolean(Convert.ToInt32(Utility.GetDynamicPropertyValue(_Inspection, "IsOutletAlterationInspectionExists"))))
                        {
                            IsDisabled = false;
                            IsExists = true;
                            url = "ViewOutletInspection.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID") + "&Type=false";
                        }
                        else
                        {
                            IsDisabled = false;
                            url = "AddOutletAlterationInspection.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID");
                        }
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet");
                        break;
                    case (long)Constants.SIInspectionType.OutletPerformance:
                        if (Convert.ToBoolean(Convert.ToInt32(Utility.GetDynamicPropertyValue(_Inspection, "IsOutletPerformanceInspectionExists"))))
                        {
                            IsDisabled = false;
                            IsExists = true;
                            url = "ViewOutletInspection.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID") + "&Type=true";
                        }
                        else
                        {
                            IsDisabled = false;
                            url = "AddOutletPerformanceInspection.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID");
                        }
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet");
                        break;
                        //**** 26-02-2018 Added by ATEEQ, As a ref by Sir Rizwan Alvi ****\\\
                    case (long)Constants.SIInspectionType.OutletChecking:
                        if (Convert.ToBoolean(Convert.ToInt32(Utility.GetDynamicPropertyValue(_Inspection, "IsOutletCheckingInspectionExists"))))
                        {
                            IsDisabled = false;
                            IsExists = true;
                            url = String.Format("OutletChecking.aspx?OutletCheckingID={0}&Outlet={1}&ScheduleID={2}&From={3}", Utility.GetDynamicPropertyValue(_Inspection, "OutletCheckingID"),Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet"),Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet"),"SC");
                        }
                        else
                        {
                            IsDisabled = false;
                            url = String.Format("OutletChecking.aspx?ScheduleDetailID={0}&ChannelID={1}&Outlet={2}&From={3}", Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID"), Utility.GetDynamicPropertyValue(_Inspection, "ChannelID"), Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet"),"SC");
                           // url = String.Format("../WaterTheft/AddWaterTheft.aspx?ScheduleDetailID={0}&ChannelID={1}&OutletID={2}" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID"),Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID"));
                        }
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet");
                        break;
                        /////////////////
                    case (long)Constants.SIInspectionType.DischargeTableCalculation:
                        if ((long)Constants.GaugeLevel.BedLevel == Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "GaugeLevelID")))
                        {
                            IsDisabled = false;
                            IsExists = new ScheduleInspectionBLL().IsDischargeTblBedLevelInspectionAlreadyExists(Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID")));
                            url = "AddDischargeTableCalcBL.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID");
                        }
                        else if ((long)Constants.GaugeLevel.CrestLevel == Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "GaugeLevelID")))
                        {
                            IsDisabled = false;
                            IsExists = new ScheduleInspectionBLL().IsDischargeTblCrestLevelInspectionAlreadyExists(Convert.ToInt64(Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID")));
                            url = "AddDischargeTableCalcCL.aspx?ScheduleDetailID=" + Utility.GetDynamicPropertyValue(_Inspection, "ScheduleDetailID");
                        }
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtGauge");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (InspectionTypeID)
                {
                    case (long)Constants.SIInspectionType.GaugeReading:
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtGauge");
                        break;
                    case (long)Constants.SIInspectionType.OutletAlteration:
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet");
                        break;
                    case (long)Constants.SIInspectionType.OutletPerformance:
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtOutlet");
                        break;
                    case (long)Constants.SIInspectionType.DischargeTableCalculation:
                        InspectionArea = Utility.GetDynamicPropertyValue(_Inspection, "InspectionAtGauge");
                        break;
                    default:
                        break;

                }
            }
            return Tuple.Create(url, IsDisabled, InspectionArea);
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            return settings.GetType().GetProperty(name) != null;
        }

    }
}