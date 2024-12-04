<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublicRepresentative.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.PublicRepresentative" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucInfrastructureDetails" TagName="InfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/InfrastructureDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnProtectionInfrastructureID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Public Representatives on Protection Infrastructure</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucInfrastructureDetails:InfrastructureDetail runat="server" ID="InfrastructureDetail" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvInfrastructureRepresentative" runat="server" DataKeyNames="ID,Name,Details,ContactNumber,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvInfrastructureRepresentative_RowCommand" OnRowDataBound="gvInfrastructureRepresentative_RowDataBound"
                            OnRowEditing="gvInfrastructureRepresentative_RowEditing" OnRowCancelingEdit="gvInfrastructureRepresentative_RowCancelingEdit"
                            OnRowUpdating="gvInfrastructureRepresentative_RowUpdating"
                            OnRowDeleting="gvInfrastructureRepresentative_RowDeleting" OnPageIndexChanging="gvInfrastructureRepresentative_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" pattern="[a-zA-Z\-:].{3,90}" MaxLength="90" class="required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Detail">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetail" runat="server" Text='<%# Eval("Details") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDetail" runat="server" MaxLength="80" class="form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNumber" runat="server" Text='<%# Eval("ContactNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContactNumber" runat="server" required="required" pattern="^[0-9]{11,12}$" MaxLength="11" class="integerInput required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddInfrastructureRepresentative" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddInfrastructureRepresentative" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditInfrastructureRepresentative" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteInfrastructureRepresentative" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveInfrastructureRepresentative" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelInfrastructureRepresentative" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
            <br />
            <%--onclick="history.go(-1);return false;"--%>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>

    <script type="text/javascript">
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

        function ConfirmIstoRecalculate() {
            var result = confirm('Hello');
            if (result) {
                //click ok button
                alert("testin true");
            }
            else {
                //click cancel button
                alert("testing false");
            }
        }
    </script>
</asp:Content>

