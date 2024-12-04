﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnticipatedIndusRiver.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.AnticipatedIndusRiver" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3 id="htitle" runat="server">Seasonal Planning</h3>
        </div>
        <div class="box-content">

            <div id="divPanel" class="panel panel-default" runat="server">
                <div id="Tabs" role="tabpanel">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li id="liVillage" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/SeasonalPlanning.aspx" id="anchBalance" onserverclick="anchBalance_ServerClick" runat="server" aria-controls="Village" role="tab">Balance Reservoir</a></li>
                        <li id="liDivision" style="width: 16%; text-align: center" runat="server"><a href="~/Modules/SeasonalPlanning/ManglaIndusOperations.aspx" id="anchDivision" runat="server" aria-controls="Division" role="tab">Mangla Operations</a></li>
                        <li id="li1" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/IndusOperations.aspx" id="a1" runat="server" aria-controls="Village" role="tab">Indus Operations</a></li>
                        <li id="li2" runat="server" style="width: 16%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedJC.aspx" id="a2" runat="server" aria-controls="Division" role="tab">Anticipated JC</a></li>
                        <li id="li3" runat="server" style="width: 18%; text-align: center" class="active"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusRiver.aspx" id="a3" runat="server" aria-controls="Village" role="tab">Anticipated Indus River</a></li>
                        <li id="li4" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusBasin.aspx" id="a4" runat="server" aria-controls="Division" role="tab">Anticipated Indus Basin</a></li>
                    </ul>
                </div>
            </div>

            <div class="form-horizontal">
                <div class="row">
                    <div id="divKharif" runat="server" style="display: none">
                        <div class="col-md-12">
                            <h4 class="text-center"><b>Anticipated water availability and operation of Indus River System</b></h4>
                            <br />

                            <div class="row">
                                <div class="col-md-offset-3 col-md-3">
                                </div>
                                <div class="col-md-3">
                                </div>
                                <div class="col-md-3">
                                </div>
                            </div>



                            <div class="row">
                                <div class="col-md-offset-3 col-md-3">
                                    <asp:Label Text="Early Kharif" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label Text="Late Kharif" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label Text="Total kharif" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="thRimStationInflows" Text="Rim Station Inflows" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblIndusAtTarbelaInflows" runat="server" Text="Indus At Tarbela Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaInflowsEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaInflowsLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaInflowsTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblIndusAtTarbelaProbability" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaProbabilityEk" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaProbabilityLK" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblIndusAtTarbelaProbabilityTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblKabulAtNowsheraInflows" runat="server" Text="Kabul At Nowshera Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraInflowsEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraInflowsLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraInflowsTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblKabulAtNowsheraProbability" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraProbabilityEK" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraProbabilityLK" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKabulAtNowsheraProbabilityTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblSubTotalRimStation" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblSubTotalRimStationEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotalRimStationLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotalRimStationTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblStorageRelease" runat="server" Text="Storage/Release " Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblTarbela" runat="server" Text="Tarbela" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblTarbelaEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTarbelaLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTarbelaTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblChashma" runat="server" Text="Chashma" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblChashmaEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChashmaLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChashmaTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblSubTotalStorageRelease" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblSubTotalStorageReleaseEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotalStorageReleaseLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotalStorageReleaseTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblJCOutflow" runat="server" Text="J-C Outflow" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblJCOutflowITEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJCOutflowITLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJCOutflowITTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblTotalSystemInflows" runat="server" Text="Total System Inflows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblSystemLossesGains" runat="server" Text="System Losses / Gains" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblSystemLossesGainsEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSystemLossesGainsLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSystemLossesGainsTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblTotaAvailability" runat="server" Text="Total Availabilitys" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblTotaAvailabilityEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotaAvailabilityLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotaAvailabilityTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblKotriBelow" runat="server" Text="Kotri Below" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblKotriBelowEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKotriBelowLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblKotriBelowEKTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblCanalAvailability" runat="server" Text="Canal Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblCanalAvailabilityEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblCanalAvailabilityLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblCanalAvailabilityTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblLessKPKplusBalochistan" runat="server" Text="Less KPK + Balochistan" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblLessKPKplusBalochistanEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblLessKPKplusBalochistanLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblLessKPKplusBalochistanTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblBalanceforPunjabAndSindh" runat="server" Text="Balance for Punjab & Sindh" Font-Bold="true" Style="float: left;"></asp:Label>                                    
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblBalanceforPunjabAndSindhEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblBalanceforPunjabAndSindhLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblBalanceforPunjabAndSindhTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblShareofPunjabAndSindh" runat="server" Text="Share of Punjab & Sindh" Font-Bold="true"></asp:Label>
                                    <asp:CheckBox ID="cbPara" runat="server" OnCheckedChanged="cbPara_CheckedChanged" Font-Bold="true" Text="Para2" AutoPostBack="true" Style="margin-left: 5px;" />
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblShareofPunjabAndSindhEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShareofPunjabAndSindhLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShareofPunjabAndSindhTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblShortage" runat="server" Text="Shortage (%)" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblShortageEK" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShortageLK" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShortageTotal" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divRabi" runat="server" style="display: none">
                        <div class="col-md-12">
                            <h4 class="text-center"><b>Anticipated water availability and operation of Indus River System</b></h4>
                            <br />
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblRimStation" Text="Rim Station Inflows" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblMAF" Text="Rabi" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="IndusAtTarbelaInflowsText" runat="server" Text="Indus At Tarbela Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblIndusAtTarbelaInflowsRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblIndusAtTarbelaProbabilityText" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblIndusAtTarbelaProbabilityRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblKabulAtNowsheraInflowsText" runat="server" Text="Kabul At Nowshera Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblKabulAtNowsheraInflowsRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblKabulAtNowsheraProbabilityText" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblKabulAtNowsheraProbabilityRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblSubTotalIndusAndKabulText" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblSubTotalIndusAndKabulRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblStorageReleaseText" runat="server" Text="Storage/Release" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblTarbelaText" runat="server" Text="Tarbela" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblTarbelaRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblChashmaText" runat="server" Text="Chashma" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblChashmatRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblSubTotalTerbelaAndChashmaText" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblSubTotalTerbelaAndChashmaRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblJCOutFlowsIKText" runat="server" Text="J-C Out Flows" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblJCOutFlowsIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblTotalSystemInflowsIKText" runat="server" Text="Total System Inflows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblTotalSystemInflowsIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblSystemLGIKText" runat="server" Text="System L (-) /G (+)" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblSystemLGIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="TotalAvailabilityIKText" runat="server" Text="Total Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="TotalAvailabilityIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblKotriBelowIKText" runat="server" Text="Kotri Below" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblKotriBelowIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblCanalAvailabilityIKText" runat="server" Text="Canal Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblCanalAvailabilityIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblLessKPKPlusBalochistanIKText" runat="server" Text="Less KPK + Balochistan" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblLessKPKPlusBalochistanIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblBalanceforPunjabandSindhIKText" runat="server" Text="Balance for Punjab and Sindh" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblBalanceforPunjabandSindhIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblShareofPunjabandSindhIKText" runat="server" Text="Share of Punjab and Sindh" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblShareofPunjabandSindhIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblShortageIKText" runat="server" Text="Shortage %" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblShortageIKRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-offset-1 col-md-3">
                            <br />
                            <asp:Label Text="<b> Note: All values are in MAF except as noted </b>" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <br />
                        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~\Modules\SeasonalPlanning\SeasonalPlanning.aspx" CssClass="btn btn-primary">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
