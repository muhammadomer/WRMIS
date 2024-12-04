<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnticipatedJC.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.AnticipatedJC" %>

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
                        <li id="li2" runat="server" style="width: 16%; text-align: center" class="active"><a href="~/Modules/SeasonalPlanning/AnticipatedJC.aspx" id="a2" runat="server" aria-controls="Division" role="tab">Anticipated JC</a></li>
                        <li id="li3" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusRiver.aspx" id="a3" runat="server" aria-controls="Village" role="tab">Anticipated Indus River</a></li>
                        <li id="li4" runat="server" style="width: 18%; text-align: center"><a href="~/Modules/SeasonalPlanning/AnticipatedIndusBasin.aspx" id="a4" runat="server" aria-controls="Division" role="tab">Anticipated Indus Basin</a></li>
                    </ul>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="row">
                    <div id="divKharif" runat="server" style="display: none">
                        <div class="col-md-12">
                            <h4 class="text-center"><b>Anticipated water availability and operation of J-C System</b></h4>
                            <br />
                            <%--<div class="row">
                                <div class="col-md-offset-9 col-md-1 text-center">
                                    <asp:Label ID="lblMAFKharif" Text="MAF" runat="server"></asp:Label>
                                </div>
                            </div>--%>
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
                                    <asp:Label ID="Label1" Text="Rim Station Inflows" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblJhelumManglaInflows" runat="server" Text="Jhelum At Mangla Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblJhelumManglaInflowsEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJhelumManglaInflowsLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJhelumManglaInflowsTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblJhelumManglaProbability" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lbljhelumManglaProbabilityEk" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lbljhelumManglaProbabilityLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lbljhelumManglaProbabilityTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblChenabMarlaInflows" runat="server" Text="Chenab At Marla Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblChenabMarlaInflowsEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChenabMarlaInflowsLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChenabMarlaInflowsTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblChenabMarlaProbability" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblChenabMarlaProbabilityEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChenabMarlaProbabilityLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblChenabMarlaProbabilityTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblERComponents" runat="server" Text="E.R Components" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblERComponentsEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblERComponentsLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblERComponentsTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblSubTotal" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblSubTotalEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotalLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSubTotal_Total" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblStorageRelease" runat="server" Text="Storage/Release " Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblMangla" runat="server" Text="Mangla" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblManglaEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblManglaLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblManglaTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblTotalSystemInflows" runat="server" Text="Total System Inflows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalSystemInflowsTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblSystemLG" runat="server" Text="System L (-) /G (+)" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblSystemLGEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSystemLGLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblSystemLGTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblTotalAvailability" runat="server" Text="Total Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblTotalAvailabilityEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalAvailabilityLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblTotalAvailabilityTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblJCOutFlows" runat="server" Text="J-C Out Flows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblJCOutFlowsEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJCOutFlowsLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblJCOutFlowsTotal" runat="server" Text="0.00"></asp:Label>
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
                                    <asp:Label ID="lblShare" runat="server" Text="Share" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblShareEK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShareLK" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShareTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblShortage" runat="server" Text="Shortage %" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 numberAlign">
                                    <asp:Label ID="lblShortageEK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShortageLK" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-1 col-md-offset-2 numberAlign">
                                    <asp:Label ID="lblShortageTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divRabi" runat="server" style="display: none">
                        <div class="col-md-12">
                            <h4 class="text-center"><b>Anticipated water availability and operation of J-C System</b></h4>
                            <br />
                            <%--<div class="row">
                                <div class="col-md-offset-7 col-md-1">
                                    <asp:Label ID="lblMAFRabi" Text="MAF" runat="server"></asp:Label>
                                </div>
                            </div>--%>
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
                                    <asp:Label ID="lblJhelumAtManglaInflowsText" runat="server" Text="Jhelum At Mangla Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblJhelumAtManglaInflowsRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblJhelumAtManglaProbabilityText" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblJhelumAtManglaProbabilityRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblChenabAtMarlaInflows" runat="server" Text="Chenab At Marla Inflows" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblChenabAtMarlaInflowsrabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblChenabAtMarlaProbability" runat="server" Text="Probability" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblChenabAtMarlaProbabilityRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblERComponent" runat="server" Text="E.R Components" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblERComponentRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblSubTotalText" runat="server" Text="Sub Total"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblSubTotalRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblStorageReleaseText" runat="server" Text="Storage/Release" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblManglaText" runat="server" Text="Mangla" CssClass="Paddingleft"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblManglaRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblTotalSystemInflowsText" runat="server" Text="Total System Inflows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblTotalSystemInflowsRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblSystemLGText" runat="server" Text="System L (-) /G (+)" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblSystemLGRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="TotalAvailabilityText" runat="server" Text="Total Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="TotalAvailabilityRabi" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblJCOutFlowsText" runat="server" Text="J-C Out Flows" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblJCOutFlowsRabi" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblCanalAvailabilityText" runat="server" Text="Canal Availability" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblCanalAvailabilityRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblShareText" runat="server" Text="Share" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblShareRabi" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-offset-4 col-md-3">
                                    <asp:Label ID="lblShortageText" runat="server" Text="Shortage %" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-1 text-center">
                                    <asp:Label ID="lblShortageRabi" runat="server"></asp:Label>
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
