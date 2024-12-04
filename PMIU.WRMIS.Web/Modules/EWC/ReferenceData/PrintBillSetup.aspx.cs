using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
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

namespace PMIU.WRMIS.Web.Modules.EWC.ReferenceData
{
    public partial class PrintBillSetup : BasePage
    {
        //Data Members   
        Effluent_WaterChargesBLL bll_EWC = new Effluent_WaterChargesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle(); 
                    LoadData();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadData()
        {
            try
            {            
                txt1.Text = txt2.Text = txtNo.Text = "";
                EC_PrintBillSertup mdl = bll_EWC.BillSetup_Get();
                if (mdl != null)
                {
                    txt1.Text = mdl.EffluentText1;
                    txt2.Text = mdl.EffluentText2;

                    txtText1Canal.Text = mdl.CanalText1;
                    txtText2Canal.Text = mdl.CanalText2;

                    txtNo.Text = mdl.HelpLineNo;
                }
            } 
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }  
        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn.ID.Equals("btnSave"))
                {
                    EC_PrintBillSertup mdl = new EC_PrintBillSertup();

                    mdl.EffluentText1 = txt1.Text.Trim();
                    mdl.EffluentText2 = txt2.Text.Trim();

                    mdl.CanalText1 = txtText1Canal.Text.Trim();
                    mdl.CanalText2 = txtText2Canal.Text.Trim();

                    mdl.HelpLineNo = txtNo.Text.Trim(); 
                    try
                    {
                        mdl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    }
                    catch (Exception ex)
                    {
                        //ADMIN as default user in case of typecast exception from long to int
                        mdl.ModifiedBy = 2;
                    }
                    mdl.ModifiedDate = DateTime.Now;
                     
                    if (bll_EWC.BillSetup_AddNew(mdl))
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    else
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            LoadData();
        }
    }
}