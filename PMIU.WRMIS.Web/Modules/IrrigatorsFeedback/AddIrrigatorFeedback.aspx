<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddIrrigatorFeedback.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.AddIrrigatorFeedback" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Literal runat="server" ID="ltlPageTitle">Add Irrigator Feedback</asp:Literal>
                    </h3>

                </div>
                <div class="box-content">

                    <div class="table-responsive">
                        <table class="table tbl-info">

                            <tr>

                                <th width="33.3%">Division</th>
                                <th width="33.3%">Channel Name</th>
                                <th width="33.3%">Name</th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblDivision" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblChannel" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblIrrigatorName" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                            <tr>
                                <th>Mobile No. 1</th>
                                <th>Mobile No. 2</th>
                                <th>Status</th>

                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblIrrigatorMobileNo1" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblIrrigatorMobileNo2" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblIrrigatorStatus" runat="server" Text="" /></td>
                                <td></td>
                            </tr>


                            <%--style="color:#428bca"--%>
                            <tr>
                                <th width="33.3%">
                                    <h3>Daily Data Information</h3>
                                </th>
                                <th>
                                    <%--  <a id="viewlasttendays" href="javascript:$('#GraphPopUp').modal('show');">Tail Status (last 10 days)</a>--%>
                                    <asp:LinkButton ID="viewlasttendays" Text="Tail Status (last 10 days)" runat="server" OnClick="viewlasttendays_Click"></asp:LinkButton>
                                    <%--  <a id="ViewLastTenDays" href="ViewGraph.aspx" target="_blank">Tail Status (Last 10 Days)</a>--%>
                                </th>
                                <th>
                                    <asp:LinkButton ID="ViewRotationalProgram" Text="View Rotational Program" runat="server" OnClick="ViewRotationalProgram_Click"></asp:LinkButton>
                                </th>
                            </tr>


                            <%--<tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>--%>


                            <tr>
                                <th width="33.3%">Tail Status</th>
                                <th width="33.3%">Head Percent</th>
                                <th width="33.3%">Tail Status Time</th>

                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblTailStatus" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblHeadPercent" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblTailStatusDateAndTime" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                            <%--<tr>
                                        <th>Tail Status Date & Time</th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTailStatusTime" runat="server" Text="" /></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>--%>
                        </table>
                    </div>
                    <br />


                    <div class="form-horizontal">
                        <h3 style="margin-left: 10px;">Feedback</h3>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblCallDateHeading" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-top: 0px;">Call Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:Label ID="lblCallDate" runat="server" CssClass="control-label"></asp:Label>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblTailStatusByIrrigator" runat="server" Text="Tail Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlTailStatusByIrrigator" runat="server" CssClass="form-control required" required="required" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblRotationalViolation" runat="server" Text="Rotational Violation" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlRotationalViolation" runat="server" CssClass="form-control required" required="required" TabIndex="2">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblWaterTheft" runat="server" Text="Water Theft" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlWaterTheft" runat="server" ClientIDMode="Static" CssClass="form-control required" onchange="dropdownValidation()" required="required" TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtTotalRDLeft" ID="lblTotalRDs" Text="RD" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtTotalRDLeft" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="form-control integerInput" MaxLength="3" TabIndex="4"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        +
                                    </div>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtTotalRDRight" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="integerInput form-control" MaxLength="3" TabIndex="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblLocal" runat="server" Text="Local Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtLocal" runat="server" CssClass="form-control" MaxLength="20" ClientIDMode="Static" onkeyup="AlphabetsWithLengthValidation(this, 3)" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblVillage" runat="server" Text="Village Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlVillage" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="7">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control multiline-no-resize" TextMode="MultiLine" MaxLength="250" Rows="5" Columns="50" TabIndex="8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" align="center" OnClick="btnSave_Click" Text="Save" OnClientClick="return MessageValidation();" TabIndex="9"></asp:Button>
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" TabIndex="10" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDivisionID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnChannelID" runat="server" ClientIDMode="Static" />
    </div>

    <style>
        .chart {
            align-content: center;
            display: flex;
            justify-content: center;
        }

        .modal {
            text-align: center;
        }

        @media screen and (min-width: 768px) {
            .modal:before {
                content: " ";
                display: inline-block;
                height: 100%;
                vertical-align: middle;
            }
        }

        .modal-dialog {
            display: inline-block;
            text-align: center;
            vertical-align: middle;
        }

        .modal-footer {
            color: #00b5e6;
            font-size: 25px;
            text-align: center;
        }
    </style>

    <%--<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div runat="server" id="dvLtScripts"></div>
                    <div runat="server" class="chart" id="chart_div"></div>
                </div>
                <div class="modal-footer">
                    <div id='png'></div>
                </div>
            </div>
        </div>
    </div>--%>

    <div id="GraphPopUp" class="modal fade">
        <div class="modal-dialog" style="width: 1050px; height: 650px;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row text-center">
                       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>--%>
                                <div class="row text-center">
                                    <h3>Last 10 days Status</h3>
                                    <br />
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <%--<asp:Label ID="lblpopTailStatus" runat="server" Text="Tail Status" CssClass="col-sm-4 col-lg-3 control-label" />--%>
                                            <%--<div class="col-sm-8 col-lg-7 controls">
                                                <asp:DropDownList ID="ddlpopTailStatus" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlpopTailStatus_SelectedIndexChanged">
                                                    <asp:ListItem Value="1" Text="Morning"> </asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Evening"> </asp:ListItem>
                                                </asp:DropDownList>
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div runat="server" id="dvLtScripts"></div>
                                    <div id="chart_div" runat="server" style="width: 900px; height: 500px; margin: auto;">
                                    </div>
                                </div>
                                <div id="scrollDiv">&nbsp;</div>
                          <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script>
        function MessageValidation() {
            $("form:first").attr('onsubmit', "return confirm('Edit is not permitted. Are you sure you want to save the record?');");
        }

        function dropdownValidation() {
            if ($('#ddlWaterTheft option:selected').text() == "Yes") {
                $('#txtTotalRDLeft').prop('disabled', false);
                $('#txtTotalRDRight').prop('disabled', false);
                $('#txtLocal').prop('disabled', false);
                $('#ddlVillage').prop('disabled', false);
            }
            else {
                $('#txtTotalRDLeft').prop('disabled', true);
                $('#txtTotalRDRight').prop('disabled', true);
                $('#txtLocal').prop('disabled', true);
                $('#ddlVillage').prop('disabled', true);
                $("select#ddlVillage").prop('selectedIndex', 0);
                $('#txtTotalRDLeft').val('');
                $('#txtTotalRDRight').val('');
                $('#txtLocal').val('');
            }
        }
    </script>


    <%--<script src="../../Scripts/Complaints/jsapi.js"></script>
    <script type="text/javascript" src="../../Design/js/loader-graph.js"></script>--%>
    <script src="../../Scripts/Complaints/loader.js"></script>
    <script src="../../Scripts/Complaints/jsapi.js"></script>
    <%-- <script type="text/javascript">
        google.charts.load('current', { 'packages': ['bar'] });
        //google.load("visualization", "1", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawVisualization);

    </script>--%>

    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart'] });
        google.setOnLoadCallback(drawVisualization);
    </script>

    <script type="text/javascript">
        $('#GraphPopUp').on('shown.bs.modal', function (event) {
            drawVisualization();
        });

        $(window).resize(function () {
            drawVisualization();
        });

        function openModal() {
            $("#GraphPopUp").modal("show");
            drawVisualization();
        };
        function closeModal() {
            $("#GraphPopUp").modal("hide");
            drawVisualization();
        };
    </script>


</asp:Content>

