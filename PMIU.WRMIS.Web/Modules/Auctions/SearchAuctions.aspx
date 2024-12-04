<%@ Page Title="SearchAuctions" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SearchAuctions.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.SearchAuctions" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Search Auctions</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                    <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" CssClass="form-control" runat="server" AutoPostBack="True"  TabIndex="1">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" CssClass="form-control" runat="server" TabIndex="2">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                <div class="row">
                     <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" CssClass="form-control" runat="server" TabIndex="3">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblAuctionNotice" class="col-sm-4 col-lg-3 control-label">Auction Notice</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtAuctionNotice" runat="server" CssClass="form-control" MaxLength="100" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblSubmissionFrom" runat="server" class="col-sm-4 col-lg-3 control-label">Opening Date From</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtOpeningDateFrom" runat="server" CssClass=" date-picker form-control" TabIndex="5"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblSubmissionTo" runat="server" class="col-sm-4 col-lg-3 control-label">Opening Date to</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtOpeningDateTo" runat="server" CssClass="date-picker form-control" TabIndex="6"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" TabIndex="9" Visible="<%# base.CanEdit %>" />
                                <asp:HyperLink ID="hlAdd" runat="server"  NavigateUrl="~/Modules/Auctions/AddAuction.aspx" CssClass="btn btn-success" TabIndex="10">Add</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAuctionNotice" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvAuctionNotice_RowDataBound" AllowPaging="true"
                                OnPageIndexChanged="gvAuctionNotice_PageIndexChanged" OnPageIndexChanging="gvAuctionNotice_PageIndexChanging"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="ID">
                                <Columns>

                                    <asp:TemplateField HeaderText="Auction Notice">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlAuctionNotice" runat="server" Text='<%#Eval("AuctionTitle") %>' NavigateUrl='<%# String.Format("~/Modules/Auctions/AddAuction.aspx?AuctionNoticeID={0}&IsEditMode={1}", Convert.ToString(Eval("ID")),false) %>'>
                                        </asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-4" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("CO_Division.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submission Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubmissionDate" runat="server" CssClass="control-label" Text='<%# Eval("SubmissionDate", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Opening Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpeningDate" runat="server" CssClass="control-label" Text='<%# Eval("OpeningDate", "{0:d-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlAssets" runat="server" CssClass="btn btn-primary btn_32 parameters" NavigateUrl='<%# String.Format("~/Modules/Auctions/AddAuctionAssets.aspx?AuctionNoticeID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Assets" />
                                            <asp:HyperLink ID="hlBidders" runat="server"  CssClass="btn btn-primary btn_24 tenderworks" NavigateUrl='<%# String.Format("~/Modules/Auctions/Bidders.aspx?AuctionNoticeID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Bidders"></asp:HyperLink>
                                            <asp:HyperLink ID="hlBiddingProcess" runat="server"  CssClass="btn btn-primary btn_24 soldtenderlist" NavigateUrl='<%# String.Format("~/Modules/Auctions/AuctionCommitteAttendance.aspx?AuctionNoticeID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Bidding Process"></asp:HyperLink>
                                            <asp:HyperLink ID="hlPayments" runat="server"  CssClass="btn btn-primary btn_24 tenderevalcommittee" NavigateUrl='<%# String.Format("~/Modules/Auctions/AddPayment.aspx?AuctionNoticeID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Payments"></asp:HyperLink>
                                            <asp:HyperLink ID="hlApprovalProcess" runat="server"  CssClass="btn btn-primary btn_24 tenderopenprocess" ToolTip="Approval Process"></asp:HyperLink>
                                            <asp:HyperLink ID="hlEdit" runat="server"  CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# String.Format("~/Modules/Auctions/AddAuction.aspx?AuctionNoticeID={0}&IsEditMode={1}", Convert.ToString(Eval("ID")),true) %>' ToolTip="Edit"></asp:HyperLink>
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
</asp:Content>