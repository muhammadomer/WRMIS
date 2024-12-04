<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IssuedNewStockInfrastructureCampSite.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.IssuedNewStockInfrastructureCampSite" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPCampSiteID" runat="server" Value="0" />
     <asp:HiddenField ID="InfrastructureTypeID" runat="server" Value="0" />
     <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Issued Item</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
               
                <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Item Issue to:" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                  <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rdolStatus_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="5">Infrastructure</asp:ListItem>
                                        <asp:ListItem Value="6">Camp Site</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                 <div runat="server" id="divInfra" visible="false">
                   <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lbl" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control required" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblitm" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control required" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlInfrastructureName_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCampSite" runat="server" Text="Item Category" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList runat="server" ID="ddlItemCategory" CssClass="form-control required" AutoPostBack="True" required="required" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                <div class="table-responsive">
                
                    <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemId,ItemName,IssuedQty,DSID,ItemSubCategoryID,AvailableIssuedCount,QuantityAvailableInStore"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-2" />
                            <asp:TemplateField HeaderText="Quantity Available in Store">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_StoreQty" runat="server" Text='<%# Eval("QuantityAvailableInStore") %>'></asp:Label>
                                    <asp:Label ID="lbl_StoreQtyAsset" Visible="false" runat="server" Text='<%# Eval("QuantityAvailableInStore") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Quantity Required">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ApprovedQty" runat="server" Text='<%# Eval("QuantityApproved") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currently Issued">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_IssuedQty" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Quantity Issued">
                                <ItemTemplate>
                                 <asp:TextBox ID="txt_Qty" runat="server"  pattern="^(0|[0-9][0-9]*)$" class="integerInput form-control" MaxLength="8"></asp:TextBox>
                                 <asp:CheckBox ID="chk_Qty" runat="server" CssClass="control-label"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
                 </div>
                <div runat="server" id="divCampSite" visible="false">

               
                <div class="table-responsive">
                 
                        <asp:GridView ID="gvIssuedStockCampSite" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="InfraStructureName,StructureType,RD,StructureTypeID,StructureID,FFPcampSiteID" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvIssuedStockCampSite_PageIndexChanging" OnRowDataBound="gvIssuedStockCampSite_RowDataBound" OnRowCommand="gvIssuedStockCampSite_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("StructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("StructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfrastructureName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <ItemTemplate>
                                         <asp:LinkButton ID="hlDetails" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
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
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
             </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
