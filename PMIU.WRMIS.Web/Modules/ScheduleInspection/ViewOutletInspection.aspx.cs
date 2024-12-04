using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ViewOutletInspection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    bool? IsScheduled = null;
                    bool? IsPerformanceOutlet = null;


                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                        {
                            IsPerformanceOutlet = Convert.ToBoolean(Request.QueryString["Type"]);
                        }
                        hdnScheduleDetailChannelID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        if (!string.IsNullOrEmpty(Request.QueryString["IsScheduled"]))
                        {
                            IsScheduled = Convert.ToBoolean(Request.QueryString["IsScheduled"]);
                            ViewOutletInspectionDetail(IsScheduled, IsPerformanceOutlet);
                            hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                            //hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/SearchInspection.aspx");
                        }
                        else
                        {
                            ViewOutletInspectionDetail(IsScheduled, IsPerformanceOutlet);
                            hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value));
                        }
                        

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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddOutletPerformanceInspectionNotes);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void ViewOutletInspectionDetail(bool? IsScheduled, bool? IsPerformanceOutlet)
        {
            try
            {
                GetOutletInspection(IsScheduled, IsPerformanceOutlet);
                if (null == IsScheduled || IsScheduled == true)
                {
                    if (Convert.ToInt32(hdnInspectionTypeID.Value) == (int)Constants.SIInspectionType.OutletAlteration)
                    {
                        SI_OutletAlterationHistroy bllOutletAlteration = new ScheduleInspectionBLL().GetOutletAlterationInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value));
                        lblInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(bllOutletAlteration.AlterationDate));
                        lblInspectionTime.Text = Convert.ToString(Utility.GetFormattedTime(bllOutletAlteration.AlterationDate.Value));
                        lblOutletHeight.Text = Convert.ToString(bllOutletAlteration.OutletHeight);
                        lblHeadAboveCrest.Text = Convert.ToString(bllOutletAlteration.OutletCrest);
                        lblWorkingHead.Text = Convert.ToString(bllOutletAlteration.OutletWorkingHead);
                        lblDBWidth.Text = Convert.ToString(bllOutletAlteration.OutletWidth);
                        lblADischarge.Text = Convert.ToString(bllOutletAlteration.Discharge);
                        lblRemarks.Text = bllOutletAlteration.Remarks;
                        tblOutletAlterationInspection.Visible = true;
                        tblOutletPerformanceInspection.Visible = false;
                    }
                    else if (Convert.ToInt32(hdnInspectionTypeID.Value) == (int)Constants.SIInspectionType.OutletPerformance)
                    {
                        CO_ChannelOutletsPerformance bllOutletPerformance = new ScheduleInspectionBLL().GetOutletsPerformanceInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value));
                        lblPInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(bllOutletPerformance.ObservationDate));
                        lblPInspectionTime.Text = Convert.ToString(Utility.GetFormattedTime(bllOutletPerformance.ObservationDate.Value));
                        lblPHeadAboveCrest.Text = Convert.ToString(bllOutletPerformance.HeadAboveCrest);
                        lblPWorkingHead.Text = Convert.ToString(bllOutletPerformance.WorkingHead);
                        lblPDischarge.Text = Convert.ToString(bllOutletPerformance.Discharge);
                        lblPEfficiency.Text = bllOutletPerformance.Discharge.HasValue ? CalculateEfficiency(Convert.ToString(bllOutletPerformance.Discharge.Value)) : string.Empty;
                        lblPRemarks.Text = bllOutletPerformance.Remarks;
                        lblPerformanceOutletHeight.Text = Convert.ToString(bllOutletPerformance.ObservedHeightY);
                        lblOutletDiameter.Text = Convert.ToString(bllOutletPerformance.ObservedWidthB);
                        tblOutletPerformanceInspection.Visible = true;
                        tblOutletAlterationInspection.Visible = false;
                    }
                }

                else if (IsScheduled == false)
                {
                    if (IsPerformanceOutlet == true)
                    {
                        CO_ChannelOutletsPerformance bllOutletPerformance = new ScheduleInspectionBLL().GetOutletsPerformanceInspectionByID(Convert.ToInt64(hdnScheduleDetailChannelID.Value));
                        lblPInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(bllOutletPerformance.ObservationDate));
                        lblPInspectionTime.Text = Convert.ToString(Utility.GetFormattedTime(bllOutletPerformance.ObservationDate.Value));
                        lblPHeadAboveCrest.Text = Convert.ToString(bllOutletPerformance.HeadAboveCrest);
                        lblPWorkingHead.Text = Convert.ToString(bllOutletPerformance.WorkingHead);
                        lblPDischarge.Text = Convert.ToString(bllOutletPerformance.Discharge);
                        hdnOutletID.Value = Convert.ToString(bllOutletPerformance.OutletID);
                        if (!string.IsNullOrEmpty(hdnOutletID.Value) && Convert.ToInt64(hdnOutletID.Value) > 0)
                            hdnDischarge.Value = Convert.ToString(new ScheduleInspectionBLL().GetOutletDischarge(Convert.ToInt64(hdnOutletID.Value)));
                        lblPEfficiency.Text = bllOutletPerformance.Discharge.HasValue ? CalculateEfficiency(Convert.ToString(bllOutletPerformance.Discharge.Value)) : string.Empty;
                        lblPRemarks.Text = bllOutletPerformance.Remarks;
                        lblPerformanceOutletHeight.Text = Convert.ToString(bllOutletPerformance.ObservedHeightY);
                        lblOutletDiameter.Text = Convert.ToString(bllOutletPerformance.ObservedWidthB);
                        // hdnDischarge.Value = bllOutletPerformance.
                        tblOutletPerformanceInspection.Visible = true;
                        tblOutletAlterationInspection.Visible = false;
                        pageTitleID.InnerText = "View Outlet Performance Inspection";
                    }
                    else if (IsPerformanceOutlet == false)
                    {
                        SI_OutletAlterationHistroy bllOutletAlteration = new ScheduleInspectionBLL().GetOutletsAlterationInspectionByID(Convert.ToInt64(hdnScheduleDetailChannelID.Value));
                        lblInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(bllOutletAlteration.AlterationDate));
                        lblInspectionTime.Text = Convert.ToString(Utility.GetFormattedTime(bllOutletAlteration.AlterationDate.Value));
                        lblOutletHeight.Text = Convert.ToString(bllOutletAlteration.OutletHeight);
                        lblHeadAboveCrest.Text = Convert.ToString(bllOutletAlteration.OutletCrest);
                        lblWorkingHead.Text = Convert.ToString(bllOutletAlteration.OutletWorkingHead);
                        lblDBWidth.Text = Convert.ToString(bllOutletAlteration.OutletWidth);
                        lblADischarge.Text = Convert.ToString(bllOutletAlteration.Discharge);
                        lblRemarks.Text = bllOutletAlteration.Remarks;
                        tblOutletAlterationInspection.Visible = true;
                        tblOutletPerformanceInspection.Visible = false;
                        pageTitleID.InnerText = "View Outlet Alteration Inspection";
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetOutletInspection(bool? IsScheduled, bool? IsPerformanceOutlet)
        {
            try
            {
                if (IsPerformanceOutlet == true)
                {
                    if (null == IsScheduled || IsScheduled == true)
                    {
                        dynamic bllOutletInspection = new ScheduleInspectionBLL().GetOutletInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value), (long)Constants.SIInspectionType.OutletPerformance);

                        ViewOutletInspectionNotes.ScheduleTitle = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleTitle");
                        ViewOutletInspectionNotes.ScheduleStatus = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleStatus");
                        ViewOutletInspectionNotes.PreparedBy = Utility.GetDynamicPropertyValue(bllOutletInspection, "PreparedBy");
                        ViewOutletInspectionNotes.FromDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "FromDate");
                        ViewOutletInspectionNotes.ToDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "ToDate");
                        ViewOutletInspectionNotes.ChannelName = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelName");
                        ViewOutletInspectionNotes.OutletName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletRDChannelSide");
                        ViewOutletInspectionNotes.OutletTypeName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeName");
                        //ViewOutletInspectionNotes.InpectedBy
                        hdnChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelID");
                        hdnOutletID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletID");
                        hdnInspectionTypeID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "InspectionTypeID");
                        hdnScheduleDetailChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleDetailChannelID");
                        hdnScheduleID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleID");
                        OutletInspectionNotes.IsVisible = false;
                        if (!string.IsNullOrEmpty(hdnOutletID.Value) && Convert.ToInt64(hdnOutletID.Value) > 0)
                            hdnDischarge.Value = Convert.ToString(new ScheduleInspectionBLL().GetOutletDischarge(Convert.ToInt64(hdnOutletID.Value)));

                        pageTitleID.InnerText = "View Outlet Performance Inspection";
                    }
                    else
                    {
                        dynamic OutletPerformanceData = new ScheduleInspectionBLL().GetGaugeChannelByOutletPerformanceID(Convert.ToInt64(hdnScheduleDetailChannelID.Value));

                        ViewOutletInspectionNotes.ScheduleTitle = "Unscheduled Inspection";
                        ViewOutletInspectionNotes.ScheduleStatus = "N/A";
                        ViewOutletInspectionNotes.PreparedBy = "N/A";
                        ViewOutletInspectionNotes.FromDate = "N/A";
                        ViewOutletInspectionNotes.ToDate = "N/A";
                        ViewOutletInspectionNotes.ChannelName = Utility.GetDynamicPropertyValue(OutletPerformanceData, "ChannelName");
                        ViewOutletInspectionNotes.OutletName = Utility.GetDynamicPropertyValue(OutletPerformanceData, "OutletName");
                        ViewOutletInspectionNotes.OutletTypeName = Utility.GetDynamicPropertyValue(OutletPerformanceData, "OutletType");
                        ViewOutletInspectionNotes.InpectedBy = Utility.GetDynamicPropertyValue(OutletPerformanceData, "CreatedBy");
                        OutletInspectionNotes.IsVisible = true;
                        pageTitleID.InnerText = "View Outlet Performance UnScheduled Inspection";
                    }

                    

                }
                else if (IsPerformanceOutlet == false)
                {
                    if (null == IsScheduled || IsScheduled == true)
                    {
                        dynamic bllOutletInspection = new ScheduleInspectionBLL().GetOutletInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value), (long)Constants.SIInspectionType.OutletAlteration);

                        ViewOutletInspectionNotes.ScheduleTitle = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleTitle");
                        ViewOutletInspectionNotes.ScheduleStatus = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleStatus");
                        ViewOutletInspectionNotes.PreparedBy = Utility.GetDynamicPropertyValue(bllOutletInspection, "PreparedBy");
                        ViewOutletInspectionNotes.FromDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "FromDate");
                        ViewOutletInspectionNotes.ToDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "ToDate");
                        ViewOutletInspectionNotes.ChannelName = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelName");
                        ViewOutletInspectionNotes.OutletName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletRDChannelSide");
                        ViewOutletInspectionNotes.OutletTypeName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeName");
                        hdnChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelID");
                        hdnOutletID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletID");
                        hdnInspectionTypeID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "InspectionTypeID");
                        hdnScheduleDetailChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleDetailChannelID");
                        hdnScheduleID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleID");
                        OutletInspectionNotes.IsVisible = false;
                        if (!string.IsNullOrEmpty(hdnOutletID.Value) && Convert.ToInt64(hdnOutletID.Value) > 0)
                            hdnDischarge.Value = Convert.ToString(new ScheduleInspectionBLL().GetOutletDischarge(Convert.ToInt64(hdnOutletID.Value)));

                        pageTitleID.InnerText = "View Outlet Alteration Inspection";
                    }
                    else
                    {
                        dynamic OutletAlterationData = new ScheduleInspectionBLL().GetGaugeChannelByOutletAlterationID(Convert.ToInt64(hdnScheduleDetailChannelID.Value));

                        ViewOutletInspectionNotes.ScheduleTitle = "Unscheduled Inspection";
                        ViewOutletInspectionNotes.ScheduleStatus = "N/A";
                        ViewOutletInspectionNotes.PreparedBy = "N/A";
                        ViewOutletInspectionNotes.FromDate = "N/A";
                        ViewOutletInspectionNotes.ToDate = "N/A";
                        ViewOutletInspectionNotes.ChannelName = Utility.GetDynamicPropertyValue(OutletAlterationData, "ChannelName");
                        ViewOutletInspectionNotes.OutletName = Utility.GetDynamicPropertyValue(OutletAlterationData, "OutletName");
                        ViewOutletInspectionNotes.OutletTypeName = Utility.GetDynamicPropertyValue(OutletAlterationData, "OutletType");
                        ViewOutletInspectionNotes.InpectedBy = Utility.GetDynamicPropertyValue(OutletAlterationData, "CreatedBy");
                        OutletInspectionNotes.IsVisible = true;
                        pageTitleID.InnerText = "View Outlet Alteration UnScheduled Inspection";
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private string CalculateEfficiency(string _Discharge)
        {
            string efficiency = string.Empty;
            string outletDischarge = hdnDischarge.Value;
            if (!string.IsNullOrEmpty(outletDischarge))
            {
                double discharge = Convert.ToDouble(outletDischarge);
                if (discharge != 0)
                {
                    if (!string.IsNullOrEmpty(_Discharge))
                    {
                        efficiency = Math.Round(((Convert.ToDouble(_Discharge) / discharge) * 100), 2).ToString();
                    }
                    else
                    {
                        efficiency = string.Empty;
                    }
                }
                else
                {
                    efficiency = string.Empty;
                }
            }
            else
            {
                efficiency = string.Empty;
            }
            return efficiency;
        }

    }
}