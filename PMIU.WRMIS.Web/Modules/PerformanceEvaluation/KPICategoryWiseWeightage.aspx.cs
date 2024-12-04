using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.PerformanceEvaluation.Controls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.PerformanceEvaluation;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class KPICategoryWiseWeightage : BasePage
    {

        int ReportID = 0,
            IrrigationBoundaryID = 0;

        double Total_FDTotalPoints = 0.0, Total_FDWeightage = 0.0,
            Total_PMIUTotalPoints = 0.0, Total_PMIUWeightage = 0.0,
            Total_TotalWeightage = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    ReportID = Utility.GetNumericValueFromQueryString("ReportID", 0);
                    IrrigationBoundaryID = Utility.GetNumericValueFromQueryString("IrrigationBoundaryID", 0);

                    if (ReportID > 0 && IrrigationBoundaryID > 0)
                    {
                        EvaluationScoresDetail.ReportIDProp = ReportID;
                        EvaluationScoresDetail.IrrigationBoundaryIDProp = IrrigationBoundaryID;
                        BindCategoryWiseWeightageGrid(ReportID, IrrigationBoundaryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 03-01-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds data to the Category Grid.
        /// Created On 03-01-2017
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        private void BindCategoryWiseWeightageGrid(long _ReportID, long _IrrigationBoundaryID)
        {
            try
            {
                List<dynamic> lstCategoryWiseWeightage = new PerformanceEvaluationBLL().GetCategoryWeightage(_ReportID, _IrrigationBoundaryID);

                if (lstCategoryWiseWeightage != null && lstCategoryWiseWeightage.Count > 0)
                {
                    gvKPICategoryWiseWeightage.DataSource = lstCategoryWiseWeightage;
                    gvKPICategoryWiseWeightage.DataBind();
                    gvKPICategoryWiseWeightage.Visible = true;

                }
                else
                {
                    Master.ShowMessage(Message.KPICategoryEmpty.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvKPICategoryWiseWeightage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DataKey Key = gvKPICategoryWiseWeightage.DataKeys[e.Row.RowIndex];
                    long ThisCatID = Convert.ToInt64(Key.Values["CatID"]);
                    double ThisFDTotalPoints = Convert.ToDouble(Key.Values["FDTotalPoints"]);
                    double ThisFDWeightage = Convert.ToDouble(Key.Values["FDWeightage"]);
                    double ThisPMIUTotalPoints = Convert.ToDouble(Key.Values["PMIUTotalPoints"]);
                    double ThisPMIUWeightage = Convert.ToDouble(Key.Values["PMIUWeightage"]);
                    double ThisTotalWeightage = Convert.ToDouble(Key.Values["TotalWeightage"]);


                    Total_FDTotalPoints = Total_FDTotalPoints + ThisFDTotalPoints;
                    Total_FDWeightage = Total_FDWeightage + ThisFDWeightage;
                    Total_PMIUTotalPoints = Total_PMIUTotalPoints + ThisPMIUTotalPoints;
                    Total_PMIUWeightage = Total_PMIUWeightage + ThisPMIUWeightage;
                    Total_TotalWeightage = Total_TotalWeightage + ThisTotalWeightage;

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPISubCategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}&CatID={2}", ReportID.ToString(), IrrigationBoundaryID.ToString(), ThisCatID.ToString());
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotal_FDTotalPoints = (Label)e.Row.FindControl("Total_FDTotalPoints");
                    if (lblTotal_FDTotalPoints != null)
                        lblTotal_FDTotalPoints.Text = Total_FDTotalPoints.ToString();

                    Label lblTotal_FDWeightage = (Label)e.Row.FindControl("Total_FieldDataWeightage");
                    if (lblTotal_FDWeightage != null)
                        lblTotal_FDWeightage.Text = Total_FDWeightage.ToString();

                    Label lblTotal_PMIUTotalPoints = (Label)e.Row.FindControl("Total_PMIUTotalPoints");
                    if (lblTotal_PMIUTotalPoints != null)
                        lblTotal_PMIUTotalPoints.Text = Total_PMIUTotalPoints.ToString();

                    Label lblTotal_PMIUWeightage = (Label)e.Row.FindControl("Total_PMIUWeightage");
                    if (lblTotal_PMIUWeightage != null)
                        lblTotal_PMIUWeightage.Text = Total_PMIUWeightage.ToString();

                    Label lblTotal_TotalWeightage = (Label)e.Row.FindControl("Total_TotalWeightage");
                    if (lblTotal_TotalWeightage != null)
                        lblTotal_TotalWeightage.Text = Total_TotalWeightage.ToString();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}