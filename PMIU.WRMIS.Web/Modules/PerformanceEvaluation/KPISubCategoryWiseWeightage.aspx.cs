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
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class KPISubCategoryWiseWeightage : BasePage
    {
        double Total_TotalPoints = 0.0, Total_Weightage = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int ReportID = Utility.GetNumericValueFromQueryString("ReportID", 0);
                    int IrrigationBoundaryID = Utility.GetNumericValueFromQueryString("IrrigationBoundaryID", 0);
                    int CatID = Utility.GetNumericValueFromQueryString("CatID", 0);

                    if (ReportID > 0 && IrrigationBoundaryID > 0 && CatID > 0)
                    {
                        EvaluationScoresDetail1.ReportIDProp = ReportID;
                        EvaluationScoresDetail1.IrrigationBoundaryIDProp = IrrigationBoundaryID;
                        EvaluationScoresDetail1.CatIDProp = CatID;
                        BindSubCategoryWiseWeightageGrid(ReportID, IrrigationBoundaryID, CatID);
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
        /// This function binds data to the Sub Category Grid.
        /// Created On 03-01-2017
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_CatID"></param>
        private void BindSubCategoryWiseWeightageGrid(long _ReportID, long _IrrigationBoundaryID, long _CatID)
        {
            try
            {
                List<dynamic> lstCategoryWiseWeightage = new PerformanceEvaluationBLL().GetSubCategoryWeightage(_ReportID, _IrrigationBoundaryID, _CatID);

                if (lstCategoryWiseWeightage != null && lstCategoryWiseWeightage.Count > 0)
                {
                    gvKPISubCategoryWiseWeightage.DataSource = lstCategoryWiseWeightage;
                    gvKPISubCategoryWiseWeightage.DataBind();
                    gvKPISubCategoryWiseWeightage.Visible = true;
                }
                else
                {
                    Master.ShowMessage(Message.KPISubCategoryEmpty.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvKPISubCategoryWiseWeightage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvKPISubCategoryWiseWeightage.DataKeys[e.Row.RowIndex];
                    double ThisTotalPoints = Convert.ToDouble(key.Values["TotalPoints"]);
                    double ThisWeightage = Convert.ToDouble(key.Values["Weightage"]);

                    Total_TotalPoints = Total_TotalPoints + ThisTotalPoints;
                    Total_Weightage = Total_Weightage + ThisWeightage;
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotal_TotalPoints = (Label)e.Row.FindControl("Total_TotalPoints");
                    if (lblTotal_TotalPoints != null)
                        lblTotal_TotalPoints.Text = Total_TotalPoints.ToString();

                    Label lblTotal_Weightage = (Label)e.Row.FindControl("Total_Weightage");
                    if (lblTotal_Weightage != null)
                        lblTotal_Weightage.Text = Total_Weightage.ToString();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}