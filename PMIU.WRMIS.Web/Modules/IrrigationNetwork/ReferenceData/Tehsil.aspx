<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tehsil.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Tehsil" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Tehsil</h3>
                    
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblDistrict" runat="server" Text="District" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" required="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvTehsil" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            Visible="false" CssClass="table header"
                            OnRowCommand="gvTehsil_RowCommand"
                            OnRowCancelingEdit="gvTehsil_RowCancelingEdit" OnRowUpdating="gvTehsil_RowUpdating"
                            OnRowEditing="gvTehsil_RowEditing" OnRowDeleting="gvTehsil_RowDeleting" OnRowCreated="gvTehsil_RowCreated"
                            OnPageIndexChanging="gvTehsil_PageIndexChanging" OnPageIndexChanged="gvTehsil_PageIndexChanged">

                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tehsil Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTehsilName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTehsilName" runat="server" required="required" MaxLength="30" onfocus="this.value = this.value;" CssClass="form-control required" placeholder="Enter Tehsil Name" Text='<%# Eval("Name") %>' Width="90%" Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTehsilDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTehsilDesc" runat="server" MaxLength="100" CssClass="form-control" placeholder="Enter Tehsil Description" Text='<%# Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-8" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
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
