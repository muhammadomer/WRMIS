<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Industry.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.Industry" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .integerInput {
            text-align: left;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Industries</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div id="divSearchCriteria">

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlDiv" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Industry Type</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlType" runat="server" AutoPostBack="true">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server" AutoPostBack="true">
                                                <asp:ListItem Text="All" Value="0" />
                                                <asp:ListItem Text="Functional" Value="1" />
                                                <asp:ListItem Text="Non Functional" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Industry No.</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox CssClass="integerInput form-control" MaxLength="10" ID="txtIndNo" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Industry Name</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control" ID="txtIndName" runat="server" MaxLength="100" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" id="searchButtonsDiv">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSearch" Visible="<%# base.CanView %>" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btn_Click" />
                                        <asp:Button ID="btnAdd" Visible="<%# base.CanAdd %>" runat="server" Text="Add New" CssClass="btn btn-success" ToolTip="Add new industry" OnClick="btn_Click" formnovalidate="formnovalidate" />
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div id="divData">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                                            OnPageIndexChanging="gv_PageIndexChanging"
                                            OnPageIndexChanged="gv_PageIndexChanged" EmptyDataText="No record found"
                                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                            ShowHeaderWhenEmpty="true" OnRowDeleting="gv_RowDeleting" OnRowCommand="gv_RowCommand" DataKeyNames="IndustryID,IndustryName,EWDivision,CWDivision">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Industry No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("IndustryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                                    <ItemStyle CssClass="text-center" />
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Industry Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("IndustryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Effluent Water Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEWDivision" runat="server" Text='<%# Eval("EWDivision") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Canal Water Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCWDivision" runat="server" Text='<%# Eval("CWDivision") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Eval("IndustryType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action">
                                                    <HeaderStyle CssClass="col-md-3 text-center" />
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                            <asp:HyperLink ID="hlSrvc" runat="server" ToolTip="Industry Services"
                                                                CssClass="btn btn-primary btn_24 services"
                                                                NavigateUrl='<%# Eval("IndustryID", "~/Modules/EWC/IndustryServices.aspx?ID={0}")%>'>
                                                            </asp:HyperLink>
                                                            <asp:HyperLink ID="hlSnctDschrg" runat="server" ToolTip="Sanctioned Discharge/Supply"
                                                                CssClass="btn btn-primary btn_24 sanctioned-charges"
                                                                NavigateUrl='<%# Eval("IndustryID", "~/Modules/EWC/AnnualScntdDschrgSuply.aspx?ID={0}")%>'>
                                                            </asp:HyperLink>
                                                            <%--  <asp:HyperLink ID="hlBilling" runat="server" ToolTip="Billing Detail"
                                                                CssClass="btn btn-primary btn_24 view"
                                                                NavigateUrl='<%#String.Format("~/Modules/EWC/BillDetail.aspx?ID={0}&Name={1}&EWDivision={2}&CWDivision={3}",Eval("IndustryID"), Eval("IndustryName"), Eval("EWDivision"), Eval("CWDivision"))%>'>

                                                            </asp:HyperLink>--%>
                                                            <asp:LinkButton ID="hlBilling" runat="server" ToolTip="Billing Details" CommandName="BillingDetail" CssClass="btn btn-primary btn_24 bill-details">
                                                            </asp:LinkButton>
                                                            <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("IndustryID","~/Modules/EWC/EditIndustry.aspx?IndustryID={0}")%>'>
                                                            </asp:HyperLink>
                                                            
                                                            <asp:Button ID="ButtonDelete" Visible="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
                                                        </asp:Panel>

                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
