using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class AddElevationCapacity : BasePage
    {
        #region Variables

        List<SP_RefElevationCapacity> lstEC;
        SP_RefElevationCapacity ECObj;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    ddlRimStation.DataSource = CommonLists.GetRimStationsForElevationCapacity();
                    ddlRimStation.DataTextField = "Name";
                    ddlRimStation.DataValueField = "ID";
                    ddlRimStation.DataBind();
                    btnSave.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lstEC = new List<SP_RefElevationCapacity>();
                long NoOfRows = Convert.ToInt64(txtNoOfRows.Text);
                int Level = Convert.ToInt32(txtInitialLevel.Text);

                lstEC = new SeasonalPlanningBLL().GetRecordsOfSelectedDate(Convert.ToDateTime(txtDate.Text), ddlRimStation.SelectedItem.Value == "" ? -1 : Convert.ToInt64(ddlRimStation.SelectedItem.Value));
                if (lstEC.Count() == 0)
                {
                    for (int i = 0; i < NoOfRows; i++)
                    {
                        ECObj = new SP_RefElevationCapacity();
                        ECObj.Level = Level;
                        lstEC.Add(ECObj);
                        Level++;
                    }
                    gvElevation.Visible = true;
                    gvElevation.DataSource = lstEC;
                    gvElevation.DataBind();
                    TextBox txCapacity = (TextBox)gvElevation.Rows[0].FindControl("txtCapacity");
                    if (txCapacity != null)
                    {
                        txCapacity.CssClass = txCapacity.CssClass.Replace("form-control decimalInput", "form-control decimalInput required");
                        txCapacity.Attributes.Add("required", "required");
                    }
                    btnSave.Visible = true;
                }
                else
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int Level;
                double Capacity;
                lstEC = new List<SP_RefElevationCapacity>();
                foreach (GridViewRow row in gvElevation.Rows)
                {
                    ECObj = new SP_RefElevationCapacity();
                    Level = Convert.ToInt32(((Label)row.FindControl("lblLevel")).Text);
                    Capacity = Convert.ToDouble(((TextBox)row.FindControl("txtCapacity")).Text);
                    ECObj.Capacity = Capacity;
                    ECObj.Level = Level;
                    ECObj.StationID = ddlRimStation.SelectedItem.Value == "" ? -1 : Convert.ToInt64(ddlRimStation.SelectedItem.Value);
                    ECObj.ElevationCapacityDate = Convert.ToDateTime(txtDate.Text);
                    ECObj.CreatedDate = DateTime.Now;
                    ECObj.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ECObj.ModifiedDate = DateTime.Now;
                    ECObj.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    lstEC.Add(ECObj);
                }
                bool Result = new SeasonalPlanningBLL().AddBulkElevationCapacity(lstEC);
                if (Result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    Response.RedirectPermanent("ElevationCapacity.aspx?ShowMsg=YES");
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }





    }
}