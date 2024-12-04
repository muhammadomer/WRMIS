<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AttachmentsJoint.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint.AttachmentsJoint" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucJointInspectionDetail" TagName="JointInspectionDetail" Src="~/Modules/FloodOperations/Controls/JointInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Attachments for Joint Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucJointInspectionDetail:JointInspectionDetail runat="server" ID="JointInspectionDetail1" />
            <div class="table-responsive">
                <asp:GridView ID="gvAttachmentsJointInspection" runat="server" DataKeyNames="ID,FileURL,CreatedDate,CreatedBy"
                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                    EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                    OnRowCommand="gvAttachmentsJointInspection_RowCommand" OnRowDataBound="gvAttachmentsJointInspection_RowDataBound"
                    OnRowEditing="gvAttachmentsJointInspection_RowEditing" OnRowCancelingEdit="gvAttachmentsJointInspection_RowCancelingEdit"
                    OnRowUpdating="gvAttachmentsJointInspection_RowUpdating"
                    OnRowDeleting="gvAttachmentsJointInspection_RowDeleting" OnPageIndexChanging="gvAttachmentsJointInspection_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypFileName" runat="server" Text='<%# Eval("FileURL")%>'></asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Browse">
                            <ItemTemplate>
                                <asp:FileUpload runat="server" ID="selectedFile" CssClass="form-control" required="true" />
                     
                                <%--<asp:Button ID="btnFileUpload" Text="Browse" runat="server" OnClientClick="checkUncheckHeaderCheckBox()" Style="width: 70px" />--%>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-4" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAddAttachmentsJointInspection" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAddAttachmentsJointInspection" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddAttachmentsJointInspection" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlActionAttachmentsJointInspection" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEditAttachmentsJointInspection" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                    <%--<asp:Button ID="btnDeleteAttachmentsJointInspection" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />--%>
                                </asp:Panel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditActionAttachmentsJointInspection" runat="server" HorizontalAlign="Center">
                                    <asp:Button runat="server" ID="btnSaveAttachmentsJointInspection" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                    <asp:Button ID="btnCancelAttachmentsJointInspection" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };


    </script>
</asp:Content>
