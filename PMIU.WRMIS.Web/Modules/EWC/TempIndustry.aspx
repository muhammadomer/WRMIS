<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TempIndustry.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.TempIndustry" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Generate Bill</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <%--<div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButton CssClass="radio-inline" required="required" Checked="true" ID="rbEffluentWater" runat="server" GroupName="ViewType" Text="Effluent Water" />
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbCanalSpecialWater" runat="server" GroupName="ViewType" Text="Canal Special Water" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Financial Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" disabled="true" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Bill Issue Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtBillIssueDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Bill Due Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtBillDueDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Taxes</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:CheckBox CssClass="radio-inline" ID="cbApplicableTaxes" Text="Include Applicable Taxes" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <h3>
                                        <label class="col-sm-4 col-lg-3 control-label">Adjustments</label></h3>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblAdjustment" Text="Adjustment" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:DropDownList ID="ddlAdjustment1" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Select" Value="" />
                                            <asp:ListItem Text="+" Value="Add" />
                                            <asp:ListItem Text="-" Value="Sub" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:DropDownList ID="ddlAdjustment2" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Select" Value="" />
                                            <asp:ListItem Text="% age of bill (excluding taxes)" Value="PercentageOfBil" />
                                            <asp:ListItem Text="(Rs.) Fix" Value="RupeesFix" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:TextBox ID="txtAdjustment" runat="server" ClientIDMode="Static" CssClass="form-control integerInput" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Reason</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtReason" runat="server" MaxLength="250" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnBillGeneration" ClientIDMode="Static" OnClick="btnBillGeneration_Click" runat="server" CssClass="btn btn-primary" align="center" Text="Generate Bill"></asp:Button>
                                </div>
                            </div>
                        </div>--%>

                        <asp:Panel ID="pnlSpecialWaters" runat="server" GroupingText="Canal Special Water Details">

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDiv" runat="server" required="required" CssClass="required form-control" AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:GridView ID="gvSCW" runat="server" CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                AllowCustomPaging="true" PageSize="100" EmptyDataText="No record added" AllowSorting="false" GridLines="None"
                                ShowHeaderWhenEmpty="true" OnRowCommand="gvSCW_RowCommand" OnRowDeleting="gvSCW_RowDeleting"
                                OnRowEditing="gvSCW_RowEditing" OnRowDataBound="gvSCW_RowDataBound" Visible="true">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Channel">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supply From">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupply" runat="server" Text='<%# Eval("SupplyFrom") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outlet/RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%# Eval("CalculatedRD") %>'></asp:Label>
                                            <asp:Label ID="lblOutlet" runat="server" Text='<%# Eval("ChannelOutletName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Installation Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("InstallationDateForGrid") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate"></asp:Button>
                                            </asp:Panel>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:Button>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <%--Canal Special Water Popup - start--%>
                            <div id="divAdd" class="modal fade">
                                <div class="modal-dialog table-responsive" style="max-height: 419px; max-width: 893.398px;">
                                    <div class="modal-content" style="width: 830px">
                                        <div class="modal-body">
                                            <div class="box">
                                                <div class="box-title">
                                                    <h3>
                                                        <asp:Label ID="lblTitle" Text="Canal Special Waters" runat="server" />
                                                    </h3>
                                                </div>
                                                <div class="box-content ">
                                                    <div class="table-responsive">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <div class="form-horizontal">
                                                                    <%--Canal Special Waters - Add Form - start--%>
                                                                    <div class="row">
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <asp:Label CssClass="col-sm-4 col-lg-3 control-label" ID="lbl" runat="server" Text="Channel"></asp:Label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:DropDownList ID="CSW_ddlChannel" runat="server" required="required" CssClass="required form-control" AutoPostBack="true" OnSelectedIndexChanged="CSW_ddlChannel_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="Select" Value="" />
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Supply From</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:RadioButton CssClass="radio-inline" required="required" ID="CSW_rbRD" runat="server" AutoPostBack="true" OnCheckedChanged="CSW_rbRD_CheckedChanged" GroupName="ViewType" Text="RD" Checked="true" />
                                                                                    <asp:RadioButton CssClass="radio-inline" required="required" ID="CSW_rbOutlet" runat="server" AutoPostBack="true" OnCheckedChanged="CSW_rbRD_CheckedChanged" GroupName="ViewType" Text="Outlet" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row" id="divRD_CSW" runat="server">
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">RD (ft)</asp:Label>
                                                                                <div class="col-sm-3 col-lg-4 controls">
                                                                                    <asp:TextBox ID="CSW_txtRDLeft" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength form-control required"></asp:TextBox>
                                                                                </div>
                                                                                <div class="col-sm-1 col-lg-1 controls">
                                                                                    +
                                                                                </div>
                                                                                <div class="col-sm-3 col-lg-4 controls">
                                                                                    <asp:TextBox ID="CSW_txtRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength form-control required"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Side</asp:Label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:DropDownList ID="CSW_ddlChnlSide" runat="server" required="required" CssClass="required form-control">
                                                                                        <asp:ListItem Text="Select" Value="" />
                                                                                        <asp:ListItem Text="Right" Value="R" />
                                                                                        <asp:ListItem Text="Left" Value="L" />
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div id="divOutlet_CSW" runat="server">
                                                                            <div class="col-md-6">
                                                                                <div class="form-group">
                                                                                    <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Outlet</asp:Label>
                                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                                        <asp:DropDownList ID="CSW_ddlOutlet" runat="server" CssClass="form-control">
                                                                                            <asp:ListItem Text="Select" Value="" />
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Source of Discharge</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:DropDownList ID="CSW_ddlDschrg" runat="server" CssClass="form-control" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Installation Date</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <div class="input-group date" data-date-viewmode="years">
                                                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                        <asp:TextBox ID="CSW_txtInstlDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Installation Cost</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:TextBox ID="CSW_txtInstlCost" runat="server" CssClass="form-control decimalInput" MaxLength="10" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Agreement Signed On</label>
                                                                                <div class="input-group date" data-date-viewmode="years">
                                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                    <asp:TextBox ID="CSW_txtAgrmntSign" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Agreement End Date</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <div class="input-group date" data-date-viewmode="years">
                                                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                        <asp:TextBox ID="CSW_txtAgrmntEndDate" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6 ">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-3 control-label">Agreement Parties</label>
                                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                                    <asp:TextBox ID="CSW_txtAgrmntPrty" runat="server" CssClass="form-control" MaxLength="50" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <%--Canal Special Waters - Add Form - end--%>
                                                                </div>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCSWSave" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnCSWSave_Click" />
                                                <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">Close</button>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--Canal Special Water Popup - end--%>
                        </asp:Panel>

                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnSave_Click" />

                    </div>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        function openModal() {
            $("#divAdd").modal("show");
        };



        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                    //ClearDateField();
                }
            });
        };

        function Reset() {
            //console.log("called");
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeDatePickerStateOnUpdatePanelRefresh();
                        //ClearDateField();
                    }
                });
            };
        }
    </script>
</asp:Content>
