<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" Language="C#" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddPlanDivSubDivDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.AddPlanDivSubDivDetail" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc" TagName="FileUploadControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="hName" runat="server">Add Rotational Program Detail</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <div class="table-responsive">
                            <asp:Table runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell CssClass="col-md-5">
                                        <asp:Label runat="server" Text="Rotational Program"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="col-md-4">
                                        <asp:Label ID="lblNamelbl" runat="server" Text="Name"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="col-md-3">
                                        <asp:Label runat="server" Text="Season"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblRotationalProgram" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                    </asp:TableCell>

                                    <asp:TableCell>
                                        <asp:Label ID="lblSeason" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Start Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Year"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="No. of Groups"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblGroups" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>

                                <asp:TableRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Closure Start Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Closure End Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label ID="lblProoflbl" runat="server" Text="View/Add Attachment"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblClosureStart" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblClosureEnd" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableHeaderCell>
                                        <asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />
                                        <uc:FileUploadControl runat="server" CssClass="CtrlClass0" required="false" ID="FileUploadControl" Size="1" />
                                    </asp:TableHeaderCell>
                                </asp:TableRow>

                            </asp:Table>
                        </div>



                        <div class="row" id="divClosure" runat="server" visible="false">
                            <div class="box-title">
                                <h4 runat="server">Closure Dates</h4>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Start Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureStartDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="End Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureEndDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <asp:Button ID="btnClosureSave" Text="Save Closure" CssClass="btn btn-primary" runat="server" OnClick="btnClosureSave_Click" ClientIDMode="Static" Style="margin-top: -3px;" />

                                </div>
                            </div>
                        </div>
                        <br />
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <h3 id="h1" runat="server">Entering Channels</h3>
                                <div class="table-responsive">
                                    <div id="divSPDrafts" runat="server">
                                        <asp:GridView ID="gvEnteringChannels" OnRowDataBound="gvEnteringChannels_RowDataBound" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                            ShowHeader="false" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Entering Channel Names">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbEntrngChnls" runat="server" AutoPostBack="true" OnCheckedChanged="cbEntrngChnls_CheckedChanged" />
                                                        <asp:Label ID="lblentrngChnlName" runat="server" Font-Bold="true" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-3" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <br />
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">

                                    <div style="float: left;">
                                        <asp:Repeater ID="rptrGroups" runat="server" OnItemDataBound="rptrGroups_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                <asp:TextBox ID="txtTotalSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                <br />
                                                <asp:Repeater ID="rptrSubGroups" runat="server" OnItemDataBound="rptrSubGroups_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="table-responsive" style="margin-left: 10px;">
                                                            <asp:CheckBox ID="cbSubGroup" runat="server" />
                                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Medium" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                            <asp:TextBox ID="txtSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                        </div>
                                                        <table cellpadding="0" border="0">
                                                            <asp:Repeater ID="rptrChannels" runat="server" OnItemDataBound="rptrChannels_ItemDataBound">

                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <div class="table-responsive" style="margin-left: 28px;">
                                                                            <td>
                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                                                <div style="float: left;">
                                                                                    <asp:CheckBox ID="cbChnl" runat="server" />
                                                                                    <asp:Label ID="lblChnlName" runat="server" CssClass="control-label" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div style="margin-left: 5px;">
                                                                                    <asp:TextBox ID="txtDischarge" Height="22px" Width="60px" runat="server" CssClass="form-control decimalInput" MaxLength="7" Text='<%# string.Format("{0:0.0}",Eval("DesignDischarge")) %>'></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </div>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <div style="float: left; margin-left: 30px;">
                                        <asp:Repeater ID="rptrGroup1" runat="server" OnItemDataBound="rptrGroup1_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                <asp:TextBox ID="txtTotalSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                <br />

                                                <asp:Repeater ID="rptrSubGroups" runat="server" OnItemDataBound="rptrSubGroups_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="table-responsive" style="margin-left: 10px;">
                                                            <asp:CheckBox ID="cbSubGroup" runat="server" />
                                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Medium" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                            <asp:TextBox ID="txtSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                        </div>

                                                        <table cellpadding="0" border="0">
                                                            <asp:Repeater ID="rptrChannels" runat="server" OnItemDataBound="rptrChannels_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <div class="table-responsive" style="margin-left: 28px;">
                                                                            <td>
                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                                                <div style="float: left;">
                                                                                    <asp:CheckBox ID="cbChnl" runat="server" />
                                                                                    <asp:Label ID="lblChnlName" runat="server" CssClass="control-label" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:TextBox ID="txtDischarge" Height="22px" Width="60px" runat="server" CssClass="form-control decimalInput" MaxLength="7" Text='<%# string.Format("{0:0.0}",Eval("DesignDischarge")) %>'></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </div>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>

                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <div style="float: left; margin-left: 30px;">
                                        <asp:Repeater ID="rptrGroup2" runat="server" OnItemDataBound="rptrGroup1_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                <asp:TextBox ID="txtTotalSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                <br />

                                                <asp:Repeater ID="rptrSubGroups" runat="server" OnItemDataBound="rptrSubGroups_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="table-responsive" style="margin-left: 10px;">
                                                            <asp:CheckBox ID="cbSubGroup" runat="server" />
                                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Medium" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                            <asp:TextBox ID="txtSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                        </div>
                                                        <table cellpadding="0" border="0">
                                                            <asp:Repeater ID="rptrChannels" runat="server" OnItemDataBound="rptrChannels_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <div class="table-responsive" style="margin-left: 28px;">
                                                                            <td>
                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                                                <div style="float: left;">
                                                                                    <asp:CheckBox ID="cbChnl" runat="server" />
                                                                                    <asp:Label ID="lblChnlName" runat="server" CssClass="control-label" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div style="margin-left: 5px;">
                                                                                    <asp:TextBox ID="txtDischarge" Height="22px" Width="60px" runat="server" CssClass="form-control decimalInput" MaxLength="7" Text='<%# string.Format("{0:0.0}",Eval("DesignDischarge")) %>'></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </div>
                                                                    </tr>
                                                                </ItemTemplate>

                                                            </asp:Repeater>
                                                        </table>

                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <div style="float: left; margin-left: 30px;">
                                        <asp:Repeater ID="rptrGroup3" runat="server" OnItemDataBound="rptrGroup1_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                <asp:TextBox ID="txtTotalSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                <br />
                                                <asp:Repeater ID="rptrSubGroups" runat="server" OnItemDataBound="rptrSubGroups_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="table-responsive" style="margin-left: 10px;">
                                                            <asp:CheckBox ID="cbSubGroup" runat="server" />
                                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Medium" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                            <asp:TextBox ID="txtSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                        </div>
                                                        <table cellpadding="0" border="0">
                                                            <asp:Repeater ID="rptrChannels" runat="server" OnItemDataBound="rptrChannels_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <div class="table-responsive" style="margin-left: 28px;">
                                                                            <td>
                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                                                <div style="float: left;">
                                                                                    <asp:CheckBox ID="cbChnl" runat="server" />
                                                                                    <asp:Label ID="lblChnlName" runat="server" CssClass="control-label" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div style="margin-left: 5px;">
                                                                                    <asp:TextBox ID="txtDischarge" Height="22px" Width="60px" runat="server" CssClass="form-control decimalInput" MaxLength="7" Text='<%# string.Format("{0:0.0}",Eval("DesignDischarge")) %>'></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </div>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <div style="float: left; margin-left: 30px;">
                                        <asp:Repeater ID="rptrGroup4" runat="server" OnItemDataBound="rptrGroup1_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                <asp:TextBox ID="txtTotalSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                <br />
                                                <asp:Repeater ID="rptrSubGroups" runat="server" OnItemDataBound="rptrSubGroups_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="table-responsive" style="margin-left: 10px;">
                                                            <asp:CheckBox ID="cbSubGroup" runat="server" />
                                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Medium" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                                            <asp:TextBox ID="txtSum" Height="22px" Width="60px" Enabled="false" runat="server" CssClass="form-control" Style="float: right;"></asp:TextBox>
                                                        </div>
                                                        <table cellpadding="0" border="0">
                                                            <asp:Repeater ID="rptrChannels" runat="server" OnItemDataBound="rptrChannels_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <div class="table-responsive" style="margin-left: 28px;">
                                                                            <td>
                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                                                <div style="float: left;">
                                                                                    <asp:CheckBox ID="cbChnl" runat="server" />
                                                                                    <asp:Label ID="lblChnlName" runat="server" CssClass="control-label" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div style="margin-left: 5px;">
                                                                                    <asp:TextBox ID="txtDischarge" Height="22px" Width="50px" runat="server" CssClass="form-control decimalInput" MaxLength="7" Text='<%# string.Format("{0:0.0}",Eval("DesignDischarge")) %>'></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </div>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Button ID="btnMapDiv" runat="server" Style="margin-left: 20px; margin-top: 15px;" Text="Save" OnClick="btnMapDiv_Click" OnClientClick="RemoveRequired()" />
                </div>

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-md-12">
                                <div class="table-responsive">

                                    <div class="table-big" style="border: 0px;">
                                        <asp:GridView ID="gvPreferanceDetail" runat="server" EmptyDataText="No record found" AutoGenerateColumns="False"
                                            DataKeyNames="StartDate,EndDate,GID1,GID2,GID3,GID4,GID5,SGID11,SGID12,SGID13,SGID21,SGID22,SGID23,
                                                SGID31,SGID32,SGID33,SGID41,SGID42,SGID43,SGID51,SGID52,SGID53"
                                            ShowHeaderWhenEmpty="True" OnRowDataBound="gvPreferanceDetail_RowDataBound" OnRowCommand="gvPreferanceDetail_RowCommand"
                                            OnRowCancelingEdit="gvPreferanceDetail_RowCancelingEdit" OnRowUpdating="gvPreferanceDetail_RowUpdating" OnRowEditing="gvPreferanceDetail_RowEditing"
                                            OnRowDeleting="gvPreferanceDetail_RowDeleting"
                                            OnRowCreated="gvPreferanceDetail_RowCreated" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="From Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div class="input-group date" data-date-viewmode="years" style="width: 150px;">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="date-picker form-control required" required="true" type="text"></asp:TextBox>
                                                            <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="To Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("EndDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div class="input-group date" data-date-viewmode="years" style="width: 150px;">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="date-picker form-control required" required="true" type="text" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                            <span id="spanToDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Preference1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblG1" runat="server" Text='<%# Eval("G1")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlG1" Width="50px" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlG1_SelectedIndexChanged"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG11" runat="server" Text='<%# Eval("SG11")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG11" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG12" runat="server" Text='<%# Eval("SG12")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG12" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG13" runat="server" Text='<%# Eval("SG13")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG13" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Preference2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblG2" runat="server" Text='<%# Eval("G2")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlG2" Width="50px" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlG2_SelectedIndexChanged"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG21" runat="server" Text='<%# Eval("SG21")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG21" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG22" runat="server" Text='<%# Eval("SG22")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG22" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG23" runat="server" Text='<%# Eval("SG23")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG23" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>




                                                <asp:TemplateField HeaderText="Preference3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblG3" runat="server" Text='<%# Eval("G3")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlG3" Width="50px" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlG3_SelectedIndexChanged"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG31" runat="server" Text='<%# Eval("SG31")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG31" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG32" runat="server" Text='<%# Eval("SG32")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG32" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG33" runat="server" Text='<%# Eval("SG33")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG33" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>




                                                <asp:TemplateField HeaderText="Preference4">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblG4" runat="server" Text='<%# Eval("G4")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlG4" Width="50px" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlG4_SelectedIndexChanged"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG41" runat="server" Text='<%# Eval("SG41")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG41" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG42" runat="server" Text='<%# Eval("SG42")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG42" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG43" runat="server" Text='<%# Eval("SG43")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG43" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>




                                                <asp:TemplateField HeaderText="Preference5">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblG5" runat="server" Text='<%# Eval("G5")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlG5" Width="50px" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlG5_SelectedIndexChanged"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG51" runat="server" Text='<%# Eval("SG51")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG51" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG52" runat="server" Text='<%# Eval("SG52")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG52" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SG3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSG53" runat="server" Text='<%# Eval("SG53")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlSG53" Width="50px" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Priority">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("Priority")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                                    <EditItemTemplate>
                                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="right">
                                                            <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" OnClientClick="RemoveRequired()"></asp:Button>
                                                            <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                                        </asp:Panel>
                                                    </EditItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="right">
                                                            <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:LinkButton>
                                                        </asp:Panel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="right">
                                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" OnClientClick="RemoveRequired()" />
                                                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:LinkButton>
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
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <%--<asp:Button ID="btnSendToSE" Visible="false" runat="server" CssClass="btn btn-primary" Text="Send To SE" OnClick="btnSendToSE_Click" />--%>
                <asp:LinkButton ID="btnSendToSE" Visible="false" runat="server" CssClass="btn btn-primary" Text="Send To SE" OnClick="btnSendToSE_Click"></asp:LinkButton>

            </div>
        </div>
    </div>


    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                }
            });
        };


        function ChannelTextboxEnable(ctlChkbx, ctlTextbox, SumTextBox, GroupSumTxtBox) {
            debugger;
            var chkbx = $('#' + ctlChkbx + '');
            var txtbox = $('#' + ctlTextbox + '');
            // alert($('#' + SumTextBox + '').val());
            if (chkbx.is(':checked')) {
                txtbox.prop('disabled', false);
                //  alert(parseFloat($('#' + SumTextBox + '').val()) + parseFloat($('#' + ctlTextbox + '').val()));
                $('#' + SumTextBox + '').val((parseFloat($('#' + SumTextBox + '').val()) + parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
                $('#' + GroupSumTxtBox + '').val((parseFloat($('#' + GroupSumTxtBox + '').val()) + parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
            }
            else {
                txtbox.prop('disabled', true);
                // alert(parseFloat($('#' + SumTextBox + '').val()) - parseFloat($('#' + ctlTextbox + '').val()));
                $('#' + SumTextBox + '').val((parseFloat($('#' + SumTextBox + '').val()) - parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
                $('#' + GroupSumTxtBox + '').val((parseFloat($('#' + GroupSumTxtBox + '').val()) - parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
            }
        }

        function UpdateSum(ctlChkbx, ctlTextbox, OldValue, SumTextBox, GroupSumTxtBox) {
            debugger;
            var chkbx = $('#' + ctlChkbx + '');
            var txtbox = $('#' + ctlTextbox + '');
            var vv = txtbox[0].attributes[6].value.split(',');
            var finl = vv[0] + "," + vv[1] + ",\"'" + $('#' + ctlTextbox + '').val() + "'\"," + vv[3] + "," + vv[4];
            var va = txtbox[0].attributes[6].value.split(',')[2].split('\'');
            OldValue = va[1];
            txtbox[0].attributes[6].value = finl;

            if (chkbx.is(':checked')) {
                txtbox.prop('disabled', false);
                //  alert(parseFloat($('#' + SumTextBox + '').val()) + parseFloat($('#' + ctlTextbox + '').val()));
                $('#' + SumTextBox + '').val(((parseFloat($('#' + SumTextBox + '').val()) - parseFloat(OldValue)) + parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
                $('#' + GroupSumTxtBox + '').val(((parseFloat($('#' + GroupSumTxtBox + '').val()) - parseFloat(OldValue)) + parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
            }
            else {
                txtbox.prop('disabled', true);
                // alert(parseFloat($('#' + SumTextBox + '').val()) - parseFloat($('#' + ctlTextbox + '').val()));
                $('#' + SumTextBox + '').val(((parseFloat($('#' + SumTextBox + '').val()) - parseFloat(OldValue)) - parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
                $('#' + GroupSumTxtBox + '').val(((parseFloat($('#' + GroupSumTxtBox + '').val()) - parseFloat(OldValue)) - parseFloat($('#' + ctlTextbox + '').val())).toFixed(2));
            }
        }

        function RemoveRequired() {
            $('.CtrlClass0').removeAttr("required");
        }

        $(function () {
            $('.CtrlClass0').removeAttr("required");
        });

        $("#btnClosureSave").click(function () {
            debugger;
            var FromDate = $("#txtClosureStartDate").val();
            var ToDate = $("#txtClosureEndDate").val();

            if (FromDate == "" || ToDate == "") {
                alert("Select both closure start and end date");
                return false;
            }
            else if (new Date(FromDate) > new Date(ToDate)) {
                alert("Closure Start date must be less than End date");
                return false;
            }
            else if (new Date(FromDate) < new Date($("#MainContent_lblStartDate").text()) || new Date(ToDate) < new Date($("#MainContent_lblStartDate").text())) {
                alert("Closure Start and End dates must be greater than Program Start date");
                return false;
            }
                //else if (FromDate < $("#MainContent_lblStartDate").text() || ToDate <= $("#MainContent_lblStartDate").text()) {
                //    alert("Closure Start and End dates must be greater than Program Start date");
                //    return false;
                //}
            else {
                return true;
                $('.CtrlClass0').removeAttr("required");
            }
        });

    </script>
</asp:Content>
