using System;
using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.Accounts.ReferenceData;

namespace PMIU.WRMIS.Web.Modules.Accounts.Controls
{
    public partial class ResourceAllocationData : System.Web.UI.UserControl
    {
        public static Int64? _AssetID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_AssetID != null)
                {
                    LoadData(_AssetID);
                    _AssetID = null;
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }




        private void LoadData(Int64? _ID)
        {
            try
            {
                object _Data = new ReferenceDataBLL().GetResourceAllocationData(_ID);
                if (_Data != null)
                {
                    ResourceTypeText.Text = Utility.GetDynamicPropertyValue(_Data, "ResourceType");
                    DesignationText.Text = Utility.GetDynamicPropertyValue(_Data, "Designation");
                    NameofStaffText.Text = Utility.GetDynamicPropertyValue(_Data, "StaffName");
                    BPSText.Text = Utility.GetDynamicPropertyValue(_Data, "BPS");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}