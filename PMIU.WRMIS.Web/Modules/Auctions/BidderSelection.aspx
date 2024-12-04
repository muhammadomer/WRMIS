<%@ Page Title="BidderSelection" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="BidderSelection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.BidderSelection" %>

<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
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
                <h3 id="htitle" runat="server">Bidder Selection</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <uc1:AuctionNotice runat="server" ID="AuctionNotice" />
                </div>

                <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li id="liCommittee" style="width: 20%; text-align: center" runat="server"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Auction Committee Attendance</a></li>
                            <li id="liBidders" style="width: 20%; text-align: center" runat="server"><a id="anchBidders" runat="server" aria-controls="BiddersAttendance" role="tab">Bidders Attendance</a></li>
                            <li id="liBidding" runat="server" style="width: 20%; text-align: center"><a id="anchBidding" runat="server" aria-controls="Bidding" role="tab">Bidding</a></li>
                            <li id="liBidderSelection" runat="server" style="width: 20%; text-align: center" class="active"><a id="anchBidderSelection" runat="server" aria-controls="Bidder Selection" role="tab">Bidder Selection</a></li>

                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <%--<asp:UpdatePanel runat="server">
                        <ContentTemplate>--%>


                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Assets</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList CssClass="form-control" required="required" ID="ddlAssets" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlAssets_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>


                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvBiddersRate" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    DataKeyNames="ID,BidderID,AuctionAssetDetailID" OnRowDataBound="gvBiddersRate_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="IsAwarded" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="IsAwarded" runat="server" CssClass="control-label" Text='<%# Eval("IsAwarded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AwardedReason" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="AwardedReason" runat="server" CssClass="control-label" Text='<%# Eval("AwardedReason") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BidderID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="BidderID" runat="server" CssClass="control-label" Text='<%# Eval("BidderID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Auction Asset Detail ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="AuctionAssetDetailID" runat="server" CssClass="control-label" Text='<%# Eval("AuctionAssetDetailID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Contractors">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContractor" runat="server" CssClass="control-label" Text='<%# Eval("Contractor") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                            <ItemStyle CssClass="col-md-4" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount(Rs.)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBidderRate" runat="server" CssClass="control-label" Text='<%# Eval("BidderRate","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-right padding-right-number" />
                                            <ItemStyle CssClass="text-right padding-right-number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Award">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdButton" name="rdBAward" ClientIDMode="Static" runat="server" CssClass="control-label" Checked='<%# bool.Parse(Eval("isChecked").ToString()) %>' GroupName="SelectGroup" onclick="RadioCheck(this);" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                            <ItemStyle CssClass="col-md-4" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-2 col-lg-1 control-label">Reason</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtReason" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize txtReason" TabIndex="5" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="divSave">
                        <div class="col-md-12">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" />
    <script type="text/javascript">
    </script>
    <script type="text/javascript">
        //$(function () {

        //});
        function RadioCheck(rb) {
            //debugger;
            var gv = document.getElementById("<%=gvBiddersRate.ClientID%>");
            var rbs = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        // $("input[id*=txtRemarks]").attr("disabled", "disabled");
                        //$("input[id*=txtRemarks]").val("");
                        break;
                    }

                }
                else {
                    //$("input[id*=txtRemarks]").attr("disabled", "disabled");
                    //$("input[id*=txtRemarks]").val("");
                }
            }
        }
    </script>
</asp:Content>

