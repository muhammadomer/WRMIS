<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplaintsType.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.ReferenceData.ComplaintsType" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    
            <div class="box">
                <div class="box-title">
                    <h3>Complaints Type</h3>
                    
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvComplaintsType" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" 
                            OnRowCommand="gvComplaintsType_RowCommand" OnRowDataBound="gvComplaintsType_RowDataBound"
                            OnRowCancelingEdit="gvComplaintsType_RowCancelingEdit" OnRowUpdating="gvComplaintsType_RowUpdating"
                            OnRowEditing="gvComplaintsType_RowEditing" OnRowDeleting="gvComplaintsType_RowDeleting"
                            OnPageIndexChanging="gvComplaintsType_PageIndexChanging" OnPageIndexChanged="gvComplaintsType_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" >
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Complaint Type">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblComplaintType" runat="server" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtComplaintType" runat="server" required="required" onfocus="this.value = this.value;" MaxLength="75" CssClass="form-control required" placeholder="Enter Complaint Type" Text='<%#Eval("Name") %>' Width="90%" Onkeyup="InputValidation(this);" ClientIDMode="Static"/>
                                        <asp:Label ID="elblComplaintType" runat="server" Visible="false" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Response Duration">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblResponseTime" runat="server" CssClass="control-label" Text='<%#Eval("ResponseTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtResponseTime" runat="server" CssClass="form-control integerInput" placeholder="Enter Response Time" Text='<%#Eval("ResponseTime") %>' ClientIDMode="Static"/>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Complaint Details">
                                    <HeaderStyle CssClass="col-md-8" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblComplaintDescription" runat="server" CssClass="control-label" Text='<%#Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtComplaintDescription" runat="server" MaxLength="150" CssClass="form-control" placeholder="Enter Description" Text='<%#Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <headerstyle cssclass="col-md-1" />
                                    
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>"></asp:Button>
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
       
</asp:Content>

