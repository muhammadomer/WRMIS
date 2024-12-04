using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigatorsFeedback;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigatorsFeedback
{
    public partial class AddIrrigator : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                    SetPageTitle();
                    hlBack.NavigateUrl = "~/Modules/IrrigatorsFeedback/SearchIrrigator.aspx?ShowHistory=true";
                    long UserID = Convert.ToInt64(Request.QueryString["UserID"]);
                    if (UserID != 0)
                    {
                        GetIrrigatorByID(UserID);
                        btnSave.Visible = base.CanEdit;
                    }
                    else
                    {
                        BindZoneDropdown();
                        btnSave.Visible = base.CanAdd;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IF_Irrigator mdlIrrigator = new IF_Irrigator();
                IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();

                long UserID = Convert.ToInt64(Request.QueryString["UserID"]);

                mdlIrrigator.ID = UserID;
                mdlIrrigator.Name = txtIrrigatorName.Text;
                mdlIrrigator.Remarks = txtRemarks.Text;
                mdlIrrigator.MobileNo1 = Convert.ToString(txtIrrigatorMobileNo.Text);
                mdlIrrigator.MobileNo2 = Convert.ToString(txtIrrigatorMobileNo2.Text);
                mdlIrrigator.ChannelID = Convert.ToInt64(ddlChannelName.SelectedValue);
                mdlIrrigator.DivisionID = Convert.ToInt64(ddlDivision.SelectedValue);
                if (rbStatusActive.Checked == true)
                {
                    mdlIrrigator.Status = "1";
                }
                else if (rbStatusActive.Checked == false)
                {
                    mdlIrrigator.Status = "2";
                }

                if (chkFront.Checked == true)
                {
                    mdlIrrigator.TailFront = true;
                }
                else
                {
                    mdlIrrigator.TailFront = false;
                }
                if (chkLeft.Checked == true)
                {
                    mdlIrrigator.TailLeft = true;
                }
                else
                {
                    mdlIrrigator.TailLeft = false;
                }
                if (chkRight.Checked == true)
                {
                    mdlIrrigator.TailRight = true;
                }
                else
                {
                    mdlIrrigator.TailRight = false;
                }


                IF_Irrigator mdlIrrigator2 = bllIrrigator.IsMobileUnique(txtIrrigatorMobileNo.Text);
                IF_Irrigator mdlIrrigator3 = null;

                if (txtIrrigatorMobileNo2.Text.Trim() != string.Empty)
                {
                    mdlIrrigator3 = bllIrrigator.IsMobileUnique(txtIrrigatorMobileNo2.Text);
                }

                if (UserID == 0)
                {

                    if (mdlIrrigator2 != null || mdlIrrigator3 != null)
                    {
                        Master.ShowMessage(Message.UserMobileExists.Description, SiteMaster.MessageType.Error);
                        return;
                    }


                    bllIrrigator.AddIrrigator(mdlIrrigator);
                }
                else
                {
                    if (mdlIrrigator2 != null || mdlIrrigator3 != null)
                    {
                        if (mdlIrrigator2 != null && mdlIrrigator2.ID != UserID)
                        {
                            Master.ShowMessage(Message.Mobile1Exist.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        else if (mdlIrrigator3 != null && mdlIrrigator3.ID != UserID)
                        {
                            Master.ShowMessage(Message.Mobile2Exist.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }


                    bllIrrigator.UpdateIrrigator(mdlIrrigator);
                }
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                btnSave.Enabled = false;


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }

        }

        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone);
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID);
        }

        private void BindChannelDropdown(long _DivisionID)
        {
            List<GetTailofChannelinDivision_Result> lstChannelWhichDontHaveOffTakesAtTail = GetChannelWhichDontHaveOffTakesAtTail(_DivisionID);
            Dropdownlist.BindDropdownlist<List<GetTailofChannelinDivision_Result>>(ddlChannelName, lstChannelWhichDontHaveOffTakesAtTail, (int)Constants.DropDownFirstOption.Select, "ChannelName", "ChannelID");
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;

                ddlChannelName.SelectedIndex = 0;
                ddlChannelName.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    ddlDivision.Enabled = true;
                }
                ddlChannelName.SelectedIndex = 0;
                ddlChannelName.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == String.Empty)
                {
                    ddlChannelName.SelectedIndex = 0;
                    ddlChannelName.Enabled = false;
                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    BindChannelDropdown(DivisionID);
                    //ddlChannelName.DataSource = lstChannelWhichDontHaveOffTakesAtTail;
                    //ddlChannelName.DataTextField = "ChannelName";
                    //ddlChannelName.DataValueField = "ChannelID";
                    //ddlChannelName.DataBind();
                    ddlChannelName.Enabled = true;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function return all channels Which Dont Have Off Takes At Tail.
        /// Created On:10/05/2016
        /// </summary>
        /// <returns>List<GetChannelOfDivision_Result></returns>
        public List<GetTailofChannelinDivision_Result> GetChannelWhichDontHaveOffTakesAtTail(long _DivisionID)
        {
            IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();
            //long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            List<GetTailofChannelinDivision_Result> lstChannelNames = bllIrrigator.GetChannelsByDivisionID(_DivisionID);

            //lstChannelNames = lstChannelNames.Where(lsn => !bllDailyData.HasOffTake((long)lsn.GaugeID)).ToList();

            return lstChannelNames;
            //CO_ChannelGauge mdlGaugeIDs = null;
            //GetChannelOfDivision_Result mdlChannel = new GetChannelOfDivision_Result();
            //bool OffTakes;
            //List<GetChannelOfDivision_Result> lstChannelWhichDontHaveOffTakesAtTail = new List<GetChannelOfDivision_Result>();


            //for (int i = 0; i < lstChannelNames.Count; i++)
            //{
            //    //mdlGaugeIDs = bllIrrigator.GetGaugeByChannelID(Convert.ToInt64(lstChannelNames.ElementAt(i).ChannelID));
            //    OffTakes = new DailyDataBLL().HasOffTake(Convert.ToInt64(lstChannelNames.ElementAt(i).GaugeID));
            //    //if (!OffTakes)
            //    //{
            //    lstChannelWhichDontHaveOffTakesAtTail.Add(lstChannelNames[i]);
            //    //}
            //}
            //return lstChannelWhichDontHaveOffTakesAtTail;
        }

        private void GetIrrigatorByID(long _IrrigatorID)
        {
            IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();
            IF_Irrigator mdlIrrigator = bllIrrigator.GetIrrigatorByID(_IrrigatorID);
            if (mdlIrrigator != null)
            {
                string ZoneID = Convert.ToString(mdlIrrigator.CO_Division.CO_Circle.CO_Zone.ID);
                string CircleID = Convert.ToString(mdlIrrigator.CO_Division.CO_Circle.ID);
                string DivisionID = Convert.ToString(mdlIrrigator.DivisionID);
                string ChannelID = Convert.ToString(mdlIrrigator.ChannelID);

                BindZoneDropdown();
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID);

                BindCircleDropdown(Convert.ToInt64(ZoneID));
                Dropdownlist.SetSelectedValue(ddlCircle, CircleID);
                ddlCircle.Enabled = true;

                BindDivisionDropdown(Convert.ToInt64(CircleID));
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
                ddlDivision.Enabled = true;

                BindChannelDropdown(Convert.ToInt64(DivisionID));
                Dropdownlist.SetSelectedValue(ddlChannelName, ChannelID);
                ddlChannelName.Enabled = true;

                txtIrrigatorName.Text = mdlIrrigator.Name;
                txtIrrigatorMobileNo.Text = Convert.ToString(mdlIrrigator.MobileNo1);
                txtIrrigatorMobileNo2.Text = Convert.ToString(mdlIrrigator.MobileNo2);
                txtRemarks.Text = mdlIrrigator.Remarks;

                if (mdlIrrigator.Status == "1")
                {
                    rbStatusActive.Checked = true;
                }
                else if (mdlIrrigator.Status == "2")
                {
                    rbStatusInactive.Checked = true;
                }

                if (mdlIrrigator.TailFront == true)
                {
                    chkFront.Checked = true;
                }
                if (mdlIrrigator.TailLeft == true)
                {
                    chkLeft.Checked = true;
                }
                if (mdlIrrigator.TailRight == true)
                {
                    chkRight.Checked = true;
                }
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 6/6/16 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddEditIrrigator);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}