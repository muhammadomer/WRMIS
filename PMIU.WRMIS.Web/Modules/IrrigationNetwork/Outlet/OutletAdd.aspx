<%@ Page Title="" AutoEventWireup="true" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" CodeBehind="OutletAdd.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet.OutletAdd" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/IrrigationNetwork/Controls/OutletChannelDetails.ascx" TagPrefix="uc1" TagName="ChannelDetails" %>
<%@ Register TagPrefix="uc1" TagName="FileUploadControl_1" Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><span runat="server" id="pageTitleID"></span></h3>
            <div class="box-tool">
                <%--<a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>--%>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAlternateID" runat="server" Value="0" />
            <uc1:ChannelDetails runat="server" ID="ChannelDetails" />
            <hr>

            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">


                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Outlet RD (ft)</asp:Label>
                            <div class="col-sm-3 col-lg-4 controls">
                                <asp:TextBox ID="txtOutletRDLeft" TabIndex="1" autofocus="autofocus" runat="server" placeholder="Outlet RD" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RDMaxLength required form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-1 col-lg-1 controls">
                                +
                            </div>
                            <div class="col-sm-3 col-lg-4 controls">
                                <asp:TextBox ID="txtOutletRDRight" TabIndex="2" runat="server" oninput="CompareRDValues(this)" placeholder="Outlet RD" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RDMaxLength required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtGrossCommandArea" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Gross Command Area (GCA - Acre)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtGrossCommandArea" TabIndex="4" runat="server" placeholder="Gross Command Area" oninput="ValidateGCA(this)" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtDesignDischarge" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Design Discharge (Cusec)</asp:Label>

                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDesignDischarge" runat="server" TabIndex="6" placeholder="Design Discharge" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>


                    </div>
                    <div class="col-md-6 ">


                        <div class="form-group">
                            <asp:Label AssociatedControlID="ddlOutletSide" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Outlet Side</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlOutletSide" TabIndex="3" required="required" runat="server" CssClass="required form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCultureableCommandArea" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Cultureable Command Area (CCA-Acre)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCultureableCommandArea" runat="server" TabIndex="5" placeholder="Cultureable Command Area" oninput="ValidateCCA(this)" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label AssociatedControlID="ddlOutletType" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Outlet Type</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlOutletType" TabIndex="7" required="required" runat="server" CssClass="required form-control" onchange="EnableDisableDesignParameters(this);">
                                </asp:DropDownList>
                            </div>
                        </div>


                    </div>
                </div>

                <hr>

                <div class="row">
                    <div class="col-md-6 ">
                        <!-- BEGIN Left Side -->

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtOutletHeight" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Height of Outlet/Orifice (Y in ft)</asp:Label>

                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtOutletHeight" TabIndex="8" runat="server" placeholder="Height of Outlet/Orifice" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtSubmergence" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Submergence (h = H - Y in ft)</asp:Label>

                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtSubmergence" TabIndex="10" runat="server" placeholder="Submergence" ReadOnly="true" CssClass="decimalInput form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCrestRL" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Crest Reduced Level (RL in ft)</asp:Label>

                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCrestRL" runat="server" TabIndex="12" placeholder="Crest Reduced Level" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtWorkingHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Working Head (ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtWorkingHead" TabIndex="14" runat="server" placeholder="Working Head" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- END Left Side -->
                    </div>

                    <div class="col-md-6 ">
                        <!-- BEGIN Right Side -->

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtOutletCrestHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Head above Crest of Outlet (H in ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtOutletCrestHead" TabIndex="9" runat="server" placeholder="Head above Crest of Outlet" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtDBWidth" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Diameter/Breadth/ Width (Dia/B in ft)</asp:Label>

                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDBWidth" TabIndex="11" runat="server" placeholder="Diameter/Breadth/ Width" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtMMHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Minimum Modular Head (MMH in ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtMMHead" TabIndex="13" runat="server" placeholder="Minimum Modular Head" required="required" pattern="^(\d{0,5})(\.\d{0,5})?$" CssClass="decimalInput required form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="AttachFile" runat="server" visible="False">
                            <label class="col-sm-4 col-lg-3 control-label">Attach File</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <uc1:FileUploadControl_1 runat="server" ID="FileUploadControl" Size="1" />
                                <uc1:FileUploadControl_1 runat="server" ID="FileUploadControl1" Size="0" />
                            </div>
                        </div>
                        <!-- END Right Side -->
                    </div>
                </div>



                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Add Comments</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtComments" runat="server" onblur="TrimInput(this);" CssClass="form-control multiline-no-resize" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>



                <br />
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        var txtOutletHeight = $('#<%=txtOutletHeight.ClientID %>');
        var txtOutletCrestHead = $('#<%= txtOutletCrestHead.ClientID %>');
        var txtDBWidth = $('#<%= txtDBWidth.ClientID %>');
        var txtCrestRL = $('#<%= txtCrestRL.ClientID %>');
        var txtMMHead = $('#<%= txtMMHead.ClientID%>');
        var txtWorkingHead = $('#<%= txtWorkingHead.ClientID %>');
        var txtDesignDischarge = $('#<%= txtDesignDischarge.ClientID %>');
        var ddlOutletType = $('#<%= ddlOutletType.ClientID %>');

        $(document).ready(function () {

            var outletType = $("select[id$='ddlOutletType']").val();
            if (outletType != "" && $("[id$='hdnOutletID']").val() == "0")
                EnableDisableDesignParameters(outletType);

            // Remove red border from disabled fields in edit mode.
            //if ($("[id$='hdnOutletID']").val() != "0")
            //    RemoveRequiredClassInEditMode();
        });
        var OutletTypes = {
            APM: 'APM',
            OCAPM: 'OCAPM',
            OFRB: 'OFRB',
            OCOFRB: 'OCOFRB',
            OCOF: 'OCOF',
            OF: 'OF',
            LTOF: 'LTOF',
            Scartchley: 'SCARTCHLEY',
            Pipe: 'PIPE'
        };
        function RemoveRequiredClassInEditMode() {
            txtOutletHeight.removeClass("required");
            txtOutletCrestHead.removeClass("required");
            txtDBWidth.removeClass("required");
            txtCrestRL.removeClass("required");
            txtMMHead.removeClass("required");
            txtWorkingHead.removeClass("required");
            txtDesignDischarge.removeClass("required");
            ddlOutletType.removeClass("required");
        }

        function EnableDisableDesignParameters(outletTypeID) {
            var outletType = $.trim($("select[id$='ddlOutletType']").val()).toUpperCase();
            // For Outlet Types APM, OCAPM, OFRB, OCOFRB and OCOF, all the design parameter are enabled
            if (OutletTypes.Pipe != outletType)
                MakeMandatoryField(txtMMHead);


            if (OutletTypes.APM == outletType
                || OutletTypes.OCAPM == outletType
                || OutletTypes.OFRB == outletType
                || OutletTypes.OCOFRB == outletType
                || OutletTypes.OCOF == outletType) {

                EnableDesignParameter(txtOutletHeight);
                EnableDesignParameter(txtOutletCrestHead);
                EnableDesignParameter(txtDBWidth);
                EnableDesignParameter(txtCrestRL);
                EnableDesignParameter(txtMMHead);
                EnableDesignParameter(txtWorkingHead);
            }
            else if (OutletTypes.OF == outletType || OutletTypes.LTOF == outletType) {
                DisableDesignParameter(txtOutletHeight);//For Outlet Types OF and LTOF, Y is disabled and all other design parameters are enabled
                EnableDesignParameter(txtOutletCrestHead);
                EnableDesignParameter(txtDBWidth);
                EnableDesignParameter(txtCrestRL);
                EnableDesignParameter(txtMMHead);
                EnableDesignParameter(txtWorkingHead);
            }
            else if (OutletTypes.Scartchley == outletType) {
                EnableDesignParameter(txtOutletHeight);
                DisableDesignParameter(txtOutletCrestHead); //For Outlet Type Scartchley, H is disabled and all other parameters are enabled
                EnableDesignParameter(txtDBWidth);
                EnableDesignParameter(txtCrestRL);
                EnableDesignParameter(txtMMHead);
                EnableDesignParameter(txtWorkingHead);
            }
            else if (OutletTypes.Pipe == outletType) {
                DisableDesignParameter(txtOutletHeight); //For Outlet Type Pipe, Y & H are disabled, and all other parameters are enabled
                DisableDesignParameter(txtOutletCrestHead);
                EnableDesignParameter(txtDBWidth);
                EnableDesignParameter(txtCrestRL);
                EnableDesignParameter(txtMMHead);
                EnableDesignParameter(txtWorkingHead);
                RemoveMandatoryField(txtMMHead);
            }
            else {
                EnableDesignParameter(txtOutletHeight);
                EnableDesignParameter(txtOutletCrestHead);
                EnableDesignParameter(txtDBWidth);
                EnableDesignParameter(txtCrestRL);
                EnableDesignParameter(txtMMHead);
                EnableDesignParameter(txtWorkingHead);
            }
        }
        function MakeMandatoryField(fieldID) {
            fieldID.attr("required", "required").addClass("required");
        }
        function RemoveMandatoryField(fieldID) {
            fieldID.removeAttr("required").removeClass("required");
        }
        function EnableDesignParameter(fieldID) {
            fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
        }
        function DisableDesignParameter(fieldID) {
            fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");

        }
    </script>


</asp:Content>
