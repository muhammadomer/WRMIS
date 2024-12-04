<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddIndent.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.DailyData.AddIndent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Placing Indents</h3>

                </div>
                <div class="box-content">

                    <div class="table-responsive">
                        <table class="table tbl-info">

                            <tr>
                                <th>Channel Name</th>
                                <th>Channel Type</th>
                                <th>Total RDs(ft)</th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblChannelName" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblChannelType" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblTotalRD" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                            <tr>
                                <th>Flow Type</th>
                                <th>Command Name</th>
                                <th>Division</th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblFlowType" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblCommandName" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblDivision" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                            <tr>
                                <th>Sub Division</th>
                                <th>IMIS CODE</th>
                                <th></th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblSubDivision" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblIMISCode" runat="server" Text="" /></td>
                                <td></td>
                                <td></td>
                            </tr>


                            <tr>
                                <th>Current Indent (cusec)</th>
                                <th>Lower SubDivision Indent (cusec)</th>
                                <th>Lower SubDivision Indent Date</th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrentIndent" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblLowerSubDivisionIndent" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblLowerSubDivisionIndentDate" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                        </table>
                    </div>
                    <br />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblPlacementDate" runat="server" CssClass="col-sm-4 col-lg-3 control-label">Placement Date</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtPlacementDate" ClientIDMode="Static" runat="server" required="required" CssClass="date-picker form-control required" TabIndex="1"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblRemarks" runat="server" CssClass="col-sm-4 col-lg-3 control-label">Remarks</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" CssClass="form-control" type="text" MaxLength="100" TabIndex="3"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblIndent" runat="server" CssClass="col-sm-4 col-lg-3 control-label">Indent</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtIndent" ClientIDMode="Static" runat="server" required="required" CssClass="form-control decimalInput required" oninput="IndentValueValidation(this)" TabIndex="2"></asp:TextBox>
                                    </div>

                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" align="center" Text="Save" OnClick="btnSave_Click" TabIndex="4"></asp:Button>
                                    <asp:LinkButton ID="lnkBtnBack" Text="Back" CssClass="btn" OnClick="lnkBtnBack_Click" runat="server" TabIndex="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
</asp:Content>
