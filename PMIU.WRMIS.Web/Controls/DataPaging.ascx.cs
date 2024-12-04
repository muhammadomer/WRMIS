using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Controls
{
    public partial class DataPaging : System.Web.UI.UserControl
    {
        #region "Delegates"
        //Help taken from
        //http://www.4guysfromrolla.com/articles/031704-1.aspx#postadlink
        private Delegate delUpdatePageIndex;
        public System.Delegate UpdatePageIndex
        {
            set { delUpdatePageIndex = value; }
        }
        #endregion

        #region "Properties"
        [Category("Behavior")]
        [Description("Total number of records")]
        [DefaultValue(0)]
        public int TotalRecords
        {
            get
            {
                object o = ViewState["TotalRecords"];
                if (o == null)
                    return 0;
                return (int)o;
            }
            set { ViewState["TotalRecords"] = value; }
        }
        [Category("Behavior")]
        [Description("Current page index")]
        [DefaultValue(1)]
        public int PageIndex
        {
            get
            {
                object o = ViewState["PageIndex"];
                if (o == null)
                    return 1;
                return (int)o;
            }
            set { ViewState["PageIndex"] = value; }
        }
        [Category("Behavior")]
        [Description("Total number of records to each page")]
        [DefaultValue(25)]
        public int RecordsPerPage
        {
            get
            {
                object o = ViewState["RecordsPerPage"];
                if (o == null)
                    return 25;
                return (int)o;
            }
            set { ViewState["RecordsPerPage"] = value; }
        }
        private decimal TotalPages
        {
            get
            {
                object o = ViewState["TotalPages"];
                if (o == null)
                    return 0;
                return (decimal)o;
            }
            set { ViewState["TotalPages"] = value; }
        }
        #endregion

        #region "Page Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int[] Paging = { 5, 10, 20, 30, 50, 100 };
                for (int p = 0; p < Paging.Length; p++)
                {
                    ddlRecords.Items.Add(Paging[p].ToString());
                    ddlRecords.SelectedIndex = 0;
                }
                this.RecordsPerPage = Convert.ToInt32(ddlRecords.Text);
                UpdatePaging(this.PageIndex, this.RecordsPerPage, this.TotalRecords);
                ResetPagesDropdownlist();
            }
        }
        #endregion

        #region "Control Events"
        protected void btnMove_Click(object sender, CommandEventArgs e)
        {
            switch (Convert.ToString(e.CommandArgument))
            {
                case "First":
                    this.PageIndex = 1;
                    break;
                case "Previous":
                    this.PageIndex--;
                    break;
                case "Next":
                    this.PageIndex++;
                    break;
                case "Last":
                    this.PageIndex = (int)this.TotalPages;
                    break;
            }
            UpdatePaging(this.PageIndex, this.RecordsPerPage, this.TotalRecords);
        }
        protected void txtPage_TextChanged(object sender, EventArgs e)
        {
            int newPage = Convert.ToInt32(txtPage.Text);
            if (newPage > this.TotalPages)
                this.PageIndex = (int)this.TotalPages;
            else
                this.PageIndex = newPage;
            UpdatePaging(this.PageIndex, this.RecordsPerPage, this.TotalRecords);
        }
        protected void ddlRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RecordsPerPage = Convert.ToInt32(ddlRecords.Text);
            this.PageIndex = 1;
            ResetPagesDropdownlist();
            UpdatePaging(this.PageIndex, this.RecordsPerPage, this.TotalRecords);
        }
        #endregion

        #region "Web Methods"
        public void UpdatePaging(int pageIndex, int pageSize, int recordCount)
        {
            if (recordCount > 0)
            {
                lblTotalRecord.ForeColor = Color.Black;
                ddlRecords.Enabled = true;
                int currentEndRow = (pageIndex * pageSize);
                if (currentEndRow > recordCount) currentEndRow = recordCount;

                if (currentEndRow < pageSize) pageSize = currentEndRow;
                //int currentStartRow = (currentEndRow - pageSize) + 1;
                int currentStartRow = ((pageIndex - 1) * pageSize) + 1;

                this.TotalPages = Math.Ceiling((decimal)recordCount / pageSize);
                txtPage.Text = string.Format("{0:00}", this.PageIndex);
                ddlPage.SelectedValue = Convert.ToString(this.PageIndex);
                lblTotalRecord.Text = string.Format("{0:00}-{1:00} of {2:00} record(s)", currentStartRow, currentEndRow, recordCount);
                lblTotalPage.Text = string.Format(" of {0:00} page(s)", this.TotalPages);

                btnMoveFirst.Enabled = (pageIndex == 1) ? false : true;
                btnMovePrevious.Enabled = (pageIndex > 1) ? true : false;
                btnMoveNext.Enabled = (pageIndex * pageSize < recordCount) ? true : false;
                btnMoveLast.Enabled = (pageIndex * pageSize >= recordCount) ? false : true;

                //call method to re-populate parent page data, 
                //given current index:
                object[] aObj = new object[1];
                aObj[0] = pageIndex;
                //delUpdatePageIndex.DynamicInvoke(aObj);
                delUpdatePageIndex.DynamicInvoke();
            }
            else
            {
                lblTotalPage.Text = "";
                lblTotalRecord.Text = "No record found!";
                lblTotalRecord.ForeColor = Color.Red;
                btnMoveFirst.Enabled = false;
                btnMovePrevious.Enabled = false;
                btnMoveNext.Enabled = false;
                btnMoveLast.Enabled = false;
                txtPage.Enabled = false;
                ddlRecords.Enabled = false;
            }
        }
        private void ResetPagesDropdownlist()
        {
            this.TotalPages = Math.Ceiling((decimal)this.TotalRecords / this.RecordsPerPage);

            ddlPage.Items.Clear();

            for (int i = 1; i <= this.TotalPages; ++i)
            {
                ddlPage.Items.Add(i.ToString());
            }
        }
        #endregion

        protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newPage = Convert.ToInt32(ddlPage.SelectedValue);
            if (newPage > this.TotalPages)
                this.PageIndex = (int)this.TotalPages;
            else
                this.PageIndex = newPage;
            //ResetPagesDropdownlist();
            UpdatePaging(this.PageIndex, this.RecordsPerPage, this.TotalRecords);
        }

        public void EnableNow()
        {
            tblPager.Disabled = false;
            btnMoveFirst.Enabled = true;
            btnMoveLast.Enabled = true;
            btnMoveNext.Enabled = true;
            btnMovePrevious.Enabled = true;
            txtPage.Enabled = true;
            ddlPage.Enabled = true;
            ddlRecords.Enabled = true;
        }

        public void DisableNow()
        {
            tblPager.Disabled = true;
            btnMoveFirst.Enabled = false;
            btnMoveLast.Enabled = false;
            btnMoveNext.Enabled = false;
            btnMovePrevious.Enabled = false;
            txtPage.Enabled = false;
            ddlPage.Enabled = false;
            ddlRecords.Enabled = false;
        }
    }
}