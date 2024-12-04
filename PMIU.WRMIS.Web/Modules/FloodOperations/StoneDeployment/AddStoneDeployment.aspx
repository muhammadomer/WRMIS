<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStoneDeployment.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.StoneDeployment.AddStoneDeployment" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
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
    
    <asp:HiddenField ID="hdnFFPStonePositionID" runat="server" Value="0" />
    <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Stone Deployment</h3>
        </div>
        <div class="box-content">

            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    &nbsp;<asp:Label ID="lblRDLabel" runat="server" Text="RD" Font-Bold="True"></asp:Label>
                </div>

                <div class="col-md-4">
                    <asp:Label ID="lblInfrastructureType" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblInfrastructureName" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    &nbsp;<asp:Label ID="lblRD" runat="server"></asp:Label>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Quantity Approved in FFP ('000 cft)" Font-Bold="True"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="lblQtyApp" runat="server"></asp:Label>
                </div>
            </div>



            <%--<asp:Label ID="lblQtyApp" runat="server"></asp:Label>--%>

            <%--  <div class="col-md-4">
                    <asp:Label runat="server" Text="" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label runat="server" Text="" Font-Bold="True"></asp:Label>
                </div>--%>


            <%-- <div class="col-md-4">
                    <asp:Label ID="lbl_RD2" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_qtyDispos" runat="server"></asp:Label>
                </div>--%>

            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvStoneDeployment" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ID,FFPStonePositionID,DisposedDate,VehicleNumber,BuiltyNo,QtyOfStoneDisposed,Cost,CreatedBy,CreatedDate" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" ShowFooter="true"
                    OnRowCancelingEdit="gvStoneDeployment_RowCancelingEdit" OnRowUpdating="gvStoneDeployment_RowUpdating" OnRowEditing="gvStoneDeployment_RowEditing"
                    OnRowDeleting="gvStoneDeployment_RowDeleting" OnPageIndexChanging="gvStoneDeployment_PageIndexChanging"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" OnPageIndexChanged="gvStoneDeployment_PageIndexChanged" OnRowCommand="gvStoneDeployment_RowCommand" OnRowDataBound="gvStoneDeployment_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text='<%#Eval("DisposedDate", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div>

                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" Text='<%#Eval("DisposedDate", "{0:dd-MMM-yyyy}") %>' required="True" Width="100%" onfocus="this.value = this.value;"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />

                            <FooterTemplate>
                                <b>
                                    <asp:Label Text="Total" runat="server" Visible="true" />
                                </b>
                            </FooterTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vehicle Number">
                            <ItemTemplate>
                                <asp:Label ID="lblVehicleNumber" runat="server" CssClass="control-label" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVehicleNumber" runat="server" required="true" MaxLength="10" CssClass="form-control required" Text='<%# Eval("VehicleNumber") %>' Width="85%" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bilty Number">
                            <ItemTemplate>
                                <asp:Label ID="lblBuiltyeNumber" runat="server" CssClass="control-label" Text='<%# Eval("BuiltyNo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtBuiltyeNumber" runat="server" MaxLength="10" required="true" CssClass="required form-control" Text='<%# Eval("BuiltyNo") %>' Width="85%" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity ('000 cft)">
                            <ItemTemplate>
                                <asp:Label ID="lblQtyMaterial" runat="server" CssClass="control-label" Text='<%# Eval("QtyOfStoneDisposed") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQtyMaterial" runat="server" required="true" MaxLength="6" CssClass="integerInput required form-control" Text='<%# Eval("QtyOfStoneDisposed") %>' Width="100%" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2 text-right padding-right-number" />
                            <ItemStyle CssClass="text-right padding-right-number" />
                            <FooterStyle CssClass="text-right padding-right-number" />

                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalQuantity" Text="" CssClass="control-label" runat="server" Visible="true" />
                                </b>
                            </FooterTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Cost (Rs)">
                            <ItemTemplate>
                                <asp:Label ID="lblCost" runat="server" CssClass="control-label" Text='<%# Eval("Cost","{0:#,##0.##}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCost" runat="server" required="true" CssClass="integerInput required form-control" MaxLength="6" Text='<%# Eval("Cost") %>' Width="100%" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-1 text-right padding-right-number" />
                            <ItemStyle CssClass="text-right padding-right-number" />
                            <FooterStyle CssClass="text-right padding-right-number" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalCost" Text="" CssClass="control-label" runat="server" Visible="true" />
                                </b>
                            </FooterTemplate>

                        </asp:TemplateField>

                       <%-- <asp:TemplateField HeaderText="Attachments">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlAttachments" runat="server" ToolTip="Attachments" CssClass="btn btn-primary btn_24 viewimg" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/StoneDeployment/AttachmentSD.aspx?ID={0}") %>' Text="">
                                </asp:HyperLink>

                            </ItemTemplate>
                            <ItemStyle CssClass="text-center" />
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>--%>

                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                     <asp:Button ID="Button1" runat="server"  CssClass="btn btn-primary btn_32 attachment" ToolTip="Attachments" Enabled="false" />
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"  />
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                       <asp:HyperLink ID="hlAttachments" runat="server" ToolTip="Attachments" CssClass="btn btn-primary btn_24 attachment" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/StoneDeployment/AttachmentSD.aspx?ID={0}") %>' Text="">
                                </asp:HyperLink>
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
