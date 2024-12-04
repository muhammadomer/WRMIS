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

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class WaterTheftReferenceData : BasePage
    {
        private enum ReferencePageName
        {
            OffenceType = 1,
            Abiana = 2,
            General = 3
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int ReferenceTypeId = 0;
            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["RP"]))
                    {
                        ReferenceTypeId = Convert.ToInt32(Request.QueryString["RP"]);
                        ShowHideDiv(ReferenceTypeId);
                        BindOffenceTypeGridView();
                        BindAbianaGridView();
                        WT_FeettoIgnore lstFeetToIgnor = new WaterTheftBLL().FeetToIgnore();
                        txtFeetToIgnor.Text = lstFeetToIgnor.NoOfFeet.ToString();
                    }

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ReferenceData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void ShowHideDiv(int ReferenceTypeId)
        {
            if ((int)ReferencePageName.OffenceType == ReferenceTypeId)
            {
                PageTitle.InnerText = "Type of offence";
                DivAbiana.Visible = false;
                DivGeneral.Visible = false;
                offenceType.Visible = true;
            }
            else if ((int)ReferencePageName.Abiana == ReferenceTypeId)
            {
                PageTitle.InnerText = "Abiana";
                offenceType.Visible = false;
                DivGeneral.Visible = false;
                DivAbiana.Visible = true;
            }
            else if ((int)ReferencePageName.General == ReferenceTypeId)
            {
                PageTitle.InnerText = "General";
                offenceType.Visible = false;
                DivAbiana.Visible = false;
                DivGeneral.Visible = true;
            }

        }

        #region "Offence Type GridView Events"
        private void BindOffenceTypeGridView()
        {
            try
            {
               // List<WT_OffenceType> lstOffenceType = new WaterTheftBLL().GetOffenceType();
                string OffenceSite =  "C";
                List<object> lstTheftType = new WaterTheftBLL().GetAllTheftType(OffenceSite);
                gvOffenceType.DataSource = lstTheftType;
                gvOffenceType.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               // string OffenceSite = PMIU.WRMIS.Common.Constants.WTOffenceSite.Channel.ToString();
                if (e.CommandName == "AddTypeOffenceType")
                {
                    List<object> lstOffenceType = new WaterTheftBLL().GetOffenceType();

                    lstOffenceType.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,

                    });

                    gvOffenceType.PageIndex = gvOffenceType.PageCount;
                    gvOffenceType.DataSource = lstOffenceType;
                    gvOffenceType.DataBind();

                    gvOffenceType.EditIndex = gvOffenceType.Rows.Count - 1;
                    gvOffenceType.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOffenceType.PageIndex = e.NewPageIndex;
                gvOffenceType.EditIndex = -1;
                BindOffenceTypeGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOffenceType.EditIndex = -1;
                BindOffenceTypeGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOffenceType.EditIndex = e.NewEditIndex;
                BindOffenceTypeGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long UserId = (long)Session[SessionValues.UserID];
                GridViewRow row = gvOffenceType.Rows[e.RowIndex];
                TextBox txtOffenceType = (TextBox)row.FindControl("txtOffenceType");
                string OffenceID = Convert.ToString(gvOffenceType.DataKeys[e.RowIndex].Values[0]);

                WT_OffenceType OffenceType = new WT_OffenceType();

                OffenceType.ID = Convert.ToInt64(OffenceID);


                if (txtOffenceType != null)
                    OffenceType.Name = txtOffenceType.Text;
                OffenceType.Description = txtOffenceType.Text;
                OffenceType.ChannelOutlet = "C";
                OffenceType.IsActive = true;
                OffenceType.CreatedBy = UserId;
                OffenceType.CreatedDate = DateTime.Now;

                bool IsSaved = new WaterTheftBLL().SaveOffenceType(OffenceType);

                if (IsSaved)
                {
                    if (Convert.ToInt64(OffenceID) == 0)
                        gvOffenceType.PageIndex = 0;

                    gvOffenceType.EditIndex = -1;
                    BindOffenceTypeGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string OffenceID = Convert.ToString(gvOffenceType.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new WaterTheftBLL().DeleteOffenceType(Convert.ToInt64(OffenceID));
                if (IsDeleted)
                {
                    BindOffenceTypeGridView();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
               // Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }

        #endregion "End Offence Type GridView Events"

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
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
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
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
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
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
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
                    if (AreaType == "Acre")
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
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.ExceptionCategory.WebApp);
            }
        }
        #endregion "End Abiana GridView Events"

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
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.ExceptionCategory.WebApp);
            }

        }
    }
}