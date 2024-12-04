using PMIU.WRMIS.BLL.Tenders;
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

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class AddWorkItems : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadAllDropDowns();
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderNoticeID"]))
                    {
                        hdnTenderNoticeID.Value = Request.QueryString["TenderNoticeID"];
                        BindTenderDetailData(Convert.ToInt64(hdnTenderNoticeID.Value));
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + Convert.ToInt64(hdnTenderNoticeID.Value);
                    }

                    ClosureWorkDiv.Visible = true;
                    gvClosureWorks.Visible = true;
                    
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadAllDropDowns()
        {
            try
            {
                BindDivisionDropDown();
                BindAssetClosureWorkTypeDropDown();
                long SelectedDivisionID = 0;
                if (ddlDivision.SelectedValue == "")
                {
                    SelectedDivisionID = -1;
                }
                else
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedValue);
                }
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlYear, new TenderManagementBLL().GetYearByDivisionID(SelectedDivisionID));
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlFiscalYear, new TenderManagementBLL().GetAssetYearByDivisionID(SelectedDivisionID));
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDivisionDropDown()
        {
            try
            {
                UA_Users MdlUser = SessionManagerFacade.UserInformation;
                      
            if (MdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.DDLDivisionsByUserID(ddlAssetDivision, MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
            }

            else if (MdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, MdlUser.ID, (long)MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.DDLDivisionsByUserID(ddlAssetDivision, MdlUser.ID, (long)MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                ddlDivision.SelectedIndex = 1;
                ddlAssetDivision.SelectedIndex = 1;
            }

        
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindAssetClosureWorkTypeDropDown()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlClosureWorkType, new TenderManagementBLL().GetClosureWorkTypes());
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlAssetType, new TenderManagementBLL().GetAssetTypes());
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long SelectedDivisionID = 0;
                if (ddlDivision.SelectedValue == "")
                {
                    SelectedDivisionID = -1;
                }
                else
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedValue);
                }
                 
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlYear, new TenderManagementBLL().GetYearByDivisionID(SelectedDivisionID));

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void BindTenderDetailData(long _TenderNoticeID)
        {
            try
            {
                dynamic TenderData = new TenderManagementBLL().GetTenderDataByID(_TenderNoticeID);
                string TenderNotice = Utility.GetDynamicPropertyValue(TenderData, "Name");
                string Domain = Utility.GetDynamicPropertyValue(TenderData, "Domain");
                string Division = Utility.GetDynamicPropertyValue(TenderData, "Division");
                string DivisionID = Utility.GetDynamicPropertyValue(TenderData, "DivisionID");
                hdnDivisionID.Value = DivisionID;

                Tenders.Controls.AddWorks.TenderNotice = TenderNotice;
                Tenders.Controls.AddWorks.TenderDomain = Domain;
                Tenders.Controls.AddWorks.TenderDivision = Division;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvClosureWorks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    Label lblComplaintDate = (Label)e.Row.FindControl("lblComplaintDate");
                //    LinkButton hlQuickAction = (LinkButton)e.Row.FindControl("hlQuickAction");
                //    HyperLink hlFavorite = (HyperLink)e.Row.FindControl("hlFavorite");
                //    LinkButton lbtnPrint = (LinkButton)e.Row.FindControl("lbtnPrint");
                //    HyperLink hlComplaintActivity = (HyperLink)e.Row.FindControl("hlComplaintActivity");
                //    Label lblFavorite = (Label)e.Row.FindControl("lblFavorite");
                //    Label lblStatusID = (Label)e.Row.FindControl("lblStatus");
                //    //HyperLink hlComplaintEdit = (HyperLink)e.Row.FindControl("hlComplaintEdit");
                    

                //    DateTime Date = DateTime.Parse(lblComplaintDate.Text);
                //    lblComplaintDate.Text = Utility.GetFormattedDate(Date); //+ " " + Utility.GetFormattedTime(Date);
                //    int LastRow = gvSearchComplaints.Rows.Count - 1;
                //    int cell = gvSearchComplaints.Columns.Count - 1;
                //    if (mdlUser.DesignationID == (long)Constants.Designation.HelplineOperator || mdlUser.DesignationID == (long)Constants.Designation.DataEntryOperator)
                //    {
                //        gvSearchComplaints.Columns[0].Visible = false;
                //        //gvSearchComplaints.Columns[cell].Visible = false;
                //        hlQuickAction.Visible = false;
                //        hlFavorite.Visible = false;
                //        lbtnPrint.Visible = false;
                //        hlComplaintActivity.Visible = false;

                //    }
                //    if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline)
                //    {
                //        hlQuickAction.Visible = true;
                     

                //    }
                //    if (lblFavorite.Text == "True")
                //    {
                //        hlFavorite.CssClass = hlFavorite.CssClass.Replace("StarComplaint", "FillStar");
                //    }
                //    if (lblStatusID.Text == "Resolved" && mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                //    {
                //        hlComplaintActivity.Enabled = false;
                //    }
              
                //}


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvClosureWorks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvClosureWorks.PageIndex = e.NewPageIndex;

                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void Enable_WorkType(object sender, EventArgs e)
        {
            try
            {
                if (RadioButtonWorkType.SelectedItem.Value == "1")
                {
                    ClosureWorkDiv.Visible = true;
                    gvClosureWorks.Visible = true;
                    AssetDiv.Visible = false;
                }
                else
                {
                    ClosureWorkDiv.Visible = false;
                    gvClosureWorks.Visible = false;
                    AssetDiv.Visible = true;

                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void BindSearchResultsGrid()
        {
            try
            {
                string rbtValue = RadioButtonWorkType.SelectedItem.Value;
                long DivisionID = 0;
                string Year = string.Empty;
                long ClosureWorkType = 0;

                if (rbtValue == "1")
                {
                    DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    if (DivisionID == -1)
                        DivisionID = Convert.ToInt64(hdnDivisionID.Value);

                    Year = ddlYear.SelectedItem.Text == "Select" ? "" : Convert.ToString(ddlYear.SelectedItem.Text);
                    ClosureWorkType = ddlClosureWorkType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlClosureWorkType.SelectedItem.Value);
                }

               
               
         // Assest
                else
                {
                    DivisionID = ddlAssetDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlAssetDivision.SelectedItem.Value);
                    if (DivisionID == -1)
                        DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                    ClosureWorkType = ddlAssetType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlAssetType.SelectedItem.Value);
                    Year = ddlFiscalYear.SelectedItem.Text == "Select" ? "" : Convert.ToString(ddlFiscalYear.SelectedItem.Text);
                }
              
                
                List<dynamic> lstClosureWorks = new TenderManagementBLL().GetClosureWorks(DivisionID, Year, ClosureWorkType, Convert.ToInt64(hdnTenderNoticeID.Value), rbtValue);
                gvClosureWorks.DataSource = lstClosureWorks;
                gvClosureWorks.DataBind();
                gvClosureWorks.Visible = true;
                BtnSave.Visible = true;
                if (lstClosureWorks.Count == 0)
                {
                    BtnSave.CssClass = "btn btn-primary disabled";
                }
                else
                {
                    BtnSave.CssClass = "btn btn-primary";
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsSaved = false;
                List<long> ClosureWorkIDs = new List<long>();
                string rbtValue = RadioButtonWorkType.SelectedItem.Value;
                string AssestorClosure = string.Empty;
                if (rbtValue == "1")
                    AssestorClosure = Convert.ToString(Constants.WorkType.CLOSURE);
                else
                    AssestorClosure = Convert.ToString(Constants.WorkType.ASSET);


                foreach (GridViewRow gvrow in gvClosureWorks.Rows)
                {
                    Label WorkID = (Label)(gvrow.FindControl("lblClosureWorkID"));
                    if (WorkID.Text == "-1")
                    {
                        continue;
                    }
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    if (chk != null & chk.Checked)
                    {
                        int ClosureWorkID = Convert.ToInt32(((Label)gvrow.Cells[0].FindControl("lblClosureWorkID")).Text);
                        IsSaved = new TenderManagementBLL().SaveClosureWorkByTenderNoticeID(Convert.ToInt64(hdnTenderNoticeID.Value), ClosureWorkID, AssestorClosure, (int)Constants.WorkStatus.NotSold, Convert.ToInt32(SessionManagerFacade.UserInformation.ID));
                        if (!IsSaved)
                        {
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (IsSaved)
                {
                    AddWorks.IsSaved = true;
                    Response.Redirect("~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + Convert.ToInt64(hdnTenderNoticeID.Value), false); 
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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