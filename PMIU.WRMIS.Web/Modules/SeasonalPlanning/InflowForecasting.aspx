<%@ Page Title="Inflow Forecasting" MasterPageFile="~/Site.Master" Language="C#" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="InflowForecasting.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.InflowForecasting" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3 id="hName" runat="server">Inflow Forecasting</h3>
        </div>
        <br />
        <div id="divScenarioSelection" runat="server">
            <asp:RadioButton ID="RBStatistical" GroupName="rbtnSelection" Checked="true" runat="server" Text="Statistical" />
            <asp:RadioButton ID="RBSRM" GroupName="rbtnSelection" runat="server" Text="SRM Model" />
            <asp:RadioButton ID="RBSelected" GroupName="rbtnSelection" runat="server" Text="Selected" />
            <br />
            <asp:Button ID="btnView" Text="View" runat="server" CssClass="btn btn-primary" OnClick="btnView_Click" />
            <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
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

    <div id="divStep1" runat="server" visible="false">
        <h4>Step 1: Draft Name </h4>
        <asp:Label ID="lblName" runat="server" Text="Scenario Name"></asp:Label>
        <br />
        <asp:TextBox ID="txtName" runat="server" Width="50%" CssClass="form-control" type="text" MaxLength="50"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>

        <div class="table-responsive">
            <asp:Table ID="tableMAF" runat="server" CssClass="table tbl-info">

                <asp:TableRow>
                    <asp:TableHeaderCell>
                                    <asp:Label runat="server" Text="SR. No."></asp:Label>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                                    <asp:Label runat="server" Text="Rim Station"></asp:Label>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                                    <asp:Label runat="server" Text="Last Season (MAF)"></asp:Label>
                    </asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="1"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblJM" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="2"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCM" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="3"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblIT" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="4"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblKN" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <asp:Button ID="btnVariation" runat="server" CssClass="btn btn-primary" Text="Place Variation" ClientIDMode="Static" OnClick="btnVariation_Click" />
        <asp:Button ID="btnBackStep1" OnClick="btnBackStep1_Click" runat="server" CssClass="btn btn-primary" Text="Back" />
    </div>

    <div id="divStep2" runat="server" visible="false">
        <h4>Step 2: Allowable Variation in Inflows </h4>
        <asp:Label ID="lblStep2Period" runat="server" Text=""></asp:Label>

        <div class="table-responsive">
            <asp:Table ID="table1" runat="server" CssClass="table tbl-info">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="20%">
                                    <asp:Label runat="server" Text="Rim Station"></asp:Label>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="14%">
                                    <asp:Label runat="server" Text="Current (MAF)"></asp:Label>
                    </asp:TableHeaderCell>

                    <asp:TableHeaderCell Width="2%">
                                    <asp:Label runat="server" Text=""></asp:Label>
                    </asp:TableHeaderCell>

                    <asp:TableHeaderCell Width="12%">
                                    <asp:Label runat="server" Text="Starting Limit %"></asp:Label>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="14%">
                                    <asp:Label runat="server" Text="Starting Inflows"></asp:Label>
                    </asp:TableHeaderCell>

                    <asp:TableHeaderCell Width="2%">
                                    <asp:Label runat="server" Text=""></asp:Label>
                    </asp:TableHeaderCell>

                    <asp:TableHeaderCell Width="14%">
                                    <asp:Label runat="server" Text="Ending Limit %"></asp:Label>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="16%">
                                    <asp:Label runat="server" Text="Ending Inflows"></asp:Label>
                    </asp:TableHeaderCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblStep2JM" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="-"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtJMStartVariation" runat="server" Width="60%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblJMStartVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="+"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtJMEndVariation" runat="server" Width="50%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblJMEndVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblStep2CM" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="-"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCMStartVariation" runat="server" Width="60%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblCMStartVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="+"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCMEndVariation" runat="server" Width="50%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblCMEndVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblStep2IT" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="-"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtITStartVariation" runat="server" Width="60%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblITStartVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="+"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtITEndVariation" runat="server" Width="50%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblITEndVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                                    <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblStep2KN" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="-"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtKNStartVariation" runat="server" Width="60%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblKNStartVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="+"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtKNEndVariation" runat="server" Width="50%" ClientIDMode="Static" Text="5" MaxLength="4" onchange="ValidateValue(this);" CssClass="integerInput form-control" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="lblKNEndVariationFinal" ClientIDMode="Static" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <asp:Button ID="btnMatchInflows" runat="server" CssClass="btn btn-primary" Text="Matching Inflows" OnClick="btnMatchInflows_Click" />
        <asp:Button ID="btnBackPlaceVariation" runat="server" OnClick="btnBackPlaceVariation_Click" CssClass="btn btn-primary" Text="Back" />
    </div>

    <div id="divStep3" runat="server" visible="false">
        <h4>Step 3: Matching Inflows </h4>
        <asp:Label ID="Label1" runat="server" Text="Historic Inflows after placing variation into current MAF "></asp:Label>
        <br />

        <asp:Panel ID="pnlMatchInflows" runat="server" Visible="true" ClientIDMode="Static">
            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Jhelum at Mangla</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="iconJhelumAtMagla" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="divJhelumAtMangla" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvJhelumAtMangla" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="true" OnRowDataBound="gvJhelumAtMangla_RowDataBound" OnRowCreated="gvJhelumAtMangla_RowCreated">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selection">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Visible="false" />
                                                <asp:CheckBox ID="cbJM" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="cbJM_CheckedChanged" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="Average" Font-Bold="true" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Years">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYears" runat="server" CssClass="control-label" Text='<%# Eval("years") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rabi(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRabiJM" runat="server" CssClass="control-label" Text='<%# Eval("Rabi") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRabi" runat="server" CssClass="control-label" Font-Bold="true" Visible="false" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Early Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEKJM" runat="server" CssClass="control-label" Text='<%# Eval("Kharif") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" CssClass="control-label" Font-Bold="true" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Late Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLKJM" runat="server" CssClass="control-label" Text='<%# Eval("LK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblLK" runat="server" CssClass="control-label" Font-Bold="true" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
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
                            <h4 style="color: #fff;">Indus at Tarbela</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="iconIndusAtTarbela" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="div2IndusAtTarbela" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvIndusAtTarbela" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="true" OnRowDataBound="gvIndusAtTarbela_RowDataBound" OnRowCreated="gvIndusAtTarbela_RowCreated">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selection">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="1" Visible="false" />
                                                <asp:CheckBox ID="cbIT" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="cbIT_CheckedChanged" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Font-Bold="true" Text="Average" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Years">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYears" runat="server" CssClass="control-label" Text='<%# Eval("years") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rabi(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRabiIT" runat="server" CssClass="control-label" Text='<%# Eval("Rabi") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRabi" runat="server" CssClass="control-label" Font-Bold="true" Visible="false" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Early Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEKIT" runat="server" CssClass="control-label" Text='<%# Eval("Kharif") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Late Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLKIT" runat="server" CssClass="control-label" Text='<%# Eval("LK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblLK" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
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
                            <h4 style="color: #fff;">Chenab at Marala</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="iconChenabAtMarala" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="divChenabAtMarala" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvChenabAtMarala" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="true" OnRowDataBound="gvChenabAtMarala_RowDataBound" OnRowCreated="gvChenabAtMarala_RowCreated">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selection">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="1" Visible="false" />
                                                <asp:CheckBox ID="cbCM" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="cbCM_CheckedChanged" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Font-Bold="true" Text="Average" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Years">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYears" runat="server" CssClass="control-label" Text='<%# Eval("years") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rabi(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRabiCM" runat="server" CssClass="control-label" Text='<%# Eval("Rabi") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRabi" runat="server" CssClass="control-label" Font-Bold="true" Visible="false" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Early Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEKCM" runat="server" CssClass="control-label" Text='<%# Eval("Kharif") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Late Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLKCM" runat="server" CssClass="control-label" Text='<%# Eval("LK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblLK" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
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
                            <h4 style="color: #fff;">Kabul at Nowshera</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="iconKabulatNowshera" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="divKabulAtNowshera" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvKabulAtNowshera" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                    AllowPaging="false" ShowFooter="true" OnRowDataBound="gvKabulAtNowshera_RowDataBound" OnRowCreated="gvKabulAtNowshera_RowCreated">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selection">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="1" Visible="false" />
                                                <asp:CheckBox ID="cbKN" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="cbKN_CheckedChanged" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Font-Bold="true" Text="Average" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Years">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYears" runat="server" CssClass="control-label" Text='<%# Eval("years") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rabi(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRabiKN" runat="server" CssClass="control-label" Text='<%# Eval("Rabi") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRabi" runat="server" CssClass="control-label" Font-Bold="true" Visible="false" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Early Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEKKN" runat="server" CssClass="control-label" Text='<%# Eval("Kharif") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Late Kharif(MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLKKN" runat="server" CssClass="control-label" Text='<%# Eval("LK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblLK" runat="server" CssClass="control-label" Font-Bold="true" Text="No Data" Visible="true" />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
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
        <asp:Button ID="btnForecastProb" runat="server" CssClass="btn btn-primary" Text="Forecast Probability" OnClick="btnForecastProb_Click" />
        <asp:Button ID="btnBackMatchInflows" runat="server" CssClass="btn btn-primary" OnClick="btnBackMatchInflows_Click" Text="Back" />
    </div>

    <div id="divStep4" runat="server" visible="false">
        <h4>Step 4: Forecast Probability</h4>
        <asp:Panel ID="PnlForecastProb" runat="server" Visible="true" ClientIDMode="Static">
            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 id="hSeasonKharif" runat="server" style="color: #fff;">Early Kharif</h4>
                            <h4 id="hSeasonRabi" visible="false" runat="server" style="color: #fff;">Rabi</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="liEK" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="div1" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:Table ID="tblKharif" runat="server" CssClass="table tbl-info">
                                    <asp:TableRow>
                                        <asp:TableHeaderCell Width="20%">
                                    <asp:Label runat="server" Text="Rim Station"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                            <asp:Label runat="server" ID="Step4lblKharif" Text="E.K Average (MAF)"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="11%">
                                            <asp:Label runat="server" ID="lblMLProb" Text="Likely Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="2%">
                                    <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Variation %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="11%">
                                    <asp:Label runat="server" Text="Max Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="2%">
                                    <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Variation %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Min Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblKharifJM"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMLJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMaxJM" Text="-10" Width="50%" MaxLength="4" ClientIDMode="Static" CssClass="form-control NegativeintegerInput"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMaxJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMinJM" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMinJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblKharifCM"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMLCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMaxCM" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMaxCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMinCM" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMinCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblKharifIT"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMLIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMaxIT" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMaxIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMinIT" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMinIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblKharifKN"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMLKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMaxKN" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMaxKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtMinKN" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblMinKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="divLK" runat="server">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 1px;">
                        <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                            <h4 style="color: #fff;">Late Kharif</h4>
                            <div class="box-tool" style="top: 7px;">
                                <a data-action="collapse" href="#"><i id="I1" runat="server" class="fa fa-chevron-down"></i></a>
                            </div>
                        </div>
                        <div id="div2" runat="server" class="box-content">
                            <div class="table-responsive">
                                <asp:Table ID="Table2" runat="server" CssClass="table tbl-info">
                                    <asp:TableRow>
                                        <asp:TableHeaderCell Width="20%">
                                    <asp:Label runat="server" Text="Rim Station"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                        <asp:Label runat="server" Text="L.K Average (MAF)"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                        <asp:Label runat="server" Text="Likely Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="2%">
                                        <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Variation %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Max Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="2%">
                                        <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Variation %"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="13%">
                                    <asp:Label runat="server" Text="Min Prob %"></asp:Label>
                                        </asp:TableHeaderCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKJM"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMLJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMaxJM" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMaxJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMinJM" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMinJM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Chenab at Marala"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKCM"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMLCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMaxCM" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMaxCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMinCM" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMinCM" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Indus at Tarbela"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKIT"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMLIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMaxIT" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMaxIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMinIT" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMinIT" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                        <asp:Label runat="server" Text="Kabul at Nowshera"></asp:Label>
                                        </asp:TableHeaderCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKKN"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMLKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMaxKN" Text="-10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMaxKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>                                         
                                            <asp:Label runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="txtLKMinKN" Text="10" Width="50%" MaxLength="4" CssClass="form-control NegativeintegerInput" ClientIDMode="Static"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center">
                                            <asp:Label runat="server" ID="lblLKMinKN" ClientIDMode="Static"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </asp:Panel>
        <br />
        <asp:Button ID="btnForecast" runat="server" CssClass="btn btn-primary" Text="Inflow Forecast" OnClick="btnForecast_Click" />
        <asp:Button ID="btnBackStep4" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBackStep4_Click" />
    </div>

    <div id="divStepforecast" runat="server" visible="false">
        <h4 id="header" runat="server">Step 5: Forecasted Scenarios </h4>
        <br />
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
                                                <asp:Label ID="lblPeriod" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla")%>'></asp:Label>
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
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblPeriod" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" /><br />
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Jhelum at Mangla"></asp:Label><br />
                                                <asp:Label ID="lblJMKahrifPer" Text="Text" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblJMLKPer" Text="Text" runat="server"></asp:Label>
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
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblPeriod" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblKharif" runat="server" Text="EK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblLK" runat="server" Text="LK (MAF)" CssClass="control-label" Visible="true" /><br />
                                                <asp:Label ID="lblTotal" runat="server" Text="Total (MAF)" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblITTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
                                                <asp:Label ID="lblKNTotal" runat="server" CssClass="control-label" Visible="true" /><br />
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
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save" />
        <asp:Button ID="btnBackForecast" runat="server" CssClass="btn btn-primary" OnClick="btnBackForecast_Click" Text="Back" />
        <asp:Button ID="btnBackView" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="btnBackView_Click" Text="Back" />
    </div>


    <script type="text/javascript">

        $(document).ready(function () {
            $("#txtJMStartVariation").change(function () {

                var KN = $('#lblStep2JM').text();
                var Variation = document.getElementById("txtJMStartVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) - parseFloat(KN * Variation);
                document.getElementById("lblJMStartVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtJMEndVariation").change(function () {
                var KN = $('#lblStep2JM').text();
                var Variation = document.getElementById("txtJMEndVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) + parseFloat(KN * Variation);
                document.getElementById("lblJMEndVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtCMStartVariation").change(function () {
                var KN = $('#lblStep2CM').text();
                var Variation = document.getElementById("txtCMStartVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) - parseFloat(KN * Variation);
                document.getElementById("lblCMStartVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtCMEndVariation").change(function () {
                var KN = $('#lblStep2CM').text();
                var Variation = document.getElementById("txtCMEndVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) + parseFloat(KN * Variation);
                document.getElementById("lblCMEndVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtITStartVariation").change(function () {
                var KN = $('#lblStep2IT').text();
                var Variation = document.getElementById("txtITStartVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) - parseFloat(KN * Variation);
                document.getElementById("lblITStartVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtITEndVariation").change(function () {
                var KN = $('#lblStep2IT').text();
                var Variation = document.getElementById("txtITEndVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) + parseFloat(KN * Variation);
                document.getElementById("lblITEndVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtKNStartVariation").change(function () {
                var KN = $('#lblStep2KN').text();
                var Variation = document.getElementById("txtKNStartVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) - parseFloat(KN * Variation);
                document.getElementById("lblKNStartVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtKNEndVariation").change(function () {
                var KN = $('#lblStep2KN').text();
                var Variation = document.getElementById("txtKNEndVariation").value;
                Variation = Variation / 100;
                KN = parseFloat(KN) + parseFloat(KN * Variation);
                document.getElementById("lblKNEndVariationFinal").innerText = KN.toFixed(3);
            });
            $("#txtMaxJM").change(function () {
                debugger;
                var Probability = $('#lblMLJM').text();
                var ChangedVal = document.getElementById("txtMaxJM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMaxJM").innerText = result;

                    //var a = $('#lblMinJM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLJM").innerText = c;
                }
            });
            $("#txtMinJM").change(function () {
                debugger;
                var Probability = $('#lblMLJM').text();
                var ChangedVal = document.getElementById("txtMinJM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMinJM").innerText = result;

                    //var a = $('#lblMaxJM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLJM").innerText = c;
                }
            });
            $("#txtMaxCM").change(function () {
                var Probability = $('#lblMLCM').text();
                var ChangedVal = document.getElementById("txtMaxCM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMaxCM").innerText = result;

                    //var a = $('#lblMinCM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLCM").innerText = c;
                }
            });
            $("#txtMinCM").change(function () {
                var Probability = $('#lblMLCM').text();
                var ChangedVal = document.getElementById("txtMinCM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMinCM").innerText = result;

                    //var a = $('#lblMaxCM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLCM").innerText = c;
                }
            });
            $("#txtMaxIT").change(function () {
                var Probability = $('#lblMLIT').text();
                var ChangedVal = document.getElementById("txtMaxIT").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMaxIT").innerText = result;

                    //var a = $('#lblMinIT').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLIT").innerText = c;
                }
            });
            $("#txtMinIT").change(function () {
                var Probability = $('#lblMLIT').text();
                var ChangedVal = document.getElementById("txtMinIT").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMinIT").innerText = result;

                    //var a = $('#lblMaxIT').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLJM").innerText = c;
                }
            });
            $("#txtMaxKN").change(function () {
                var Probability = $('#lblMLKN').text();
                var ChangedVal = document.getElementById("txtMaxKN").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMaxKN").innerText = result;

                    //var a = $('#lblMinKN').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLKN").innerText = c;
                }
            });
            $("#txtMinKN").change(function () {
                var Probability = $('#lblMLKN').text();
                var ChangedVal = document.getElementById("txtMinKN").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblMinKN").innerText = result;

                    //var a = $('#lblMaxJM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblMLJM").innerText = c;
                }
            });
            $("#txtLKMaxJM").change(function () {
                debugger;
                var Probability = $('#lblLKMLJM').text();
                var ChangedVal = document.getElementById("txtLKMaxJM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMaxJM").innerText = result;

                    //var a = $('#lblLKMinJM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLJM").innerText = c;
                }
            });
            $("#txtLKMinJM").change(function () {
                var Probability = $('#lblLKMLJM').text();
                var ChangedVal = document.getElementById("txtLKMinJM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMinJM").innerText = result;

                    //var a = $('#lblLKMaxJM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLJM").innerText = c;
                }
            });
            $("#txtLKMaxCM").change(function () {
                var Probability = $('#lblLKMLCM').text();
                var ChangedVal = document.getElementById("txtLKMaxCM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMaxCM").innerText = result;

                    //var a = $('#lblLKMinCM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLCM").innerText = c;
                }
            });
            $("#txtLKMinCM").change(function () {
                var Probability = $('#lblLKMLCM').text();
                var ChangedVal = document.getElementById("txtLKMinCM").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMinCM").innerText = result;

                    //var a = $('#lblLKMaxCM').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLCM").innerText = c;
                }
            });
            $("#txtLKMaxIT").change(function () {
                var Probability = $('#lblLKMLIT').text();
                var ChangedVal = document.getElementById("txtLKMaxIT").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMaxIT").innerText = result;

                    //var a = $('#lblLKMinIT').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLIT").innerText = c;
                }
            });
            $("#txtLKMinIT").change(function () {
                var Probability = $('#lblLKMLIT').text();
                var ChangedVal = document.getElementById("txtLKMinIT").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMinIT").innerText = result;

                    //var a = $('#lblLKMaxIT').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLIT").innerText = c;
                }
            });
            $("#txtLKMaxKN").change(function () {
                var Probability = $('#lblLKMLKN').text();
                var ChangedVal = document.getElementById("txtLKMaxKN").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMaxKN").innerText = result;

                    //var a = $('#lblLKMinKN').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLKN").innerText = c;
                }
            });
            $("#txtLKMinKN").change(function () {
                var Probability = $('#lblLKMLKN').text();
                var ChangedVal = document.getElementById("txtLKMinKN").value;
                var result = parseInt(Probability) + parseInt(ChangedVal);
                if (result < 0 || result > 100)
                    alert("Probability must be in between 0-100.");
                else {
                    document.getElementById("lblLKMinKN").innerText = result;

                    //var a = $('#lblLKMaxKN').text();
                    //var b = (parseInt(result) + parseInt(a)) / 2;
                    //var c = Math.ceil(b / 5) * 5;
                    //c = parseInt(c);
                    //document.getElementById("lblLKMLKN").innerText = c;
                }
            });
        });
        
        function ValidateValue(textboxID) {
            if (textboxID.value > 100 && textboxID.value.fixedlength != 0)
                textboxID.value = '5';
        }
    </script>

</asp:Content>



