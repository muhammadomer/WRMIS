<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClosureOperationPlanDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.ClosureOperationPlanDetail" %>

<%@ MasterType VirtualPath="~/Site.Master" %> 
<%@ Register Src="~/Modules/ClosureOperations/UserControls/CWInfo.ascx" TagPrefix="ucCWPInfo" TagName="ucCWPInfoControl" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Closure Work Plan Details</h3>
                </div>
                <div class="box-content">
                    <ucCWPInfo:ucCWPInfoControl runat="server" ID="ucInfo" /> 
                    <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnPrint" Visible="<%# base.CanPrint %>" runat="server" Text="Print" CssClass="btn btn-primary" ToolTip="Print" OnClick="btnPrint_Click"/> 
                                    <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="btn btn-success" ToolTip="Publish" OnClientClick="return confirm('Are you sure you want to publish this plan?');" OnClick="btnPublish_Click"/>
                                    <asp:Button ID="btnUnPublish" runat="server" Text="Un-Publish" CssClass="btn btn-success" ToolTip="Un-Publish" OnClientClick="return confirm('Are you sure you want to un-publish this plan?');" OnClick="btnUnPublish_Click" Visible ="false"/>
                                </div>
                            </div>
                        </div> 
                    <div class="form-horizontal"> 
                        <div class="row">
                            <div class="col-md-12"> 
                                    <div class="table-responsive">
                                        <asp:HiddenField ID="hdnF_CWPID" Value="" runat="server" />
                                        <asp:GridView ID="gvCW" runat="server" CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" 
                                            AllowCustomPaging="true" PageSize="100" EmptyDataText="No record found" AllowSorting="false" GridLines="None" 
                                            ShowHeaderWhenEmpty="true" OnRowCreated="gvCW_RowCreated" OnRowDataBound="gvCW_RowDataBound" OnRowDeleting="gvCW_RowDeleting"
                                             >
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label> 
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-3" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Work Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemDesc" runat="server" Text='<%# Eval("WorkType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" /> 
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Work Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSancQty" runat="server" Text='<%# Eval("WorkName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" /> 
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="&nbsp;&nbsp; Estimated Cost (Rs.)">
                                                    <ItemTemplate>
                                                        <asp:Label  ID="lblUnit" runat="server" Text='<%# Eval("EstimatedCost") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="col-md-1 text-center"/>
                                                </asp:TemplateField>  
                                                <asp:TemplateField HeaderText="Sanctioned Amount (Rs.)">
                                                    <ItemTemplate>
                                                        <asp:Label  ID="lblSnctAmount" runat="server" Text='<%# Eval("SnctAmnt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="col-md-1 text-center"/>
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Awarded (Rs.)">
                                                    <ItemTemplate>
                                                        <asp:Label CssClass="text-right" ID="lblAmnt" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                                </asp:TemplateField>  
                                                <asp:TemplateField> 
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <HeaderTemplate>
                                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                            <asp:HyperLink ID="hlAddCW" runat="server" ToolTip="Add Closure Work" CssClass="btn btn-success btn_add plus" Text=""></asp:HyperLink>
                                                        </asp:Panel>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                            <asp:HyperLink ID="hlCWDetail" runat="server" ToolTip="Closure Work Details" CssClass="btn btn-primary btn_32 view" Text=""></asp:HyperLink>
                                                            <asp:HyperLink ID="hlCWItems" runat="server" ToolTip="Closure Work Items" CssClass="btn btn-primary btn_32 audit" Text=""></asp:HyperLink> 
                                                            <asp:HyperLink ID="hlAddPrgrs" runat="server" ToolTip="Add Progress" CssClass="btn btn-primary btn_32 AddPrgs" Text=""></asp:HyperLink> 
                                                            <asp:HyperLink ID="hlPrgrsHstry" runat="server" ToolTip="Progress History" CssClass="btn btn-primary btn_32 Prgs" Text=""></asp:HyperLink> 
                                                            <asp:HyperLink ID="hlEditCW" runat="server" ToolTip="Edit Closure Work" CssClass="btn btn-primary btn_32 edit" Text=""></asp:HyperLink> 
                                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
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
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn"> 
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" NavigateUrl="~/Modules/ClosureOperations/CWP/ClosureWorkPlan.aspx?RestoreState=1">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div> 
                     
                </div>
            </div>
        </div>
    </div> 
    
</asp:Content>