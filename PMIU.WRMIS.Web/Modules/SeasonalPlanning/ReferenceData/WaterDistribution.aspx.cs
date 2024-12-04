using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class WaterDistribution : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindData();
                    SetTitle();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindData()
        {
            try
            {
                gvWaterDistribution.DataSource = new SeasonalPlanningBLL().GetWaterDistribution();
                gvWaterDistribution.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.WaterDistrubution);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvWaterDistribution_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvWaterDistribution.EditIndex = -1;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWaterDistribution_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvWaterDistribution.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}