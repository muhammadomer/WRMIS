using PMIU.WRMIS.BLL.ClosureOperations;
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

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP
{
    public partial class AddACCP : BasePage
    {
        Dictionary<string, object> dd_ACCP = new Dictionary<string, object>();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                ResetPage();
                if (!string.IsNullOrEmpty(Request.QueryString["ACCPID"]))
                {
                    LoadACCP(Convert.ToInt64(Request.QueryString["ACCPID"]));
                    hlBack.NavigateUrl = string.Format("~/Modules/ClosureOperations/ACCP/AnnualCanalClosureProgram.aspx?CFCH=1");
                }

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int _acpID = 0;
                string name = txtTitle.Text.Trim();
                string year = txtYear.Text.Trim();
                List<Tuple<string, string, string>> lstNameofFiles;
                lstNameofFiles = AddFileUploadControl.UploadNow(Configuration.ClosureOperations);
                string attachment = lstNameofFiles.Count > 0 ? lstNameofFiles[0].Item3 : "";
                long id = 0;
                if (!string.IsNullOrEmpty(hdnFldID.Value)) // Edit case
                    id = Convert.ToInt64(hdnFldID.Value);

                ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
                dd_ACCP.Clear();
                dd_ACCP.Add("Name", name);
                dd_ACCP.Add("Year", year);
                CW_AnnualClosureProgram mdlWork = (CW_AnnualClosureProgram)bllCO.ACCP_Operations(Constants.CRUD_READ, dd_ACCP, out _acpID);
                if (mdlWork != null && id != mdlWork.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                dd_ACCP.Add("ID", id);
                dd_ACCP.Add("Attachment", attachment);
                dd_ACCP.Add("UserID", SessionManagerFacade.UserInformation.ID);
                bool status = false;
                if (id == 0)
                {
                    if (chkCopyLastYearDetail.Checked)
                    {
                        status = (bool)bllCO.CopyLastYearACCP_Detial_ExcludedChannels(name, year, attachment, SessionManagerFacade.UserInformation.ID);
                    }
                    else
                    {
                        status = (bool)bllCO.ACCP_Operations(Constants.CRUD_CREATE, dd_ACCP, out _acpID);
                    }

                }
                else
                    status = (bool)bllCO.ACCP_Operations(Constants.CRUD_UPDATE, dd_ACCP, out _acpID);
                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    ResetPage();
                    long ACPID = Utility.GetNumericValueFromQueryString("ACCPID", 0);
                    if (!string.IsNullOrEmpty(Request.QueryString["ACCPID"]))
                    {
                        Response.Redirect("~/Modules/ClosureOperations/ACCP/AnnualCanalClosureProgram.aspx?CFCH=2");
                    }
                    else
                    { 
                        dd_ACCP.Clear();
                        dd_ACCP.Add("Year", year);
                        CW_AnnualClosureProgram mdlACCP = (CW_AnnualClosureProgram)bllCO.ACCP_Operations(Constants.CRUD_READ, dd_ACCP, out _acpID);
                        long accppID = _acpID;
                        if (mdlACCP != null)
                            accppID = mdlACCP.ID;
                        Response.Redirect(string.Format("~/Modules/ClosureOperations/ACCP/ACCPDetails.aspx?ACCPID={0}", accppID));
                    }
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddAnnualCanalClosureProgram);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadACCP(long _ACCPID)
        {
            dd_ACCP.Clear();
            dd_ACCP.Add("ID", _ACCPID);
            int _acpID = (int)_ACCPID;

            CW_AnnualClosureProgram mdlWork = (CW_AnnualClosureProgram)bllCO.ACCP_Operations(Constants.CRUD_READ, dd_ACCP, out _acpID);

            if (mdlWork != null)
            {
                lblHeader.Text = "Edit Annual Canal Closure Programme";
                hdnFldID.Value = _ACCPID + "";
                txtTitle.Text = mdlWork.Title;
                txtYear.Text = mdlWork.Year;
                if (!string.IsNullOrEmpty(mdlWork.Attachment))
                {
                    hlImage.NavigateUrl = Utility.GetImageURL("ClosureOperations", mdlWork.Attachment);
                    hlImage.Text = "View Attachment";
                    hlImage.Visible = true;
                }
                CellOfCopyLastYearDetail.Visible = false;
            }
        }
        private void ResetPage()
        {
            lblHeader.Text = "Add Annual Canal Closure Programme";
            txtTitle.Text = "";
            txtYear.Text = GetFinancialYear(); //DateTime.Now.Year + "-" + (DateTime.Now.Year + 1);
            hdnFldID.Value = "";
        }
        private string GetFinancialYear()
        {
            DateTime nowDate = DateTime.Now;

            if (nowDate.Month >= 1 && nowDate.Month <= 6)
            {
                return "" + (DateTime.Today.Year - 1) + "-" + (DateTime.Today.Year);
            }

            return DateTime.Today.Year + "-" + (DateTime.Today.Year + 1);
        }

    }
}