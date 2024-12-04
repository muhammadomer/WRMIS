using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class SearchBreachIncident : BasePage
    {
        #region Hash Table Keys

        public const string CaseIDKey = "BreachCaseID";
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string PageIndexKey = "PageIndex";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    //if (!string.IsNullOrEmpty(Request.QueryString["RP"]))
                    //{
                    //    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    //}
                    if (!string.IsNullOrEmpty(Request.QueryString["RP"]))
                    {
                        int PageID = Convert.ToInt32(Request.QueryString["RP"]);

                        if (PageID == 1)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            DateTime CurrentDate = DateTime.Now;
                            txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                            txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-1)));
                        }
                         else if (PageID == 2)
	                    {
                            if (Session[SessionValues.SearchBreachCriteria] != null)
                            {
                                BindHistoryData();
                            }
                            else
                            {
                                DateTime CurrentDate = DateTime.Now;
                                txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                                txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-1)));
                            }
	                    }
                       
                    }
                    else
                    {
                        DateTime CurrentDate = DateTime.Now;
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-1)));
                    }
                    SetTitle();
                    //btnAddNewIncident.Visible = base.CanAdd;
             }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchBreachIncident);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnBreachSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindBreachSearchGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindBreachSearchGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();
                String CaseID = string.Empty;
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (txtCaseID.Text != String.Empty)
                {
                    CaseID = txtCaseID.Text;
                    SearchCriteria.Add(CaseIDKey, CaseID);
                }

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }
                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());
                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }
                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }
                // List<object> lstBreachSearch = new ChannelBLL().GetChannelsBySearchCriteria(_CaseID
                //, _FromDate
                //, _ToDate
                //    );
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long UserId = mdlUser.ID;
                long IrrigationLevelID = 0;
                if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                    IrrigationLevelID = 0;
                else
                 IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;

                DataTable lstBreachSearch = new WaterTheftBLL().GetBreachBySearchCriteria(CaseID, FromDate, ToDate, UserId, IrrigationLevelID);
                //  List<object> lstBreachSearch = null;
                gvSearchBreachResult.DataSource = lstBreachSearch;
                gvSearchBreachResult.DataBind();

                SearchCriteria.Add(PageIndexKey, gvSearchBreachResult.PageIndex);

                Session[SessionValues.SearchBreachCriteria] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvSearchBreachResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchBreachResult.PageIndex = e.NewPageIndex;
                gvSearchBreachResult.EditIndex = -1;
                BindBreachSearchGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnAddNewIncident_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AddBreachIncident.aspx", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchBreachCriteria];

            txtCaseID.Text = (string)SearchCriteria[CaseIDKey];
            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];
            gvSearchBreachResult.PageIndex = (int)SearchCriteria[PageIndexKey];
            BindBreachSearchGrid();
        }

        protected void gvSearchBreachResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblReducedDistance = (Label)e.Row.FindControl("lblReducedDistance");
                    lblReducedDistance.Text = Calculations.GetRDText(Convert.ToDouble(lblReducedDistance.Text));

                    Label lblOffenceDate = (Label)e.Row.FindControl("lblOffenceDate");
                    DateTime Date = DateTime.Parse(lblOffenceDate.Text);
                    lblOffenceDate.Text = Utility.GetFormattedDate(Date);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}