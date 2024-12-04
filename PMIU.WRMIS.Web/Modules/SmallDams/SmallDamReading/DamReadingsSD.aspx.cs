using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;

namespace PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading
{
    public partial class DamReadingsSD : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    DateTime _ReadingDate = Convert.ToDateTime(Utility.GetStringValueFromQueryString("Date", "0"));
                    hdnDate.Value = Utility.GetFormattedDate(_ReadingDate);
                    hdnDamID.Value = Utility.GetNumericValueFromQueryString("DamID", 0).ToString();
                    DamReadingsData._DAMID = Convert.ToInt64(hdnDamID.Value);
                    DamReadingsData._DATE = Convert.ToDateTime(hdnDate.Value);

                    Int64 _DamDataID = new SmallDamsBLL().DamIsExits(Convert.ToInt64(hdnDamID.Value), Convert.ToDateTime(hdnDate.Value));
                    if (_DamDataID > 0)
                    {
                        BindFormDate();
                    }


                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Save
                if (Convert.ToDouble(txtLiveStorage.Text) > 50000)
                {
                    Master.ShowMessage("Live Storage Value must be between 0 - 50000.00", SiteMaster.MessageType.Error);
                    return;
                }

                SD_SmallDamData SmallDamData = PrepareDamTypeEntity();
                Int64 damdataID = SmallDamData.ID;
                bool isSaved = new SmallDamsBLL().SaveSmallDamReading(SmallDamData);
                if (isSaved)
                {
                    if (damdataID > 0)
                    {
                        Master.ShowMessage("Record update successfully.", SiteMaster.MessageType.Success);

                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    BindFormDate();       
                }

                #endregion
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        void BindFormDate()
        {
            SmallDamsBLL smallDamBll = new SmallDamsBLL();
            Int64 _DamDataID = smallDamBll.DamIsExits(Convert.ToInt64(hdnDamID.Value), Convert.ToDateTime(hdnDate.Value));
            object smallDam = smallDamBll.GetSmallDamReadingByID(_DamDataID);

            if (smallDam != null)
            {
                txtDamLevel.Text = String.Format("{0:0.00}", smallDam.GetType().GetProperty("DamLevel").GetValue(smallDam));
                txtLiveStorage.Text = Convert.ToString(smallDam.GetType().GetProperty("LiveStorage").GetValue(smallDam));
                txtGaugeReader.Text = Convert.ToString(smallDam.GetType().GetProperty("ReaderName").GetValue(smallDam));
                txtRemarks.Text = Convert.ToString(smallDam.GetType().GetProperty("Remarks").GetValue(smallDam));
                txtDischarge.Text = Convert.ToString(smallDam.GetType().GetProperty("Discharge").GetValue(smallDam));
                lblCreatedDate.Text = Convert.ToString(smallDam.GetType().GetProperty("CreatedDate").GetValue(smallDam));
            }

        }

        SD_SmallDamData PrepareDamTypeEntity()
        {
            SD_SmallDamData smallDam = new SD_SmallDamData();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            Int64 _DamDataID = new SmallDamsBLL().DamIsExits(Convert.ToInt64(hdnDamID.Value), Convert.ToDateTime(hdnDate.Value));
            if (_DamDataID == 0)
            {
                smallDam.ID = 0;
                smallDam.ModifiedBy = null;
                smallDam.ModifiedDate = null;
                smallDam.CreatedDate = DateTime.Now;
                smallDam.CreatedBy = Convert.ToInt32(mdlUser.ID);
            }
            else
            {
                smallDam.ID = _DamDataID;
                smallDam.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                smallDam.ModifiedDate = DateTime.Now;
                smallDam.CreatedDate = Convert.ToDateTime(lblCreatedDate.Text);
            }

            smallDam.SmallDamID = Convert.ToInt64(hdnDamID.Value);
            
            if (txtDamLevel.Text != "")
            smallDam.DamLevel = Convert.ToDouble(txtDamLevel.Text);
            if (txtDischarge.Text != "")
            smallDam.Discharge = Convert.ToDouble(txtDischarge.Text);
            if (txtLiveStorage.Text != "")
            smallDam.LiveStorage = Convert.ToDouble(txtLiveStorage.Text);
            if (hdnDate.Value != "0")
            smallDam.ReadingDate = Convert.ToDateTime(hdnDate.Value);
            if (txtGaugeReader.Text != "")
            smallDam.ReaderName = Convert.ToString(txtGaugeReader.Text);
            if (txtRemarks.Text != "")
            smallDam.Remarks = Convert.ToString(txtRemarks.Text);

            return smallDam;
        }
        #endregion
        

        
    }
}