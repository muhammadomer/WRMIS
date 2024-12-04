<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AssetsCategory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData.AssetsCategory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Asset Category</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvAssetCategory" runat="server" DataKeyNames="ID,Name,Description,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvAssetCategory_RowCommand" OnRowDataBound="gvAssetCategory_RowDataBound"
                            OnRowEditing="gvAssetCategory_RowEditing" OnRowCancelingEdit="gvAssetCategory_RowCancelingEdit"
                            OnRowUpdating="gvAssetCategory_RowUpdating"
                            OnRowDeleting="gvAssetCategory_RowDeleting" OnPageIndexChanging="gvAssetCategory_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Asset Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" oninput="javascript:AlphabetsWithLengthValidation(this,'2')" MaxLength="30" class="required form-control" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" class="form-control" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="ChkActive" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                     <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddCategory" runat="server" Text="" CommandName="AddCategory" Visible="<%# base.CanAdd %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditCategory" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteCategory" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionCategory" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveCategory" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
