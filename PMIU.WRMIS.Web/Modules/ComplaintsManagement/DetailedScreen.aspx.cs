using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement
{
    public partial class DetailedScreen : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchActivity_ServerClick(object sender, EventArgs e)
        {
            try
            {
                liActivity.Attributes.Add("class", "active");
                Activity.Attributes.Add("class", "tab-pane active");

                liDetails.Attributes.Remove("class");
                Details.Attributes.Add("class", "tab-pane");

                liAccessibility.Attributes.Remove("class");
                Accessibility.Attributes.Add("class", "tab-pane");
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchDetails_ServerClick(object sender, EventArgs e)
        {
            try
            {
                liDetails.Attributes.Add("class", "active");
                Details.Attributes.Add("class", "tab-pane active");

                liActivity.Attributes.Remove("class");
                Activity.Attributes.Add("class", "tab-pane");

                liAccessibility.Attributes.Remove("class");
                Accessibility.Attributes.Add("class", "tab-pane");
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchAccessibility_ServerClick(object sender, EventArgs e)
        {
            try
            {
                liAccessibility.Attributes.Add("class", "active");
                Accessibility.Attributes.Add("class", "tab-pane active");

                liDetails.Attributes.Remove("class");
                Details.Attributes.Add("class", "tab-pane");

                liActivity.Attributes.Remove("class");
                Activity.Attributes.Add("class", "tab-pane");
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetComplaintByID(long _ComplaintID)
        {
            ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
            CM_Complaint mdlComplaint = bllComplaints.GetComplaintByID(_ComplaintID);

            string[] strFullName = mdlComplaint.ComplainantName.Split(' ');
            string FirstName = strFullName[0];
            string LastName = strFullName[1];

            Dropdownlist.SetSelectedValue(ddlComplaintSource, mdlComplaint.ComplaintSourceID.ToString());
            txtComplaintID.Text = mdlComplaint.ComplaintNumber;
            txtFirstName.Text = FirstName;
            txtLastName.Text = LastName;
            txtAddress.Text = mdlComplaint.Address;
            txtPhoneNo.Text = mdlComplaint.Phone;
            txtMobileNo.Text = mdlComplaint.MobilePhone;
            txtComplaintDate.Text = Convert.ToString(mdlComplaint.ComplaintDate);


        }
    }
}