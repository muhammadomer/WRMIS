<%@ Page Title="Probability" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Probability.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.Probability" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Probabilistic Flows</h3>
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblRimStation" class="col-sm-4 col-lg-3 control-label">Rim Station</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlRimStatn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRimStatn_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                    <div id="roleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                        <asp:DropDownList CssClass="form-control required" required="true" Enabled="false" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-horizontal">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="lblYear" class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="required" Enabled="false" ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-big" style="border: 0px;">
                        <asp:GridView ID="gvProbability" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            OnRowCreated="gvProbability_RowCreated" OnRowDataBound="gvProbability_RowDataBound"
                            ShowHeaderWhenEmpty="True" ShowFooter="false" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("TDailyID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Period">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDailyName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="EK(MAF)" ID="lblPeriodEK" runat="server" Visible="true" />
                                        <br />
                                        <asp:Label Text="LK(MAF)" ID="lblPeriodLK" runat="server" Visible="true" />
                                        <br />
                                        <asp:Label Text="Total(MAF)" ID="lblPeriodTotal" runat="server" Visible="true" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZeroDis" runat="server" Text='<%# Eval("MaxDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZeroVol" runat="server" Text='<%# Eval("MaxVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEK0" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLK0" runat="server" /><br />
                                        <asp:Label Text="" ID="lblKharif0" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiveDis" runat="server" Text='<%# Eval("FiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiveVol" runat="server" Text='<%# Eval("FiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEK5" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLK5" runat="server" /><br />
                                        <asp:Label Text="" ID="lblKharif5" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTenDis" runat="server" Text='<%# Eval("TenDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTenVol" runat="server" Text='<%# Eval("TenVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKTen" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKTen" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalTen" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFifteenDis" runat="server" Text='<%# Eval("FifteenDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFifteenVol" runat="server" Text='<%# Eval("FifteenVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKFifteen" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKFifteen" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalFifteen" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTwentyDis" runat="server" Text='<%# Eval("TwentyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTwentyVol" runat="server" Text='<%# Eval("TwentyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKTwenty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKTwenty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalTwenty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTwentyFiveDis" runat="server" Text='<%# Eval("TwentyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTwentyFiveVol" runat="server" Text='<%# Eval("TwentyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKTwentyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKTwentyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalTwentyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblThirtyDis" runat="server" Text='<%# Eval("ThirtyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblThirtyVol" runat="server" Text='<%# Eval("ThirtyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKThirty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKThirty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalThirty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblThirtyFiveDis" runat="server" Text='<%# Eval("ThirtyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblThirtyFiveVol" runat="server" Text='<%# Eval("ThirtyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKThirtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKThirtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalThirtyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFourtyDis" runat="server" Text='<%# Eval("FourtyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFourtyVol" runat="server" Text='<%# Eval("FourtyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKFourty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKFourty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalFourty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFourtyFiveDis" runat="server" Text='<%# Eval("FourtyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFourtyFiveVol" runat="server" Text='<%# Eval("FourtyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKFourtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKFourtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalFourtyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiftyDis" runat="server" Text='<%# Eval("FiftyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiftyVol" runat="server" Text='<%# Eval("FiftyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKFifty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKFifty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalFifty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiftyFiveDis" runat="server" Text='<%# Eval("FiftyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiftyFiveVol" runat="server" Text='<%# Eval("FiftyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKFiftyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKFiftyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalFiftyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSixtyDis" runat="server" Text='<%# Eval("SixtyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSixtyVol" runat="server" Text='<%# Eval("SixtyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKSixty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKSixty" runat="server" /><br />
                                        <asp:Label Text="" ID="lbltotalSixty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSixtyFiveDis" runat="server" Text='<%# Eval("SixtyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSixtyFiveVol" runat="server" Text='<%# Eval("SixtyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKSixtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKSixtyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalSixtyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeventyDis" runat="server" Text='<%# Eval("SeventyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeventyVol" runat="server" Text='<%# Eval("SeventyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKSeventy" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKSeventy" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalSeventy" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeventyFiveDis" runat="server" Text='<%# Eval("SeventyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeventyFiveVol" runat="server" Text='<%# Eval("SeventyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKSeventyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKSeventyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalSeventyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEightyDis" runat="server" Text='<%# Eval("EightyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEightyVol" runat="server" Text='<%# Eval("EightyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKEighty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKEighty" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalEighty" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEightyFiveDis" runat="server" Text='<%# Eval("EightyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEightyFiveVol" runat="server" Text='<%# Eval("EightyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKEightyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKEightyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalEightyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNinetyDis" runat="server" Text='<%# Eval("NinetyDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNinetyVol" runat="server" Text='<%# Eval("NinetyVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKNinety" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKNinety" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalNinety" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNinetyFiveDis" runat="server" Text='<%# Eval("NinetyFiveDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNinetyFiveVol" runat="server" Text='<%# Eval("NinetyFiveVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKNinetyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKNinetyFive" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalNinetyFive" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dis.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMinimumDis" runat="server" Text='<%# Eval("MinimumDis") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Vol.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMinimumVol" runat="server" Text='<%# Eval("MinimumVol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Text="" ID="lblEKMinimum" runat="server" /><br />
                                        <asp:Label Text="" ID="lblLKMinimum" runat="server" /><br />
                                        <asp:Label Text="" ID="lblTotalMinimum" runat="server" />
                                    </FooterTemplate>
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
    </div>

    <%--<div class="modal fade" id="Alert" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">
                    <div class="box">
                        <div class="box-title">
                            <h3>Probability</h3>
                        </div>
                        <div class="box-content">
                            <asp:Label ID="lblMsg" runat="server" Text="Any Msg."></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnYes" Text="Yes" runat="server" CssClass="btn btn-info" OnClick="btnYes_Click" />
                    <asp:Button ID="btnNo" Text="No" runat="server" CssClass="btn btn-info" OnClick="btnNo_Click" />
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
