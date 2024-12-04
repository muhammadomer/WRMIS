<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PunjabIndent.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.PunjabIndent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:UpdatePanel ID="UpdatePanelInspectionArea" runat="server" UpdateMode="Conditional">--%>
        <%--<ContentTemplate>--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>
                                <asp:Literal ID="litTitle" runat="server" Text="Punjab Indent" />

                            </h3>
                        </div>
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label id="lblFromDate" class="col-sm-4 col-lg-3 control-label">From</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtFromDate" ClientIDMode="Static" TabIndex="1" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label id="lblToDate" class="col-sm-4 col-lg-3 control-label">To</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtToDate" ClientIDMode="Static" TabIndex="2" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:Button ID="BtnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                                                <asp:HyperLink ID="hlPunjabIndent" class="btn btn-success" runat="server" NavigateUrl="~/Modules/EntitlementDelivery/AddPunjabIndent.aspx?">&nbsp;Add New Punjab Indent</asp:HyperLink>
                                                <%--<asp:Button ID="btnAddPI" class="btn btn-success" runat="server" Text="Add New Punjab Indent" OnClick="btnAddPI_Click" />--%>
                                                <%--<button class="btn btn-success" id="btnAddPI">Add New Punjab Indent</button>--%>
                                                <%--<input type="button" id="btnAddPI"   Class="btn btn-success buttons"  Value="Add New Punjab Indent"/>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-md-offset-4 ">
                                        <div class="form-group text-bold text-left">
                                            <p>*All Values are in '000Cusecs</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <%--<asp:UpdatePanel runat="server" ID="PunjabIndentUP" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                        <asp:GridView ID="gvPunjabIndent" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            DataKeyNames="ID,FromDate,ToDate,Thal,CJLinkAtHead,GreaterThal,BelowChashmaBarrage,CRBC,Mangla,Remarks"
                                            ShowHeaderWhenEmpty="True" Visible="false" OnRowDeleting="gvPunjabIndent_RowDeleting" OnRowEditing="gvPunjabIndent_RowEditing" OnRowCommand="gvPunjabIndent_RowCommand" AllowPaging="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Date" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="control-label" Text='<%# Utility.GetFormattedDate(Convert.ToDateTime(Eval("FromDate")))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="gvtxtFromDate" ClientIDMode="Static" TabIndex="1" runat="server" ValidationGroup="P_I" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="control-label" Text='<%#Utility.GetFormattedDate(Convert.ToDateTime( Eval("ToDate")))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <div class="input-group date" data-date-viewmode="years">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="gvtxtToDate" ClientIDMode="Static" TabIndex="2" ValidationGroup="P_I" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Thal">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblThal" runat="server" CssClass="control-label" Text='<%# Eval("Thal")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtThal" ClientIDMode="Static" TabIndex="3" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '12.0');" runat="server" class=" form-control decimalInput left" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CJLinkAtHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCJLinkAtHead" runat="server" CssClass="control-label" Text='<%# Eval("CJLinkAtHead")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtCJ" ClientIDMode="Static" TabIndex="4" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '25.0');" runat="server" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="GreaterThal">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGreaterThal" runat="server" CssClass="control-label" Text='<%# Eval("GreaterThal")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtGreaterThal" ClientIDMode="Static" TabIndex="7" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '10.0');" runat="server" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Below Chashma Barrage">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBelowChashmaBarrage" runat="server" CssClass="control-label" Text='<%# Eval("BelowChashmaBarrage")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtBelowChashmaBarrage" ClientIDMode="Static" TabIndex="5" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '30.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CRBC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCRBC" runat="server" CssClass="control-label" Text='<%# Eval("CRBC")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtCRBC" ClientIDMode="Static" TabIndex="6" runat="server" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '2.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mangla">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMangla" runat="server" CssClass="control-label" Text='<%# Eval("Mangla")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtMangla" ClientIDMode="Static" TabIndex="8" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '70.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" CssClass="control-label" Text='<%# Eval("Remarks")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="gvtxtRemarks" ClientIDMode="Static" TabIndex="9" ValidationGroup="P_I" Style="resize: none" TextMode="MultiLine" MaxLength="250" Columns="10" Rows="5" runat="server" class=" form-control text" type="text"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="FromDate" HeaderText="From Date" DataFormatString="{0:dd-MMMM-yyyy}" ReadOnly="true" />
                                                <asp:BoundField DataField="ToDate" HeaderText="To Date" DataFormatString="{0:dd-MMMM-yyyy}" ReadOnly="true" />
                                                <asp:BoundField DataField="Thal" HeaderText="Thal" ReadOnly="true" />
                                                <asp:BoundField DataField="CJLinkAtHead" HeaderText="C-J Link at Head" ReadOnly="true" />
                                                <asp:BoundField DataField="BelowChashmaBarrage" HeaderText="Below Chashma Barrage" ReadOnly="true" />
                                                <asp:BoundField DataField="CRBC" HeaderText="CRBC" ReadOnly="true" />
                                                <asp:BoundField DataField="GreaterThal" HeaderText="Greate Thal" ReadOnly="true" />
                                                <asp:BoundField DataField="Mangla" HeaderText="Mangla" ReadOnly="true" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true" />--%>
                                                
                                                <asp:TemplateField ItemStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlActionGaugeInspectionEdit" runat="server" HorizontalAlign="Right">
                                                            <asp:Button ID="btnEditGaugeInspection" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                             <asp:Button ID="lbtnDeleteGaugeInspection" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                                                OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                   <%-- <HeaderTemplate>
                                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float: right;">
                                                            <asp:LinkButton ID="btnMulGaugeInspection" runat="server" Text="" CommandName="AddPI" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add">
                                                            </asp:LinkButton>
                                                        </asp:Panel>
                                                    </HeaderTemplate>--%>
                                                    <EditItemTemplate>
                                                        <asp:Panel ID="pnlEditActionItemCategory" runat="server" HorizontalAlign="Center">
                                                            <asp:Button runat="server" ID="btnSaveItemCategory" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                            <asp:Button ID="btnCancelItemCategory" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                        </asp:Panel>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                        <div class="row" runat="server" id="divSave">
                                            <div class="col-md-6">
                                                <div class="fnc-btn">
                                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdateProgress runat="server">
                                            <ProgressTemplate>
                                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="../../Design/assets/prettyPhoto/images/prettyPhoto/default/loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="MpAddUpdatePunjabIndent" class="modal fade" style="margin-top: 100px;">
                <div class="modal-dialog" style="width: 60%;">
                    <div class="modal-content">
                        <div class="box">
                            <div class="box-title">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="hdrText" Style="font-size: 18px;" Text="Add Punjab Indent" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-md-offset-6" style="text-align: right">
                                        <p>*All Values are in '000Cusecs</p>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-body">
                                <div class="form-horizontal table-striped">
                                    <br>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="addlblFromDate" class="col-sm-6 col-lg-5 control-label">From</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtFromDateAdd" ClientIDMode="Static" TabIndex="1" runat="server" ValidationGroup="P_I" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:HiddenField runat="server" ID="hfPunjabIndentID" />
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="addlblToDate" class="col-sm-6 col-lg-5 control-label">To</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtToDateAdd" ClientIDMode="Static" TabIndex="2" ValidationGroup="P_I" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="Thal" class="col-sm-6 col-lg-5 control-label">Thal</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtThal" ClientIDMode="Static" TabIndex="3" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '12.0');" runat="server" class=" form-control decimalInput left" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblCJLinkatHead" class="col-sm-6 col-lg-5 control-label">C-J Link at Head</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtCJ" ClientIDMode="Static" TabIndex="4" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '25.0');" runat="server" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblBelowChashmaBarrage" class="col-sm-6 col-lg-5 control-label">Below Chashma Barrage</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtBelowChashmaBarrage" ClientIDMode="Static" TabIndex="5" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '30.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblCRBC" class="col-sm-6 col-lg-5 control-label">CRBC</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtCRBC" ClientIDMode="Static" TabIndex="6" runat="server" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '2.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblGreaterThal" class="col-sm-6 col-lg-5 control-label">GreaterThal</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtGreaterThal" ClientIDMode="Static" TabIndex="7" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '10.0');" runat="server" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblMangla" class="col-sm-6 col-lg-5 control-label">Mangla</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtMangla" ClientIDMode="Static" TabIndex="8" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '70.0');" class=" form-control decimalInput" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label id="lblRemarks" class="col-sm-6 col-lg-5 control-label">Remarks</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtRemarks" ClientIDMode="Static" TabIndex="9" ValidationGroup="P_I" Style="resize: none" TextMode="MultiLine" MaxLength="250" Columns="10" Rows="5" runat="server" class=" form-control text" type="text"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSavePunjabIndent" runat="server" class="btnSave btn btn-primary" ValidationGroup="P_I" Style="width: 68px" CausesValidation="true" Text="Save" OnClick="btnSavePunjabIndent_Click"></asp:Button>
                                <button class="btn btn-default" id="btnClose" type="button" onclick="ClosePopupModel();">Close</button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        <%--</ContentTemplate>--%>
   <%-- </asp:UpdatePanel>--%>

    <script type="text/javascript">

        var txtFromDate = $('#txtFromDate');
        var txtToDate = $('#<%= txtToDate.ClientID %>');
        var txtFromDateAdd = $('#<%= txtFromDateAdd.ClientID %>');
        var txtToDateAdd = $('#<%= txtToDateAdd.ClientID %>');
        var txtThal = $('#<%= txtThal.ClientID %>');
        var txtCJ = $('#<%= txtCJ.ClientID %>');
        var txtCRBC = $('#<%= txtCRBC.ClientID %>');
        var txtBelowChashmaBarrage = $('#<%= txtBelowChashmaBarrage.ClientID %>');
        var txtGreaterThal = $('#<%= txtGreaterThal.ClientID %>');
        var txtMangla = $('#<%= txtMangla.ClientID %>');
        var txtRemarks = $('#<%= txtRemarks.ClientID %>');




        function closeModalPUP() {
            $("#MpAddUpdatePunjabIndent").modal("hide");
        };
        function ClosePopupModel() {
            $("#MpAddUpdatePunjabIndent").modal("hide");
            EnableDesignParameter(txtFromDate);
            EnableDesignParameter(txtToDate);
            RemoveValidation();
            ResetControls();
        }
        function ResetControls() {
            txtFromDateAdd.removeData();
            txtToDateAdd.val('');
            txtThal.val('');
            txtCJ.val('');
            txtCRBC.val('');
            txtBelowChashmaBarrage.val('');
            txtGreaterThal.val('');
            txtMangla.val('');
            txtRemarks.val('');
        }
        function EnableDesignParameter(fieldID) {

            fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
        }
        function DisableDesignParameter(fieldID) {

            fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");

        }
        function RemoveRequiredAttribute(fieldID) {

            fieldID.removeAttr("required").removeClass("required");
        }
        function AddRequiredAttribute(fieldID) {
            fieldID.attr("required", "required").addClass("required");
        }
        function AddValidation() {

            EnableDesignParameter($("#txtFromDateAdd"));
            EnableDesignParameter($("#txtToDateAdd"));
            EnableDesignParameter($("#txtThal"));
            EnableDesignParameter($("#txtCJ"));
            EnableDesignParameter($("#txtCRBC"));
            EnableDesignParameter($("#txtBelowChashmaBarrage"));
            EnableDesignParameter($("#txtGreaterThal"));
            EnableDesignParameter($("#txtMangla"));
        }
        function RemoveValidation() {

            DisableDesignParameter($("#txtFromDateAdd"));
            DisableDesignParameter($("#txtToDateAdd"));
            DisableDesignParameter($("#txtThal"));
            DisableDesignParameter($("#txtCJ"));
            DisableDesignParameter($("#txtCRBC"));
            DisableDesignParameter($("#txtBelowChashmaBarrage"));
            DisableDesignParameter($("#txtGreaterThal"));
            DisableDesignParameter($("#txtMangla"));
        }
        function ResetControls() {
            txtFromDateAdd.val('');
            txtToDateAdd.val('');
            txtThal.val('');
            txtCJ.val('');
            txtCRBC.val('');
            txtBelowChashmaBarrage.val('');
            txtGreaterThal.val('');
            txtMangla.val('');
            txtRemarks.val('');
        }
        function EnableDesignParameter(fieldID) {

            fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
        }
        function DisableDesignParameter(fieldID) {

            fieldID.removeAttr("required").removeClass("required");

        }
        $('#btnAddPI').click(
                  function (e) {
                      //debugger
                      //$('#MpAddUpdatePunjabIndent').modal();
                      //$('#hdrText').text('Add Punjab Indent');
                      DisableDesignParameter($("#txtFromDate"));
                      DisableDesignParameter($("#txtToDate"));
                      //ResetControls();
                      //AddValidation();
                      //e.preventDefault();
                  });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                    function closeModalPUP() {
                        $("#MpAddUpdatePunjabIndent").modal('hide');
                    }
                    function ClosePopupModel(e) {
                        debugger;
                        EnableDesignParameter(txtFromDate);
                        EnableDesignParameter(txtToDate);
                        RemoveValidation();
                        ResetControls();
                        e.preventDefault();
                    }
                    function RemoveRequiredAttribute(fieldID) {

                        fieldID.removeAttr("required").removeClass("required");
                    }
                    function AddRequiredAttribute(fieldID) {
                        fieldID.attr("required", "required").addClass("required");
                    }
                    function AddValidation() {
                        EnableDesignParameter(txtFromDateAdd);
                        EnableDesignParameter(txtToDateAdd);
                        EnableDesignParameter(txtThal);
                        EnableDesignParameter(txtCJ);
                        EnableDesignParameter(txtCRBC);
                        EnableDesignParameter(txtBelowChashmaBarrage);
                        EnableDesignParameter(txtGreaterThal);
                        EnableDesignParameter(txtMangla);
                    }
                    function RemoveValidation() {

                        DisableDesignParameter(txtFromDateAdd);
                        DisableDesignParameter(txtToDateAdd);
                        DisableDesignParameter(txtThal);
                        DisableDesignParameter(txtCJ);
                        DisableDesignParameter(txtCRBC);
                        DisableDesignParameter(txtBelowChashmaBarrage);
                        DisableDesignParameter(txtGreaterThal);
                        DisableDesignParameter(txtMangla);
                    }
                    function ResetControls() {
                        txtFromDateAdd.removeData();
                        txtToDateAdd.val('');
                        txtThal.val('');
                        txtCJ.val('');
                        txtCRBC.val('');
                        txtBelowChashmaBarrage.val('');
                        txtGreaterThal.val('');
                        txtMangla.val('');
                        txtRemarks.val('');
                    }
                    function EnableDesignParameter(fieldID) {

                        fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
                    }
                    function DisableDesignParameter(fieldID) {

                        fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");

                    }
                }
            });
        };
        $(function () {


        });
    </script>
</asp:Content>
