<%@ Page Title="SRM Inflow forecasting" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SRMInflowForecasting.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.SRMInflowForecasting" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>SRM Inflow Forecasting</h3>
        </div>
    </div>


    <div id="divView" runat="server" visible="false">
        <asp:GridView ID="gvView" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
            OnRowEditing="gvView_RowEditing" OnRowDeleting="gvView_RowDeleting" ShowHeaderWhenEmpty="True" CssClass="table header"
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
        <asp:Button ID="btnBackViewDiv" Text="Back" OnClick="btnBackViewDiv_Click" runat="server" CssClass="btn btn-primary" />
    </div>

    <div id="divStep1" runat="server">
        <asp:UpdatePanel UpdateMode="Conditional" RenderMode="Inline" runat="server">
            <ContentTemplate>
                DraftName:<asp:TextBox ID="txtName" required="required" runat="server" CssClass="form-control required"></asp:TextBox>
                <h4>Maximum</h4>
                <asp:Table ID="tblMax" runat="server" CssClass="table tbl-info">
                    <asp:TableRow>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text=""></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblKharifMax" runat="server" Text="E.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMKharifMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMKharifMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMKharifMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMKharifMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITKharifMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITKharifMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNKharifMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNKharifMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trLKMax" runat="server">
                        <asp:TableCell>
                            <asp:Label ID="lblLKMax" runat="server" Text="L.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMLKMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMLKMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMLKMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMLKMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITLKMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITLKMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNLKMax" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNLKMax_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <h4>Minimum</h4>
                <asp:Table ID="tblMin" runat="server" CssClass="table tbl-info">
                    <asp:TableRow>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text=""></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblKharifMin" runat="server" Text="E.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMKharifMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMKharifMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMKharifMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMKharifMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITKharifMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITKharifMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNKharifMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNKharifMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trLKMin" runat="server">
                        <asp:TableCell>
                            <asp:Label ID="lblLKMin" runat="server" Text="L.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMLKMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMLKMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMLKMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMLKMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITLKMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITLKMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNLKMin" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNLKMin_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <h4>Most Likely</h4>
                <asp:Table ID="tblML" runat="server" CssClass="table tbl-info">
                    <asp:TableRow>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text=""></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                    <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblKharifML" runat="server" Text="E.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMKharifML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMKharifML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMKharifML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMKharifML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITKharifML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITKharifML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNKharifML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNKharifML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trLKML" runat="server">
                        <asp:TableCell>
                            <asp:Label ID="lblLKML" runat="server" Text="L.K (MAF)"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtJMLKML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtJMLKML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCMLKML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtCMLKML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtITLKML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtITLKML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtKNLKML" runat="server" AutoPostBack="true" CssClass="form-control" MaxLength="7" OnTextChanged="txtKNLKML_TextChanged"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnForecast" runat="server" CssClass="btn btn-primary" Text="Forecast" OnClick="btnForecast_Click" />
        <asp:Button ID="btnBackStep1" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBackViewDiv_Click" />
    </div>

    <div id="divStepforecast" runat="server" visible="false">
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
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
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
                        <div class="box-content" style="display: none;">
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
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
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
                        <div class="box-content" style="display: none;">
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
                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblJMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Chenab at Marala"></asp:Label><br />
                                                <asp:Label ID="lblCMKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCMLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCM" runat="server" CssClass="control-label" Text='<%# Eval("ChenabMarala") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblCMKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label><br />
                                                <asp:Label ID="lblITKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblITLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIT" runat="server" CssClass="control-label" Text='<%# Eval("IndusTarbela") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblITKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label><br />
                                                <asp:Label ID="lblKNKahrifPer" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblKNLKPer" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblKN" runat="server" CssClass="control-label" Text='<%# Eval("KabulNowshera") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKNKharif" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNLK" runat="server" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="col-md-2" />
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
        <asp:Button ID="btnbackStepForecast" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnbackStepForecast_Click" />
        <asp:Button ID="btnBackToView" Visible="false" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBackToView_Click" />
    </div>
</asp:Content>

