<%@ Page Title="ApprovalProcess" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ApprovalProcess.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.ApprovalProcess" %>

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
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Approval Process</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                 <uc1:AuctionNotice runat="server" ID="AuctionNotice" />
                 </div>

                <div class="form-horizontal">
                   
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                           
                                <asp:GridView ID="gvApprovalProcess" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                DataKeyNames="ID,EarnestMoney,TokenMoney,EarnestMoneySubmitted,TokenMoneySubmitted,BidderRate,Status" OnRowDataBound="gvApprovalProcess_OnRowDataBound" OnRowCommand="gvApprovalProcess_RowCommand">
                                <Columns>
                                  <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Asset">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetName" runat="server" CssClass="control-label" Text='<%# Eval("AssetName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Selected Bidder">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSelectedBidder" runat="server" CssClass="control-label" Text='<%# Eval("SelectedBidder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Bidder Rate(Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBidderRate" runat="server" CssClass="control-label" Text='<%# Eval("BidderRate","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                          <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                <ItemStyle CssClass="text-right padding-right-number" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Money Paid">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalMoneyPaid" runat="server" CssClass="control-label" Text='<%# Eval("TotalMoneyPaid","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                <ItemStyle CssClass="text-right padding-right-number" />
                                    </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Status">
                                        
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Update Delivery Status">
                                      
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelivery" runat="server" Text="" CommandName="Delivery" CssClass="btn btn-primary btn_24 view" ToolTip="Delivery" formnovalidate="formnovalidate" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                           
                        </div>

                        
                    </div>
                </div>
                       
               
                      

                    <div class="row" runat="server" id="divSave">
                                <div class="col-md-12">
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                                  <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />--%>
                                </div>
                            </div>
              
                    </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" />
    <asp:HiddenField ID="hdnAuctionPriceID" runat="server" />
    

    <script type="text/javascript">
       


    </script>

    <script type="text/javascript">
       
</script>
</asp:Content>
