using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class IBreachingSection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int floodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (floodInspectionID > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = floodInspectionID;
                        FloodInspectionDetail1.ShowInspectionStatusProp = false;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        hdnInspectionStatus.Value = (new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID)).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                        LoadIBreachingSection(floodInspectionID);
                    }
                    // hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                }
            }

            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadIBreachingSection(long _FloodInspectionID)
        {

            List<object> oIBreachingSection = new FloodInspectionsBLL().GetIBreachingSectionByInspectionID(_FloodInspectionID);

            gvIBreachingSection.DataSource = oIBreachingSection;
            gvIBreachingSection.DataBind();

            hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
        }


        protected void gvIBreachingSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditStoneStock = (Button)e.Row.FindControl("btnEditStoneStock");

                if (gvIBreachingSection.EditIndex == e.Row.RowIndex)
                {
                    #region "Data Keys"

                    DataKey key = gvIBreachingSection.DataKeys[e.Row.RowIndex];
                    long thisInfraBreachingSectionID = Convert.ToInt64(key.Values["InfraBreachingSectionID"]);
                    long IBreachingSectionID = Convert.ToInt64(key.Values["IBreachingSectionID"]);
                    string thisFromRD = Convert.ToString(key.Values["FromRD"]);
                    string thisToRD = Convert.ToString(key.Values["ToRD"]);
                    short thisAffectedRowsNo = Convert.ToInt16(key.Values["AffectedRowsNo"]);
                    short thisAffectedLinersNo = Convert.ToInt16(key.Values["AffectedLinersNo"]);
                    string thisRecommendedSolution = Convert.ToString(key.Values["RecommendedSolution"]);
                    long thisRestorationCost = Convert.ToInt64(key.Values["RestorationCost"]);

                    #endregion

                    #region "Controls"

                    TextBox txtAffectedRowsNo = (TextBox)e.Row.FindControl("txtAffectedRowsNo");
                    TextBox txtAffectedLinersNo = (TextBox)e.Row.FindControl("txtAffectedLinersNo");

                    TextBox txtRecommendedSolution = (TextBox)e.Row.FindControl("txtRecommendedSolution");
                    TextBox txtRestorationCost = (TextBox)e.Row.FindControl("txtRestorationCost");

                    #endregion

                    if (thisAffectedRowsNo != 0 && txtAffectedRowsNo != null)
                    {
                        txtAffectedRowsNo.Text = Convert.ToString(thisAffectedRowsNo);
                    }

                    if (thisAffectedLinersNo != 0 && txtAffectedLinersNo != null)
                    {
                        txtAffectedLinersNo.Text = Convert.ToString(thisAffectedLinersNo);
                    }

                    if (thisRecommendedSolution != null && txtRecommendedSolution != null)
                    {
                        txtRecommendedSolution.Text = Convert.ToString(thisRecommendedSolution);
                    }

                    if (thisRestorationCost != 0 && txtRestorationCost != null)
                    {
                        txtRestorationCost.Text = Convert.ToString(thisRestorationCost);
                    }
                }
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    btnEditStoneStock.Enabled = false;
                }
                if (_InspectionTypeID == 1)
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                    if (CanEdit)
                        btnEditStoneStock.Enabled = CanEdit;
                    else
                        btnEditStoneStock.Enabled = false;
                }
                else
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                    if (CanEdit)
                        btnEditStoneStock.Enabled = CanEdit;
                }
            }
        }

        protected void gvIBreachingSection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvIBreachingSection.EditIndex = e.NewEditIndex;
                LoadIBreachingSection(Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIBreachingSection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                #region "Data Keys"
                DataKey key = gvIBreachingSection.DataKeys[e.RowIndex];
                long thisInfraBreachingSectionID = Convert.ToInt64(key.Values["InfraBreachingSectionID"]);
                long IBreachingSectionID = Convert.ToInt64(key.Values["IBreachingSectionID"]);
                string thisFromRD = Convert.ToString(key.Values["FromRDTotal"]);
                string thisToRD = Convert.ToString(key.Values["ToRDTotal"]);
                short thisAffectedRowsNo = Convert.ToInt16(key.Values["AffectedRowsNo"]);
                short thisAffectedLinersNo = Convert.ToInt16(key.Values["AffectedLinersNo"]);
                string thisRecommendedSolution = Convert.ToString(key.Values["RecommendedSolution"]);
                long thisRestorationCost = Convert.ToInt64(key.Values["RestorationCost"]);
                long thisCreatedBy = Convert.ToInt64(key.Values["CreatedBy"]);
                DateTime thisCreatedDate = Convert.ToDateTime(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow Row = gvIBreachingSection.Rows[e.RowIndex];
                Label lblFromRD = (Label)Row.FindControl("lblFromRD");
                Label lblToRD = (Label)Row.FindControl("lblToRD");
                TextBox txtAffectedRowsNo = (TextBox)Row.FindControl("txtAffectedRowsNo");
                TextBox txtAffectedLinersNo = (TextBox)Row.FindControl("txtAffectedLinersNo");

                TextBox txtRecommendedSolution = (TextBox)Row.FindControl("txtRecommendedSolution");
                TextBox txtRestorationCost = (TextBox)Row.FindControl("txtRestorationCost");
                #endregion


                FO_IBreachingSection oIBreachingSection = new FO_IBreachingSection();

                if (IBreachingSectionID != 0)
                    oIBreachingSection.ID = Convert.ToInt64(IBreachingSectionID);

                oIBreachingSection.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);
                if (lblFromRD != null)
                {
                    oIBreachingSection.FromRD = Convert.ToInt32(thisFromRD);
                }

                if (lblToRD != null)
                {
                    oIBreachingSection.ToRD = Convert.ToInt32(thisToRD);
                }


                if (txtAffectedRowsNo != null && String.IsNullOrEmpty(txtAffectedRowsNo.Text) == false)
                    oIBreachingSection.AffectedRowsNo = Convert.ToInt16(txtAffectedRowsNo.Text);

                if (txtAffectedLinersNo != null && String.IsNullOrEmpty(txtAffectedLinersNo.Text) == false)
                    oIBreachingSection.AffectedLinersNo = Convert.ToInt16(txtAffectedLinersNo.Text);

                if (txtRecommendedSolution != null && String.IsNullOrEmpty(txtRecommendedSolution.Text) == false)
                    oIBreachingSection.RecommendedSolution = txtRecommendedSolution.Text;

                if (txtRestorationCost != null && String.IsNullOrEmpty(txtRestorationCost.Text) == false)
                    oIBreachingSection.RestorationCost = Convert.ToInt64(txtRestorationCost.Text);

                if (oIBreachingSection.ID == 0)
                {

                    oIBreachingSection.CreatedDate = DateTime.Now;
                    oIBreachingSection.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                else
                {
                    oIBreachingSection.CreatedDate = Convert.ToDateTime(thisCreatedDate);
                    oIBreachingSection.CreatedBy = Convert.ToInt32(thisCreatedBy);
                    oIBreachingSection.ModifiedDate = DateTime.Now;
                    oIBreachingSection.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                if (oIBreachingSection.FromRD >= oIBreachingSection.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }
                //object CheckRDs = new FloodInspectionsBLL().GetInspectionDivisionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                //if (CheckRDs != null)
                //{
                //    DataSet DS = new FloodOperationsBLL().FO_irrigationRDs(Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "DivisionID")),
                //        Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureID")),
                //        Convert.ToInt64(Utility.GetDynamicPropertyValue(CheckRDs, "StructureTypeID")));
                //    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                //    {
                //        DataRow DR = DS.Tables[0].Rows[0];
                //        Tuple<int, int> tupleRDs = Tuple.Create(Convert.ToInt32(DR["minsectionRd"].ToString()), Convert.ToInt32(DR["maxsectionTo"].ToString()));
                //        if (!((oIBreachingSection.FromRD >= Convert.ToInt64(tupleRDs.Item1) && oIBreachingSection.FromRD <= Convert.ToInt64(tupleRDs.Item2))
                //            && (oIBreachingSection.ToRD >= Convert.ToInt64(tupleRDs.Item1) && oIBreachingSection.ToRD <= Convert.ToInt64(tupleRDs.Item2))))
                //        {
                //            Master.ShowMessage("RDs are out of range", SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }
                //}

                bool isSaved = new FloodInspectionsBLL().SaveIBreachingSection(oIBreachingSection);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(IBreachingSectionID) == 0)
                        gvIBreachingSection.PageIndex = 0;


                    gvIBreachingSection.EditIndex = -1;
                    LoadIBreachingSection(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIBreachingSection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIBreachingSection.EditIndex = -1;
                LoadIBreachingSection(Convert.ToInt64(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

    }
}