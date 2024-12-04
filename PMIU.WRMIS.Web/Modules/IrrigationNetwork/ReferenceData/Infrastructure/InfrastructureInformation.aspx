<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfrastructureInformation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.InfrastructureInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
  <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
  <!-- BEGIN Main Content -->
  <asp:Label ID="lblMessage" runat="server"></asp:Label>
  <div class="box">
    <div class="box-title">
      <h3>Protection Infrastructure Detail</h3>
    </div>
    <div class="box-content">

      <div class="form-horizontal">
        <div class="row">
          <div class="col-md-12">
            <div class="hidden">
              <asp:HiddenField ID="hdnInfrastructureID" runat="server" Value="0" />
              <asp:HiddenField ID="hdnInfrastructureCreatedDate" runat="server" Value="0" />
              <asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Type</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblTypeVal" runat="server"></asp:Label>
                </div>
              </div>
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Name</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblNameValue" runat="server"></asp:Label>
                </div>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Total Length (ft)</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblTotalLengthVal" runat="server"></asp:Label>
                </div>
              </div>
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Initial Cost (Rs.)</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblInitialCostVal" runat="server"></asp:Label>
                </div>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Designed Top Width (ft)</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblDesignedTopWidthVal" runat="server"></asp:Label>
                </div>
              </div>
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Designed Free Board (ft)</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblDesignedFreeBoardVal" runat="server"></asp:Label>
                </div>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Country Side Slope</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblCountrySideSlopeVal" runat="server"></asp:Label>
                </div>
              </div>
              <div class="col-lg-6 col-sm-12 form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Designed River Side Slope</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblDesignedRiverSideSlopeVal" runat="server"></asp:Label>
                </div>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 text-left form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Status</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblInfrastructureStatusVal" runat="server"></asp:Label>
                </div>
              </div>
            </div>

            <div class="row">
              <div class="col-lg-6 col-sm-12 text-left form-group">
                <strong class="col-xs-6 col-lg-6 control-label">Description</strong>
                <div class="col-sm-6 col-lg-6 controls">
                  <asp:Label ID="lblInfrastructureDescriptionVal" runat="server"></asp:Label>
                </div>
              </div>
            </div>

          </div>

        </div>

        <br />

        <div class="row">
          <div class="col-md-6">
            <div class="fnc-btn">
              <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx" CssClass="btn">Back</asp:HyperLink>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- END Main Content -->
</asp:Content>
