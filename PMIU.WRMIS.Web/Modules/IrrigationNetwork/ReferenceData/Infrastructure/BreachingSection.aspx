<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BreachingSection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.BreachingSection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">

   .gridReachStartingRDs {
            width: 18%;
   }
    
    @media only screen and (min-width: 1279px) {
        .gridReachStartingRDs {
            width: 18%;
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
    @media only screen and (min-width: 1650px) {
        .gridReachStartingRDs {
            width: 12%;
        }
    }
</style>
    <asp:HiddenField ID="hdnProtectioninfrastructure" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Breaching Section of Protection Infrastructure</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="udpBreachingSection" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvBreachingSection" runat="server" DataKeyNames="ID,StartingRDTotal,EndingRDTotal,Rows,Liners,StartingRD,EndingRD,CreatedBy,CreatedDate" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                    OnRowCommand="gvBreachingSection_RowCommand" OnRowCancelingEdit="gvBreachingSection_RowCancelingEdit" OnRowEditing="gvBreachingSection_RowEditing"
                                    OnRowDataBound="gvBreachingSection_RowDataBound" OnRowDeleting="gvBreachingSection_RowDeleting" OnRowUpdating="gvBreachingSection_RowUpdating"
                                    OnPageIndexChanging="gvBreachingSection_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Reach Starting  R.Ds. (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartingRD" runat="server" Text='<%#Eval("StartingRD") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>

                                                <asp:Panel ID="pnlStartingRD" runat="server">
                                                    <asp:TextBox ID="txtStartingRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                    +
                                                <asp:TextBox ID="txtStartingRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="text-right gridReachStartingRDs" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reach Ending  R.Ds. (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndingRD" runat="server" Text='<%# Eval("EndingRD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEndingRD" runat="server">
                                                    <asp:TextBox ID="txtEndingRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                                    +
                                                <asp:TextBox ID="txtEndingRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 35%; display: inline;"></asp:TextBox>
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="text-right" Width="18%" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. of Rows">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRows" runat="server" Text='<%# Eval("Rows") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNoOfRows" runat="server" required="required" class="integerInput required form-control" Style="max-width: 60%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="text-right" Width="15%" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. of Liners*">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLiners" runat="server" Text='<%# Eval("Liners") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNoOfLiners" runat="server" required="required" class="integerInput required form-control" Style="max-width: 60%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="text-right" Width="15%" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Explosive Material/Accessories">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlExplosiveDetail" runat="server" ToolTip="Breaching Section" CssClass="btn btn-primary" NavigateUrl='<%# Eval("ID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/ExplosiveMaterial.aspx?BreachingSectionID={0}") %>' Text="Explosive Detail" CommandName="ExplosiveDetail"></asp:HyperLink>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:HyperLink ID="hlExplosiveDetail" runat="server" ToolTip="Breaching Section" CssClass="btn btn-primary" NavigateUrl='<%# Eval("ID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/ExplosiveMaterial.aspx?BreachingSectionID={0}") %>' Text="Explosive Detail" CommandName="ExplosiveDetail"></asp:HyperLink>

                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="text-center" Width="30%" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAddBreachingSection" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnAddBreachingSection" Visible="<%# base.CanAdd %>" runat="server" Text="" CommandName="AddBreachingSection" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlActionBreachingSection" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEditBreachingSection" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                    <asp:Button ID="lbtnDeleteBreachingSection" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="<%# base.CanDelete %>"
                                                        OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditChannelReach" runat="server" HorizontalAlign="Center">
                                                    <asp:Button runat="server" ID="btnSaveChannelReach" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                    <asp:Button ID="btnCancelChannelReach" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="4%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
    </script>
</asp:Content>
