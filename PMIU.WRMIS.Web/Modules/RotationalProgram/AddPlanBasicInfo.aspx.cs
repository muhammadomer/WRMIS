using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.RotaionalProgram;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class AddPlanBasicInfo : BasePage
    {
        static string PlanName = " Rotational Program for ";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());
                    ddlGroups.DataSource = CommonLists.GetNoOfGroupsDropDown();
                    ddlGroups.DataTextField = "Name";
                    ddlGroups.DataValueField = "ID";
                    ddlGroups.DataBind();
                    txtYear.Text = DateTime.Now.Year.ToString();

                    if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.Circle)
                        SELoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));
                    else if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.Division)
                        XENLoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));
                    else if (SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID == (long?)Constants.IrrigationLevelID.SubDivision)
                        SDOLoggedUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID));

                    if (Request.QueryString["CloneID"] != null)
                        GetClonePlanDeatil(Convert.ToInt64(Request.QueryString["CloneID"]));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void GetClonePlanDeatil(long _CloneID)
        {
            try
            {
                RP_RotationalProgram objPlan = new RotationalProgramBLL().GetClonePlanDetail(_CloneID);
                if (objPlan != null)
                {
                    ddlSeason.SelectedValue = objPlan.SeasonID.ToString();
                    ddlSeason.Enabled = false;
                    ddlGroups.SelectedValue = objPlan.NoOfGroup.ToString();
                    ddlGroups.Enabled = false;
                    cbPriority.Checked = (bool)objPlan.IsPriority;
                    cbPriority.Enabled = false;

                    if (objPlan.SeasonID == (long)Constants.Seasons.Rabi)
                    {
                        if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2 || DateTime.Now.Month == 3)
                            txtYear.Text = Convert.ToString(DateTime.Now.Year - 1) + "-" + (DateTime.Now.Year % 100);
                        else
                            txtYear.Text = Convert.ToString(DateTime.Now.Year) + "-" + ((DateTime.Now.Year + 1) % 100);

                        divClosure.Visible = true;
                        txtClosureStartDate.Text = objPlan.ClosureStartDate.Value.ToString();
                        txtClosureEndDate.Text = objPlan.ClosureEndDate.Value.ToString();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void SELoggedUser(long _CircleID)
        {
            try
            {
                List<CO_Circle> lstCircle = new List<CO_Circle> { new CircleBLL().GetByID(_CircleID) };
                ddlCircle.DataSource = lstCircle;
                ddlCircle.DataValueField = "ID";
                ddlCircle.DataTextField = "Name";
                ddlCircle.DataBind();

                string ZoneName = new RotationalProgramBLL().GetZoneName(lstCircle.FirstOrDefault().ZoneID);
                txtName.Text = PlanName + lstCircle.FirstOrDefault().Name.ToString() + " Circle " + "(" + ZoneName + ") ";

                txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                ddlDivision.Enabled = false;
                ddlSubDivision.Enabled = false;
                RBDivision.Enabled = false;
                RBSubDivision.Enabled = false;
                cbPriority.Enabled = false;
                divShowHide.Visible = false;
                divPriority.Visible = false;
                RBCircle.Checked = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void XENLoggedUser(long _DivisionID)
        {
            try
            {
                List<CO_Division> lstDivisions = new List<CO_Division> { new DivisionBLL().GetByID(_DivisionID) };
                ddlDivision.DataSource = lstDivisions;
                ddlDivision.DataValueField = "ID";
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataBind();
                ddlDivision.CssClass = ddlDivision.CssClass.Replace("form-control", "form-control required");
                ddlDivision.Attributes.Add("required", "required");

                long CircleID = new CircleBLL().GetCircleIDByDivisionID(_DivisionID);
                List<CO_Circle> lstCircles = new List<CO_Circle> { new CircleBLL().GetByID(CircleID) };
                ddlCircle.DataSource = lstCircles;
                ddlCircle.DataValueField = "ID";
                ddlCircle.DataTextField = "Name";
                ddlCircle.DataBind();

                txtName.Text = PlanName + lstDivisions.FirstOrDefault().Name.ToString() + " Division " + "(" + lstCircles.FirstOrDefault().Name.ToString() + ") ";
                txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                ddlSubDivision.Enabled = false;
                RBSubDivision.Enabled = false;
                RBCircle.Enabled = false;
                RBDivision.Checked = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SDOLoggedUser(long _SubDivisionID)
        {
            try
            {
                List<CO_SubDivision> lstSubDivision = new List<CO_SubDivision> { new SubDivisionBLL().GetByID(_SubDivisionID) };
                ddlSubDivision.DataSource = lstSubDivision;
                ddlSubDivision.DataValueField = "ID";
                ddlSubDivision.DataTextField = "Name";
                ddlSubDivision.DataBind();
                ddlSubDivision.CssClass = ddlSubDivision.CssClass.Replace("form-control", "form-control required");
                ddlSubDivision.Attributes.Add("required", "required");

                long DivisionID = new DivisionBLL().GetDivisionIDBySubDivisionID(_SubDivisionID);
                List<CO_Division> lstDivisions = new List<CO_Division> { new DivisionBLL().GetByID(DivisionID) };
                ddlDivision.DataSource = lstDivisions;
                ddlDivision.DataValueField = "ID";
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataBind();
                ddlDivision.CssClass = ddlDivision.CssClass.Replace("form-control", "form-control required");
                ddlDivision.Attributes.Add("required", "required");

                long CircleID = new CircleBLL().GetCircleIDByDivisionID(DivisionID);
                ddlCircle.DataSource = new List<CO_Circle> { new CircleBLL().GetByID(CircleID) };
                ddlCircle.DataValueField = "ID";
                ddlCircle.DataTextField = "Name";
                ddlCircle.DataBind();

                txtName.Text = PlanName + lstSubDivision.FirstOrDefault().Name.ToString() + " SubDivision " + "(" + lstDivisions.FirstOrDefault().Name.ToString() + ") ";
                txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                RBDivision.Enabled = false;
                RBCircle.Enabled = false;
                RBSubDivision.Checked = true;
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
                RP_RotationalProgram objSave = new RP_RotationalProgram();
                objSave.Name = txtName.Text;
                objSave.SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                objSave.ProgramStartDate = Convert.ToDateTime(txtDate.Text);
                objSave.IsPriority = false;

                if (Convert.ToInt64(ddlSeason.SelectedItem.Value) == (int)Constants.Seasons.Rabi)
                {
                    if (!string.IsNullOrEmpty(txtClosureStartDate.Text) && !string.IsNullOrEmpty(txtClosureEndDate.Text))
                    {
                        objSave.ClosureStartDate = Convert.ToDateTime(txtClosureStartDate.Text);
                        objSave.ClosureEndDate = Convert.ToDateTime(txtClosureEndDate.Text);

                        if (objSave.ClosureEndDate < objSave.ClosureStartDate)
                        {
                            Master.ShowMessage(Message.ClosureStartEndDate.Description, SiteMaster.MessageType.Error);
                            return;
                        }

                        if (objSave.ClosureStartDate < objSave.ProgramStartDate)
                        {
                            Master.ShowMessage(Message.ClosureStartDateCannotBeLess.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                }

                objSave.RPYear = txtYear.Text.ToString();
                objSave.NoOfGroup = Convert.ToInt16(ddlGroups.SelectedItem.Value);
                objSave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                objSave.CreatedDate = DateTime.Now;

                if (RBCircle.Checked == true)
                {
                    objSave.IrrigationLevelID = (int)Constants.IrrigationLevelID.Circle;
                    objSave.IrrigationBoundaryID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                else if (RBDivision.Checked == true)
                {
                    objSave.IrrigationLevelID = (int)Constants.IrrigationLevelID.Division;
                    objSave.IrrigationBoundaryID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    if (cbPriority.Checked == true)
                        objSave.IsPriority = true;
                }
                else if (RBSubDivision.Checked == true)
                {
                    objSave.IrrigationLevelID = (int)Constants.IrrigationLevelID.SubDivision;
                    objSave.IrrigationBoundaryID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                    if (cbPriority.Checked == true)
                        objSave.IsPriority = true;
                }

                string PlanCount = new RotationalProgramBLL().GetPlanCount(objSave.IrrigationLevelID, objSave.IrrigationBoundaryID, objSave.SeasonID, objSave.RPYear);
                if (PlanCount != "")
                {
                    objSave.Name = objSave.Name + PlanCount;

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        long Result = new RotationalProgramBLL().AddPlan(objSave);
                        if (Result != -1)
                        {
                            RP_Attachment Attachment = new RP_Attachment();
                            List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.RotationalProgram);
                            if (lstNameofFiles.Count > 0)
                            {
                                Attachment.RPID = Result;
                                Attachment.FileURL = lstNameofFiles[0].Item3;
                                Attachment.CreatedDate = DateTime.Now;
                                Attachment.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                                new RotationalProgramBLL().SaveAttachments(Attachment);
                            }

                            RP_Approval objApproval = new RP_Approval();
                            objApproval.RPID = Result;
                            objApproval.DesignationFromID = Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID);
                            objApproval.DesignationToID = Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID);
                            objApproval.Status = Constants.RP_Draft;
                            objApproval.CreatedDate = DateTime.Now;
                            objApproval.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                            objApproval.ModifiedDate = DateTime.Now;
                            objApproval.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                            new RotationalProgramBLL().SetApprovelStatus(objApproval, false);

                            //////////////////  Clone Work  /////////////////
                            bool Reslt = false;
                            if (Request.QueryString["CloneID"] != null)
                            {
                                Reslt = new RotationalProgramBLL().SaveCloneDraftData(Convert.ToInt64(Request.QueryString["CloneID"]), Result, Convert.ToInt32(Session[SessionValues.UserID]), objSave.IrrigationLevelID);
                            }
                            else
                                Reslt = true;
                            ////////////////// End /////////////////////////

                            transaction.Complete();

                            if (Reslt)
                            {
                                if (RBCircle.Checked == true)
                                    Response.RedirectPermanent((String.Format("AddPlanDetail.aspx?ID={0}&Msg={1}", Result, true)), false);
                                else
                                    Response.RedirectPermanent((String.Format("AddPlanDivSubDivDetail.aspx?ID={0}&Msg={1}", Result, true)), false);
                            }
                            else
                                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                        }
                        else
                            Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                    Master.ShowMessage(Message.DraftsLimit.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value != "" && Convert.ToInt64(ddlSeason.SelectedItem.Value) == (int)Constants.Seasons.Rabi)
                {
                    divClosure.Visible = true;
                    //txtClosureStartDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2 || DateTime.Now.Month == 3)
                        txtYear.Text = Convert.ToString(DateTime.Now.Year - 1) + "-" + (DateTime.Now.Year % 100);
                    else
                        txtYear.Text = Convert.ToString(DateTime.Now.Year) + "-" + ((DateTime.Now.Year + 1) % 100);
                }
                else
                {
                    divClosure.Visible = false;
                    txtYear.Text = DateTime.Now.Year.ToString();
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public string GetYear()
        {
            string Year = "";

            if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 9)
                Year = DateTime.Now.Year.ToString();
            else if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2 || DateTime.Now.Month == 3)
                Year = Convert.ToString(DateTime.Now.Year - 1) + "-" + (DateTime.Now.Year % 100);
            else
                Year = Convert.ToString(DateTime.Now.Year) + "-" + ((DateTime.Now.Year + 1) % 100);
            return Year;
        }

        public long GetSeason()
        {
            long Season = 1;

            if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 9)
                Season = 2;

            return Season;
        }
    }
}