<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GaugeInformation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.GaugeInformation" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/IrrigationNetwork/Controls/ChannelDetails.ascx" TagPrefix="ucChannelDetail" TagName="ChannelDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <asp:UpdatePanel runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="hdnID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDivID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSubDivID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSectionID" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnNoDeletionMessage" runat="server" ClientIDMode="Static" Value="Record cannot be deleted." />
    <div class="box">
        <div class="box-title">
            <h3>Gauge Information</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucChannelDetail:ChannelDetails runat="server" ID="ChannelDetails" />

            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvGaugeInformation" runat="server" DataKeyNames="ID,SubDivisionID,SectionID,GaugeTypeID,GaugeCategoryID,TotalGaugeRD,DesignDischarge,GaugeAtBedID,DivisionName,DivisionID,CircleID"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvGaugeInformation_RowCommand" OnRowDataBound="gvGaugeInformation_RowDataBound"
                            OnRowEditing="gvGaugeInformation_RowEditing" OnRowCancelingEdit="gvGaugeInformation_RowCancelingEdit"
                            OnRowUpdating="gvGaugeInformation_RowUpdating" OnRowDeleting="gvGaugeInformation_RowDeleting" OnPageIndexChanging="gvGaugeInformation_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubDivisionName" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditSubDivisionName" runat="server" CssClass="hidden" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlSubDivision" runat="server" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" required="required" CssClass="required form-control" Style="max-width: 90%;" AutoPostBack="true"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Section">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval("SectionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditSectionName" runat="server" Text='<%# Eval("SectionName") %>' CssClass="hidden"></asp:Label>
                                        <asp:DropDownList ID="ddlSection" runat="server" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged" AutoPostBack="true" CssClass="required form-control" required="required" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gauge Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeTypeName" runat="server" Text='<%# Eval("GaugeTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGaugeType" runat="server" CssClass="form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gauge Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeCategoryName" runat="server" Text='<%# Eval("GaugeCategoryName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditGaugeCategoryName" runat="server" Text='<%# Eval("GaugeCategoryName") %>' CssClass="hidden"></asp:Label>
                                        <asp:DropDownList ID="ddlGaugeCategory" runat="server" OnSelectedIndexChanged="ddlGaugeCategory_SelectedIndexChanged" AutoPostBack="true" CssClass="required form-control" required="required" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gauge R.D (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeRD" runat="server" Text='<%# Eval("GaugeRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditGaugeRD" runat="server" Text='<%# Eval("GaugeRD") %>' CssClass="hidden"></asp:Label>
                                        <asp:Panel ID="panelRDs" runat="server">
                                            <asp:TextBox ID="txtGaugeRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                            +
                                        <asp:TextBox ID="txtGaugeRDRight" runat="server" oninput="CompareRDValues(this)" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Design Discharge (Cusec)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignDischarge" runat="server" Text='<%# Eval("DesignDischarge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesignDischarge" runat="server" class="decimalInput form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gauge at Bed or Crest?">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGaugeAtBed" runat="server" Text='<%# Eval("GaugeAtBedName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGaugeAtBed" runat="server" required="required" CssClass="required form-control" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddGaugeInformation" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddGaugeInformation" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddGaugeInformation" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionGaugeInformation" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnMapping" runat="server" CommandName="GaugeMapping" CssClass="btn btn-primary btn_32 userlocation" ToolTip="Gauge Mapping" formnovalidate="formnovalidate"></asp:Button>
                                            <asp:HyperLink ID="hlDTParameters" runat="server" CssClass="btn btn-primary btn_24 dt-parameter" NavigateUrl='<%# GetDTParameterPageURL(Convert.ToString(Eval("GaugeAtBedID")), Convert.ToString(Eval("ID"))) %>' ToolTip="DT Parameters">
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="hlDTHistory" runat="server" CssClass="btn btn-primary btn_24 dt-parameter-history" NavigateUrl='<%# GetDTHistoryPageURL(Convert.ToString(Eval("GaugeAtBedID")), Convert.ToString(Eval("ID"))) %>' ToolTip="DT History">
                                            </asp:HyperLink>
                                            <asp:Button ID="btnEditGaugeInformation" Visible="<%# base.CanEdit %>" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteGaugeInformation" Visible="<%# base.CanDelete %>" runat="server" Text="" CommandName="Delete"
                                                CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionGaugeInformation" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveGaugeInformation" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_32 save" />
                                            <asp:Button ID="btnCancelGaugeInformation" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />

            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
            <div class="row">
                <div id="AddGaugeLocation" class="modal fade">
                    <div class="modal-dialog modal-lg" style="width: 1000px;">
                        <div class="modal-content">
                            <div class="box">
                                <div class="box-title">
                                    <h5>Gauge Mapping</h5>
                                </div>
                                <div class="modal-body" style="padding-right: 20px;">
                                    <div class="form-horizontal">
                                        <asp:UpdatePanel runat="server" ID="up">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <h4 style="padding-left: 7px">Divisional Gauge Location</h4>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Division</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlDivisionalGL1" runat="server">
                                                                    <%--<asp:ListItem Text="All" Value="" />--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <h4 style="padding-left: 7px">Sub-Divisional Gauge Location</h4>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Division</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlDivisionalGL2" runat="server" OnSelectedIndexChanged="ddlDivisionalGL2_SelectedIndexChanged" AutoPostBack="true">
                                                                    <%--<asp:ListItem Text="All" Value="" />--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" id="Label1" runat="server">Sub-Division</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control " ID="ddlSubDivisionalGL1" runat="server" Enabled="False">
                                                                    <asp:ListItem Text="Select" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <h4 style="padding-left: 7px">Sectional Gauge Location</h4>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Division</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlDivisionalGL3" runat="server" OnSelectedIndexChanged="ddlDivisionalGL3_SelectedIndexChanged" AutoPostBack="true">
                                                                    <%--  <asp:ListItem Text="All" Value="" />--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" id="lblSubDivisionalGL2" runat="server">Sub-Division</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlSubDivisionalGL2" runat="server" OnSelectedIndexChanged="ddlSubDivisionalGL2_SelectedIndexChanged" AutoPostBack="true" Enabled="False">
                                                                    <asp:ListItem Text="Select" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Section</label>
                                                            <div class="col-sm-8 col-lg-8 controls">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlSection" runat="server" Enabled="False">
                                                                    <asp:ListItem Text="Select" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                                <br />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSave1" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:Button>
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-info" OnClick="btnClose_Click" formnovalidate="formnovalidate"></asp:Button>
                                        <%--<button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
        function DisplayAutoGeneratedMessage() {
            $('#lblMsgs').addClass('ErrorMsg').show();
            $('#lblMsgs').html($("#hdnNoDeletionMessage").val());
            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
        }
        //$('#btnClose').click(function () {
        //    debugger
        //    $("#ddlDivisionalGL1").removeClass("required");
        //    $("#ddlDivisionalGL1").removeAttr("required");
        //    $("#ddlDivisionalGL2").removeClass("required");
        //    $("#ddlDivisionalGL2").removeAttr("required");
        //    $("#ddlDivisionalGL3").removeClass("required");
        //    $("#ddlDivisionalGL3").removeAttr("required");
        //    $("#ddlSubDivisionalGL1").removeClass("required");
        //    $("#ddlSubDivisionalGL1").removeAttr("required");
        //    $("#ddlSubDivisionalGL2").removeClass("required");
        //    $("#ddlSubDivisionalGL2").removeAttr("required");
        //    $("#ddlSection").removeClass("required");
        //    $("#ddlSection").removeAttr("required");
        //});
    </script>
</asp:Content>
