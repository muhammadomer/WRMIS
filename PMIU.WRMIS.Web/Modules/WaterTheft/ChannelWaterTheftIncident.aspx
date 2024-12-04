<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelWaterTheftIncident.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ChannelWaterTheftIncident" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3 id="pageTitle">Water Theft Incident - SDO</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">
            <asp:PlaceHolder runat="server" ID="WaterTheftIncidentInformation"></asp:PlaceHolder>
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCanalWireNo" ID="lblCanalWireNo" Text="Sub Engineer Canal Wire #" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCanalWireNo" autofocus="autofocus" runat="server" placeholder="Sub Engineer Canal Wire #" required="required" data-errormessage-value-missing="This field is required" CssClass="required form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCanalWireDate" ID="lblCanalWireDate" Text="Sub Engineer Canal Wire Date" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtCanalWireDate" runat="server" required="required" CssClass="required form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div id="DivHideClosingRepairDateField" runat="server" class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtClosingRepairDate" ID="lblClosingRepairDate" Text="Date of Closing/Repair" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtClosingRepairDate" runat="server" CssClass="form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtComments" ID="lblComments" Text="Add Comments" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtComments" Rows="3" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="lblAttachments" ID="lblAttachments" Text="Attachments" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <input type="file" class="form-control">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:Button runat="server" OnClick="btnAssignToSDO_Click" ID="btnAssignToSDO" CssClass="btn btn-primary" Text="&nbsp;Assign To SDO" />
                        <asp:PlaceHolder runat="server" ID="pnlIsSDOWorking" Visible="false">
                            <asp:Button runat="server" ID="btnMarkCaseNA" OnClick="btnMarkCaseNA_Click" CssClass="btn btn-primary" Text="&nbsp;Mark Case as N/A" />&nbsp;
                            <asp:Button runat="server" ID="btnAssignToSBE" CssClass="btn btn-primary" Text="&nbsp;Assign Back to SBE" />&nbsp;
                            <asp:Button runat="server" ID="btnAssignToZiladar" OnClick="btnAssignToZiladar_Click" CssClass="btn btn-primary" Text="&nbsp;Assign to Ziladar" />&nbsp;
                        </asp:PlaceHolder>
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
