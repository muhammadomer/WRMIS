<%@ Page Title="Outlet Performance Data" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddOutletPerformanceData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.AddOutletPerformanceData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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

                    <div class="table-responsive">
                        <asp:Table ID="table1" runat="server" CssClass="table tbl-info">

                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblDesignParameters" runat="server" Text="Design Parameters"></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>


                            <asp:TableRow>
                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblHeadlbl" runat="server" Text="Head Above Crest of Outlet (H in ft)"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblMMHlbl" runat="server" Text="Minimum Modular Head (MMH in ft)"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell>
                                    <asp:Label ID="lblDesignDischargelbl" runat="server" Text="Design Discharge"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblHead" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblMMH" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="lblDesignDischarge" runat="server"></asp:Label>
                                </asp:TableCell>

                            </asp:TableRow>

                        </asp:Table>
                    </div>

                    <p id="lblObserved" runat="server" style="font-weight: bold;">Observed Parameters</p>
                    <div class="form-horizontal" id="divFields" runat="server">
                        <div class="row" id="fields" runat="server">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-sm-4 col-lg-3 control-label">Head Above Crest of Outlet (H in ft) </asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtHeadAbove" runat="server" CssClass="decimalInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="select" class="col-sm-4 col-lg-3 control-label">Working Head (wh in ft)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkingHead" runat="server" CssClass="decimalInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row" id="mfields" runat="server">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="select" class="col-sm-4 col-lg-3 control-label">Discharge (Cusec)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDischarge" runat="server" required="true" CssClass="decimalInput form-control required" AutoPostBack="true" OnTextChanged="txtDischarge_TextChanged"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="select" class="col-sm-4 col-lg-3 control-label">Efficiency (Observed Discharge/Design Discharge x 100)%</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtEfficiency" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <%--<input type="button" value="Back" class="btn" onclick="javascript: history.go(-1); return false;" />--%>
                                    <asp:LinkButton ID="lbtnBack" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='CriteriaForSpecificOutlet.aspx?History=true'; return false;"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
