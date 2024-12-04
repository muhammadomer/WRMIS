<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnnualScntdDschrgSuply.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.AnnualScntdDschrgSuply" %>

<%@ Import Namespace="PMIU.WRMIS.Common" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Annual Sanctioned Discharge/Supply</h3>
                </div>


                <div class="box-content">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hdfID" Value="0" runat="server" />

                        <asp:Panel ID="pnlEffluent" runat="server" GroupingText="Effluent Waters">
                            <asp:HiddenField ID="hdfEffluentDschrgID" Value="0" runat="server" />
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-4 control-label">Sanctioned Date</label>
                                            <div class="col-sm-8 col-lg-8 controls">
                                                <asp:Label ID="lblSnctDate" runat="server" CssClass="form-control" Enabled="false"></asp:Label>
                                                <div class="input-group date" data-date-viewmode="years" id="divSnctDate" runat="server" visible="false">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtSnctDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-4 control-label">Sanctioned No.</label>
                                            <div class="col-sm-8 col-lg-8 controls">
                                                <asp:Label ID="lblSnctNo" runat="server" CssClass="form-control"></asp:Label>
                                                <asp:TextBox ID="txtNo" runat="server" autofocus="true" CssClass="form-control required" required="required" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-4 control-label">Sanctioned Authority</label>
                                            <div class="col-sm-8 col-lg-8 controls">
                                                <asp:Label ID="lblSnctAuth" runat="server" CssClass="form-control"></asp:Label>
                                                <asp:TextBox ID="txtAuth" runat="server" CssClass="form-control" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-4 control-label">Sanctioned Discharge (Cusec)</label>
                                            <div class="col-sm-8 col-lg-8 controls">
                                                <asp:Label ID="lblSnctDschrg" runat="server" CssClass="form-control"></asp:Label>
                                                <asp:TextBox ID="txtDschrg" runat="server" CssClass="form-control required decimalIntegerInput required" required="required" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-4 control-label">Attachment</label>
                                            <div class="col-sm-8 col-lg-8 controls">
                                                <uc1:FileUploadControl runat="server" ID="fcUploadFile" Size="1" Mode="2" Visible="false" />
                                                <asp:HyperLink ID="hlAtchmnt" Text="Attachment" runat="server" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="fnc-btn" style="float: right;">
                                            <asp:Button ID="btnChange" runat="server" Text="Change" CssClass="btn btn-primary" ToolTip="Change" OnClick="btn_Click" />
                                            <%--formnovalidate="formnovalidate"  --%>
                                            <asp:Button ID="btnHistry" runat="server" Text="View History" data-toggle="modal" formnovalidate="formnovalidate" CssClass="btn btn-success" ToolTip="View History" OnClick="btn_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <br />

                        <asp:Panel ID="pnlCanal" runat="server" GroupingText="Canal Special Waters">
                            <div class="table-responsive" id="divEffluent" runat="server">
                                <asp:GridView ID="gvCSW" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                                    OnPageIndexChanging="gvCSW_PageIndexChanging" ShowHeaderWhenEmpty="true" OnRowCommand="gvCSW_RowCommand"
                                    CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvCSW_PageIndexChanged"
                                    EmptyDataText="" CssClass="table header" BorderWidth="0px">
                                    <Columns>

                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdf_Atchmnt" runat="server" Value='<%# Eval("Attcmnt") %>' />
                                                <asp:Label ID="lblRecordID" runat="server" CssClass="control-label" Text='<%# Eval("recordID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblChannelID" runat="server" CssClass="control-label" Text='<%# Eval("ChannelID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblDivisionID" runat="server" CssClass="control-label" Text='<%# Eval("DivisionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Outlet/RD">
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Supply Type">
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" CssClass="control-label" Text='<%# Eval("Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sanctioned Date">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%#   Eval ("SnctDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sanctioned No.">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" CssClass="control-label" Text='<%# Eval("SnctNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sanctioned Authority">
                                            <HeaderStyle CssClass="col-md-3" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAuth" runat="server" CssClass="control-label" Text='<%# Eval("SnctAuth") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sanctioned Supply (Cusec)">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmnt" runat="server" CssClass="control-label" Text='<%# Eval("SnctSuply") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Change" CssClass="btn btn-primary btn_32 edit" ToolTip="Change" CommandArgument='<%# Eval("ID") %>' formnovalidate="formnovalidate"></asp:Button>
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="History" CssClass="btn btn-primary btn_32 history" CommandArgument='<%# Eval("ID") %>' ToolTip="History" formnovalidate="formnovalidate"></asp:Button>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <div id="Add" class="modal fade">
                            <div class="modal-dialog table-responsive" style="max-height: 419px; max-width: 893.398px;">
                                <div class="modal-content" style="width: 830px">
                                    <div class="modal-body">
                                        <div class="box">
                                            <div class="box-title">
                                                <h3>
                                                    <asp:Label ID="lblTitle" Text="" runat="server" />
                                                </h3>
                                            </div>
                                            <div class="box-content ">
                                                <div class="table-responsive">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <div id="divHistory" runat="server" visible="false" class="table-responsive">
                                                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="30" ShowHeaderWhenEmpty="true"
                                                                    CellSpacing="-1" GridLines="None" EmptyDataText="No record found" CssClass="table header" BorderWidth="0px"
                                                                    DataKeyNames="Attachment" OnRowDataBound="gv_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sanctioned Date">
                                                                            <HeaderStyle CssClass="col-md-2" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedDate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Sanctioned No.">
                                                                            <HeaderStyle CssClass="col-md-2 text-center" />
                                                                            <ItemStyle CssClass="text-right" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedNo")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Sanctioned Authority">
                                                                            <HeaderStyle CssClass="col-md-3 " />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedAuthority") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sanctioned Discharge (Cusec)">
                                                                            <HeaderStyle CssClass="col-md-3 text-center" />
                                                                            <ItemStyle CssClass="text-right" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescdis" runat="server" CssClass="control-label" Text='<%# Eval("SanctionedSupplyDischarge") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Attachment">
                                                                            <ItemTemplate>
                                                                                <asp:HyperLink ID="hlImage" runat="server" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.EffluentWaterCharges , Convert.ToString(Eval("Attachment"))) %>' Text="Attachment" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="col-md-2" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerSettings Mode="NumericFirstLast" />
                                                                    <PagerStyle CssClass="PagerStyle" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="divHistoryCanal" runat="server" visible="false" class="table-responsive">
                                                                <asp:GridView ID="gvCanal" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="30" ShowHeaderWhenEmpty="true"
                                                                    CellSpacing="-1" GridLines="None" EmptyDataText="No record found" CssClass="table header" BorderWidth="0px"
                                                                    DataKeyNames="Attcmnt" OnRowDataBound="gvCanal_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdf_Atchmnt" runat="server" Value='<%# Eval("Attcmnt") %>' />
                                                                                <asp:Label ID="lblRecordID" runat="server" CssClass="control-label" Text='<%# Eval("recordID") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>

                                                                            </ItemTemplate>

                                                                        </asp:TemplateField>
                                                                        <%--  <asp:TemplateField HeaderText="Outlet/RD">
                                                                    <HeaderStyle CssClass="col-md-1" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Supply Type">
                                                                    <HeaderStyle CssClass="col-md-1" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblType" runat="server" CssClass="control-label" Text='<%# Eval("Type") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

                                                                        <asp:TemplateField HeaderText="Sanctioned Date">
                                                                            <HeaderStyle CssClass="col-md-3" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%#   Eval ("SnctDate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Sanctioned No.">
                                                                            <HeaderStyle CssClass="col-md-3" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblNo" runat="server" CssClass="control-label" Text='<%# Eval("SnctNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Sanctioned Authority">
                                                                            <HeaderStyle CssClass="col-md-3" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAuth" runat="server" CssClass="control-label" Text='<%# Eval("SnctAuth") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Sanctioned Supply">
                                                                            <HeaderStyle CssClass="col-md-2 text-center" />
                                                                            <ItemStyle CssClass="text-right" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmnt" runat="server" CssClass="control-label" Text='<%# Eval("SnctSuply") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Attachment">
                                                                            <ItemTemplate>
                                                                                <asp:HyperLink ID="hlImage" runat="server" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.EffluentWaterCharges , Convert.ToString(Eval("Attcmnt"))) %>' Text="Attachment" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="col-md-1" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerSettings Mode="NumericFirstLast" />
                                                                    <PagerStyle CssClass="PagerStyle" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="divAdd" runat="server" visible="false">
                                                                <div class="form-horizontal">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <asp:HiddenField ID="hdf_RcrdID_CSW" runat="server" Value="0" />
                                                                                <asp:HiddenField ID="hdfID_CSW" runat="server" Value="" />
                                                                                <label class="col-sm-4 col-lg-4 control-label">Sanctioned Date</label>
                                                                                <div class="col-sm-8 col-lg-8 controls">
                                                                                    <div class="input-group date" data-date-viewmode="years" id="div1" runat="server">
                                                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                        <asp:TextBox ID="txtSnctDate_CSW" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-4 control-label">Sanctioned No.</label>
                                                                                <div class="col-sm-8 col-lg-8 controls">
                                                                                    <asp:TextBox ID="txtSnctNo_CSW" runat="server" CssClass="form-control required" required="required" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-4 control-label">Sanctioned Authority</label>
                                                                                <div class="col-sm-8 col-lg-8 controls">
                                                                                    <asp:TextBox ID="txtSnctAuth_CSW" runat="server" CssClass="form-control" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-4 control-label">Sanctioned Discharge (Cusec)</label>
                                                                                <div class="col-sm-8 col-lg-8 controls">
                                                                                    <asp:TextBox ID="txtSnctDschrg_CSW" runat="server" CssClass="form-control required decimalIntegerInput" required="required" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <div class="form-group">
                                                                                <label class="col-sm-4 col-lg-4 control-label">Attachment</label>
                                                                                <div class="col-sm-8 col-lg-8 controls">
                                                                                    <uc1:FileUploadControl runat="server" ID="fuc_CSW" Size="1" Mode="2" />
                                                                                    <asp:HyperLink ID="hlAtchmnt_CSW" Text="Attachment" runat="server" Visible="false" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnChangeCSW" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btn_Click" />
                                            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:HyperLink ID="hlback" runat="server" NavigateUrl="~/Modules/EWC/Industry.aspx?RestoreState=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openModal() {
            $("#Add").modal("show");
        };
    </script>
</asp:Content>
