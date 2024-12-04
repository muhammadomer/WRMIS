<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddGeneralInspectionType.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.Reference_Data.AddGeneralInspectionType" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>General Inspection Types</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvGeneralInspectionTypes" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" DataKeyNames="CreatedBy,CreatedDate"
                            OnRowCommand="gvGeneralInspectionTypes_RowCommand"
                            OnRowCancelingEdit="gvGeneralInspectionTypes_RowCancelingEdit" OnRowUpdating="gvGeneralInspectionTypes_RowUpdating"
                            OnRowEditing="gvGeneralInspectionTypes_RowEditing" OnRowDeleting="gvGeneralInspectionTypes_RowDeleting"
                            OnPageIndexChanging="gvGeneralInspectionTypes_PageIndexChanging" OnPageIndexChanged="gvGeneralInspectionTypes_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Inspection Name">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label required" required="required" Text='<%#Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required"  MaxLength="90" CssClass="form-control required" placeholder="Enter Inspection Name" Text='<%#Eval("Name") %>' Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-md-5" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%#Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="250" CssClass="form-control" placeholder="Enter Description" Text='<%#Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
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
