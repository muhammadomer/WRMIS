<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutletAlterationHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet.OutletAlterationHistory" %>

<%@ Register Src="~/Modules/IrrigationNetwork/Controls/ChannelOutletAlterationDetails.ascx" TagPrefix="uc1" TagName="ChannelOutletAlterationDetails" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><%--<i class="fa fa-file"></i>--%>Outlet History</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnAlternateID" runat="server" Value="0" />

        <div class="box-content">
            <uc1:ChannelOutletAlterationDetails runat="server" ID="ChannelOutletAlterationDetail" />
            <%--            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date">
                                    <span class="input-group-addon clear"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker"></asp:TextBox>
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
                <h3>Outlet History</h3>
                <asp:GridView ID="gvAlterationHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True" DataKeyNames="FileName,ActionType"
                    OnPageIndexChanging="gvAlterationHistory_PageIndexChanging" OnRowDataBound="gvAlterationHistory_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblActionType" runat="server" Text='<%# Eval("ActionType")==null?Eval("ActionType"):Eval("ActionType").ToString()=="E"?"Edit":Eval("ActionType").ToString()=="D"?"Add":Eval("ActionType").ToString()=="A"?"Alter":Eval("ActionType") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet R.D & Side">
                            <ItemTemplate>
                                <asp:Label ID="lblRDSide" runat="server" Text='<%# Eval("RDandSide") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblAlterationDate" runat="server" Text='<%# Eval("AlterationDate") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet Type">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletType" runat="server" Text='<%#Eval("OutletType") %>'>
                                </asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design Discharge (Cusec)">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignDischarge" runat="server" Text='<%# Eval("DesignDischarge") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Diameter/Width (ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblDiameterWidth" runat="server" Text='<%# Eval("OutletWidth") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height of Outlet (Y in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletHeight" runat="server" Text='<%# Eval("OutletHeight") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head above Crest (H in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblHeadaboveCrest" runat="server" Text='<%# Eval("OutletCrest") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Submergence (h in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblSubmergence" runat="server" Text='<%# Eval("OutletSubmergence") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Crest Reduced Level (ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblCrestReducedLevel" runat="server" Text='<%# Eval("OutletCrestRL") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Minimum Modular Head (mmh in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblMimimumModularHead" runat="server" Text='<%# Eval("OutletMMH") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Working Head (wh in ft)">
                            <ItemTemplate>
                                <asp:Label ID="lblOutletWorkingHead" runat="server" Text='<%# Eval("OutletWorkingHead") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("OutletStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attachment">
                            <ItemTemplate>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </div>
    <script type="text/javascript">
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
</asp:Content>
