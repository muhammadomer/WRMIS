<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorksEmergencyPurchases.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.WorksEmergencyPurchases" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnEmergencyPurchaseID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTotalRD" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureType" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Flood Fighting Works</h3>
                </div>

                <div class="box-content">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Division" Font-Bold="true"></asp:Label>

                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label2" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label4" runat="server" Text="Camp site" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_division" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_infrastructure" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_camp_site" runat="server"></asp:Label>
                        </div>

                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="udpBreachingSection" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gv_FloodFighting" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,EmergencyPurchaseID,NatureOfWorkID,RDtotal,Description,CreatedDate,CreatedBy" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" OnRowCommand="gv_FloodFighting_RowCommand"
                                    OnRowCancelingEdit="gv_FloodFighting_RowCancelingEdit" OnRowUpdating="gv_FloodFighting_RowUpdating" OnRowEditing="gv_FloodFighting_RowEditing"
                                    OnRowDeleting="gv_FloodFighting_RowDeleting" OnPageIndexChanging="gv_FloodFighting_PageIndexChanging"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" OnPageIndexChanged="gv_FloodFighting_PageIndexChanged" OnRowDataBound="gv_FloodFighting_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            </ItemTemplate>
                                     <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nature of work">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNature_work" runat="server" Text='<%# Eval("NatureOfWorkName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlNatureOfWork" runat="server" required="required" CssClass="required form-control" onfocus="this.value = this.value;" Style="max-width: 90%"></asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="RD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRD" runat="server" CssClass="control-label" Text='<%# Eval("RD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                                +
                                        <asp:TextBox ID="txtRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle CssClass="text-right" ></ItemStyle>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
          

                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="256" pattern="^[a-zA-Z0-9 ]+$" placeholder="Enter Description" Text='<%# Eval("Description") %>' Width="100%" />
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                        </asp:TemplateField>
                                  <%--      <asp:TemplateField HeaderText="Disposal Detail">
                                            <ItemTemplate>

                                                <asp:HyperLink ID="hlDisposalDetails" runat="server" ToolTip="Details" CssClass="btn btn-primary" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/EmergencyPurchases/DisposalEmergencyPurchases.aspx?EPWorkID={0}") %>' Text="Details">
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-center" />
                                            <ItemStyle CssClass="text-center"></ItemStyle>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="Button1" runat="server"  CssClass="btn btn-primary btn_32 details" ToolTip="Details" Enabled="false" />
                                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Panel ID="PanelWorks" runat="server" HorizontalAlign="center">
                                                    <asp:Button ID="AddWorks" runat="server" Text="" CommandName="Add" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" Visible="<%# base.CanAdd %>" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                     <asp:HyperLink ID="hlDisposalDetails" runat="server" ToolTip="Details" CssClass="btn btn-primary btn_32 details" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/EmergencyPurchases/DisposalEmergencyPurchases.aspx?EPWorkID={0}") %>' >
                                                </asp:HyperLink>
                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Enabled="<%# base.CanEdit %>" />
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Enabled="<%# base.CanDelete %>" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="form-group">
                        <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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
