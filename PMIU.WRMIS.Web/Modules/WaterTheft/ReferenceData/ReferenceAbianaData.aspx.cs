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
    public partial class ReferenceAbianaData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
          
                   
                         BindAbianaGridView();
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
        #region "Abiana GridView Events"
        private void BindAbianaGridView()
        {
            try
            {
                List<object> lstOfAbiana = new WaterTheftBLL().GetListOfAbiana();
                gvAbiana.DataSource = lstOfAbiana;
                gvAbiana.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAbiana_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAbiana.EditIndex = -1;
                BindAbianaGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvAbiana_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAbiana.EditIndex = e.NewEditIndex;
                BindAbianaGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvAbiana_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long UserId = (long)Session[SessionValues.UserID];
                GridViewRow row = gvAbiana.Rows[e.RowIndex];
                TextBox txtAreaType = (TextBox)row.FindControl("txtAreaType");
                TextBox txtAbianaRate = (TextBox)row.FindControl("txtAbianaRate");
                TextBox txtMaxPercentage = (TextBox)row.FindControl("txtMaxPercentage");
                string AbianaID = Convert.ToString(gvAbiana.DataKeys[e.RowIndex].Values[0]);

                WT_Abiana abiana = new WT_Abiana();

                abiana.ID = Convert.ToInt64(AbianaID);

                String AreaType = txtAreaType.Text;
                if (txtAreaType != null)
                {
                    if (AreaType == Constants.WTAreaType.Acre.ToString())  //"Acre"
                    {
                        abiana.AreaTypeID = (int)Constants.WTAreaType.Acre;
                    }
                    else
                    {
                        abiana.AreaTypeID = (int)Constants.WTAreaType.Kanal;
                    }
                }

                if (txtAbianaRate != null)
                {
                    abiana.AbianaRate = Convert.ToInt32(txtAbianaRate.Text);
                }
                if (txtMaxPercentage != null)
                {
                    abiana.MaxPercentage = Convert.ToInt32(txtMaxPercentage.Text);
                }

                abiana.CreatedBy = UserId;
                abiana.CreatedDate = DateTime.Now;

                bool IsSaved = new WaterTheftBLL().SaveWTAbiana(abiana);

                if (IsSaved)
                {
                    if (Convert.ToInt64(AbianaID) == 0)
                        gvAbiana.PageIndex = 0;

                    gvAbiana.EditIndex = -1;
                    BindAbianaGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion "End Abiana GridView Events"
    }
}