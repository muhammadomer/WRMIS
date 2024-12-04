<%@ Page Title="FillingFraction" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShareDistribution.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.ShareDistribution" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Share Distribution</h3>
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
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12 ">
                                    <asp:GridView ID="gvShare" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        OnPageIndexChanged="gvShare_PageIndexChanged" OnRowCancelingEdit="gvShare_RowCancelingEdit" OnRowCreated="gvShare_RowCreated"
                                        OnRowUpdating="gvShare_RowUpdating" OnRowEditing="gvShare_RowEditing" OnPageIndexChanging="gvShare_PageIndexChanging"
                                        OnRowDataBound="gvShare_RowDataBound" ShowFooter="true" ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px"
                                        CellSpacing="-1" GridLines="None">
                                        <Columns>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                                    <asp:Label ID="lblTDailyID" runat="server" Text='<%# Eval("TDailyID") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="10-Day">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="EK (MAF)" ID="lblPeriodEK" runat="server" />
                                                    <br />
                                                    <asp:Label Text="LK (MAF)" ID="lblPeriodLK" runat="server" />
                                                    <br />
                                                    <asp:Label Text="Total (MAF)" ID="lblPeriodTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Balochistan('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalochistan" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Balochistan")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtBalochistan" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# String.Format("{0:0.00}",Eval("Balochistan")) %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblBalEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblBalLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblBalTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="KPK('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKPK" runat="server" Text='<%# String.Format("{0:0.0}",Eval("KPK")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtKPK" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# String.Format("{0:0.00}",Eval("KPK")) %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblKPKEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblKPKLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblKPKTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Historic Punjab('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHistPunj" runat="server" Text='<%# String.Format("{0:0.0}",Eval("HistPunjab")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtHistPunj" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("HistPunjab") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblHistPunjEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblHistPunjLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblHistPunjTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Historic Sindh('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHistSindh" runat="server" Text='<%# String.Format("{0:0.0}",Eval("HistSindh")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtHistSindh" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("HistSindh") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblHistSindhEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblHistSindhLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblHistSindhTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Para 2 Punjab('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParaPunj" runat="server" Text='<%#String.Format("{0:0.0}", Eval("ParaPunjab")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtParaPunj" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("ParaPunjab") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblParaPunjEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblParaPunjLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblParaPunjTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Para2 Sindh('000 cusecs)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParasindh" runat="server" Text='<%# String.Format("{0:0.0}",Eval("ParaSindh")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtParaSindh" runat="server" CssClass="form-control decimalInput required" required="required" autocomplete="off" onkeyup="return ValidateValue(this);" MaxLength="6" Text='<%# Eval("ParaSindh") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label Text="" ID="lblParaSindhEK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblParaSindhLK" runat="server" /><br />
                                                    <asp:Label Text="" ID="lblParaSindhTotal" runat="server" />
                                                </FooterTemplate>
                                                <%--<HeaderStyle CssClass="col-md-1" />--%>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:Button>
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                                    </asp:Panel>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                                        <asp:LinkButton ID="lbtnHistory" runat="server" CommandArgument='<%# Eval("TDailyID") %>' CssClass="btn btn-primary btn_24 audit" ToolTip="History" OnClick="lbtnHistory_Click"></asp:LinkButton>
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




            <div class="modal fade" id="ModalHistory" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body" id="content">


                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="box">
                                        <div class="box-title">
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

                                                                    <asp:TemplateField HeaderText="10-Day">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Balochistan">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("Balochistan") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="KPK">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("KPK") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Historic Punjab">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("HistPunjab") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Para2 Punjab">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ParaPunjab") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Para2 Sindh">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ParaSindh") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Modified Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Modified By">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedBy") %>'></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        function ValidateValue(textboxID) {
            var MaxValue = 99;
            if ((textboxID.value >= 0 && textboxID.value <= MaxValue) && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {
                alert('Value range is 0 to' + ' ' + MaxValue);
                textboxID.value = '';
                return false;
            }
        }
    </script>
</asp:Content>


