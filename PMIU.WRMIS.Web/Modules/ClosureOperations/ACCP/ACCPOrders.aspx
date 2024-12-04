<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ACCPOrders.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP.ACCPOrders" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucACCP" TagName="AccpTitleYear" Src="~/Modules/ClosureOperations/UserControls/ACCP.ascx" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnACCPID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnACCPStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Annual Canal Closure Programme Orders/Letters</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucACCP:AccpTitleYear runat="server" ID="ACCPID" />
            <div class="table-responsive">
                <%-- <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMain">--%>
                <%--<contenttemplate>--%>
                <asp:GridView ID="gvACCPOrder" runat="server" DataKeyNames="ID,FileName" PageSize="20"
                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                    EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                    OnRowCommand="gvACCPOrder_RowCommand" OnRowDataBound="gvACCPOrder_RowDataBound"
                    OnRowEditing="gvACCPOrder_RowEditing" OnRowCancelingEdit="gvACCPOrder_RowCancelingEdit"
                    OnRowUpdating="gvACCPOrder_RowUpdating"
                    OnRowDeleting="gvACCPOrder_RowDeleting"
                    OnPageIndexChanging="gvACCPOrder_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="btn btn-link" Text='<%# Eval("ID")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="140px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Letter No">
                            <ItemTemplate>
                                <asp:Label ID="lblLatterNo" runat="server" CssClass="control-label" Text='<%# Eval("LatterNo")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <asp:TextBox runat="server" ID="txtLatterNo" required="required"  CssClass="form-control required" />
                            </EditItemTemplate>
                            <HeaderStyle Width="140px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Letter Date">
                            <ItemTemplate>
                                <asp:Label ID="lblLatterDate" runat="server" CssClass="control-label" Text='<%# Eval("LatterDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <div>
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtDate" TabIndex="5"  runat="server" class="form-control required date-picker" size="16" type="text"  Text='<%#Eval("LatterDate", "{0:dd-MMM-yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle Width="140px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attachment">
                          <%--  <ItemTemplate>
                                <%--<uc1:FileUploadControl runat="server" ID="FileUpload2" Size="1" />
                                
                            </ItemTemplate>--%>
                                  <ItemTemplate>
                                        <asp:HyperLink ID="hlFileName" runat="server" CssClass="btn btn-link" Text='Attachment'   NavigateUrl='<%#PMIU.WRMIS.Common.Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.ClosureOperations , Convert.ToString(Eval("FileName"))) %>'> </asp:HyperLink>
                                    </ItemTemplate>
                                 <EditItemTemplate>
                                        <uc1:FileUploadControl runat="server" Mode="2" ID="FileUpload3" Size="1"  Visible="false" />
                                        <uc1:FileUploadControl runat="server" CssClass="form-control required"  ID="FileUpload2" Size="1" />
                                     <asp:HyperLink ID="hlFileName" runat="server" CssClass="btn btn-link"></asp:HyperLink>
                                 </EditItemTemplate>
                                    <HeaderStyle Width="140px" />
                                </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAddACCPOrder" runat="server"  Text="" Enabled="<%# base.CanAdd %>" CommandName="AddACCPOrder" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                </asp:Panel>
                            </HeaderTemplate>
                          <ItemTemplate>
                              <asp:Panel ID="pnlActionItemCategory" runat="server" HorizontalAlign="Center">
                                  <asp:Button ID="btnEditItemCategory" runat="server" Text="" CommandName="Edit"   Enabled="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                  <asp:Button ID="btnDeleteItemCategory" runat="server" Text="" CommandName="Delete"  Enabled="<%# base.CanDelete%>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                              </asp:Panel>
                          </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditActionItemCategory" runat="server" HorizontalAlign="Center">
                                    <asp:Button runat="server" ID="btnSaveItemCategory"  CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                    <asp:Button ID="btnCancelItemCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
            <br />

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

<script type="text/javascript">
    $(function ()
    {
       
        $(function HandlerFileUploader(clientID) {
            debugger;
            var div = document.getElementById('MainContent_gvACCPOrder');
            $(div).find('input:file').each(function () {
                if ($(this).id == (clientID)) {
                    $(this).visible = true;
                }
                else {
                    $(this).visible = false;
                }
            }
            );
        }
        );
    }
    );
   </script>

</asp:Content>
