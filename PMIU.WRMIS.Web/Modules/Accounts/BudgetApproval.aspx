<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetApproval.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.BudgetApproval" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function GetTotalNewAmount() {
            debugger;
            ////Get the Row based on TextBox
            //var row = objRef.parentNode.parentNode;
            ////Get the reference of GridView
            //var tbody = row.parentNode;
            var totalOldValue = 0.0;

            var ResultArray = [];
            $(function () {
                $('#<%=gvBudget.ClientID %>').find('input:Text').each(function () {
                    var CurrentValue = $(this).val();
                    if (CurrentValue.length) {
                        a = CurrentValue.replace(/\,/g, '');
                        CurrentValue = parseInt(a, 10);
                        totalOldValue = Number(totalOldValue) + Number(CurrentValue);
                    }

                });
            });
            if (totalOldValue > 0) {
                $('.ApprovalAmount').html(totalOldValue.toString().replace(/(.)(?=(.{3})+$)/g, "$1,"));
            }
        }
    </script>
    <script type="text/javascript">
        function TotalReApropriate() {

            ////Get the Row based on TextBox
            //var row = objRef.parentNode.parentNode;
            ////Get the reference of GridView
            //var tbody = row.parentNode;

            var totalOldValue = 0.0;
            var ResultArray = [];
            $(function () {
                $('#<%=gvBudget.ClientID %>').find('input:Text').each(function () {
                    var CurrentValue = $(this).val();
                    if (CurrentValue.length) {
                        a = CurrentValue.replace(/\,/g, '');
                        CurrentValue = parseInt(a, 10);
                        totalOldValue = Number(totalOldValue) + Number(CurrentValue);
                    }

                });
            });
            if (totalOldValue > 0) {
                $('.ReApproPriate').html(totalOldValue.toString().replace(/(.)(?=(.{3})+$)/g, "$1,"));
            }

        }

    </script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('.date-picker-user').datepicker({
                startDate: '<%= ViewState[StartDate].ToString() %>',
                endDate: '<%= ViewState[EndDate].ToString() %>',
                autoclose: true,
                todayHighlight: true,
                language: 'ru'
            });

        });

    </script>


    <div class="box">
        <div class="box-title">
            <h3>Budget Approval & Reappropriation</h3>
        </div>
        <div class="box-content">
            <asp:HiddenField runat="server" Value="" ID="ReAproDate" />
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlFinancialYear" TabIndex="1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" CssClass="form-control required" required="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <br>
                <div class="row" id="tabs" runat="server">
                    <div class="col-md-6 col-md-offset-2" style="margin-left: 96px;">
                        <div class=" hor">

                            <asp:LinkButton ID="btnBudgetApproval" runat="server" TabIndex="2" Text="Approved Budget" CssClass="btn btn-primary" OnClick="btnBudgetApproval_Click" ToolTip="Approved Budget" Width="234px" />

                            <%--<asp:LinkButton ID="btnBudgetReAppropriation" TabIndex="3" runat="server" Text="Reappropriation" CssClass="btn btn-primary" OnClick="btnBudgetReAppropriation_Click" ToolTip="Reappropriation" Width="234px"  Visible="false"/>--%>

                            <asp:LinkButton ID="btnRevised" runat="server" TabIndex="4" Text="Revised" CssClass="btn btn-primary" OnClick="btnRevised_Click" ToolTip="Revised" Width="234px" />

                        </div>
                    </div>
                </div>
                <br>
                <br>
                <br>
                <div class="row" id="date" runat="server">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblDate" runat="server" Text="Budget Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div>
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control required date-picker-user" size="16" type="text" required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="dAllocatedBudget" runat="server">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblAllocatedBudget" runat="server" Text="Allocated Budget" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtAllocatedBudget" TabIndex="1" MaxLength="10" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" required="required" CssClass="decimalIntegerInput required form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="False" ShowFooter="true" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID"
                                OnRowCommand="gvBudget_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Code" FooterText="Total Amount" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountCode" runat="server" CssClass="control-label" Text='<%# Eval("AccoundCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object/Classification">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectClassification" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassification") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Budgetary Provision (Rs)" FooterStyle-CssClass="text-right" ItemStyle-CssClass="text-right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedBudget" runat="server" CssClass="control-label" Text='<%# Eval("ApprovedBudget") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("ApprovedBudget"))) : Eval("ApprovedBudget") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalApproveBudget" Font-Bold="true" runat="server" CssClass="ApproveBudget" Text='' />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approved Budget (Rs)" Visible="false" FooterStyle-CssClass="text-right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtApproveBudget" runat="server" MaxLength="10" Text="" onfocus="RemoveCommas(this);" onblur="GetTotalNewAmount(this); AddCommas(this);" CssClass="decimalIntegerInput form-control required" required="true" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalOldValue" runat="server" Font-Bold="true" CssClass="ApprovalAmount" Text='' />
                                        </FooterTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New Value" Visible="false" FooterStyle-CssClass="text-right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNewValue" runat="server" MaxLength="10" CssClass="form-control decimalIntegerInput required ReAppropriateBudget" onfocus="RemoveCommas(this);" onblur="TotalReApropriate(this); AddCommas(this);" required="true" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalNewValue" runat="server" Font-Bold="true" CssClass="ReApproPriate" Text="" />
                                        </FooterTemplate>
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
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="&nbsp;Save" />
                            <%--<asp:HyperLink ID="hlCancel" runat="server" CssClass="btn btn-default">&nbsp;Cancel</asp:HyperLink>--%>
                        </div>
                    </div>
                </div>

            </div>

            <div id="AreYouSureWantToSave" class="modal fade" style="margin-top: 400px;">
                <div class="modal-dialog" style="width: 30%;">
                    <div class="modal-content" style="text-align: center;">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" Text="Are you sure you want to save the information? Once saved, it will not be edited" CssClass="control-label" />
                                            </div>
                                        </div>
                                    </div>
                                    <br></br>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSaveBudgetApproval" runat="server" class="btn btn-primary" Text="Yes" OnClick="btnSaveBudgetApproval_Click"></asp:Button>
                            <button class="btn btn-default" data-dismiss="modal" onclick="GetTotalNewAmount();">No</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
