<%@ Page Title="ViewBidders" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ViewBidderList.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.ViewBidderList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <style type="text/css">
        .padding-right-number {
            padding-right: 35px !important;
        }

        @media only screen and (min-width: 1300px) {
            .padding-right-number {
                padding-right: 45px !important;
            }
        }

        @media only screen and (min-width: 1400px) {
            .gridReachStartingRDs {
                width: 15%;
            }
        }

        @media only screen and (min-width: 1500px) {
            .gridReachStartingRDs {
                width: 14%;
            }
        }

        @media only screen and (min-width: 1400px) {
            .padding-right-number {
                padding-right: 75px !important;
            }
        }

        .Hide {
            display: none;
        }
    </style>
       <div class="box">
        <div class="box-title">
            <h3>Bidders</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <uc1:AuctionNotice runat="server" id="AuctionNotice" />
                 <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAssetBidders" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowDataBound="gvAssetBidders_OnRowDataBound"
                                DataKeyNames="Attachment,EarnestMoney,EarnestMoneySubmitted">
                        <Columns>
                            <asp:TemplateField HeaderText="Company/BidderName">
                              <ItemTemplate>
                                    <asp:Label ID="lblBidderName" runat="server" Text='<%# Eval("BidderName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asset Name">
                                 <ItemTemplate>
                                    <asp:Label ID="lblAssetName" runat="server" Text='<%# Eval("AssetName") %>'></asp:Label>
                                    </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Earnest Money">
                            <ItemTemplate>
                                    <asp:Label ID="lblEarnestMoney" runat="server" Text='<%# Eval("EarnestMoney") %>'></asp:Label>
                                    </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                <ItemStyle CssClass="text-right padding-right-number" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Earnest Money Submitted">
                              <ItemTemplate>
                                    <asp:Label ID="lblEarnestMoneySubmitted" runat="server" Text='<%# Eval("EarnestMoneySubmitted") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                <ItemStyle CssClass="text-right padding-right-number" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Attachment">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 " />
                              </asp:TemplateField>
                          
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                        </div>

                        
                    </div>
                </div>
                 </div>

          
               <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <%--<asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click"  />--%>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    
</asp:Content>
