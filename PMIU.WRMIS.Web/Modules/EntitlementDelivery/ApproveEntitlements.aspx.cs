using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.WaterLosses;
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

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class ApproveEntitlements : BasePage
    {
        List<double> lstAccumulative = new List<double>();
        double MAF = 0;
        double Cusecs = 0;
        List<double> lstEntitlement = new List<double>();
        List<double> lstDeliveriesCS = new List<double>();
        List<double> lstDifference = new List<double>();
        List<double> lstDeliveriesMAF = new List<double>();
        double AccumulativeDeliveries = 0;
        double BalanceEntitlement = 0;
        char ParentChild = 'P';
        string MaskedValue = "";

        EntitlementDeliveryBLL bllEntdel = new EntitlementDeliveryBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    GetCommandType();
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlSeason, CommonLists.GetSeasonDropDown(), (int)Constants.DropDownFirstOption.NoOption);
                    PageLoadData();
                }

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void PageLoadData()
        {
            //if ((DateTime.Now.Month >= 3 && DateTime.Now.Day >= 11) && (DateTime.Now.Month <= 4 && DateTime.Now.Day <= 30))
            //{
            //    btnApprove.Enabled = true;
            //}
            //else if ((DateTime.Now.Month >= 9 && DateTime.Now.Day >= 11) && (DateTime.Now.Month <= 10 && DateTime.Now.Day <= 31))
            //{
            //    btnApprove.Enabled = true;
            //}
            //else
            //{
            //    btnApprove.Enabled = false;
            //}

            //if ((DateTime.Now.Month >= 3 && DateTime.Now.Day >= 11) && (DateTime.Now.Month <= 9 && DateTime.Now.Day <= 10))
            //{
            //    btnApprove.Enabled = true;
            //}
            //else if ((DateTime.Now.Month >= 9 && DateTime.Now.Day >= 11) && (DateTime.Now.Month <= 3 && DateTime.Now.Day <= 10))
            //{
            //    btnApprove.Enabled = true;
            //}
            //else
            //{
            //    btnApprove.Enabled = false;
            //}

            ddlSeason.ClearSelection();
            ddlYear.ClearSelection();
            if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 9)
            {
                Dropdownlist.SetSelectedValue(ddlSeason, "2");
                ddlYear.Items.Add(Convert.ToString(DateTime.Today.Year));
                Dropdownlist.SetSelectedValue(ddlYear, DateTime.Today.Year.ToString());
            }
            else
            {
                Dropdownlist.SetSelectedValue(ddlSeason, "1");
                if (DateTime.Today.Month >= 10 && DateTime.Today.Month <= 12)
                {
                    ddlYear.Items.Add(new ListItem(Convert.ToString(DateTime.Today.Year) + "-" + Convert.ToString(DateTime.Today.Year + 1), Convert.ToString(DateTime.Today.Year)));
                    Dropdownlist.SetSelectedValue(ddlYear, DateTime.Today.Year.ToString());
                }
                else if (DateTime.Today.Month >= 1 && DateTime.Today.Month <= 3)
                {
                    ddlYear.Items.Add(new ListItem(Convert.ToString(DateTime.Today.Year - 1) + "-" + Convert.ToString(DateTime.Today.Year), Convert.ToString(DateTime.Today.Year - 1)));
                    Dropdownlist.SetSelectedValue(ddlYear, (DateTime.Today.Year - 1).ToString());
                }

            }
            //ddlAssetsCountryOthersone.Items.Add(new ListItem("Others", "Others"));
            //Dropdownlist.DDLYearsForEntitlement(ddlYear, Convert.ToInt64(ddlSeason.SelectedItem.Value));

            ddlSeason.Enabled = false;
            ddlYear.Enabled = false;


            EntitlementDeliveryBLL bllEntitlements = new EntitlementDeliveryBLL();
            long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
            long Year = Convert.ToInt64(ddlYear.SelectedItem.Value);

            List<long> lstSeasonIDs = null;

            if (SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
            }
            else
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
            }

            bool IsApproved = bllEntitlements.IsEntitlementApprove(Year, lstSeasonIDs);

            if (IsApproved)
            {
                // lnkbtnApprove.Enabled = false;
                lnkbtnApprove.Text = "Unapprove";
                lnkbtnApprove.ToolTip = "Unapprove";
                lnkbtnApprove.OnClientClick = "return confirm('Are you sure you want to Unapprove?');";
                //lnkbtnApprove.CssClass = "btn btn-primary disabled";
                //lnkbtnApprove.OnClientClick = "";
            }
            else
            {
                //lnkbtnApprove.Enabled = true;
                lnkbtnApprove.CssClass = "btn btn-primary";
                lnkbtnApprove.Text = "Approve";
                lnkbtnApprove.ToolTip = "Approve";
                lnkbtnApprove.OnClientClick = "return confirm('Are you sure you want to Approve?');";
            }
        }

        private void DisableDD(DropDownList DDL)
        {
            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("Select", ""));
            DDL.Enabled = false;
        }
        private void LoadMainCanal()
        {
            List<object> lstMainCanals = new WaterLossesBLL().GetMainCanals(0, false);
            Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstMainCanals);
            if (lstMainCanals != null && lstMainCanals.Count > 0)
                ddlMainCanal.Enabled = true;

            DisableDD(ddlBranch);
            DisableDD(ddlDistributry);
            DisableDD(ddlMinor);
            DisableDD(ddlSubMinor);
        }
        private void LoadAllChannels(long _UserID, long? _IrrigationLevel)
        {
            if (_IrrigationLevel == null)
                LoadMainCanal();

            int boundry = Convert.ToInt32(_IrrigationLevel);

            List<object> lstChildCanals = new List<object>();
            List<object> lstCanal = new WaterLossesBLL().GetCanalsByUserLocation(_UserID, boundry);

            lstChildCanals = (List<object>)lstCanal.ElementAt(0); //Main Canal 
            if (lstChildCanals != null && lstChildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                ddlMainCanal.Enabled = true;
            }

            lstChildCanals = (List<object>)lstCanal.ElementAt(1); //Branch Canal 
            if (lstChildCanals != null && lstChildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlBranch, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                ddlBranch.Enabled = true;
            }

            lstChildCanals = (List<object>)lstCanal.ElementAt(2); //Distributry Canal 
            if (lstChildCanals != null && lstChildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlDistributry, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                ddlDistributry.Enabled = true;
            }

            lstChildCanals = (List<object>)lstCanal.ElementAt(3); //MInor Canal 
            if (lstChildCanals != null && lstChildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                ddlMinor.Enabled = true;
            }

            lstChildCanals = (List<object>)lstCanal.ElementAt(4); //Sub Minor Canal 
            if (lstChildCanals != null && lstChildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlSubMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                ddlSubMinor.Enabled = true;
            }
        }

        protected void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                //int boundryID = Convert.ToInt32(SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID);
                long? boundryID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                long CommandID = ddlCommand.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCommand.SelectedItem.Value);
                //if (CommandID != -1)
                //{
                if (userID > 0 && boundryID != null)
                {
                    Dropdownlist.DDLGetCannalSystemByCommandTypeID(ddlCanalSystem, userID, true, boundryID, CommandID);
                }
                else
                {
                    Dropdownlist.DDLGetCannalSystemByCommandTypeID(ddlCanalSystem, userID, false, boundryID, CommandID);
                }
                ddlCanalSystem.Enabled = true;
                ddlCanalSystem.SelectedIndex = 0;
                //}
                //else
                //{
                //DisableDD(ddlCanalSystem);
                DisableDD(ddlMainCanal);
                DisableDD(ddlBranch);
                DisableDD(ddlDistributry);
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);
                //}

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCanalSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDropDowns(ddlCanalSystem.SelectedItem.Value.ToString(), 0);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMainCanal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDropDowns(ddlMainCanal.SelectedItem.Value.ToString(), 1);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDropDowns(ddlBranch.SelectedItem.Value.ToString(), 3);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlDistributry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDropDowns(ddlDistributry.SelectedItem.Value.ToString(), 4);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlMinor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDropDowns(ddlMinor.SelectedItem.Value.ToString(), 5);
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Dropdownlist.DDLYearsForEntitlement(ddlYear, Convert.ToInt64(ddlSeason.SelectedItem.Value));
                //ddlYear.DataSource = new EntitlementDeliveryBLL().GetDistinctYearsBySeason(Convert.ToInt64(ddlSeason.SelectedItem.Value));
                //ddlYear.DataBind();
                ddlYear.Enabled = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadDropDowns(string _strID, int _ChannelType)
        {
            string selectedId = _strID;
            if (string.IsNullOrEmpty(selectedId))
            {
                switch (_ChannelType)
                {
                    case 0: // Canal System
                        DisableDD(ddlMainCanal);
                        DisableDD(ddlBranch);
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;
                    case 1: // MAIN Canal
                        DisableDD(ddlBranch);
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 3: // Branch Canal 
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 4: //Distributory Canal 
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 5: //Minor Canal 
                        DisableDD(ddlSubMinor);
                        break;

                    default:
                        break;
                }
                return;
            }


            long ID = Convert.ToInt64(selectedId);
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

            List<object> _DataList = new List<object>();
            if (userID > 0 && boundryID != null)
            {
                _DataList = new WaterLossesBLL().GetCanalsByParentIDForED(userID, true, Convert.ToInt32(boundryID), ID, _ChannelType);
            }
            else
            {
                _DataList = new WaterLossesBLL().GetCanalsByParentIDForED(0, false, 0, ID, _ChannelType);
            }

            List<object> lstChildCanals = new List<object>();

            int MainIndex = -1, BIndex = -1, DIndex = -1, MIndex = -1, SMIndex = -1;

            switch (_ChannelType)
            {
                case 0: // Canal System
                    MainIndex = 0; BIndex = 1; DIndex = 2; MIndex = 3; SMIndex = 4;
                    ddlBranch.Enabled = false;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;
                    break;
                case 1: // MAIN Canal
                    MainIndex = -1; BIndex = 0; DIndex = 1; MIndex = 2; SMIndex = 3;
                    ddlBranch.Enabled = false;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;
                    break;

                case 3: // Branch Canal
                    MainIndex = -1; BIndex = -1; DIndex = 0; MIndex = 1; SMIndex = 2;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;
                    break;

                case 4: //Distributory Canal
                    MainIndex = -1; BIndex = -1; DIndex = -1; MIndex = 0; SMIndex = 1;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;
                    break;

                case 5: //Minor Canal
                    MainIndex = 0; BIndex = -1; DIndex = -1; MIndex = -1; SMIndex = 0;
                    ddlSubMinor.Enabled = false;
                    break;

                default:
                    break;
            }
            if (MainIndex != -1)
            {
                lstChildCanals = (List<object>)_DataList.ElementAt(MainIndex); //Main canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    ddlMainCanal.Enabled = true;
                }
            }

            if (BIndex != -1)
            {
                lstChildCanals = (List<object>)_DataList.ElementAt(BIndex); //Branch canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlBranch, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    ddlBranch.Enabled = true;
                }
            }

            if (DIndex != -1)
            {
                lstChildCanals = (List<object>)_DataList.ElementAt(DIndex); //Distributry Canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlDistributry, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    ddlDistributry.Enabled = true;
                }
            }

            if (MIndex != -1)
            {
                lstChildCanals = (List<object>)_DataList.ElementAt(MIndex); //Minor Canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    ddlMinor.Enabled = true;
                }
            }

            if (SMIndex != -1)
            {
                lstChildCanals = (List<object>)_DataList.ElementAt(SMIndex); //Sub Minor Canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlSubMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    ddlSubMinor.Enabled = true;
                }
            }

            switch (_ChannelType)
            {
                case 0: // Canal System
                    ddlMainCanal.SelectedValue = "";
                    ddlBranch.SelectedValue = "";
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 1: // MAIN Canal
                    ddlBranch.SelectedValue = "";
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 3: // Branch Canal
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 4: //Distributory Canal 
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 5: //Minor Canal 
                    ddlSubMinor.SelectedValue = "";
                    break;

                default:
                    break;
            }

            gvEntitlements.Visible = false;
            HeaderDiv.Visible = false;
        }

        private void GetCommandType()
        {
            Dropdownlist.DDLCommandNames(ddlCommand);
            ddlCommand.Enabled = true;
        }

        private long SelectedChannelID()
        {
            if (ddlSubMinor.Enabled && !ddlSubMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlSubMinor.SelectedValue);
            }
            else if (ddlMinor.Enabled && !ddlMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMinor.SelectedValue);
            }
            else if (ddlDistributry.Enabled && !ddlDistributry.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlDistributry.SelectedValue);
            }
            else if (ddlBranch.Enabled && !ddlBranch.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlBranch.SelectedValue);
            }
            else if (ddlMainCanal.Enabled && !ddlMainCanal.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMainCanal.SelectedValue);
            }
            return 0;
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }



        /*********************************************************************************************************************/
        protected void gvEntitlements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTenDailyID = (Label)e.Row.FindControl("lblTenDailyID");
                    Label lblEntitlementCs = (Label)e.Row.FindControl("lblEntitlementCs");
                    Label lblDeliveriesCs = (Label)e.Row.FindControl("lblDeliveriesCs");
                    Label lblDifferenceCs = (Label)e.Row.FindControl("lblDifferenceCs");
                    Label lblDeliveriesMAF = (Label)e.Row.FindControl("lblDeliveriesMAF");
                    Label lblAccumulativeDeliveriesMAF = (Label)e.Row.FindControl("lblAccumulativeDeliveriesMAF");
                    Label lblBalanceEntitlementMAF = (Label)e.Row.FindControl("lblBalanceEntitlementMAF");                    

                    if (lblDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblDeliveriesMAF.Text);
                        MAF = Math.Round(MAF, 3);

                        lstAccumulative.Add(MAF);

                        lblAccumulativeDeliveriesMAF.Text = Convert.ToString(lstAccumulative.Sum());

                        if (e.Row.RowIndex == 0)
                        {
                            if (lblEntitlement.Text != string.Empty)
                                lblBalanceEntitlementMAF.Text = Convert.ToString(Convert.ToDouble(lblEntitlement.Text) - Convert.ToDouble(lblDeliveriesMAF.Text));
                        }
                        else
                        {
                            double MAFofChannel = Convert.ToDouble(((Label)gvEntitlements.Rows[gvEntitlements.Rows.Count - 1].Cells[7].Controls[1]).Text);
                            lblBalanceEntitlementMAF.Text = (MAFofChannel - Convert.ToDouble(lblDeliveriesMAF.Text)).ToString();
                        }
                    }

                    if (lblEntitlementCs.Text != string.Empty)
                    {
                        Cusecs = Convert.ToDouble(lblEntitlementCs.Text);

                        if (ParentChild == 'P')
                        {
                            Cusecs = Math.Round(Cusecs / 100) * 100;
                        }
                        else
                        {
                            Cusecs = Math.Round(Cusecs);
                        }

                        //MAF = Math.Round(MAF / 100) * 100;
                        lblEntitlementCs.Text = Convert.ToString(Cusecs);

                        //MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Convert.ToDouble(lblEntitlementCs.Text), ParentChild);
                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[0]);
                        MAF = Math.Round(MAF, 3);

                        lstEntitlement.Add(MAF);
                    }

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        Cusecs = Convert.ToDouble(lblDeliveriesCs.Text);

                        if (ParentChild == 'P')
                        {
                            Cusecs = Math.Round(Cusecs / 100) * 100;
                        }
                        else
                        {
                            Cusecs = Math.Round(Cusecs);
                        }

                        lblDeliveriesCs.Text = Convert.ToString(Cusecs);
                        //MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Convert.ToDouble(lblDeliveriesCs.Text), ParentChild);
                        MAF = Convert.ToDouble(gvEntitlements.DataKeys[e.Row.RowIndex].Values[1]);
                        MAF = Math.Round(MAF, 3);

                        lstDeliveriesCS.Add(MAF);
                    }

                    if (lblDeliveriesCs.Text != string.Empty)
                    {
                        double Difference = Convert.ToDouble(lblDeliveriesCs.Text) - Convert.ToDouble(lblEntitlementCs.Text);
                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        if (ParentChild == 'P')
                        {
                            Difference = Math.Round(Difference / 100) * 100;
                        }
                        else
                        {
                            Difference = Math.Round(Difference);
                        }

                        lblDifferenceCs.Text = Convert.ToString(Difference);

                        MAF = bllEntdel.GetMAFByCusecs(Convert.ToInt64(lblTenDailyID.Text), Difference, ParentChild);
                        MAF = Math.Round(MAF, 3);

                        lstDifference.Add(MAF);
                    }

                    if (lblDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblDeliveriesMAF.Text);

                        if (ParentChild == 'C')
                        {
                            MAF = MAF * 1000;
                        }

                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblDeliveriesMAF.Text = MaskedValue;

                        lstDeliveriesMAF.Add(MAF);
                    }

                    if (lblAccumulativeDeliveriesMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblAccumulativeDeliveriesMAF.Text);
                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblAccumulativeDeliveriesMAF.Text = MaskedValue;

                        AccumulativeDeliveries = MAF;
                    }

                    if (lblBalanceEntitlementMAF.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblBalanceEntitlementMAF.Text);
                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblBalanceEntitlementMAF.Text = MaskedValue;

                        BalanceEntitlement = MAF;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label ftrEntitlement = (Label)e.Row.FindControl("ftrEntitlement");
                    Label ftrDeliveriesCS = (Label)e.Row.FindControl("ftrDeliveriesCS");
                    Label ftrDifference = (Label)e.Row.FindControl("ftrDifference");
                    Label ftrDeliveriesMAF = (Label)e.Row.FindControl("ftrDeliveriesMAF");
                    Label ftrAccumulativeDeliveries = (Label)e.Row.FindControl("ftrAccumulativeDeliveries");
                    Label ftrBalanceEntitlement = (Label)e.Row.FindControl("ftrBalanceEntitlement");

                    //ftrEntitlement.Text = Convert.ToString(lstEntitlement.Sum());
                    MAF = lstEntitlement.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrEntitlement.Text = MaskedValue;

                    //ftrDeliveriesCS.Text = Convert.ToString(lstDeliveriesCS.Sum());
                    MAF = lstDeliveriesCS.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesCS.Text = MaskedValue;

                    //ftrDifference.Text = Convert.ToString(lstDifference.Sum());
                    MAF = lstDifference.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDifference.Text = MaskedValue;

                    //ftrDeliveriesMAF.Text = Convert.ToString(lstDeliveriesMAF.Sum());
                    MAF = lstDeliveriesMAF.Sum();
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrDeliveriesMAF.Text = MaskedValue;

                    //ftrAccumulativeDeliveries.Text = AccumulativeDeliveries.ToString();
                    MAF = AccumulativeDeliveries;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrAccumulativeDeliveries.Text = MaskedValue;

                    //ftrBalanceEntitlement.Text = BalanceEntitlement.ToString();
                    MAF = BalanceEntitlement;
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    ftrBalanceEntitlement.Text = MaskedValue;
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ParentChild == 'C')
                    {
                        e.Row.Cells[5].Text = "Deliveries (1000 AF)";
                        e.Row.Cells[6].Text = "Accumulative Deliveries (1000AF)";
                        e.Row.Cells[7].Text = "Balance Entitlement (1000 AF)";
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();
            List<dynamic> lstEntitlements = null;
            long ChannelID = SelectedChannelID();
            long SeasonID = ddlSeason.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSeason.SelectedItem.Value);
            long Year = ddlYear.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlYear.SelectedItem.Value);

            if (SeasonID == (long)Constants.Seasons.Rabi)
            {
                long IncrementedYEarYear = Year + 1;

                if (ChannelID == 0)
                {
                    ChannelID = ddlCanalSystem.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCanalSystem.SelectedItem.Value);

                    ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);

                    lblEntitlementText.Text = "Entitlement for Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF):";
                    lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + mdlCommandChannel.ChannelName + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (MAF)";

                    if (mdlCommandChannel != null)
                    {
                        lblDesignDischarge.Text = Convert.ToString(mdlCommandChannel.DesignDischargeRabi);
                    }
                    else
                    {
                        lblDesignDischarge.Text = "0";
                    }

                    MAF = bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year));

                    List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
                    double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                    Average7782 = Math.Round(Average7782, 3);
                    MaskedValue = String.Format("{0:0.000}", Average7782);
                    lbl7782Average.Text = MaskedValue;

                    double PercentChange = ((Convert.ToDouble(lblEntitlement.Text) / Average7782) * 100) - 100;
                    lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();

                    //if (ddlCommand.SelectedItem.Value == "1")
                    //{
                    //    double Para2 = bllEntitlementDelivery.GetPara2ChannelAverageMAF(lstSeasonIDs, chnl.ID);
                    //    lblPara2.Text = (Math.Round(Para2, 3)).ToString();
                    //    lblPara2Text.Text = "Punjab Para(2) Rabi Share (MAF):";
                    //    lblPara2.Visible = true;
                    //    lblPara2Text.Visible = true;
                    //}
                    //else
                    //{
                    //    lblPara2Text.Visible = false;
                    //    lblPara2.Visible = false;
                    //}

                    lstEntitlements = bllEntitlementDelivery.ViewEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);

                    lbl7782AverageText.Visible = true;
                    lbl7782Average.Visible = true;
                    lblPercentChangeText.Visible = true;
                    lblPercentChange.Visible = true;
                    lblMainDesc.Visible = true;

                }
                else
                {
                    CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                    lblEntitlementText.Text = "Entitlement for Rabi" + " " + Year + "-" + IncrementedYEarYear + " (1000 AF):";
                    lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + chnl.NAME + " Rabi" + " " + Year + "-" + IncrementedYEarYear + " (1000 AF)";
                    CO_ChannelGauge mdlChannelGauge = bllEntitlementDelivery.GetChannelGaugeByChannelID(chnl.ID);
                    lblDesignDischarge.Text = Convert.ToString(mdlChannelGauge.DesignDischarge);


                    MAF = bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);

                    lblEntitlement.Text = MaskedValue; //Convert.ToString(bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year));

                    lstEntitlements = bllEntitlementDelivery.ViewChildEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);
                    lbl7782AverageText.Visible = false;
                    lbl7782Average.Visible = false;
                    lblPercentChangeText.Visible = false;
                    lblPercentChange.Visible = false;
                    //lblMainDesc.Visible = false;
                    //lblPara2.Visible = false;
                    //lblPara2Text.Visible = false;
                }

            }
            else if (SeasonID == (long)Constants.Seasons.Kharif)
            {
                lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (MAF):";

                if (ChannelID == 0)
                {
                    ChannelID = ddlCanalSystem.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCanalSystem.SelectedItem.Value);

                    ED_CommandChannel mdlCommandChannel = bllEntitlementDelivery.GetCommandChannelByID(ChannelID);

                    lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + mdlCommandChannel.ChannelName + " Kharif" + " " + Year + " (MAF)";
                    lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (MAF):";
                    
                    if (mdlCommandChannel != null)
                    {
                        lblDesignDischarge.Text = Convert.ToString(mdlCommandChannel.DesignDischarge);
                    }
                    else
                    {
                        lblDesignDischarge.Text = "0";
                    }

                    MAF = bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);
                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFEntitlementBySearchCriteria(ChannelID, SeasonID, Year));
                    
                    List<long> lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
                    double Average7782 = bllEntitlementDelivery.Get7782ChannelAverageMAF(lstSeasonIDs, ChannelID);
                    Average7782 = Math.Round(Average7782, 3);
                    MaskedValue = String.Format("{0:0.000}", Average7782);
                    lbl7782Average.Text = MaskedValue;
                    
                    double PercentChange = ((Convert.ToDouble(lblEntitlement.Text) / Average7782) * 100) - 100;
                    lblPercentChange.Text = (Math.Round(PercentChange, 3)).ToString();

                    //if (ddlCommand.SelectedItem.Value == "1")
                    //{
                    //    double Para2 = bllEntitlementDelivery.GetPara2ChannelAverageMAF(lstSeasonIDs, chnl.ID);
                    //    lblPara2.Text = (Math.Round(Para2, 3)).ToString();
                    //    lblPara2Text.Text = "Punjab Para(2) Kharif Share (MAF):";
                    //    lblPara2.Visible = true;
                    //    lblPara2Text.Visible = true;
                    //}
                    //else
                    //{
                    //    lblPara2.Visible = false;
                    //    lblPara2Text.Visible = false;
                    //}

                    lstEntitlements = bllEntitlementDelivery.ViewEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);

                    lbl7782AverageText.Visible = true;
                    lbl7782Average.Visible = true;
                    lblPercentChangeText.Visible = true;
                    lblPercentChange.Visible = true;
                    lblMainDesc.Visible = true;
                }
                else
                {
                    CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);

                    lblMainDesc.Text = "Entitlement and Actual Distribution of" + " " + chnl.NAME + " Kharif" + " " + Year + " (1000 AF)";
                    lblEntitlementText.Text = "Entitlement for Kharif" + " " + Year + " (1000 AF):";
                    CO_ChannelGauge mdlChannelGauge = bllEntitlementDelivery.GetChannelGaugeByChannelID(chnl.ID);
                    lblDesignDischarge.Text = Convert.ToString(mdlChannelGauge.DesignDischarge);


                    MAF = bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year);
                    MAF = Math.Round(MAF, 3);
                    MaskedValue = String.Format("{0:0.000}", MAF);

                    lblEntitlement.Text = MaskedValue;//Convert.ToString(bllEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(ChannelID, SeasonID, Year));

                    lstEntitlements = bllEntitlementDelivery.ViewChildEntitlementsBySearchCriteria(ChannelID, SeasonID, Year);
                    lbl7782AverageText.Visible = false;
                    lbl7782Average.Visible = false;
                    lblPercentChangeText.Visible = false;
                    lblPercentChange.Visible = false;
                    //lblMainDesc.Visible = false;
                    //lblPara2.Visible = false;
                    //lblPara2Text.Visible = false;
                }

            }

            gvEntitlements.DataSource = lstEntitlements;
            gvEntitlements.DataBind();
            gvEntitlements.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedChannelID() == 0)
                {
                    ParentChild = 'P';
                }
                else
                {
                    ParentChild = 'C';
                }

                BindGrid();
                HeaderDiv.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lnkbtnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();
                long Season = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                long Year = Convert.ToInt64(ddlYear.SelectedItem.Value);
                List<ED_SeasonalEntitlement> lstSeasonalEntitlement = bllEntitlementDelivery.GetAllCommandChannelsByYearAndSeason(Year, Season);
                string Text = lnkbtnApprove.Text;

                for (int i = 0; i < lstSeasonalEntitlement.Count; i++)
                {
                    ED_SeasonalEntitlement mdlSeasonalEntitlement = lstSeasonalEntitlement[i];
                    if (Text == "Approve")
                    {
                        mdlSeasonalEntitlement.IsApproved = true;
                        lnkbtnApprove.Text = "Unapprove";
                        lnkbtnApprove.ToolTip = "Unapprove";
                        lnkbtnApprove.OnClientClick = "return confirm('Are you sure you want to Unapprove?');";
                    }
                    else
                    {
                        mdlSeasonalEntitlement.IsApproved = false;
                        lnkbtnApprove.Text = "Approve";
                        lnkbtnApprove.ToolTip = "Approve";
                        lnkbtnApprove.OnClientClick = "return confirm('Are you sure you want to Approve?');";
                    }

                    bllEntitlementDelivery.UpdateSeasonalEntitlement(mdlSeasonalEntitlement);
                }

                lnkbtnApprove.CssClass = "btn btn-primary";
                //lnkbtnApprove.Enabled = false;
                //lnkbtnApprove.CssClass = "btn btn-primary disabled";
                //lnkbtnApprove.OnClientClick = "";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /*********************************************************************************************************************/
    }
}