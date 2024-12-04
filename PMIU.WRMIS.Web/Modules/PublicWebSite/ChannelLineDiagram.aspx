<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ChannelLineDiagram.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.ChannelLineDiagram" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Channel Line Diagram</title>
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

    <!-- Css Style for Add User Screen -->
    <%--<link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.19/themes/cupertino/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" rel="Stylesheet" />--%>
    <link href="/Design/css/jquery-ui.css" rel="stylesheet" />

    <link href="/Design/css/custom.css" rel="stylesheet" />

    <%--These local files have been used due to internet problem--%>
    <%-- Original files have been restored 01-06-2016 --%>
    <%--START--%>
    <%--<script src="/Design/assets/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Design/assets/jquery-ui/jquery-ui.min.js"></script>--%>
    <script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <%--<script type="text/javascript" src="/bower_components/jquery/jquery.min.js"></script>--%>
    <%--<script type="text/javascript" src="/bower_components/moment/min/moment.min.js"></script>--%>
    <script type="text/javascript" src="/Design/assets/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/Design/assets/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>

    <%--END--%>
    <style>
        .table > tbody > tr > td {
            padding: 0px;
        }

        hr {
            border-top: 1px solid #f1f1f1;
            border-bottom: 1px solid #f1f1f1;
        }

        #form1 span.col-md-2 {
            font-size: 13px;
            font-weight: bold;
        }

        #form1 span.col-md-1 {
            font-size: 13px;
            font-weight: bold;
        }

        #tblComplaintInformation td span.bold {
            font-size: 13px;
            font-weight: bold;
        }

        /*#btnRefreshGraph {
            display: none;
        }*/

        div.bold {
            font-weight: bold;
            line-height: 1.42857143;
            padding-top: 8px;
        }

        #dvChannelDetail {
            background-color: #ffffff;
            /*padding-bottom: 15px;*/
        }

        .table-condensed > thead > tr > th, .table-condensed > tbody > tr > th, .table-condensed > tfoot > tr > th, .table-condensed > thead > tr > td, .table-condensed > tbody > tr > td, .table-condensed > tfoot > tr > td {
            padding: 1px;
        }
    </style>

    <script>window.jQuery || document.write('<script src="/Design/assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="/Design/assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="/Design/assets/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <script src="/Design/assets/jquery-cookie/jquery.cookie.js"></script>

    <!--page specific plugin scripts-->
    <script type="text/javascript" src="/Design/assets/bootstrap-datepicker/js/bootstrap-datepicker.js?1"></script>
    <script type="text/javascript" src="/Design/assets/bootstrap-timepicker/js/bootstrap-timepicker.min.js"></script>

    <script src="/Scripts/IrrigationNetwork/InputValidations.js"></script>
    <!--flaty scripts-->
    <%--<script src="/Design/js/flaty.js"></script>--%>
    <script src="/Design/js/custom.js"></script>
    <script src="/Scripts/jquery.numeric.js"></script>

    <!-- END Main Content -->
    <script src="../../Scripts/Complaints/loader.js"></script>
    <script src="../../Scripts/Complaints/jsapi.js"></script>


</head>
<body style="background: #ffffff;">
    <div class="container-fluid">
        <div class="row">
            <form id="form1" class="form-horizontal" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Source Channel</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlChannel" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row text-center">
                    <div class="col-md-12 col-sm-12 text-center">
                        <asp:ScriptManager ID="ScriptManagerMaster" runat="server" EnablePartialRendering="true">
                        </asp:ScriptManager>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-2 col-md-offset-2">
                                        <%--<div class="form-group">--%>
                                        <div class="row">
                                            <div class="col-md-8">
                                                <div id="datepicker12"></div>
                                            </div>
                                            <div class="col-md-offset-1 col-md-3" style="margin-top: 10px; display: none;">
                                                <input type="text" runat="server" id="txtDateTime" style="width: 125px; padding: 2px 12px 2px 24px;" />
                                                <%--<asp:Button runat="server" class="btn btn-primary" ID="btnRefreshGraph" OnClick="btnRefreshGraph_Click" Text="Refresh Graph" />--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 col-md-offset-1">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChannelLineDiagram" runat="server" DataKeyNames="CSide,ChannelName,ParrentFeederRDS,Status" AutoGenerateColumns="False" EmptyDataText="No record found" OnRowDataBound="gvChannelLineDiagram_RowDataBound"
                                ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false">
                                <Columns>

                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("Division") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Design Discharge(CS)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignDischarge" runat="server" Text='<%# Eval("DesignDischarge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Indent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLegacyIndent" runat="server" Text='<%# Eval("Legacy_indent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Head Gauge(ft)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeadGauge" runat="server" Text='<%# Eval("HeadGauge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Head Discharge(CS)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeadDischare" runat="server" Text='<%# Eval("HeadDischare") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Auth. Tail Gauge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAuthorizedTailGauge" runat="server" Text='<%# Eval("AuthorizedTailGauge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Tail Gauge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTailGauge" runat="server" Text='<%# Eval("TailGauge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="&nbsp;">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelColor" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" />

                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="&nbsp;">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Right Offtakes">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLOfftakes" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-center" />
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="R.D.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLRD" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-center" />
                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Source">
                                        <ItemTemplate>
                                            <asp:Image ID="imgSource" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Left Offtakes">
                                        <ItemTemplate>
                                            <asp:Label ID="lblROfftakes" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R.D.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRRD" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-center" />
                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">


        $('#datepicker12').datepicker({
            inline: true,
            sideBySide: true

        });

        var yearStr, monthStr, dayStr;

        if ($("#txtDateTime").val() != '') {
            yearStr = new Date($("#txtDateTime").val()).getFullYear();
            monthStr = new Date($("#txtDateTime").val()).getMonth();
            dayStr = new Date($("#txtDateTime").val()).getDate();
        }
        else {
            yearStr = new Date().getFullYear();
            monthStr = new Date().getMonth();
            dayStr = new Date().getDate();
        }

        $('#datepicker12').datepicker('update', new Date(yearStr, monthStr, dayStr));

        $('#datepicker12').on("changeDate", function () {
            var currentDate1 = new Date($('#datepicker12').datepicker('getDate'));
            if (isNaN(currentDate1)) {
                //alert("Errror");
            } else {
                $('#txtDateTime').val(currentDate1.format("yyyy-MM-dd"));
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    $('#datepicker12').datepicker({
                        inline: true,
                        sideBySide: true
                    });

                    var yearStr, monthStr, dayStr;

                    if ($("#txtDateTime").val() != '') {
                        yearStr = new Date($("#txtDateTime").val()).getFullYear();
                        monthStr = new Date($("#txtDateTime").val()).getMonth();
                        dayStr = new Date($("#txtDateTime").val()).getDate();
                    }
                    else {
                        yearStr = new Date().getFullYear();
                        monthStr = new Date().getMonth();
                        dayStr = new Date().getDate();
                    }

                    $('#datepicker12').datepicker('update', new Date(yearStr, monthStr, dayStr));

                    $('#datepicker12').on("changeDate", function () {
                        var currentDate1 = new Date($('#datepicker12').datepicker('txtDateTime'));

                        if (isNaN(currentDate1)) {
                            // alert("Error");
                        } else {
                            $('#txtDateTime').val(currentDate1.format("yyyy-MM-dd"));
                        }
                        //var currentDate1 = new Date($('#datepicker12').datepicker('getDate'));
                        //$('#txtDateTime').val(currentDate1.format("yyyy-MM-dd"));
                        //window.setTimeout(function () { $("#btnRefreshGraph").click(); }, 500);
                    });

                }
            });
        }
    </script>
















</body>
</html>

