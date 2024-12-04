<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintBill.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.PrintBill" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">


        $(document).ready(function () {
            $("#btnAllprint").hide();
        });

        function SelectAll() {
            $("#btnAllprint").show();
            var all = document.getElementById("AllCheckBox");
            if (all != null)
                var res = document.getElementById("AllCheckBox").checked;

            var chkBox = document.getElementsByClassName("AllCB");
            for (var i = 0 ; i < chkBox.length; i++) {
                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }

            if (res == true)
                $("#btnAllprint").show();
            else
                $("#btnAllprint").hide();
        }

        function AllTrueFalse() {
            var CheckAll = 1;
            var chkBox = document.getElementsByClassName("AllCB");
            if (chkBox.length == 0)
                CheckAll = 0;
            else {
                for (var i = 0 ; i < chkBox.length; i++) {
                    if (!chkBox[i].getElementsByTagName('input')[0].checked)
                        CheckAll = 0;
                }
            }
            return CheckAll;
        }

        function AllFalse() {
            var AllUnchecked = 1;
            var chkBox = document.getElementsByClassName("AllCB");
            if (chkBox.length == 0)
                AllUnchecked = 0;
            else {
                for (var i = 0 ; i < chkBox.length; i++) {
                    if (chkBox[i].getElementsByTagName('input')[0].checked)
                        AllUnchecked = 0;
                }
            }
            return AllUnchecked;
        }

        function SelectCheckBox(CheckBox) {
            var Res = 0;
            if (CheckBox.checked) {
                $("#btnAllprint").show();
                var Res = AllTrueFalse();
            }
            else {
                var Result = AllFalse();
                if (Result == 1)
                    $("#btnAllprint").hide();
                else
                    $("#btnAllprint").show();
            }

            if (Res == 1) {
                var res = document.getElementById("AllCheckBox").checked = true;
            }
            else {
                var res = document.getElementById("AllCheckBox").checked = false;
            }
        }
    </script>


    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Print Bill</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdServiceType" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rdServiceType_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Selected="True">&nbsp;Effluent Water</asp:ListItem>
                                            <asp:ListItem Value="2">&nbsp;Canal Special Water</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Financial Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlfinancialyear" runat="server" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlType" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDiv" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry No.</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="integerInput form-control" MaxLength="10" ID="txtIndNo" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtIndName" runat="server" MaxLength="100" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" Visible="<%# base.CanView %>" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btn_Click" formnovalidate="formnovalidate" />
                                    <asp:Button ID="btnAllprint" ClientIDMode="Static" runat="server" Text="Print All Bills" CssClass="btn btn-primary" ToolTip="Print Bills" OnClick="btnAllprint_Click" formnovalidate="formnovalidate" />
                                </div>
                            </div>
                        </div>


                    </div>
                    <div id="divData">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="false" PageSize="10" DataKeyNames="IndustryID,BillID,ServiceType,Status,IndustryName,Division"
                                        OnPageIndexChanging="gv_PageIndexChanging" OnPageIndexChanged="gv_PageIndexChanged" EmptyDataText="No record found" CssClass="table header" BorderWidth="0px"
                                        CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCommand="gv_RowCommand" OnRowDataBound="gv_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField HeaderText="No.">
                                                <HeaderTemplate>
                                                    <label class="checkbox-inline">
                                                        <asp:CheckBox ID="AllCheckBox" name="AllCheckBox" ClientIDMode="Static" runat="server" OnClick="SelectAll();" Text="Check All" HorizontalAlign="Center" AutoPostBack="false" />
                                                    </label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb" runat="server" Class="AllCB" OnClick="SelectCheckBox(this);" HorizontalAlign="Center" AutoPostBack="false" />
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("IndustryID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width=".7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("IndustryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Division" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bill No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillNo" runat="server" Text='<%# Eval("BillNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Service Charges (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceCharges" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString( Eval("Surcharge") )) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Applicable Taxes (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApplicableTax" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("ApplicableTax"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arrears (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArears" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("Arrears"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Adjustments (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdjustments" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("Adjustment"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Bill (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalBill" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("TotalAmount"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Balance (Rs.)" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastBalance" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("LastBalance"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payable Amount (Rs.)" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayableAmount" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("BillAmount"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surcharge (Rs.)" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayableSurchargeRate" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("SurchargeRate"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payable After Due Date (Rs.)" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayableAfterDueDate" runat="server" Text='<%# Utility.GetRoundOffValue (Convert.ToString(Eval("BillAmountAfterDueDate"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <HeaderStyle CssClass="col-md-1 text-center" />
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnArrears" CommandName="Arrears" runat="server" CssClass="btn btn-primary btn_24 view" ToolTip="Arrears" Visible="false"></asp:Button>
                                                        <asp:Button ID="btnAdjustment" CommandName="Adjustment" runat="server" CssClass="btn btn-primary btn_24 adjustments" ToolTip="Special Adjustment" formnovalidate="formnovalidate" Visible="false"></asp:Button>
                                                        <asp:Button ID="btnFinalizeBill" CommandName="FinalizeBill" runat="server" CssClass="btn btn-primary btn_24 Finilized-Bill" ToolTip="Finalize Bill" OnClientClick="return confirm('Are you sure you want to Finalize the selected bill(s)?');" formnovalidate="formnovalidate" Visible="false"></asp:Button>
                                                        <asp:LinkButton ID="hlBilling" runat="server" ToolTip="Bill Detail" CommandName="BillingDetail" CssClass="btn btn-primary btn_24 bill-details">
                                                        </asp:LinkButton>
                                                        <asp:Button ID="btnPrint" CommandName="Print" runat="server" CssClass="btn btn-primary btn_24 print" ToolTip="Print Bill" formnovalidate="formnovalidate"></asp:Button>

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
                    </div>
                    <div class="row" id="finalizebillDiv" runat="server" visible="false">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnfinalizebill" runat="server" Text="Finalize All Bills" CssClass="btn btn-primary" ToolTip="Finalize Bills" OnClick="btnfinalizebill_Click" formnovalidate="formnovalidate" Visible="false" />
                                <%--<asp:Button ID="btnAllprint" runat="server" Text="Print All Bills" CssClass="btn btn-primary" ToolTip="Print Bills" OnClick="btnAllprint_Click" formnovalidate="formnovalidate" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


