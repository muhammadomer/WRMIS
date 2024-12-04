<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivedStockInfrastructures.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ReceivedStockDivisionStore" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
     <asp:HiddenField ID="InfrastructureTypeID" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Division Store Received Stock</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" AutoPostBack="True" required="required">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblReceivedStockType" runat="server" Text="Received Stock Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlReceivedStockType" runat="server" CssClass=" rquired form-control" required="true">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass=" rquired form-control" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass=" required form-control" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlInfrastructureName_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblItemCategory" runat="server" Text="Item Category" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList runat="server" ID="ddlItemCategory" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvReceivedStockInfrastructure" runat="server" DataKeyNames="ItemId,ItemName,ReceivedQty,DSID"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true">
                       <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Item Name" ItemStyle-Width="12%" />
                                <asp:TemplateField HeaderText="Last Quantity Issued">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_flood" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-right" />
                                    <ItemStyle CssClass="integerInput" Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Received">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Received_Qty" runat="server" Text='<%# Eval("ReceivedQty") %>' pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8" Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemStyle CssClass="text-right" Width="7%" />
                                </asp:TemplateField>
                            </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" Enabled="false" OnClick="btnSave_Click" />
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>

</asp:Content>
