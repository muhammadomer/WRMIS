<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuctionCommitteAttendance.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.AuctionCommitteAttendance" %>

<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Auction Committee Attendance</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <uc1:AuctionNotice runat="server" ID="AuctionNotice" />
                </div>
                <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li id="liCommittee" style="width: 20%; text-align: center" runat="server" class="active"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Auction Committee Attendance</a></li>
                            <li id="liBidders" style="width: 20%; text-align: center" runat="server"><a id="anchBidders" runat="server" aria-controls="BiddersAttendance" role="tab">Bidders Attendance</a></li>
                            <li id="liBidding" runat="server" style="width: 20%; text-align: center"><a id="anchBidding" runat="server" aria-controls="Bidding" role="tab">Bidding</a></li>
                            <li id="liBidderSelection" runat="server" style="width: 20%; text-align: center"><a id="anchBidderSelection" runat="server" aria-controls="Bidder Selection" role="tab">Bidder Selection</a></li>
                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Monitored By</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control " required="required" ID="ddlMonitoredBy" runat="server" TabIndex="1">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="12">ADM</asp:ListItem>
                                                <asp:ListItem Value="13">MA</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="DivisionDiv">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%-- <asp:DropDownList CssClass="form-control " required="required"  ID="ddlName" runat="server" TabIndex="2">
                                        </asp:DropDownList>--%>
                                                <asp:TextBox runat="server" ID="txtMonitordByName" required="required" type="text" MaxLength="100" class="form-control required"> </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label runat="server" id="lblOpenedBy" class="col-sm-4 col-lg-3 control-label">Opened By</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtOpenedBy" required="required" type="text" MaxLength="100" class="form-control required"> </asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label runat="server" id="lblbDesignation" class="col-sm-4 col-lg-3 control-label">Designation</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtDesignation" required="required" type="text" MaxLength="100" class="form-control required"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                    <div class="col-md-12">
                        <%--<h5><b>Advertisement Source</b></h5>--%>
                        <asp:GridView ID="gvAuctionCommiteeMembers" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowDeleting="gvAuctionCommiteeMembers_RowDeleting" OnRowDataBound="gvAuctionCommiteeMembers_OnRowDataBound"
                            AllowPaging="False"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <%-- <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control" ID="txtAdvertisementSource" Text='<%# Eval("AdvertisementSource") %>' runat="server"></asp:TextBox>
                                 </EditItemTemplate>--%>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblAdvertisementSource" runat="server" Text='<%# Eval("AdvertisementSource") %>'></asp:Label>--%>
                                        <asp:TextBox CssClass="form-control required" ID="txtMemberName" Text='<%# Eval("MemberName") %>' runat="server" MaxLength="100" required="required"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation">
                                    <%--  <EditItemTemplate>
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtAdvertisementDate" Text='<%# Eval("AdvertisementDate") %>' runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </EditItemTemplate>--%>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblAdvertisementDate" runat="server" Text='<%# Eval("AdvertisementDate") %>'></asp:Label>--%>
                                        <asp:TextBox CssClass="form-control required" ID="txtDesignation" Text='<%# Eval("Designation") %>' runat="server" MaxLength="100" required="required"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                            <asp:Button ID="btnAdvertisementGrid" runat="server" Text="" CommandName="AddMember" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" OnClick="AddRow_Grid" Visible="<%# base.CanAdd %>" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionAdvertisement" runat="server" HorizontalAlign="Center">
                                            <%--<asp:Button ID="btnEditAdvertisement" runat="server" Text="" CommandName="Edit" Visible="true" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />--%>
                                            <asp:Button ID="lbtnDeleteAdvertisement" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                                OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" Visible="<%# base.CanAdd %>" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <%--   <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionAdvertisement" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveAdvertisement" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelAdvertisement" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>--%>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                    </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-3 col-lg-2 control-label" id="lblAttachment" runat="server">Attach Attendance Sheet</label>
                                <div class="col-sm-2 col-lg-3 controls">
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                    <%--<asp:HyperLink ID="hlAttendanceSheet" CssClass="" Visible="false" runat="server" Text="abc" />--%>
                                </div>
                                <div>
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="row" runat="server" id="divSave">
                        <div class="col-md-12">
                            <%--   <asp:LinkButton TabIndex="10" ID="btnSave" runat="server" Text="Save & Proceed" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:LinkButton>--%>
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save & Continue" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnAuctionOpeningID" runat="server" Value="0" />

    <script type="text/javascript">
        function RemoveRequired() {
            //console.log("called");
            $('.CtrlClass0').removeAttr("required");
        }


    </script>


</asp:Content>
