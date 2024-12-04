<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WorkProgessHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.WorkProgessHistory" %> 
<%@ MasterType VirtualPath="~/Site.Master" %> 
 <%@ Register Src="~/Modules/ClosureOperations/UserControls/CWInfo.ascx" TagPrefix="ucCWPInfo" TagName="ucCWPInfoControl" %>  
<asp:content contentplaceholderid="MainContent" runat="server">  
    <div class="row">
        <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3>Progress History</h3>
            </div>
            
            <div class="box-content"> 
               <asp:HiddenField ID="hdnF_CWID" Value="" runat="server" />
                <ucCWPInfo:ucCWPInfoControl runat="server" ID="ucInfo" />
               
                <div class="form-horizontal">
                    
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Work Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlWorkStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Inspected by" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlInspectedBy" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div> 
                    </div>  
                    
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtFromDate" TabIndex="4"  runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtToDate" TabIndex="4" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSearch"  runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btnSearch_Click"/>  
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div> 
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvCWP" runat="server" CssClass="table header" AllowSorting="false"
                                    AutoGenerateColumns="False" EmptyDataText="No record found" GridLines="None" PageSize="20"
                                    ShowHeaderWhenEmpty="true" OnRowCommand="gvCWP_RowCommand" AllowPaging="true"
                                    OnPageIndexChanged="gvCWP_PageIndexChanged" OnPageIndexChanging="gvCWP_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inspection Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inspected By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>                                           
                                        <asp:TemplateField HeaderText="Work Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Progress (%)" >
                                            <ItemTemplate>
                                            <asp:Label ID="lblPrgPrcntg" runat="server" Text='<%# Eval("PrgPrcntg") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 text-center" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField> 
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center"> 
                                                    <asp:Button ID="btnDetail" data-toggle="modal" runat="server" Text="" CommandName="Detail" CssClass="btn btn-primary btn_32 view" ToolTip="Detail" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>

                                    </Columns> 
                                </asp:GridView>
                            </div>
                        </div>
                    </div> 
                    
                </div> 
            </div>
        </div>
        </div>
    </div>  
</asp:content>
