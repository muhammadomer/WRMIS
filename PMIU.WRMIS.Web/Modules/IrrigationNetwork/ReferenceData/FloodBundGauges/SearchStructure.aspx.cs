using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.FloodBundGauges
{
    public partial class SearchStructure : BasePage
    {
        private static bool _IsSaved = false;

        #region Hash Table Keys

        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string InfrastructureTypeIDKey = "InfrastructureTypeID";
        public const string InfrastructureNameIDKey = "InfrastructureNameID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                BindDropdownlists();
                this.Form.DefaultButton = btnShow.UniqueID;
                long StructureNallahHillTorentID = Utility.GetNumericValueFromQueryString("StructureNallahHillTorentID", 0);
                if (_IsSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description);
                    _IsSaved = false; // Reset flag after displaying message.
                }
                BindSearchResultsGrid(Convert.ToInt64(hdnStructureNallahHillTorentID.Value));
                //if (StructureNallahHillTorentID > 0)
                //{
                //    hdnStructureNallahHillTorentID.Value = StructureNallahHillTorentID.ToString();
                //}
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }



        #region BindDropdown
        private void BindDropdownlists()
        {
            BindStructureTypeFloodBund();
            BindZoneDropdown();
        }
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        }
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.All);
        }
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisionsForDFAndIrrigation(ddlDivision, false, _CircleID, (int)Constants.DropDownFirstOption.All);
        }

        private void BindStructureTypeFloodBund()
        {
            Dropdownlist.DDLStructureTypeFloodBund(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
        }

        #endregion BindDropdown

        #region Dropdown Event
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    //ddlCircle.Enabled = true;
                }
                ddlDivision.SelectedIndex = 0;
                //ddlDivision.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    //  ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    //  ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion Dropdown Event

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void BindSearchResultsGrid(long _StructureNallahHillTorentID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();
                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                string SelectedInfrastyructureTypeID = null;
                string SelectedInfrastyructureName = null;

                if (ddlZone.SelectedItem.Value != String.Empty && ddlZone.SelectedItem.Value != "0")
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty && ddlCircle.SelectedItem.Value != "0")
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty && ddlDivision.SelectedItem.Value != "0")
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlInfrastructureType.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureTypeID = Convert.ToString(ddlInfrastructureType.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureTypeIDKey, SelectedInfrastyructureTypeID);

                if (txtStructureName.Text != String.Empty)
                {
                    SelectedInfrastyructureName = Convert.ToString(txtStructureName.Text.Trim());
                }

                SearchCriteria.Add(InfrastructureNameIDKey, txtStructureName.Text.Trim());

                var ID = _StructureNallahHillTorentID == 0 ? (long?)null : _StructureNallahHillTorentID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> ieFloodInapection = new FloodOperationsBLL().GetFloodBundRefSearch(ID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedInfrastyructureTypeID, SelectedInfrastyructureName);

                var lstFloodInspection = ieFloodInapection.Select(dataRow => new
                {
                    StructureNalaHillTorantID = dataRow.Field<long>("StructureNalaHillTorantID"),
                    Division = dataRow.Field<string>("Division"),
                    StructureType = dataRow.Field<string>("StructureType"),
                    StructureName = dataRow.Field<string>("StructureName"),

                }).ToList();

                gvStructureSearch.DataSource = lstFloodInspection;
                gvStructureSearch.DataBind();
                SearchCriteria.Add(PageIndexKey, gvStructureSearch.PageIndex);
                Session[SessionValues.SearchFloodBundRef] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid(0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}