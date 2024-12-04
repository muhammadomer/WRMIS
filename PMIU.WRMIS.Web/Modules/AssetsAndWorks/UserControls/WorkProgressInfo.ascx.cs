using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls
{
    public partial class WorkProgressInfo : System.Web.UI.UserControl
    {
        public long _AWID { get; set; }
        public long _UserID { get; set; }
        public bool _ShowProgress { get; set; }
        //public bool _IsScheduled { get; set; }
        public long _RefMonitoringID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_ShowProgress)
                {
                    tblAdd.Visible = _ShowProgress;
                    tblView.Visible = !_ShowProgress;

                    object objDetail = new AssetsWorkBLL().GetCWDetailByID(_AWID);
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("AWName"))
                                lblTitleV.Text = propValue + "";

                            if (prop.ToString().Contains("FinancialYear"))
                                lblYearV.Text = propValue + "";

                            if (prop.ToString().Contains("AWWorkType"))
                                lblTypeV.Text = propValue + "";
                        }
                    }

                }
                else
                {
                    tblAdd.Visible = _ShowProgress;
                    tblView.Visible = !_ShowProgress;

                    object objDetail = new AssetsWorkBLL().GetCWDetailByID(_AWID);
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("AWName"))
                                lblWorkName.Text = propValue + "";

                            if (prop.ToString().Contains("FinancialYear"))
                                lblYear.Text = propValue + "";

                            if (prop.ToString().Contains("AWWorkType"))
                                lblAW_Type.Text = propValue + "";
                        }
                    }

                    objDetail = new AssetsWorkBLL().GetWorkProgressByUser(_AWID, _UserID);
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("Progress"))
                                lblAW_Pgrs.Text = propValue + "";

                            if (prop.ToString().Contains("Date"))
                                lblDate.Text = propValue + "";
                             if (prop.ToString().Contains("FinancialPercentage"))
                                 lblFinancial.Text = propValue + "";
                            
                        }
                    }
                }

            }
        }
    }
}