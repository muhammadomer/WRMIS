<%@ Page Title="Outlet Performance Data" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddOutletPerformanceData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddOutletPerformanceData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Outlet Performance Data</h3>
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:Table ID="tableInfo" runat="server" CssClass="table tbl-info">

                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblChnlNamelbl" runat="server" Text="Channel Name"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblChnlTypelbl" runat="server" Text="Channel Type"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblDesignationlbl" runat="server" Text="Outlets R.Ds. (ft)"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblChnlName" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblChnlType" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblOutletRD" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblOutletSidelbl" runat="server" Text="Outlet Side"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblPoliceStationlbl" runat="server" Text="Police station"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblVillagelbl" runat="server" Text="Village"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblOutletSide" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblPoliceStation" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblVillage" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblOutletTypelbl" runat="server" Text="Outlet Type"></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblOutletType" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>

                        </asp:Table>
                    </div>

                    <div class="form-horizontal" id="divDate" runat="server">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Date of Observation</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="1" runat="server" required="true" CssClass="form-control date-picker required"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-horizontal" id="divFields" runat="server">
                        <div class="row" id="fields" runat="server">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-sm-4 col-lg-3 control-label">Head Above Crest of Outlet (H in ft) </asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtHeadAbove" oninput="ValidateIntergerInputRange(this,'0','10')" required="required" runat="server" CssClass="decimal2PInput required form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="select" class="col-sm-4 col-lg-3 control-label">Working Head (wh in ft)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkingHead" oninput="ValidateIntergerInputRange(this,'0','10')" required="required" runat="server" CssClass="decimal2PInput required form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <div class="row" id="bfields" runat="server">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtOutletHeight" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Height of Outlet/Orifice (Y in ft)</asp:Label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtOutletHeight" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Height of Outlet/Orifice" required="required"  CssClass="decimal2PInput form-control required"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtDBWidth" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Diameter/Breadth/ Width (Dia/B in ft)</asp:Label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDBWidth" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Diameter/Breadth/ Width" CssClass="decimal2PInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <div class="row" id="mfields" runat="server">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="select" class="col-sm-4 col-lg-3 control-label">Discharge (Cusec)</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtDischarge" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" onblur="CalculateEfficiency();" required="true" CssClass="decimal2PInput form-control required"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="select" class="col-sm-4 col-lg-3 control-label">Efficiency (Observed Discharge/Design Discharge x 100)%</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtEfficiency" ReadOnly="true" runat="server" CssClass="right form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" Value="" ID="hdnDesignDischarge" />
    <script src="../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script type="text/javascript">
        txtDischarge = $('#<%= txtDischarge.ClientID %>');
        txtEfficiency = $('#<%= txtEfficiency.ClientID %>');
        hdnDesignDischarge = $('#<%= hdnDesignDischarge.ClientID %>');

        function CalculateEfficiency() {
            if (txtDischarge[0].checkValidity()) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("AddOutletPerformanceData.aspx/CalculateEfficiency") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_Discharge: "' + txtDischarge.val() + '",_DesignDischarge:"' + hdnDesignDischarge.val() + '"}',
                    success: function (data) {
                        if (data.d != null && data.d != "") {
                            txtEfficiency.val(data.d);
                        }
                        else {
                            console.log("testing");
                            $('#lblMsgs').addClass('ErrorMsg').show();
                            $('#lblMsgs').html("Cannot divide by Zero");
                            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
