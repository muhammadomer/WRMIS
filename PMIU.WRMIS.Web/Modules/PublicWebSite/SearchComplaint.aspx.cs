using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class SearchComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divComplaintInformation.Visible = false;
            divNoResultFound.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divNoResultFound.Visible = false;
            divComplaintInformation.Visible = false;

            try
            {
                if (txtComplaintNumber.Text.Trim() != "" || txtComplainCellNumber.Text.Trim() != "")
                {

                    DataSet DS = new ComplaintsManagementBLL().GetComplaintInformation_SearchForPublicWebSite(txtComplaintNumber.Text.Trim(), txtComplainCellNumber.Text.Trim());
                    
                    string NotAvailable = "NA";

                    if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                    {
                        DataRow DR = DS.Tables[0].Rows[0];

                        if (DR["ComplaintNumber"].ToString().Trim() != "")
                        {
                            lblComplaintNumber.Text = DR["ComplaintNumber"].ToString().Trim();
                        }
                        else
                        {
                            lblComplaintNumber.Text = NotAvailable;
                        }

                        if (DR["ComplaintNumber"].ToString().Trim() != "")
                        {
                            spnStatus.InnerText = DR["ComplaintStatus"].ToString().Trim();
                            spnStatus.Style.Add("color", DR["ComplaintStatusColor"].ToString().Trim());
                        }
                        else
                        {
                            spnStatus.InnerText = NotAvailable;
                        } 
                        
                        lblComplainerName.Text = DR["ComplainantName"].ToString().Trim();
                        if (DR["ComplaintNumber"].ToString().Trim() != "")
                        {
                            lblComplaintNumber.Text = DR["ComplaintNumber"].ToString().Trim();
                        }
                        else
                        {
                            lblComplaintNumber.Text = NotAvailable;
                        } 
                        
                        if (DR["Address"].ToString().Trim() != "")
                        {
                            lblAddress.Text = DR["Address"].ToString().Trim();
                        }
                        else
                        {
                            lblAddress.Text = NotAvailable;
                        }

                        if (DR["Phone"].ToString().Trim() != "")
                        {
                            lblPhone.Text = DR["Phone"].ToString().Trim();
                        }
                        else
                        {
                            lblPhone.Text = NotAvailable;
                        }

                        if (DR["MobilePhone"].ToString().Trim() != "")
                        {
                            lblCell.Text = DR["MobilePhone"].ToString().Trim();
                        }
                        else
                        {
                            lblCell.Text = NotAvailable;
                        }

                        if (DR["ComplaintDate"].ToString().Trim() != "")
                        {
                            lblComplaintDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(DR["ComplaintDate"].ToString().Trim()));                            
                        }
                        else
                        {
                            lblComplaintDate.Text = NotAvailable;
                        }

                        if (DR["ComplaintType"].ToString().Trim() != "")
                        {
                            lblComplaintType.Text = DR["ComplaintType"].ToString().Trim();
                        }
                        else
                        {
                            lblComplaintType.Text = NotAvailable;
                        }

                        if (DR["ComplaintDomain"].ToString().Trim() != "")
                        {
                            lblComplaintDomain.Text = DR["ComplaintDomain"].ToString().Trim();
                        }
                        else
                        {
                            lblComplaintDomain.Text = NotAvailable;
                        }

                        if (DR["Structure"].ToString().Trim() != "")
                        {
                            lblStructureDivision.Text = DR["Structure"].ToString().Trim();
                        }
                        else
                        {
                            lblStructureDivision.Text = NotAvailable;
                        }

                        if (DR["StructureName"].ToString().Trim() != "")
                        {
                            lblStructureName.Text = DR["StructureName"].ToString().Trim();
                        }
                        else
                        {
                            lblStructureName.Text = NotAvailable;
                        }

                        if (DR["ComplaintDetails"].ToString().Trim() != "")
                        {
                            lblComplaintDetails.Text = DR["ComplaintDetails"].ToString().Trim();
                        }
                        else
                        {
                            lblComplaintDetails.Text = NotAvailable;
                        } 
                        
                        divNoResultFound.Visible = false;
                        divComplaintInformation.Visible = true;
                    }
                    else
                    {
                        divNoResultFound.Visible = true;
                        divComplaintInformation.Visible = false;
                    }
                }
                else
                {
                    divNoResultFound.Visible = true;
                    divNoResultFound.InnerText = "Please enter Complaint Number & Complain CellNumber both.";
                    divComplaintInformation.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
                //lblMessage.Text = ex.Message;
            }

        }

    }
}