<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LSectionParameters.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach.LSectionParameters" %>

<%@ Register Src="~/Modules/IrrigationNetwork/Controls/LSectionParameters.ascx" TagPrefix="uc1" TagName="LSectionParameters" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add/Edit L Section Parameter</asp:Literal></h3>
        </div>
        <asp:HiddenField ID="hdnLinedTypeID" runat="server" Value="" />
        <div class="box-content">
            <uc1:LSectionParameters runat="server" ID="LSectionParameter" />
            <div class="form-horizontal">
                <div class="row">
                    <!-- BEGIN Left Side -->
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Parameter Change Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtParameterChangeDate" runat="server" CssClass="date-picker form-control required" required="true" onfocus="this.value = this.value;" TabIndex="1"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Natural Surface Level (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtNaturalSurfaceLevel" class="decimalInput form-control required" runat="server" TabIndex="2" MaxLength="10" required="true" ClientIDMode="Static" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Bed Level (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtBedLevel" class="decimalInput form-control required" runat="server" TabIndex="4" MaxLength="10" required="true" ClientIDMode="Static" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Bed Width (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtBedWidth" class="decimalInput form-control required" runat="server" TabIndex="6" required="true" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Side Slope (h:w)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtSideSlop" class="decimalInput form-control required" runat="server" TabIndex="8" required="true" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Linned or Unlinned</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlLinedOrUnlined" runat="server" CssClass="form-control" required="true" TabIndex="14" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label Text="Lacey's f or Critical Velocity Ratio" ID="lblLacey" runat="server" CssClass="col-sm-4 col-lg-3 control-label"/>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCriticalVelocityRatio" runat="server" CssClass="decimalInput form-control required" required="true" TabIndex="10" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Type of Lining</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlTypeofLining" runat="server" CssClass="form-control" TabIndex="15">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <!-- END Left Side -->
                    <!-- BEGIN Right Side -->
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Authorized Full Supply (Cusec)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtAuthorizedFullSupply" class=" decimalInput form-control required" runat="server" TabIndex="3" MaxLength="10" required="true" ClientIDMode="Static" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Full Supply Level (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtFullSupplyLevel" class="decimalInput form-control required" runat="server" TabIndex="5" MaxLength="10" required="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Full Supply Depth (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtFullSupplyDepth" class="decimalInput form-control required" runat="server" TabIndex="7" required="true" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Slope in 0/00 (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtSlopeIn" class="slopeDecimalInput text-right form-control required" runat="server" TabIndex="9" required="true" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <%--                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Manning's Roughness Coefficient (N)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtMRCoefficient" runat="server" CssClass="decimalInput form-control required" required="true" TabIndex="10" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Free Board (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtFreeBoard" runat="server" CssClass="decimalInput form-control required" required="true" TabIndex="11" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Right Bank Width (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtRightBankWidth" runat="server" CssClass="decimalInput form-control required" required="true" TabIndex="13" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Left Bank Width (ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtLeftBankWidth" runat="server" CssClass="decimalInput form-control required" required="true" TabIndex="12" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">L Section</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:FileUpload ID="fuLSection" runat="server" CssClass="form-control" />
                                <%--    <asp:HyperLink ID="lnkFile" Text="" runat="server"></asp:HyperLink>--%>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="0" />
                            </div>
                        </div>
                    </div>
                    <!-- END Right Side -->
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Label ID="hdnLabel" runat="server" Visible="false"></asp:Label>
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slopeDecimalInput").numeric({ decimal: ".", negative: false, scale: 4, decimalPlaces: 4 });
        });
    </script>


    <script type="text/javascript">
        var txtCriticalVelocityRatio = $('#<%=txtCriticalVelocityRatio.ClientID %>');

        jQuery(document).ready(function () {
            var linedType = $("select[id$='ddlLinedOrUnlined']").val();
            if (linedType != "" && $("[id$='hdnLinedTypeID']").val() != "") {
                console.log(linedType);
              //  EnableDisableLaceyManning(linedType);
            }
        });

        var LinedTypes = {
            Lined: '0',
            Unlined: '1'
        };

        function EnableDisableLaceyManning(linedType) {
            var linedType = $.trim($("select[id$='ddlLinedOrUnlined']").val());

            if (LinedTypes.Lined == linedType) {
                $("select[id$='ddlTypeofLining']").removeAttr("disabled");
                DisableDesignParameter(txtCriticalVelocityRatio);
                txtCriticalVelocityRatio.val('');
            }
            else if (LinedTypes.Unlined == linedType) {
                EnableDesignParameter(txtCriticalVelocityRatio);
                $("select[id$='ddlLinedOrUnlined']").find('option:eq(0)').attr('selected', true);
                $("select[id$='ddlTypeofLining']").attr('disabled', 'disabled');
                txtMRCoefficient.val('');
            }
            else {
                $("select[id$='ddlLinedOrUnlined']").val('');
                $("select[id$='ddlTypeofLining']").attr('disabled', 'disabled');
                EnableDesignParameter(txtCriticalVelocityRatio);
            }
        }
        function EnableDesignParameter(fieldID) {
            fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
        }
        function DisableDesignParameter(fieldID) {
            fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");

        }
    </script>
</asp:Content>
