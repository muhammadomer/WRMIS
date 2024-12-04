<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChannelDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.ChannelDetail" EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Channel Detail</title>
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

    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart'] });
    </script>

</head>
<body style="background: #ffffff;">
    <div class="container-fluid">
        <div class="row">
            <form id="form1" class="form-horizontal" runat="server">
                <div class="col-md-12 col-sm-12">

                    <div class="col-md-12 col-sm-12">
                        <div runat="server" id="divNoResultFound" class="has-error" style="min-height: 20px; vertical-align: middle; margin: 28px auto; position: absolute; font-size: 14px; color: #ff0000;" visible="false">No Result Found</div>
                    </div>

                    <div class="col-md-12 col-sm-12">
                        <hr style="border-top: 1px solid #989797;" />
                    </div>

                    <%-- <div class="col-md-12 col-sm-12">--%>
                    <div class="col-md-12" runat="server" id="dvChannelDetail">
                        <input type="hidden" id="hdnChannelID" runat="server" />

                        <div class="row">

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Zone</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblZone" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Circle</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblCircle" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Division</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblDivision" runat="server" Text="" />
                                </div>
                            </div>

                        </div>


                        <div class="row">

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Sub Division</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblSubDivision" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Section</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblSection" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">IMIS Code</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblIMISCode" runat="server" Text="" />
                                </div>
                            </div>

                        </div>


                        <div class="row">


                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Channel Name</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblChannelName" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Parent Channel</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblParentChannel" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Channel Type</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblChannelType" runat="server" Text="" />
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Flow Type</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblFlowType" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Total RD</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblTotalRD" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Distance</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblDistance" runat="server" Text="" />
                                </div>
                            </div>

                        </div>


                        <div class="row">

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Designed Head Discharge</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblAuthHeadDischarge" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Authorized Tail Discharge</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblAuthTailDischarge" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Authorized Tail Gauge</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblAuthTailGuage" runat="server" Text="" />
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Gross Command Area</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblGCA" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="col-md-12 col-sm-12 bold">Culturable Command Area</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblCCA" runat="server" Text="" />
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-md-12 col-sm-12">
                                <div class="col-md-12 col-sm-12 bold">Channel Remarks</div>
                                <div class="col-md-12 col-sm-12">
                                    <asp:Label ID="lblChnlRemrks" runat="server" Text="" />
                                </div>
                            </div>

                        </div>

                    </div>
                    <%-- </div>--%>

                    <div class="col-md-12 col-sm-12" style="margin-top: 10px;">

                        <asp:ScriptManager ID="ScriptManagerMaster" runat="server" EnablePartialRendering="true">
                        </asp:ScriptManager>

                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>

                                <div class="col-md-12">
                                    <hr style="border-top: 1px solid #989797; margin: 0px;" />
                                </div>

                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <div class="col-md-12 col-sm-12 bold">Reading Date</div>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:Label ID="lblReadDate" runat="server" Text="" />
                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <div class="col-md-12 col-sm-12 bold">Head Discharge</div>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:Label ID="lblCurrHeadDis" runat="server" Text="" />
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-sm-4">
                                        <div class="col-md-12 col-sm-12 bold">Tail Discharge</div>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:Label ID="lblCurrTailDis" runat="server" Text="" />
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-sm-4">
                                        <div class="col-md-12 col-sm-12 bold">Tail Gauge</div>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:Label ID="lblCurrTailGauge" runat="server" Text="" />
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-12">
                                    <hr style="border-top: 1px solid #989797; margin-top: 5px; margin-bottom: 5px;" />
                                </div>

                                <div class="row text-center">

                                    <div class="col-md-12 col-sm-12 text-center">
                                        <div class="col-md-2 col-md-offset-5">
                                            <div class="form-group">
                                                <div class="row">
                                                    <div class="col-md-8">
                                                        <div id="datepicker12"></div>
                                                    </div>
                                                    <div class="col-md-offset-1 col-md-3" style="margin-top: 10px; display: none;">
                                                        <input type="text" runat="server" id="txtDateTime" disabled="disabled" style="width: 125px; padding: 2px 12px 2px 24px;" /><asp:Button runat="server" class="btn btn-primary" ID="btnRefreshGraph" OnClick="btnRefreshGraph_Click" Text="Refresh Graph" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-center">

                                    <div class="col-md-12 col-sm-12 text-center">
                                        <div runat="server" id="dvLtScripts"></div>
                                        <div id="chart_div" runat="server">
                                        </div>
                                    </div>

                                </div>
                                <div id="scrollDiv">&nbsp;</div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <%--<script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            xPos = $('#scrollDiv').offset().left;
            yPos = $('#scrollDiv').offset().top;
        }
        function EndRequestHandler(sender, args) {
            $(document).scrollTop(yPos);    // code here
            alert(yPos);
            $(document).ready(function () {
                //alert(xPos);
                alert(yPos);
                
            });
        }
    </script>--%>

    <script type="text/javascript">

        drawVisualization();
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
                $('#chart_div').hide();
                $('#dvLtScripts').hide();
            } else {
                $('#txtDateTime').val(currentDate1.format("yyyy-MM-dd"));
                window.setTimeout(function () { $("#btnRefreshGraph").click(); }, 500);
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    google.load('visualization', '1.0', { 'packages': ['corechart'] });

                    drawVisualization();

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
                            $('#chart_div').hide();
                            $('#dvLtScripts').hide();
                        } else {
                            $('#txtDateTime').val(currentDate1.format("yyyy-MM-dd"));
                            window.setTimeout(function () { $("#btnRefreshGraph").click(); }, 500);
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




