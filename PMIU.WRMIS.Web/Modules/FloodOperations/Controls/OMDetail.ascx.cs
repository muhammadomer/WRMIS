using System;
using System.Data;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;


namespace PMIU.WRMIS.Web.Modules.FloodOperations.Controls
{
    public partial class OMDetail : System.Web.UI.UserControl
    {
        public static long? _FFPSPID;
        public static long? _CampSiteID;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (_FFPSPID == null)
                {
                    LoadFFPDetail(_FFPSPID, _CampSiteID);
                    _CampSiteID = null;
                }
                else
                {
                    LoadFFPDetail(_FFPSPID, _CampSiteID);
                    _FFPSPID = null;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadFFPDetail(long? _ID = null, long? _CampSiteID = null)
        {
            try
            {

                DataSet DS = new OnsiteMonitoringBLL().GetOMDetail(_ID, _CampSiteID, null, null, null, null, null, null);
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    lblZone.Text = DR["Zone"].ToString();
                    lblCircle.Text = DR["Circle"].ToString();
                    lblDivision.Text = DR["Division"].ToString();
                    lblinfraname.Text = DR["InfrastructureName"].ToString();
                    lblYear.Text = DR["Year"].ToString();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}