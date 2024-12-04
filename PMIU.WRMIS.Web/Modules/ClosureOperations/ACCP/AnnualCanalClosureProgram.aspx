<%@ Page Title=""  Language="C#" MasterPageFile ="~/Site.Master" AutoEventWireup="true" CodeBehind="AnnualCanalClosureProgram.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP.AnnualCanalClosureProgram" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Annual Canal Closure Programme</h3>
                </div>
                <div class="box-content"> 
                    <div class="form-horizontal">
                        <div class="row" id="searchDiv">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server"  >
                                            <asp:ListItem Text="ALL" Value=""/>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>  
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btnSearch_Click"/>  
                                     <asp:HyperLink ID="hlAddACCP" Visible="<%# base.CanAdd %>" runat="server" NavigateUrl="~/Modules/ClosureOperations/ACCP/AddACCP.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvACCP" runat="server" CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" 
                                        PageSize="20" EmptyDataText="No record found" AllowSorting="false" GridLines="None" 
                                        ShowHeaderWhenEmpty="true" OnRowDataBound="gvACCP_RowDataBound" OnRowCreated="gvACCP_RowCreated" OnPageIndexChanging="gvACCP_PageIndexChanging"
                                        OnRowCommand="gvACCP_RowCommand">
                                        <Columns>
                                             <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                     <asp:HiddenField ID="attachmentName" Value ='<%# Eval("Attachment") %>' runat="server"/>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Title">
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
                                            <asp:TemplateField HeaderText="Attachment">
                                                <ItemTemplate>
                                                  
                                                     <asp:HyperLink ID="hlImage"  Text='View Attachment'   NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.ClosureOperations , Convert.ToString(Eval("Attachment"))) %>'  runat="server" /> 
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:HyperLink ID="hlDetails" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Details" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# Eval("ID", "~/Modules/ClosureOperations/ACCP/ACCPDetails.aspx?ACCPID={0}")%>'></asp:HyperLink>
                                                        <asp:HyperLink ID="btnOrder"  Enabled="<%# base.CanEdit %>" runat="server" Text="" NavigateUrl='<%# Eval("ID", "~/Modules/ClosureOperations/ACCP/ACCPOrders.aspx?ACCPID={0}")%>'     CssClass="btn btn-primary btn_24 order" Style="height: 24px;width: 29px;" ToolTip="Orders" />
                                                        <asp:Button ID="btnPrint" Enabled="<%# base.CanPrint%>" runat="server" formnovalidate="formnovalidate" ToolTip="Print"  CssClass="btn btn-primary btn_24 print" Style="height: 24px;width: 29px;" CommandName="PrintReport" />
                                                        <asp:HyperLink ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("ID", "~/Modules/ClosureOperations/ACCP/AddACCP.aspx?ACCPID={0}")%>'>
                                                        </asp:HyperLink>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />  
                                              <ItemStyle Width="285px" HorizontalAlign="Right" /> 
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
</asp:Content>