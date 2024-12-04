<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PMIU.WRMIS.Web._Default" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
    <%--<%PMIU.WRMIS.Logging.LogMessage.LogMessageNow(2, "Default Page ASPX - Load - IIS Session: " + Environment.MachineName.ToString());%>--%>

    <%--<link rel="stylesheet" href="~/Design/css/phc.css" />--%>
    <!-- BEGIN Container -->
    <style>
        .dashboard div {
            -webkit-transition: all .5s ease-in-out;
            -moz-transition: all .5s ease-in-out;
            -o-transition: all .5s ease-in-out;
            -ms-transition: all .5s ease-in-out;
        }

        .dashboard:hover div {
            -webkit-transform: rotate(360deg);
            -moz-transform: rotate(360deg);
            -o-transform: rotate(360deg);
            -ms-transform: rotate(360deg);
        }
    </style>
    <div class="container" id="main-container">
        <!-- BEGIN Content -->
        <div id="main-content">
            <!-- BEGIN Page Title -->
            <%--<div class="page-title">
                <div>
                    <h1>WRMIS Dashboard</h1>
                </div>
            </div>--%>
            <!-- END Page Title -->
            <!-- BEGIN Tiles -->
            <div class="row">
                <asp:Repeater ID="rpDashboard" runat="server">
                    <ItemTemplate>
                        <div class="col-md-2 dashboard" id='<%# "Block_" + Eval ("Icon")%>' <%--onmouseover="RotateTo('<%# "Block_" + Eval ("Icon")%>')" onmouseout="RotateBack('<%# "Block_" + Eval ("Icon")%>')"--%>>
                            <a class="tile tile-info"  style="color:white;" href="#<%# Eval ("Icon")%>" onclick="clickmenu('<%# Eval ("Icon")%>');">
                                <div class="img img-center">
                                    <img src='<%# "/Design/img/dashboard/" + Eval ("Icon") + ".png"%>' />
                                </div>
                                <p class="title text-center"><%# Eval ("Name")%></p>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <!-- END Tiles -->
            <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"><i class="fa fa-chevron-up"></i></a>
        </div>
        <!-- END Content -->
    </div>
    <!-- END Container -->

    <script type="text/javascript">
        function clickmenu(mnu) {
            $('#' + mnu).click();
        }

        //function RotateTo(mnu) {
        //    //alert('on');
        //    //$('#' + mnu).rotate({ animateTo: -25 });
        //}

        //function RotateBack(mnu) {
        //    //alert('out');
        //    //$('#' + mnu).rotate({ animateTo: 0 });
        //}

        //$(function(){
        //    $(".dashboard").flip({
        //        trigger: 'hover',
        //        axis: 'x'
        //    });

        //$('.dashboard').rotate({
        //        bind:
        //        {
        //            mouseover: function () {
        //                alert('on');
        //                $(this).rotate({ animateTo: -25 })

        //            },
        //            mouseout: function () {
        //                alert('out');
        //                $(this).rotate({ animateTo: 0 })
        //            }
        //        }
        //    });

    </script>
</asp:Content>
