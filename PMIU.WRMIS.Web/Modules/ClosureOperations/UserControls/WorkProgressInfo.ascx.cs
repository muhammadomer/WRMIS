using PMIU.WRMIS.BLL.ClosureOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls
{
    public partial class WorkProgressInfo : System.Web.UI.UserControl
    {
        public long _CWID { get; set; }
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

                    object objDetail = new ClosureOperationsBLL().GetCWDetailByID(_CWID);
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("CWPTitle"))
                                lblTitleV.Text = propValue + "";

                            if (prop.ToString().Contains("CWPYear"))
                                lblYearV.Text = propValue + "";

                            if (prop.ToString().Contains("CWPDivision"))
                                lblDivV.Text = propValue + "";

                            if (prop.ToString().Contains("CWTitle"))
                                lblNameV.Text = propValue + "";

                            if (prop.ToString().Contains("CWWorkType"))
                                lblTypeV.Text = propValue + "";
                        }
                    }

                }
                else
                {
                    tblAdd.Visible = _ShowProgress;
                    tblView.Visible = !_ShowProgress;

                    object objDetail = new ClosureOperationsBLL().GetCWDetailByID(_CWID);
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("CWPTitle"))
                                lblCW_CWPTitle.Text = propValue + "";

                            if (prop.ToString().Contains("CWPYear"))
                                lblCW_Year.Text = propValue + "";

                            if (prop.ToString().Contains("CWPDivision"))
                                lblCW_DivName.Text = propValue + "";

                            if (prop.ToString().Contains("CWTitle"))
                                lblCW_Title.Text = propValue + "";

                            if (prop.ToString().Contains("CWWorkType"))
                                lblCW_Type.Text = propValue + "";
                        }
                    }
                    //if (_IsScheduled)
                    //{
                    //    objDetail = new ClosureOperationsBLL().GetWorkProgressByUserScheduled(_CWID, _UserID, _RefMonitoringID);
                    //}
                    //else
                    //{
                        objDetail = new ClosureOperationsBLL().GetWorkProgressByUser(_CWID, _UserID);   
                    //}
                   
                    if (objDetail != null)
                    {
                        Type propertiesType = objDetail.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            object propValue = prop.GetValue(objDetail, null);
                            if (prop.ToString().Contains("Progress"))
                                lblCW_Pgrs.Text = propValue + "";

                            if (prop.ToString().Contains("Date"))
                                lblDate.Text = propValue + "";
                        }
                    } 
                }
                
            }
        }
    }
}