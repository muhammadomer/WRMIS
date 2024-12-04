<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchChannel.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.SearchChannel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Channels</title>
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

    <style>
        hr {
            border-top: 1px solid #f1f1f1;
            border-bottom: 1px solid #f1f1f1;
        }

        #tblComplaintInformation td.col-md-2 {
            font-size: 13px;
            font-weight: bold;
        }

        #tblComplaintInformation td span.bold {
            font-size: 13px;
            font-weight: bold;
        }

        .seprator {
            padding: 5px;
            border-bottom: 1px solid #F3F3F3;
            font-size: 11px;
        }
    </style>
</head>
<body style="background: #ffffff;">
    <div class="container-fluid">
        <div class="row">
            <form id="form1" class="form-horizontal" runat="server">
                <div class="row">
                    <div class="col-md-5">
                        <strong class="col-sm-4 col-lg-4 control-label">Division</strong>
                        <div class="col-sm-8 col-lg-8 controls">
                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <strong class="col-sm-4 col-lg-4 control-label">Channel Name</strong>
                        <div class="col-sm-8 col-lg-8 controls">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtChannelName" MaxLength="150"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="col-sm-12 col-lg-12 controls">
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnChannelSearch_Click" CssClass="btn btn-primary" Text="&nbsp;Search" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />


                    <%--  <img src="~/Design/img/Legand.png" runat="server" id="image" />--%>


                    <%--                    <div style="height:70px; width:10px; margin-left:20px; background-color:#35ca38; float:left; border:1px solid #ebebeb;" ></div>
                    <div style="height:70px; width:23%; margin-left:1px; background-color:#ebebeb; float:left; border-radius: 9px; border:1px solid #35ca38;" >
                        <div style="margin:auto; width:64%;margin-top: 6px; padding-top:2px;text-align:center;border-radius: 9px; border:1px solid #35ca38; height:22px;">
                           <label style="font-stretch:extra-expanded; font-weight:bold;margin:auto;">Tail as pre Authorized Tail Discharge</label>
                        </div>
                        <div style="width:100%; margin:2px; height:30px;">
                           <p style="font-size:10px;">Head Discharge is more than or equal to 95% of either Design Discharge or Indent. 
                                        Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge</p>
                        </div>
                    </div>--%>




                    <div class="container" id="Banners" runat="server">
                        <%--<div class="col-md-12">
                            <div class="col-md-3">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color: #35ca38">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;">Tail as pre Authorized Tail Discharge</label>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                        Head Discharge is more than or equal to 95% of either Design Discharge or Indent. 
                                        Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#741605">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;color:white;">Tails are Dry</label>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                            Head discharge is more than or equal to 95% of 
                                            either Design Discharge or Indent and Thail Gauge 
                                            is less than or equal to 30% of Authorized Tail Gauge.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#feb7ad">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;">Closed Channel</label>
                                    </div>
                                    <div class="panel-body" style="height: 93px;">
                                        <p>
                                        Head Discharge is Zero.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#9ecded">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;">Excessive Supply at Tails</label>
                                    </div>
                                    <div class="panel-body" style="height: 93px;">
                                        <p>
                                            Head Discharge is more than or equal to 95% of either Design Discharge or Indent and
                                        Tail Gauge is more than or equal to 115% of Authorized Tail Gauge
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-3" style="width: 437px;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#fcff05">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;">Short Supply at Tails.</label>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                            Head Discharge is more than or equal to 95% of either Design Discharge or Indent and 
                                        Tail Gauge is more than or equal to 30% and less than or equal to 90% of Authorized Tail Gauge
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#808080">
                                        <label style="font-stretch: extra-expanded; font-weight: bold; color:white;">Data not receieved</label>
                                    </div>
                                    <div class="panel-body" style="height: 76px;">
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" style="width: 550px;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#7d017a">
                                        <label style="font-stretch: extra-expanded; font-weight: bold; color:white">Channel is running less than designed discharge at head and tails are short</label>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                            Head Discharge is more than or equal to 95% of either Design Discharge or Indent. 
                                        Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" style="width:729px;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="background-color:#99f387">
                                        <label style="font-stretch: extra-expanded; font-weight: bold;">Channel is running less than designed discharge at head and tails are as per Authorized Discharge</label>
                                    </div>
                                    <div class="panel-body" style="height:94px;">
                                        <p>
                                            Head Discharge is more than or equal to 95% of either Design Discharge and Indent. 
                                        Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                        <div class="container" style="min-width:70%">
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #32CD32;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Tails  are as per Authorized Tail Discharge</b>
                                    <br />
                                    <span>Head Discharge is more than or equal to 95% of either Design Discharge or Indent and Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge</span>
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #90EE90;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Channel is running less than designed discharge at head and tails are as per Authorized Discharge</b>
                                    <br />
                                    Head Discharge is less than 95% of Design Discharge and Indent Tail Gauge is more than or equal to 90% and less than or equal to 115% of Authorized Tail Gauge
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #800000;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Tails are dry</b>
                                    <br />
                                    Head Discharge is more than or equal to 95% of either Design Discharge or Indent and Tail Gauge is less than or equal to 30% of Authorized Tail Gauge
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #FFC0CB;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Closed Channel</b>
                                    <br />
                                    Head Discharge is zero
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #87CEEB;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Excessive supply at Tails.</b>
                                    <br />
                                    Head Discharge is more than or equal to 95% of either Design Discharge or Indent and Tail Gauge is more than 115% of Authorized Tail Gauge
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #FFFF00;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Short supply at Tails</b>
                                    <br />
                                    Head Discharge is more than or equal to 95% of either Design Discharge or Indent and Tail Gauge is more than 30% and less than 90% of Authorized Tail Gauge
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #708090;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Data not recieved</b>
                                </div>
                            </div>
                            <div class="row seprator">
                                <div class="col-sm-1" style="background: #800080;">&nbsp;</div>
                                <div class="col-sm-11">
                                    <b>Channel is running less than designed discharge at head and tails are short</b>
                                    <br />
                                    Head Discharge is less than 95% of Design Discharge and Indent and Tail Gauge is greater than 30% and less than 90% of Authorized Tail Gauge
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div runat="server" id="divNoResultFound" class="has-error" style="min-height: 20px; vertical-align: middle; margin: 28px auto; position: absolute; font-size: 14px; color: #ff0000;" visible="false">No Result Found</div>
                    </div>
                    <div class="col-md-12">
                        <hr style="border-top: 1px solid #989797;" />
                    </div>
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChannel" runat="server" DataKeyNames="ChannelID,ChannelColor" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                OnRowDataBound="gvChannel_RowDataBound" OnPageIndexChanging="gvChannel_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelID" runat="server" Text='<%# Eval("ChannelID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelName" runat="server" Text='<%#Eval("ChannelName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IMIS Code">
                                        <ItemTemplate>
                                            <asp:Label ID="IMISCode" runat="server" Text='<%#Eval("IMISCode") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Type">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelType" runat="server" Text='<%# Eval("ChannelType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="18%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Channel Color">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelColor" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>


                                    <%--<asp:TemplateField HeaderText="Sub Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("SubDivision") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="12.4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSection" runat="server" Text='<%# Eval("Section") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="12.4%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Total RD">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="TotalRDs" runat="server" Text='<%# string.Format("{0:n0}", Eval("TotalRDs")) %>'></asp:Label>--%>
                                            <asp:Label ID="TotalRDs" runat="server" Text='<%# PMIU.WRMIS.AppBlocks.Calculations.GetRDText(Convert.ToInt64(Eval("TotalRDs"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Comments" runat="server" Text='<%# Eval("Comments") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="24.8%" />
                                    </asp:TemplateField>--%>
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



</body>
</html>
