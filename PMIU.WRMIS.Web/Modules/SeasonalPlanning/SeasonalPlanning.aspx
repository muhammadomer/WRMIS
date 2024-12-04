<%@ Page Title="Seasonal Planning" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SeasonalPlanning.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.SeasonalPlanning" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upID" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <style type="text/css">
                .SPtable > tbody > tr > td {
                    vertical-align: middle;
                    padding-left: 8px;
                    padding-right: 8px;
                    padding-top: 1px;
                    padding-bottom: 1px;
                    font-size: 13px;
                    /*text-align:right;*/
                }

                .SPTextbox {
                    height: 17px;
                    padding-top: 0px;
                    padding-bottom: 0px;
                }
            </style>


            <div class="box">
                <div class="box-title">
                    <h3 id="htitle" runat="server">Seasonal Planning</h3>
                </div>
                <div class="box-content">


                    <div id="divSPDrafts" runat="server">
                        <asp:GridView ID="gvSPDraftsView" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            OnRowEditing="gvSPDraftsView_RowEditing" OnRowCommand="gvSPDraftsView_RowCommand" ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPDraftID") %>' />
                                        <asp:Label ID="lblIFID" runat="server" Text='<%# Eval("IFDraftID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Seasonal Planning Draft Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSPDescription" runat="server" Text='<%# Eval("SPDraftDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Inflow Forecast Draft Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIFDescription" runat="server" Text='<%# Eval("IFDraftDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="right">
                                            <asp:LinkButton ID="lbtnMax" runat="server" Text="" CommandName="Maximum" CommandArgument='<%# Eval("IFDraftID") %>' CssClass="btn btn-primary btn_32 view" ToolTip="Maximum"></asp:LinkButton>
                                            <asp:LinkButton ID="lbtnMin" runat="server" Text="" CommandName="Minimum" CommandArgument='<%# Eval("IFDraftID") %>' CssClass="btn btn-primary btn_32 view" ToolTip="Minimum"></asp:LinkButton>
                                            <asp:LinkButton ID="lbtnLikely" runat="server" Text="" CommandName="Likely" CommandArgument='<%# Eval("IFDraftID") %>' CssClass="btn btn-primary btn_32 view" ToolTip="Likely"></asp:LinkButton>
                                            <asp:LinkButton ID="lbtnApprove" runat="server" Text="" CommandName="Approve" CommandArgument='<%# Eval("IFDraftID") %>' Visible='<%# Eval("Approve") == null ? false : Eval("Approve") %>' CssClass="btn btn-primary fa fa-check" Style="padding: 5px 6px;" ToolTip="Approve/UnApprove"></asp:LinkButton>

                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <asp:Button ID="btnAddNew" Text="Add New Draft" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" OnClick="btnAddNew_Click"></asp:Button>
                    </div>

                    <div id="divForecastDrafts" runat="server" visible="false">
                        DraftName:<asp:TextBox ID="txtName" required="required" runat="server" Width="50%" CssClass="form-control required"></asp:TextBox>
                        <label title="Select Inflow Forcast Draft"></label>
                        <asp:GridView ID="gvView" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            OnRowEditing="gvView_RowEditing" OnRowCommand="gvView_RowCommand" ShowHeaderWhenEmpty="True" CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelection" runat="server" onclick="CheckOne(this)" ClientIDMode="Static" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Forecast Draft Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="right">
                                            <asp:LinkButton ID="lbtnView" runat="server" Text="" CommandName="View" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 view" ToolTip="View"></asp:LinkButton>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <asp:Button ID="btnSaveViewDiv" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnSaveViewDiv_Click" />
                        <asp:LinkButton ID="lbtnCancelViewDiv" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="lbtnCancelViewDiv_Click"></asp:LinkButton>
                    </div>

                    <div id="divPanel" class="panel panel-default" runat="server" visible="false">
                        <div id="Tabs" role="tabpanel">
                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs" role="tablist">
                                <li id="liVillage" style="width: 16%; text-align: center" runat="server" class="active"><a href="~/Modules/SeasonalPlanning/SeasonalPlanning.aspx" id="anchVillage" runat="server" aria-controls="Village" role="tab">Balance Reservoir</a></li>
                                <li id="liDivision" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/ManglaIndusOperations.aspx" id="anchDivision" runat="server" aria-controls="Division" role="tab">Mangla Operations</a></li>
                                <li id="li1" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/IndusOperations.aspx" id="a1" runat="server" aria-controls="Village" role="tab">Indus Operations</a></li>
                                <li id="li2" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedJC.aspx" id="a2" runat="server" aria-controls="Division" role="tab">Anticipated JC</a></li>
                                <li id="li3" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusRiver.aspx" id="a3" runat="server" aria-controls="Village" role="tab">Anticipated Indus River</a></li>
                                <li id="li4" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusBasin.aspx" id="a4" runat="server" aria-controls="Division" role="tab">Anticipated Indus Basin</a></li>
                            </ul>
                        </div>
                    </div>

                    <div id="divKharif" runat="server" class="table-responsive" visible="false">
                        <asp:Label ID="lblScenarioName" runat="server" CssClass="control-label" Font-Bold="true"></asp:Label>
                        <asp:Button ID="btnEdit" Style="float: right;" runat="server" CssClass="btn btn-primary" Text="Edit" OnClientClick="DisableTextBox(); return false;" />
                        <asp:Table ID="tblMainTable" runat="server" ClientIDMode="Static" CssClass="table tbl-info SPtable">
                            <asp:TableRow>
                                <asp:TableHeaderCell ColumnSpan="3" ID="tcJCHeader" runat="server" Text="Jhelum-Chenab Command Kharif"></asp:TableHeaderCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableHeaderCell ColumnSpan="3" ID="tcITHeader" runat="server" Text="Indus Command Kharif"></asp:TableHeaderCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell Width="20%">MCL at Mangla EL</asp:TableCell>
                                <asp:TableCell Width="9%">
                                    <asp:TextBox ID="txtJCManglaLevel" ClientIDMode="Static" TabIndex="1" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCManglaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Width="9%">Storage</asp:TableCell>
                                <asp:TableCell Width="9%">
                                    <asp:Label ID="lblManglaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <%--<asp:TableCell Width="6%"></asp:TableCell>--%>
                                <asp:TableCell Width="20%">MCL at Tarbela EL</asp:TableCell>
                                <asp:TableCell Width="9%">
                                    <asp:TextBox ID="txtTarbelaLevel" TabIndex="12" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Width="9%">Storage</asp:TableCell>
                                <asp:TableCell Width="9%">
                                    <asp:Label ID="lblTarbelaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>

                                <asp:TableCell>Filling Limit Start at EL</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtFillingLimit" TabIndex="13" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblFillingStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>

                                <asp:TableCell>Chashma Reservoir Level</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtChashmaLevel" TabIndex="14" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblChashmaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>

                                <asp:TableCell>Chashma Min operating Level</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtChashmaMinLevel" TabIndex="15" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblChashmaMinStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Forecast of Inflows"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Early Kharif"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Late Kharif"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Total"></asp:TableCell>

                                <asp:TableCell runat="server" Text="Forecast of Inflows"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Early Kharif"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Late Kharif"></asp:TableCell>
                                <asp:TableCell runat="server" Text="Total"></asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Jhelum at Mangla"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJMEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJMLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJMTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Indus at Tarbela"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblITEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblITLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblITTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Chenab at Marala"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblCMEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblCMLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblCMTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Kabul at Nowshera"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKNEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKNLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblKNotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server">
                                    <label style="float: left;">Eastern Rivers</label>
                                    <asp:DropDownList ID="ddlKharifEastern" runat="server" AutoPostBack="true" Width="50px" CssClass="form-control" OnSelectedIndexChanged="ddlEastern_SelectedIndexChanged"></asp:DropDownList>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtEasternEK" Text="1.3" TabIndex="2" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtEasternEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtEasternLK" Text="3.3" TabIndex="3" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtEasternEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblEasternTotal" Text=" 4.6" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>

                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Inflows in J-C Command"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCTotalEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCTotalLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Inflows in Indus Command"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICTotalEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICTotalLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Initial Reservoir Level"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCInitialLevel" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell runat="server" Text="Initial Reservoir Level"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICInitialLevel" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Initial Storage"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCInitialStorage" TabIndex="4" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Initial Storage"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICInitialStorage" TabIndex="16" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage to fill %"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCStrToFillEK" TabIndex="5" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCStrToFillLK" TabIndex="6" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>

                                <asp:TableCell runat="server" Text="Storage to fill %"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICStrToFillEK" TabIndex="17" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICStrToFillLK" TabIndex="18" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage to fill in MAF"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCStrFillEKMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCStrFillLKMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCStrFillTotalMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Storage to fill in MAF"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICStrFillEKMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICStrFillLKMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICStrFillTotalMAF" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage Dep. at end of season(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCStrDep" TabIndex="7" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Storage Dep. at end of season(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICStrDep" TabIndex="19" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorage_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage (-)/Release(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCStrRel" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Storage (-)/Release(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICStrRel" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Inflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysInflowEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysInflowLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCSysInflowTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="System Inflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysInflowEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysInflowLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICSysInflowTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="J-C Inflow's"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICOutflowEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICOutflowLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICOutflowTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+) %"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCSysLossGainPerEK" TabIndex="8" ClientIDMode="Static" runat="server" Style="text-align: right;" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCSysLossGainPerLK" TabIndex="9" ClientIDMode="Static" runat="server" Style="text-align: right;" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+) %"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtICSysLossGainPerEK" TabIndex="20" ClientIDMode="Static" runat="server" Style="text-align: right;" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtICSysLossGainPerLK" TabIndex="21" ClientIDMode="Static" runat="server" Style="text-align: right;" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+) MAF"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysLossGainMAFEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysLossGainMAFLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+) MAF"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysLossGainMAFEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysLossGainMAFLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Total Water Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCWaterAvailEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCWaterAvailLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCWaterAvailTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Total Water Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICWaterAvailEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICWaterAvailLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICWaterAvailTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Para 2 Release"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCParaEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCParaLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCParaTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="KPK + Balochistan"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKPKBalEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKPKBalLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblKPKBalTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Canal Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtJCCanalAvailEK" TabIndex="10" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCCanalAvailEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtJCCanalAvailLK" TabIndex="11" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCCanalAvailEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCCanalAvailTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Balance for Punjab and Sindh"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblPunjSindhEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblPunjSindhLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblPunjSindhTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="J-C Outflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCOutflowEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCOutflowLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCOutflowTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Para 2 Releases"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICParaEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICParaLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICParaTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Shortage"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCShortageEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCShortageLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblJCShortageTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Canal Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtICCanalAvailEK" TabIndex="22" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtICCanalAvailLK" TabIndex="23" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerEK_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICCanalAvailTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Kotri Below"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKotriEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKotriLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblKotriTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>

                                <asp:TableCell runat="server" Text="Shortage"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICShortageEK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICShortageLK" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblICShortageTotal" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <label style="font-weight: bold;">Note: All values in MAF except as noted.</label><br />
                        <asp:Button ID="btnKharifSave" ClientIDMode="Static" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnKharifSave_Click" />
                        <asp:Button ID="btnKharifBack" Text="Back" runat="server" CssClass="btn btn-primary" OnClick="btnRabiBack_Click" />
                    </div>

                    <div id="divRabi" runat="server" class="table-responsive" visible="false">
                        <asp:Label ID="lblRabiScenarioName" runat="server" CssClass="control-label" Font-Bold="true"></asp:Label>
                        <asp:Button ID="btnREdit" runat="server" CssClass="btn btn-primary" Style="float: right;" Text="Edit" OnClientClick="EnableRabiTextBox(); return false;" />
                        <asp:Table ID="tblRabiBalance" runat="server" ClientIDMode="Static" CssClass="table tbl-info SPtable">
                            <asp:TableRow>
                                <asp:TableHeaderCell ColumnSpan="2" ID="thRMain" runat="server" Text="Jhelum-Chenab Command Rabi"></asp:TableHeaderCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableHeaderCell ColumnSpan="2" ID="TableHeaderCell2" runat="server" Text="Indus Command Rabi"></asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="20%">MCL at Mangla EL(ft)</asp:TableCell>
                                <asp:TableCell Width="20%">
                                    <asp:TextBox ID="txtRJCManglaLevel" TabIndex="1" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtRJCManglaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Width="20%"></asp:TableCell>
                                <asp:TableCell Width="20%">MCL at Tarbela EL(ft)</asp:TableCell>
                                <asp:TableCell Width="20%">
                                    <asp:TextBox ID="txtRTarbelaLevel" TabIndex="7" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtRTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblRManglaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblRTarbelaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Filling Limit Start at EL(ft)</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtRFillingLimit" TabIndex="8" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtRTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblRFillingStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Chashma Reservoir Level(ft)</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtRChashmaLevel" TabIndex="9" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtRTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblRChashmaStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Chashma Min operating Level(ft)</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtRChashmaMinLevel" TabIndex="10" ClientIDMode="Static" runat="server" CssClass="form-control decimalInput SPTextbox" Enabled="false" AutoPostBack="true" MaxLength="7" OnTextChanged="txtRTarbelaLevel_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>Storage</asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblRChashmaMinStorage" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Forecast of Inflows"></asp:TableCell>
                                <asp:TableCell runat="server" Text=""></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Forecast of Inflows"></asp:TableCell>
                                <asp:TableCell runat="server" Text=""></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Jhelum at Mangla"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJMRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Indus at Tarbela"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblITRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Chenab at Marala"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblCMRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Kabul at Nowshera"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKNRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Eastern Rivers">
                                    <label style="float: left;">Eastern Rivers</label>
                                    <asp:DropDownList ID="ddlRabiEastern" runat="server" AutoPostBack="true" Width="50px" CssClass="form-control" OnSelectedIndexChanged="ddlEastern_SelectedIndexChanged"></asp:DropDownList>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtEasternRabi" Text="1.3" TabIndex="2" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtEasternRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Inflows in J-C Command"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCTotalRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Inflows in Indus Command"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICTotalRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Initial Reservoir Level(ft)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCInitialLevelRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Initial Reservoir Level(ft)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICInitialLevelRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Initial Storage"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCInitialStorageRabi" TabIndex="3" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorageRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Initial Storage"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICInitialStorageRabi" TabIndex="11" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorageRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Maximum storage"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCMaxStorageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Maximum storage"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICMaxStorageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage Available"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCInitialStorageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Storage Available"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICInitialStorageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage Dep. at end of season(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCStrDepRabi" TabIndex="4" ClientIDMode="Static" oninput="test(this,0,100)" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCInitialStorageRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Storage Dep. at end of season(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICStrDepRabi" TabIndex="12" ClientIDMode="Static" oninput="test(this,0,100)" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICInitialStorageRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Storage (-)/Release(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCStrRelRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Storage (-)/Release(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICStrRelRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="J-C Inflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICOutflowRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Inflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysInflowRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="System Inflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysInflowRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+)(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCSysLossGainPerRabi" TabIndex="5" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCSysLossGainPerRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+)(%)"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtICSysLossGainPerRabi" TabIndex="13" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control NegativedecimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCSysLossGainMAFRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="System Loss(-)/Gain(+)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICSysLossGainMAFRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Total Water Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCWaterAvailRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Total Water Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICWaterAvailRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Historic 77-82 Releases"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCParaRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Kotri Below"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKotriRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Canal Availability"></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtJCCanalAvailRabi" TabIndex="6" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtJCCanalAvailRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Canal Availability"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:TextBox ID="txtICCanalAvailRabi" TabIndex="14" ClientIDMode="Static" runat="server" Enabled="false" CssClass="form-control decimalInput SPTextbox" AutoPostBack="true" MaxLength="7" OnTextChanged="txtICSysLossGainPerRabi_TextChanged"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="J-C Outflows"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCOutflowRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="KPK + Balochistan"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblKPKBalRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell runat="server" Text="Shortage(%)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblJCShortageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Balance for Punjab and Sindh"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblPunjSindhRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                            
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Historic 77-82 Releases"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICHistoricRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>                        
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell runat="server" Text="Shortage(%)"></asp:TableCell>
                                <asp:TableCell Style="text-align: right;">
                                    <asp:Label ID="lblICShortageRabi" runat="server" CssClass="control-label"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <label style="font-weight: bold;">Note: All values in MAF except as noted.</label><br />
                        <asp:Button ID="btnRabiSave" ClientIDMode="Static" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnRabiSave_Click" />
                        <asp:Button ID="btnRabiBack" Text="Back" runat="server" CssClass="btn btn-primary" OnClick="btnRabiBack_Click" />
                    </div>

                    <div id="divforecast" runat="server" visible="false">
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
                                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMax_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Period">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label runat="server" Text="Jhelummm at Mangla"></asp:Label><br />
                                                                <asp:Label ID="lblJMKahrifPer" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblJMLKPer" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblJMDraft" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblJM" runat="server" CssClass="control-label" Text='<%# Eval("JhelumMangla") %>'></asp:Label>
                                                            </ItemTemplate>
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
                                                            <HeaderStyle CssClass="col-md-2" />
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
                                                    AllowPaging="false" ShowFooter="false" ShowHeader="true" OnRowDataBound="gvMax_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Period">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTDaily" runat="server" CssClass="control-label" Text='<%# Eval("Shortname") %>'></asp:Label>
                                                            </ItemTemplate>
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
                        <asp:Button ID="btnForecastOK" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnForecastOK_Click" />
                    </div>


                </div>
            </div>

            <script type="text/javascript">
                function DisableTextBox() {
                    debugger;
                    document.getElementById("txtJCManglaLevel").disabled = false;
                    document.getElementById("txtTarbelaLevel").disabled = false;
                    document.getElementById("txtFillingLimit").disabled = false;
                    document.getElementById("txtChashmaLevel").disabled = false;
                    document.getElementById("txtChashmaMinLevel").disabled = false;
                    document.getElementById("txtEasternEK").disabled = false;
                    document.getElementById("txtEasternLK").disabled = false;
                    document.getElementById("txtJCInitialStorage").disabled = false;
                    document.getElementById("txtICInitialStorage").disabled = false;
                    document.getElementById("txtJCStrToFillEK").disabled = false;
                    document.getElementById("txtJCStrToFillLK").disabled = false;
                    document.getElementById("txtICStrToFillEK").disabled = false;
                    document.getElementById("txtICStrToFillLK").disabled = false;
                    document.getElementById("txtJCStrDep").disabled = false;
                    document.getElementById("txtICStrDep").disabled = false;
                    document.getElementById("txtJCSysLossGainPerEK").disabled = false;
                    document.getElementById("txtJCSysLossGainPerLK").disabled = false;
                    document.getElementById("txtICSysLossGainPerEK").disabled = false;
                    document.getElementById("txtICSysLossGainPerLK").disabled = false;
                    document.getElementById("txtJCCanalAvailEK").disabled = false;
                    document.getElementById("txtJCCanalAvailLK").disabled = false;
                    // document.getElementById("txtICCanalAvailEK").disabled = false;
                    // document.getElementById("txtICCanalAvailLK").disabled = false;
                    //event.preventDefault();

                }

                function EnableRabiTextBox() {
                    debugger;
                    document.getElementById("txtRJCManglaLevel").disabled = false;
                    document.getElementById("txtRTarbelaLevel").disabled = false;
                    document.getElementById("txtRFillingLimit").disabled = false;
                    document.getElementById("txtRChashmaLevel").disabled = false;
                    document.getElementById("txtRChashmaMinLevel").disabled = false;
                    document.getElementById("txtEasternRabi").disabled = false;
                    document.getElementById("txtJCInitialStorageRabi").disabled = false;
                    document.getElementById("txtICInitialStorageRabi").disabled = false;
                    document.getElementById("txtJCStrDepRabi").disabled = false;
                    document.getElementById("txtICStrDepRabi").disabled = false;
                    document.getElementById("txtJCSysLossGainPerRabi").disabled = false;
                    document.getElementById("txtICSysLossGainPerRabi").disabled = false;
                    document.getElementById("txtJCCanalAvailRabi").disabled = false;
                    // document.getElementById("txtICCanalAvailRabi").disabled = false;
                    //event.preventDefault();
                }
                function CheckOne(obj) {
                    var grid = document.getElementById("<%=gvView.ClientID %>");
                    var inputs = grid.getElementsByTagName("input");
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].type == "checkbox") {
                            if (obj.checked && inputs[i] != obj && inputs[i].checked)
                                inputs[i].checked = false;
                        }
                    }
                }
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
