<%@ Page Title="ViewWorksItems" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ViewWorkItems.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.ViewWorkItems" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="ucViewWorksControl" TagName="ViewWorksUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">

   .padding-right-number {
            padding-right:35px !important;
   }
    
    @media only screen and (min-width: 1300px) {
       .padding-right-number {
            padding-right:45px !important;
   }
    }
    @media only screen and (min-width: 1400px) {
        .gridReachStartingRDs {
            width: 15%;
        }
    }
    @media only screen and (min-width: 1500px) {
        .gridReachStartingRDs {
            width: 14%;
        }
    }
    @media only screen and (min-width: 1400px) {
        .padding-right-number {
            padding-right:75px !important;
   }
    }
</style>
    <div class="box">
        <div class="box-title">
            <h3>Work Items</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                 <ucViewWorksControl:ViewWorksUserControl runat="server" ID="ViewWorksUserControl" />
</div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvWorkItems" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWorkItemID" runat="server" Text='<%# Eval("WorkItemID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemDescription" runat="server" Text='<%#Eval("ItemDescription") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Technical Sanctioned Rate (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTechnicalSanctionedRate" runat="server" Text='<%# Eval("TechnicalSanctionedRate","{0:n0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                 <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                                <ItemStyle CssClass="text-right padding-right-number" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sanctioned Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSanctionedQuantity" runat="server" Text='<%# Eval("SanctionedQuantity","{0:n0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                                <ItemStyle CssClass="text-right padding-right-number" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Technical Sanctioned Amount (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTechnicalSanctionedAmount" runat="server" Text='<%# Eval("TechnicalSanctionedAmount","{0:n0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<HeaderStyle CssClass="col-md-2" />--%>
                                                <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                                                <ItemStyle CssClass="text-right padding-right-number" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

              <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnWorkSourceID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" Value="0" />

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
       
    </script>
</asp:Content>
