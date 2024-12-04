<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AttachmentSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.StoneDeployment.AttachmentSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnSDID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Attachments of Stone Deployment</h3>
        </div>
        <div class="box-content">

            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="Label4" runat="server" Text="Division" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label1" runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>

                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label2" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_Division" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_infra_type" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_infrastructure" runat="server"></asp:Label>
                </div>


            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    &nbsp;<asp:Label ID="lbl_D" runat="server" Text="RD" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_" runat="server" Text="Quantity Approved in FFP ('000 cft)" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label5" runat="server" Text="Quantity Disposed till now in ('000 cft)" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    &nbsp;<asp:Label ID="lbl_RD" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_qtyApp" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_qtyDispos" runat="server"></asp:Label>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="Label3" runat="server" Text="Disposed Date" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label6" runat="server" Text="Vehicle Number" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label7" runat="server" Text="Bilty Number" Font-Bold="True"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_disposDate" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_vehicleNumber" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_builtyNumber" runat="server"></asp:Label>
                </div>
            </div>



            <br />
            <br />
            <div class="table-responsive">
                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:GridView ID="gv_Attachment" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,SDID,ImageURL,CreatedBy,CreatedDate" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowCommand="gv_Attachment_RowCommand"
                            OnRowCancelingEdit="gv_Attachment_RowCancelingEdit" OnRowUpdating="gv_Attachment_RowUpdating" OnRowEditing="gv_Attachment_RowEditing"
                            OnRowDeleting="gv_Attachment_RowDeleting" OnPageIndexChanging="gv_Attachment_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" OnPageIndexChanged="gv_Attachment_PageIndexChanged" OnRowDataBound="gv_Attachment_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachments">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkimage" runat="server" CommandName="image" Text='<%# Eval("ImageURL")%>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:FileUpload runat="server" ID="fileupdloadID" required="true" CssClass="form-control" Style="max-width: 80%;" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-4" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                            <%--<asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />--%>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%--  <Triggers>
                       
                             <asp:AsyncPostBackTrigger ControlID="gv_Attachment" EventName="PageIndexChanging" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
            <!-- Start Of Image -->
            <div id="viewimage" class="modal fade">
                <div class="modal-dialog" style="max-height: 519px; max-width: 793.398px;">
                    <div class="modal-content" style="width: 730px">
                        <div class="modal-body" style="height: 500px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <center>
                                        <asp:Image ID="imgSDImage" runat="server" style="display: block; max-width: 100%; height:auto; max-height:500px;"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gv_Attachment" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
                <!-- END Of Image -->
            </div>
        </div>
    </div>
    <script type="text/ecmascript" src="../../Scripts/jquery-1.10.2.min.js"></script>

    <link href="../../Design/css/ui.jqgrid-bootstrap.css" rel="stylesheet" />

</asp:Content>
