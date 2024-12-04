<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TenderOpeningOffice.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.ReferenceData.TenderOpeningOffice" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Tender Opening Office</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvTenderOpeningOffice" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvTenderOpeningOffice_RowCommand"
                            OnRowCancelingEdit="gvTenderOpeningOffice_RowCancelingEdit" OnRowUpdating="gvTenderOpeningOffice_RowUpdating"
                            OnRowEditing="gvTenderOpeningOffice_RowEditing" OnRowDeleting="gvTenderOpeningOffice_RowDeleting"
                            OnPageIndexChanging="gvTenderOpeningOffice_PageIndexChanging" OnPageIndexChanged="gvTenderOpeningOffice_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Office">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficeName" runat="server" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOfficeName" runat="server" required="required" onfocus="this.value = this.value;" MaxLength="90" CssClass="form-control required" placeholder="Enter Office" Text='<%#Eval("Name") %>' Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address">
                                    <HeaderStyle CssClass="col-md-8" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" CssClass="control-label" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="150" CssClass="form-control" placeholder="Enter Address" Text='<%#Eval("Address") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-1" />

                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="btn btn-danger btn_32 delete" ToolTip="delete" Visible="<%# base.CanDelete %>"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
