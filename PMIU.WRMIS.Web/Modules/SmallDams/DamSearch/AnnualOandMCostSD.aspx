<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnnualOandMCostSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.DamSearch.AnnualOandMCostSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucDamInfo" TagName="DamInfo" Src="~/Modules/SmallDams/Controls/DamInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSmallDamID" runat="server" Value="0" />
    

    <div class="box">
        <div class="box-title">
            <h3>Annual Operation and Maintenance Cost</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucDamInfo:DamInfo runat="server" ID="DamInfo" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvOMCost" runat="server" DataKeyNames="ID,ApprovedDate,Cost,Description,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvOMCost_RowCommand" OnRowDataBound="gvOMCost_RowDataBound"
                            OnRowEditing="gvOMCost_RowEditing" OnRowCancelingEdit="gvOMCost_RowCancelingEdit"
                            OnRowUpdating="gvOMCost_RowUpdating"
                            OnRowDeleting="gvOMCost_RowDeleting" OnPageIndexChanging="gvOMCost_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Village ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Approved Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedDate" CssClass="control-label" runat="server" Text='<%# Eval("ApprovedDate","{0:dd-MMM-yyyy}") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtApprovedDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("ApprovedDate","{0:dd-MMM-yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="col-md-2 text-center"/>
                                    <HeaderStyle CssClass="text-center"/>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cost (Rs.)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCost" runat="server" CssClass="control-label" Text='<%# Eval("Cost","{0:n0}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCost" runat="server" CssClass="form-control text-right" MaxLength="15" pattern="^[0-9]+$" Style=" display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-4 text-center" width="20%"/>
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="250" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="text-left"/>
                                    
                                </asp:TemplateField>


                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddOMCost" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddOMCost" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddOMCost" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionOMCost" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditOMCost" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteOMCost" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionOMCost" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveOMCost" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelOMCost" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--onclick="history.go(-1);return false;"--%>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
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
                    ClearDateField();
                }
            });
        };
    </script>
</asp:Content>
