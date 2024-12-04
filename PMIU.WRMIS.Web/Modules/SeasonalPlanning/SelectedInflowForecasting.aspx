<%@ Page Title="Selected Inflow forecasting" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SelectedInflowForecasting.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.SelectedInflowForecasting" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Selected Inflow Forecasting</h3>
        </div>
    </div>

    <div id="divView" runat="server" visible="false">
        <asp:GridView ID="gvView" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
            OnRowDeleting="gvView_RowDeleting" OnRowEditing="gvView_RowEditing" ShowHeaderWhenEmpty="True" CssClass="table header"
            BorderWidth="0px" CellSpacing="-1" GridLines="None">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Statistical Inflow Forecasted Drafts">
                    <ItemTemplate>
                        <asp:Label ID="lblRoleDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-8" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 view" ToolTip="View"></asp:LinkButton>
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:LinkButton>
                        </asp:Panel>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle CssClass="PagerStyle" />
        </asp:GridView>
        <asp:Button ID="btnBackViewDiv" Text="Back" runat="server" CssClass="btn btn-primary" OnClick="btnBackViewDiv_Click" />
    </div>

    <div id="divStep1DraftSelection" runat="server">
        DraftName:<asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="required"></asp:TextBox>
        <br />
        SRM Draft:
        <asp:Label ID="lblSRMName" runat="server" CssClass="control-label" Font-Bold="true"></asp:Label>
        <br />
        <br />
        <div class="box-content">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:GridView ID="gvStatisticalDrafts" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            ShowFooter="true" ShowHeader="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Statistical Draft(s)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                        <asp:RadioButton ID="rbDrafts" GroupName="StatDrafts" OnCheckedChanged="rbDrafts_CheckedChanged" AutoPostBack="true" runat="server" CssClass="control-label" />
                                        <asp:Label ID="lblDraftsName" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <asp:Button ID="btnOk" runat="server" CssClass="btn btn-primary" Text="Ok" OnClick="btnOk_Click" />
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
    </div>

    <div id="divStep2InflowsSelection" runat="server" visible="false">
        <asp:Panel ID="PnlForecast" runat="server" Visible="true" ClientIDMode="Static">
            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Maximum</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i2" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMaxCombined" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMaxCombined_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMStaticticalMax" runat="server" Checked="true" ClientIDMode="Static" OnClick="checkJhelumAtManglaStatistical();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMStaticticalMax" runat="server" Checked="true" ClientIDMode="Static" OnClick="checkChenabAtMaralaStatistical();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITStaticticalMax" runat="server" Checked="true" ClientIDMode="Static" OnClick="checkIndusAtTarbelaStatistical();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNStaticticalMax" runat="server" Checked="true" ClientIDMode="Static" OnClick="checkKabulAtNowsheraStatistical();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label Text="" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMSRMMax" runat="server" ClientIDMode="Static" OnClick="checkJhelumAtManglaSRM();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJMSRM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumManglaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotalSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMSRMMax" runat="server" ClientIDMode="Static" OnClick="checkChenabAtMaralaSRM();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCMSRM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMaralaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITSRMMax" runat="server" ClientIDMode="Static" OnClick="checkIndusAtTarbelaSRM();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblITSRM" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbelaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNSRMMax" runat="server" ClientIDMode="Static" OnClick="checkKabulAtNowsheraSRM();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKNSRM" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowsheraSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Minimum</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i3" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMinCombined" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMinCombined_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMStatisticalMin" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMStatisticalMin" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITStatisticalMin" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNStatisticalMin" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label Text="" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMSRMMin" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJMSRM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumManglaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMSRMMin" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCMSRM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMaralaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITSRMMin" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblITSRM" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbelaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNSRMMin" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKNSRM" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowsheraSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Most Likely</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i4" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvLikelyCombined" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvLikelyCombined_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMStatisticalML" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMStatisticalML" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITStatisticalML" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNStatisticalML" Checked="true" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label Text="" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbJMSRMML" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJMSRM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumManglaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbCMSRMML" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCMSRM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMaralaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbITSRMML" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblITSRM" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbelaSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPerSRM" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPerSRM" runat="server"></asp:Label><br />
                                                <asp:CheckBox ID="cbKNSRMML" runat="server" Enabled="false" ClientIDMode="Static" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKNSRM" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowsheraSRM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharifSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLKSRM" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotalSRM" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <asp:Button ID="btnFinalize" runat="server" CssClass="btn btn-primary" Text="Finalize Flow Forecast" OnClick="btnFinalize_Click" />
        <asp:Button ID="btnSelectionBack" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnSelectionBack_Click" />
    </div>

    <div id="divStep3forecast" runat="server" visible="false">
        <asp:Panel ID="Panel1" runat="server" Visible="true" ClientIDMode="Static">
            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Maximum</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i1" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMax" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMax_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Minimum</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i5" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMin" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMin_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Most Likely</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="i6" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvLikely" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvLikely_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotalLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNDraft" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnViewBack" Visible="false" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnViewBack_Click" />
    </div>

    <asp:HiddenField ID="hfStatDraftID" runat="server" />

    <script type="text/javascript">

        function checkJhelumAtManglaStatistical() {

            var JM = document.getElementById("cbJMStaticticalMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbJMStatisticalMin").checked = true;
                JM = document.getElementById("cbJMStatisticalML").checked = true;
                JM = document.getElementById("cbJMSRMMax").checked = false;
                JM = document.getElementById("cbJMSRMMin").checked = false;
                JM = document.getElementById("cbJMSRMML").checked = false;
            }
            else {
                JM = document.getElementById("cbJMStatisticalMin").checked = false;
                JM = document.getElementById("cbJMStatisticalML").checked = false;
            }
        }

        function checkJhelumAtManglaSRM() {

            var JM = document.getElementById("cbJMSRMMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbJMStatisticalMin").checked = false;
                JM = document.getElementById("cbJMStatisticalML").checked = false;
                JM = document.getElementById("cbJMStaticticalMax").checked = false;
                JM = document.getElementById("cbJMSRMMin").checked = true;
                JM = document.getElementById("cbJMSRMML").checked = true;
            }
            else {
                JM = document.getElementById("cbJMSRMMin").checked = false;
                JM = document.getElementById("cbJMSRMML").checked = false;
            }
        }

        function checkChenabAtMaralaStatistical() {
            var JM = document.getElementById("cbCMStaticticalMax").checked;
            if (JM == true) {
                JM = document.getElementById("cbCMStatisticalMin").checked = true;
                JM = document.getElementById("cbCMStatisticalML").checked = true;
                JM = document.getElementById("cbCMSRMMax").checked = false;
                JM = document.getElementById("cbCMSRMMin").checked = false;
                JM = document.getElementById("cbCMSRMML").checked = false;
            }
            else {
                JM = document.getElementById("cbCMStatisticalMin").checked = false;
                JM = document.getElementById("cbCMStatisticalML").checked = false;
            }
        }

        function checkChenabAtMaralaSRM() {

            var JM = document.getElementById("cbCMSRMMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbCMStatisticalMin").checked = false;
                JM = document.getElementById("cbCMStatisticalML").checked = false;
                JM = document.getElementById("cbCMStaticticalMax").checked = false;
                JM = document.getElementById("cbCMSRMMin").checked = true;
                JM = document.getElementById("cbCMSRMML").checked = true;
            }
            else {
                JM = document.getElementById("cbCMSRMMin").checked = false;
                JM = document.getElementById("cbCMSRMML").checked = false;
            }
        }

        function checkIndusAtTarbelaStatistical() {

            var JM = document.getElementById("cbITStaticticalMax").checked;
            if (JM == true) {
                JM = document.getElementById("cbITStatisticalMin").checked = true;
                JM = document.getElementById("cbITStatisticalML").checked = true;
                JM = document.getElementById("cbITSRMMax").checked = false;
                JM = document.getElementById("cbITSRMMin").checked = false;
                JM = document.getElementById("cbITSRMML").checked = false;
            }
            else {
                JM = document.getElementById("cbITStatisticalMin").checked = false;
                JM = document.getElementById("cbITStatisticalML").checked = false;
            }
        }

        function checkIndusAtTarbelaSRM() {

            var JM = document.getElementById("cbITSRMMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbITStatisticalMin").checked = false;
                JM = document.getElementById("cbITStatisticalML").checked = false;
                JM = document.getElementById("cbITStaticticalMax").checked = false;
                JM = document.getElementById("cbITSRMMin").checked = true;
                JM = document.getElementById("cbITSRMML").checked = true;
            }
            else {
                JM = document.getElementById("cbITSRMMin").checked = false;
                JM = document.getElementById("cbITSRMML").checked = false;
            }
        }

        function checkKabulAtNowsheraStatistical() {

            var JM = document.getElementById("cbKNStaticticalMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbKNStatisticalMin").checked = true;
                JM = document.getElementById("cbKNStatisticalML").checked = true;
                JM = document.getElementById("cbKNSRMMax").checked = false;
                JM = document.getElementById("cbKNSRMMin").checked = false;
                JM = document.getElementById("cbKNSRMML").checked = false;
            }
            else {
                JM = document.getElementById("cbKNStatisticalMin").checked = false;
                JM = document.getElementById("cbKNStatisticalML").checked = false;
            }
        }

        function checkKabulAtNowsheraSRM() {

            var JM = document.getElementById("cbKNSRMMax").checked;

            if (JM == true) {
                JM = document.getElementById("cbKNStatisticalMin").checked = false;
                JM = document.getElementById("cbKNStatisticalML").checked = false;
                JM = document.getElementById("cbKNStaticticalMax").checked = false;
                JM = document.getElementById("cbKNSRMMin").checked = true;
                JM = document.getElementById("cbKNSRMML").checked = true;
            }
            else {
                JM = document.getElementById("cbKNSRMMin").checked = false;
                JM = document.getElementById("cbKNSRMML").checked = false;
            }
        }
    </script>
</asp:Content>


