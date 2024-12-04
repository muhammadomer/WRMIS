<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FundReleaseDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.FundReleaseDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Fund Release Details</h3>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <table class="table tbl-info">
                    <tr>
                        <th width="33.3%">Fund Release Type</th>
                        <th width="33.3%">Fund Release Date</th>
                        <th width="33.3%">Financial Year</th>
                        <th></th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCutType" runat="server" Text="" /></td>
                        <td>
                            <asp:Label ID="lblCutDate" runat="server" Text="" /></td>
                        <td>
                            <asp:Label ID="lblFinancialYear" runat="server" Text="" /></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvFundReleaseDetails" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvFundReleaseDetails_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Account Code">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountCode" runat="server" CssClass="control-label" Text='<%# Eval("AccountCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Objects/Classification">
                            <ItemTemplate>
                                <asp:Label ID="lblObjectsClassification" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassification") %>'></asp:Label>
                                <asp:Label ID="lblObjectsClassificationID" Visible="false" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassificationID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Budgetary Provision (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedBudget" runat="server" CssClass="control-label" Text='<%# Eval("BudgetAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Previously Released Amount (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPreviouslyReleasedAmount" runat="server" CssClass="control-label" Text='<%# Eval("PreviouslyReleasedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Supplymentory Grant (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblSupplymentoryGrant" runat="server"  Text='<%# Eval("SupplyMentoryGrant") %>' CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Re-appropriation (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblReAppropriation" runat="server" CssClass="control-label" Text='<%# Eval("ReAppropriation") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Previous Balance (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPreviousBalance" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Release (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:TextBox ID="txtCurrentRelease"  runat="server" MaxLength="10" onfocus="RemoveCommas(this);" autocomplete="off" onblur="AddCommas(this);"  CssClass="form-control" placeholder="Enter Value" />
                                <asp:Label ID="lblCurrentReleaseAmount" runat="server" Visible="false" CssClass="control-label" Text='<%# Eval("CurrentReleaseAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Balance Amount (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblBalanceAmount" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script>
        
        function callFunctions(Param1, Param2, Param3) {
            var CurrentRelease = parseFloat(Param1.value);
            var PreviousBalance = parseFloat(Param2);
                var BalanceAmountParam = Param3;
                var BalanceAmount = 0;
                var bool = true;

                if (CurrentRelease) {
                    BalanceAmount = PreviousBalance - CurrentRelease;

                    if (BalanceAmount < 0) {
                        alert('‘Current Release Amount’ should be less than or equal to ‘Previous Balance’ of the respective Object /Classification');
                        BalanceAmount = PreviousBalance;
                        Param1.value = "";
                    }
                }
                else {
                    BalanceAmount = PreviousBalance;
                }

                //var formattdNum = BalanceAmount.toLocaleString('en-US', { minimumFractionDigits: 2 });
                $('#' + BalanceAmountParam).html(BalanceAmount.toString().replace(/(.)(?=(.{3})+$)/g, "$1,"));
        
        }

    </script>
</asp:Content>
