using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public partial class TawaanWorking : System.Web.UI.UserControl
    {
        public static long WTCaseID = -1;
        public static long DesID = -1;
        public static long AA = -1;
        public static bool View = false;
        public bool FromET = false; // from employee tracking screen in that case show view screns 


        public string SDOToPolice
        {
            get { return txtLetterSDOToPolice.ClientID; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetDecisiontype();
                    GetImprisonmentype();

                    if (!string.IsNullOrEmpty(Request.QueryString["ET"])) // from employee tracking 
                    {
                        FromET = true;
                    }
                    else
                        FromET = false;


                    TawaanCalculation(WTCaseID);
                    taComments.Attributes.Add("maxlength", "250");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void GetDecisiontype()
        {
            try
            {
                ddlDecisiontype.Items.Insert(0, new ListItem("Select", ""));
                ddlDecisiontype.Items.Insert(1, new ListItem("To Police", "1"));
                ddlDecisiontype.Items.Insert(2, new ListItem("Proceed US 70", "2"));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void GetImprisonmentype()
        {
            try
            {
                ddlImprisonment.Items.Insert(0, new ListItem("Select", ""));
                ddlImprisonment.Items.Insert(1, new ListItem("Yes", "1"));
                ddlImprisonment.Items.Insert(2, new ListItem("No", "0"));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void TawaanCalculation(long _WTCaseID)
        {
            try
            {
                object FineInfo = new WaterTheftBLL().GetFineCalculation(_WTCaseID);
                DesID = Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID);
                hdnDateOfChecking.Value = WaterTheftCase.IncidentDateTime.ToString("MM/dd/yyyy");
                // Show working screen acc. to designation
                if (!FromET && 
                    SessionManagerFacade.UserInformation.DesignationID != null &&
                    (
                    (SessionManagerFacade.UserInformation.DesignationID.Value == WaterTheftCase.AssignedToDesignationID && WaterTheftCase.CaseStatusID != 1)
                    ||
                    (WaterTheftCase.AssignedToDesignationID == (int)Constants.Designation.XEN && (AA == 2 || AA == 1)))
                    ) // for SE and Chief
                {
                    if (DesID == (long)Constants.Designation.SDO)  // SDO Case
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            if (Decision != "")
                            {
                                ddlDecisiontype.ClearSelection();
                                ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            }

                            if (FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo) != null)
                                txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            //else
                            //    txtDecisionDate.Text = Utility.GetFormattedDate(DateTime.Now);

                            txtDecisionDate.Enabled = false;
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");

                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";

                            if (Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo)) != "")
                                txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            else
                                txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("Fine").GetValue(FineInfo));

                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));

                            if (FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo) != null)
                                txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            else
                                txtFIRDate.Text = Utility.GetFormattedDate(DateTime.Now);

                            txtDays.Enabled = false;

                            if (FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo).ToString() != "No")
                            {
                                ddlImprisonment.ClearSelection();
                                string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                                if (Imprison != "")
                                {
                                    ddlImprisonment.Items.FindByText(Imprison).Selected = true;

                                    if (Imprison.ToUpper() == "YES")
                                    {
                                        txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                                        txtDays.Enabled = true;
                                    }
                                    else
                                        txtDays.Enabled = false;
                                }
                            }

                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            if (txtCaseToXEN.Text != "")
                            {
                                txtCaseToXEN.Enabled = false;
                                txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            }
                            else
                                txtCaseToXEN.Enabled = true;

                            if (Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo)) != "")
                                txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            else
                                txtCaseToXENDate.Text = Utility.GetFormattedDate(DateTime.Now);

                            divAppeal.Visible = false;
                            //taComments.Text = Convert.ToString(FineInfo.GetType().GetProperty("ZildarRemarks").GetValue(FineInfo));

                        }
                    }
                    else if (DesID == (long)Constants.Designation.XEN) // XEN Case
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            if (txtDecisionDate.Text == "")
                                txtDecisionDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            //txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtDecisionDate.Attributes.Add("class", "form-control");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = true;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("form-control", "form-control required");
                            txtSpecialCharges.Attributes.Add("required", "required");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            txtFIRDate.Attributes.Add("class", "form-control");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            divAppeal.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                            //  taComments.Text = Convert.ToString(FineInfo.GetType().GetProperty("SDORemarks").GetValue(FineInfo));
                        }
                    }
                    else if (DesID == (long)Constants.Designation.SE)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            txtDecisionDate.Attributes.Add("class", "form-control");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            txtFIRDate.Attributes.Add("class", "form-control");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                            hdnAA.Value = "Appeal";
                            if (AA == 2) // Case when chief is only assigning back to SDO without fine 
                                hdnAA.Value = "AssignBack";
                        }
                    }
                    else if (DesID == (long)Constants.Designation.ChiefIrrigation)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtDecisionDate.Attributes.Add("class", "form-control");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            txtCaseToXENDate.Attributes.Add("class", "form-control");
                            taComments.CssClass = taComments.CssClass.Replace("form-control multiline-no-resize", "form-control multiline-no-resize required");
                            taComments.Attributes.Add("required", "required");
                            divAppeal.Visible = true;
                            txtAmountPaidForAppeal.Enabled = true;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                            hdnAA.Value = "Appeal";
                            if (AA == 2) // Case when chief is only assigning back to SDO without fine 
                            {
                                hdnAA.Value = "AssignBack";
                                txtAmountPaidForAppeal.CssClass = txtAmountPaidForAppeal.CssClass.Replace("required", "");
                                txtAmountPaidForAppeal.Enabled = false;
                                FileUploadControlAppeal.Visible = false;
                                lblAttachmentProof.Visible = false;
                            }
                        }
                    }
                }
                else   // Show view only screens
                {
                    if ((DesID == (long)Constants.Designation.SDO || DesID == (long)Constants.Designation.SBE) && FineInfo != null)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            if (Decision != "")
                            {
                                ddlDecisiontype.ClearSelection();
                                ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            }
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.ClearSelection();
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = false;
                            taComments.Visible = false;
                            lblAddComments.Visible = false;
                            Attachment.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                        }
                    }
                    else if (DesID == (long)Constants.Designation.Ziladaar && FineInfo != null)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = false;
                            taComments.Visible = false;
                            lblAddComments.Visible = false;
                            Attachment.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                        }
                    }
                    else if (DesID == (long)Constants.Designation.XEN && FineInfo != null) // XEN Case
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.ClearSelection();
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = false;
                            taComments.Visible = false;
                            lblAddComments.Visible = false;
                            Attachment.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                        }
                    }
                    else if (DesID == (long)Constants.Designation.SE && FineInfo != null)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.ClearSelection();
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = false;
                            taComments.Visible = false;
                            lblAddComments.Visible = false;
                            Attachment.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                        }
                    }
                    else if (DesID == (long)Constants.Designation.ChiefIrrigation && FineInfo != null)
                    {
                        if (FineInfo != null)
                        {
                            string Decision = Convert.ToString(FineInfo.GetType().GetProperty("DecisionType").GetValue(FineInfo));
                            ddlDecisiontype.ClearSelection();
                            ddlDecisiontype.Items.FindByText(Decision).Selected = true;
                            ddlDecisiontype.Enabled = false;
                            ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                            txtDecisionDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("DecisionDate").GetValue(FineInfo));
                            txtDecisionDate.Enabled = false;
                            txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                            spanDateID.Attributes.Add("class", "input-group-addon");
                            txtSpecialCharges.Text = Convert.ToString(FineInfo.GetType().GetProperty("SpecialCharges").GetValue(FineInfo));
                            txtSpecialCharges.Enabled = false;
                            txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                            txtAreaBooked.Text = Convert.ToString(FineInfo.GetType().GetProperty("AreaBooked").GetValue(FineInfo));
                            txtAreaBooked.Enabled = false;
                            txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                            lblAreaBooked.Text = "Area Booked (" + Convert.ToString(FineInfo.GetType().GetProperty("AreaName").GetValue(FineInfo)) + ")";
                            txtFine.Text = Convert.ToString(FineInfo.GetType().GetProperty("ProposedFine").GetValue(FineInfo));
                            txtFine.Enabled = false;
                            txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                            txtLetterSDOToPolice.Text = Convert.ToString(FineInfo.GetType().GetProperty("LetterSDOToPolice").GetValue(FineInfo));
                            txtLetterSDOToPolice.Enabled = false;
                            txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                            txtFirNo.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRNumber").GetValue(FineInfo));
                            txtFirNo.Enabled = false;
                            txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                            txtFIRDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("FIRDate").GetValue(FineInfo));
                            txtFIRDate.Enabled = false;
                            txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                            spanFIRDate.Attributes.Add("class", "input-group-addon");
                            ddlImprisonment.ClearSelection();
                            string Imprison = Convert.ToString(FineInfo.GetType().GetProperty("Imprisonment").GetValue(FineInfo));
                            ddlImprisonment.ClearSelection();
                            ddlImprisonment.Items.FindByText(Imprison).Selected = true;
                            ddlImprisonment.Enabled = false;
                            ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                            txtDays.Text = Convert.ToString(FineInfo.GetType().GetProperty("InprisonmentDays").GetValue(FineInfo));
                            txtDays.Enabled = false;
                            txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                            txtCaseToXEN.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENNo").GetValue(FineInfo));
                            txtCaseToXEN.Enabled = false;
                            txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                            txtCaseToXENDate.Text = Convert.ToString(FineInfo.GetType().GetProperty("CaseToXENDate").GetValue(FineInfo));
                            txtCaseToXENDate.Enabled = false;
                            txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                            spanXENDate.Attributes.Add("class", "input-group-addon");
                            divAppeal.Visible = true;
                            txtAmountPaidForAppeal.Text = Convert.ToString(FineInfo.GetType().GetProperty("FeeAmount").GetValue(FineInfo));
                            txtAmountPaidForAppeal.Enabled = false;
                            txtAmountPaidForAppeal.CssClass = txtAmountPaidForAppeal.CssClass.Replace("required", "");
                            txtAmountPaidForAppeal.Attributes.Add("required", "false");
                            taComments.Visible = false;
                            lblAddComments.Visible = false;
                            Attachment.Visible = false;
                            UploadLetter.Visible = false;
                            UploadFIR.Visible = false;
                            FileUploadControlAppeal.Visible = false;
                            lblAttachmentProof.Visible = false;
                        }
                    }
                    else  // case when only canal wires are entered by SDO and SBE
                    {
                        ddlDecisiontype.Enabled = false;
                        ddlDecisiontype.CssClass = ddlDecisiontype.CssClass.Replace("required", "");
                        txtDecisionDate.Enabled = false;
                        txtDecisionDate.CssClass = txtDecisionDate.CssClass.Replace("required", "");
                        spanDateID.Attributes.Add("class", "input-group-addon");
                        txtSpecialCharges.Enabled = false;
                        txtSpecialCharges.CssClass = txtSpecialCharges.CssClass.Replace("required", "");
                        txtAreaBooked.Enabled = false;
                        txtAreaBooked.CssClass = txtAreaBooked.CssClass.Replace("required", "");
                        txtFine.Enabled = false;
                        txtFine.CssClass = txtFine.CssClass.Replace("required", "");
                        txtLetterSDOToPolice.Enabled = false;
                        txtLetterSDOToPolice.CssClass = txtLetterSDOToPolice.CssClass.Replace("required", "");
                        txtFirNo.Enabled = false;
                        txtFirNo.CssClass = txtFirNo.CssClass.Replace("required", "");
                        txtFIRDate.Enabled = false;
                        txtFIRDate.CssClass = txtFIRDate.CssClass.Replace("required", "");
                        spanFIRDate.Attributes.Add("class", "input-group-addon");
                        ddlImprisonment.Enabled = false;
                        ddlImprisonment.CssClass = ddlImprisonment.CssClass.Replace("required", "");
                        txtDays.Enabled = false;
                        txtDays.CssClass = txtDays.CssClass.Replace("required", "");
                        txtCaseToXEN.Enabled = false;
                        txtCaseToXEN.CssClass = txtCaseToXEN.CssClass.Replace("required", "");
                        txtCaseToXENDate.Enabled = false;
                        txtCaseToXENDate.CssClass = txtCaseToXENDate.CssClass.Replace("required", "");
                        spanXENDate.Attributes.Add("class", "input-group-addon");
                        divAppeal.Visible = false;
                        taComments.Visible = false;
                        lblAddComments.Visible = false;
                        Attachment.Visible = false;
                        UploadLetter.Visible = false;
                        UploadFIR.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlImprisonment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Value = ddlImprisonment.SelectedItem.Value;
            if (Value != "1")
            {
                txtDays.Text = "";
                txtDays.Enabled = false;
            }
            else
            {
                txtDays.Enabled = true;
            }
        }


        //protected void gvOffender_PageIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvOffender.EditIndex = -1;
        //        //LoadWaterTheftUserControl();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        //protected void gvOffender_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        gvOffender.PageIndex = e.NewPageIndex;
        //        //LoadWaterTheftUserControl();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}


    }
}