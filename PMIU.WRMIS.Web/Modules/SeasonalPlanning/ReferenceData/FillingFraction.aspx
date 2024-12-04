<%@ Page Title="FillingFraction" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FillingFraction.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.FillingFraction" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Filling Fraction</h3>
                        </div>
                        <div class="box-content">
                            <div class="row">
                                <div class="form-horizontal">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                            <div id="roleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                                <asp:DropDownList CssClass="form-control required" required="true" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblRimStation" class="col-sm-4 col-lg-3 control-label">Rim Station</label>
                                            <div id="moduleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                                <asp:DropDownList CssClass="form-control required" required="true" Enabled="false" ID="ddlRimstation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRimstation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:GridView ID="gvRimstation" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                OnPageIndexChanged="gvRimstation_PageIndexChanged" OnRowCancelingEdit="gvRimstation_RowCancelingEdit"
                                OnRowUpdating="gvRimstation_RowUpdating" OnRowEditing="gvRimstation_RowEditing" OnPageIndexChanging="gvRimstation_PageIndexChanging"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>

                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Period">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maximum %" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaxPer" runat="server" Text='<%# Eval("MaxPercentage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMaxPercentage" runat="server" CssClass="form-control decimalInput" autocomplete="off" ClientIDMode="Static" onchange="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("MaxPercentage") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Minimum %">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMinPer" runat="server" Text='<%# Eval("MinPercentage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMinPercentage" runat="server" CssClass="form-control decimalInput" autocomplete="off" ClientIDMode="Static" onchange="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("MinPercentage") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Most Likely %">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLikelyPer" runat="server" Text='<%# Eval("LikelyPercentage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLikelyPercentage" runat="server" CssClass="form-control decimalInput" autocomplete="off" ClientIDMode="Static" onchange="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("LikelyPercentage") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Right">
                                                <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:Button>
                                                <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Right">
                                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnHistory" runat="server" CommandArgument='<%# Eval("TDailyID") %>' OnClick="lbtnHistory_Click" CssClass="btn btn-primary btn_24 audit" ToolTip="History"></asp:LinkButton>
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="ModalHistory" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body" id="content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="box">
                                        <div class="box-title">
                                            <%--<h3 id="hName" title="H History" runat="server"></h3>--%>
                                            <asp:Label ID="lblName" runat="server" Style="font-size: 30px;"></asp:Label>
                                        </div>
                                        <div class="box-content">
                                            <div class="table-responsive">
                                                <div class="row" id="gvOffenders" runat="server">
                                                    <div class="col-md-12">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                                ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                                                OnPageIndexChanged="gvHistory_PageIndexChanged" OnPageIndexChanging="gvHistory_PageIndexChanging"
                                                                CellSpacing="-1" GridLines="None">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Period">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Maximum %">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("MaxPercentage") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Minimum %">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("MinPercentage") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Most Likely %">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("LikelyPercentage") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Modified Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Modified By">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedBy") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        function ValidateValue(textboxID) {
            debugger;
            var MaxValue = 100;
            if (textboxID.value >= -100 && textboxID.value <= MaxValue && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {
                textboxID.setCustomValidity("");
                //alert('Value range is 0 to' + ' ' + MaxValue);
                //textboxID.value = '';
                return false;
            }
        }
    </script>
</asp:Content>
