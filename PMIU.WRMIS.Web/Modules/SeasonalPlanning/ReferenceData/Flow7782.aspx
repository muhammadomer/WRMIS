<%@ Page Title="Flow7782" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Flow7782.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.Flow7782" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Flow7782</h3>
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                    <div id="roleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:GridView ID="gvFlow" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        OnPageIndexChanged="gvFlow_PageIndexChanged" OnRowCancelingEdit="gvFlow_RowCancelingEdit"
                        OnRowUpdating="gvFlow_RowUpdating" OnRowEditing="gvFlow_RowEditing" OnPageIndexChanging="gvFlow_PageIndexChanging" OnRowDataBound="gvFlow_RowDataBound"
                        ShowHeaderWhenEmpty="True" ShowFooter="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                    <asp:Label ID="lblTDailyID" runat="server" Text='<%# Eval("TDailyID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Period">
                                <ItemTemplate>
                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                </ItemTemplate>

                                <FooterTemplate>
                                    <asp:Label Text="EK (MAF)" ID="lblPeriodEK" runat="server" />
                                    <br />
                                    <asp:Label Text="LK (MAF)" ID="lblPeriodLK" runat="server" />
                                    <br />
                                    <asp:Label Text="Total (MAF)" ID="lblPeriodTotal" runat="server" />
                                </FooterTemplate>

                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Indus Command">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndus" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Indus")) %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblInudsEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblIndusLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblIndusTotal" runat="server" />
                                </FooterTemplate>

                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="J-C Command">
                                <ItemTemplate>
                                    <asp:Label ID="lblJC" runat="server" Text='<%# String.Format("{0:0.0}",Eval("JC")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtJC" runat="server" CssClass="form-control decimalInput required" required="true" autocomplete="off" MaxLength="6" Text='<%# Eval("JC") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblJCEK" runat="server" Visible="true" /><br />
                                    <asp:Label Text="" ID="lblJCLK" runat="server" Visible="true" /><br />
                                    <asp:Label Text="" ID="lblJCTotal" runat="server" Visible="true" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:Button>
                                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                    </asp:Panel>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnHistory" runat="server" CommandArgument='<%# Eval("TDailyID") %>' OnClick="lbtnHistory_Click" CssClass="btn btn-primary btn_24 audit" ToolTip="History"></asp:LinkButton>
                                    </asp:Panel>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalHistory" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">
                    <%--<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>--%>
                    <div class="box">
                        <div class="box-title">
                            <asp:Label ID="lblName" runat="server" Style="font-size: 30px;"></asp:Label>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <div class="row" id="gvOffenders" runat="server">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                                OnPageIndexChanging="gvHistory_PageIndexChanging" OnPageIndexChanged="gvHistory_PageIndexChanged"
                                                CellSpacing="-1" GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="10-Day">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="JC Command">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("JC") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Modified Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Modified By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("ModifiedBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <div class="modal-footer">
                    <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>

    <script type="text/javascript">

        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                }
            });
        };

        function ValidateValue(textboxID) {
            var MaxValue = 100;
            if ((textboxID.value >= 0 && textboxID.value <= MaxValue) && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {
                alert('Value range is 0 to' + ' ' + MaxValue);
                textboxID.value = '';
                return false;
            }
        }
    </script>
</asp:Content>
