using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class ViewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ReportData mdlReportData = (ReportData)Session[SessionValues.ReportData];

                    Session[SessionValues.ReportData] = null;

                    if (mdlReportData != null && mdlReportData.Name != string.Empty && mdlReportData.Parameters.Count() > 0)
                    {
                        // Set the processing mode for the ReportViewer to Remote  
                        rvReport.ProcessingMode = ProcessingMode.Remote;

                        rvReport.ShowCredentialPrompts = false;

                        ServerReport ServerReport = rvReport.ServerReport;


                        string RPTUser = System.Configuration.ConfigurationManager.AppSettings["RPTUser"].ToString();
                        string RPTPassword = System.Configuration.ConfigurationManager.AppSettings["RPTPassword"].ToString();
                        string RPTDomain = System.Configuration.ConfigurationManager.AppSettings["RPTDomain"].ToString();
                        string RPTURL = System.Configuration.ConfigurationManager.AppSettings["RPTURL"].ToString();


                        IReportServerCredentials SSRSCredentials = new CustomSSRSCredentials(RPTUser, RPTPassword, RPTDomain);
                        ServerReport.ReportServerCredentials = SSRSCredentials;

                        // Set the report server URL and report path  
                        ServerReport.ReportServerUrl = new Uri(RPTURL);
                        ServerReport.ReportPath = mdlReportData.Name;

                        UA_SystemParameters mdlSystemParameters = new UserBLL().GetSystemParameterValue((short)Constants.SystemParameter.ReportFooter);

                        UA_Users mdlUser = SessionManagerFacade.UserInformation;

                        string UserDesignation = string.Empty;

                        if (mdlUser != null)
                        {
                            if (mdlUser.DesignationID != null)
                            {
                                UserDesignation = mdlUser.UA_Designations.Name;
                            }
                            else
                            {
                                UserDesignation = mdlUser.UA_Roles.Name;
                            }
                        }

                        string UserName = string.Format("{0} {1}", mdlUser.FirstName, mdlUser.LastName);
                        ReportParameter reportParameter = new ReportParameter("UserDesignation", UserDesignation);
                        mdlReportData.Parameters.Add(reportParameter);

                        reportParameter = new ReportParameter("UserName", UserName);
                        mdlReportData.Parameters.Add(reportParameter);

                        //reportParameter = new ReportParameter("ReportFooter", mdlSystemParameters.ParameterValue);
                        //mdlReportData.Parameters.Add(reportParameter);

                        //// Set the report parameters for the report  
                        ServerReport.SetParameters(mdlReportData.Parameters);

                        ServerReport.Refresh();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "javascript:window.close();", true);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}