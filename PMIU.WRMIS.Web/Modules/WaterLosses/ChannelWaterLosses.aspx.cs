using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace PMIU.WRMIS.Web.Modules.WaterLosses
{
    public partial class ChannelWaterLosses : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                ResetPage(); 
            }
        } 

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelWaterLosses);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void DisableDD(DropDownList DDL)
        {
            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("Select", ""));
            DDL.Enabled = false;
        }
        private void LoadMainCanal()
        {
            List<object> lstMainCanals = new WaterLossesBLL().GetMainCanals(0, false);
            Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstMainCanals);
            if (lstMainCanals != null && lstMainCanals.Count > 0)
                ddlMainCanal.Enabled = true;

            DisableDD(ddlBranch);
            DisableDD(ddlDistributry);
            DisableDD(ddlMinor);
            DisableDD(ddlSubMinor);
        }
        private void LoadAllChannels(long _UserID, long? _IrrigationLevel)
        {
            if (_IrrigationLevel == null)
                LoadMainCanal();

            int boundry = Convert.ToInt32(_IrrigationLevel);

            List<object> lstChaildCanals = new List<object>();
            List<object> lstCanal = new WaterLossesBLL().GetCanalsByUserLocation(_UserID, boundry);
            
            lstChaildCanals = (List<object>)lstCanal.ElementAt(0); //Main Canal 
            if (lstChaildCanals != null && lstChaildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                ddlMainCanal.Enabled = true;
            }

            lstChaildCanals = (List<object>)lstCanal.ElementAt(1); //Branch Canal 
            if (lstChaildCanals != null && lstChaildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlBranch, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                ddlBranch.Enabled = true;
            }

            lstChaildCanals = (List<object>)lstCanal.ElementAt(2); //Distributry Canal 
            if (lstChaildCanals != null && lstChaildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlDistributry, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                ddlDistributry.Enabled = true;
            }

            lstChaildCanals = (List<object>)lstCanal.ElementAt(3); //MInor Canal 
            if (lstChaildCanals != null && lstChaildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlMinor, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                ddlMinor.Enabled = true;
            }

            lstChaildCanals = (List<object>)lstCanal.ElementAt(4); //Sub Minor Canal 
            if (lstChaildCanals != null && lstChaildCanals.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlSubMinor, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                ddlSubMinor.Enabled = true;
            }
        }
        
        protected void btnView_Click(object sender, EventArgs e)
        {
            if (!AllParamsProvided())
                return;

            if (!VerifyDateRange())
                return;

            DateTime fromDate = Utility.GetParsedDate(txtFromDate.Text), toDate = Utility.GetParsedDate(txtToDate.Text);
            long ChannelID = SelectedChannelID();

            if (ChannelID > 0)
            {

                List<object> lstData = new WaterLossesBLL().GetChannelWaterLosses(ChannelID, fromDate, toDate);
                if (lstData != null)
                {
                    AddTableHeader();

                    Type propertiesType = lstData.ElementAt(0).GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());

                    foreach (var item in lstData)
                    {
                        TableRow row = new TableRow();
                        string rowStyle = "";
                        int count = 0;
                        foreach (PropertyInfo prop in props)
                        {
                            
                            object propValue = prop.GetValue(item, null);
                            if (propValue.ToString().Contains("Total"))
                                rowStyle = "text-bold";
                            if (prop.ToString().Contains("Channel"))
                                row.Cells.Add(GetCell(propValue + "", "text-left"));
                            else
                                row.Cells.Add(GetCell(propValue + "", "text-right"));

                            count++;
                            if(count == 3)
                                 row.Cells.Add(GetCell("", "text-left"));
                        }

                        if (!string.IsNullOrEmpty(rowStyle))
                            row.Attributes.Add("class", rowStyle);

                        tblChnlWtrLoses.Rows.Add(row);
                    }
                    showErrorDiv(false);
                }
                else
                {
                    AddTableHeader();
                    TableRow errorRow = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Text = ("No Record Found.");
                    cell.ColumnSpan = 9;
                    cell.RowSpan = 1;
                    errorRow.Cells.Add(cell);
                    tblChnlWtrLoses.Rows.Add(errorRow);
                    showErrorDiv(false);
                }
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetPage();
        }
        protected void ddlMainCanal_SelectedIndexChanged(object sender, EventArgs e)
        { 
            LoadDropDowns(ddlMainCanal.SelectedItem.Value.ToString(), 1); 
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {  
            LoadDropDowns(ddlBranch.SelectedItem.Value.ToString() , 3);
        }
        protected void ddlDistributry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDropDowns(ddlDistributry.SelectedItem.Value.ToString(), 4); 
        }
        protected void ddlMinor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDropDowns(ddlMinor.SelectedItem.Value.ToString(), 5); 
        }

        private TableCell GetHeaderCell(string _CellValue, int _ColSpan, int _RowSpan , string _Style)
        {
            TableCell cell = new TableCell();
            cell.Text = (string.IsNullOrEmpty(_CellValue) ? "" : _CellValue);
            cell.ColumnSpan = _ColSpan;
            cell.RowSpan = _RowSpan;
            cell.Attributes.Add("style" , _Style);

            return cell;
        }
        private TableCell GetCell(string _CellValue, string _CssClass)
        {
            TableCell cell = new TableCell();
            cell.Text = (string.IsNullOrEmpty(_CellValue) ? "" : _CellValue);
            cell.Attributes.Add("class", _CssClass);
            return cell;
        }
         
         
        private void AddTableHeader()
        {
            //Draw header...
            //1. fill header fields
            TableHeaderRow header_upper = new TableHeaderRow();
            //Set Css on Row level
            header_upper.Attributes.Add("class", "text-center table-header");
            header_upper.Attributes.Add("style", "border:0px;");
            //Add columns/cells in a row
            header_upper.Cells.Add(GetHeaderCell("Channel Name", 1, 2,"text-align:left"));
            header_upper.Cells.Add(GetHeaderCell("RD", 2, 1, "border-bottom: 1px solid #393939;"));
            header_upper.Cells.Add(GetHeaderCell("", 1, 1, ""));
            header_upper.Cells.Add(GetHeaderCell("Discharge", 4, 1, "border-bottom: 1px solid #393939;"));
            header_upper.Cells.Add(GetHeaderCell("Difference", 1, 2, ""));
            header_upper.Cells.Add(GetHeaderCell("% Loss", 1, 2, ""));
            //Add row to the table
            tblChnlWtrLoses.Rows.Add(header_upper);

            TableHeaderRow header_lower = new TableHeaderRow();
            //Set Css on Row level
            header_lower.Attributes.Add("class", "text-center table-header");
            //Add columns/cells in a row
            header_lower.Cells.Add(GetHeaderCell("From", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("To", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("From", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("To", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("Offtakes", 1, 1, ""));
            header_lower.Cells.Add(GetHeaderCell("Direct Outlets", 1, 1, ""));
            //Add row to the table
            tblChnlWtrLoses.Rows.Add(header_lower);
        }       
        private bool AllParamsProvided ()
        {
            if (string.IsNullOrEmpty(txtFromDate.Text) ||string.IsNullOrEmpty(txtFromDate.Text))
            {
                Master.ShowMessage(Message.FromToDateBothRequired.Description, SiteMaster.MessageType.Error);
                 
                return false;
            }

            if (ddlSubMinor.SelectedValue.Equals("") && ddlMinor.SelectedValue.Equals("") &&
                ddlDistributry.SelectedValue.Equals("") && ddlBranch.SelectedValue.Equals("") &&
                ddlMainCanal.SelectedValue.Equals(""))
            {
                Master.ShowMessage("Select a channel.", SiteMaster.MessageType.Error);
                return false;
            }
            return true;
        }

        private bool VerifyDateRange()
        {
            DateTime fromDate = Utility.GetParsedDate(txtFromDate.Text), toDate = Utility.GetParsedDate(txtToDate.Text);
            //Future date check
            if (fromDate > DateTime.Now || toDate > DateTime.Now)
            {
                Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                return false;
            }

            if (fromDate > toDate)
            {
                Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                return false;
            }
            //6 month date duration check
            TimeSpan diff = toDate.Subtract(fromDate);
            if (diff.Days > 180)
            {
                Master.ShowMessage(Message.DateRangeCannotBeMoreThan6Months.Description, SiteMaster.MessageType.Error);
                return false ;
            }
            return true;
        }

        private long SelectedChannelID()

        {
            if (ddlSubMinor.Enabled && !ddlSubMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlSubMinor.SelectedValue);
            }
            else if (ddlMinor.Enabled && !ddlMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMinor.SelectedValue);
            }
            else if (ddlDistributry.Enabled && !ddlDistributry.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlDistributry.SelectedValue);
            }
            else if (ddlBranch.Enabled && !ddlBranch.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlBranch.SelectedValue);
            }
            else if (ddlMainCanal.Enabled && !ddlMainCanal.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMainCanal.SelectedValue);
            }
            return 0;
        }        
        private void LoadDropDowns(string _strID, int _ChannelType)
        {
            string selectedId = _strID;
            if (string.IsNullOrEmpty(selectedId)) 
            {
                switch (_ChannelType)
                {
                    case 1: // MAIN Canal
                        DisableDD(ddlBranch);
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 3: // Branch Canal 
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 4: //Distributory Canal 
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        break;

                    case 5: //Minor Canal 
                        DisableDD(ddlSubMinor);
                        break;

                    default:
                        break;
                }
                return;
            }
                

            long ID = Convert.ToInt64(selectedId);
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

            List<object> _DataList = new List<object>();
            if (userID > 0 && boundryID != null)
            {
                _DataList = new WaterLossesBLL().GetCanalsByParentID(userID, true, Convert.ToInt32(boundryID), ID, _ChannelType);
            }
            else{
              _DataList =   new WaterLossesBLL().GetCanalsByParentID(0, false, 0, ID, _ChannelType);
            }
           
            List<object> lstChaildCanals = new List<object>(); 

            int BIndex = -1, DIndex = -1, MIndex = -1, SMIndex = -1;
            
            switch (_ChannelType)
            {
                case 1: // MAIN Canal
                    BIndex = 0; DIndex = 1; MIndex = 2; SMIndex = 3;
                    ddlBranch.Enabled = false; 
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false; 
                    break;

                case 3: // Branch Canal
                    BIndex = -1; DIndex = 0; MIndex = 1; SMIndex = 2;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false; 
                    break;

                case 4: //Distributory Canal
                    BIndex = -1; DIndex = -1; MIndex = 0; SMIndex = 1;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false; 
                    break;

                case 5: //Minor Canal
                    BIndex = -1; DIndex = -1; MIndex = -1; SMIndex = 0; 
                    ddlSubMinor.Enabled = false;  
                    break;

                default:
                    break;
            }

            if (BIndex != -1)
            {
                lstChaildCanals = (List<object>)_DataList.ElementAt(BIndex); //Branch canal
                if (lstChaildCanals != null && lstChaildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlBranch, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                    ddlBranch.Enabled = true;
                }
            }

            if (DIndex != -1)
            {
                lstChaildCanals = (List<object>)_DataList.ElementAt(DIndex); //Distributry Canal
                if (lstChaildCanals != null && lstChaildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlDistributry, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                    ddlDistributry.Enabled = true;
                }
            }

            if (MIndex != -1)
            {
                lstChaildCanals = (List<object>)_DataList.ElementAt(MIndex); //Minor Canal
                if (lstChaildCanals != null && lstChaildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlMinor, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                    ddlMinor.Enabled = true;
                }
            }

            if (SMIndex != -1)
            {
                lstChaildCanals = (List<object>)_DataList.ElementAt(SMIndex); //Sub Minor Canal
                if (lstChaildCanals != null && lstChaildCanals.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlSubMinor, false, (int)Constants.DropDownFirstOption.Select, lstChaildCanals);
                    ddlSubMinor.Enabled = true;
                }
            }

            switch (_ChannelType)
            {
                case 1: // MAIN Canal
                    ddlBranch.SelectedValue = "";
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 3: // Branch Canal
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 4: //Distributory Canal 
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

                case 5: //Minor Canal 
                    ddlSubMinor.SelectedValue = "";
                    break;

                default:
                    break;
            }
        } 
        private void ResetPage()
        {
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

            if (userID > 0 && boundryID != null)  // Irrigation Staff 
                LoadAllChannels(userID, boundryID); 
            else // PMIU staff or non irrigation staff
                LoadMainCanal();
             
            
            txtFromDate.Text = "";
            txtToDate.Text = "";

            showErrorDiv(false);
            tblChnlWtrLoses.Visible = false;
            lbUnits.Visible = false;
        }
        private void showErrorDiv(bool show)
        {
            divError.Visible = show;
            tblChnlWtrLoses.Visible = !show;
            lbUnits.Visible = !show;
            divData.Visible = !show;
            
        } 

    }
}