<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewChlidCanalsMAF.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.ViewChlidCanalsMAF" %>

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
                     <asp:Panel runat="server" ID="pnlContainer">
                      
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvChildChannels" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="ChannelID,ChannelName,Season,Year,CommandID" OnRowCommand="gvChildChannels_RowCommand" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerialNumber" runat="server" CssClass="control-label" Text='<%# Container.DataItemIndex+1 %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="hlChannel" CssClass="control-label" Text='<%# Eval("ChannelName") %>'  CommandName="MoreChild" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposed Share (MAF)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedShare" runat="server" CssClass="control-label" Text='<%# String.Format("{0:0.000}", Convert.ToDouble(Eval("MAFEntitlement"))) %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlDetail" CssClass="control-label" Text='Detail' NavigateUrl='<%# String.Format("ViewChildEntitlements.aspx?ChannelID={0}&SeasonID={1}&Year={2}&ParentChild={3}&CommandID={4}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),"C",Convert.ToString(Eval("CommandID"))).ToString() %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-7" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                         <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" onclick="javascript:history.go(-1)" CssClass="btn" Text="Back" />
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
