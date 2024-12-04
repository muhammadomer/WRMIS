<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewWorkTenderDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.ViewWorkTenderDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Work/Tender Details</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div>


                            <asp:Table ID="tblHeader" runat="server" CssClass="table tbl-info">

                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Tender Notice Name</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Work/Tender Name</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Work Type</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblTenderNotice" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblWorkName" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblWorkType" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>


                        </div>

                        <div id="Desilting" runat="server" visible="false">
                            <asp:Table ID="DesiltingTable" runat="server" CssClass="table tbl-info">
                                <%--<asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Work/Tender Name</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Work Type</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Channel</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingTenderName" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingWorkType" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingChannel" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>--%>
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Silt to be Removed (cft)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">From RD</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">To RD</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblCilt" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingFromRD" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingToRD" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">District</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Tehsil</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Channel</asp:TableHeaderCell>
                                    <%--        <asp:TableHeaderCell></asp:TableHeaderCell>--%>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingDistrict" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingTehsil" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDesiltingChannel" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>

                        <div id="div_EM" runat="server" visible="false">
                            <hr />

                            <asp:Table ID="Table1" runat="server" CssClass="table tbl-info">

                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="16.8%">Structure</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Channel</asp:TableHeaderCell>

                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblStructure" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblEMChannel" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>


                        </div>
                        <div id="div_BW" runat="server" visible="false">
                            <hr />
                            <asp:Table ID="Table2" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Building Works</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblBWType" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="div_OGP" runat="server" visible="false">
                            <hr />

                            <asp:Table ID="Table3" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Sub-Division</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Section</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Type</asp:TableHeaderCell>


                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOGPSubDivision" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOGPSection" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOGPType" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>


                        </div>
                        <div id="div_OR" runat="server" visible="false">
                            <hr />
                            <asp:Table ID="Table4" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Sub-Division</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Section</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Channel</asp:TableHeaderCell>


                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblORSubDivision" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblORSection" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblORChannel" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="div_CSW" runat="server" visible="false">
                            <hr />
                            <asp:Table ID="Table5" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Channel</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblCSWChannel" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="div_Other" runat="server" visible="false">
                            <hr />
                            <asp:Table ID="Table6" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="16.5%">Sub-Division</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Section</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOWSubDivision" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOWSection" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <asp:Panel ID="pnlEstDtl" runat="server" GroupingText="Estimation Details">

                            <asp:Table ID="Table7" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Funding Source</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Completion Period</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%"> Cost (Rs)</asp:TableHeaderCell>

                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblFundingSource" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblCompPeriod" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblCost" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Start Date</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">End Date</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%"> Work/Tender Office</asp:TableHeaderCell>

                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblStratDate" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOfficeLocated" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Office</asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblOfficeAddress" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>


                        <asp:Panel ID="pnlTechnical" runat="server" GroupingText="Technical Sanctioned Details">

                            <asp:Table ID="Table8" runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Sanctioned No</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Sanctioned Date</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%"> Earnest Money (Rs)</asp:TableHeaderCell>

                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblSanctionNo" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblSanctionDate" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblEarnestMoney" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell Width="33.3%">Tender Fee (Rs)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="33.3%">Description</asp:TableHeaderCell>


                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblTenderFee" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                    </asp:TableCell>

                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>

                         <asp:Panel ID="PnlAssets" runat="server" GroupingText="Association with Assets" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvWork" runat="server" AutoGenerateColumns="false"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Category">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Category">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control " Width="90%" />--%>
                                                            <asp:Label ID="lblSubCategory" runat="server" Text='<%# Eval("SubCategory") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Level">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control" Width="90%" />--%>
                                                            <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("Level") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlAssetName" runat="server" CssClass="form-control" Width="90%" />--%>
                                                            <asp:Label ID="lblAssetName" runat="server" Text='<%# Eval("AssetName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnAssetWork" runat="server" Text="" CommandName="AddWorkAssetAssociate" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />--%>
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>--%>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>

                        </div>

                    </asp:Panel>
                    <asp:Panel ID="PnlFlood" runat="server" GroupingText="Association with Flood" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvFlood" runat="server" AutoGenerateColumns="false"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDF" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlInfraType" runat="server" required="required" CssClass="form-control" Width="100%" />--%>
                                                            <asp:Label ID="lblSource" runat="server" Text='<%# Eval("Source") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlInfraStructure" runat="server" CssClass="form-control" Width="90%" />--%>
                                                            <asp:Label ID="lblStructureNameText" runat="server" Text='<%# Eval("StructureNameText") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategory" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnFloodWork" runat="server" Text="" CommandName="AddFlodAssociate" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />--%>
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnFloodDelete" runat="server" CommandName="DeleteFlood" CssClass="btn btn-danger btn_32 delete" formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>--%>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlInfrastructure" runat="server" GroupingText="Association with Infrastructure" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gv_Infra" runat="server" AutoGenerateColumns="false"
                                                EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDInfra" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlType" runat="server" required="required" CssClass="form-control" Width="100%" />--%>
                                                            <asp:Label ID="lblStructureTypeName" runat="server" Text='<%# Eval("StructureTypeName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <HeaderStyle CssClass="col-md-3" />
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlName" runat="server" CssClass="form-control" Width="90%" />--%>
                                                            <asp:Label ID="lblStructureNameText" runat="server" Text='<%# Eval("StructureNameText") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <HeaderTemplate>
                                                            <asp:Panel ID="pnlItemCategoryInfra" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnInfraWork" runat="server" Text="" CommandName="AddInfra" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />--%>
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlActionInfra" runat="server" HorizontalAlign="Center">
                                                                <%--<asp:Button ID="btnInfraDelete" runat="server" CommandName="DeleteInfra" CssClass="btn btn-danger btn_32 delete" formnovalidate="formnovalidate" ToolTip="Delete"></asp:Button>--%>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                        <div class="row" style="padding-left: 16px;">
                            <%--<asp:LinkButton TabIndex="10" ID="btnAddOpOffice" runat="server" CssClass="btn btn-primary" Text="Tender Opening Office" OnClick="btnAddOpOffice_Click"></asp:LinkButton>--%>
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>




    <div id="AddOpeningOffices" class="modal fade">

        <div class="modal-dialog modal-lg">
            <div class="modal-content">


                <div class="box">
                    <div class="box-title">
                        <h5>Tender Opening Office</h5>
                    </div>
                    <div class="modal-body">

                        <div class="form-horizontal">


                            <asp:UpdatePanel runat="server" ID="up">
                                <ContentTemplate>


                                    <div class="row">

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Opening Office</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control required" ID="ddlOpeningOffice" runat="server" required="true" OnSelectedIndexChanged="ddlOpeningOffice_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="All" Value="" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label" id="lblOfficeLocation" runat="server"></label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control required" ID="ddlOfficeLocated" runat="server" required="true">
                                                        <asp:ListItem Text="All" Value="" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="modal-footer">
                            <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            <asp:Button TabIndex="10" ID="btnSave1" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:Button>
                        </div>
                    </div>
                </div>



            </div>
        </div>

    </div>


    <!-- END Main Content -->
    <asp:HiddenField ID="hdnWordSourceID" runat="server" />
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" />
        <asp:HiddenField ID="hdnTenderNoticeID" runat="server" />
</asp:Content>
