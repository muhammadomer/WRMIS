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
    public partial class CWInfo : System.Web.UI.UserControl
    {
        public long _ID { get; set; }

        public bool isCWP { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_ID > 0)
                    LoadInfo(_ID);
            } 
        }

        private void LoadInfo(long _CWID)
        {
            tblCW.Visible = !isCWP;
            TblCWP.Visible = isCWP;

            if (isCWP)
            { 
                object objDetail = new ClosureOperationsBLL().GetCWPDetailByID(_CWID);
                if (objDetail != null)
                {
                    Type propertiesType = objDetail.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(objDetail, null);
                        if (prop.ToString().Contains("Title"))
                            lblCWP_Title.Text = propValue + "";

                        if (prop.ToString().Contains("Year"))
                            lblCWP_Year.Text = propValue + "";

                        if (prop.ToString().Contains("Division"))
                            lblCWP_Div.Text = propValue + "";
                    }
                } 
            }
            else
            {
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

                        if(prop.ToString().Contains("CWCost"))
                            lblCWCost.Text = propValue + ""; 
                    }
                } 
            }
        }
    }
}