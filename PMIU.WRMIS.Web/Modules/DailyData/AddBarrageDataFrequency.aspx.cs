using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class AddBarrageDataFrequency : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitialBind();
                    SetTitle();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void InitialBind()
        {
            try
            {
                long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/DailyData/AddBarrageDataFrequency.aspx");
                btnSave.Visible = false;
                if (HasRights)
                {
                    ddlBarrages.DataSource = new DailyDataBLL().GetAllBarrages();
                    ddlBarrages.DataTextField = "Name";
                    ddlBarrages.DataValueField = "ID";
                    ddlBarrages.DataBind();
                    ddlBarrages.Items.Insert(0, new ListItem("Select", ""));

                    ddlFrequency.DataSource = new DailyDataBLL().FrequencyValues();
                    ddlFrequency.DataTextField = "Name";
                    ddlFrequency.DataValueField = "ID";
                    ddlFrequency.DataBind();
                    ddlFrequency.Items.Insert(0, new ListItem("Select", ""));
                    //ddlFrequency.Items.Insert(0, new ListItem("Select", "-1"));
                    //ddlFrequency.Items.Insert(1, new ListItem("Twice a day", "2"));
                    //ddlFrequency.Items.Insert(2, new ListItem("Six Hourly", "4"));
                    //ddlFrequency.Items.Insert(3, new ListItem("Three Hourly", "8"));
                    //ddlFrequency.Items.Insert(4, new ListItem("Hourly", "24"));
                }
                else
                {
                    Master.ShowMessage("You are not a valid user to view this screen.", SiteMaster.MessageType.Error);
                    ddlBarrages.Visible = false;
                    ddlFrequency.Visible = false;
                    btnCancel.Visible = false;
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
                string SelectedBarrage = ddlBarrages.SelectedItem.Text.ToString();
                string SelectedFrequency = ddlFrequency.SelectedItem.Text.ToString();

                if (SelectedBarrage != "Select")
                {
                    if (SelectedFrequency != "Select")
                    {
                        long BarrageID = Convert.ToInt64(ddlBarrages.SelectedItem.Value);
                        long FrequencyID = Convert.ToInt64(ddlFrequency.SelectedItem.Value);
                        bool result = false;
                        bool Exist = new DailyDataBLL().BarrageFrequencyExist(BarrageID, FrequencyID);
                        if (!Exist)
                        {
                            btnSave.Visible = false;
                            result = new DailyDataBLL().SavaFrequency(BarrageID, FrequencyID, DateTime.Now);
                            if (result)
                            {
                                Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                                ddlBarrages.ClearSelection();
                                ddlBarrages.Items.FindByText("Select").Selected = true;
                                ddlFrequency.ClearSelection();
                                ddlFrequency.Items.FindByText("Select").Selected = true;
                            }
                        }
                        else
                        {
                            Master.ShowMessage(Message.ValueAlreadySet.Description, SiteMaster.MessageType.Error);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //ddlBarrages.ClearSelection();
                //ddlBarrages.Items.FindByText("Select").Selected = true;
                //ddlFrequency.Items.FindByText("Select").Selected = true;
                //btnSave.Visible = false;

                ddlFrequency.ClearSelection();
                long BarrageID = Convert.ToInt64(ddlBarrages.SelectedItem.Value);
                long? LastFrequency = new DailyDataBLL().ViewBarrageFrequency(BarrageID);
                if (LastFrequency != 0)
                {
                    ddlFrequency.Items.FindByValue(Convert.ToString(LastFrequency)).Selected = true;
                }
                else
                {
                    ddlFrequency.Items.FindByText("Twice A Day").Selected = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.BarrageDataFrequency);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void ddlBarrages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CanEdit == false)
                    btnSave.Visible = false;
                else
                {
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                }

                long BarrageID = Convert.ToInt64(ddlBarrages.SelectedItem.Value);
                long? LastFrequency = new DailyDataBLL().ViewBarrageFrequency(BarrageID);
                ddlFrequency.ClearSelection();
                if (LastFrequency != 0)
                {
                    ddlFrequency.Items.FindByValue(Convert.ToString(LastFrequency)).Selected = true;
                }
                else
                {
                    ddlFrequency.Items.FindByText("Twice A Day").Selected = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}