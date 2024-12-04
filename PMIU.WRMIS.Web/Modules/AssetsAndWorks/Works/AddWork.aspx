<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddWork.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Works.AddWork" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="ClosureWorkPlan_Screen" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Label ID="lblPageTitle" runat="server" Text="Add New Work"></asp:Label></h3>
                    <asp:HiddenField runat="server" Value="" ID="hfWorkID" />
                    <asp:HiddenField runat="server" Value="" ID="hdnAuctionedWorkID" />
                    <asp:HiddenField runat="server" Value="" ID="hdnWorkStatus" />
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row" id="searchDiv">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFinancialYear" required="required" runat="server" Enabled="false" CssClass="required form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label12" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" required="required" runat="server" CssClass="required form-control" autofocus="autofocus">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Work Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkName" MaxLength="150" required="required" runat="server" CssClass="required form-control"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Work Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlWorkType" required="required" runat="server" CssClass="required form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Funding Source" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFundingSource" required="required" runat="server" CssClass="required form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlAssetCategory" required="required" runat="server" CssClass="required form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnlEstDtl" runat="server" GroupingText="Estimation Details">
                            <div class="table-responsive" style="margin-top: 10px;">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label4" runat="server" Text="Cost (Rs)" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtCost" required="required" Style="text-align: left" CssClass="required integerInput form-control" MaxLength="15" runat="server">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Completion Period" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-4 col-lg-5 controls">
                                                <asp:TextBox ID="txtCompletionPeriod" MaxLength="3" required="required" runat="server" CssClass="required integerInput form-control">
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-lg-4 controls">
                                                <asp:DropDownList ID="ddlCompletionPeriodUnit" required="required" runat="server" CssClass="required form-control">
                                                    <asp:ListItem Text="Days" Value="Days" />
                                                    <asp:ListItem Text="Weeks" Value="Weeks" />
                                                    <asp:ListItem Text="Months" Value="Months" />
                                                    <asp:ListItem Text="Years" Value="Years" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Start Date</label>
                                            <div class="col-sm-8 col-lg-9 control" runat="server" id="divStartDate">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtStartDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">End Date</label>
                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id="divEndDate">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtEndDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>


                        <asp:Panel ID="Panel1" runat="server" GroupingText="Technical Sanction and Tendering Details ">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sanction No</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtSanctionNo" required="required" runat="server" placeholder="" MaxLength="30" CssClass="required form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sanction Date</label>
                                        <div class="col-sm-8 col-lg-9 controls" runat="server" id="divSanctionDate">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtSnctnDate" required="required" TabIndex="4" runat="server" CssClass="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Earnest Money (Rs)</label>
                                        <div class="col-sm-4 col-lg-5 controls">
                                            <asp:TextBox ID="txtErnsMny" required="required" runat="server" placeholder="" Style="text-align: left" MaxLength="5" CssClass="required decimalInput form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:DropDownList ID="ddlErnsMnyType" required="required" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Lumpsum" Value="Lumpsum" />
                                                <asp:ListItem Text="% of Financial Bid" Value="% of Financial Bid" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Tender Fee (Rs)</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtndrFee" runat="server" placeholder="" Style="text-align: left" required="required" MaxLength="8" CssClass="required decimalInput form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Description</label>
                                        <div class="col-sm-7 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control commentsMaxLengthRow multiline-no-resize"
                                                minlength="3" MaxLength="250" TextMode="MultiLine" Rows="5" Columns="50"
                                                ID="txtDesc" runat="server" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="PnlAssets" runat="server" GroupingText="Association with Assets" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvWork" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvWork_RowDeleting" OnRowCommand="gvWork_RowCommand"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Category">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlCategory" runat="server" required="required" CssClass="required form-control" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                                AutoPostBack="true" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Category">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="required form-control " required="required" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Level">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlLevel" runat="server" CssClass="required form-control" required="required" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged" AutoPostBack="true" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlAssetName" runat="server" CssClass="required form-control" required="required" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnAssetWork" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddWorkAssetAssociate" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" Enabled='<%# base.CanDelete %>' formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>

                        </div>

                    </asp:Panel>
                    <asp:Panel ID="PnlFlood" runat="server" GroupingText="Association with Flood" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvFlood" runat="server" AutoGenerateColumns="false" OnRowCommand="gvFlood_RowCommand"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDF" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlInfraType" runat="server" required="required" CssClass="required form-control" OnSelectedIndexChanged="ddlInfrastructuresType_SelectedIndexChanged"
                                                                AutoPostBack="true" onfocus="this.value = this.value;" Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlInfraStructure" runat="server" CssClass="required form-control" required="required" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnFloodWork" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddFlodAssociate" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnFloodDelete" runat="server" CommandName="DeleteFlood" CssClass="btn btn-danger btn_32 delete" Enabled='<%# base.CanDelete %>' formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlInfrastructure" runat="server" GroupingText="Association with Infrastructure" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gv_Infra" runat="server" AutoGenerateColumns="false" OnRowCommand="gv_Infra_RowCommand"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDInfra" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlType" runat="server" required="required" CssClass="required form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                                AutoPostBack="true" onfocus="this.value = this.value;" Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlName" runat="server" CssClass="required form-control" required="required" onfocus="this.value = this.value;" Width="90%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategoryInfra" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnInfraWork" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddInfra" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlActionInfra" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnInfraDelete" runat="server" CommandName="DeleteInfra" CssClass="btn btn-danger btn_32 delete" Enabled='<%# base.CanDelete %>' formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row" id="searchButtonsDiv">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnAddWork" runat="server" CssClass="btn btn-primary" Text="Save" ToolTip="Add" OnClick="btnAddWork_Click"></asp:Button>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                var title = $('#<%=lblPageTitle.ClientID%>').text();
                if (title.toLowerCase() === 'work') {

                    $('input').removeClass('required');
                    $('select').removeClass("required");

                }
            });
        </script>
        <script type="text/javascript" src="../../Scripts/jquery-1.10.2.min.js"></script>
</asp:Content>
