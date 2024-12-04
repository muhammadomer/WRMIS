using System;
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Modules.AssetsAndWorks;
using System.Data;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls
{
    public partial class AssetsDetail : System.Web.UI.UserControl
    {
        public static long? _AssetsID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_AssetsID != null)
                {
                    LoadAssetHeader(_AssetsID);
                    _AssetsID = null;
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadAssetHeader(long? _ID)
        {
            try
            {

                DataSet DS = new AssetsWorkBLL().GetAssetsHeader(null, _ID, null, null, null, null, null, null, null, null, null, null, null);
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    lblLevel.Text = DR["IrrigationLevelID"].ToString();
                    lbllocation.Text = DR["Location"].ToString();
                    lblCategory.Text = DR["CategoryName"].ToString();
                    lblSubcategory.Text = DR["SubCategoryName"].ToString();
                    lblName.Text = DR["AssetName"].ToString();

                    lblAssetType.Text = DR["AssetType"].ToString();
                    if (DR["LotQuantity"].ToString() != "")
                    {
                        lblQuantityText.Visible = true;
                        lblQuantity.Visible = true;
                        if (DR["Units"].ToString() != "")
                        {
                            lblUnitsText.Visible = true;
                            lblUnits.Visible = true;
                            lblUnits.Text = DR["Units"].ToString();
                        }
                        lblQuantity.Text = DR["LotQuantity"].ToString();
                        lblAvailableQuantityText.Visible = true;
                        lblAvailableQuantity.Visible = true;
                        lblAvailableQuantity.Text = DR["AvailableQuantity"].ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}