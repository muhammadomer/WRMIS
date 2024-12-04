<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchEntitlements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.SearchEntitlements" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Entitlements</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link rel="stylesheet" href="/Design/assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/Design/assets/font-awesome/css/font-awesome.min.css" />

    <!--page specific css styles-->
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-timepicker/css/bootstrap-timepicker.min.css" />
    <!--flaty css styles-->
    <link rel="stylesheet" href="/Design/css/flaty.css?v=0" />
    <link rel="stylesheet" href="/Design/css/flaty-responsive.css" />

    <link rel="shortcut icon" href="/Design/img/favicon.html" />

    <link href="/Design/css/jquery-ui.css" rel="stylesheet" />

    <link href="/Design/css/custom.css" rel="stylesheet" />

    <script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <div class="col-md-12">
            <div class="box">
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Season</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                        <hr />
                    </div>
                    <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-md-12 text-left">
                                <h3>
                                    <asp:Label runat="server" ID="lblTitle" Text="TENTATIVE DISTRIBUTION PROGRAMME RABI 2016-17" Font-Bold="true" /></h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lbl7782AverageText" runat="server" Text="Average System Usage 1977-1982 (MAF):" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lbl7782Average" runat="server" Text="19.751" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblPara2Text" runat="server" Text="Punjab Para(2) Share (MAF):" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblPara2" runat="server" Text="37.070" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblExpectedShareText" runat="server" Text="Expected Share Approved by IRSA (MAF):" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblExpectedShare" runat="server" Text="16.160" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblPercentChangeText" runat="server" Text=" % Change w.r.t Average Usage System:" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblPercentChange" runat="server" Text="-17.2" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblTarbelaCommandText" runat="server" Text="Tarbela Command Total (MAF):" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblTarbelaCommand" runat="server" Text="6.499" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblManglaCommandText" runat="server" Text="Mangla Command Total (MAF):" />
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblManglaCommand" runat="server" Text="9.661" />
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <br />
                        <div class="row" style="margin-bottom: 20px;">
                            <div class="col-md-12 text-center">
                                <asp:Label runat="server" ID="lblTarbela" Text="INDUS ZONE (TARBELA COMMAND)" Font-Bold="true" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvTarbelaCommand" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerialNumber" runat="server" CssClass="control-label" Text='<%# Container.DataItemIndex+1 %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlChannel" CssClass="control-label" Text='<%# Eval("ChannelName") %>' NavigateUrl='<%# String.Format("ViewChlidCanalsMAF.aspx?ChannelID={0}&SeasonID={1}&Year={2}&CommandID={3}&Channel={4}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),((long)PMIU.WRMIS.Common.Constants.Commands.IndusCommand),Convert.ToString(Eval("ChannelName"))).ToString() %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposed Share (MAF)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedShare" runat="server" CssClass="control-label" Text='<%# String.Format("{0:0.000}", Convert.ToDouble(Eval("Entitlement"))) %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlDetail" CssClass="control-label" Text='Detail' NavigateUrl='<%# String.Format("ViewEntitlement.aspx?ChannelID={0}&SeasonID={1}&Year={2}&CommandID={3}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),((long)PMIU.WRMIS.Common.Constants.Commands.IndusCommand)).ToString() %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 20px;">
                            <div class="col-md-12 text-center">
                                <asp:Label runat="server" ID="lblMangla" Text="JHELUM CHENAB ZONE (MANGLA COMMAND)" Font-Bold="true" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvManglaCommand" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerialNumber" runat="server" CssClass="control-label" Text='<%# Container.DataItemIndex+1 %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlChannel" CssClass="control-label" Text='<%# Eval("ChannelName") %>' NavigateUrl='<%# String.Format("ViewChlidCanalsMAF.aspx?ChannelID={0}&SeasonID={1}&Year={2}&CommandID={3}&Channel={4}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),((long)PMIU.WRMIS.Common.Constants.Commands.JhelumChenabCommand),Convert.ToString(Eval("ChannelName"))).ToString() %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposed Share (MAF)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedShare" runat="server" CssClass="control-label" Text='<%# String.Format("{0:0.000}", Convert.ToDouble(Eval("Entitlement"))) %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlDetailMang" CssClass="control-label" Text='Detail' NavigateUrl='<%# String.Format("ViewEntitlement.aspx?ChannelID={0}&SeasonID={1}&Year={2}&CommandID={3}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),((long)PMIU.WRMIS.Common.Constants.Commands.JhelumChenabCommand)).ToString() %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
