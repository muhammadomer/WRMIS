<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LSectionParametersHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Reach.LSectionParametersHistory" %>

<%@ Register Src="~/Modules/IrrigationNetwork/Controls/LSectionParameters.ascx" TagPrefix="uc1" TagName="LSectionParameter" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><%--<i class="fa fa-file"></i>--%>L Section Parameters History</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnReachID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnL" runat="server" Value="" />
        <div class="box-content">
            <uc1:LSectionParameter runat="server" ID="LSectionParameters" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From Date</label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To Date</label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnShowHistory" OnClientClick="javascript:return ValidateDate();" OnClick="btnShowHistory_Click" CssClass="btn btn-primary" Text="&nbsp;Show History" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive" visible="false" id="content" runat="server">
                        <h3>L-Section Parameters History</h3>
                        <asp:GridView ID="gvLSectionHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="LSectionImage"
                            ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                            OnPageIndexChanging="gvLSectionHistory_PageIndexChanging" OnRowDataBound="gvLSectionHistory_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Parameter change Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParameterChangeDate" runat="server" Text='<%# Eval("ParameterDate") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NS Level (ft.)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNSLevel" runat="server" Text='<%#Eval("NaturalSurfaceLevel") %>'>
                                        </asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AFS (Cusec)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAFS" runat="server" Text='<%# Eval("AFS") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bed Level (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedLevel" runat="server" Text='<%# Eval("BedLevel") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FSL (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFSL" runat="server" Text='<%# Eval("FSL") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bed Width (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedWidth" runat="server" Text='<%# Eval("BedWidth") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FS Depth (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFSDepth" runat="server" Text='<%# Eval("FSDepth") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Side Slope (h:w)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSideSlope" runat="server" Text='<%# Eval("SideSlop") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Slope in 0/00 (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSlopIn" runat="server" Text='<%# Eval("SlopeIn") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Laceys's f or C.V.R">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLaceys" runat="server" Text='<%# Eval("Laceys") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Free Board (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFreeBoard" runat="server" Text='<%# Eval("FreeBoard") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Width Left (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankWidthLeft" runat="server" Text='<%# Eval("BankWidthLeft") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Width Right (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankWidthRight" runat="server" Text='<%# Eval("BankWidthRight") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lined or Unlined">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinedorUnlined" runat="server" Text='<%# Eval("LinedorUnlined") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type of Lining">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTypeofLining" runat="server" Text='<%# Eval("LiningType") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachment">
                                    <ItemTemplate>
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="0" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionDailyData" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnViewLSectionImage" runat="server" Text="" CommandName="ViewLSectionImage" CssClass="btn btn-primary btn_24 viewimg" ToolTip="View L Section Image" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle Width="285px" HorizontalAlign="Right" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnShowHistory" EventName="click" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- Start Of View Image -->
            <%--            <div id="viewimage" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <center>
                                        <asp:Image ID="imgLSectioinImage" runat="server" width="300px" height="420px"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
                <!-- END Of View Gauge Image -->
            </div>--%>
        </div>
    </div>
    <%--    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>
    <script src="/Design/js/custom.js"></script>
    <script type="text/javascript">

<%--        var today = new Date(GetParsedDate(GetCurrentDate(), "dd-MM-yyyy"));
        $("#<%=txtToDate.ClientID%>").val(GetFormatedDate((today)));
        $("#<%=txtFromDate.ClientID%>").val(GetFormatedDate(today.addDays(-1)));--%>


        function ValidateDate() {
            dpg = $.fn.datepicker.DPGlobal;
            date_format = 'dd-MM-yyyy';
            var fromDate = dpg.parseDate($('#<%=txtFromDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en');
            var toDate = dpg.parseDate($('#<%=txtToDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en');

            console.log(dpg.parseDate($('#<%=txtFromDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en'));
            console.log(dpg.parseDate($('#<%=txtToDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en'));

            if (fromDate >= toDate) {
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("To date should be greater then " + $('#<%=txtFromDate.ClientID%>').val());
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                    ClearDateField();
                }
            });
        };
    </script>
</asp:Content>
