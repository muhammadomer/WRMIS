<%@ Page Title="AuctionAssetDetails" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AuctionAssetDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.AuctionAssetDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionItemDetails.ascx" TagPrefix="uc1" TagName="AuctionItemDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Asset Details</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <uc1:AuctionItemDetails runat="server" ID="AuctionItemDetails" />
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Reserve Price(Rs)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtReservePrice" runat="server" CssClass="form-control decimalIntegerInput required" required="required" MaxLength="15" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Earnest Money(Rs)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlEarnestMoney" CssClass="form-control required" required="required" runat="server" TabIndex="2">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <%--<label class="col-sm-4 col-lg-3 control-label">Earnest Money(Rs)</label>--%>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtEarnestMoney" runat="server" CssClass="form-control decimalIntegerInput required" required="required" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Submission Date of Balance Amount</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtBalanceAmountDate" runat="server" CssClass="form-control date-picker" type="text" TabIndex="4"></asp:TextBox>
                                        <span id="spanBalanceAmountDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Token Money(Rs)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlTokenMoney" CssClass="form-control required" required="required" runat="server" TabIndex="5">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                           <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtTokenMoney" runat="server" CssClass="form-control decimalIntegerInput required" required="required" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click"  />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnAuctionAssetID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnAuctionAssetDetailID" runat="server" Value="0" />
</asp:Content>
