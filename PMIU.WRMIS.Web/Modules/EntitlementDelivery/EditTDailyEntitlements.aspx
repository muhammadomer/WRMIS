<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="EditTDailyEntitlements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.EditTDailyEntitlements" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Edit TDaily Entitlements</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Command</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" ID="ddlCommand" runat="server" TabIndex="1" required="true" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server" id="dvScenario" visible="false">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Scenario:</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:Label ID="lblScenario" CssClass="control-label" Visible="false" runat="server" Text="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server">
                            <br />
                            <div class="row">
                                <div class="col-md-12 text-center">
                                    <asp:Label ID="lblMainDesc" Visible="false" runat="server" Text="Tarbela Command Entitlement for Rabi 2016-17" Font-Bold="true" />
                                </div>
                            </div>
                            <br />
                            <asp:Panel ID="pnlRabiHeader" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4">
                                        Average System Usage Rabi 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782Average" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row" id="dvRabiPara2" runat="server">
                                    <div class="col-md-4">
                                        Punjab Para(2) Rabi Share (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblEntitlementText" runat="server" Text="Entitlement for Rabi 2016-2017 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEntitlement" runat="server" Text="23.185" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        % Change in Rabi w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                   <br />
                            <label style="font-weight: bold;">Note: All values in cusecs except as noted.</label>
                            </asp:Panel>
                            <asp:Panel ID="pnlKharifHeader" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-4">
                                        Average System Usage Early Kharif 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782EKAverage" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        Average System Usage Late Kharif 1977-1982 (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lbl7782LKAverage" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row" id="dvKharifPara2" runat="server">
                                    <div class="col-md-4">
                                        Punjab Para(2) Early Kharif Share (MAF): 
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        Punjab Para(2) Late Kharif Share (MAF):  
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKPara2" runat="server" Text="25.781" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblEKEntitlementText" runat="server" Text="Entitlement for Early Kharif 2016 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKEntitlement" runat="server" Text="23.185" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblLKEntitlementText" runat="server" Text="Entitlement for Late Kharif 2016 (MAF):" />
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKEntitlement" runat="server" Text="23.185" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        % Change in Early Kharif w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblEKPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        % Change in Late Kharif w.r.t 1977-1982:
                                    </div>
                                    <div class="col-md-1 text-right">
                                        <asp:Label ID="lblLKPercentChange" runat="server" Text="-10.07" />
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                </div>
                                 <br />
                            <label style="font-weight: bold;">Note: All values in cusecs except as noted.</label>
                            </asp:Panel>
                           

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvTenDailyEntitlementsIndus" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found"
                                            ShowHeader="true" AllowPaging="False" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" Visible="false" OnRowDataBound="gvTenDailyEntitlementsIndus_RowDataBound">
                                            <Columns>

                                                <asp:TemplateField HeaderText="A">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("[0]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnTenDailyID" runat="server" Value='<%# Eval("[1]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="col-md-1" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtHD" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[2]") %>' OnTextChanged="txtHD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblHD" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[2]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnHDID" runat="server" Value='<%# Eval("[3]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[4]") %>' OnTextChanged="txtSC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblSC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[4]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnSCID" runat="server" Value='<%# Eval("[5]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[6]") %>' OnTextChanged="txtRC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblRC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[6]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnRCID" runat="server" Value='<%# Eval("[7]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPML" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[8]") %>' OnTextChanged="txtPML_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblPML" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[8]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnPMLID" runat="server" Value='<%# Eval("[9]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[10]") %>' OnTextChanged="txtAC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblAC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[10]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnACID" runat="server" Value='<%# Eval("[11]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTCMLU" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[12]") %>' OnTextChanged="txtTCMLU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblTCMLU" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[12]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnTCMLUID" runat="server" Value='<%# Eval("[13]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPCL" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[14]") %>' OnTextChanged="txtPCL_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblPCL" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[14]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnPCLID" runat="server" Value='<%# Eval("[15]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[16]") %>' OnTextChanged="txtMC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblMC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[16]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnMCID" runat="server" Value='<%# Eval("[17]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLBC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[18]") %>' OnTextChanged="txtLBC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblLBC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[18]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnLBCID" runat="server" Value='<%# Eval("[19]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMGC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[20]") %>' OnTextChanged="txtMGC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblMGC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[20]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnMGCID" runat="server" Value='<%# Eval("[21]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDGKC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[22]") %>' OnTextChanged="txtDGKC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblDGKC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[22]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnDGKCID" runat="server" Value='<%# Eval("[23]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCRBC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[24]") %>' OnTextChanged="txtCRBC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblCRBC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[24]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnCRBCID" runat="server" Value='<%# Eval("[25]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGTC" runat="server" onblur="myFunction(this);" CssClass="form-control numberAlign integerInput" Text='<%# Eval("[26]") %>' OnTextChanged="txtGTC_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="lblGTC" runat="server" Visible="false" CssClass="control-label text-right text-bold" Text='<%# Eval("[26]") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnGTCID" runat="server" Value='<%# Eval("[27]") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtTotal" runat="server" CssClass="control-label text-bold" Text='<%# Eval("[28]") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text-right" />
                                                    <HeaderStyle CssClass="text-center" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSave" runat="server" Visible="false" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script>
        function myFunction(input) {
            //    var number = parseInt(input.value);
            //    input.value = Math.round(number / 100) * 100;
        }
    </script>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
