<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RiversAndCanalsDischarge.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.RiversAndCanalsDischarge" EnableEventValidation="true" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>R & C Discharges</title>


    <style>
        @import url('https://fonts.googleapis.com/css?family=Lato:400,400i');

        @page {
            size: auto;
            margin: 5mm 15mm 5mm 15mm;
        }

        * {
            padding: 0;
            margin: 0;
        }

        body {
            font-family: 'Lato', sans-serif;
            color: #3B3A38;
            margin: 10px;
        }

        .template {
            display: block;
            width: 950px;
            margin: 0 auto;
            position: relative;
        }

            .template img {
                width: 100%;
            }

            .template .big {
                font-size: 16px;
                font-weight: bold;
            }

            .template .med {
                font-size: 13px;
                font-weight: bold;
            }

            .template .small {
                font-size: 10px;
            }

            .template span {
                position: absolute;
                text-align: center;
                overflow: hidden;
            }

        #skardu {
            top: 83px;
            left: 35px;
            width: 50px;
        }

        #dated {
            top: 34px;
            left: 413px;
            width: 110px;
        }

        #bunji {
            top: 126px;
            left: 28px;
            width: 65px;
        }

        #bisham {
            top: 173px;
            left: 28px;
            width: 65px;
        }

        #tarbela {
            top: 203px;
            left: 18px;
            width: 90px;
        }

        #tarbela_top {
            top: 228px;
            left: 28px;
            width: 65px;
        }

        #tarbela_bot {
            top: 257px;
            left: 28px;
            width: 65px;
        }

        #kabul {
            top: 298px;
            left: 29px;
            width: 60px;
        }

        #kalabagh_top {
            top: 356px;
            left: 19px;
            width: 85px;
        }

        #kalabagh_bot {
            top: 384px;
            left: 19px;
            width: 85px;
        }

        #chashma_top {
            top: 491px;
            left: 11px;
            width: 85px;
        }

        #chashma_bot {
            top: 517px;
            left: 11px;
            width: 85px;
        }

        #cbrc_head {
            top: 621px;
            left: 28px;
            transform: rotate(-75deg);
            width: 100px;
        }

        #cbrc_pb_rd {
            top: 646px;
            left: 54px;
            transform: rotate(-75deg);
            width: 100px;
        }

        #taunsa_top {
            top: 837px;
            left: 8px;
            width: 85px;
        }

        #taunsa_bot {
            top: 861px;
            left: 8px;
            width: 85px;
        }

        #kachhi {
            top: 998px;
            left: 18px;
            transform: rotate(-68deg);
            width: 100px;
        }

        #dgkhan {
            top: 1062px;
            left: 44px;
            transform: rotate(-78deg);
            width: 100px;
        }

        #guddu_top {
            top: 1200px;
            left: 30px;
            width: 62px;
        }

        #guddu_bot {
            top: 1219px;
            left: 30px;
            width: 62px;
        }

        #sukkur_top {
            top: 1252px;
            left: 6px;
            width: 62px;
        }

        #sukkur_bot {
            top: 1271px;
            left: 6px;
            width: 62px;
        }

        #kotri_top {
            top: 1294px;
            left: 0px;
            width: 62px;
        }

        #kotri_bot {
            top: 1313px;
            left: 0px;
            width: 62px;
        }

        #thal {
            top: 579px;
            left: 116px;
            transform: rotate(90deg);
            width: 110px;
        }

        #cjlink {
            top: 544px;
            left: 230px;
            transform: rotate(40deg);
            width: 110px;
        }

        #greater_thal {
            top: 598px;
            left: 179px;
            transform: rotate(-67deg);
            width: 110px;
        }

        #tplink {
            top: 939px;
            left: 138px;
            transform: rotate(30deg);
            width: 110px;
        }

        #muzaffargarh {
            top: 998px;
            left: 120px;
            transform: rotate(76deg);
            width: 100px;
        }

        #mangla {
            top: 189px;
            left: 243px;
            width: 90px;
        }

        #mangla_top {
            top: 222px;
            left: 253px;
            width: 70px;
        }

        #mangla_bot {
            top: 244px;
            left: 253px;
            width: 70px;
        }

        #rasul_top {
            top: 409px;
            left: 262px;
            width: 70px;
        }

        #rasul_bot {
            top: 432px;
            left: 262px;
            width: 70px;
        }

        #trimmu_top {
            top: 714px;
            left: 246px;
            width: 70px;
        }

        #trimmu_bot {
            top: 737px;
            left: 246px;
            width: 70px;
        }

        #rangpur {
            top: 920px;
            left: 231px;
            transform: rotate(-58deg);
            width: 100px;
        }

        #rtp {
            top: 1002px;
            left: 193px;
            transform: rotate(-65deg);
            font-size: 8px;
            width: 85px;
        }

        #punjnad_top {
            top: 1048px;
            left: 180px;
            width: 70px;
        }

        #punjnad_bot {
            top: 1074px;
            left: 180px;
            width: 70px;
        }

        #punjnad {
            top: 1227px;
            left: 184px;
            transform: rotate(-55deg);
            width: 100px;
        }

        #abbasia {
            top: 1261px;
            left: 218px;
            transform: rotate(-59deg);
            width: 100px;
        }

        #abbasia_link {
            top: 1274px;
            left: 288px;
            transform: rotate(-63deg);
            width: 100px;
        }

        #ujc {
            top: 378px;
            left: 345px;
            transform: rotate(76deg);
            width: 100px;
        }

        #rpc {
            top: 402px;
            left: 326px;
            font-size: 8px;
            transform: rotate(-91deg);
            width: 75px;
        }

        #ujc_int {
            top: 478px;
            left: 376px;
            transform: rotate(43deg);
            font-size: 8px;
            width: 75px;
        }

        #rqlink {
            top: 501px;
            left: 364px;
            transform: rotate(28deg);
            font-size: 8px;
            width: 100px;
        }

        #lower_jehlum {
            top: 591px;
            left: 348px;
            transform: rotate(79deg);
            width: 100px;
        }

        #tslink {
            top: 793px;
            left: 355px;
            transform: rotate(27deg);
            width: 120px;
        }

        #smblink {
            top: 865px;
            left: 473px;
            transform: rotate(31deg);
            width: 111px;
            font-size: 7px;
        }

        #havali {
            top: 833px;
            left: 393px;
            transform: rotate(28deg);
            font-size: 8px;
            width: 50px;
        }

        #havali_int {
            top: 871px;
            left: 332px;
            transform: rotate(-58deg);
            font-size: 8px;
            width: 75px;
        }

        #marala_top {
            top: 266px;
            left: 440px;
            width: 56px;
        }

        #marala_bot {
            top: 288px;
            left: 440px;
            width: 56px;
        }

        #khanki_top {
            top: 489px;
            left: 445px;
            width: 56px;
        }

        #khanki_bot {
            top: 510px;
            left: 445px;
            width: 56px;
        }

        #qadirabad_top {
            top: 605px;
            left: 435px;
            width: 56px;
        }

        #qadirabad_bot {
            top: 626px;
            left: 435px;
            width: 56px;
        }

        #sidhnal_top {
            top: 738px;
            left: 452px;
            width: 56px;
        }

        #sidhnal_bot {
            top: 757px;
            left: 452px;
            width: 56px;
        }

        #sidhnal {
            top: 914px;
            left: 340px;
            transform: rotate(-52deg);
            width: 100px;
        }

        #pakpattan {
            top: 903px;
            left: 420px;
            transform: rotate(-19deg);
            width: 100px;
        }

        #lmailsi {
            top: 931px;
            left: 451px;
            transform: rotate(-17deg);
            width: 100px;
        }

        #smlink {
            top: 988px;
            left: 485px;
            transform: rotate(-32deg);
            width: 100px;
        }

        #rd161 {
            top: 1083px;
            left: 472px;
            transform: rotate(-50deg);
            width: 114px;
        }

        #lalsohara {
            top: 1155px;
            left: 496px;
            transform: rotate(-52deg);
            width: 100px;
        }

        #mrlink {
            top: 326px;
            left: 529px;
            transform: rotate(29deg);
            width: 114px;
        }

        #mrint {
            top: 374px;
            left: 567px;
            transform: rotate(27deg);
            width: 75px;
            font-size: 8px;
        }

        #brbdlink {
            top: 411px;
            left: 568px;
            transform: rotate(16deg);
            width: 56px;
            font-size: 8px;
        }

        #ucc {
            top: 437px;
            left: 517px;
            transform: rotate(67deg);
            width: 86px;
            font-size: 8px;
        }

        #mll {
            top: 529px;
            left: 595px;
            transform: rotate(63deg);
            width: 85px;
            font-size: 8px;
        }

        #uccint {
            top: 515px;
            left: 553px;
            transform: rotate(77deg);
            width: 75px;
            font-size: 8px;
        }

        #lcc {
            top: 515px;
            left: 504px;
            transform: rotate(79deg);
            width: 75px;
            font-size: 8px;
        }

        #qblink {
            top: 579px;
            left: 556px;
            transform: rotate(30deg);
            width: 93px;
            font-size: 8px;
        }

        #lcc_total {
            top: 630px;
            left: 526px;
            transform: rotate(78deg);
            width: 100px;
            font-size: 8px;
        }

        #lcc_feeder {
            top: 645px;
            left: 471px;
            transform: rotate(82deg);
            width: 100px;
            font-size: 8px;
        }

        #balloki_top {
            top: 676px;
            left: 586px;
            width: 56px;
        }

        #balloki_bot {
            top: 697px;
            left: 586px;
            width: 56px;
        }

        #lbdc {
            top: 797px;
            left: 570px;
            transform: rotate(-31deg);
            width: 100px;
        }

        #mplink {
            top: 815px;
            left: 615px;
            transform: rotate(85deg);
            width: 56px;
            font-size: 8px;
        }

        #pilink {
            top: 875px;
            left: 614px;
            transform: rotate(85deg);
            width: 60px;
            font-size: 8px;
        }

        #qaim {
            top: 955px;
            left: 591px;
            transform: rotate(-46deg);
            width: 52px;
            font-size: 8px;
        }

        #ubc {
            top: 1045px;
            left: 591px;
            transform: rotate(-51deg);
            width: 100px;
        }

        #islam_top {
            top: 967px;
            left: 659px;
            width: 50px;
        }

        #islam_bot {
            top: 987px;
            left: 659px;
            width: 50px;
        }

        #cbdc {
            top: 562px;
            left: 650px;
            transform: rotate(84deg);
            width: 100px;
        }

        #brbd_int {
            top: 462px;
            left: 700px;
            transform: rotate(44deg);
            width: 100px;
        }

        #udc {
            top: 572px;
            left: 712px;
            transform: rotate(101deg);
            width: 100px;
        }

        #bslink {
            top: 629px;
            left: 661px;
            transform: rotate(28deg);
            width: 65px;
            font-size: 8px;
        }

        #bslink_2 {
            top: 649px;
            left: 716px;
            transform: rotate(27deg);
            width: 100px;
        }

        #bslink_1 {
            top: 687px;
            left: 675px;
            transform: rotate(27deg);
            width: 65px;
            font-size: 8px;
        }

        #ldc {
            top: 737px;
            left: 698px;
            transform: rotate(-81deg);
            width: 100px;
            font-size: 8px;
        }

        #upakpattan {
            top: 838px;
            left: 677px;
            transform: rotate(-27deg);
            width: 100px;
        }

        #fardwah {
            top: 867px;
            left: 761px;
            transform: rotate(-49deg);
            width: 100px;
        }

        #sadiqia {
            top: 914px;
            left: 769px;
            transform: rotate(-52deg);
            width: 100px;
        }

        #sulemanki_top {
            top: 775px;
            left: 872px;
            width: 60px;
        }

        #sulemanki_bot {
            top: 797px;
            left: 872px;
            width: 60px;
        }
    </style>

    <link href="/Content/DefaultStyle.css" rel="stylesheet" type="text/css" />
    <%--<link href="/Content/pager.css"  rel="stylesheet" type="text/css" />--%>

    <link rel="stylesheet" href="/Design/assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="/Design/assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-timepicker/css/bootstrap-timepicker.min.css" />
    <!--flaty css styles-->
    <link rel="stylesheet" href="/Design/css/flaty.css?v=0">
    <link rel="stylesheet" href="/Design/css/flaty-responsive.css">


    <link rel="shortcut icon" type="image/x-icon" href="/Design/img/favicon.ico" />
    <%--<link rel="icon"            type="image/ico"    href="/Design/img/favicon.ico" runat="server" />--%>
    <%--<link rel="shortcut icon" href="/Design/img/favicon.html">--%>

    <!-- Css Style for Add User Screen -->
    <%--<link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.19/themes/cupertino/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" rel="Stylesheet" />--%>
    <link href="/Design/css/jquery-ui.css" rel="stylesheet" />

    <link href="/Design/css/custom.css?v=1.7" rel="stylesheet" />

    <%--These local files have been used due to internet problem--%>
    <%-- Original files have been restored 01-06-2016 --%>
    <%--START--%>
    <%--<script src="/Design/assets/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Design/assets/jquery-ui/jquery-ui.min.js"></script>--%>
    <script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/chosen/1.1.0/chosen.jquery.min.js"></script>
    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/chosen/1.1.0/chosen.min.css">
    <script src="/Scripts/jquery.mcautocomplete.js"></script>






    <script>window.jQuery || document.write('<script src="/Design/assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="/Design/assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="/Design/assets/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <script src="/Design/assets/jquery-cookie/jquery.cookie.js"></script>

    <!--page specific plugin scripts-->
    <script type="text/javascript" src="/Design/assets/bootstrap-datepicker/js/bootstrap-datepicker.js?1"></script>
    <script type="text/javascript" src="/Design/assets/bootstrap-timepicker/js/bootstrap-timepicker.min.js"></script>

    <script src="/Scripts/IrrigationNetwork/InputValidations.js"></script>
    <!--flaty scripts-->
    <script src="/Design/js/flaty.js"></script>
    <script src="/Design/js/custom.js"></script>
    <script src="/Scripts/jquery.numeric.js"></script>
    <script type="text/javascript">
        function printDiv(divName) {
            if (document.getElementById(divName).innerHTML.trim() != '') {
                var printContents = document.getElementById(divName).innerHTML;
                var originalContents = document.body.innerHTML;

                document.body.innerHTML = printContents;

                window.print();

                document.body.innerHTML = originalContents;
            }
        }
    </script>
</head>
<body style="background: #ffffff;">
    <div class="container-fluid">
        <form id="form1" class="form-horizontal" runat="server">
            <div class="row">
                <div class="col-md-3 col-md-offset-3">
                    <div class="form-group">
                        <label class="col-sm-4 col-lg-3 control-label">Date</label>
                        <div class="col-sm-8 col-lg-9 controls">
                            <div class="input-group date" data-date-viewmode="years">
                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker required" Width="200px" required="required" size="16" type="text"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-md-offset-1">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                            <button id="btnPrint" class="btn btn-primary" onclick="printDiv('PrintArea')">Print</button>
                        </div>
                    </div>
                </div>
            </div>
            <br>
            <br>
            <div id="PrintArea">
                <div class="template" runat="server" visible="false" id="DiagramSec">
                    <img src="../../Images/bg.jpg" />
                    <asp:Label runat="server" class="big" ID="dated">Dated</asp:Label>
                    <asp:Label runat="server" class="big" ID="skardu">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="bunji">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="bisham">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="tarbela">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="tarbela_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="tarbela_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="kabul">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="kalabagh_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="kalabagh_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="chashma_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="chashma_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="cbrc_head">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="cbrc_pb_rd">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="big" ID="taunsa_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="taunsa_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="kachhi">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="dgkhan">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="med" ID="guddu_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="guddu_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="sukkur_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="sukkur_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="kotri_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="kotri_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="thal">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="cjlink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="greater_thal">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="tplink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="muzaffargarh">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="big" ID="mangla">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="mangla_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="mangla_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="rasul_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="rasul_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="trimmu_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="trimmu_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="rangpur">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="rtp">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="big" ID="punjnad_top">NIL</asp:Label>
                    <asp:Label runat="server" class="big" ID="punjnad_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="punjnad">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="abbasia">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="abbasia_link">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="ujc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="rpc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="ujc_int">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="rqlink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lower_jehlum">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="tslink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="havali">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="havali_int">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="med" ID="marala_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="marala_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="khanki_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="khanki_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="qadirabad_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="qadirabad_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="sidhnal_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="sidhnal_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="sidhnal">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="pakpattan">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lmailsi">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="smlink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="rd161">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lalsohara">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="smblink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="mrlink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="mrint">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="brbdlink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="ucc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="mll">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="uccint">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lcc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="qblink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lcc_total">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="lcc_feeder">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="med" ID="balloki_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="balloki_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="lbdc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="mplink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="pilink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="qaim">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="ubc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="med" ID="islam_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="islam_bot">NIL</asp:Label>
                    <asp:Label runat="server" class="small" ID="cbdc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="brbd_int">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="udc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="bslink">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="bslink_2">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="bslink_1">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="ldc">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="upakpattan">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="fardwah">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="small" ID="sadiqia">NIL (NIL)(NIL)</asp:Label>
                    <asp:Label runat="server" class="med" ID="sulemanki_top">NIL</asp:Label>
                    <asp:Label runat="server" class="med" ID="sulemanki_bot">NIL</asp:Label>
                </div>
            </div>
        </form>
    </div>
</body>
</html>



