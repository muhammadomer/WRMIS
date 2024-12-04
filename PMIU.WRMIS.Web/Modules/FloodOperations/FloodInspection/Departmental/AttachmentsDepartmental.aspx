<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AttachmentsDepartmental.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental.AttachmentsDepartmental" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDepartmentalInspectionDetail" TagName="DepartmentalInspectionDetail" Src="~/Modules/FloodOperations/Controls/DepartmentalInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Attachments for Departmental Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucDepartmentalInspectionDetail:DepartmentalInspectionDetail runat="server" ID="DepartmentalInspectionDetail1" />
            <div class="table-responsive">
                <%-- <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMain">--%>
                <%--<contenttemplate>--%>
                <asp:GridView ID="gvAttachmentsDFI" runat="server" DataKeyNames="ID,FileName,CreatedDate,CreatedBy"
                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                    EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                    OnRowCommand="gvAttachmentsDFI_RowCommand" OnRowDataBound="gvAttachmentsDFI_RowDataBound"
                    OnRowEditing="gvAttachmentsDFI_RowEditing" OnRowCancelingEdit="gvAttachmentsDFI_RowCancelingEdit"
                    OnRowUpdating="gvAttachmentsDFI_RowUpdating"
                    OnRowDeleting="gvAttachmentsDFI_RowDeleting"
                    OnPageIndexChanging="gvAttachmentsDFI_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlFileName" runat="server" CssClass="btn btn-link" Text='<%# Eval("FileName")%>'></asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Browse">
                            <ItemTemplate>
                                <asp:FileUpload ID="FileUpload2" runat="server" CssClass="form-control" required="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-4" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAddAttachments" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAddAttachments" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddAttachments" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlActionAttachments" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEditAttachments" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                    <%--<asp:Button ID="btnDeleteAttachments" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />--%>
                                </asp:Panel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditActionAttachments" runat="server" HorizontalAlign="Center">
                                    <asp:Button runat="server" ID="btnSaveAttachments" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                    <asp:Button ID="btnCancelAttachments" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
                <%--</contenttemplate>--%>
                <triggers>
                    <asp:PostBackTrigger ControlId="FileUpload2" />
                </triggers>
                <%--</asp:UpdatePanel>--%>
            </div>
            <%--onclick="history.go(-1);return false;"--%>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CheckFileType(name) {

            var filename = $(name).val();
            if (filename != "") {
                debugger;
                var lastIndex = filename.lastIndexOf("\\");
                if (lastIndex >= 0) {
                    filename = filename.substring(lastIndex + 1);
                }
                var FileExt = filename.split('.').pop();
                if (FileExt == "JPG" || FileExt == "JPEG" || FileExt == "PNG" || FileExt == "GIF" ||
                    FileExt == "jpg" || FileExt == "jpeg" || FileExt == "png" || FileExt == "gif" ||
                    FileExt == "docx" || FileExt == "doc" || FileExt == "pdf" ||
                    FileExt == "DOCX" || FileExt == "DOC" || FileExt == "PDF") {
                    if (name.files[0].size > 5242880) {
                        alert("Maximum 5 MB length is allowed");
                        name.value = '';
                        return false;
                    }
                    if (name.className.substring(0, 9) == "CtrlClass") {
                        $('.CtrlClass0').blur();
                        $('.CtrlClass0').removeAttr('required');
                        if ($('#AttachFileCount').val() == '') {
                            $('#AttachFileCount').val(1);
                        }
                        else {
                            var count = $('#AttachFileCount').val();
                            count++;
                            $('#AttachFileCount').val(count);
                        }
                    }
                    else if (name.className.substring(0, 7) == "FIRCtrl") {
                        $('.FIRCtrl0').blur();
                        $('.FIRCtrl0').removeAttr('required');
                        if ($('#FIRCtrlCount').val() == '') {
                            $('#FIRCtrlCount').val(1);
                        }
                        else {
                            var count = $('#FIRCtrlCount').val();
                            count++;
                            $('#FIRCtrlCount').val(count);
                        }
                    }
                    else if (name.className.substring(0, 8) == "DesnCtrl") {
                        $('.DesnCtrl0').blur();
                        $('.DesnCtrl0').removeAttr('required');
                        if ($('#DesnCtrlCount').val() == '') {
                            $('#DesnCtrlCount').val(1);
                        }
                        else {
                            var count = $('#DesnCtrlCount').val();
                            count++;
                            $('#DesnCtrlCount').val(count);
                        }
                    }
                    else if (name.className.substring(0, 9) == "AttchCtrl") {
                        $('.AttchCtrl0').blur();
                        $('.AttchCtrl0').removeAttr('required');
                        if ($('#AttchCtrlCount').val() == '') {
                            $('#AttchCtrlCount').val(1);
                        }
                        else {
                            var count = $('#AttchCtrlCount').val();
                            count++;
                            $('#AttchCtrlCount').val(count);
                        }
                    }

                    return true;
                }

                else if (filename == '') {
                }
                else {
                    alert("This types of file not allowed");
                    name.value = '';
                    return false;
                }
            }
            else {
                if (filename == "") {
                    if (name.className.substring(0, 9) == "CtrlClass") {
                        var AttachFileCount = $('#AttachFileCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.CtrlClass0').blur();
                                $('.CtrlClass0').prop('required', true);
                            }
                            $('#AttachFileCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 7) == "FIRCtrl") {
                        var AttachFileCount = $('#FIRCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.FIRCtrl0').blur();
                                $('.FIRCtrl0').prop('required', true);
                            }
                            $('#FIRCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 8) == "DesnCtrl") {
                        var AttachFileCount = $('#DesnCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.DesnCtrl0').blur();
                                $('.DesnCtrl0').prop('required', true);
                            }
                            $('#DesnCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 9) == "AttchCtrl") {
                        var AttachFileCount = $('#AttchCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.AttchCtrl0').blur();
                                $('.AttchCtrl0').prop('required', true);
                            }
                            $('#AttchCtrlCount').val(AttachFileCount);
                        }
                    }
                }
            }
        }

    </script>
</asp:Content>
