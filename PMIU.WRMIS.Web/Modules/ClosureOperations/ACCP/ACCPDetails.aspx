<%@ Page AutoEventWireup="true" CodeBehind="ACCPDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.ACCP.ACCPDetails" Language="C#" MasterPageFile="~/Site.Master" Title="" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<%@ Register TagPrefix="ucACCP" TagName="AccpTitleYear" Src="~/Modules/ClosureOperations/UserControls/ACCP.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Annual Canal Closure Programme Details </h3>
                    <asp:HiddenField runat="server" ID="hfAccpID" Value="0" />
                </div>

                <div style="padding:10px;background: #fff;">
                    <ucACCP:AccpTitleYear runat="server" ID="ACCPID" />
                    <asp:Panel ID="pnlACCPDetails" runat="server" Visible="true" ClientIDMode="Static">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box" style="margin-bottom: 1px;">
                                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                                        <h4 style="color: #fff;">Mangla Command</h4>
                                        <div class="box-tool" style="top: 7px;">
                                            <a data-action="collapse" href="divMC"><i id="iconManglaCommand" runat="server" class="fa fa-chevron-down"></i></a>
                                        </div>
                                    </div>
                                    <div id="divMC" runat="server" class="box-content" style="display: none;">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvMC" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                                AllowPaging="false" OnRowDataBound="gvMC_RowDataBound" OnRowCreated="gvMC_RowCreated" OnRowCommand="gvMC_RowCommand" OnRowUpdating="gvMC_RowUpdating"
                                                OnRowEditing="gvMC_RowEditing" OnRowDeleting="gvMC_RowDeleting" OnRowCancelingEdit="gvMC_RowCancelingEdit" DataKeyNames="ID,ChannelCmdType,ACCPID,MainCanalID">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Site" Visible="false">
                                                        <ItemTemplate>
                                                            <itemtemplate>
                                                            <asp:Label ID="lblMCID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                                            <asp:Label ID="lblMCCommandType" runat="server" CssClass="control-label" Text='<%# Eval("ChannelCmdType") %>' Visible="false" />
                                                            <asp:Label ID="lblMCAccpID" runat="server" CssClass="control-label" Text='<%# Eval("ACCPID") %>' Visible="false" />
                                                            <asp:Label ID="lblMCMainCanalID" runat="server" CssClass="control-label" Text='<%# Eval("MainCanalID") %>' Visible="false" />
                                                        </itemtemplate>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main Canal Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMCMainCanalName" runat="server" CssClass="control-label" Text='<%# Eval("MainCanalName") %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlMCChannel" runat="server" TabIndex="3" required="required" CssClass=" rquired form-control">
                                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMCFromDate" runat="server" CssClass="control-label" Text='<%# Eval("FromDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtMCFromDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("FromDate", "{0:dd-MMM-yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMCToDate" runat="server" CssClass="control-label" Text='<%# Eval("ToDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtMcToDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("ToDate", "{0:dd-MMM-yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnAddACCPMCDetail" runat="server" Text="" CommandName="AddACCPDetail" Enabled="<%# base.CanAdd%>" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlActionItemCategory" runat="server" HorizontalAlign="Center">


                                                                <asp:HyperLink ID="btnMCExclude" runat="server" Text="" CommandName="Exclude" CssClass="btn btn-primary btn_24 Exclude" Style="height: 24px; width: 29px;" ToolTip="Exclude Channels"
                                                                    NavigateUrl='<%# String.Format("~/Modules/ClosureOperations/ACCP/ACCPExcludedChannels.aspx?ACCPID={0}&MainCanalID={1}&DetailID={2}", Eval("ACCPID"), Eval("MainCanalID"), Eval("ID")) %>'
                                                                    formnovalidate="formnovalidate" />
                                                                <asp:Button ID="btnMCEdit" runat="server" Text="" CommandName="Edit" Enabled="<%# base.CanEdit%>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                                <asp:Button ID="btnMCDelete" runat="server" Text="" CommandName="Delete" Enabled="<%# base.CanDelete%>" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Panel ID="pnlEditActionItemCategory" runat="server" HorizontalAlign="Center">
                                                                <asp:Button runat="server" ID="btnSaveItemCategory" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                                <asp:Button ID="btnCancelItemCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="form-horizontal">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="fnc-btn">
                                                        <%--<asp:Button ID="btnIndusDamSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnIndusDamSave_Click" />
                                        <asp:LinkButton ID="lbtnIndusDamCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnIndusDamCancel_Click" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box" style="margin-bottom: 1px;">
                                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                                        <h4 style="color: #fff;">Tarbela Command</h4>
                                        <div class="box-tool" style="top: 7px;">
                                            <a data-action="collapse" href="divTBC"><i id="iconTerbelaCommand" runat="server" class="fa fa-chevron-down"></i></a>
                                        </div>
                                    </div>
                                    <div id="divTBC" runat="server" class="box-content" style="display: none;">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvTC" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                ShowHeaderWhenEmpty="True" OnRowEditing="gvTC_RowEditing" OnRowCancelingEdit="gvTC_RowCancelingEdit" OnRowUpdating="gvTC_RowUpdating"
                                                OnRowCommand="gvTC_RowCommand" CssClass="table header" OnRowCreated="gvTC_RowCreated" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                                AllowPaging="false" OnRowDeleting="gvTC_RowDeleting" OnRowDataBound="gvTC_RowDataBound" DataKeyNames="ID,ChannelCmdType,ACCPID,MainCanalID">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Site" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                                            <asp:Label ID="lblCommandType" runat="server" CssClass="control-label" Text='<%# Eval("ChannelCmdType") %>' Visible="false" />
                                                            <asp:Label ID="lblAccpID" runat="server" CssClass="control-label" Text='<%# Eval("ACCPID") %>' Visible="false" />
                                                            <asp:Label ID="lblMainCanalID" runat="server" CssClass="control-label" Text='<%# Eval("MainCanalID") %>' Visible="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main Canal Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTCMainCanalName" runat="server" CssClass="control-label" Text='<%# Eval("MainCanalName") %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlTCChannel" runat="server" TabIndex="3" required="required" CssClass=" rquired form-control">
                                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTCFromDate" runat="server" CssClass="control-label" Text='<%# Eval("FromDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtTCFromDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("FromDate", "{0:MMM dd,yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTCToDate" runat="server" CssClass="control-label" Text='<%# Eval("ToDate", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtTcToDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("ToDate", "{0:MMM dd,yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnAddACCPOrder" runat="server" Text="" CommandName="AddACCPDetail" Enabled="<%# base.CanAdd%>" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlActionACCPDetail" runat="server" HorizontalAlign="Center">
                                                                <asp:HyperLink ID="btnExclude" runat="server" Text="" CommandName="Exclude" Enabled="<%# base.CanEdit && base.CanEdit%>" CssClass="btn btn-primary btn_24 Exclude" Style="height: 24px; width: 29px;" ToolTip="Exclude"
                                                                    NavigateUrl='<%# String.Format("~/Modules/ClosureOperations/ACCP/ACCPExcludedChannels.aspx?ACCPID={0}&MainCanalID={1}&DetailID={2}", Eval("ACCPID"), Eval("MainCanalID"), Eval("ID")) %>'
                                                                    formnovalidate="formnovalidate" />

                                                                <asp:Button ID="btnEditACCDetail" runat="server" Text="" CommandName="Edit" Enabled="<%# base.CanEdit%>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                                <asp:Button ID="btnDeleteACCDetail" runat="server" Text="" CommandName="Delete" Enabled="<%# base.CanDelete%>" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Panel ID="pnlEditACCDetail" runat="server" HorizontalAlign="Center">
                                                                <asp:Button runat="server" ID="btnSaveItemCategory" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                                <asp:Button ID="btnCancelItemCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="form-horizontal">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="fnc-btn">
                                                        <%-- <asp:Button ID="btnPunjabIndusBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnPunjabIndusBarragesSave_Click" />
                                        <asp:LinkButton ID="lbtnPunjabIndusBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnPunjabIndusBarragesCancel_Click" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />

        <%--onclick="history.go(-1);return false;"--%>
    </div>

</asp:Content>
