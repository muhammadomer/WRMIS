<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchComplaint.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.SearchComplaint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search and View Complaints</title>
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
        body {
            padding: 0px;
            margin: 0px;
        }

        hr {
            border-top: 1px solid #f1f1f1;
            border-bottom: 1px solid #f1f1f1;
        }

        #tblComplaintInformation td.col-md-2 {
            font-size: 13px;
            font-weight: bold;
            text-align: left;
        }

        #tblComplaintInformation td.col-md-4 span {
            text-align: left;
        }

        #tblComplaintInformation td.col-md-10 span {
            text-align: left;
        }

        #tblComplaintInformation td span.bold {
            font-size: 13px;
            font-weight: bold;
        }

        div.bold {
            font-weight: bold;
            line-height: 1.42857143;
            padding-top: 8px;
        }

        #divComplaintInformation {
            background-color: #ffffff;
            padding-bottom: 15px;
        }

        /*input[type=text] {
            width: 150px;
        }

        #btnSearch {
            height: 26px;
            margin-left: 17px;
            padding: 0px;
        }*/
    </style>
    <script type="text/javascript">

        $(document).ready(function () {                        

            setRequiredFields();

            $('#txtComplaintNumber').change(function () {
                setRequiredFields();
            });

            $('#txtComplainCellNumber').change(function () {
                setRequiredFields()
            });

            function setRequiredFields()
            {
                if (($('#txtComplaintNumber').val().trim() != '' && $('#txtComplainCellNumber').val().trim() != '') ||
                ($('#txtComplaintNumber').val().trim() == '' && $('#txtComplainCellNumber').val().trim() == '')) {
                    $('#txtComplaintNumber').attr('required', true);
                    $('#txtComplainCellNumber').attr('required', true);
                }
                else if ($('#txtComplaintNumber').val().trim() != '' && $('#txtComplainCellNumber').val().trim() == '') {
                    $('#txtComplainCellNumber').removeAttr('required');
                }
                else if ($('#txtComplaintNumber').val().trim() == '' && $('#txtComplainCellNumber').val().trim() != '') {
                    $('#txtComplaintNumber').removeAttr('required');
                }
            }

        });

    </script>
</head>
<body style="background: #ffffff;">
    <div class="container-fluid">
        <div class="row">
            <form id="form1" class="form-horizontal" runat="server">
                <div class="col-md-12 col-sm-12">

                    <div class="col-md-12 col-sm-12">
                        <div class="col-md-2 col-sm-2">
                            <strong>Complaint ID:</strong>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <asp:TextBox runat="server" CssClass="col-md-12 form-control" ID="txtComplaintNumber" MaxLength="12" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-sm-2">
                            <strong>Complainant Cell No.</strong>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <asp:TextBox runat="server" CssClass="col-md-12 form-control" ID="txtComplainCellNumber" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" CssClass="btn btn-primary" Text="&nbsp;Search" />
                        </div>
                    </div>
                    <div class="col-md-12 col-sm-12">
                        <div runat="server" id="divNoResultFound" class="has-error" style="min-height: 20px; vertical-align: middle; margin: 28px auto; position: absolute; font-size: 14px; color: #ff0000;" visible="false">No Result Found</div>
                    </div>
                    <div class="col-md-12 col-sm-12">
                        <hr style="border-top: 1px solid #989797;" />
                    </div>

                    <div class="col-md-12 col-sm-12">
                        <div class="col-md-12 col-sm-12" id="divComplaintInformation" runat="server" style="padding: 0px;">

                            <div class="box">
                                <h3 style="color: #575555; margin-top: 0px; margin-bottom: 5px; font-weight: bold;font-size:25px;">Complaint Information</h3>
                            </div>
                            <div class="box-content">


                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Complaint ID</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplaintNumber" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Status</div>
                                    <div class="col-md-12 col-sm-12"><span id="spnStatus" runat="server" style="font-weight: bold;"></span></div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-12 col-sm-12 bold">Complainant Name</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplainerName" runat="server" Text="" /><br />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Phone Number</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblPhone" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Cell Number</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblCell" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-12 col-sm-12 bold">Address</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblAddress" runat="server" Text="" /><br />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Complaint Date</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplaintDate" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Complaint Type</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplaintType" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Complaint Domain</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplaintDomain" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 bold">Structure/Division</div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblStructureDivision" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12 bold">
                                    <div class="col-md-12 col-sm-12">Structure Name</div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblStructureName" runat="server" Text="" />
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12 bold">
                                    <div class="col-md-12 col-sm-12">Complaint Details</div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Label ID="lblComplaintDetails" runat="server" Text="" />
                                    </div>
                                </div>
                                <%--<table class="table header" id="tblComplaintInformation" runat="server" style="border-collapse: collapse; border-spacing: 0;">
                                <tbody>
                                    <tr>
                                        <td class="col-md-2">Complaint ID:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblComplaintNumber" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                        <td class="col-md-2">Status:</td>
                                        <td class="col-md-4">
                                            <span id="spnStatus" runat="server" style="margin-left: 10px; font-weight: bold;"></span></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Complainant Name:</td>
                                        <td class="col-md-10" colspan="10">
                                            <asp:Label ID="lblComplainerName" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Address:</td>
                                        <td class="col-md-10" colspan="3">
                                            <asp:Label ID="lblAddress" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Phone:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblPhone" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                        <td class="col-md-2">Cell:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblCell" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Complaint Date:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblComplaintDate" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                        <td class="col-md-2">Complaint Type:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblComplaintType" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Complaint Domain:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblComplaintDomain" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                        <td class="col-md-2">Structure/Division:</td>
                                        <td class="col-md-4">
                                            <asp:Label ID="lblStructureDivision" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                    <tr>
                                        <td class="col-md-2">Structure Name:</td>
                                        <td class="col-md-10" colspan="3">
                                            <asp:Label ID="lblStructureName" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>

                                    <tr>
                                        <td class="col-md-2">Complaint Details:</td>
                                        <td class="col-md-10" colspan="3">
                                            <asp:Label ID="lblComplaintDetails" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                    </tr>
                                </tbody>
                            </table>--%>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        </form>
    </div>
    </div>
</body>
</html>
