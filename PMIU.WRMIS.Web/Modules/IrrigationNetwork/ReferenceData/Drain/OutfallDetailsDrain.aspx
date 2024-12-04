<%@ Page Title="OutfallDrain" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="OutfallDetailsDrain.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain.OutfallDetailsDrain" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDrainDetails" TagName="DrainDetails" Src="~/Modules/IrrigationNetwork/Controls/DrainDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Outfall Details of Drain</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">
            <ucDrainDetails:DrainDetails runat="server" ID="DrainDetails" />


            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <h3>Outfall Details</h3>

                    </div>
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlOutfallType" id="lblOutfallType" class="col-sm-4 col-lg-3 control-label">Outfall Type</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlOutfallType" CssClass="form-control required" AutoPostBack="True" required="required" OnSelectedIndexChanged="ddlOutfallType_SelectedIndexChanged" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlZone" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" Enabled="false" required="required" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlCircle" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" Enabled="false" required="required" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" Enabled="false" required="required" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub-Division</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlSubDivision" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" required="required" Enabled="false" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlOutfallName" id="lblOutfallName" class="col-sm-4 col-lg-3 control-label">Outfall Name</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlOutfallName" CssClass="form-control required" AutoPostBack="True" Enabled="false" required="required" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlOutfallSide" id="lblOutfallSide" class="col-sm-4 col-lg-3 control-label">Outfall Side</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlOutfallSide" CssClass="form-control required" required="required" AutoPostBack="True" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblOutfallRD" class="col-sm-4 col-lg-3 control-label">Outfall RD</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtFromRDLeft" runat="server" class="integerInput RDMaxLength required form-control" required="required" Style="max-width: 47.5%; display: inline;"></asp:TextBox>
                                    <div style="width:2%;display:inline;">+</div>
                                    <asp:TextBox ID="txtFromRDRight" runat="server" class="integerInput RDMaxLength required form-control" required="required" Style="max-width: 47.5%; display: inline;"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-12">
                        <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" TabIndex="10" />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" TabIndex="11">&nbsp;Back</asp:HyperLink>
                    </div>



                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnDrainOutfallID" runat="server" Value="0" />
</asp:Content>
