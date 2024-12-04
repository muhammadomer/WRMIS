using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.EntitlementDelivery;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
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

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class SearchEntitlements : BasePage
    {
        #region GridIndex

        public const int ChannelIDIndex = 0;
        public const int SeasonIndex = 1;
        public const int YearIndex = 2;
        public const int ParentChildIndex = 3;

        #endregion

        public char ParentChild = 'P';
        double MAF = 0;
        string MaskedValue = "";
        public long RPCChannelID = 784;
        public long SubLinkChannelID = 1059;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    GetCommandType();
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlSeason, CommonLists.GetSeasonDropDown());

                    //ResetPage();
                }

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
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

        protected void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                //int boundryID = Convert.ToInt32(SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID);
                long? boundryID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                long CommandID = ddlCommand.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCommand.SelectedItem.Value);
                //if (CommandID != -1)
                //{
                if (userID > 0 && boundryID != null)
                {
                    Dropdownlist.DDLGetCannalSystemByCommandTypeID(ddlCanalSystem, userID, true, boundryID, CommandID);

                }
                else
                {
                    Dropdownlist.DDLGetCannalSystemByCommandTypeID(ddlCanalSystem, userID, false, boundryID, CommandID);
                }
                ddlCanalSystem.Enabled = true;
                ddlCanalSystem.SelectedIndex = 0;
                ddlSeason.SelectedIndex = 0;
                //}
                //else
                //{
                //DisableDD(ddlCanalSystem);
                DisableDD(ddlMainCanal);
                DisableDD(ddlBranch);
                DisableDD(ddlDistributry);
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);
                DisableDD(ddlYear);

                dvMainSub.Visible = false;
                dvBranchSub.Visible = false;
                dvDistSub.Visible = false;
                dvMinorSub.Visible = false;
                dvSMinorSub.Visible = false;
                //}
                ddlMainCanal.Visible = true;
                gvSearchEntitlements.Visible = false;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCanalSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisableDD(ddlMainCanal);
                DisableDD(ddlBranch);
                DisableDD(ddlDistributry);
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);
                LoadDropDowns(ddlCanalSystem.SelectedItem.Value.ToString(), 0, "Main");

                bool CallAgain = false;

                //string Canal = ddlCanalSystem.SelectedItem.Text.ToString().Trim().ToLower();
                long ChannelID = 0;
                if (ddlCanalSystem.SelectedItem.Text != "Select")
                    ChannelID = new WaterLossesBLL().GetCommandChannelID(Convert.ToInt64(ddlCanalSystem.SelectedItem.Value));

                if (ddlMainCanal.Items.Count == 2)
                {
                    foreach (ListItem item in ddlMainCanal.Items)
                    {
                        if (item.Text != "Select")
                        {
                            long MainChannelID = Convert.ToInt64(item.Value);
                            if (MainChannelID == ChannelID)
                            {
                                CallAgain = true;
                                break;
                            }
                        }
                    }
                }

                if (CallAgain)    // if (ddlMainCanal.Items.Count == 2 && itemExist != null) // select and only one main canal  -- jugad 
                {
                    DisableDD(ddlBranch);
                    DisableDD(ddlDistributry);
                    DisableDD(ddlMinor);
                    DisableDD(ddlSubMinor);
                    ddlBranch.ClearSelection();
                    ddlDistributry.ClearSelection();
                    ddlMinor.ClearSelection();
                    ddlSubMinor.ClearSelection();
                    LoadDropDowns(ddlMainCanal.Items[1].Value.ToString(), 1, "Main");
                    CallAgain = false;
                    ddlMainCanal.Visible = false;
                }
                else
                    ddlMainCanal.Visible = true;
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMainCanal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisableDD(ddlBranch);
                DisableDD(ddlDistributry);
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);

                DropDownList ddl = (DropDownList)sender;
                string ID = ddl.ClientID;
                if (ID == "MainContent_ddlMainCanal")
                {
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 1, "Main");
                    ddlMainCanal.Items.FindByValue(ddl.SelectedItem.Value.ToString()).Selected = true;

                }
                else
                {
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 1, "Sub");

                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisableDD(ddlDistributry);
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);
                //LoadDropDowns(ddlBranch.SelectedItem.Value.ToString(), 3, "Main");
                DropDownList ddl = (DropDownList)sender;
                string ID = ddl.ClientID;
                if (ID == "MainContent_ddlBranch")
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 3, "Main");
                else
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 3, "Sub");

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlDistributry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisableDD(ddlMinor);
                DisableDD(ddlSubMinor);
                //LoadDropDowns(ddlDistributry.SelectedItem.Value.ToString(), 4, "Main");
                DropDownList ddl = (DropDownList)sender;
                string ID = ddl.ClientID;
                if (ID == "MainContent_ddlDistributry")
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 4, "Main");
                else
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 4, "Sub");
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlMinor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisableDD(ddlSubMinor);
                //LoadDropDowns(ddlMinor.SelectedItem.Value.ToString(), 5, "Main");
                DropDownList ddl = (DropDownList)sender;
                string ID = ddl.ClientID;
                if (ID == "MainContent_ddlMinor")
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 5, "Main");
                else
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 5, "Sub");
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value != string.Empty)
                {
                    Dropdownlist.DDLYearsForEntitlement(ddlYear, Convert.ToInt64(ddlSeason.SelectedItem.Value));
                    ddlYear.Enabled = true;
                }
                else
                {
                    ddlYear.SelectedIndex = 0;
                    ddlYear.Enabled = false;
                }

                gvSearchEntitlements.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadDropDowns(string _strID, int _ChannelType, string _From)
        {
            gvSearchEntitlements.Visible = false;
            string selectedId = _strID;
            if (string.IsNullOrEmpty(selectedId))
            {
                switch (_ChannelType)
                {
                    case 0: // Canal System
                        DisableDD(ddlMainCanal);
                        DisableDD(ddlBranch);
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        dvMainSub.Visible = false;
                        dvBranchSub.Visible = false;
                        dvDistSub.Visible = false;
                        dvMinorSub.Visible = false;
                        dvSMinorSub.Visible = false;
                        break;
                    case 1: // MAIN Canal
                        DisableDD(ddlBranch);
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        if (_From == "Main")
                            dvMainSub.Visible = false;
                        dvBranchSub.Visible = false;
                        dvDistSub.Visible = false;
                        dvMinorSub.Visible = false;
                        dvSMinorSub.Visible = false;
                        break;

                    case 3: // Branch Canal 
                        DisableDD(ddlDistributry);
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        if (_From == "Main")
                            dvBranchSub.Visible = false;
                        dvDistSub.Visible = false;
                        dvMinorSub.Visible = false;
                        dvSMinorSub.Visible = false;
                        break;

                    case 4: //Distributory Canal 
                        DisableDD(ddlMinor);
                        DisableDD(ddlSubMinor);
                        if (_From == "Main")
                            dvDistSub.Visible = false;
                        dvMinorSub.Visible = false;
                        dvSMinorSub.Visible = false;
                        break;

                    case 5: //Minor Canal 
                        DisableDD(ddlSubMinor);
                        if (_From == "Main")
                            dvMinorSub.Visible = false;
                        dvSMinorSub.Visible = false;
                        break;

                    case 6: //Sub Minor Canal                        
                        if (_From == "Main")
                            dvSMinorSub.Visible = false;
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
                _DataList = new WaterLossesBLL().GetCanalsByParentIDForED(userID, true, Convert.ToInt32(boundryID), ID, _ChannelType);
            }
            else
            {
                _DataList = new WaterLossesBLL().GetCanalsByParentIDForED(0, false, 0, ID, _ChannelType);
            }

            List<object> lstChildCanals = new List<object>();

            int MainIndex = -1, BIndex = -1, DIndex = -1, MIndex = -1, SMIndex = -1;

            switch (_ChannelType)
            {
                case 0: // Canal System
                    MainIndex = 0; BIndex = 1; DIndex = 2; MIndex = 3; SMIndex = 4;
                    ddlBranch.Enabled = false;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;

                    dvMainSub.Visible = false;
                    dvBranchSub.Visible = false;
                    dvDistSub.Visible = false;
                    dvMinorSub.Visible = false;
                    dvSMinorSub.Visible = false;
                    break;
                case 1: // MAIN Canal
                    MainIndex = 0; BIndex = 1; DIndex = 2; MIndex = 3; SMIndex = 4;
                    ddlBranch.Enabled = false;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;

                    if (_From == "Main")
                        dvMainSub.Visible = false;
                    dvBranchSub.Visible = false;
                    dvDistSub.Visible = false;
                    dvMinorSub.Visible = false;
                    dvSMinorSub.Visible = false;
                    break;

                case 3: // Branch Canal
                    MainIndex = -1; BIndex = 0; DIndex = 1; MIndex = 2; SMIndex = 3;
                    ddlDistributry.Enabled = false;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;

                    if (_From == "Main")
                        dvBranchSub.Visible = false;
                    dvDistSub.Visible = false;
                    dvMinorSub.Visible = false;
                    dvSMinorSub.Visible = false;
                    break;

                case 4: //Distributory Canal
                    MainIndex = -1; BIndex = -1; DIndex = 0; MIndex = 1; SMIndex = 2;
                    ddlMinor.Enabled = false;
                    ddlSubMinor.Enabled = false;

                    if (_From == "Main")
                        dvDistSub.Visible = false;
                    dvMinorSub.Visible = false;
                    dvSMinorSub.Visible = false;
                    break;

                case 5: //Minor Canal
                    MainIndex = -1; BIndex = -1; DIndex = -1; MIndex = 0; SMIndex = 1;
                    ddlSubMinor.Enabled = false;

                    if (_From == "Main")
                        dvMinorSub.Visible = false;
                    dvSMinorSub.Visible = false;
                    break;

                case 6: //Sub Minor Canal
                    MainIndex = -1; BIndex = -1; DIndex = -1; MIndex = -1; SMIndex = 0;

                    if (_From == "Main")
                        dvSMinorSub.Visible = false;
                    break;

                default:
                    break;
            }

            bool HasChild = false;

            if (MainIndex != -1)
            {
                HasChild = false;
                lstChildCanals = (List<object>)_DataList.ElementAt(MainIndex); //Main canal 
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    if (_ChannelType == 0)
                    {
                        Dropdownlist.DDLLoading(ddlMainCanal, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                        ddlMainCanal.Enabled = true;
                        dvMainSub.Visible = false;
                    }
                    else
                    {
                        HasChild = true;
                        ddlMainCanal.Enabled = true;
                        //List<object> lstddl = ddlMainCanalSub.Items//ddlMainCanal.Items
                        //              .Cast<ListItem>()
                        //              .Where(z => z.Value != "")
                        //              .Select(x => new
                        //                  {
                        //                      ID = Convert.ToInt64(x.Value),
                        //                      Name = x.Text
                        //                  }).ToList<object>();

                        //lstChildCanals.AddRange(lstddl);
                        //lstChildCanals = (from l in lstChildCanals
                        //                  select new
                        //                 {
                        //                     ID = l.GetType().GetProperty("ID").GetValue(l),
                        //                     Name = l.GetType().GetProperty("Name").GetValue(l)
                        //                 }).GroupBy(x => x.ID).Select(z => z.OrderBy(i => i.ID).First()).ToList<object>();

                        Dropdownlist.DDLLoading(ddlMainCanalSub, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    }
                }
                if (_ChannelType == 1)
                {
                    if (_From == "Main") //if (ddlMainCanal.Items.FindByValue(_strID) != null)
                    {
                        // ddlMainCanal.ClearSelection();
                        // ddlMainCanal.Items.FindByValue(_strID).Selected = true;
                        if (HasChild)
                            dvMainSub.Visible = true;
                        else
                        {
                            ddlMainCanalSub.ClearSelection();
                            dvMainSub.Visible = false;
                        }
                    }

                    else if (ddlMainCanalSub.Items.FindByValue(_strID) != null)
                    {
                        ddlMainCanalSub.ClearSelection();
                        ddlMainCanalSub.Items.FindByValue(_strID).Selected = true;
                        //ddlMainCanal.Items.FindByValue("").Selected = true;
                    }
                }
            }

            if (BIndex != -1)
            {
                HasChild = false;
                lstChildCanals = (List<object>)_DataList.ElementAt(BIndex); //Branch canal
                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    if (_ChannelType == 0 || _ChannelType == 1)
                    {
                        ddlBranch.ClearSelection();
                        ddlBranch.Items.Clear();
                        object obj = lstChildCanals.ElementAt(0);
                        ddlBranch.SelectedValue = Convert.ToString(obj.GetType().GetProperty("ID").GetValue(obj));
                        Dropdownlist.DDLLoading(ddlBranch, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                        //ddlBranch.DataSource = lstChildCanals;
                        //ddlBranch.DataTextField = "Name";
                        //ddlBranch.DataValueField = "ID";
                        //ddlBranch.DataBind();
                        //ddlBranch.Items.Insert(0, new ListItem("Select", ""));
                        ddlBranch.Enabled = true;
                        dvBranchSub.Visible = false;
                    }
                    else
                    {
                        HasChild = true;
                        ddlBranch.Enabled = true;
                        //List<object> lstddl = ddlBranchSub.Items
                        //              .Cast<ListItem>()
                        //              .Where(z => z.Value != "")
                        //              .Select(x => new
                        //              {
                        //                  ID = Convert.ToInt64(x.Value),
                        //                  Name = x.Text
                        //              }).ToList<object>();

                        //lstChildCanals.AddRange(lstddl);
                        //lstChildCanals = (from l in lstChildCanals
                        //                  select new
                        //                  {
                        //                      ID = l.GetType().GetProperty("ID").GetValue(l),
                        //                      Name = l.GetType().GetProperty("Name").GetValue(l)
                        //                  }).GroupBy(x => x.ID).Select(z => z.OrderBy(i => i.ID).First()).ToList<object>();

                        Dropdownlist.DDLLoading(ddlBranchSub, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    }
                }
                if (_ChannelType == 3)
                {
                    if (_From == "Main") //if (ddlBranch.Items.FindByValue(_strID) != null)
                    {
                        ddlBranch.ClearSelection();
                        ddlBranch.Items.FindByValue(_strID).Selected = true;
                        if (HasChild)
                            dvBranchSub.Visible = true;
                        else
                        {
                            dvBranchSub.Visible = false;
                            ddlBranchSub.ClearSelection();
                        }

                    }
                    else if (ddlBranchSub.Items.FindByValue(_strID) != null)
                    {
                        ddlBranchSub.ClearSelection();
                        ddlBranchSub.Items.FindByValue(_strID).Selected = true;
                        // ddlBranch.Items.FindByValue("").Selected = true;
                    }
                }
            }

            if (DIndex != -1)
            {
                HasChild = false;
                lstChildCanals = (List<object>)_DataList.ElementAt(DIndex); //Distributry Canal                

                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    if (_ChannelType == 0 || _ChannelType == 1 || _ChannelType == 3)
                    {
                        ddlDistributry.ClearSelection();
                        ddlDistributry.Items.Clear();
                        object obj = lstChildCanals.ElementAt(0);
                        ddlDistributry.SelectedValue = Convert.ToString(obj.GetType().GetProperty("ID").GetValue(obj));

                        Dropdownlist.DDLLoading(ddlDistributry, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                        ddlDistributry.Enabled = true;
                        dvDistSub.Visible = false;
                    }
                    else
                    {
                        HasChild = true;
                        ddlDistributry.Enabled = true;
                        //List<object> lstddl = ddlDistributrySub.Items
                        //              .Cast<ListItem>()
                        //              .Where(z => z.Value != "")
                        //              .Select(x => new
                        //              {
                        //                  ID = Convert.ToInt64(x.Value),
                        //                  Name = x.Text
                        //              }).ToList<object>();

                        //lstChildCanals.AddRange(lstddl);
                        //lstChildCanals = (from l in lstChildCanals
                        //                  select new
                        //                  {
                        //                      ID = l.GetType().GetProperty("ID").GetValue(l),
                        //                      Name = l.GetType().GetProperty("Name").GetValue(l)
                        //                  }).GroupBy(x => x.ID).Select(z => z.OrderBy(i => i.ID).First()).ToList<object>();

                        Dropdownlist.DDLLoading(ddlDistributrySub, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    }
                }
                if (_ChannelType == 4)
                {
                    if (_From == "Main") //if (ddlDistributry.Items.FindByValue(_strID) != null)
                    {
                        ddlDistributry.ClearSelection();
                        ddlDistributry.Items.FindByValue(_strID).Selected = true;
                        if (HasChild)
                            dvDistSub.Visible = true;
                        else
                        {
                            dvDistSub.Visible = false;
                            ddlDistributrySub.ClearSelection();
                        }

                    }
                    else if (ddlDistributrySub.Items.FindByValue(_strID) != null)
                    {
                        ddlDistributrySub.ClearSelection();
                        ddlDistributrySub.Items.FindByValue(_strID).Selected = true;
                        // ddlDistributry.Items.FindByValue("").Selected = true;
                    }
                }
            }

            if (MIndex != -1)
            {
                HasChild = false;
                lstChildCanals = (List<object>)_DataList.ElementAt(MIndex); //Minor Canal

                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    if (_ChannelType == 0 || _ChannelType == 1 || _ChannelType == 3 || _ChannelType == 4)
                    {
                        ddlMinor.ClearSelection();
                        ddlMinor.Items.Clear();
                        object obj = lstChildCanals.ElementAt(0);
                        ddlMinor.SelectedValue = Convert.ToString(obj.GetType().GetProperty("ID").GetValue(obj));

                        Dropdownlist.DDLLoading(ddlMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                        ddlMinor.Enabled = true;
                        dvMinorSub.Visible = false;
                    }
                    else
                    {
                        HasChild = true;
                        ddlMinor.Enabled = true;
                        //List<object> lstddl = ddlMinorSub.Items
                        //              .Cast<ListItem>()
                        //              .Where(z => z.Value != "")
                        //              .Select(x => new
                        //              {
                        //                  ID = Convert.ToInt64(x.Value),
                        //                  Name = x.Text
                        //              }).ToList<object>();

                        //lstChildCanals.AddRange(lstddl);
                        //lstChildCanals = (from l in lstChildCanals
                        //                  select new
                        //                  {
                        //                      ID = l.GetType().GetProperty("ID").GetValue(l),
                        //                      Name = l.GetType().GetProperty("Name").GetValue(l)
                        //                  }).GroupBy(x => x.ID).Select(z => z.OrderBy(i => i.ID).First()).ToList<object>();

                        Dropdownlist.DDLLoading(ddlMinorSub, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    }
                }
                if (_ChannelType == 5)
                {
                    if (_From == "Main") //if (ddlMinor.Items.FindByValue(_strID) != null)
                    {
                        ddlMinor.ClearSelection();
                        ddlMinor.Items.FindByValue(_strID).Selected = true;
                        if (HasChild)
                            dvMinorSub.Visible = true;
                        else
                        {
                            dvMinorSub.Visible = false;
                            ddlMinorSub.ClearSelection();
                        }
                    }
                    else if (ddlMinorSub.Items.FindByValue(_strID) != null)
                    {
                        ddlMinorSub.ClearSelection();
                        ddlMinorSub.Items.FindByValue(_strID).Selected = true;
                        // ddlMinor.Items.FindByValue("").Selected = true;
                    }
                }
            }

            if (SMIndex != -1)
            {
                HasChild = false;
                lstChildCanals = (List<object>)_DataList.ElementAt(SMIndex); //Sub Minor Canal                

                if (lstChildCanals != null && lstChildCanals.Count > 0)
                {
                    if (_ChannelType == 0 || _ChannelType == 1 || _ChannelType == 3 || _ChannelType == 4 || _ChannelType == 5)
                    {
                        ddlSubMinor.ClearSelection();
                        ddlSubMinor.Items.Clear();
                        object obj = lstChildCanals.ElementAt(0);
                        ddlSubMinor.SelectedValue = Convert.ToString(obj.GetType().GetProperty("ID").GetValue(obj));

                        Dropdownlist.DDLLoading(ddlSubMinor, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                        ddlSubMinor.Enabled = true;
                        dvSMinorSub.Visible = false;
                    }
                    else
                    {
                        HasChild = true;
                        ddlSubMinor.Enabled = true;
                        //List<object> lstddl = ddlSubMinorSub.Items
                        //              .Cast<ListItem>()
                        //              .Where(z => z.Value != "")
                        //              .Select(x => new
                        //              {
                        //                  ID = Convert.ToInt64(x.Value),
                        //                  Name = x.Text
                        //              }).ToList<object>();

                        //lstChildCanals.AddRange(lstddl);
                        //lstChildCanals = (from l in lstChildCanals
                        //                  select new
                        //                  {
                        //                      ID = l.GetType().GetProperty("ID").GetValue(l),
                        //                      Name = l.GetType().GetProperty("Name").GetValue(l)
                        //                  }).GroupBy(x => x.ID).Select(z => z.OrderBy(i => i.ID).First()).ToList<object>();

                        Dropdownlist.DDLLoading(ddlSubMinorSub, false, (int)Constants.DropDownFirstOption.Select, lstChildCanals);
                    }
                }
                if (_ChannelType == 6)
                {
                    if (_From == "Main") // if (ddlSubMinor.Items.FindByValue(_strID) != null)
                    {
                        ddlSubMinor.ClearSelection();
                        ddlSubMinor.Items.FindByValue(_strID).Selected = true;
                        if (HasChild)
                            dvSMinorSub.Visible = true;
                        else
                        {
                            dvSMinorSub.Visible = false;
                            ddlSubMinorSub.ClearSelection();
                        }
                    }
                    else if (ddlSubMinorSub.Items.FindByValue(_strID) != null)
                    {
                        ddlSubMinorSub.ClearSelection();
                        ddlSubMinorSub.Items.FindByValue(_strID).Selected = true;
                        // ddlSubMinor.Items.FindByValue("").Selected = true;
                    }
                }
            }

            switch (_ChannelType)
            {
                case 0: // Canal System
                    ddlMainCanal.SelectedValue = "";
                    ddlBranch.SelectedValue = "";
                    ddlDistributry.SelectedValue = "";
                    ddlMinor.SelectedValue = "";
                    ddlSubMinor.SelectedValue = "";
                    break;

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

            ddlSeason.SelectedIndex = 0;
            DisableDD(ddlYear);

            //RemoveChannels();
        }

        private void GetCommandType()
        {
            Dropdownlist.DDLCommandNames(ddlCommand);
            ddlCommand.Enabled = true;
        }

        private long SelectedChannelID()
        {
            if (ddlSubMinorSub.Visible == true && ddlSubMinorSub.Enabled && !ddlSubMinorSub.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlSubMinorSub.SelectedValue);
            }
            else if (ddlSubMinor.Enabled && !ddlSubMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlSubMinor.SelectedValue);
            }
            else if (ddlMinorSub.Visible == true && ddlMinorSub.Enabled && !ddlMinorSub.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMinorSub.SelectedValue);
            }
            else if (ddlMinor.Enabled && !ddlMinor.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMinor.SelectedValue);
            }
            else if (ddlDistributrySub.Visible == true && ddlDistributrySub.Enabled && !ddlDistributrySub.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlDistributrySub.SelectedValue);
            }
            else if (ddlDistributry.Enabled && !ddlDistributry.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlDistributry.SelectedValue);
            }
            else if (ddlBranchSub.Visible == true && ddlBranchSub.Enabled && !ddlBranchSub.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlBranchSub.SelectedValue);
            }
            else if (ddlBranch.Enabled && !ddlBranch.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlBranch.SelectedValue);
            }
            else if (ddlMainCanalSub.Visible == true && ddlMainCanalSub.Enabled && !ddlMainCanalSub.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMainCanalSub.SelectedValue);
            }
            else if (ddlMainCanal.Enabled && !ddlMainCanal.SelectedValue.Equals(""))
            {
                return Convert.ToInt64(ddlMainCanal.SelectedValue);
            }

            return 0;

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EntitlementDelivery);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvSearchEntitlements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblSeason = (Label)e.Row.FindControl("lblSeason");
                    Label lblYear = (Label)e.Row.FindControl("lblYear");
                    Label lblMAFEntitlement = (Label)e.Row.FindControl("lblMAFEntitlement");
                    Label lblMAFDistribution = (Label)e.Row.FindControl("lblMAFDistribution");

                    long Year = Convert.ToInt64(lblYear.Text);
                    long IncrementedYEarYear = Year + 1;

                    if (lblSeason.Text == "1")
                    {
                        lblSeason.Text = "Rabi";
                        lblYear.Text = Year + "-" + IncrementedYEarYear;
                    }
                    else
                    {
                        lblSeason.Text = "Kharif";
                    }

                    if (lblMAFEntitlement.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblMAFEntitlement.Text);
                        MAF = Math.Round(MAF, 3);
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblMAFEntitlement.Text = MaskedValue;
                    }
                    if (lblMAFDistribution.Text != string.Empty)
                    {
                        MAF = Convert.ToDouble(lblMAFDistribution.Text);
                        MAF = Math.Round(MAF, 3);
                        if (ParentChild == 'C')
                        {
                            MAF = MAF * 1000;
                        }
                        MaskedValue = String.Format("{0:0.000}", MAF);
                        lblMAFDistribution.Text = MaskedValue;
                    }

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (ParentChild == 'C')
                    {
                        //Label HeadTextEntitlement = (Label)e.Row.FindControl("lblMAFEntitlement");
                        //HeadTextEntitlement.Text = "Entitlement (AF)";
                        e.Row.Cells[5].Text = "Entitlement (1000 AF)";
                        e.Row.Cells[6].Text = "Delivery (1000 AF)";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvSearchEntitlements.PageIndex = 0;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            EntitlementDeliveryBLL bllEntitlementDeliver = new EntitlementDeliveryBLL();
            List<dynamic> lstEntitlements;
            long ChannelID = SelectedChannelID();
            long SeasonID = ddlSeason.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSeason.SelectedItem.Value);
            long Year = ddlYear.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlYear.SelectedItem.Value);

            List<long> lstSeasonIDs = new List<long>();
            if (SeasonID == 1)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.Rabi };
            }
            else if (SeasonID == 2)
            {
                lstSeasonIDs = new List<long>() { (long)Constants.Seasons.EarlyKharif, (long)Constants.Seasons.LateKharif };
            }


            if (ChannelID == 0)
            {
                ParentChild = 'P';
                ChannelID = ddlCanalSystem.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCanalSystem.SelectedItem.Value);
                lstEntitlements = bllEntitlementDeliver.GetChannelEntitlementBySearchCriteria(ChannelID, lstSeasonIDs, Year);
            }
            else
            {
                ParentChild = 'C';
                lstEntitlements = bllEntitlementDeliver.GetChildChannelEntitlementBySearchCriteria(ChannelID, lstSeasonIDs, Year);
            }

            gvSearchEntitlements.DataSource = lstEntitlements;
            gvSearchEntitlements.DataBind();
            gvSearchEntitlements.Visible = true;
        }

        protected void gvSearchEntitlements_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchEntitlements.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                long ChannelID = Convert.ToInt64(gvSearchEntitlements.DataKeys[gvrCurrent.RowIndex].Values[ChannelIDIndex]);
                long SeasonID = Convert.ToInt64(gvSearchEntitlements.DataKeys[gvrCurrent.RowIndex].Values[SeasonIndex]);
                int Year = Convert.ToInt32(gvSearchEntitlements.DataKeys[gvrCurrent.RowIndex].Values[YearIndex]);
                string ParentChild = gvSearchEntitlements.DataKeys[gvrCurrent.RowIndex].Values[ParentChildIndex].ToString();

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("ChannelID", ChannelID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("SeasonID", SeasonID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("Year", Year.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("ParentChild", ParentChild.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                mdlReportData.Name = Constants.EntitlementReport;

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSearchEntitlements.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void RemoveChannels()
        {
            ddlMainCanal.Items.Remove(ddlMainCanal.Items.FindByValue(RPCChannelID.ToString()));
            ddlMainCanal.Items.Remove(ddlMainCanal.Items.FindByValue(SubLinkChannelID.ToString()));

            ddlBranch.Items.Remove(ddlBranch.Items.FindByValue(RPCChannelID.ToString()));
            ddlBranch.Items.Remove(ddlBranch.Items.FindByValue(SubLinkChannelID.ToString()));

            ddlDistributry.Items.Remove(ddlDistributry.Items.FindByValue(RPCChannelID.ToString()));
            ddlDistributry.Items.Remove(ddlDistributry.Items.FindByValue(SubLinkChannelID.ToString()));

            ddlMinor.Items.Remove(ddlMinor.Items.FindByValue(RPCChannelID.ToString()));
            ddlMinor.Items.Remove(ddlMinor.Items.FindByValue(SubLinkChannelID.ToString()));

            ddlSubMinor.Items.Remove(ddlSubMinor.Items.FindByValue(RPCChannelID.ToString()));
            ddlSubMinor.Items.Remove(ddlSubMinor.Items.FindByValue(SubLinkChannelID.ToString()));
        }

        protected void ddlSubMinor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //LoadDropDowns(ddlSubMinor.SelectedItem.Value.ToString(), 6, "Main");
                DropDownList ddl = (DropDownList)sender;
                string ID = ddl.ClientID;
                if (ID == "MainContent_ddlSubMinor")
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 6, "Main");
                else
                    LoadDropDowns(ddl.SelectedItem.Value.ToString(), 6, "Sub");
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}