<%@ Page Title="Ziladaar Outlet Working" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ZiladaarOutletWorking.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ZiladaarOutletWorking" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucOutletDetails" Src="~/Modules/WaterTheft/Controls/OutletWaterTheftIncident.ascx" TagName="OutletDetail" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="ucOutletDetails" TagName="FileUploadControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnCaseID" runat="server" Value="0" />
     <asp:HiddenField ID="hdnDateOfChecking" runat="server" />
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Ziladaar Working</h3>
                </div>
                <div class="box-content" id="divMain" runat="server">
                    <div class="table-responsive">
                        <asp:PlaceHolder runat="server" ID="WaterTheftIncidentInformation"></asp:PlaceHolder>
                    </div>
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Area Booked</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtAreaBooked" runat="server" CssClass="form-control required integerInput" autocomplete="off" required="true" MaxLength="6" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Units</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">No of Accused</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtNoOfAccused" runat="server" CssClass="form-control" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Processed Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtProcessedDate" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" id="spnProcessedDate" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Add Comments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtComments" runat="server" onblur="TrimInput(this);" CssClass="form-control multiline-no-resize" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvOffender" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                        CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvOffender_PageIndexChanged" OnPageIndexChanging="gvOffender_PageIndexChanging"
                                        OnRowCommand="gvOffender_RowCommand" OnRowCancelingEdit="gvOffender_RowCancelingEdit" OnRowCreated="gvOffender_RowCreated" OnRowEditing="gvOffender_RowEditing"
                                        OnRowDeleting="gvOffender_RowDeleting" OnRowUpdating="gvOffender_RowUpdating">
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
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" AutoPostBack="false" required="required" MaxLength="25" Width="90%" Text='<%# Eval("OffenderName") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CNIC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("CNIC") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control phoneNoInput" autocomplete="off" AutoPostBack="false" MaxLength="13" Width="90%" Text='<%# Eval("CNIC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control required" required="required" AutoPostBack="false" MaxLength="250" Width="90%" Text='<%# Eval("Address") %>' />
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
                        <div class="row" id="Attachment" runat="server">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <ucOutletDetails:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnAssignSDO" runat="server" class="btn btn-primary" Text="Assign to SDO" OnClick="btnAssignSDO_Click" OnClientClick="return RequiredGrid();" />
                                    <%--<asp:Button ID="btnBack" runat="server" class="btn" Text="Back" OnClick="btnBack_Click" />--%>


                                    <asp:LinkButton ID="lbtnBack" runat="server" Text="Back" class="btn" OnClientClick="window.location.href='SearchWaterTheft.aspx?ShowHistory=true'; return false;"></asp:LinkButton>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function RequiredGrid() {
            $('.CtrlClass0').removeAttr("required");
            var Accused = document.getElementById("txtNoOfAccused");
            if (Accused.value > 0) {
                return true;
            }
            else {
                // document.getElementById("txtAreaBooked").setCustomValidity('Message still to be finalized');
                //document.getElementById("txtNoOfAccused").setCustomValidity('Message still to be finalized');                
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Please add offenders.");
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                return false;
            }
        }




        $('#<%=txtProcessedDate.ClientID%>').change(function () {
            ValidateWorkingDate(this);
        });

        function ValidateWorkingDate(datePickerDate) {
            dpg = $.fn.datepicker.DPGlobal;
            date_format = 'dd-MM-yyyy';
            console.log($('#<%=hdnDateOfChecking.ClientID%>').val());
            console.log(dpg.parseDate(dpg.parseDate($('#<%=hdnDateOfChecking.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en')));
            console.log(dpg.parseDate(datePickerDate.value, dpg.parseFormat(date_format), 'en'));

            var workingDate = dpg.parseDate(datePickerDate.value, dpg.parseFormat(date_format), 'en');
            var lastWorkingDate = dpg.parseDate($('#<%=hdnDateOfChecking.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en');
            if (lastWorkingDate > workingDate) {
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Date should be greater then " + GetFormatedDate(lastWorkingDate));
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                datePickerDate.value = "";
            }
        }

    </script>
</asp:Content>

