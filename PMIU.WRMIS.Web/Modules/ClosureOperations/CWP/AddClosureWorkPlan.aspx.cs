
﻿using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

﻿using PMIU.WRMIS.Web.Common;

﻿using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
    //User of this screen is XEN only 
    public partial class AddClosureWorkPlan : BasePage
    {
        Dictionary<string, object> dd_CWP = new Dictionary<string, object>();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        CW_ClosureWorkPlan Gcwp; 
        long UserID = 0; 
        protected void Page_Load(object sender, EventArgs e)
        { 
            Gcwp = new CW_ClosureWorkPlan();
            if (!IsPostBack)
            {
                if(( (bool)(Master.GetPageRoleRights().BAdd)) || ((bool)(Master.GetPageRoleRights().BEdit)))
                    btnSaveClosureWorkPlan.Enabled =true;
                else
                    btnSaveClosureWorkPlan.Enabled = false ;
                SetPageTitle();
                hdnFldID.Value = "0";
                LoadDDL();  
                if (!string.IsNullOrEmpty(Request.QueryString["CWPID"])) 
                      EditLoad(Request.QueryString["CWPID"]);  
            } 
        }
        private void EditLoad(string CWPID)
        {
            Gcwp.ID = Convert.ToInt32(CWPID);
            Gcwp = bllCO.GetClosureWorkPlanByID(Convert.ToInt64(CWPID));
            ddlDivision.Items.FindByValue(Gcwp.DivisionID.ToString()).Selected = true;
            ddlYear.Items.FindByText(Gcwp.Year).Selected = true;
            txtComments.Text = Gcwp.Comments;
            txtCWPTitle.Text = Gcwp.Title;
            ddlYear.Enabled = false;
            lblAdCWP.InnerText = "Edit Closure Work Plan";
            hdnFldID.Value = CWPID; 
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddClosureWorkPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSaveClosureWorkPlan_Click(object sender, EventArgs e)
        { 
            try
            {
                long ID = Convert.ToInt64(hdnFldID.Value);
                dd_CWP.Clear();
                dd_CWP.Add("DivisionID", Convert.ToInt32(ddlDivision.SelectedItem.Value));
                dd_CWP.Add("Year", ddlYear.SelectedItem.Text);
                
                //Duplication check
                CW_ClosureWorkPlan ExistCWP = bllCO.GetCWPBy_Year_Name(dd_CWP["Year"].ToString(), Convert.ToInt64(dd_CWP["DivisionID"]));
                if (ExistCWP != null && ExistCWP.ID != ID)
                {
                    Master.ShowMessage(Message.UniqueYearandDivision.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (ID == 0 )//Add case
                { 
                    dd_CWP.Add("CreatedDate", DateTime.Now);
                    dd_CWP.Add("CreatedBy", SessionManagerFacade.UserInformation.ID); 
                }
                else //Edit case
                {
                    dd_CWP.Add("ID", ID);
                    dd_CWP.Add("ModifiedBy", SessionManagerFacade.UserInformation.ID);
                    dd_CWP.Add("ModifiedDate", DateTime.Now);
                } 

                dd_CWP.Add("ACPID", ddlYear.SelectedItem.Value);
                dd_CWP.Add("Title", txtCWPTitle.Text);
                dd_CWP.Add("Comments",  txtComments.Text);

                bool status = false;
                if (ID == 0)
                    status = (bool)bllCO.CWP_Operations(Constants.CRUD_CREATE, dd_CWP);
                else
                    status = (bool)bllCO.CWP_Operations(Constants.CRUD_UPDATE, dd_CWP);
                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    Response.Redirect("ClosureWorkPlan.aspx?RestoreState=1", false);
                }
            }

            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        } 
        private void LoadDDL()
        {
            try
            {
                //Years List
                List<object> lstYear = new List<object>();
                lstYear = new ClosureOperationsBLL().GetOldestClosureWorkPlan();
                Dropdownlist.DDLLoading(ddlYear, false,(int)Constants.DropDownFirstOption.NoOption, lstYear);

                //Divisions List
                long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                if (userID > 0) // Irrigation Staff 
                    Dropdownlist.DDLDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.NoOption); 
                else
                    Dropdownlist.DDLDivisionNames(ddlDivision); 
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }  
    }
}