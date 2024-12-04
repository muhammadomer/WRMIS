using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation.Controls
{
    public partial class EvaluationScoresDetail : System.Web.UI.UserControl
    {
        private long ReportID = 0;
        private long IrrigationBoundaryID = 0;
        private long CatID = 0;

        public long ReportIDProp { get { return ReportID; } set { ReportID = value; } }
        public long IrrigationBoundaryIDProp { get { return IrrigationBoundaryID; } set { IrrigationBoundaryID = value; } }
        public long CatIDProp { get { return CatID; } set { CatID = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEvaluationScoresDetail(ReportID, IrrigationBoundaryID, CatID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        /// <summary>
        /// This function loads the details baseed on the parameters passed to it.
        /// Created By 03-01-2017
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_CatID"></param>
        private void LoadEvaluationScoresDetail(long _ReportID, long _IrrigationBoundaryID, long _CatID)
        {
            try
            {
                dynamic EvaluationScoresDetail = new PerformanceEvaluationBLL().GetEvaluationScoresDetail(_ReportID, _IrrigationBoundaryID, _CatID);
                lblPerformanceEvaluationLevel.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("PerformanceEvaluationLevel").GetValue(EvaluationScoresDetail));
                lblFromDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(EvaluationScoresDetail.GetType().GetProperty("FromDate").GetValue(EvaluationScoresDetail)));
                lblToDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(EvaluationScoresDetail.GetType().GetProperty("ToDate").GetValue(EvaluationScoresDetail)));
                lblSession.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("Session").GetValue(EvaluationScoresDetail));
                lblAnalyzedGauges.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("AnalyzedGauges").GetValue(EvaluationScoresDetail));
                lblClosedGauges.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("ClosedGauges").GetValue(EvaluationScoresDetail));
                lblAggregatedGauges.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("AggregatedGauges").GetValue(EvaluationScoresDetail));
                lblCheckedbyPMIU.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("PMIUTotalChannels").GetValue(EvaluationScoresDetail));
                lblTotalPointsobtained.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("ObtainedPointsTotal").GetValue(EvaluationScoresDetail));
                lblEvaluationScoresDetailPath.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("EvaluationScoresDetailPath").GetValue(EvaluationScoresDetail));

                if (EvaluationScoresDetail.GetType().GetProperty("CatName") != null)
                {
                    lblKPICategoryName.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("CatName").GetValue(EvaluationScoresDetail));
                    lblWeightageinKPICategory.Text = Convert.ToString(EvaluationScoresDetail.GetType().GetProperty("WeightageInKPICategory").GetValue(EvaluationScoresDetail));
                    dvKPICategory.Visible = true;
                }

                ReportIDProp = _ReportID;
                IrrigationBoundaryIDProp = _IrrigationBoundaryID;
                CatIDProp = _CatID;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}