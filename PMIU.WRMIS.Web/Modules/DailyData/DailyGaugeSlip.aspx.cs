using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;


namespace PMIU.WRMIS.Web.Modules.DailyData
{
    // IIf(Fields!Gauge.Value Mod 1 > 0,Format(Fields!Gauge.Value,"#,0.00"),Format(Fields!Gauge.Value,"#,0")) Gauge Formatting in report
    public partial class DailyGaugeSlip : BasePage
    {
        #region Page Constants and Variables

        #region View State Keys

        public const string IndusPunjabBarrageName = "IndusPunjabBarrageName";
        public const string IndusPunjabChannelName = "IndusPunjabChannelName";
        public const string IndusSindhBarrageName = "IndusSindhBarrageName";
        public const string IndusSindhChannelName = "IndusSindhChannelName";
        public const string JhelumBarrageName = "JhelumBarrageName";
        public const string JhelumChannelName = "JhelumChannelName";
        public const string ChenabBarrageName = "ChenabBarrageName";
        public const string ChenabChannelName = "ChenabChannelName";
        public const string ChenabOtherBarrageName = "ChenabOtherBarrageName";
        public const string ChenabOtherChannelName = "ChenabOtherChannelName";
        public const string RaviBarrageName = "RaviBarrageName";
        public const string RaviChannelName = "RaviChannelName";
        public const string SutlejBarrageName = "SutlejBarrageName";
        public const string SutlejChannelName = "SutlejChannelName";
        public const string AllowEditFlag = "AllowEditFlag";

        #endregion

        #region Fixed Site Labels

        public const string RaviRiver = "Ravi River";
        public const string SutlejRiver = "Sutlej River";
        public const string ReservoirFullConservationLevel = "Reservoir Full Conservation Level";
        public const string TotalOutflow = "Total Outflow";
        public const string ChashmaBarrage = "Chashma Barrage";
        public const string FullConservationLevel = "Full Conservation Level";
        public const string Withdrawals = "Withdrawals";
        public const string GudduBarrage = "Guddu";
        public const string SukkurBarrage = "Sukkur";
        public const string KotriBarrage = "Kotri";
        public const string Storage = "Storage";

        #endregion

        #region Dam Total Discharge Calculation Variables

        public double TerbelaTotalDischarge = 0;
        public double ManglaTotalDischarge = 0;
        public double SindhBarragesWithdrawals = 0;
        public bool ShowSindhBarragesWithdrawals = false;

        #endregion

        #region Screen Constants

        public const int IndusDamTotalRowIndex = 9;
        public const int ManglaDamTotalRowIndex = 9;
        public const int ReserviorFullConservationLevelRowIndex = 0;
        public const int GudduBarrageWithdrawalRowIndex = 3;
        public const int SukkurBarrageWithdrawalRowIndex = 7;
        public const int KotriBarrageWithdrawalRowIndex = 11;
        public const int FullConservationLevelRowIndex = 4;
        public const int MaxDamGaugeLength = 7;
        public const int MaxDamDischargeLength = 10;
        public const int MaxBarrageGaugeLength = 6;
        public const int MaxBarrageDischargeLength = 10;
        public const int DataTableFirstRowIndex = 0;
        public const string DamReservoirLevelColumnName = "DamReservoirLevel";
        public const bool LoadLatest = true;

        #endregion

        #region GridIndex

        public const int IndusDamIndex = 0;
        public const int IndusPunjabBarragesIndex = 1;
        public const int IndusSindhBarragesIndex = 2;
        public const int JhelumDamIndex = 3;
        public const int JhelumBarragesIndex = 4;
        public const int ChenabBarragesIndex = 5;
        public const int ChenabOtherBarragesIndex = 6;
        public const int RaviBarragesIndex = 7;
        public const int SutlejBarragesIndex = 8;

        #endregion

        #region Accordion Complete Label Booleans

        public bool IndusDamAccordion = true;
        public bool IndusPunjabBarragesAccordion = true;
        public bool IndusSindhBarragesAccordion = true;
        public bool ManglaDamAccordion = true;
        public bool JhelumBarragesAccordion = true;
        public bool ChenabBarragesAccordion = true;
        public bool ChenabOtherBarragesAccordion = true;
        public bool RaviBarragesAccordion = true;
        public bool SutlejBarragesAccordion = true;

        #endregion

        #region DataKeys Indexes

        public int EnableGaugeDischargeIndex = 0;
        public int MinValueGaugeIndex = 1;
        public int MaxValueGaugeIndex = 2;
        public int MinValueDischargeIndex = 3;
        public int MaxValueDischargeIndex = 4;
        public int GaugeCategoryIDIndex = 5;

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    string roles = Utility.ReadConfiguration("AllowEditRoles");
                    List<string> AllowEditRoles = roles.Split(',').ToList();

                    bool AllowEdit = false;

                    if (mdlUser != null && AllowEditRoles.Contains(mdlUser.UA_Roles.Name))
                    {
                        AllowEdit = base.CanEdit;
                    }

                    ViewState[AllowEditFlag] = AllowEdit;

                    SetPageTitle();
                    string Now = Utility.GetFormattedDate(DateTime.Now);
                    txtDate.Text = Now;
                    BindGrids();
                    BindMonthDropdown();
                    bindYearDropDown();



                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 22-02-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyGaugeSlip);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds data to all grids
        /// Created on 23-02-2016
        /// </summary>
        private void BindGrids()
        {
            DateTime Date = Utility.GetParsedDate(txtDate.Text.Trim());
            bool IsLoadLatest = false;

            DailyDataBLL bllDailyData = new DailyDataBLL();

            ResetAccordions();

            //if (Date < DateTime.Now.Date)
            //{
            //    IsLoadLatest = true;
            //}

            DataSet dsGaugeSlip = bllDailyData.GetGaugeSlipData(Date, IsLoadLatest);

            BindIndusDam(dsGaugeSlip.Tables[IndusDamIndex], Date);
            BindIndusPunjabBarrages(dsGaugeSlip.Tables[IndusPunjabBarragesIndex], Date);
            BindIndusSindhBarrages(dsGaugeSlip.Tables[IndusSindhBarragesIndex], Date);
            BindManglaDam(dsGaugeSlip.Tables[JhelumDamIndex], Date);
            BindJhelumBarrages(dsGaugeSlip.Tables[JhelumBarragesIndex], Date);
            BindChenabBarrages(dsGaugeSlip.Tables[ChenabBarragesIndex], Date);
            BindChenabOtherBarrages(dsGaugeSlip.Tables[ChenabOtherBarragesIndex], Date);
            BindRaviBarrages(dsGaugeSlip.Tables[RaviBarragesIndex], Date);
            BindSutlejBarrages(dsGaugeSlip.Tables[SutlejBarragesIndex], Date);
        }

        /// <summary>
        /// This function closes all the accordions.
        /// Created On 03-03-2016
        /// </summary>
        private void ResetAccordions()
        {
            iconIndusDam.Attributes["class"] = "fa fa-chevron-down";
            divIndusDam.Attributes["style"] = "display: none;";
            iconPunjabIndusBarrages.Attributes["class"] = "fa fa-chevron-down";
            divPunjabIndusBarrages.Attributes["style"] = "display: none;";
            iconSindhIndusBarrages.Attributes["class"] = "fa fa-chevron-down";
            divSindhIndusBarrages.Attributes["style"] = "display: none;";
            iconManglaDam.Attributes["class"] = "fa fa-chevron-down";
            divManglaDam.Attributes["style"] = "display: none;";
            iconJhelumBarrages.Attributes["class"] = "fa fa-chevron-down";
            divJhelumBarrages.Attributes["style"] = "display: none;";
            iconChenabBarrages.Attributes["class"] = "fa fa-chevron-down";
            divChenabBarrages.Attributes["style"] = "display: none;";
            iconChenabOtherBarrages.Attributes["class"] = "fa fa-chevron-down";
            divChenabOtherBarrages.Attributes["style"] = "display: none;";
            iconRaviBarrages.Attributes["class"] = "fa fa-chevron-down";
            divRaviBarrages.Attributes["style"] = "display: none;";
            iconSutlejBarrages.Attributes["class"] = "fa fa-chevron-down";
            divSutlejBarrages.Attributes["style"] = "display: none;";
        }

        #region Indus Dam

        /// <summary>
        /// This function binds data to the Indus river dams grid.
        /// Created On 23-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindIndusDam(DataTable _dtIndusDam, DateTime _Date)
        {
            var DamReservoirLevelRow = (from row in _dtIndusDam.AsEnumerable()
                                        where row.Field<double?>("DamReservoirLevel") != null
                                        select row).FirstOrDefault();

            DataRow drIndusDam = _dtIndusDam.NewRow();

            drIndusDam["ID"] = 0;
            drIndusDam["GaugeSlipSiteID"] = 0;
            drIndusDam["Name"] = ReservoirFullConservationLevel;
            drIndusDam["AFSQ"] = DBNull.Value;
            drIndusDam["Gauge"] = Convert.ToDouble(DamReservoirLevelRow[DamReservoirLevelColumnName]);
            drIndusDam["Discharge"] = DBNull.Value;
            drIndusDam["MinValueGauge"] = DBNull.Value;
            drIndusDam["MaxValueGauge"] = DBNull.Value;
            drIndusDam["MinValueDischarge"] = DBNull.Value;
            drIndusDam["MaxValueDischarge"] = DBNull.Value;
            drIndusDam["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusDam.Rows.InsertAt(drIndusDam, ReserviorFullConservationLevelRowIndex);

            drIndusDam = _dtIndusDam.NewRow();

            drIndusDam["ID"] = 0;
            drIndusDam["GaugeSlipSiteID"] = 0;
            drIndusDam["Name"] = TotalOutflow;
            drIndusDam["AFSQ"] = DBNull.Value;
            drIndusDam["Gauge"] = DBNull.Value;
            drIndusDam["Discharge"] = DBNull.Value;
            drIndusDam["MinValueGauge"] = DBNull.Value;
            drIndusDam["MaxValueGauge"] = DBNull.Value;
            drIndusDam["MinValueDischarge"] = DBNull.Value;
            drIndusDam["MaxValueDischarge"] = DBNull.Value;
            drIndusDam["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusDam.Rows.InsertAt(drIndusDam, IndusDamTotalRowIndex);

            gvIndusDam.DataSource = _dtIndusDam;
            gvIndusDam.DataBind();

            if (IndusDamAccordion)
            {
                spanIndusDam.Attributes["class"] = "badge badge-success";
                spanIndusDam.InnerHtml = "Complete";
            }
            else
            {
                spanIndusDam.Attributes["class"] = "badge badge-important";
                spanIndusDam.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnIndusDamSave.Enabled = false;
                lbtnIndusDamCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnIndusDamSave.Visible = false;
                }
            }
            else
            {
                btnIndusDamSave.Enabled = true;
                lbtnIndusDamCancel.Enabled = true;
            }

        }

        protected void gvIndusDam_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSiteName = (Label)e.Row.FindControl("lblSiteName");
                    Label lblAFSQ = (Label)e.Row.FindControl("lblAFSQ");
                    Label lblGauge = (Label)e.Row.FindControl("lblGauge");
                    Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    List<string> SiteForTotalOutflow = new List<string>() { "Aux Spilway", "Power Channel", "R Bank Tunnels / Units", "L. Bank Tunnel # 5" };

                    if (lblSiteName.Text.Trim() == ReservoirFullConservationLevel)
                    {
                        lblGauge.Visible = true;
                        lblAFSQ.Visible = false;
                        txtGauge.Visible = false;
                        txtDischarge.Visible = false;
                    }
                    else if (SiteForTotalOutflow.Contains(lblSiteName.Text.Trim()))
                    {
                        if (DischargeString != string.Empty)
                        {
                            double Discharge = Convert.ToDouble(DischargeString);
                            TerbelaTotalDischarge = TerbelaTotalDischarge + Discharge;
                        }

                        if (GaugeString == string.Empty && DischargeString == string.Empty)
                        {
                            IndusDamAccordion = false;
                        }
                    }
                    else if (lblSiteName.Text.Trim() == TotalOutflow)
                    {
                        lblSiteName.Font.Bold = true;
                        lblAFSQ.Visible = false;
                        txtGauge.Visible = false;
                        txtDischarge.Visible = false;
                        lblDischarge.Visible = true;
                        lblDischarge.Font.Bold = true;

                        if (TerbelaTotalDischarge != 0)
                        {
                            lblDischarge.Text = TerbelaTotalDischarge.ToString();
                        }
                    }
                    else
                    {
                        if (GaugeString == string.Empty && DischargeString == string.Empty)
                        {
                            IndusDamAccordion = false;
                        }
                    }

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvIndusDam.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvIndusDam.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvIndusDam.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvIndusDam.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvIndusDam.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvIndusDam.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvIndusDam.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvIndusDam.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvIndusDam.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvIndusDam.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnIndusDamSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvIndusDam.Rows)
                {
                    Label lblSiteName = (Label)Row.FindControl("lblSiteName");

                    if (lblSiteName.Text.Trim() != ReservoirFullConservationLevel && lblSiteName.Text.Trim() != TotalOutflow)
                    {
                        Label lblID = (Label)Row.FindControl("lblID");
                        Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                        TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                        TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                        Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                        string GaugeString = txtGauge.Text.Trim();
                        string DischargeString = txtDischarge.Text.Trim();
                        string AFSQValue = lblAFSQ.Text.Trim();

                        long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                        CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                        if (mdlGaugeSlipDailyData != null)
                        {
                            mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                            mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                            bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                        else
                        {
                            mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                            mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                            mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                            if (AFSQValue != string.Empty)
                            {
                                mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                            }

                            if (GaugeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                            }

                            if (DischargeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                            }

                            bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                    }
                }

                ResetAccordions();

                DataTable dtIndusDam = new DailyDataBLL().GetGaugeSlipDamData(ReadingDate, IndusDamIndex, LoadLatest);

                BindIndusDam(dtIndusDam, ReadingDate);

                iconIndusDam.Attributes["class"] = "fa fa-chevron-up";
                divIndusDam.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnIndusDamCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtIndusDam = new DailyDataBLL().GetGaugeSlipDamData(Date, IndusDamIndex);

                BindIndusDam(dtIndusDam, Date);

                iconIndusDam.Attributes["class"] = "fa fa-chevron-up";
                divIndusDam.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Indus Punjab Barrages

        /// <summary>
        /// This function binds data to the Indus river punjab barrages grid.
        /// Created On 24-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindIndusPunjabBarrages(DataTable _dtIndusPunjabBarrages, DateTime _Date)
        {
            var DamReservoirLevelRow = (from row in _dtIndusPunjabBarrages.AsEnumerable()
                                        where row.Field<double?>("DamReservoirLevel") != null
                                        select row).FirstOrDefault();

            DataRow drIndusPunjabBarrages = _dtIndusPunjabBarrages.NewRow();

            drIndusPunjabBarrages["ID"] = 0;
            drIndusPunjabBarrages["GaugeSlipSiteID"] = 0;
            drIndusPunjabBarrages["StationName"] = ChashmaBarrage;
            drIndusPunjabBarrages["ChannelName"] = string.Empty;
            drIndusPunjabBarrages["SiteName"] = FullConservationLevel;
            drIndusPunjabBarrages["AFSQ"] = DBNull.Value;
            drIndusPunjabBarrages["GaugeID"] = 0;
            drIndusPunjabBarrages["Gauge"] = Convert.ToDouble(DamReservoirLevelRow[DamReservoirLevelColumnName]);
            drIndusPunjabBarrages["Indent"] = DBNull.Value;
            drIndusPunjabBarrages["Discharge"] = DBNull.Value;
            drIndusPunjabBarrages["MinValueGauge"] = DBNull.Value;
            drIndusPunjabBarrages["MaxValueGauge"] = DBNull.Value;
            drIndusPunjabBarrages["MinValueDischarge"] = DBNull.Value;
            drIndusPunjabBarrages["MaxValueDischarge"] = DBNull.Value;
            drIndusPunjabBarrages["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusPunjabBarrages.Rows.InsertAt(drIndusPunjabBarrages, FullConservationLevelRowIndex);

            gvPunjabIndusBarrages.DataSource = _dtIndusPunjabBarrages;
            gvPunjabIndusBarrages.DataBind();

            ViewState[IndusPunjabBarrageName] = null;
            ViewState[IndusPunjabChannelName] = null;

            if (IndusPunjabBarragesAccordion)
            {
                spanPunjabIndusBarrages.Attributes["class"] = "badge badge-success";
                spanPunjabIndusBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanPunjabIndusBarrages.Attributes["class"] = "badge badge-important";
                spanPunjabIndusBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnPunjabIndusBarragesSave.Enabled = false;
                lbtnPunjabIndusBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnPunjabIndusBarragesSave.Visible = false;
                }
            }
            else
            {
                btnPunjabIndusBarragesSave.Enabled = true;
                lbtnPunjabIndusBarragesCancel.Enabled = true;
            }
        }

        protected void gvPunjabIndusBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblSiteName = (Label)e.Row.FindControl("lblSiteName");
                    Label lblGauge = (Label)e.Row.FindControl("lblGauge");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (lblSiteName.Text.Trim() == FullConservationLevel)
                    {
                        lblGauge.Visible = true;
                        txtGauge.Visible = false;
                        txtDischarge.Visible = false;

                        lblGauge.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginLeft, "7px");
                    }
                    else
                    {
                        if (GaugeString == string.Empty && DischargeString == string.Empty)
                        {
                            IndusPunjabBarragesAccordion = false;
                        }
                    }

                    if (ViewState[IndusPunjabBarrageName] == null)
                    {
                        ViewState[IndusPunjabBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[IndusPunjabChannelName] = lblChannelName.Text.Trim();
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[IndusPunjabBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[IndusPunjabChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[IndusPunjabChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[IndusPunjabBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[IndusPunjabChannelName] = lblChannelName.Text.Trim();
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvPunjabIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnPunjabIndusBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvPunjabIndusBarrages.Rows)
                {
                    Label lblSiteName = (Label)Row.FindControl("lblSiteName");

                    if (lblSiteName.Text.Trim() != FullConservationLevel)
                    {
                        Label lblID = (Label)Row.FindControl("lblID");
                        Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                        TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                        TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                        TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                        Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                        string GaugeString = txtGauge.Text.Trim();
                        string DischargeString = txtDischarge.Text.Trim();
                        string IndentString = txtIndent.Text.Trim();
                        string AFSQValue = lblAFSQ.Text.Trim();

                        long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                        CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                        if (mdlGaugeSlipDailyData != null)
                        {
                            mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                            mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                            bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                        else
                        {
                            mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                            mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                            mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                            if (AFSQValue != string.Empty)
                            {
                                mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                            }

                            if (GaugeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                            }

                            if (IndentString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                            }

                            if (DischargeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                            }

                            bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                    }
                }

                ResetAccordions();

                DataTable dtIndusPunjabBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, IndusPunjabBarragesIndex, LoadLatest);

                BindIndusPunjabBarrages(dtIndusPunjabBarrages, ReadingDate);

                iconPunjabIndusBarrages.Attributes["class"] = "fa fa-chevron-up";
                divPunjabIndusBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnPunjabIndusBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtIndusPunjabBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, IndusPunjabBarragesIndex);

                BindIndusPunjabBarrages(dtIndusPunjabBarrages, Date);

                iconPunjabIndusBarrages.Attributes["class"] = "fa fa-chevron-up";
                divPunjabIndusBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Indus Sindh Barrages

        /// <summary>
        /// This function binds data to the Indus river sindh barrages grid.
        /// Created On 25-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindIndusSindhBarrages(DataTable _dtIndusSindhBarrages, DateTime _Date)
        {
            DataRow drIndusSindhBarrages = _dtIndusSindhBarrages.NewRow();

            drIndusSindhBarrages["ID"] = 0;
            drIndusSindhBarrages["GaugeSlipSiteID"] = 0;
            drIndusSindhBarrages["StationName"] = GudduBarrage;
            drIndusSindhBarrages["ChannelName"] = string.Empty;
            drIndusSindhBarrages["SiteName"] = Withdrawals;
            drIndusSindhBarrages["AFSQ"] = DBNull.Value;
            drIndusSindhBarrages["GaugeID"] = 0;
            drIndusSindhBarrages["Gauge"] = DBNull.Value;
            drIndusSindhBarrages["Indent"] = DBNull.Value;
            drIndusSindhBarrages["Discharge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusSindhBarrages.Rows.InsertAt(drIndusSindhBarrages, GudduBarrageWithdrawalRowIndex);

            drIndusSindhBarrages = _dtIndusSindhBarrages.NewRow();

            drIndusSindhBarrages["ID"] = 0;
            drIndusSindhBarrages["GaugeSlipSiteID"] = 0;
            drIndusSindhBarrages["StationName"] = SukkurBarrage;
            drIndusSindhBarrages["ChannelName"] = string.Empty;
            drIndusSindhBarrages["SiteName"] = Withdrawals;
            drIndusSindhBarrages["AFSQ"] = DBNull.Value;
            drIndusSindhBarrages["GaugeID"] = 0;
            drIndusSindhBarrages["Gauge"] = DBNull.Value;
            drIndusSindhBarrages["Indent"] = DBNull.Value;
            drIndusSindhBarrages["Discharge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusSindhBarrages.Rows.InsertAt(drIndusSindhBarrages, SukkurBarrageWithdrawalRowIndex);

            drIndusSindhBarrages = _dtIndusSindhBarrages.NewRow();

            drIndusSindhBarrages["ID"] = 0;
            drIndusSindhBarrages["GaugeSlipSiteID"] = 0;
            drIndusSindhBarrages["StationName"] = KotriBarrage;
            drIndusSindhBarrages["ChannelName"] = string.Empty;
            drIndusSindhBarrages["SiteName"] = Withdrawals;
            drIndusSindhBarrages["AFSQ"] = DBNull.Value;
            drIndusSindhBarrages["GaugeID"] = 0;
            drIndusSindhBarrages["Gauge"] = DBNull.Value;
            drIndusSindhBarrages["Indent"] = DBNull.Value;
            drIndusSindhBarrages["Discharge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueGauge"] = DBNull.Value;
            drIndusSindhBarrages["MinValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["MaxValueDischarge"] = DBNull.Value;
            drIndusSindhBarrages["EnableGaugeDischarge"] = DBNull.Value;

            _dtIndusSindhBarrages.Rows.InsertAt(drIndusSindhBarrages, KotriBarrageWithdrawalRowIndex);

            gvSindhIndusBarrages.DataSource = _dtIndusSindhBarrages;
            gvSindhIndusBarrages.DataBind();

            ViewState[IndusSindhBarrageName] = null;
            ViewState[IndusSindhChannelName] = null;

            if (IndusSindhBarragesAccordion)
            {
                spanSindhIndusBarrages.Attributes["class"] = "badge badge-success";
                spanSindhIndusBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanSindhIndusBarrages.Attributes["class"] = "badge badge-important";
                spanSindhIndusBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnSindhIndusBarragesSave.Enabled = false;
                lbtnSindhIndusBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnSindhIndusBarragesSave.Visible = false;
                }
            }
            else
            {
                btnSindhIndusBarragesSave.Enabled = true;
                lbtnSindhIndusBarragesCancel.Enabled = true;
            }
        }

        protected void gvSindhIndusBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblSiteName = (Label)e.Row.FindControl("lblSiteName");
                    Label lblAFSQ = (Label)e.Row.FindControl("lblAFSQ");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string SiteNameString = lblSiteName.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (SiteNameString != Withdrawals && GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        IndusSindhBarragesAccordion = false;
                    }

                    if (ViewState[IndusSindhBarrageName] == null)
                    {
                        ViewState[IndusSindhBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[IndusSindhChannelName] = lblChannelName.Text.Trim();
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[IndusSindhBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[IndusSindhChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[IndusSindhChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[IndusSindhBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[IndusSindhChannelName] = lblChannelName.Text.Trim();
                        }
                    }

                    if (lblSiteName.Text.Trim() == Constants.UpStream)
                    {
                        if (DischargeString != string.Empty)
                        {
                            double Discharge = Convert.ToDouble(DischargeString);
                            SindhBarragesWithdrawals = Discharge;
                            ShowSindhBarragesWithdrawals = true;
                        }
                    }
                    else if (lblSiteName.Text.Trim() == Constants.DownStream)
                    {
                        if (DischargeString != string.Empty)
                        {
                            double Discharge = Convert.ToDouble(DischargeString);
                            SindhBarragesWithdrawals = SindhBarragesWithdrawals - Discharge;
                            ShowSindhBarragesWithdrawals = true;
                        }
                    }
                    else if (lblSiteName.Text.Trim() == Storage)
                    {
                        if (DischargeString != string.Empty)
                        {
                            double Discharge = Convert.ToDouble(DischargeString);
                            SindhBarragesWithdrawals = SindhBarragesWithdrawals - Discharge;
                            ShowSindhBarragesWithdrawals = true;
                        }
                    }
                    else if (lblSiteName.Text.Trim() == Withdrawals)
                    {
                        if (ShowSindhBarragesWithdrawals)
                        {
                            if (SindhBarragesWithdrawals > 0)
                            {
                                lblDischarge.Text = SindhBarragesWithdrawals.ToString();
                            }
                            else
                            {
                                lblDischarge.Text = "0";
                            }
                        }

                        SindhBarragesWithdrawals = 0;
                        ShowSindhBarragesWithdrawals = false;

                        lblBarrageName.Visible = false;
                        lblChannelName.Visible = false;
                        lblAFSQ.Visible = false;
                        txtGauge.Visible = false;
                        lblIndent.Visible = false;
                        txtDischarge.Visible = false;
                        lblDischarge.Visible = true;

                        lblSiteName.Font.Bold = true;
                        lblDischarge.Font.Bold = true;

                        lblDischarge.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginLeft, "7px");
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvSindhIndusBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSindhIndusBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvSindhIndusBarrages.Rows)
                {
                    Label lblSiteName = (Label)Row.FindControl("lblSiteName");

                    if (lblSiteName.Text.Trim() != Withdrawals)
                    {
                        Label lblID = (Label)Row.FindControl("lblID");
                        Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                        TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                        TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                        TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                        Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                        string GaugeString = txtGauge.Text.Trim();
                        string DischargeString = txtDischarge.Text.Trim();
                        string IndentString = txtIndent.Text.Trim();
                        string AFSQValue = lblAFSQ.Text.Trim();

                        long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                        CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                        if (mdlGaugeSlipDailyData != null)
                        {
                            mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                            mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                            bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                        else
                        {
                            mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                            mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                            mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                            if (AFSQValue != string.Empty)
                            {
                                mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                            }

                            if (GaugeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                            }

                            if (IndentString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                            }

                            if (DischargeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                            }

                            bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                    }
                }

                ResetAccordions();

                DataTable dtIndusSindhBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, IndusSindhBarragesIndex, LoadLatest);

                BindIndusSindhBarrages(dtIndusSindhBarrages, ReadingDate);

                iconSindhIndusBarrages.Attributes["class"] = "fa fa-chevron-up";
                divSindhIndusBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnSindhIndusBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtIndusSindhBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, IndusSindhBarragesIndex);

                BindIndusSindhBarrages(dtIndusSindhBarrages, Date);

                iconSindhIndusBarrages.Attributes["class"] = "fa fa-chevron-up";
                divSindhIndusBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Mangla Dam

        /// <summary>
        /// This function binds data to the Mangla Dam grid.
        /// Created On 24-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindManglaDam(DataTable _dtJhelumDam, DateTime _Date)
        {
            var DamReservoirLevelRow = (from row in _dtJhelumDam.AsEnumerable()
                                        where row.Field<double?>("DamReservoirLevel") != null
                                        select row).FirstOrDefault();

            DataRow drJhelumDam = _dtJhelumDam.NewRow();

            drJhelumDam["ID"] = 0;
            drJhelumDam["GaugeSlipSiteID"] = 0;
            drJhelumDam["Name"] = ReservoirFullConservationLevel;
            drJhelumDam["AFSQ"] = DBNull.Value;
            drJhelumDam["Gauge"] = Convert.ToDouble(DamReservoirLevelRow[DamReservoirLevelColumnName]);
            drJhelumDam["Discharge"] = DBNull.Value;
            drJhelumDam["MinValueGauge"] = DBNull.Value;
            drJhelumDam["MaxValueGauge"] = DBNull.Value;
            drJhelumDam["MinValueDischarge"] = DBNull.Value;
            drJhelumDam["MaxValueDischarge"] = DBNull.Value;
            drJhelumDam["EnableGaugeDischarge"] = DBNull.Value;

            _dtJhelumDam.Rows.InsertAt(drJhelumDam, ReserviorFullConservationLevelRowIndex);

            drJhelumDam = _dtJhelumDam.NewRow();

            drJhelumDam["ID"] = 0;
            drJhelumDam["GaugeSlipSiteID"] = 0;
            drJhelumDam["Name"] = TotalOutflow;
            drJhelumDam["AFSQ"] = DBNull.Value;
            drJhelumDam["Gauge"] = DBNull.Value;
            drJhelumDam["Discharge"] = DBNull.Value;
            drJhelumDam["MinValueGauge"] = DBNull.Value;
            drJhelumDam["MaxValueGauge"] = DBNull.Value;
            drJhelumDam["MinValueDischarge"] = DBNull.Value;
            drJhelumDam["MaxValueDischarge"] = DBNull.Value;
            drJhelumDam["EnableGaugeDischarge"] = DBNull.Value;

            _dtJhelumDam.Rows.InsertAt(drJhelumDam, ManglaDamTotalRowIndex);

            gvManglaDam.DataSource = _dtJhelumDam;
            gvManglaDam.DataBind();

            if (ManglaDamAccordion)
            {
                spanManglaDam.Attributes["class"] = "badge badge-success";
                spanManglaDam.InnerHtml = "Complete";
            }
            else
            {
                spanManglaDam.Attributes["class"] = "badge badge-important";
                spanManglaDam.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnManglaDamSave.Enabled = false;
                lbtnManglaDamCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnManglaDamSave.Visible = false;
                }
            }
            else
            {
                btnManglaDamSave.Enabled = true;
                lbtnManglaDamCancel.Enabled = true;
            }
        }

        protected void gvManglaDam_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSiteName = (Label)e.Row.FindControl("lblSiteName");
                    Label lblAFSQ = (Label)e.Row.FindControl("lblAFSQ");
                    Label lblGauge = (Label)e.Row.FindControl("lblGauge");
                    Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    List<string> SiteForTotalOutflow = new List<string>() { "Exalary Spilway", "Power Channel", "Jari Outlet" };

                    if (lblSiteName.Text.Trim() == ReservoirFullConservationLevel)
                    {
                        lblGauge.Visible = true;
                        lblAFSQ.Visible = false;
                        txtGauge.Visible = false;
                        txtDischarge.Visible = false;

                        lblGauge.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginLeft, "7px");
                    }
                    else if (SiteForTotalOutflow.Contains(lblSiteName.Text.Trim()))
                    {
                        if (DischargeString != string.Empty)
                        {
                            double Discharge = Convert.ToDouble(DischargeString);
                            ManglaTotalDischarge = ManglaTotalDischarge + Discharge;
                        }

                        if (GaugeString == string.Empty && DischargeString == string.Empty)
                        {
                            ManglaDamAccordion = false;
                        }
                    }
                    else if (lblSiteName.Text.Trim() == TotalOutflow)
                    {
                        lblSiteName.Font.Bold = true;
                        lblAFSQ.Visible = false;
                        txtGauge.Visible = false;
                        txtDischarge.Visible = false;
                        lblDischarge.Visible = true;
                        lblDischarge.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                        lblDischarge.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginLeft, "7px");

                        if (ManglaTotalDischarge != 0)
                        {
                            lblDischarge.Text = ManglaTotalDischarge.ToString();
                        }
                    }
                    else
                    {
                        if (GaugeString == string.Empty && DischargeString == string.Empty)
                        {
                            ManglaDamAccordion = false;
                        }
                    }

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvManglaDam.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvManglaDam.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvManglaDam.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvManglaDam.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvManglaDam.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvManglaDam.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvManglaDam.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvManglaDam.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvManglaDam.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvManglaDam.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnManglaDamSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvManglaDam.Rows)
                {
                    Label lblSiteName = (Label)Row.FindControl("lblSiteName");

                    if (lblSiteName.Text.Trim() != ReservoirFullConservationLevel && lblSiteName.Text.Trim() != TotalOutflow)
                    {
                        Label lblID = (Label)Row.FindControl("lblID");
                        Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                        TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                        TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                        Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                        string GaugeString = txtGauge.Text.Trim();
                        string DischargeString = txtDischarge.Text.Trim();
                        string AFSQValue = lblAFSQ.Text.Trim();

                        long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                        CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                        if (mdlGaugeSlipDailyData != null)
                        {
                            mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                            mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                            mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                            bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                        else
                        {
                            mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                            mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                            mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                            if (AFSQValue != string.Empty)
                            {
                                mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                            }

                            if (GaugeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                            }

                            if (DischargeString != string.Empty)
                            {
                                mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                            }

                            bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                        }
                    }
                }

                ResetAccordions();

                DataTable dtJhelumDam = new DailyDataBLL().GetGaugeSlipDamData(ReadingDate, JhelumDamIndex, LoadLatest);

                BindManglaDam(dtJhelumDam, ReadingDate);

                iconManglaDam.Attributes["class"] = "fa fa-chevron-up";
                divManglaDam.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnManglaDamCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtJhelumDam = new DailyDataBLL().GetGaugeSlipDamData(Date, JhelumDamIndex);

                BindManglaDam(dtJhelumDam, Date);

                iconManglaDam.Attributes["class"] = "fa fa-chevron-up";
                divManglaDam.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Jhelum Barrages

        /// <summary>
        /// This function binds data to the Jhelum river barrages grid.
        /// Created On 25-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindJhelumBarrages(DataTable _dtJhelumBarrages, DateTime _Date)
        {
            gvJhelumBarrages.DataSource = _dtJhelumBarrages;
            gvJhelumBarrages.DataBind();

            ViewState[JhelumBarrageName] = null;
            ViewState[JhelumChannelName] = null;

            if (JhelumBarragesAccordion)
            {
                spanJhelumBarrages.Attributes["class"] = "badge badge-success";
                spanJhelumBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanJhelumBarrages.Attributes["class"] = "badge badge-important";
                spanJhelumBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnJhelumBarragesSave.Enabled = false;
                lbtnJhelumBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnJhelumBarragesSave.Visible = false;
                }
            }
            else
            {
                btnJhelumBarragesSave.Enabled = true;
                lbtnJhelumBarragesCancel.Enabled = true;
            }
        }

        protected void gvJhelumBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        JhelumBarragesAccordion = false;
                    }

                    if (ViewState[JhelumBarrageName] == null)
                    {
                        ViewState[JhelumBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[JhelumChannelName] = lblChannelName.Text.Trim();
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[JhelumBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[JhelumChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[JhelumChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[JhelumBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[JhelumChannelName] = lblChannelName.Text.Trim();
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvJhelumBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnJhelumBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvJhelumBarrages.Rows)
                {
                    Label lblID = (Label)Row.FindControl("lblID");
                    Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                    TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                    Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string IndentString = txtIndent.Text.Trim();
                    string AFSQValue = lblAFSQ.Text.Trim();

                    long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                    CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                    if (mdlGaugeSlipDailyData != null)
                    {
                        mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                        mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                        bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                    else
                    {
                        mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                        mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                        mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                        if (AFSQValue != string.Empty)
                        {
                            mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                        }

                        if (GaugeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                        }

                        if (IndentString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                        }

                        if (DischargeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                        }

                        bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                }

                ResetAccordions();

                DataTable dtJhelumBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, JhelumBarragesIndex, LoadLatest);

                BindJhelumBarrages(dtJhelumBarrages, ReadingDate);

                iconJhelumBarrages.Attributes["class"] = "fa fa-chevron-up";
                divJhelumBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnJhelumBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtJhelumBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, JhelumBarragesIndex);

                BindJhelumBarrages(dtJhelumBarrages, Date);

                iconJhelumBarrages.Attributes["class"] = "fa fa-chevron-up";
                divJhelumBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Chenab Barrages

        /// <summary>
        /// This function binds data to the Chenab river barrages grid.
        /// Created On 26-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindChenabBarrages(DataTable _dtChenabBarrages, DateTime _Date)
        {
            gvChenabBarrages.DataSource = _dtChenabBarrages;
            gvChenabBarrages.DataBind();

            ViewState[ChenabBarrageName] = null;
            ViewState[ChenabChannelName] = null;

            if (ChenabBarragesAccordion)
            {
                spanChenabBarrages.Attributes["class"] = "badge badge-success";
                spanChenabBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanChenabBarrages.Attributes["class"] = "badge badge-important";
                spanChenabBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnChenabBarragesSave.Enabled = false;
                lbtnChenabBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnChenabBarragesSave.Visible = false;
                }
            }
            else
            {
                btnChenabBarragesSave.Enabled = true;
                lbtnChenabBarragesCancel.Enabled = true;
            }
        }

        protected void gvChenabBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        ChenabBarragesAccordion = false;
                    }

                    if (ViewState[ChenabBarrageName] == null)
                    {
                        ViewState[ChenabBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[ChenabChannelName] = lblChannelName.Text.Trim();
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[ChenabBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[ChenabChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[ChenabChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[ChenabBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[ChenabChannelName] = lblChannelName.Text.Trim();
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvChenabBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnChenabBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvChenabBarrages.Rows)
                {
                    Label lblID = (Label)Row.FindControl("lblID");
                    Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                    TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                    Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string IndentString = txtIndent.Text.Trim();
                    string AFSQValue = lblAFSQ.Text.Trim();

                    long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                    CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                    if (mdlGaugeSlipDailyData != null)
                    {
                        mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                        mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                        bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                    else
                    {
                        mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                        mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                        mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                        if (AFSQValue != string.Empty)
                        {
                            mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                        }

                        if (GaugeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                        }

                        if (IndentString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                        }

                        if (DischargeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                        }

                        bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                }

                ResetAccordions();

                DataTable dtChenabBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, ChenabBarragesIndex, LoadLatest);

                BindChenabBarrages(dtChenabBarrages, ReadingDate);

                iconChenabBarrages.Attributes["class"] = "fa fa-chevron-up";
                divChenabBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnChenabBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtChenabBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, ChenabBarragesIndex);

                BindChenabBarrages(dtChenabBarrages, Date);

                iconChenabBarrages.Attributes["class"] = "fa fa-chevron-up";
                divChenabBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Chenab Other Barrages

        /// <summary>
        /// This function binds data to the Chenab river Other barrages grid.
        /// Created On 26-02-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindChenabOtherBarrages(DataTable _dtChenabOtherBarrages, DateTime _Date)
        {
            gvChenabOtherBarrages.DataSource = _dtChenabOtherBarrages;
            gvChenabOtherBarrages.DataBind();

            ViewState[ChenabOtherBarrageName] = null;
            ViewState[ChenabOtherChannelName] = null;

            if (ChenabOtherBarragesAccordion)
            {
                spanChenabOtherBarrages.Attributes["class"] = "badge badge-success";
                spanChenabOtherBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanChenabOtherBarrages.Attributes["class"] = "badge badge-important";
                spanChenabOtherBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnChenabOtherBarragesSave.Enabled = false;
                lbtnChenabOtherBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnChenabOtherBarragesSave.Visible = false;
                }
            }
            else
            {
                btnChenabOtherBarragesSave.Enabled = true;
                lbtnChenabOtherBarragesCancel.Enabled = true;
            }
        }

        protected void gvChenabOtherBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    // All row will show Indent TextBox
                    //if (lblChannelName.Text == string.Empty) 
                    //{
                    lblIndent.Visible = false;
                    txtIndent.Visible = true;
                    //}

                    if (GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        ChenabOtherBarragesAccordion = false;
                    }

                    if (ViewState[ChenabOtherBarrageName] == null)
                    {
                        ViewState[ChenabOtherBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[ChenabOtherChannelName] = lblChannelName.Text.Trim();
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[ChenabOtherBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[ChenabOtherChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[ChenabOtherChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[ChenabOtherBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[ChenabOtherChannelName] = lblChannelName.Text.Trim();
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvChenabOtherBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnChenabOtherBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvChenabOtherBarrages.Rows)
                {
                    Label lblID = (Label)Row.FindControl("lblID");
                    Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                    TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                    Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string IndentString = txtIndent.Text.Trim();
                    string AFSQValue = lblAFSQ.Text.Trim();

                    long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                    CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                    if (mdlGaugeSlipDailyData != null)
                    {
                        mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                        mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                        bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                    else
                    {
                        mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                        mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                        mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                        if (AFSQValue != string.Empty)
                        {
                            mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                        }

                        if (GaugeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                        }

                        if (IndentString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                        }

                        if (DischargeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                        }

                        bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                }

                ResetAccordions();

                DataTable dtChenabOtherBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, ChenabOtherBarragesIndex, LoadLatest);

                BindChenabOtherBarrages(dtChenabOtherBarrages, ReadingDate);

                iconChenabOtherBarrages.Attributes["class"] = "fa fa-chevron-up";
                divChenabOtherBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnChenabOtherBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtChenabOtherBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, ChenabOtherBarragesIndex);

                BindChenabOtherBarrages(dtChenabOtherBarrages, Date);

                iconChenabOtherBarrages.Attributes["class"] = "fa fa-chevron-up";
                divChenabOtherBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Ravi Barrages

        /// <summary>
        /// This function binds data to the Ravi river barrages grid.
        /// Created On 01-03-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindRaviBarrages(DataTable _dtRaviBarrages, DateTime _Date)
        {
            gvRaviBarrages.DataSource = _dtRaviBarrages;
            gvRaviBarrages.DataBind();

            ViewState[RaviBarrageName] = null;
            ViewState[RaviChannelName] = null;

            if (RaviBarragesAccordion)
            {
                spanRaviBarrages.Attributes["class"] = "badge badge-success";
                spanRaviBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanRaviBarrages.Attributes["class"] = "badge badge-important";
                spanRaviBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnRaviBarragesSave.Enabled = false;
                lbtnRaviBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnRaviBarragesSave.Visible = false;
                }
            }
            else
            {
                btnRaviBarragesSave.Enabled = true;
                lbtnRaviBarragesCancel.Enabled = true;
            }
        }

        protected void gvRaviBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        RaviBarragesAccordion = false;
                    }

                    if (ViewState[RaviBarrageName] == null)
                    {
                        ViewState[RaviBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[RaviChannelName] = lblChannelName.Text.Trim();

                        if (lblBarrageName.Text.Trim() == RaviRiver)
                        {
                            lblBarrageName.Text = String.Empty;
                        }
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[RaviBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[RaviChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[RaviChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[RaviBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[RaviChannelName] = lblChannelName.Text.Trim();

                            if (lblBarrageName.Text.Trim() == RaviRiver)
                            {
                                lblBarrageName.Text = String.Empty;
                            }
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvRaviBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnRaviBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvRaviBarrages.Rows)
                {
                    Label lblID = (Label)Row.FindControl("lblID");
                    Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                    TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                    Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string IndentString = txtIndent.Text.Trim();
                    string AFSQValue = lblAFSQ.Text.Trim();

                    long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                    CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                    if (mdlGaugeSlipDailyData != null)
                    {
                        mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                        mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                        bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                    else
                    {
                        mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                        mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                        mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                        if (AFSQValue != string.Empty)
                        {
                            mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                        }

                        if (GaugeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                        }

                        if (IndentString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                        }

                        if (DischargeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                        }

                        bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                }

                ResetAccordions();

                DataTable dtRaviBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, RaviBarragesIndex, LoadLatest);

                BindRaviBarrages(dtRaviBarrages, ReadingDate);

                iconRaviBarrages.Attributes["class"] = "fa fa-chevron-up";
                divRaviBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnRaviBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtRaviBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, RaviBarragesIndex);

                BindRaviBarrages(dtRaviBarrages, Date);

                iconRaviBarrages.Attributes["class"] = "fa fa-chevron-up";
                divRaviBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Sutlej Barrages

        /// <summary>
        /// This function binds data to the Sutlej river barrages grid.
        /// Created On 01-03-2016
        /// </summary>
        /// <param name="_DailyDataBLL"></param>
        /// <param name="_Date"></param>
        private void BindSutlejBarrages(DataTable _dtSutlejBarrages, DateTime _Date)
        {
            gvSutlejBarrages.DataSource = _dtSutlejBarrages;
            gvSutlejBarrages.DataBind();

            ViewState[SutlejBarrageName] = null;
            ViewState[SutlejChannelName] = null;

            if (SutlejBarragesAccordion)
            {
                spanSutlejBarrages.Attributes["class"] = "badge badge-success";
                spanSutlejBarrages.InnerHtml = "Complete";
            }
            else
            {
                spanSutlejBarrages.Attributes["class"] = "badge badge-important";
                spanSutlejBarrages.InnerHtml = "Incomplete";
            }

            bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

            if (!AllowEdit && (_Date.Date < DateTime.Now.Date || !base.CanEdit))
            {
                btnSutlejBarragesSave.Enabled = false;
                lbtnSutlejBarragesCancel.Enabled = false;

                if (!base.CanEdit)
                {
                    btnSutlejBarragesSave.Visible = false;
                }
            }
            else
            {
                btnSutlejBarragesSave.Enabled = true;
                lbtnSutlejBarragesCancel.Enabled = true;
            }
        }

        protected void gvSutlejBarrages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBarrageName = (Label)e.Row.FindControl("lblBarrageName");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblGaugeID = (Label)e.Row.FindControl("lblGaugeID");
                    Label lblIndent = (Label)e.Row.FindControl("lblIndent");
                    TextBox txtGauge = (TextBox)e.Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)e.Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)e.Row.FindControl("txtIndent");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();

                    long GaugeID = Convert.ToInt64(lblGaugeID.Text.Trim());

                    if (lblChannelName.Text != string.Empty)
                    {
                        lblIndent.Visible = false;
                        txtIndent.Visible = true;
                    }

                    if (GaugeString == string.Empty && DischargeString == string.Empty)
                    {
                        SutlejBarragesAccordion = false;
                    }

                    if (ViewState[SutlejBarrageName] == null)
                    {
                        ViewState[SutlejBarrageName] = lblBarrageName.Text.Trim();
                        ViewState[SutlejChannelName] = lblChannelName.Text.Trim();

                        if (lblBarrageName.Text.Trim() == SutlejRiver)
                        {
                            lblBarrageName.Text = String.Empty;
                        }
                    }
                    else
                    {
                        string ViewStateBarrageName = (string)ViewState[SutlejBarrageName];

                        if (lblBarrageName.Text.Trim() == ViewStateBarrageName)
                        {
                            lblBarrageName.Text = String.Empty;

                            string ViewStateChannelName = (string)ViewState[SutlejChannelName];

                            if (lblChannelName.Text.Trim() == ViewStateChannelName)
                            {
                                lblChannelName.Text = String.Empty;
                            }
                            else
                            {
                                ViewState[SutlejChannelName] = lblChannelName.Text.Trim();
                            }
                        }
                        else
                        {
                            ViewState[SutlejBarrageName] = lblBarrageName.Text.Trim();
                            ViewState[SutlejChannelName] = lblChannelName.Text.Trim();

                            if (lblBarrageName.Text.Trim() == SutlejRiver)
                            {
                                lblBarrageName.Text = String.Empty;
                            }
                        }
                    }

                    //if (GaugeID != 0)
                    //{
                    //    long GaugeCategoryID = Convert.ToInt64(gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[GaugeCategoryIDIndex].ToString());

                    //    if (GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                    //    {
                    //        lblIndent.Visible = false;
                    //        txtIndent.Visible = true;
                    //    }
                    //}

                    DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());
                    bool AllowEdit = Convert.ToBoolean(ViewState[AllowEditFlag]);

                    if (!AllowEdit && (SelectedDate.Date < DateTime.Now.Date || !base.CanEdit))
                    {
                        txtGauge.Enabled = false;
                        txtDischarge.Enabled = false;
                        txtIndent.Enabled = false;
                    }
                    else
                    {
                        txtGauge.Enabled = true;
                        txtDischarge.Enabled = true;
                        txtIndent.Enabled = true;

                        string EnableGaugeDischarge = String.Empty;
                        if (gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex] != null)
                        {
                            EnableGaugeDischarge = gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[EnableGaugeDischargeIndex].ToString();
                        }

                        if (EnableGaugeDischarge != String.Empty)
                        {
                            string EnableGauge = Convert.ToString(Constants.EnableGaugeDischarge.G);
                            string EnableDischarge = Convert.ToString(Constants.EnableGaugeDischarge.D);

                            if (EnableGaugeDischarge == EnableGauge)
                            {
                                txtDischarge.Enabled = false;
                            }
                            else if (EnableGaugeDischarge == EnableDischarge)
                            {
                                txtGauge.Enabled = false;
                            }
                            else
                            {
                                txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                            }
                        }
                        else
                        {
                            txtGauge.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGauge.ClientID + "','" + txtDischarge.ClientID + "');");
                        }

                        if (txtGauge.Enabled)
                        {
                            string MinValueGauge = String.Empty;
                            if (gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex] != null)
                            {
                                MinValueGauge = gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MinValueGaugeIndex].ToString();
                            }

                            string MaxValueGauge = String.Empty;
                            if (gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex] != null)
                            {
                                MaxValueGauge = gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueGaugeIndex].ToString();
                            }

                            if (MinValueGauge != string.Empty && MaxValueGauge != string.Empty)
                            {
                                txtGauge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueGauge + "','" + MaxValueGauge + "');");
                                txtGauge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueGauge, MaxValueGauge));
                            }
                        }

                        if (txtDischarge.Enabled)
                        {
                            string MinValueDischarge = String.Empty;
                            if (gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex] != null)
                            {
                                MinValueDischarge = gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MinValueDischargeIndex].ToString();
                            }

                            string MaxValueDischarge = String.Empty;
                            if (gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex] != null)
                            {
                                MaxValueDischarge = gvSutlejBarrages.DataKeys[e.Row.RowIndex].Values[MaxValueDischargeIndex].ToString();
                            }

                            if (MinValueDischarge != string.Empty && MaxValueDischarge != string.Empty)
                            {
                                txtDischarge.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValueDischarge + "','" + MaxValueDischarge + "');");
                                txtDischarge.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValueDischarge, MaxValueDischarge));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSutlejBarragesSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ReadingDate = Utility.GetParsedDate(txtDate.Text.Trim());

                DailyDataBLL bllDailyData = new DailyDataBLL();

                foreach (TableRow Row in gvSutlejBarrages.Rows)
                {
                    Label lblID = (Label)Row.FindControl("lblID");
                    Label lblSiteID = (Label)Row.FindControl("lblSiteID");
                    TextBox txtGauge = (TextBox)Row.FindControl("txtGauge");
                    TextBox txtDischarge = (TextBox)Row.FindControl("txtDischarge");
                    TextBox txtIndent = (TextBox)Row.FindControl("txtIndent");
                    Label lblAFSQ = (Label)Row.FindControl("lblAFSQ");

                    string GaugeString = txtGauge.Text.Trim();
                    string DischargeString = txtDischarge.Text.Trim();
                    string IndentString = txtIndent.Text.Trim();
                    string AFSQValue = lblAFSQ.Text.Trim();

                    long GaugeSlipSiteID = Convert.ToInt64(lblSiteID.Text.Trim());

                    CO_GaugeSlipDailyData mdlGaugeSlipDailyData = bllDailyData.GetGaugeSlipRecord(ReadingDate, GaugeSlipSiteID);

                    if (mdlGaugeSlipDailyData != null)
                    {
                        mdlGaugeSlipDailyData.AFSQ = (AFSQValue != string.Empty ? Convert.ToDouble(AFSQValue) : (double?)null);
                        mdlGaugeSlipDailyData.DailyGauge = (GaugeString != string.Empty ? Convert.ToDouble(GaugeString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyIndent = (IndentString != string.Empty ? Convert.ToDouble(IndentString) : (double?)null);
                        mdlGaugeSlipDailyData.DailyDischarge = (DischargeString != string.Empty ? Convert.ToDouble(DischargeString) : (double?)null);

                        bllDailyData.UpdateGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                    else
                    {
                        mdlGaugeSlipDailyData = new CO_GaugeSlipDailyData();

                        mdlGaugeSlipDailyData.GaugeSlipSiteID = GaugeSlipSiteID;
                        mdlGaugeSlipDailyData.ReadingDate = ReadingDate;

                        if (AFSQValue != string.Empty)
                        {
                            mdlGaugeSlipDailyData.AFSQ = Convert.ToDouble(AFSQValue);
                        }

                        if (GaugeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyGauge = Convert.ToDouble(GaugeString);
                        }

                        if (IndentString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyIndent = Convert.ToDouble(IndentString);
                        }

                        if (DischargeString != string.Empty)
                        {
                            mdlGaugeSlipDailyData.DailyDischarge = Convert.ToDouble(DischargeString);
                        }

                        bllDailyData.AddGaugeSlipDailyData(mdlGaugeSlipDailyData);
                    }
                }

                ResetAccordions();

                DataTable dtSutlejBarrages = new DailyDataBLL().GetGaugeSlipOtherData(ReadingDate, SutlejBarragesIndex, LoadLatest);

                BindSutlejBarrages(dtSutlejBarrages, ReadingDate);

                iconSutlejBarrages.Attributes["class"] = "fa fa-chevron-up";
                divSutlejBarrages.Attributes["style"] = "display: block;";

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnSutlejBarragesCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = Utility.GetParsedDate(txtDate.Text);

                DailyDataBLL bllDailyData = new DailyDataBLL();

                ResetAccordions();

                DataTable dtSutlejBarrages = new DailyDataBLL().GetGaugeSlipOtherData(Date, SutlejBarragesIndex);

                BindSutlejBarrages(dtSutlejBarrages, Date);

                iconSutlejBarrages.Attributes["class"] = "fa fa-chevron-up";
                divSutlejBarrages.Attributes["style"] = "display: block;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());

                if (SelectedDate.Date > DateTime.Now.Date)
                {
                    pnlSlipSites.Visible = false;

                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    pnlSlipSites.Visible = true;

                    BindGrids();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function calculates the discharge based on the Gauge ID and Gauge Value
        /// Created On 08-03-2016
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns>double</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static double? CalculateDischarge(long _GaugeID, double _GaugeValue)
        {
            double? Discharge = new DailyDataBLL().CalculateDischarge(_GaugeID, _GaugeValue);

            return Discharge;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime SelectedDate = Utility.GetParsedDate(txtDate.Text.Trim());

                if (SelectedDate.Date > DateTime.Now.Date)
                {
                    pnlSlipSites.Visible = false;

                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    ReportData mdlReportData = new ReportData();

                    ReportParameter ReportParameter = new ReportParameter("ReadingDate", SelectedDate.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    mdlReportData.Name = Constants.DailyGaugeSlip;

                    Session[SessionValues.ReportData] = mdlReportData;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btn_show_Click(object sender, EventArgs e)
        {
            //  List<CO_GaugeSlipDailyData> lstMissedRec=new List<CO_GaugeSlipDailyData>();

            // DailyDataBLL ddb = new DailyDataBLL(); 



            //    lstMissedRec = ddb.GetMissedGaugeSlipRecord(2013,5, 2);


            //    gv_missedgaugeslip.DataSource = lstMissedRec;
            //  gv_missedgaugeslip.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            { 

            List<CO_GaugeSlipDailyData> lstMissedRec = new List<CO_GaugeSlipDailyData>();

            DailyDataBLL ddb = new DailyDataBLL();

            //    BindMonthDropdown(); 


                //  string a = ddlyear.SelectedItem.Value;
                //  string b = ddlmonth.SelectedItem.Value;   


                int a = Convert.ToInt32(ddlyear.SelectedItem.Value);
                int b = Convert.ToInt32(ddlmonth.SelectedItem.Value);
                int c = Convert.ToInt32(ddlGaugeSite.SelectedItem.Value);   

                      lstMissedRec = ddb.GetMissedGaugeSlipRecord(a,b, c);

                gv_missedgaugeslip.DataSource = lstMissedRec;
            gv_missedgaugeslip.DataBind();

            }
            catch(Exception ex) { ex.Message.ToString() ; }


        }


        private void BindMonthDropdown()
        {
           // List<ListItem> lstMonths = new List<ListItem>();

            for (int Month = 1; Month <= 12; Month++)
            {
                DateTime FirstDay = Convert.ToDateTime(string.Format("{0}-{1}-{2}", Month, 1, DateTime.Now.Year));

                ddlmonth.Items.Add(new ListItem
                {
                    Text = FirstDay.ToString("MMMM"),
                    Value=Month.ToString()   
                    //Value = FirstDay.ToString("MMMM")
                });
            }

            DateTime Now = DateTime.Now;
            //  Dropdownlist.BindDropdownlist(ddlmonth , lstMonths, (int)Constants.DropDownFirstOption.Select, "Text", "Value");

            ddlmonth.SelectedValue = Now.Month.ToString();  
        }

        private void bindYearDropDown()
        {
            DateTime Now = DateTime.Now;
            for (int Year = Now.Year; Year > Now.Year - 15; Year--)
            {
                DateTime NextYear = new DateTime(Year + 1, 1, 1);

              //  string FinancialYear = string.Format("{0}-{1}", Year, NextYear.ToString("yy"));
                string FinancialYear =  NextYear.ToString("yyyy");


                ddlyear.Items.Add(new ListItem
                {
                    Text = FinancialYear,
                    Value = FinancialYear
                });
            }
            ddlyear.SelectedValue = Now.Year.ToString();    
        }
    }
}