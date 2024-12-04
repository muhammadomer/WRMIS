<%@ Page Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="WaterTheftIncidentWorking.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.WaterTheftIncidentWorking" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<%--<%@ Register Src="~/Modules/WaterTheft/Controls/OutletWaterTheftIncident.ascx" TagPrefix="uc" TagName="OutletWaterTheftIncident" %>--%>
<%@ Register Src="~/Modules/WaterTheft/Controls/TawaanWorking.ascx" TagPrefix="uc" TagName="TawaanWorking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">

                <div class="box-title">
                    <h3 id="hTitle" runat="server"></h3>
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:PlaceHolder runat="server" ID="WaterTheftIncidentInformation"></asp:PlaceHolder>
                        <%-- <asp:HyperLink ID="hlViewOffenders" runat="server" CssClass="btn btn-primary">View Offenders</asp:HyperLink>--%>

                        <asp:Button ID="btnOffenders" class="btn btn-primary" runat="server" Text="Offenders" formnovalidate="formnovalidate" OnClick="btnOffenders_Click" />

                    </div>
                    <br />
                    <div class="form-horizontal">
                        <uc:TawaanWorking runat="server" ID="TawaanWorking" />
                        <div class="row" id="divSDOBtn" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnAssignToZilardaar" runat="server" Text="Assign Back to Ziladaar" class="btn btn-primary" OnClick="btnAssignToZilardaar_Click" OnClientClick="return RequiredTextArea()" />&nbsp;
                                    <asp:Button ID="btnAssignToXEN" runat="server" Text="Assign  to XEN" class="btn btn-primary" OnClick="btnAssignToXEN_Click" OnClientClick="return AddRequiredFieldValidation()" />&nbsp;   
                                    <%--<asp:Button ID="btnBack" runat="server" Text="Backkkk" class="btn" OnClientClick="return RequiredTextArea()" />--%>
                                    <asp:LinkButton ID="lbtnBack" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divXENBtn" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnAssignToSDO" runat="server" Text="Assign Back to SDO" OnClick="btnAssignToSDO_Click" class="btn btn-primary" OnClientClick="return RequiredTextAreaXEN()" />&nbsp;
                                    <asp:Button ID="btnFinalizeClose" runat="server" Text="Finalize and Close" OnClick="btnFinalizeClose_Click" class="btn btn-primary" OnClientClick="return AddRequiredFieldValidationXEN()" />&nbsp;                                    
                                    <asp:LinkButton ID="lbtnXENBack" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divSEBtn" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnAssignToXENFromSE" runat="server" Text="Assign Back to XEN" OnClick="btnAssignToXENFromSE_Click" class="btn btn-primary" OnClientClick="return RequiredTextAreaSE()" />&nbsp;
                                    <%--<asp:Button ID="btnCancelFromSE" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancelFromSE_Click" />&nbsp;--%>
                                    <asp:LinkButton ID="lbtnBackFromSE" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divChiefBtn" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnAssignToXENFromChief" runat="server" Text="Assign Back to XEN" class="btn btn-primary" OnClick="btnAssignToXENFromChief_Click" OnClientClick="return RequiredTextAreaChief()" />&nbsp;
                                    <%--<asp:Button ID="btnCancelFromChief" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancelFromChief_Click" />&nbsp;--%>
                                    <asp:LinkButton ID="lbtnBackFromChief" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divNone" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:LinkButton ID="lbtnCancelForView" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="ViewOffenders" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">


                    <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="box">
                                <div class="box-title">
                                    <h3>View Offenders</h3>
                                </div>
                                <div class="box-content">
                                    <div class="table-responsive">
                                        <div class="row" id="gvOffenders" runat="server">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvOffender" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                                        CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvOffender_PageIndexChanged" OnPageIndexChanging="gvOffender_PageIndexChanging"
                                                        OnRowCommand="gvOffender_RowCommand" OnRowCancelingEdit="gvOffender_RowCancelingEdit" OnRowCreated="gvOffender_RowCreated"
                                                        OnRowEditing="gvOffender_RowEditing" OnRowDeleting="gvOffender_RowDeleting" OnRowUpdating="gvOffender_RowUpdating">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Offender Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("OffenderName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="true" MaxLength="25" Width="90%" Text='<%# Eval("OffenderName") %>' />
                                                                </EditItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="CNIC">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("CNIC") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtCNIC" runat="server" CssClass="phoneNoInput form-control" autocomplete="off" MaxLength="13" Width="90%" Text='<%# Eval("CNIC") %>' />
                                                                </EditItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control required" required="true" MaxLength="250" Width="90%" Text='<%# Eval("Address") %>' />
                                                                </EditItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <EditItemTemplate>
                                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                                        <asp:LinkButton ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:LinkButton>
                                                                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                                                    </asp:Panel>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                                        <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:LinkButton>
                                                                    </asp:Panel>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                                                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:LinkButton>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="PagerStyle" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
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
                    InitilizeNumericValidation();
                }
            });
        };
        function RequiredTextArea() {
            var comment = document.getElementById("taComments");
            comment.value = comment.value.trim();
            if (comment.value != null) {
                RemoveRequiredAttribute(document.getElementById("ddlDecisiontype"));
                RemoveRequiredAttribute(document.getElementById("txtFine"));
                RemoveRequiredAttribute(document.getElementById("txtLetterSDOToPolice"));
                $('.CtrlClass' + 0).blur();
                $('.CtrlClass' + 0).removeAttr('required');
                $('.FIRCtrl' + 0).blur();
                $('.FIRCtrl' + 0).removeAttr('required');
                $('.DesnCtrl' + 0).blur();
                $('.DesnCtrl' + 0).removeAttr('required');
                RemoveRequiredAttribute(document.getElementById("txtFirNo"));
                RemoveRequiredAttributeDate(document.getElementById("txtFIRDate"));
                RemoveRequiredAttribute(document.getElementById("txtCaseToXEN"));
                RemoveRequiredAttributeDate(document.getElementById("txtCaseToXENDate"));
                if (comment.value != "")
                    return true;
                else {
                    comment.setAttribute("required", "required");
                    comment.setAttribute("Class", "form-control multiline-no-resize required");
                    comment.get(0).setCustomValidity('');
                    return false;
                }
            }
            $('.CtrlClass0').removeAttr("required");
            $('.FIRCtrl0').removeAttr("required");
            $('.DesnCtrl0').removeAttr("required");
        }

        function RemoveRequiredAttribute(controlID) {
            controlID.removeAttribute("required");
            controlID.setAttribute("Class", "form-control");
        }

        function AddRequiredAttribute(controlID) {
            controlID.setAttribute("required", "required");
            controlID.setAttribute("Class", "form-control required");
        }

        function RemoveRequiredAttributeDate(controlID) {
            controlID.removeAttribute("required");
            controlID.setAttribute("Class", "form-control date-picker");
        }

        function AddRequiredAttributeDate(controlID) {
            controlID.setAttribute("required", "required");
            controlID.setAttribute("Class", "form-control date-picker required");
        }


        function AddRequiredFieldValidation() {

            AddRequiredAttribute(document.getElementById("ddlDecisiontype"));
            AddRequiredAttribute(document.getElementById("txtFine"));
            AddRequiredAttribute(document.getElementById("txtLetterSDOToPolice"));
            $('.CtrlClass' + 0).blur();
            $('.CtrlClass' + 0).attr('required');
            $('.FIRCtrl' + 0).blur();
            $('.FIRCtrl' + 0).attr('required');
            $('.DesnCtrl' + 0).blur();
            $('.DesnCtrl' + 0).attr('required');
            AddRequiredAttribute(document.getElementById("txtFirNo"));
            AddRequiredAttributeDate(document.getElementById("txtFIRDate"));
            if (document.getElementById("txtCaseToXEN").value == "")
                AddRequiredAttribute(document.getElementById("txtCaseToXEN"));
            AddRequiredAttributeDate(document.getElementById("txtCaseToXENDate"));
            var comment = document.getElementById("taComments");
            comment.removeAttribute("required");
            comment.setAttribute("Class", "form-control multiline-no-resize");

            $('.CtrlClass0').removeAttr("required");
            $('.FIRCtrl0').removeAttr("required");
            $('.DesnCtrl0').removeAttr("required");
            return true;
        }


        function RequiredTextAreaXEN() {

            var comment = document.getElementById("taComments");
            comment.value = comment.value.trim();
            if (comment.value != null) {
                RemoveRequiredAttribute(document.getElementById("txtSpecialCharges"));
                $('.CtrlClass' + 0).blur();
                $('.CtrlClass' + 0).removeAttr('required');
                $('.FIRCtrl' + 0).blur();
                $('.FIRCtrl' + 0).removeAttr('required');
                $('.DesnCtrl' + 0).blur();
                $('.DesnCtrl' + 0).removeAttr('required');
                if (comment.value != "")
                    return true;
                else {
                    comment.setAttribute("required", "required");
                    comment.setAttribute("Class", "form-control multiline-no-resize required");
                    comment.get(0).setCustomValidity('');
                    return false;
                }
            }
        }


        function AddRequiredFieldValidationXEN() {

            var comment = document.getElementById("taComments");
            comment.value = comment.value.trim();
            if (comment.value != null) {
                AddRequiredAttribute(document.getElementById("txtSpecialCharges"));
                $('.CtrlClass' + 0).blur();
                $('.CtrlClass' + 0).attr('required', 'required');
                $('.FIRCtrl' + 0).blur();
                $('.FIRCtrl' + 0).attr('required');
                $('.DesnCtrl' + 0).blur();
                $('.DesnCtrl' + 0).attr('required');
                comment.removeAttribute("required");
                comment.setAttribute("Class", "form-control multiline-no-resize");
                $('.DesnCtrl0').removeAttr("required");
                return true;
            }
        }

        function RequiredTextAreaSE() {

            var AA = document.getElementById("hdnAA");
            var comment = document.getElementById("taComments");
            comment.value = comment.value.trim();
            if (comment.value != null) {
                //regular  flow til  UAT
                // remove attributes from all the fields  
                //if (AA.value == "AssignBack") {    // 
                //    $('.DesnCtrl' + 0).blur();
                //    $('.DesnCtrl' + 0).removeAttr('required');
                //}

                //Final changes to remove required attribute from attachment
                $('.DesnCtrl' + 0).blur();
                $('.DesnCtrl' + 0).removeAttr('required');
                ///end 
                comment.setAttribute("required", "required");
                comment.setAttribute("Class", "form-control multiline-no-resize required");
                comment.get(0).setCustomValidity('');
                return true;
            }
        }

        function RequiredTextAreaChief() {

            var AA = document.getElementById("hdnAA");
            var comment = document.getElementById("taComments");
            comment.value = comment.value.trim();
            if (comment.value != null) {
                //regular flow till UAT
                // remove attributes from all the fields
                //if (AA.value == "AssignBack") {
                //    $('.AttchCtrl' + 0).blur();
                //    $('.AttchCtrl' + 0).removeAttr('required');
                //    $('.DesnCtrl' + 0).blur();
                //    $('.DesnCtrl' + 0).removeAttr('required');
                //}

                // final flow i.e. remove required attribute from attachments
                $('.AttchCtrl' + 0).blur();
                $('.AttchCtrl' + 0).removeAttr('required');
                $('.DesnCtrl' + 0).blur();
                $('.DesnCtrl' + 0).removeAttr('required');
                //end
                comment.setAttribute("required", "required");
                comment.setAttribute("Class", "form-control multiline-no-resize required");
                comment.get(0).setCustomValidity('');
                return true;
            }
        }

    </script>
</asp:Content>







