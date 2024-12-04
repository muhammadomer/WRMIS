using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class AddPunjabIndent : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetPageTitle();
                if (Request.QueryString["mode"] != null && Convert.ToString(Request.QueryString["mode"]) == "e")
                {
                    long PID = Convert.ToInt64(Request.QueryString["PID"]);
                    IW_PunjabIndent pi = new IW_PunjabIndent();
                    pi = new EntitlementDeliveryBLL().GetPunjabIndentByID(PID);
                    setControlsValues(pi);
                }    
            }
            

        }

        protected void btnSaveAccp_Click(object sender, EventArgs e)
        {
            IW_PunjabIndent pi=new IW_PunjabIndent();
           // bool isUpdate = false;
            if (Request.QueryString["PID"] !=null)
	            {
		            pi.ID=Convert.ToInt64(Request.QueryString["PID"]);
                    //isUpdate = true;
	            }

            pi = AddPunjabIndentEntity(pi.ID);
            #region Save  Date
            if (pi.ToIndentDate > DateTime.Now && pi.FromIndentDate > DateTime.Now)
            {
                if (pi.FromIndentDate <= pi.ToIndentDate)
                {
                    new EntitlementDeliveryBLL().AddUpdatePunjabIndent(pi);
                    Response.Redirect("~/Modules/EntitlementDelivery/PunjabIndent.aspx?save=1&td="+pi.ToIndentDate+"&fd="+pi.FromIndentDate+"");
                    //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.FromDateMustbGreaterThan.Description, SiteMaster.MessageType.Error);
                }

            }
            else
            {
                Master.ShowMessage(Message.RecordNotSavedInPerviousDate.Description, SiteMaster.MessageType.Error);
            }
            #endregion
        }
        private IW_PunjabIndent AddPunjabIndentEntity(long PunjabIndentID = 0)
        {
            IW_PunjabIndent pi = new IW_PunjabIndent();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            pi.ID = PunjabIndentID;
            pi.FromIndentDate = Convert.ToDateTime(txtFromDateAdd.Text);
            pi.ToIndentDate = Convert.ToDateTime(txtToDateAdd.Text);
            pi.Thal = Convert.ToDouble(txtThal.Text);
            pi.CJ = Convert.ToDouble(txtCJ.Text);
            pi.CRBCPunjab = Convert.ToDouble(txtCRBC.Text);
            pi.ChashmaDS = Convert.ToDouble(txtBelowChashmaBarrage.Text);
            pi.GTC = Convert.ToDouble(txtGreaterThal.Text);
            pi.Mangla = Convert.ToDouble(txtMangla.Text);
            pi.Remarks = txtRemarks.Text;
            pi.ModifiedBy = (int)mdlUser.ID;
            pi.ModifiedDate = DateTime.Now;
            pi.CreatedBy = (int)mdlUser.ID;
            pi.CreatedDate = DateTime.Now;
            return pi;
        }
        protected void setControlsValues(IW_PunjabIndent pi)
        {
            txtFromDateAdd.Text=Utility.GetFormattedDate(pi.FromIndentDate);
            txtToDateAdd.Text = Utility.GetFormattedDate(pi.ToIndentDate);
            txtThal.Text = Convert.ToString(pi.Thal);
            txtCJ.Text = Convert.ToString(pi.CJ);
            txtCRBC.Text = Convert.ToString(pi.CRBCPunjab);
            txtGreaterThal.Text = Convert.ToString(pi.GTC);
            txtBelowChashmaBarrage.Text = Convert.ToString(pi.ChashmaDS);
            txtMangla.Text = Convert.ToString(pi.Mangla);
            txtRemarks.Text = pi.Remarks;

        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}