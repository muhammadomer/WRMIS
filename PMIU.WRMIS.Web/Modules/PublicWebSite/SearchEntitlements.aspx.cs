using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class SearchEntitlements : System.Web.UI.Page
    {
        #region Screen Constants

        public const int PercentageRoundDigit = 2;
        public const int MAFRoundDigit = 3;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindSeasonDropdown();
                    BindYearsDropdown();
                }
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds Season dropdown
        /// Created On 21-03-2017
        /// </summary>
        private void BindSeasonDropdown()
        {
            Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown(), (int)Constants.DropDownFirstOption.NoOption);
        }

        /// <summary>
        /// This function binds Year dropdown
        /// Created On 22-03-2017
        /// </summary>
        private void BindYearsDropdown()
        {
            int Season = Convert.ToInt32(ddlSeason.SelectedItem.Value);

            Dropdownlist.BindDropdownlist<List<dynamic>>(ddlYear, new EntitlementDeliveryBLL().GetDistinctApprovedYearsBySeason(Season), (int)Constants.DropDownFirstOption.NoOption);
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value == string.Empty)
                {
                    ddlYear.SelectedIndex = 0;
                    ddlYear.Enabled = false;
                }
                else
                {
                    BindYearsDropdown();
                    ddlYear.Enabled = true;
                }

                pnlContainer.Visible = false;
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindHeadingData();
                BindGrids();
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds data to the heading area
        /// Created On 27-03-2017
        /// </summary>
        private void BindHeadingData()
        {
            string Season = ddlSeason.SelectedItem.Text;
            string YearString = ddlYear.SelectedItem.Text;

            lblTitle.Text = string.Format("TENTATIVE DISTRIBUTION PROGRAMME {0} {1}", Season.ToUpper(), YearString);

            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
            int Year = Convert.ToInt32(ddlYear.SelectedItem.Value);

            List<long> lstSeasonIDs = null;

            if (SeasonID == 1)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
            }
            else if (SeasonID == 2)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
            }

            double Seasonal7782MAF = bllEntitlementDelivery.Get7782AverageSeasonalMAF(lstSeasonIDs);
            lbl7782Average.Text = Math.Round(Seasonal7782MAF, MAFRoundDigit).ToString();

            double SeasonalPara2MAF = bllEntitlementDelivery.GetPara2AverageSeasonalMAF(lstSeasonIDs);
            lblPara2.Text = Math.Round(SeasonalPara2MAF, MAFRoundDigit).ToString();

            double SeasonalEntitlementMAF = bllEntitlementDelivery.GetSeasonalEntitlement(Year, lstSeasonIDs);
            lblExpectedShare.Text = Math.Round(SeasonalEntitlementMAF, MAFRoundDigit).ToString();

            double PercentChange = ((SeasonalEntitlementMAF / Seasonal7782MAF) * 100) - 100;
            lblPercentChange.Text = Math.Round(PercentChange, PercentageRoundDigit).ToString();

            double TerbelaEntitlementMAF = bllEntitlementDelivery.GetSeasonalEntitlement(Year, lstSeasonIDs, (long)Constants.Commands.IndusCommand);
            lblTarbelaCommand.Text = Math.Round(TerbelaEntitlementMAF, MAFRoundDigit).ToString();

            double ManglaEntitlementMAF = bllEntitlementDelivery.GetSeasonalEntitlement(Year, lstSeasonIDs, (long)Constants.Commands.JhelumChenabCommand);
            lblManglaCommand.Text = Math.Round(ManglaEntitlementMAF, MAFRoundDigit).ToString();
        }

        /// <summary>
        /// This function binds data to the Grids
        /// Created on 22-03-2017
        /// </summary>
        private void BindGrids()
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();

            long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
            int Year = Convert.ToInt32(ddlYear.SelectedItem.Value);

            List<long> lstSeasonIDs = new List<long>();

            if (SeasonID == 1)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
            }
            else if (SeasonID == 2)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
            }

            List<dynamic> lstTerbelaEntitlements = bllEntitlementDelivery.GetCommandSeasonalEntitlement(Year, lstSeasonIDs, (long)Constants.Commands.IndusCommand);
            List<dynamic> lstManglaEntitlements = bllEntitlementDelivery.GetCommandSeasonalEntitlement(Year, lstSeasonIDs, (long)Constants.Commands.JhelumChenabCommand);

            gvTarbelaCommand.DataSource = lstTerbelaEntitlements;
            gvManglaCommand.DataSource = lstManglaEntitlements;

            gvTarbelaCommand.DataBind();
            gvManglaCommand.DataBind();

            pnlContainer.Visible = true;
        }

        protected void ddlCanalSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlContainer.Visible = false;
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlContainer.Visible = false;
            }
            catch (WRException exp)
            {
                new WRException(0, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}