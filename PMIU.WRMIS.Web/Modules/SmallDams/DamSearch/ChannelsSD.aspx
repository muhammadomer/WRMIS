<%@ Page Title="ChannelsSD" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ChannelsSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.ChannelsSD" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDamNameType" TagName="DamNameType" Src="~/Modules/SmallDams/Controls/DamNameType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSmallDamID" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Small Dam-Channels</h3>
        </div>
        <div class="box-content">
            <ucDamNameType:DamNameType runat="server" ID="DamNameType" />
            <div class="form-horizontal">
                <div class="table-responsive">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvChannels" runat="server" DataKeyNames="ID,ChannelName,ParentType,ParentName,ChannelCode,ChannelCapacity,OffTakingRD,OffTakingSide,TailRD,DesignedCCA,CreatedBy,CreatedDate"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                                OnRowCommand="gvChannels_RowCommand" OnRowDataBound="gvChannels_RowDataBound"
                                OnRowEditing="gvChannels_RowEditing" OnRowCancelingEdit="gvChannels_RowCancelingEdit"
                                OnRowUpdating="gvChannels_RowUpdating"
                                OnRowDeleting="gvChannels_RowDeleting" OnPageIndexChanging="gvChannels_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Channel ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelID" runat="server" Text='<%#Eval("ID")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Parent Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParentType" runat="server" CssClass="control-label" Text='<%# Eval("ParentType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Parent Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParentName" runat="server" CssClass="control-label" Text='<%# Eval("ParentName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Channel Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelCode" runat="server" CssClass="control-label" Text='<%# Eval("ChannelCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Capacity (Cusec)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelCapacity" runat="server" CssClass="control-label" Text='<%# Eval("ChannelCapacity","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" Width="13%"/>
                                        <ItemStyle CssClass="integerInput text-right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Off-Taking RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffTakingRD" runat="server" CssClass="control-label" Text='<%# Eval("OffTakingRD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1 " />
                                        <ItemStyle CssClass="integerInput text-right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Off-Taking Side">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffTakingSide" runat="server" CssClass="control-label" Text='<%# Eval("OffTakingSide") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1 " />

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tail RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTailRD" runat="server" CssClass="control-label" Text='<%# Eval("TailRD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" Width="6%"/>
                                        <ItemStyle CssClass="integerInput text-right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CCA (Acres)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignedCCA" runat="server" CssClass="control-label" Text='<%# Eval("DesignedCCA","{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" Width="9%" />
                                        <ItemStyle CssClass=" integerInput text-right" />

                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAddChannels" runat="server" HorizontalAlign="Center">
                                                <asp:HyperLink ID="hlAddChannels" runat="server" ToolTip="Add" CssClass="btn btn-success btn_add plus" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/ChannelInformationSD.aspx?SmallDamID={0}&ChannelID=0",hdnSmallDamID.Value) %>' Text=""></asp:HyperLink>
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionChannels" runat="server" HorizontalAlign="Center">
                                                <asp:HyperLink ID="hlVillagesBen" runat="server" ToolTip="Villages Benefitted" CssClass="btn btn-primary btn_24 villages" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/VillagesBenefittedSD.aspx?SmallDamID={0}&ChannelID={1}",hdnSmallDamID.Value,Eval("ID")) %>' Text=""></asp:HyperLink>
                                                <asp:HyperLink ID="hlEditChannels" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# String.Format("~/Modules/SmallDams/DamSearch/ChannelInformationSD.aspx?SmallDamID={0}&ChannelID={1}",hdnSmallDamID.Value,Eval("ID")) %>' Text=""></asp:HyperLink>
                                                <asp:Button ID="btnDeleteChannels" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />

                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
