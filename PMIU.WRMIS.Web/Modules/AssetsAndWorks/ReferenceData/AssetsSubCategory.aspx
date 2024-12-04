<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="AssetsSubCategory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData.AssetsSubCategory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>--%>
            <div class="box">
                <div class="box-title">
                    <h3>Asset Sub-Category</h3>

                </div>
                <div class="box-content" style="min-height: 48px;">

                    <div class="col-md-6" style="margin-bottom: 10px;">

                        <label for="lblCategory" id="lblDivision" style="margin-left: -20px;" class="col-sm-4 col-lg-3  control-label">Asset Category</label>
                        <div class="col-sm-8 col-lg-9 controls">
                            <asp:DropDownList TabIndex="3" ID="ddlCategory" AutoPostBack="true" required="required" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvAssetSubCategory" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                            OnRowCommand="gvAssetSubCategory_RowCommand" OnPageIndexChanging="gvAssetSubCategory_PageIndexChanging" OnRowDataBound="gvAssetSubCategory_RowDataBound"
                            OnRowUpdating="gvAssetSubCategory_RowUpdating" OnRowDeleting="gvAssetSubCategory_RowDeleting"
                            OnRowCancelingEdit="gvAssetSubCategory_RowCancelingEdit" 
                            OnRowEditing="gvAssetSubCategory_RowEditing" OnPageIndexChanged="gvAssetSubCategory_PageIndexChanged"
                            EmptyDataText="No record found" CssClass="table header" BorderWidth="0px"
                            CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Sub-Category" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control required" required="required" MaxLength="30" oninput="javascript:AlphabetsWithLengthValidation(this,'2')" value='<%# Eval("Name") %>'  Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" Visible="true">
                                    <HeaderStyle CssClass="col-md-4" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="50" onfocus="this.value = this.value;" value='<%# Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flood Association" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFoBit" runat="server" CssClass="control-label" Text='<%# Eval("FOBit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="ChkFOBit" runat="server" Text="" Checked='<%# Eval("FOBit") %>' />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
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
                                    <HeaderStyle CssClass="col-md-2" />
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" Visible="<%# base.CanAdd %>" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlAssetsAttribute" runat="server" ToolTip="Attribute" CssClass="btn btn-primary btn_32 details" NavigateUrl='<%#String.Format("~/Modules/AssetsAndWorks/ReferenceData/AssetsAttributes.aspx?SubCatgID={0}&SubName={1}",Eval("ID"), Eval("Name"))%>'>
                                                </asp:HyperLink>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
  <%--      </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
