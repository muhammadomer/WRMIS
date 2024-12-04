<%@ Page Title="Eastern Component" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EasternComponent.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.EasternComponent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Eastern Component</h3>
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

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblYear" class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div id="moduleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                        <asp:DropDownList CssClass="form-control required" required="true" Enabled="false" ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:GridView ID="gvEasternComponent" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        OnRowDataBound="gvEasternComponent_RowDataBound"
                        ShowHeaderWhenEmpty="True" AllowPaging="false" ShowFooter="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Period">
                                <ItemTemplate>
                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="EK (MAF)" ID="lblPeriodEK" runat="server" Visible="true" />
                                    <br />
                                    <asp:Label Text="LK (MAF)" ID="lblPeriodLK" runat="server" Visible="true" />
                                    <br />
                                    <asp:Label Text="Total (MAF)" ID="lblPeriodTotal" runat="server" Visible="true" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sulemanki u/s" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSulemanki" runat="server" CssClass="control-label" Text='<%# string.Format("{0:0.0}",Eval("Sulemanki")) %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblSulemankiEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblSulemankiLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblSulemankiTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="BS I Tail " ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblBS1" runat="server" CssClass="control-label" Text='<%# string.Format("{0:0.0}",Eval("BS1"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblBS1EK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBS1LK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBS1Total" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="BS II Tail" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblBS2" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("BS2"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblBS2EK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBS2LK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBS2Total" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Balloki u/s" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblBalloki" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("Balloki"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblBallokiEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBallokiLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblBallokiTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UCC Tail" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblUCC" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("UCC"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblUCCEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblUCCLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblUCCTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="MR Tail" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMR" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("MR"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblMREK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblMRLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblMRTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="QB Tail" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblQB" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("QB"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblQBEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblQBLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblQBTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Eastern" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblEastern" runat="server" CssClass="control-label" Text='<%#string.Format("{0:0.0}",Eval("Eastern"))%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="" ID="lblEasternEK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblEasternLK" runat="server" /><br />
                                    <asp:Label Text="" ID="lblEasternTotal" runat="server" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField>
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
                            </asp:TemplateField>--%>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>

    <script type="text/javascript">

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

