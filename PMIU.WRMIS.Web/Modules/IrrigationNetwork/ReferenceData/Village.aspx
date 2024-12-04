<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Village.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Village" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Village</h3>
                    
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblDistrict" runat="server" Text="District" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblPoliceStation" runat="server" Text="Police Station" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlPoliceStation" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlPoliceStation_SelectedIndexChanged" Enabled="False">
                                            <asp:ListItem Value="0">Select</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblTehsil" runat="server" Text="Tehsil" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlTehsil" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged" Enabled="False">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvVillage" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            ShowHeaderWhenEmpty="True" OnRowCommand="gvVillage_RowCommand"
                            OnRowCancelingEdit="gvVillage_RowCancelingEdit" OnRowUpdating="gvVillage_RowUpdating" OnRowCreated="gvVillage_RowCreated"
                            OnRowEditing="gvVillage_RowEditing" OnRowDeleting="gvVillage_RowDeleting" AllowPaging="True"
                            OnPageIndexChanging="gvVillage_PageIndexChanging" OnPageIndexChanged="gvVillage_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Village">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVillageName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtVillageName" runat="server" required="required" onfocus="this.value = this.value;" MaxLength="100" CssClass="form-control required" placeholder="Enter Village Name" Text='<%# Eval("Name") %>' Width="90%" Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVillageDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtVillageDesc" runat="server" MaxLength="150" CssClass="form-control" placeholder="Enter Village Description" Text='<%# Eval("Description") %>' Width="90%" />
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
                                            <asp:Button ID="btnEdit" runat="server"  CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
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
