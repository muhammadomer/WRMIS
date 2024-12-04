<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="BarrageHeadworkDischargeData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.BarrageHeadworkDischargeData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Discharge Data of Barrage/Headwork</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblBarrageName" runat="server" Text="Barrage Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlBarrage" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" required="required" CssClass="date-picker form-control required"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Show" OnClick="btnShow_Click" />
                                    <input id="txtReasonForChangeValue" runat="server" clientidmode="Static" style="display: none;" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvBarrage" runat="server" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowDataBound="gvBarrage_RowDataBound"
                            OnRowCancelingEdit="gvBarrage_RowCancelingEdit" OnRowUpdating="gvBarrage_RowUpdating"
                            OnRowEditing="gvBarrage_RowEditing" AutoGenerateEditButton="false"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="right">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="right">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-success btn_32 plus" ToolTip="Add" />
                                            <asp:Button ID="btnAuditTrail" runat="server" CssClass="btn btn-primary btn_24 audit" ToolTip="History" OnClick="btnAuditTrail_Click" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />

                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>










                    <!-- START Of Audit Trail -->
                    <div id="auditTrail" class="modal fade">
                        <div class="modal-dialog" style="width: 95%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table id="tblChannelDetail" class="table tbl-info">
                                                <tr>
                                                    <td><b>Barrage Name</b></td>
                                                    <td>
                                                        <asp:Label ID="lblatBarrageName" runat="server"></asp:Label></td>
                                                    <td><b>Reading TimeStamp</b></td>
                                                    <td>
                                                        <asp:Label ID="lblatTimeHrs" runat="server"></asp:Label></td>
                                                </tr>

                                                <%--<asp:TableRow>
                                                    <asp:TableHeaderCell>Barrage Name</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell>Time (Hrs)</asp:TableHeaderCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblatBarrageName" runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblatTimeHrs" runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>--%>
                                            </table>

                                            <asp:GridView ID="gvAuditTrail" runat="server"
                                                CssClass="table header" AllowPaging="True" AllowCustomPaging="true"
                                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                                                OnRowDataBound="gvAuditTrail_RowDataBound">

                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Detail Modal Ends here -->
                    <!-- END Of Audit Trail -->

















                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        function CalculateDischarge(GuageID, GuageInputID, DischargeInputID, UPStreamID, DownStreamID, ListOfGaugeDischargeID) {
            var GuageValue = $('#' + GuageInputID).val();

            if ($('#' + GuageID).val().trim() != '0' && GuageValue.trim() != '' && GuageValue.trim() != '0') {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("BarrageHeadworkDischargeData.aspx/CalculateDischarge") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_AttributeID: "' + $('#' + GuageID).val().trim() + '",_GaugeValue:"' + GuageValue + '"}',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        if (data.d != null) {

                            $('#' + DischargeInputID).val(data.d);
                            CalculateTotalDischargeUS(UPStreamID, DownStreamID, ListOfGaugeDischargeID);
                        }
                    }
                });
            }
        }

        function CalculateTotalDischargeUS(UPStreamID, DownStreamID, ListOfGaugeDischargeID) {
            var UpStream = 0;
            var DownStream;
            //alert($('#' + DownStreamID));
            if ($('#' + DownStreamID).val().trim() != '') {
                UpStream = parseFloat($('#' + DownStreamID).val().trim());
            }
            //code for UpStream
            //if ($('#' + UPStreamID).val().trim() != '') {
            //    var a = $('#' + UPStreamID).val().trim();
            //    alert(a);
            //}
            //alert(ListOfGaugeDischargeID);
            var removeFirstBracket = ListOfGaugeDischargeID.replace("[", "");
            var removeSecondBracket = removeFirstBracket.replace("]", "");
            var removeQuotes = removeSecondBracket.replace(/['"]+/g, '');
            var Splitter = removeQuotes.split(",");

            for (var i = 0; i < Splitter.length; i++) {
                //var a = $("#" + Splitter[i]).val();
                //alert(Splitter[i]);
                if ($('#' + Splitter[i]).val().trim() != '') {
                    UpStream = UpStream + parseFloat($('#' + Splitter[i]).val().trim());
                }
            }
            $('#' + UPStreamID).val(UpStream);

        }

        function GetReasonForChangeValue(ddlReasonForChangeID, txtReasonForChangeID) {
            var ReasonForChangeValue = $("#" + ddlReasonForChangeID + " option:selected").text();
            $('#' + txtReasonForChangeID).val(ReasonForChangeValue.toString());
            //alert(ReasonForChangeValue.toString());
        }


    </script>
</asp:Content>
