using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.ClosureOperations;
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

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP
{
    public partial class AnnualCanalClosureProgram : BasePage
    {
        UA_RoleRights mdlRoleRights;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    mdlRoleRights = Master.GetPageRoleRights();
                    hlAddACCP.Enabled = (bool)mdlRoleRights.BAdd;
                    if (!string.IsNullOrEmpty(Request.QueryString["CFCH"]))// if come from child screen
                    {
                        long isUpdateOrSimpleBack = Utility.GetNumericValueFromQueryString("CFCH", 0);
                        string ComeFromDetailScreen = Utility.GetStringValueFromQueryString("CFCH", "EmptyString");
                        if (Session["FilterdACCP"] != null)
                        {
                            List<object> filteredACCP = Session["FilterdACCP"] as List<object>;
                            List<object> ACCP = Session["ACCP"] as List<object>;
                            List<object> lstData = new ClosureOperationsBLL().GetAllACCP();
                            if (lstData.Count > ACCP.Count)
                            {
                                LoadDDLYear();
                                BindGV();
                            }
                            else
                            {
                                filteredACCP = (from lstItem1 in lstData
                                                join lstItem2 in filteredACCP on Convert.ToString(lstItem1.GetType().GetProperty("ID").GetValue(lstItem1)) equals Convert.ToString(lstItem2.GetType().GetProperty("ID").GetValue(lstItem2))
                                                select lstItem1).ToList<object>();
                                LoadDDLYear(Convert.ToString(filteredACCP[0].GetType().GetProperty("Year").GetValue(filteredACCP[0])));
                                Session["FilterdACCP"] = filteredACCP;
                                gvACCP.DataSource = filteredACCP;
                                gvACCP.DataBind();
                            }
                        }
                        else
                        {
                            LoadDDLYear();
                            BindGV();
                        }
                    }
                    else
                    {
                        LoadDDLYear();
                        BindGV();
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<object> lstData = new List<object>();
            if (ddlYear.SelectedItem.Value == "0")
            {
                lstData = new ClosureOperationsBLL().GetAllACCP();
            }
            else
            {
                lstData = new ClosureOperationsBLL().GetACCP(ddlYear.SelectedItem.Value); //TODO Get Data from DB     
            }


            Session["FilterdACCP"] = lstData;
            gvACCP.DataSource = lstData;
            gvACCP.DataBind();

        }

        #region Grid View Events
        private void BindGV()
        {
            try
            {
                List<object> lstData = new List<object>();
                if (ddlYear.SelectedItem.Value == "0")
                {
                    lstData = new ClosureOperationsBLL().GetAllACCP();
                }
                else
                {
                    lstData = new ClosureOperationsBLL().GetACCP(ddlYear.SelectedItem.Value); //TODO Get Data from DB     
                }
                Session["ACCP"] = lstData;
                gvACCP.DataSource = lstData;
                gvACCP.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvACCP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        if (btnEdit != null)
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvACCP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvACCP.PageIndex = e.NewPageIndex;
                BindGV();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvACCP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                if (e.CommandName.Equals("PrintReport"))
                {
                    long accpID = Convert.ToInt64( ((Label)gvrow.FindControl("lblID")).Text.ToString());
                  //  Master.ShowMessage("Report integration under development", SiteMaster.MessageType.Warning);


                    CW_AnnualClosureProgram mdlCWP = new ClosureOperationsBLL().GetACCP_ByID(accpID);

                    // Create Object of "ReportData"
                    PMIU.WRMIS.Web.Common.ReportData mdlReportData = new ReportData();
                    // Set the Report Name (planced in Constants)
                    mdlReportData.Name = Constants.ACCPReport;

                    // Set the report parameters
                    ReportParameter ReportParameter = new ReportParameter("Year", mdlCWP.Year);
                    mdlReportData.Parameters.Add(ReportParameter);

                    // Set the ReportData in Session with specific Key
                    Session[SessionValues.ReportData] = mdlReportData;
                    string ReportViwerurl = "../" + Constants.ReportsUrl;
                    // Open the report printable in new tab
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);

                    return;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvACCP_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HyperLink hlDetails = (HyperLink)e.Row.FindControl("hlDetails");
                        HyperLink btnOrder = (HyperLink)e.Row.FindControl("btnOrder");
                        HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                        if (hlDetails != null && btnOrder != null && hlEdit != null)
                        {
                            hlEdit.Enabled = (bool)mdlRoleRights.BEdit;
                            btnOrder.Enabled = (bool)mdlRoleRights.BEdit;
                            hlDetails.Enabled = (bool)mdlRoleRights.BEdit;

                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AnnualCanalClosureProgram);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadDDLYear(string isSetSelectedYear = "")
        {
            List<object> lstYears = new ClosureOperationsBLL().GetACCPYears();
            Dropdownlist.DDLLoading(ddlYear, false, (int)Constants.DropDownFirstOption.NoOption, lstYears);
            ddlYear.Items.Insert(0, new ListItem("All", "0"));
            if (!string.IsNullOrEmpty(isSetSelectedYear))
            {
                object obj = (from o in lstYears where Convert.ToString(o.GetType().GetProperty("Name").GetValue(o)) == isSetSelectedYear select o).FirstOrDefault();
                ddlYear.SelectedValue = Convert.ToString(obj.GetType().GetProperty("ID").GetValue(obj));
            } 
        }

    }
}