<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClosureWorkPlan.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ClosureWorkPlan.ClosureWorkPlan" %>

<%@ MasterType VirtualPath="~/Site.Master" %> 

<asp:content id="ClosureWorkPlan_Screen" contentplaceholderid="MainContent" runat="server">     
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Closure Work Plan</h3>
                </div>
                <div class="box-content"> 
                    <div class="form-horizontal">
                        <div class="row" id="searchDiv">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control"  AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server"  CssClass="form-control" >
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>                            
                        </div>  
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" ToolTip="Search" />  
                                    <asp:Button ID="btnAddCWP" runat="server" CssClass="btn btn-success" Text="Add New" ToolTip="Add"  Enabled='<%# base.CanAdd %>' OnClick="btnAddCWP_Click" formnovalidate="formnovalidate"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvCWP" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" 
                                        OnPageIndexChanging="gvCWP_PageIndexChanging" OnRowDeleting="gvCWP_RowDeleting"  
                                        OnPageIndexChanged="gvCWP_PageIndexChanged" EmptyDataText="No record found" 
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" 
                                        ShowHeaderWhenEmpty="true" OnRowDataBound="gvCWP_RowDataBound" OnRowCreated="gvCWP_RowCreated">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Closure Work Plan Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Status" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="col-md-1" /> 
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:HyperLink ID="hlClosureWorkPlanDetail" runat="server" ToolTip="Details" 
                                                            CssClass="btn btn-primary btn_24 workdetails"  Text="" 
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/ClosureOperations/CWP/ClosureOperationPlanDetail.aspx?CWPID={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:HyperLink ID="hlEditClosureWorkPlan" runat="server" CssClass="btn btn-primary btn_24 edit" 
                                                            ToolTip="Edit" formnovalidate="formnovalidate" Text=""  Enabled='<%# base.CanEdit %>'
                                                            NavigateUrl='<%# Eval("ID", "~/Modules/ClosureOperations/CWP/AddClosureWorkPlan.aspx?CWPID={0}")%>'>
                                                        </asp:HyperLink>
                                                        <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' Enabled='<%# base.CanDelete %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
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
    <script type="text/javascript" src="../../Scripts/jquery-1.10.2.min.js"></script> 
</asp:content>
