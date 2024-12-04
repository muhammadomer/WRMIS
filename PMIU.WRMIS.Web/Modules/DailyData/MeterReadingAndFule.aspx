<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeterReadingAndFule.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.MeterReadingAndFule" %>

<%@ Register TagPrefix="uc1" TagName="FileUploadControl" Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Meter Reading And Fuel</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblActivity" runat="server" Text="Activity" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlactivity" runat="server" CssClass="form-control required" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFromDate" runat="server" Text="Date From" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblToDate" runat="server" Text="Date To" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblActivityBy" runat="server" Text="Activity By" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlActivityBy" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlActivityBy_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblADM" runat="server" Text="ADM" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlADM" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlADM_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblMA" runat="server" Text="MA" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlMA" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            <%--<asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/FloodInspection/Joint/AddJoint.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvMeterReadingAndFule" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="VehicleReadingID"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvMeterReadingAndFule_PageIndexChanging" OnPageIndexChanged="gvMeterReadingAndFule_PageIndexChanged" OnRowDataBound="gvMeterReadingAndFule_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFloodInspectionID" runat="server" Text='<%# Eval("VehicleReadingID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observation Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblObservationDate" runat="server" Text='<%#Eval("ReadingServerDate", "{0:dd-MMM-yyyy}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observe By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblObserveBy" runat="server" Text='<%#Eval("ObserveBY") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Meter Reading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMeterReading" runat="server" Text='<%#Eval("MeterReading") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Petrol Reading" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPetrolReading" runat="server" Text='<%#Eval("PetrolQuantity","{0:0,0.0}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTime" runat="server" Text='<%#Eval("ServerTime") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachment">

                                    <ItemTemplate>
                                        <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("AttachmentFile1")%>' Visible="False"></asp:Label>
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Name="FormCtrl" />
                                        <%--<asp:HyperLink ID="hlAttachment" runat="server" CssClass="btn btn-link" Text='<%# Eval("AttachmentFile1")%>'></asp:HyperLink>--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>


                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <%--<asp:HiddenField ID="hdnFloodInspectionID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />--%>
    </div>
<%--    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };
    </script>--%>

</asp:Content>
