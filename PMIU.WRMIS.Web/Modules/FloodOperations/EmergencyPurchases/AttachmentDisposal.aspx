<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AttachmentDisposal.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.AttachmentDisposal" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnMaterialDisposalID" runat="server" Value="0" />
    <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Material Disposal Details (Infrastructure Name)</h3>
        </div>
        <div class="box-content">

            <div class="row">
                <div class="col-md-4">
                    <asp:Label ID="Label1" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label2" runat="server" Text="Vehicle Number" Font-Bold="true" CssClass="text-right"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label4" runat="server" Text="Bilty Number" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_Date" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_VehicleNumber" runat="server"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lbl_BuiltyNumber" runat="server"></asp:Label>
                </div>

            </div>
            <br />
            <div class="table-responsive">
                  <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
                <asp:GridView ID="gv_M_Disposal_Attachment" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,MaterialDisposalID,FileName,FileURL,CreatedBy,CreatedDate" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" OnRowCommand="gv_M_Disposal_Attachment_RowCommand"
                    OnRowCancelingEdit="gv_M_Disposal_Attachment_RowCancelingEdit" OnRowUpdating="gv_M_Disposal_Attachment_RowUpdating" OnRowEditing="gv_M_Disposal_Attachment_RowEditing"
                    OnRowDeleting="gv_M_Disposal_Attachment_RowDeleting" OnPageIndexChanging="gv_M_Disposal_Attachment_PageIndexChanging"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" OnPageIndexChanged="gv_M_Disposal_Attachment_PageIndexChanged" OnRowDataBound="gv_M_Disposal_Attachment_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Picture">
                            <ItemTemplate>
                                   <asp:LinkButton ID="lnkimage" runat="server" CommandName="image" Text='<%# Eval("FileName")%>'>
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
                       
                             <asp:AsyncPostBackTrigger ControlID="gv_M_Disposal_Attachment" EventName="PageIndexChanging" />
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
                                     <asp:AsyncPostBackTrigger ControlID="gv_M_Disposal_Attachment"  EventName="RowCommand" />  
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
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
