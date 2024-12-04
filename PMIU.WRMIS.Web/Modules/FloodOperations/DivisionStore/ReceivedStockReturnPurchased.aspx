<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivedStockReturnPurchased.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ReceivedStockReturnPurchased" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Division Store Received Stock</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="box-content">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="rquired form-control required" AutoPostBack="True" required="required">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="Received Stock Type" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlStockType" runat="server" CssClass=" rquired form-control" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlStockType_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div runat="server" id="div_Purchased" visible="false">


                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lbl" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass=" rquired form-control" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblitm" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="rquired form-control" required="required" OnSelectedIndexChanged="ddlInfrastructureName_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div runat="server" id="Div_category" visible="false">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCampSite" runat="server" Text="Item Category" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList runat="server" ID="ddlItemCategory" CssClass="rquired form-control" AutoPostBack="True" required="required" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br/>
                    <div runat="server" id="Div_GvPurchased" visible="false">
                        <div class="table-responsive">
                            <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemId,ItemName,ReceivedQty,DSID"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItems_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="12%" />
                                    <asp:TemplateField HeaderText="Quantity Purchased">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_flood" runat="server" Text='<%# Eval("PurchasedQty") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Last Quantity Received" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ReceivedQty" runat="server" Text='<%# Eval("ReceivedQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-right" />
                                        <ItemStyle CssClass="integerInput" Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUns" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity Received">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Received_Qty" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8" Width="100%"></asp:TextBox>
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
                    <div runat="server" id="Div_GvInfraStruct">

                        <div class="table-responsive">
                            <asp:GridView ID="gvReceivedStockInfrastructure" runat="server" DataKeyNames="ItemId,ItemName,ReceivedQty,DSID,IssuedQty,ItemSubCategoryID,AvailableIssuedCount"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvReceivedStockInfrastructure_PageIndexChanging" OnRowDataBound="gvReceivedStockInfrastructure_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="12%" />
                                    <asp:TemplateField HeaderText="Last Quantity Issued">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_flood" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>      
                                            <asp:Label ID="lbl_floodAsset" Visible="false" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>                                       
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

                                    <asp:TemplateField HeaderText="Currently Received">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ReceivedQtyInfrastructure" runat="server" Text='<%# Eval("ReceivedQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-right" />
                                        <ItemStyle CssClass="integerInput" Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUinf" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity Received">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Received_Qty" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8" Width="100%"></asp:TextBox>
                                            <asp:CheckBox ID="chk_Qty" runat="server" CssClass="control-label"></asp:CheckBox>
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
                    <div runat="server" id="Div_GvCampSite" visible="false">
                        <div class="table-responsive">
                            <asp:GridView ID="gvCampSite" runat="server" DataKeyNames="InfraStructureName,StructureType,RD,StructureTypeID,StructureID,FFPcampSiteID"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvCampSite_RowDataBound" OnRowCommand="gvCampSite_RowCommand" OnPageIndexChanging="gvCampSite_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%# (Convert.ToString(Eval("StructureType"))) == "Control Structure1" ? "Barrage/Headwork": Eval("StructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfraStructureName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"></ItemStyle>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hlDetails" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary" Text="Items">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-center" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>

                    <div runat="server" id="Div_GvFFP" visible="false">
                        <div class="table-responsive">
                            <asp:GridView ID="gvFFP" runat="server" DataKeyNames="ItemId,ItemName,ReceivedQty,DSID,FFPCampSiteID"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvFFP_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-2" />
                                    <asp:TemplateField HeaderText="Current Quantity in Store">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_StoreQty" runat="server" Text='<%# Eval("QuantityAvailableInStore") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity Approved Flood Fighting plan">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ApprovedQty" runat="server" Text='<%# Eval("QuantityApprovedFFP") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Quantity Received" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ReceivedQtyFFP" runat="server" Text='<%# Eval("ReceivedQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity Received">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Received_QtyFFP" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput form-control" MaxLength="8"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>

                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" Enabled="false" OnClick="btnSave_Click" />
                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                </div>
            </div>
            <asp:HiddenField ID="hdnID" runat="server" Value="0" />
            <asp:HiddenField ID="InfrastructureTypeID" runat="server" Value="0" />
        </div>

    </div>
</asp:Content>
