<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntitlementOnDeliveries.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.EntitlementOnDeliveries" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Literal ID="litTitle" runat="server" Text="Entitlement On Deliveries" />
                    </h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Command</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" ID="ddlCommand" runat="server" TabIndex="1" required="true" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server" id="dvScenario" visible="false">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Scenario</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" ID="ddlScenario" runat="server" TabIndex="2" required="true" OnSelectedIndexChanged="ddlScenario_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Select" Value="" />
                                            <asp:ListItem Text="Maximum" Value="1" />
                                            <asp:ListItem Text="Minimum" Value="2" />
                                            <asp:ListItem Text="Likely" Value="3" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                            <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="text-primary text-center">
                                <asp:Label ID="lblEdit" runat="server" />
                            </asp:Panel>
                            <br />
                            <div class="row">
                                <div class="col-md-12 text-center">
                                    <asp:Label ID="lblMainDesc" runat="server" Text="Tarbela Command Entitlement for Rabi 2016-17" Font-Bold="true" />
                                </div>
                            </div>
                            <br />
                            <asp:Panel ID="pnlRabiHeader" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4">
                                        Average System Usage Rabi 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782Average" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row" id="dvRabiPara2" runat="server">
                                    <div class="col-md-4">
                                        Punjab Para(2) Rabi Share (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblEntitlementText" runat="server" Text="Entitlement for Rabi 2016-2017 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEntitlement" runat="server" Text="23.185" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        % Change in Rabi w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlKharifHeader" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4">
                                        Average System Usage Early Kharif 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782EKAverage" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        Average System Usage Late Kharif 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782LKAverage" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row" id="dvKharifPara2" runat="server">
                                    <div class="col-md-4">
                                        Punjab Para(2) Early Kharif Share (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        Punjab Para(2) Late Kharif Share (MAF):  
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblEKEntitlementText" runat="server" Text="Entitlement for Early Kharif 2016 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKEntitlement" runat="server" Text="23.185" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblLKEntitlementText" runat="server" Text="Entitlement for Late Kharif 2016 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKEntitlement" runat="server" Text="23.185" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        % Change in Early Kharif w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        % Change in Late Kharif w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                            </asp:Panel>
                            <br />
                            <label style="font-weight: bold;">Note: All values in MAF except as noted.</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvRabiAverage" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="False" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" Visible="false" ShowFooter="true" OnRowDataBound="gvRabiAverage_RowDataBound"
                                            DataKeyNames="PercentageFiveYr, PercentageTenYr, Percentage7782, CommandChannelID, SeasonalAverageID" ClientIDMode="Static">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Channel Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        Total
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                    <FooterStyle CssClass="text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHMAF5y" runat="server" CssClass="control-label" Text="Average 5-Year" />
                                                        <asp:RadioButton ID="rb5y" runat="server" CssClass="control-label" GroupName="Entitlement" Checked="true" OnCheckedChanged="rb5y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMAF5y" runat="server" CssClass="control-label" Text='<%# Eval("MAFFiveYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalMAF5y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHMAF10y" runat="server" CssClass="control-label" Text="Average 10-Year" />
                                                        <asp:RadioButton ID="rb10y" runat="server" CssClass="control-label" GroupName="Entitlement" OnCheckedChanged="rb10y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMAF10y" runat="server" CssClass="control-label" Text='<%# Eval("MAFTenYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalMAF10y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHMAF7782" runat="server" CssClass="control-label" Text="1977-1982" />
                                                        <asp:RadioButton ID="rb7782" runat="server" CssClass="control-label" GroupName="Entitlement" OnCheckedChanged="rb7782_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMAF7782" runat="server" CssClass="control-label" Text='<%# Eval("MAF7782") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalMAF7782" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:RadioButton ID="rbSYear" runat="server" Style="float: right; margin-left: 5px;" CssClass="control-label" GroupName="Entitlement" OnCheckedChanged="rbSYear_CheckedChanged" AutoPostBack="true" />
                                                        <asp:DropDownList CssClass="form-control required" Style="float: right;" ID="ddlSelectedYear" Width="50%" runat="server" TabIndex="5" required="true" OnSelectedIndexChanged="ddlSelectedYear_SelectedIndexChanged" AutoPostBack="true" />
                                                        <asp:Label ID="hlblSYearMAF" runat="server" Style="float: right; margin-right: 5px" CssClass="control-label" Text="Year" />
                                                        <div style="clear: right;"></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSYearMAF" runat="server" CssClass="control-label" Text='<%# Eval("SelectedYearMAF") %>'></asp:Label>
                                                        <asp:Label ID="lblSYearPer" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("SelectedYearPercentage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblSYearTotalMAF" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHEntitlement" runat="server" CssClass="control-label" Text="Rabi 2016-2017" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtChannelEntitlement" runat="server" CssClass="form-control decimalInput" Text='<%# Eval("ChannelEntitlement") %>' AutoPostBack="true" OnTextChanged="txtChannelEntitlement_TextChanged" required="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnChannelEntitlement" runat="server" Value='<%# Eval("ChannelEntitlement") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalChannelEntitlement" Text="0" ClientIDMode="Static" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvKharifAverage" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="False" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" Visible="false" ShowFooter="true" OnRowDataBound="gvKharifAverage_RowDataBound"
                                            DataKeyNames="EKPercentageFiveYr, EKPercentageTenYr, EKPercentage7782, LKPercentageFiveYr, LKPercentageTenYr, 
                                            LKPercentage7782, CommandChannelID, EKSeasonalAverageID, LKSeasonalAverageID">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Channel Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        Total
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <FooterStyle CssClass="text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEKHMAF5y" runat="server" CssClass="control-label" Text="EK Average 5-Year" />
                                                        <asp:RadioButton ID="rbEK5y" runat="server" CssClass="control-label" GroupName="EKEntitlement" Checked="true" OnCheckedChanged="rbEK5y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEKMAF5y" runat="server" CssClass="control-label" Text='<%# Eval("EKMAFFiveYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalEKMAF5y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEKHMAF10y" runat="server" CssClass="control-label" Text="EK Average 10-Year" />
                                                        <asp:RadioButton ID="rbEK10y" runat="server" CssClass="control-label" GroupName="EKEntitlement" OnCheckedChanged="rbEK10y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEKMAF10y" runat="server" CssClass="control-label" Text='<%# Eval("EKMAFTenYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalEKMAF10y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEKHMAF7782" runat="server" CssClass="control-label" Text="EK 77-82" />
                                                        <asp:RadioButton ID="rbEK7782" runat="server" CssClass="control-label" GroupName="EKEntitlement" OnCheckedChanged="rbEK7782_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEKMAF7782" runat="server" CssClass="control-label" Text='<%# Eval("EKMAF7782") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalEKMAF7782" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:RadioButton ID="rbEKYear" runat="server" Style="float: right; margin-left: 5px;" CssClass="control-label" GroupName="EKEntitlement" OnCheckedChanged="rbEKYear_CheckedChanged" AutoPostBack="true" />
                                                        <asp:DropDownList CssClass="form-control" Style="float: right;" ID="ddlEKSelectedYear" Width="55%" runat="server" OnSelectedIndexChanged="ddlEKSelectedYear_SelectedIndexChanged" AutoPostBack="true" />
                                                        <asp:Label ID="hlblEKYearMAF" runat="server" Style="float: right; margin-right: 5px" CssClass="control-label" Text="Year" />
                                                        <div style="clear: right;"></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEKYearMAF" runat="server" CssClass="control-label" Text='<%# Eval("SelectedYearMAFEK") %>'></asp:Label>
                                                        <asp:Label ID="lblEKYearPer" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("SelectedYearPercentageEK") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblEKYearTotalMAF" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEKHEntitlement" runat="server" CssClass="control-label" Text="Early Kharif 2016" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEKChannelEntitlement" runat="server" CssClass="form-control decimalInput" Text='<%# Eval("EKChannelEntitlement") %>' AutoPostBack="true" OnTextChanged="txtEKChannelEntitlement_TextChanged" required="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnEKChannelEntitlement" runat="server" Value='<%# Eval("EKChannelEntitlement") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalEKChannelEntitlement" Text="0" ClientIDMode="Static" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLKHMAF5y" runat="server" CssClass="control-label" Text="LK Average 5-Year" />
                                                        <asp:RadioButton ID="rbLK5y" runat="server" CssClass="control-label" GroupName="LKEntitlement" Checked="true" OnCheckedChanged="rbLK5y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLKMAF5y" runat="server" CssClass="control-label" Text='<%# Eval("LKMAFFiveYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLKMAF5y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLKHMAF10y" runat="server" CssClass="control-label" Text="LK Average 10-Year" />
                                                        <asp:RadioButton ID="rbLK10y" runat="server" CssClass="control-label" GroupName="LKEntitlement" OnCheckedChanged="rbLK10y_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLKMAF10y" runat="server" CssClass="control-label" Text='<%# Eval("LKMAFTenYr") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLKMAF10y" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLKHMAF7782" runat="server" CssClass="control-label" Text="LK 77-82" />
                                                        <asp:RadioButton ID="rbLK7782" runat="server" CssClass="control-label" GroupName="LKEntitlement" OnCheckedChanged="rbLK7782_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLKMAF7782" runat="server" CssClass="control-label" Text='<%# Eval("LKMAF7782") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLKMAF7782" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:RadioButton ID="rbLKYear" runat="server" Style="float: right; margin-left: 5px;" CssClass="control-label" GroupName="LKEntitlement" OnCheckedChanged="rbLKYear_CheckedChanged" AutoPostBack="true" />
                                                        <asp:DropDownList CssClass="form-control" Style="float: right;" ID="ddlLKSelectedYear" Width="55%" runat="server"  OnSelectedIndexChanged="ddlLKSelectedYear_SelectedIndexChanged" AutoPostBack="true" />
                                                        <asp:Label ID="hlblLKYearMAF" runat="server" Style="float: right; margin-right: 5px" CssClass="control-label" Text="Year" />
                                                        <div style="clear: right;"></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLKYearMAF" runat="server" CssClass="control-label" Text='<%# Eval("SelectedYearMAFLK") %>'></asp:Label>
                                                        <asp:Label ID="lblLKYearPer" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("SelectedYearPercentageLK") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblLKYearTotalMAF" Text="0" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLKHEntitlement" runat="server" CssClass="control-label" Text="Late Kharif 2016" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLKChannelEntitlement" runat="server" CssClass="form-control decimalInput" Text='<%# Eval("LKChannelEntitlement") %>' AutoPostBack="true" OnTextChanged="txtLKChannelEntitlement_TextChanged" required="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnLKChannelEntitlement" runat="server" Value='<%# Eval("LKChannelEntitlement") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLKChannelEntitlement" Text="0" ClientIDMode="Static" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                    <FooterStyle CssClass="text-right text-bold" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" ClientIDMode="Static" />
                                        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" ClientIDMode="Static" Visible="false" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#btnSave').click(function () {

                if ($('#gvRabiAverage').length > 0) {

                    var EntitlementShare = parseFloat($('#lblEntitlement').text());
                    var GeneratedEntitlement = parseFloat($('#lblTotalChannelEntitlement').text());

                    //if (EntitlementShare > GeneratedEntitlement) {
                    //    return confirm('Entered Rabi Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    //}

                    if (EntitlementShare > GeneratedEntitlement) {
                        return confirm('Entered Rabi Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (EntitlementShare < GeneratedEntitlement) {
                        return confirm('Entered Rabi Entitlement is more than its Provincial Entitlement. Are you sure you want to save it?');
                    }

                }
                else {

                    var EKEntitlementShare = parseFloat($('#lblEKEntitlement').text());
                    var EKGeneratedEntitlement = parseFloat($('#lblTotalEKChannelEntitlement').text());

                    var LKEntitlementShare = parseFloat($('#lblLKEntitlement').text());
                    var LKGeneratedEntitlement = parseFloat($('#lblTotalLKChannelEntitlement').text());

                    //if (EKEntitlementShare > EKGeneratedEntitlement && LKEntitlementShare <= LKGeneratedEntitlement) {
                    //    return confirm('Entered Early Kharif Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    //}
                    //else if (LKEntitlementShare > LKGeneratedEntitlement && EKEntitlementShare <= EKGeneratedEntitlement) {
                    //    return confirm('Entered Late Kharif Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    //}
                    //else if (EKEntitlementShare > EKGeneratedEntitlement && LKEntitlementShare > LKGeneratedEntitlement) {
                    //    return confirm('Entered Early and Late Kharif Entitlements are less than their Provincial Entitlements. Are you sure you want to save these?');
                    //}


                    if (EKEntitlementShare > EKGeneratedEntitlement && LKEntitlementShare == LKGeneratedEntitlement) {
                        return confirm('Entered Early Kharif Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (EKEntitlementShare < EKGeneratedEntitlement && LKEntitlementShare == LKGeneratedEntitlement) {
                        return confirm('Entered Early Kharif Entitlement is more than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (LKEntitlementShare > LKGeneratedEntitlement && EKEntitlementShare == EKGeneratedEntitlement) {
                        return confirm('Entered Late Kharif Entitlement is less than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (LKEntitlementShare < LKGeneratedEntitlement && EKEntitlementShare == EKGeneratedEntitlement) {
                        return confirm('Entered Late Kharif Entitlement is more than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (EKEntitlementShare > EKGeneratedEntitlement && LKEntitlementShare < LKGeneratedEntitlement) {
                        return confirm('Entered Early Kharif Entitlement is less than its Provincial Entitlement and Late Kharif Entitlement is more than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (LKEntitlementShare > LKGeneratedEntitlement && EKEntitlementShare < EKGeneratedEntitlement) {
                        return confirm('Entered Late Kharif Entitlement is less than its Provincial Entitlement and Early Kharif Entitlement is more than its Provincial Entitlement. Are you sure you want to save it?');
                    }
                    else if (EKEntitlementShare > EKGeneratedEntitlement && LKEntitlementShare > LKGeneratedEntitlement) {
                        return confirm('Entered Early and Late Kharif Entitlements are less than their Provincial Entitlements. Are you sure you want to save these?');
                    }
                    else if (EKEntitlementShare < EKGeneratedEntitlement && LKEntitlementShare < LKGeneratedEntitlement) {
                        return confirm('Entered Early and Late Kharif Entitlements are more than their Provincial Entitlements. Are you sure you want to save these?');
                    }
                }

                return true;

            });

        });

    </script>
</asp:Content>
