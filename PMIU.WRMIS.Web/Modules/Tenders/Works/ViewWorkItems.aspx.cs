using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class ViewWorkItems : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                        {
                            hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                            long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                            hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        }
                        hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        string WorkSource = Convert.ToString(Request.QueryString["WorkSource"]);
                        BindWorkDetailData(Convert.ToInt64(hdnWorkSourceID.Value), WorkSource, Convert.ToInt64(hdnTenderWorkID.Value));
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindWorkDetailData(long _WorkSourceID, string _WorkSource, long _TenderWorkID)
        {
            try
            {
                if (_WorkSource == "CLOSURE")
                { }
                    dynamic mdlWorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                    Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(mdlWorkData, "TenderNotice");
                    Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(mdlWorkData, "WorkName");
                    Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(mdlWorkData, "WorkType");

                    List<dynamic> lstItemsData = new TenderManagementBLL().GetClosureWorkItemsByWorkID(_WorkSourceID, _WorkSource);
                    gvWorkItems.DataSource = lstItemsData;
                    gvWorkItems.DataBind();
                
                


            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}