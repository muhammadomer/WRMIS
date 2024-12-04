using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain
{
    public partial class AddDrain : BasePage
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
                    if (!string.IsNullOrEmpty(Request.QueryString["DrainID"]))
                    {
                        Session[DrainID_VS] = Convert.ToInt64(Request.QueryString["DrainID"]);

                        FO_Drain _objDrain = new DrainBLL().GetExisitngRecord(Convert.ToInt64(Session[DrainID_VS]));
                        hdnDrainID.Value = Convert.ToString(_objDrain.ID);
                        txtDrainName.Text = _objDrain.Name;
                        txtLength.Text = Convert.ToString(_objDrain.TotalLength);
                        txtCatchmentArea.Text = Convert.ToString(_objDrain.CatchmentArea);
                        txtMajorBuildUpArea.Text = _objDrain.BuildupArea;
                        RadioButtonListStatus.SelectedIndex = RadioButtonListStatus.Items.IndexOf(RadioButtonListStatus.Items.FindByValue(_objDrain.IsActive == true ? "1" : "0"));
                        txtDesignedDischarg.Text = Convert.ToString(_objDrain.DesignedDischarge);
                        txtBedWidth.Text = Convert.ToString(_objDrain.BedWidth);
                        txtFullCapacityDepth.Text = Convert.ToString(_objDrain.FullSupplyDepth);
                        txtDescription.Text = _objDrain.Description;

                        //Function to delete drain data by Drain ID
                        // bool IsRecordDeleted = new DrainBLL().DeleteDrainDataByID(3);
                    }
                    hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/ReferenceData/Drain/SearchDrain.aspx";
                }
                if (Convert.ToInt64(Request.QueryString["DrainID"]) > 0)
                {
                    h3PageTitle.InnerText = "Edit Drain";
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                double? x;
                x = null;
                FO_Drain _objDrain = new FO_Drain();
                _objDrain.ID = Convert.ToInt64(hdnDrainID.Value);
                _objDrain.Name = txtDrainName.Text;
                _objDrain.TotalLength = Convert.ToDouble(txtLength.Text);
                _objDrain.CatchmentArea = txtCatchmentArea.Text == "" ? x : Convert.ToDouble(txtCatchmentArea.Text);
                _objDrain.BuildupArea = txtMajorBuildUpArea.Text;
                _objDrain.IsActive = (RadioButtonListStatus.SelectedItem.Value == "1") ? true : false;
                _objDrain.DesignedDischarge = Convert.ToDouble(txtDesignedDischarg.Text);
                _objDrain.BedWidth = txtBedWidth.Text == "" ? x : Convert.ToDouble(txtBedWidth.Text);
                _objDrain.FullSupplyDepth = txtFullCapacityDepth.Text == "" ? x : Convert.ToDouble(txtFullCapacityDepth.Text);
                _objDrain.Description = txtDescription.Text;
                _objDrain.DrainTypeID = 17;
                _objDrain.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                _objDrain.CreatedDate = DateTime.Today;

                var IsRecordSaved = new DrainBLL().AddDrainData(_objDrain);
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/Drain/SearchDrain.aspx?DrainID=" + _objDrain.ID, false);
                    //ClearControl(this);

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

        private void ClearControl(Control control)
        {
            try
            {
                var textbox = control as TextBox;
                if (textbox != null)
                    textbox.Text = string.Empty;



                foreach (Control childControl in control.Controls)
                {
                    ClearControl(childControl);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}