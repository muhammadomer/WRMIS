using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebFormsTest
{
    public partial class FileUploadControl : System.Web.UI.UserControl
    {
        protected int _Size;
        protected string _UploadPath;
        protected string _Name;
        protected int? _Mode;
        public bool? _IsRequiredCheck;
        public bool? _AllowMultiple;

        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public string UploadPath
        {
            get { return _UploadPath; }
            set { _UploadPath = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        public int? Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public bool? AllowMultiple
        {
            get { return _AllowMultiple; }
            set { _AllowMultiple = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Mode == null)
            {
                Mode = Convert.ToInt32(Constants.ModeValue.Edit);
            }

            if (Mode == Convert.ToInt32(Constants.ModeValue.Edit))
            {
                for (int i = 0; i < Size; ++i)
                {
                    System.Web.UI.HtmlControls.HtmlTableRow TableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                    System.Web.UI.HtmlControls.HtmlTableCell TableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                    FileUpload oFileUpload = new FileUpload();

                    if (AllowMultiple == true)
                    {
                        oFileUpload.AllowMultiple = true;
                        oFileUpload.Attributes.Add("accept", "application/pdf,image/*");
                    }
                    //oFileUpload.ID = "ctrl" + i + "_FU";    by salman

                    //if (oFileUpload.ID == "ctrl0_FU")
                    //{
                    //    oFileUpload.Attributes.Add("required", "required");
                    //}

                    oFileUpload.ID = "ctrl" + i + "_FU";  // newly added 

                    if (!string.IsNullOrEmpty(Name))
                    {
                        oFileUpload.CssClass = Name + i;
                        oFileUpload.ID = Name + i + "_FU";    //newly added 
                    }
                    else
                        oFileUpload.CssClass = "CtrlClass" + i;

                    if (oFileUpload.CssClass == "CtrlClass0")
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "FIRCtrl0") // for Tawaan Working screen
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "DesnCtrl0") // for Tawaan Working screen
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "AttchCtrl0") // for Tawaan Working screen
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "FormCtrl0") // for Tender Management Screens
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "GridCtrl0") // for Tender Management Screens
                        oFileUpload.Attributes.Add("required", "required");
                    else if (oFileUpload.CssClass == "MultipleCtrl0") // for Multiple Uploads
                        oFileUpload.Attributes.Add("required", "required");

                    oFileUpload.Attributes.Add("OnChange", "checkfile(this)");
                    TableCell.Controls.Add(oFileUpload);
                    TableRow.Controls.Add(TableCell);
                    tblFileUpload.Rows.Add(TableRow);
                }
            }
            if (Mode == Convert.ToInt32(Constants.ModeValue.RemoveValidation))
            {
                for (int i = 0; i < Size; ++i)
                {
                    System.Web.UI.HtmlControls.HtmlTableRow TableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                    System.Web.UI.HtmlControls.HtmlTableCell TableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                    FileUpload oFileUpload = new FileUpload();
                    oFileUpload.ID = "ctrl" + i + "_FU";  // newly added 
                    if (!string.IsNullOrEmpty(Name))
                    {
                        oFileUpload.CssClass = Name + i;
                        oFileUpload.ID = Name + i + "_FU";    //newly added 
                    }
                    else
                        oFileUpload.CssClass = "CtrlClass" + i;

                    if (oFileUpload.CssClass == "CtrlClass0")
                        oFileUpload.Attributes.Remove("required");

                    oFileUpload.Attributes.Add("OnChange", "checkfile(this)");
                    TableCell.Controls.Add(oFileUpload);
                    TableRow.Controls.Add(TableCell);
                    tblFileUpload.Rows.Add(TableRow);
                }
            }
            else
            {
                return;
            }
        }
        public List<Tuple<string, string, string>> UploadNow(string _ModuleName = Configuration.WaterTheft, string _CTRLID = "ctrl", int _FileControlIndex = 0)
        {
            bool PostedFile = true;
            string FullFileName = string.Empty;
            string FileName = string.Empty;
            string FileExt = string.Empty;
            var list = new List<Tuple<string, string, string>>();
            for (int i = 0; i < Size; ++i, ++_FileControlIndex)
            {
                string sGuid = Guid.NewGuid().ToString();
                FileUpload oFileUpload = null;
                // FileUpload oFileUpload = tblFileUpload.Rows[i].Cells[0].FindControl("ctrl" + i + "_FU") as FileUpload;
                //For Auctions only

                if ((_ModuleName == Configuration.Auctions || _ModuleName == Configuration.Accounts) && _CTRLID.Contains("_"))
                {
                    int Index = _CTRLID.IndexOf('_');
                    var ID = _CTRLID.Substring(Index + 1);
                    var CtrlID = _CTRLID.Substring(0, Index);
                    oFileUpload = tblFileUpload.Rows[i].Cells[0].FindControl(CtrlID + i + "_FU_" + ID) as FileUpload;
                }
                else
                {
                    oFileUpload = tblFileUpload.Rows[i].Cells[0].FindControl(_CTRLID + i + "_FU") as FileUpload;
                }

                // HttpPostedFile UploadedFiles = Request.Files[i];
                //if (_FileControlIndex < Request.Files.Count)
                //    break;
                HttpPostedFile UploadedFiles = null;
                if ((_ModuleName == Configuration.Auctions || _ModuleName == Configuration.Accounts) && _CTRLID.Contains("_"))
                {
                    int Index = _CTRLID.IndexOf('_');
                    int ID = Convert.ToInt32(_CTRLID.Substring(Index + 1));
                    UploadedFiles = Request.Files[ID];
                }
                else
                {
                    UploadedFiles = Request.Files[_FileControlIndex];
                }

                FullFileName = UploadedFiles.FileName;
                FileExt = UploadedFiles.ContentType;
                if (_ModuleName == Configuration.ScheduleInspection)
                {
                    if (FileExt == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || FileExt == "application/pdf" || FileExt == "Doc")
                    {
                        return list;
                    }

                }
                if (FileExt == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileExt = "Docx";
                }
                if (FullFileName == "")
                {
                    if (oFileUpload != null)
                    {
                        FullFileName = oFileUpload.FileName;
                        if (oFileUpload.PostedFile != null)
                            FileExt = oFileUpload.PostedFile.ContentType;
                    }
                    if (FileExt == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        FileExt = "Docx";
                    }
                    PostedFile = false;
                }
                if (FullFileName.Trim() == "")
                    continue;
                System.IO.FileInfo oFileInfo = new System.IO.FileInfo(FullFileName);
                string NewFileName = sGuid + "_" + oFileInfo.Name;
                FileName = Path.GetFileNameWithoutExtension(oFileInfo.Name);

                string filePath = Utility.GetImagePath(_ModuleName);
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    filePath = filePath + "\\" + NewFileName;

                    if (PostedFile == false)
                    {
                        oFileUpload.SaveAs(filePath);
                    }
                    else
                    {
                        UploadedFiles.SaveAs(filePath);
                    }

                }
                list.Add(new Tuple<string, string, string>(FileName, FileExt, NewFileName));
            }



            return list;
        }

        public void UploadedFilesNames(string _ModuleName, List<string> _LstFileNames)
        {
            try
            {
                List<string> URLS = new List<string>();

                for (int i = 0; i < _LstFileNames.Count; i++)
                {
                    //Generating URLs through file name
                    URLS.Add(Utility.GetImageURL(_ModuleName, _LstFileNames[i]));
                }

                for (int i = 0; i < Size; ++i)
                {
                    HtmlTableRow TableRow = new HtmlTableRow();
                    HtmlTableCell TableCell = new HtmlTableCell();

                    //splitting File original name from its full name
                    int index = _LstFileNames[i].IndexOf('_');
                    string FileName = _LstFileNames[i].Substring(index + 1);
                    HtmlGenericControl anchor = new HtmlGenericControl("a");
                    anchor.Attributes.Add("href", URLS[i]);
                    anchor.InnerText = FileName;
                    TableCell.Controls.Add(anchor);
                    TableRow.Controls.Add(TableCell);
                    tblFileUpload.Rows.Add(TableRow);


                }
            }
            catch (Exception)
            {

                throw;
            }


        }


        public void ViewUploadedFilesAsThumbnail(string _ModuleName, List<string> _LstFileNames)
        {
            try
            {
                List<string> URLS = new List<string>();

                for (int i = 0; i < _LstFileNames.Count; i++)
                {
                    //Generating URLs through file name
                    URLS.Add(Utility.GetImageURL(_ModuleName, _LstFileNames[i]));
                }

                for (int i = 0; i < Size; i++)
                {
                    HtmlTableRow TableRow = new HtmlTableRow();
                    HtmlTableCell TableCell = new HtmlTableCell();

                    //splitting File original name from its full name
                    int index = _LstFileNames[i].IndexOf('_');
                    string FileName = _LstFileNames[i].Substring(index + 1);
                    var parts = FileName.Split('.');
                    string ext = parts[1].ToString();
                    if (ext.Trim().ToUpper() != "DOC" && ext.Trim().ToUpper() != "DOCX" && ext.Trim().ToUpper() != "PDF")
                    {
                        if (Mode == Convert.ToInt32(Constants.ModeValue.Thumbnail))
                        {

                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            HImage.Attributes.Add("src", URLS[i]);
                            HImage.Attributes.Add("class", "imagePreview");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);


                            //  TableRow.Controls.Add(TableCell);
                            //  tblFileUpload.Rows.Add(TableRow);


                        }


                    }
                    else
                    {
                        if (ext.Trim().ToUpper() != "DOC" && ext.Trim().ToUpper() != "DOCX")
                        {
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            //  HImage.Attributes.Add("src", URLS[i]);
                            //  HImage.Attributes.Add("class", "imagePreview");
                            HImage.Attributes.Add("src", "/Design/img/icons_24/pdf.png");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);
                        }
                        else
                        {
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            //  HImage.Attributes.Add("src", URLS[i]);
                            //    HImage.Attributes.Add("class", "imagePreview");
                            HImage.Attributes.Add("src", "/Design/img/icons_24/ms-word.png");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);

                        }



                        //HtmlGenericControl anchor = new HtmlGenericControl("a");
                        //anchor.Attributes.Add("href", URLS[i]);
                        //anchor.InnerText = FileName;
                        //TableCell.Controls.Add(anchor);
                        //TableRow.Controls.Add(TableCell);
                        //tblFileUpload.Rows.Add(TableRow);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void ViewUploadedFilesAsThumbnailHorizontally(string _ModuleName, List<string> _LstFileNames)
        {
            try
            {
                List<string> URLS = new List<string>();

                for (int i = 0; i < _LstFileNames.Count; i++)
                {
                    //Generating URLs through file name
                    URLS.Add(Utility.GetImageURL(_ModuleName, _LstFileNames[i]));
                }
                HtmlTableRow TableRow = new HtmlTableRow();
                for (int i = 0; i < Size; i++)
                {

                    HtmlTableCell TableCell = new HtmlTableCell();

                    //splitting File original name from its full name
                    int index = _LstFileNames[i].IndexOf('_');
                    string FileName = _LstFileNames[i].Substring(index + 1);
                    var parts = FileName.Split('.');
                    string ext = parts[1].ToString();
                    if (ext.Trim().ToUpper() != "DOC" && ext.Trim().ToUpper() != "DOCX" && ext.Trim().ToUpper() != "PDF")
                    {
                        if (Mode == Convert.ToInt32(Constants.ModeValue.Thumbnail))
                        {

                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            HImage.Attributes.Add("src", URLS[i]);
                            HImage.Attributes.Add("class", "imagePreview");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            TableCell.Style.Add("padding-left", "4px;");
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);


                            //  TableRow.Controls.Add(TableCell);
                            //  tblFileUpload.Rows.Add(TableRow);


                        }


                    }
                    else
                    {
                        if (ext.Trim().ToUpper() != "DOC" && ext.Trim().ToUpper() != "DOCX")
                        {
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            //  HImage.Attributes.Add("src", URLS[i]);
                            //  HImage.Attributes.Add("class", "imagePreview");
                            HImage.Attributes.Add("src", "/Design/img/icons_24/pdf.png");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            TableCell.Style.Add("padding-left", "4px;");
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);
                        }
                        else
                        {
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            HtmlGenericControl HImage = new HtmlGenericControl("img");
                            //  HImage.Attributes.Add("src", URLS[i]);
                            //    HImage.Attributes.Add("class", "imagePreview");
                            HImage.Attributes.Add("src", "/Design/img/icons_24/ms-word.png");
                            anchor.Attributes.Add("href", URLS[i]);
                            //  anchor.InnerText = FileName;
                            anchor.Controls.Add(HImage);
                            TableCell.Controls.Add(anchor);
                            TableCell.Style.Add("padding-left", "4px;");
                            // TableCell.Controls.Add(anchor);
                            TableRow.Controls.Add(TableCell);
                            tblFileUpload.Rows.Add(TableRow);

                        }



                        //HtmlGenericControl anchor = new HtmlGenericControl("a");
                        //anchor.Attributes.Add("href", URLS[i]);
                        //anchor.InnerText = FileName;
                        //TableCell.Controls.Add(anchor);
                        //TableRow.Controls.Add(TableCell);
                        //tblFileUpload.Rows.Add(TableRow);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}