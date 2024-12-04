<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="District.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.District" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>District</h3>
                    
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvDistrict" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" 
                            OnRowCommand="gvDistrict_RowCommand"
                            OnRowCancelingEdit="gvDistrict_RowCancelingEdit" OnRowUpdating="gvDistrict_RowUpdating"
                            OnRowEditing="gvDistrict_RowEditing" OnRowDeleting="gvDistrict_RowDeleting" OnRowCreated="gvDistrict_RowCreated"
                            OnPageIndexChanging="gvDistrict_PageIndexChanging" OnPageIndexChanged="gvDistrict_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" >
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="District Name">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrictName" runat="server" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDistrictName" runat="server" required="required" onfocus="this.value = this.value;" MaxLength="75" CssClass="form-control required" placeholder="Enter District Name" Text='<%#Eval("Name") %>' Width="90%" Onkeyup="InputValidation(this);" ClientIDMode="Static"/>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-md-8" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrictDescription" runat="server" CssClass="control-label" Text='<%#Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDistrictDescription" runat="server" MaxLength="150" CssClass="form-control" placeholder="Enter Description" Text='<%#Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <headerstyle cssclass="col-md-1" />
                                    
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="btn btn-danger btn_32 delete" ToolTip="delete"></asp:Button>
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
