using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft.Controls
{
    public static class WaterTheftCase
    {
        #region "Properties"
        public static long WaterTheftID
        {
            get
            {
                return HttpContext.Current.Session["WaterTheftID"] == null ? 0 :
                    (long)HttpContext.Current.Session["WaterTheftID"];
            }
            set { HttpContext.Current.Session["WaterTheftID"] = value; }
        }
        public static string OffenceSite
        {
            get
            {
                return HttpContext.Current.Session["OffenceSite"] == null ? "O" :
                    Convert.ToString(HttpContext.Current.Session["OffenceSite"]);
            }
            set
            {
                HttpContext.Current.Session["OffenceSite"] = value;
            }
        }
        public static long CaseStatusID
        {
            get
            {
                return HttpContext.Current.Session["CaseStatusID"] == null ? 0 :
                    Convert.ToInt64(HttpContext.Current.Session["CaseStatusID"]);
            }
            set
            {
                HttpContext.Current.Session["CaseStatusID"] = value;
            }
        }


        public static long AssignedToUserID
        {
            get
            {
                return HttpContext.Current.Session["AssignedToUserID"] == null ? 0 : (long)HttpContext.Current.Session["AssignedToUserID"];
            }
            set { HttpContext.Current.Session["AssignedToUserID"] = value; }
        }





        public static long AssignedToDesignationID
        {
            get
            {
                return HttpContext.Current.Session["AssignedToDesignationID"] == null ? 0 :
                    (long)HttpContext.Current.Session["AssignedToDesignationID"];
            }
            set { HttpContext.Current.Session["AssignedToDesignationID"] = value; }
        }
        public static long AssignedByDesignationID
        {
            get
            {
                return HttpContext.Current.Session["AssignedByDesignationID"] == null ? 0 :
                    (long)HttpContext.Current.Session["AssignedByDesignationID"];
            }
            set { HttpContext.Current.Session["AssignedByDesignationID"] = value; }
        }
        public static long CanalWireID
        {
            get
            {
                return HttpContext.Current.Session["CanalWireID"] == null ? 0 : (long)HttpContext.Current.Session["CanalWireID"];
            }
            set
            {
                HttpContext.Current.Session["CanalWireID"] = value;
            }
        }
        public static DateTime IncidentDateTime
        {
            get
            {
                return Convert.ToDateTime(HttpContext.Current.Session["IncidentDateTime"]);
            }
            set
            {
                HttpContext.Current.Session["IncidentDateTime"] = value;
            }
        }
        #endregion

        #region "Enum"
        public enum ControlToLoad
        {
            Channel = 1,
            Outlet
        }
        #endregion

        #region "Public Methods"
        public static void GetWaterTheftCaseAssignee(long _WaterTheftID)
        {
            try
            {
                dynamic bllWaterTheftCaseAssignee = new WaterTheftBLL().GetWaterTheftCaseAssignee(_WaterTheftID);
                if (bllWaterTheftCaseAssignee == null)
                    return;

                string OS = Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "OffenceSite");

                WaterTheftCase.WaterTheftID = _WaterTheftID;
                WaterTheftCase.OffenceSite = OS.ToUpper() == "C" ? Convert.ToString((int)ControlToLoad.Channel) : Convert.ToString((int)ControlToLoad.Outlet);
                WaterTheftCase.AssignedToDesignationID = Convert.ToInt64(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "AssignedToDesignationID"));
                WaterTheftCase.AssignedToUserID = Convert.ToInt64(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "AssignedToUserID"));
                WaterTheftCase.AssignedByDesignationID = Convert.ToInt64(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "AssignedByDesignationID"));
                WaterTheftCase.CaseStatusID = Convert.ToInt64(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "CaseStatusID"));
                WaterTheftCase.CanalWireID = Convert.ToInt64(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "CanalWireID"));
                WaterTheftCase.IncidentDateTime = Convert.ToDateTime(Utility.GetDynamicPropertyValue(bllWaterTheftCaseAssignee, "IncidentDateTime"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataTable(long WaterTheftBreachID, long UserID, long DesignationID, List<Tuple<string, string, string>> lstNameofFiles, string Source, Double GIS_X, Double GIS_Y)
        {
            int Size = lstNameofFiles.Count;
            DataSet DataSet = new DataSet();
            DataTable DataTable = DataSet.Tables.Add("SampleData");
            DataTable.Columns.Add("WaterTheftID", typeof(long));
            DataTable.Columns.Add("FileName", typeof(string));
            DataTable.Columns.Add("FileType", typeof(string));
            DataTable.Columns.Add("AttachmentPath", typeof(string));
            DataTable.Columns.Add("AttachmentByUserID", typeof(long));
            DataTable.Columns.Add("IsActive", typeof(bool));
            DataTable.Columns.Add("CreatedBy", typeof(long));
            DataTable.Columns.Add("AttachmentByDesignationID", typeof(long));
            DataTable.Columns.Add("Source", typeof(string));
            DataTable.Columns.Add("GIS_X", typeof(Double));
            DataTable.Columns.Add("GIS_Y", typeof(Double));
            for (int i = 0; i < Size; ++i)
            {
                DataRow DataRow;
                DataRow = DataTable.NewRow();
                DataRow["WaterTheftID"] = WaterTheftBreachID;
                DataRow["FileName"] = lstNameofFiles[i].Item1;
                DataRow["FileType"] = lstNameofFiles[i].Item2;
                DataRow["AttachmentPath"] = lstNameofFiles[i].Item3;
                DataRow["AttachmentByUserID"] = UserID;
                DataRow["IsActive"] = true;
                DataRow["CreatedBy"] = UserID;
                DataRow["AttachmentByDesignationID"] = DesignationID;
                DataRow["Source"] = Source;
                DataRow["GIS_X"] = GIS_X;
                DataRow["GIS_Y"] = GIS_Y;
                DataTable.Rows.Add(DataRow);
            }
            return DataTable;
        }



        public static DataTable GetDataTableForBreach(long WaterTheftBreachID, long UserID, long DesignationID, List<Tuple<string, string, string>> lstNameofFiles)
        {
            int Size = lstNameofFiles.Count;
            DataSet DataSet = new DataSet();
            DataTable DataTable = DataSet.Tables.Add("SampleData");
            DataTable.Columns.Add("BreachID", typeof(long));
            DataTable.Columns.Add("FileName", typeof(string));
            DataTable.Columns.Add("FileType", typeof(string));
            DataTable.Columns.Add("AttachmentPath", typeof(string));
            DataTable.Columns.Add("AttachmentByUserID", typeof(long));
            DataTable.Columns.Add("IsActive", typeof(bool));
            DataTable.Columns.Add("CreatedBy", typeof(long));
            DataTable.Columns.Add("AttachmentByDesignationID", typeof(long));

            for (int i = 0; i < Size; ++i)
            {
                DataRow DataRow;
                DataRow = DataTable.NewRow();
                DataRow["BreachID"] = WaterTheftBreachID;
                DataRow["FileName"] = lstNameofFiles[i].Item1;
                DataRow["FileType"] = lstNameofFiles[i].Item2;
                DataRow["AttachmentPath"] = lstNameofFiles[i].Item3;
                DataRow["AttachmentByUserID"] = UserID;
                DataRow["IsActive"] = true;
                DataRow["CreatedBy"] = UserID;
                DataRow["AttachmentByDesignationID"] = DesignationID;
                DataTable.Rows.Add(DataRow);
            }
            return DataTable;
        }

        #endregion
    }
}