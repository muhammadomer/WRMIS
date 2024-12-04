<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/Modules/WaterLosses/ReachLagTime.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterLosses.ReachLagTime" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Lag Time</h3>                    
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvLagTime" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found" GridLines="None"
                            ShowHeaderWhenEmpty="True" OnRowEditing="gvLagTime_RowEditing" OnRowCancelingEdit ="gvLagTime_RowCancelingEdit"
                            OnRowUpdating="gvLagTime_RowUpdating" OnRowCommand="gvLagTime_RowCommand" CssClass="table header" BorderWidth="0px" CellSpacing="-1"  >
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="River">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommand" runat="server" CssClass="control-label" Text='<%# Eval("Command") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reaches">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate> 
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Nov-Apr">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNov" runat="server" CssClass="text-right control-label" Text='<%# Eval("NOV_APRIL") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNovApr" runat="server" required="required"  MaxLength="1" CssClass="form-control required text-right" Text='<%# Eval("NOV_APRIL") %>'  Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May-Jun">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMay" runat="server" CssClass="control-label text-right" Text='<%# Eval("MAY_JUNE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtMayJun" runat="server" required="required"  MaxLength="1" CssClass="text-right form-control required" Text='<%# Eval("MAY_JUNE") %>'  Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField> 
                               <asp:TemplateField HeaderText="Jul-Aug">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJul" runat="server" CssClass="control-label text-right" Text='<%# Eval("JUL_AUG") %>'></asp:Label>
                                    </ItemTemplate>
                                   <EditItemTemplate>
                                        <asp:TextBox ID="txtJulAug" runat="server" required="required"  MaxLength="1" CssClass="form-control required text-right" Text='<%# Eval("JUL_AUG") %>'  Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField> 
                               <asp:TemplateField HeaderText="Sep-Oct">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSep" runat="server" CssClass="control-label text-right" Text='<%# Eval("SEP_OCT") %>'></asp:Label>
                                    </ItemTemplate>
                                   <EditItemTemplate>
                                        <asp:TextBox  ID="txtSepOct" runat="server" required="required" MaxLength="1" CssClass="form-control required text-right" Text='<%# Eval("SEP_OCT") %>'  Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                            <asp:Button ID="btnHistory" data-toggle="modal" runat="server" Text="" CommandName="History" CssClass="btn btn-primary btn_24 audit" ToolTip="History" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns> 
                        </asp:GridView>
                    </div>

               <%-- <div class="table-responsive">    --%>     
                    <div id="history" class="modal fade">
                       <div class="modal-dialog table-responsive" style="max-height: 419px; max-width: 893.398px;">
                            <div class="modal-content" style="width: 830px">
                                <div class="modal-body" >
                                   <div class="box">
                                        <div class="box-title"> <h3>Lag Time History</h3> </div>
                                        <div class="box-content " >
                                            <div class="table-responsive" style="height:400px; overflow-y:auto;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server"  >
                                        <ContentTemplate>
                                            <asp:GridView ID="gvHistory" runat="server"
                                                CssClass="table header" AutoGenerateColumns="False" GridLines="None" 
                                                EmptyDataText="No Record Found" AllowSorting="false" ShowHeaderWhenEmpty="true" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("SrNo") %>'  />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Nov-Apr">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNov" runat="server" CssClass="control-label" Text='<%# Eval("NOV_APRIL") %>'></asp:Label>
                                                    </ItemTemplate> 
                                                </asp:TemplateField>  
                                                <asp:TemplateField HeaderText="May-Jun">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMay" runat="server" CssClass="control-label" Text='<%# Eval("MAY_JUNE") %>'></asp:Label>
                                                    </ItemTemplate> 
                                                </asp:TemplateField> 
                                               <asp:TemplateField HeaderText="Jul-Aug">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJul" runat="server" CssClass="control-label" Text='<%# Eval("JUL_AUG") %>'></asp:Label>
                                                    </ItemTemplate> 
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Sep-Oct">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSep" runat="server" CssClass="control-label" Text='<%# Eval("SEP_OCT") %>'></asp:Label>
                                                        </ItemTemplate> 
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Modified By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModifiedBy" runat="server" CssClass="control-label" Text='<%# Eval("ModifiedBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Modified Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModifiedDate" runat="server" CssClass="control-label" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                            </ItemTemplate> 
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField> 
                                            </Columns> 
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvLagTime" EventName="RowCommand" /> 
                                        </Triggers>
                                    </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                <div class="modal-footer">
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                </div>
                            </div>
                        </div>
                                </div>
                    </div>
                <%--</div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <link href="../../Design/css/ui.jqgrid-bootstrap.css" rel="stylesheet" />
</asp:Content>

