using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain
{
    public partial class OutfallDetailsDrain : BasePage
    {
        #region ViewState Constants

        public const string DrainID_VS = "DrainID";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindOutfallSideDropDown();
                    BindOutfallTypeDropDown();

                    if (!string.IsNullOrEmpty(Request.QueryString["DrainID"]))
                    {
                        Session[DrainID_VS] = Convert.ToInt64(Request.QueryString["DrainID"]);

                        BindDrainDataByID(Convert.ToInt64(Session[DrainID_VS]));
                        bool IsOutfallRecordExists = new DrainBLL().IsDrainRecordExists(Convert.ToInt64(Session[DrainID_VS]));
                        if (IsOutfallRecordExists)
                        {
                            GetExistingDrainOutfallRecord();

                        }
                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/ReferenceData/Drain/SearchDrain.aspx";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Dropdowns"

        /// <summary>
        /// This function binds Outfall Side dropdown
        /// Created on 09-28-2016
        /// </summary>
        private void BindOutfallSideDropDown()
        {
            try
            {
                ddlOutfallSide.DataSource = CommonLists.GetOutfallSide();
                ddlOutfallSide.DataTextField = "Name";
                ddlOutfallSide.DataValueField = "ID";
                ddlOutfallSide.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds Outfall Type dropdown
        /// Created on 09-28-2016
        /// </summary>
        private void BindOutfallTypeDropDown()
        {
            try
            {
                ddlOutfallType.DataSource = CommonLists.GetOutfallType();
                ddlOutfallType.DataTextField = "Name";
                ddlOutfallType.DataValueField = "ID";
                ddlOutfallType.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 09-28-2016
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone);

        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 09-28-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            //Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
            List<dynamic> CircleList = new List<dynamic>()
            {
                new { ID="", Name="Select"}


            };
            List<dynamic> data = new DrainBLL().GetCircleForDrainDomain(_ZoneID);
            CircleList.AddRange(data);
            ddlCircle.DataSource = CircleList;
            ddlCircle.DataTextField = "Name";
            ddlCircle.DataValueField = "ID";
            ddlCircle.DataBind();

        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 09-28-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 09-28-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        private void BindSubDivisionDropdown(long _DivisionID)
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, _DivisionID);
        }


        /// <summary>
        /// This function binds outfall Name Dropdown
        /// Created on 09-29-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        private void BindOutfallNameDropdown(bool IsRiver, long _ID, int Type)
        {
            List<dynamic> List = new List<dynamic>()
            {
                new { ID="", Name="Select"}
            };
            if (IsRiver)
            {
                List<dynamic> data = new DrainBLL().GetRiversNameByStructureType();
                List.AddRange(data);
            }
            else if (_ID != 0)
            {
                List<dynamic> data = new DrainBLL().GetDrainsByID(_ID, Type);
                List.AddRange(data);
            }
            ddlOutfallName.DataSource = List;
            ddlOutfallName.DataTextField = "Name";
            ddlOutfallName.DataValueField = "ID";
            ddlOutfallName.Enabled = true;
            ddlOutfallName.CssClass = "form-control required";
            ddlOutfallName.DataBind();
        }
        protected void ddlOutfallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long OutfallTypeID = ddlOutfallType.SelectedItem.Value == "" ? 0 : Convert.ToInt64(ddlOutfallType.SelectedItem.Value);
                if (OutfallTypeID == 1)
                {
                    ddlZone.SelectedIndex = 0;
                    ddlCircle.SelectedIndex = 0;
                    ddlDivision.SelectedIndex = 0;
                    ddlSubDivision.SelectedIndex = 0;
                    ddlZone.Enabled = false;
                    ddlZone.CssClass = "aspNetDisabled form-control";
                    ddlCircle.Enabled = false;
                    ddlCircle.CssClass = "aspNetDisabled form-control";
                    ddlDivision.Enabled = false;
                    ddlDivision.CssClass = "aspNetDisabled form-control";
                    ddlSubDivision.Enabled = false;
                    ddlSubDivision.CssClass = "aspNetDisabled form-control";
                    BindOutfallNameDropdown(true, 0, 0);
                }
                else if (OutfallTypeID == 2)
                {
                    ddlZone.Enabled = true;
                    ddlZone.CssClass = "form-control required";
                    BindZoneDropdown();
                    BindOutfallNameDropdown(false, 0, 0);

                }
                else
                {
                    ddlZone.Enabled = false;
                    ddlZone.CssClass = "aspNetDisabled form-control";
                    ddlCircle.Enabled = false;
                    ddlCircle.CssClass = "aspNetDisabled form-control";
                    ddlDivision.Enabled = false;
                    ddlDivision.CssClass = "aspNetDisabled form-control";
                    ddlSubDivision.Enabled = false;
                    ddlSubDivision.CssClass = "aspNetDisabled form-control";
                    ddlOutfallName.Enabled = false;
                    ddlOutfallName.CssClass = "aspNetDisabled form-control";
                    ddlOutfallName.SelectedIndex = 0;
                    ddlZone.SelectedIndex = 0;
                    ddlCircle.SelectedIndex = 0;
                    ddlDivision.SelectedIndex = 0;
                    ddlSubDivision.SelectedIndex = 0;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                    ddlCircle.CssClass = "aspNetDisabled form-control";
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    if (ddlOutfallType.SelectedIndex == 1 || ddlOutfallType.SelectedIndex == 2)
                    {
                        BindOutfallNameDropdown(false, ZoneID, (int)Constants.IrrigationLevelID.Zone);
                    }
                    ddlCircle.Enabled = true;
                    ddlCircle.CssClass = "form-control required";
                }
                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;
                ddlDivision.CssClass = "aspNetDisabled form-control";
                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;
                ddlSubDivision.CssClass = "aspNetDisabled form-control";


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
                    ddlDivision.CssClass = "aspNetDisabled form-control";
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    if (ddlOutfallType.SelectedIndex == 1 || ddlOutfallType.SelectedIndex == 2)
                    {
                        BindOutfallNameDropdown(false, CircleID, (int)Constants.IrrigationLevelID.Circle);
                    }
                    ddlDivision.Enabled = true;
                    ddlDivision.CssClass = "form-control required";

                }
                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;
                ddlSubDivision.CssClass = "aspNetDisabled form-control";


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
                    ddlSubDivision.SelectedIndex = 0;
                    ddlSubDivision.Enabled = false;
                    ddlSubDivision.CssClass = "aspNetDisabled form-control";
                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    BindSubDivisionDropdown(DivisionID);
                    if (ddlOutfallType.SelectedIndex == 1 || ddlOutfallType.SelectedIndex == 2)
                    {
                        BindOutfallNameDropdown(false, DivisionID, (int)Constants.IrrigationLevelID.Division);
                    }
                    ddlSubDivision.Enabled = true;
                    ddlSubDivision.CssClass = "form-control required";
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                if (ddlOutfallType.SelectedIndex == 1 || ddlOutfallType.SelectedIndex == 2)
                {
                    BindOutfallNameDropdown(false, SubDivisionID, (int)Constants.IrrigationLevelID.SubDivision);
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }



        }
        #endregion

        /// <summary>
        /// This function binds user control data.
        /// Created on 09-29-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        private void BindDrainDataByID(long _DrainID)
        {
            try
            {
                //dynamic DrainData = new DrainBLL().GetDrainDataByID(_DrainID);
                //string DrainNAme = DrainData.GetType().GetProperty("Name").GetValue(DrainData, null);
                //double TotalLength = DrainData.GetType().GetProperty("TotalLength").GetValue(DrainData, null);
                //double? CatchmentArea = DrainData.GetType().GetProperty("CatchmentArea").GetValue(DrainData, null);
                //string MajorBuildupArea = DrainData.GetType().GetProperty("BuildupArea").GetValue(DrainData, null);
                //bool DrainStatus = DrainData.GetType().GetProperty("IsActive").GetValue(DrainData, null);
                //IrrigationNetwork.Controls.DrainDetails.DrainName = DrainNAme;
                //IrrigationNetwork.Controls.DrainDetails.TotalLength = Convert.ToString(TotalLength);
                //IrrigationNetwork.Controls.DrainDetails.CatchmentArea = Convert.ToString(CatchmentArea);
                //IrrigationNetwork.Controls.DrainDetails.MajorBuildupArea = MajorBuildupArea;
                //IrrigationNetwork.Controls.DrainDetails.DrainStatus = (Convert.ToString(DrainStatus) == "true" ? "Active" : "Inactive");

                FO_Drain DrainData = new DrainBLL().GetDrainDataByID(_DrainID);
                IrrigationNetwork.Controls.DrainDetails.DrainName = DrainData.Name;
                IrrigationNetwork.Controls.DrainDetails.TotalLength = Convert.ToString(DrainData.TotalLength);
                IrrigationNetwork.Controls.DrainDetails.CatchmentArea = Convert.ToString(DrainData.CatchmentArea);
                IrrigationNetwork.Controls.DrainDetails.MajorBuildupArea = DrainData.BuildupArea;
                IrrigationNetwork.Controls.DrainDetails.DrainStatus = DrainData.IsActive == true ? "Active" : "Inactive";

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function saves\update outfall details.
        /// Created on 09-29-2016
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                FO_DrainOutfall OutfallObj = new FO_DrainOutfall();
                OutfallObj.ID = Convert.ToInt32(hdnDrainOutfallID.Value);
                OutfallObj.DrainID = Convert.ToInt64(Session[DrainID_VS]);
                OutfallObj.OutfallType = Convert.ToString(ddlOutfallType.SelectedItem);
                OutfallObj.CreatedDate = DateTime.Now;
                OutfallObj.CreatedBy = SessionManagerFacade.UserInformation.ID;
                OutfallObj.SubDivID = ddlSubDivision.SelectedIndex == 0 ? (long?)null : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                OutfallObj.OutfallID = ddlOutfallName.SelectedIndex == 0 ? (long?)null : Convert.ToInt64(ddlOutfallName.SelectedItem.Value);
                OutfallObj.OutfallSide = ddlOutfallSide.SelectedItem.Text == "Right" ? "R" : "L";
                OutfallObj.OutfallRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

                bool IsSaved = new DrainBLL().SaveDrainOutfallData(OutfallObj);
                if (IsSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    //ClearControl(this);
                    //BindZoneDropdown();
                    //ddlZone.SelectedIndex = 0;
                    //ddlZone.Enabled = false;
                    //ddlZone.CssClass = "aspNetDisabled form-control";
                    //ddlCircle.Enabled = false;
                    //ddlCircle.CssClass = "aspNetDisabled form-control";
                    //ddlDivision.Enabled = false;
                    //ddlDivision.CssClass = "aspNetDisabled form-control";
                    //ddlSubDivision.Enabled = false;
                    //ddlSubDivision.CssClass = "aspNetDisabled form-control";
                    //ddlOutfallName.Enabled = false;
                    //ddlOutfallName.CssClass = "aspNetDisabled form-control";
                    //btnSave.CssClass += " disabled";
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This function gets existing drain outfall record.
        /// Created on 09-29-2016
        /// </summary>
        public void GetExistingDrainOutfallRecord()
        {
            try
            {
                FO_DrainOutfall OutfallData = new DrainBLL().GetOutfallExistingRecordByDrainID(Convert.ToInt64(Session[DrainID_VS]));
                hdnDrainOutfallID.Value = Convert.ToString(OutfallData.ID);
                if (null == OutfallData.SubDivID)
                {
                    ddlZone.Enabled = false;
                    ddlZone.CssClass = "aspNetDisabled form-control";
                    ddlCircle.Enabled = false;
                    ddlCircle.CssClass = "aspNetDisabled form-control";
                    ddlDivision.Enabled = false;
                    ddlDivision.CssClass = "aspNetDisabled form-control";
                    ddlSubDivision.Enabled = false;
                    ddlSubDivision.CssClass = "aspNetDisabled form-control";
                    BindOutfallNameDropdown(true, 0, 0);
                    Dropdownlist.SetSelectedValue(ddlOutfallName, Convert.ToString(OutfallData.OutfallID));

                }
                else
                {
                    ddlZone.Enabled = true;
                    ddlZone.CssClass = "form-control required";
                    ddlCircle.Enabled = true;
                    ddlCircle.CssClass = "form-control required";
                    ddlDivision.Enabled = true;
                    ddlDivision.CssClass = "form-control required";
                    ddlSubDivision.Enabled = true;
                    ddlSubDivision.CssClass = "form-control required";
                    object SelectedDropdownsIDS = new DrainBLL().GetSelectedDropdownsHeirarchyIDs(OutfallData.SubDivID);
                    BindZoneDropdown();
                    Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(SelectedDropdownsIDS.GetType().GetProperty("ZoneID").GetValue(SelectedDropdownsIDS)));
                    BindCircleDropdown(Convert.ToInt64(SelectedDropdownsIDS.GetType().GetProperty("ZoneID").GetValue(SelectedDropdownsIDS)));
                    Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(SelectedDropdownsIDS.GetType().GetProperty("CircleID").GetValue(SelectedDropdownsIDS)));
                    BindDivisionDropdown(Convert.ToInt64(SelectedDropdownsIDS.GetType().GetProperty("CircleID").GetValue(SelectedDropdownsIDS)));
                    Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(SelectedDropdownsIDS.GetType().GetProperty("DivisionID").GetValue(SelectedDropdownsIDS)));
                    BindSubDivisionDropdown(Convert.ToInt64(SelectedDropdownsIDS.GetType().GetProperty("DivisionID").GetValue(SelectedDropdownsIDS)));
                    Dropdownlist.SetSelectedValue(ddlSubDivision, Convert.ToString(OutfallData.SubDivID));
                    BindOutfallNameDropdown(false, OutfallData.SubDivID.Value, (int)Constants.IrrigationLevelID.SubDivision);
                    Dropdownlist.SetSelectedValue(ddlOutfallName, Convert.ToString(OutfallData.OutfallID));
                }
                BindOutfallSideDropDown();
                Dropdownlist.SetSelectedText(ddlOutfallSide, OutfallData.OutfallSide == "R" ? "Right" : "Left");
                BindOutfallTypeDropDown();
                Dropdownlist.SetSelectedText(ddlOutfallType, OutfallData.OutfallType);
                Tuple<string, string> tuple = Calculations.GetRDValues(OutfallData.OutfallRD);
                txtFromRDLeft.Text = tuple.Item1;
                txtFromRDRight.Text = tuple.Item2;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function clear all controls after saving process.
        /// Created on 09-29-2016
        /// </summary>
        private void ClearControl(Control control)
        {
            var textbox = control as TextBox;
            if (textbox != null)
                textbox.Text = string.Empty;
            var dropDownList = control as DropDownList;
            if (dropDownList != null)
                dropDownList.SelectedIndex = 0;


            foreach (Control childControl in control.Controls)
            {
                ClearControl(childControl);
            }


        }

    }
}