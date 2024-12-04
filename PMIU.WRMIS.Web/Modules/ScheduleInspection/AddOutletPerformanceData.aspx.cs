using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddOutletPerformanceData : BasePage
    {
        #region ViewState

        public string OutletID_VS = "OutletID";
        public string Designdis = "Designdis";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    InitialBind();
                    hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/CriteriaForSpecificOutlet.aspx?ChannelID={0}", Convert.ToString(hdnChannelID.Value));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void InitialBind()
        {
            try
            {
                long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/ScheduleInspection/AddOutletPerformanceData.aspx");

                if (HasRights)
                {
                    if (CanAdd == false)
                    {
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnSave.Visible = true;
                    }

                    long OutletID = Convert.ToInt64(Request.QueryString["OutletID"]);
                    ViewState[OutletID_VS] = OutletID;
                    object OutletInformation = new DailyDataBLL().OutletInformation(OutletID);
                    txtDate.Text = Utility.GetFormattedDate(DateTime.Now);

                    if (OutletInformation != null)
                    {
                        lblChnlName.Text = Convert.ToString(OutletInformation.GetType().GetProperty("ChannelName").GetValue(OutletInformation));
                        lblChnlType.Text = Convert.ToString(OutletInformation.GetType().GetProperty("ChannelType").GetValue(OutletInformation));
                        lblOutletRD.Text = Convert.ToString(OutletInformation.GetType().GetProperty("OutletRDs").GetValue(OutletInformation));
                        lblOutletSide.Text = Convert.ToString(OutletInformation.GetType().GetProperty("Outletside").GetValue(OutletInformation));
                        lblPoliceStation.Text = Convert.ToString(OutletInformation.GetType().GetProperty("PoliceStation").GetValue(OutletInformation));
                        lblVillage.Text = Convert.ToString(OutletInformation.GetType().GetProperty("Village").GetValue(OutletInformation));
                        lblOutletType.Text = Convert.ToString(OutletInformation.GetType().GetProperty("OutletType").GetValue(OutletInformation));

                        //lblHead.Text = Convert.ToString(OutletInformation.GetType().GetProperty("HeadAboveCrest").GetValue(OutletInformation));
                        //lblMMH.Text = Convert.ToString(OutletInformation.GetType().GetProperty("MMH").GetValue(OutletInformation));
                        //lblDesignDischarge.Text = 
                        hdnDesignDischarge.Value = Convert.ToString(OutletInformation.GetType().GetProperty("DesignDischarge").GetValue(OutletInformation));
                        hdnChannelID.Value = Convert.ToString(OutletInformation.GetType().GetProperty("ChannelID").GetValue(OutletInformation));
                    }
                    else
                    {
                        lblChnlName.Text = "";
                        lblChnlType.Text = "";
                        lblOutletRD.Text = "";
                        lblOutletSide.Text = "";
                        lblPoliceStation.Text = "";
                        lblVillage.Text = "";
                        lblOutletType.Text = "";

                        // lblMMH.Text = "";
                        // lblDesignDischarge.Text = "";
                        hlBack.Visible = true;

                        Master.ShowMessage(Message.NoOutlet.Description, SiteMaster.MessageType.Error);
                        divDate.Visible = false;
                        fields.Visible = false;
                        mfields.Visible = false;
                        bfields.Visible = false;
                        btnSave.Visible = false;
                        //lblObserved.Visible = false;
                    }
                }
                else
                {
                    Master.ShowMessage("Not allowed to view screen/Appropriate message", SiteMaster.MessageType.Error);
                    divDate.Visible = false;
                    divFields.Visible = false;
                    btnSave.Visible = false;
                    //lblObserved.Visible = false;
                    tableInfo.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SIOutletPerformanceData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CalculateEfficiency(string _Discharge, string _DesignDischarge)
        {
            string efficiency = string.Empty;
            if (!string.IsNullOrEmpty(_DesignDischarge))
            {
                double discharge = Convert.ToDouble(_DesignDischarge);
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

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static void CalculateEfficiency(string _Discharge, string _DesignDischarge)
        //{
        //    if (!string.IsNullOrEmpty(_DesignDischarge))
        //    {
        //        double DesignDisch = Convert.ToDouble(_DesignDischarge);
        //        if (DesignDisch != 0)
        //        {
        //            if (!string.IsNullOrEmpty(_Discharge))
        //            {
        //                double dDischarge = Convert.ToDouble(_Discharge);
        //                txtEfficiency.Text = Math.Round(((dDischarge / DesignDisch) * 100), 2).ToString();
        //            }
        //            else
        //            {
        //                txtEfficiency.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            Master.ShowMessage(Message.DivideByZero.Description, SiteMaster.MessageType.Error);
        //        }
        //    }
        //    else
        //    {
        //        Master.ShowMessage(Message.ValueDoesNotExist.Description, SiteMaster.MessageType.Error);
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text != null && DateTime.Now < Convert.ToDateTime(txtDate.Text))
                {
                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    CO_ChannelOutletsPerformance objSave = new CO_ChannelOutletsPerformance();
                    objSave.OutletID = Convert.ToInt64(ViewState[OutletID_VS]);
                    if (txtDate.Text != null)
                        objSave.ObservationDate = Utility.GetParsedDate(txtDate.Text).Add(DateTime.Now.TimeOfDay);
                    if (txtHeadAbove.Text != "")
                        objSave.HeadAboveCrest = Convert.ToDouble(txtHeadAbove.Text);
                    if (txtWorkingHead.Text != "")
                        objSave.WorkingHead = Convert.ToDouble(txtWorkingHead.Text);
                    objSave.Discharge = Convert.ToDouble(txtDischarge.Text);
                    objSave.CreatedBy = SessionManagerFacade.UserInformation.ID;
                    objSave.CreatedDate = DateTime.Now;
                    objSave.Source = Configuration.RequestSource.RequestFromWeb;
                    objSave.UserID = SessionManagerFacade.UserInformation.ID;
                    if (txtOutletHeight.Text != "")
                    {
                        objSave.ObservedHeightY = Convert.ToDouble(txtOutletHeight.Text);
                    }
                    if (txtDBWidth.Text != "")
                    {
                        objSave.ObservedWidthB = Convert.ToDouble(txtDBWidth.Text);
                    }
                    new DailyDataBLL().SaveData(objSave);
                    CriteriaForSpecificOutlet.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/CriteriaForSpecificOutlet.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value), false);

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}