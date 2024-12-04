using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class EntitlementDeliveryDirection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle();
            DateTime Now = DateTime.Now;
            if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) <= Now && new DateTime(Now.Year, Constants.KharifEntitlementEndMonth, Constants.KharifEntitlementEndDay) >= Now)
            {
                SetStrings(Convert.ToString(Constants.Seasons.Kharif));
            }
            else
            {
                SetStrings(Convert.ToString(Constants.Seasons.Rabi));
            }


        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        protected void btnGoTo_Click(object sender, EventArgs e)
        {
            if (rbEntitlement.Checked)
            {
                Response.Redirect("/Modules/EntitlementDelivery/Entitlement.aspx");
            }
            else
            {
                Response.Redirect("/Modules/EntitlementDelivery/EntitlementOnDeliveries.aspx");
            }

        }
        public void SetStrings(string Season)
        {
            EntitlementDeliveryBLL bllEntitlementDelivery = new EntitlementDeliveryBLL();
            if (Season == Convert.ToString(Constants.Seasons.Rabi))
            {
                DateTime Now = DateTime.Now;

                int Year = Now.Year;

                if (new DateTime(Now.Year, Constants.KharifEntitlementStartMonth, Constants.KharifEntitlementStartDay) > Now)
                {
                    Year = Year - 1;
                }

                dynamic mdlEditDataRabiIndus = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.Rabi, (long)Constants.Commands.IndusCommand);
                dynamic mdlEditDataRabiJC = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.Rabi, (long)Constants.Commands.JhelumChenabCommand);

                if (mdlEditDataRabiIndus != null)
                {
                    string SelectedAvgi = mdlEditDataRabiIndus.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataRabiIndus, null);
                    DateTime? Datei = mdlEditDataRabiIndus.GetType().GetProperty("Date").GetValue(mdlEditDataRabiIndus, null);
                    string UserNamei = mdlEditDataRabiIndus.GetType().GetProperty("UserName").GetValue(mdlEditDataRabiIndus, null);
                    string ESourcei = mdlEditDataRabiIndus.GetType().GetProperty("ESource").GetValue(mdlEditDataRabiIndus, null);
                    string ModifiedDatei = Utility.GetFormattedDate(Datei);
                    ED_ProvincialEntitlement mdlProvincialEntitlementri = bllEntitlementDelivery.GetProvincialEntitlement((long)Constants.Commands.IndusCommand, Year, (long)Constants.Seasons.Rabi, (long)Constants.PunjabProvinceID);
                    string RabiIndus = string.Format("{0} Entitlement for {1} {2}-{3} on {4} ({5}) basis using {8} had been generated on {6} by {7}",
                    "Indus", "Rabi", Year, Year + 1, SelectedAvgi, mdlProvincialEntitlementri.SP_PlanScenario.Scenario, ModifiedDatei, UserNamei, ESourcei);
                    lblEditIndus.Text = RabiIndus;
                }
                if (mdlEditDataRabiJC != null)
                {
                    string SelectedAvgj = mdlEditDataRabiJC.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataRabiJC, null);
                    DateTime? Datej = mdlEditDataRabiJC.GetType().GetProperty("Date").GetValue(mdlEditDataRabiJC, null);
                    string UserNamej = mdlEditDataRabiJC.GetType().GetProperty("UserName").GetValue(mdlEditDataRabiJC, null);
                    string ESourcej = mdlEditDataRabiJC.GetType().GetProperty("ESource").GetValue(mdlEditDataRabiJC, null);
                    string ModifiedDatej = Utility.GetFormattedDate(Datej);
                    ED_ProvincialEntitlement mdlProvincialEntitlementrj = bllEntitlementDelivery.GetProvincialEntitlement((long)Constants.Commands.JhelumChenabCommand, Year, (long)Constants.Seasons.Rabi, (long)Constants.PunjabProvinceID);
                    string RabiJc = string.Format("{0} Entitlement for {1} {2}-{3} on {4} ({5}) basis using {8} had been generated on {6} by {7}",
                   "JC", "Rabi", Year, Year + 1, SelectedAvgj, mdlProvincialEntitlementrj.SP_PlanScenario.Scenario, ModifiedDatej, UserNamej, ESourcej);
                    lblEditJC.Text = RabiJc;
                }
            }
            else if (Season == Convert.ToString(Constants.Seasons.Kharif))
            {
                int Year = DateTime.Now.Year;

                dynamic mdlEditDataEarlyKIndus = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.EarlyKharif, (long)Constants.Commands.IndusCommand);
                dynamic mdlEditDataEarlyKJC = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.EarlyKharif, (long)Constants.Commands.JhelumChenabCommand);
                string indus = "";
                string jc = "";
                if (mdlEditDataEarlyKIndus != null)
                {
                    string SelectedAvgeki = mdlEditDataEarlyKIndus.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataEarlyKIndus, null);
                    DateTime? Dateeki = mdlEditDataEarlyKIndus.GetType().GetProperty("Date").GetValue(mdlEditDataEarlyKIndus, null);
                    string UserNameeki = mdlEditDataEarlyKIndus.GetType().GetProperty("UserName").GetValue(mdlEditDataEarlyKIndus, null);
                    string ESourceeki = mdlEditDataEarlyKIndus.GetType().GetProperty("ESource").GetValue(mdlEditDataEarlyKIndus, null);
                    string ModifiedDateeki = Utility.GetFormattedDate(Dateeki);
                    dynamic mdlEditDataLateKIndus = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.LateKharif, (long)Constants.Commands.IndusCommand);

                    if (mdlEditDataLateKIndus != null)
                    {
                        string SelectedAvglki = mdlEditDataLateKIndus.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataLateKIndus, null);
                        DateTime? Datelki = mdlEditDataLateKIndus.GetType().GetProperty("Date").GetValue(mdlEditDataLateKIndus, null);
                        string UserNamelki = mdlEditDataLateKIndus.GetType().GetProperty("UserName").GetValue(mdlEditDataLateKIndus, null);
                        string ESourcelki = mdlEditDataLateKIndus.GetType().GetProperty("ESource").GetValue(mdlEditDataLateKIndus, null);
                        string ModifiedDatelki = Utility.GetFormattedDate(Datelki);
                        ED_ProvincialEntitlement mdlProvincialEntitlementki = bllEntitlementDelivery.GetProvincialEntitlement((long)Constants.Commands.IndusCommand, Year, (long)Constants.Seasons.Kharif, (long)Constants.PunjabProvinceID);
                        indus = string.Format("{0} Entitlement for {1} {2} on {3} basis and {4} {5} on {6} ({7}) basis using {10} had been generated on {8} by {9}",
                            "Indus", "Early Kharif", Year, SelectedAvgeki, "Late Kharif", Year, SelectedAvglki, mdlProvincialEntitlementki == null ? "" : mdlProvincialEntitlementki.SP_PlanScenario.Scenario,
                       ModifiedDateeki, UserNameeki, ESourcelki);
                        lblEditEK.Text = indus;
                    }

                }
                if (mdlEditDataEarlyKJC != null)
                {
                    string SelectedAvgekj = mdlEditDataEarlyKJC.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataEarlyKJC, null);
                    DateTime? Dateekj = mdlEditDataEarlyKJC.GetType().GetProperty("Date").GetValue(mdlEditDataEarlyKJC, null);
                    string UserNameekj = mdlEditDataEarlyKJC.GetType().GetProperty("UserName").GetValue(mdlEditDataEarlyKJC, null);
                    string ESourceeki = mdlEditDataEarlyKJC.GetType().GetProperty("ESource").GetValue(mdlEditDataEarlyKJC, null);
                    string ModifiedDateekj = Utility.GetFormattedDate(Dateekj);
                    dynamic mdlEditDataLateKJC = bllEntitlementDelivery.GetEditInformation(Year, (long)Constants.Seasons.LateKharif, (long)Constants.Commands.JhelumChenabCommand);
                    if (mdlEditDataLateKJC != null)
                    {
                        string SelectedAvglkj = mdlEditDataLateKJC.GetType().GetProperty("SelectedAvg").GetValue(mdlEditDataLateKJC, null);
                        DateTime? Datelkj = mdlEditDataLateKJC.GetType().GetProperty("Date").GetValue(mdlEditDataLateKJC, null);
                        string UserNamelkj = mdlEditDataLateKJC.GetType().GetProperty("UserName").GetValue(mdlEditDataLateKJC, null);
                        string ESourcelkj = mdlEditDataLateKJC.GetType().GetProperty("ESource").GetValue(mdlEditDataLateKJC, null);
                        string ModifiedDatelkj = Utility.GetFormattedDate(Datelkj);
                        ED_ProvincialEntitlement mdlProvincialEntitlementkj = bllEntitlementDelivery.GetProvincialEntitlement((long)Constants.Commands.JhelumChenabCommand, Year, (long)Constants.Seasons.Kharif, (long)Constants.PunjabProvinceID);
                        jc = string.Format("{0} Entitlement for {1} {2} on {3} basis and {4} {5} on {6} ({7}) basis using {10} had been generated on {8} by {9}",
                            "JC", "Early Kharif", Year, SelectedAvgekj, "Late Kharif", Year, SelectedAvglkj, mdlProvincialEntitlementkj == null ? "" : mdlProvincialEntitlementkj.SP_PlanScenario.Scenario,
                        ModifiedDatelkj, UserNamelkj, ESourcelkj);
                        lblEditLk.Text = jc;
                    }
                }
            }
        }
    }
}