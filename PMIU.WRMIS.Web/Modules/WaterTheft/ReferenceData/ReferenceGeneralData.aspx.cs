using PMIU.WRMIS.BLL.WaterTheft;
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

namespace PMIU.WRMIS.Web.Modules.WaterTheft.ReferenceData
{
    public partial class ReferenceGeneralData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
                    WT_FeettoIgnore lstFeetToIgnor = new WaterTheftBLL().FeetToIgnore();
                    txtFeetToIgnor.Text = lstFeetToIgnor.NoOfFeet.ToString();
                    btnUpdateFeet.Visible = base.CanEdit;

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ReferenceData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnUpdateFeetToIgnor_Click(object sender, EventArgs e)
        {
            try
            {
                long UserID = (long)Session[SessionValues.UserID];
                WT_FeettoIgnore FeetToIgnor = new WT_FeettoIgnore();
                FeetToIgnor.NoOfFeet = Convert.ToInt32(txtFeetToIgnor.Text);
                FeetToIgnor.ID = 1;
                FeetToIgnor.IsActive = true;
                FeetToIgnor.ModifiedBy = UserID;
                FeetToIgnor.ModifiedDate = DateTime.Now;
                bool IsSaved = new WaterTheftBLL().UpdateFeetToIgnor(FeetToIgnor);

                if (IsSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}