<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluationScoresDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.Controls.EvaluationScoresDetail" %>
<div class="row">
    <div class="col-md-4"><strong>Performance Evaluation Level</strong></div>
    <div class="col-md-4"><strong>From Date</strong></div>
    <div class="col-md-4"><strong>To Date</strong></div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblPerformanceEvaluationLevel" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblFromDate" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblToDate" runat="server"></asp:Label>
    </div>
</div>
<br />
<div class="row">
    <div class="col-md-4"><strong>Session</strong></div>
    <div class="col-md-4"><strong>Analyzed Gauges</strong></div>
    <div class="col-md-4"><strong>Closed Gauges</strong></div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblSession" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblAnalyzedGauges" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblClosedGauges" runat="server"></asp:Label>
    </div>
</div>
<br />
<div class="row">
    <div class="col-md-4"><strong>Aggregated Gauges</strong></div>
    <div class="col-md-4"><strong>Checked by PMIU</strong></div>
    <div class="col-md-4"><strong>Total Points obtained</strong></div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblAggregatedGauges" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblCheckedbyPMIU" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblTotalPointsobtained" runat="server"></asp:Label>
    </div>
</div>
<br />
<div id="dvKPICategory" runat="server" visible="false">
    <div class="row">
        <div class="col-md-4"><strong>KPI Category Name</strong></div>
        <div class="col-md-4"><strong>Weightage in KPI Category</strong></div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <asp:Label ID="lblKPICategoryName" runat="server"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblWeightageinKPICategory" runat="server"></asp:Label>
        </div>
    </div>
    <br />
</div>
<div class="row">
    <div class="col-md-12" style="color: #248dc1; font-size: 12px; font-weight: bold;">
        <asp:Label ID="lblEvaluationScoresDetailPath" runat="server"></asp:Label>
    </div>
</div>
