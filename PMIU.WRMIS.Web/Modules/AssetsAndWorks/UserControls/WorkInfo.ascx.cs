using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls
{
    public partial class WorkInfo : System.Web.UI.UserControl
    {
        public long _WorkID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_WorkID> 0)
                    LoadInfo(_WorkID);
            } 
        }

        private void LoadInfo(long _CWID)
        {
            
                object  objDetail = new AssetsWorkBLL().GetWorkByID(_CWID);
                if (objDetail != null)
                {
                    Type propertiesType = objDetail.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(objDetail, null);
                        if (prop.ToString().Contains("FinancialYear"))
                            lblFinancialYear.Text = propValue + "";

                        if (prop.ToString().Contains("Division"))
                            lblDivision.Text = propValue + "";

                        if (prop.ToString().Contains("AssetWorkType"))
                            lblWorkType.Text = propValue + "";

                        if (prop.ToString().Contains("WorkName"))
                            lblWorkName.Text = propValue + "";

                        if (prop.ToString().Contains("EstimatedCost"))
                            lblECost.Text = propValue + "";
                    }
                } 
            }
    }
}