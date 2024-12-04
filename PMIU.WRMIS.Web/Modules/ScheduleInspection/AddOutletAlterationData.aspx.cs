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

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddOutletAlterationData : BasePage
    {
        #region ViewState

        public string OutletID_VS = "OutletID";
        public string Designdis = "Designdis";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    InitialBind();
                    hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/CriteriaForSpecificOutletAlteration.aspx?ChannelID={0}", Convert.ToString(hdnChannelID.Value));

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
                bool HasRights = new DailyDataBLL().HasPageRights(LoggedUser, "/Modules/ScheduleInspection/AddOutletAlterationData.aspx");

                if (HasRights)
                {
                    if (CanAdd == false)
                    {
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnSave.Visible = true;
                    }

                    long OutletID = Convert.ToInt64(Request.QueryString["OutletID"]);
                    ViewState[OutletID_VS] = OutletID;
                    object OutletInformation = new DailyDataBLL().OutletInformation(OutletID);
                    txtDate.Text = Utility.GetFormattedDate(DateTime.Now);

                    if (OutletInformation != null)
                    {
                        lblChnlName.Text = Convert.ToString(OutletInformation.GetType().GetProperty("ChannelName").GetValue(OutletInformation));
                        lblOutletRD.Text = Convert.ToString(OutletInformation.GetType().GetProperty("OutletRDs").GetValue(OutletInformation));
                        lblOutletType.Text = Convert.ToString(OutletInformation.GetType().GetProperty("OutletType").GetValue(OutletInformation));
                        hdnChannelID.Value = Convert.ToString(OutletInformation.GetType().GetProperty("ChannelID").GetValue(OutletInformation));
                    }
                    else
                    {
                        lblChnlName.Text = "";
                        lblOutletRD.Text = "";
                        lblOutletType.Text = "";
                        hlBack.Visible = true;
                        Master.ShowMessage(Message.NoOutlet.Description, SiteMaster.MessageType.Error);
                        divDate.Visible = false;
                        fields.Visible = false;
                        mfields.Visible = false;
                        btnSave.Visible = false;

                    }
                }
                else
                {
                    Master.ShowMessage("Not allowed to view screen/Appropriate message", SiteMaster.MessageType.Error);
                    divDate.Visible = false;
                    divFields.Visible = false;
                    btnSave.Visible = false;
                    tableInfo.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OutletAlterationData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text != null && DateTime.Now < Convert.ToDateTime(txtDate.Text))
                {
                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    SI_OutletAlterationHistroy objSave = new SI_OutletAlterationHistroy();
                    objSave.OutletID = Convert.ToInt64(ViewState[OutletID_VS]);
                    if (txtDate.Text != null)
                        objSave.AlterationDate = Utility.GetParsedDate(txtDate.Text).Add(DateTime.Now.TimeOfDay);
                    if (txtHeadAbove.Text != "")
                        objSave.OutletCrest = Convert.ToDouble(txtHeadAbove.Text);
                    if (txtWorkingHead.Text != "")
                        objSave.OutletWorkingHead = Convert.ToDouble(txtWorkingHead.Text);
                    if (txtDiameterBreadth.Text != "")
                        objSave.OutletWidth = Convert.ToDouble(txtDiameterBreadth.Text);
                    if (txtHeightOfOrfice.Text != "")
                        objSave.OutletHeight = Convert.ToDouble(txtHeightOfOrfice.Text);
                    if (txtRemarks.Text != "")
                        objSave.Remarks = Convert.ToString(txtRemarks.Text);
                    objSave.Discharge = Convert.ToDouble(txtDischarge.Text);
                    objSave.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    objSave.CreatedDate = DateTime.Now;
                    objSave.UserID = SessionManagerFacade.UserInformation.ID;
                    objSave.Source = Configuration.RequestSource.RequestFromWeb;
                    new DailyDataBLL().SaveDataAlteration(objSave);
                    CriteriaForSpecificOutletAlteration.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/CriteriaForSpecificOutletAlteration.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value), false);

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}