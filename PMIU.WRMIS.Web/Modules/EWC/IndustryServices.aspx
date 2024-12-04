<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndustryServices.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.IndustryServices" EnableEventValidation="false" %> 
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Industry Services</h3>
                </div>
                <div class="box-content"> 
                    <div class="form-horizontal"> 

                         <div class="table-responsive" >
                                <table class="table tbl-info">
                                    <tr>
                                        <th> <div><strong>Industry Name</strong></div> </th>
                                        <th> <div ><strong>Industry ID</strong></div> </th>
                                    </tr>
                                    <tr> 
                                        <td> <asp:Label ID="lblName" runat="server" ></asp:Label> </td>
                                        <td> <asp:Label ID="lblID" runat="server"></asp:Label> </td>
                                    </tr>
                                    <tr>
                                        <th> <div><strong>Industry Type</strong></div> </th>
                                        <th> <div><strong>Industry Status</strong></div> </th>  
                                    </tr>
                                    <tr>
                                        <td> <asp:Label ID="lblType" runat="server" ></asp:Label> </td> 
                                        <td> <asp:Label ID="lblStatus" runat="server" ></asp:Label> </td> 
                                    </tr>
                                     <%--<tr>
                                        <th> <div><strong>Effluent Water Balance (Rs.)</strong></div> </th>
                                        <th> <div ><strong>Canal Special Water Balance (Rs.)</strong></div> </th>
                                    </tr>
                                    <tr> 
                                        <td> <asp:Label ID="lblEffBlnc" runat="server" ></asp:Label> </td>
                                        <td> <asp:Label ID="lblCnlBlnc" runat="server"></asp:Label> </td>
                                    </tr>--%>
                                </table>
                            </div>

                         
                            <asp:Panel ID="pnlEffluents" runat="server" GroupingText ="Effluent Water Details" Visible="false">
                                <div class="row">
                                     <asp:HiddenField ID="hdnEffWtrID" Value ="" runat="server" />
                                    <div class="col-md-6" id="divEffDeAc" runat="server" visible="false">
                                        <div class="form-group"> 
                                            <label class="col-sm-4 col-lg-3 control-label">In-active</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:CheckBox style="margin-left:-15px; margin-top:-5px;" CssClass="radio-inline" required="required" ID="radioInActiveEff" runat="server" AutoPostBack="true" Text="" />
                                             </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Outfall into</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="rbDrain" runat="server" AutoPostBack="true" GroupName="ViewTypeS" Text="Drain" OnCheckedChanged="rb_CheckedChanged" style="margin-top:-5px;"/>
                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="rbChnl" runat="server" AutoPostBack="true" GroupName="ViewTypeS" Text ="Channel" OnCheckedChanged="rb_CheckedChanged" style="margin-top:-5px;"/>
                                            </div>
                                        </div>
                                    </div> 
                                </div>
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                 <asp:DropDownList ID="ddlDiv" runat="server" required="required" CssClass="required form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value=""/>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <asp:label CssClass="col-sm-4 col-lg-3 control-label" ID="lblOutfall" runat="server" Text="Drain" ></asp:label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlOutfall" runat="server" required="required" CssClass="required form-control" >
                                                    <asp:ListItem Text="Select" Value=""/>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">RD (ft)</asp:Label>
                                            <div class="col-sm-3 col-lg-4 controls">
                                                <asp:TextBox ID="txtRDLeft" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength form-control required"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1 col-lg-1 controls">
                                                +
                                            </div>
                                            <div class="col-sm-3 col-lg-4 controls">
                                                <asp:TextBox ID="txtRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength form-control required"></asp:TextBox>
                                            </div>
                                        </div> 
                                    </div> 
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Side</asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlOutfallSide" runat="server" required="required" CssClass="required form-control" >
                                                    <asp:ListItem Text="Right" Value="R" />
                                                    <asp:ListItem Text="Left" Value="L" />
                                                </asp:DropDownList>
                                            </div> 
                                        </div>
                                    </div>
                                </div> 
                             
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Discharge Source</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlDschrgSrcs" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Installation Date</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtInstlDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                </div>

                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Installation Cost</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtInstlCost" runat="server" CssClass="form-control decimalIntegerInput" MaxLength="10" />
                                            </div>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Agreement Signed</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtAgrmntSignedOn" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div> 
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Agreement End</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtAgremntEndDate" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Agreement Parties</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                  <asp:TextBox ID="txtAgrmntParties" runat="server" CssClass="form-control" MaxLength="50" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="fnc-btn" Style="float: right;"> 
                                            <asp:Button ID="btnEffSave" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btn_Click"/>     
                                          </div>
                                    </div>
                                </div> 
                            </asp:Panel>   
                             
                        <asp:Panel ID="pnlCSW" runat="server" GroupingText="Canal Special Waters" Visible="false">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Division </asp:Label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDiv_CSW" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" > 
                                            </asp:DropDownList>
                                        </div> 
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                    EmptyDataText="No record found" OnRowCommand="gv_RowCommand"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" 
                                    ShowHeaderWhenEmpty="true">
                                    <Columns>
                                         <asp:TemplateField HeaderText="Channel"> 
                                            <ItemTemplate>
                                                <asp:HiddenField ID="lbl1" runat="server" Value='<%# Bind("ID") %>'></asp:HiddenField>
                                                <asp:Label ID="lbl2" runat="server" Text='<%# Bind("Channel") %>' />
                                            </ItemTemplate>  
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supply From"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lbl3" runat="server" Text='<%# Bind("SForm") %>' />
                                            </ItemTemplate>  
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Outlet/RD"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lbl4" runat="server" Text='<%# Bind("RO") %>' />
                                            </ItemTemplate>  
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Installation Date"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lbl5" runat="server" Text='<%# Bind("Date") %>' />
                                            </ItemTemplate>  
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lbl6" runat="server" Text='<%# Bind("InActive") %>' />
                                            </ItemTemplate>  
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnAdd" runat="server" formnovalidate="formnovalidate" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                                </asp:Panel>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEdit" runat="server" formnovalidate="formnovalidate" CommandName="Change" CssClass="btn btn-primary btn_32 edit" CommandArgument='<%# Eval("ID") %>' ToolTip="Change" ></asp:Button>
                                                    <asp:Button ID="btnDelete" runat="server" formnovalidate="formnovalidate" CommandName="Remove" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' ToolTip="Remove"></asp:Button>
                                                </asp:Panel>
                                            </ItemTemplate> 

                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                      <asp:HyperLink ID="hlCWPback" runat="server" NavigateUrl="~/Modules/EWC/Industry.aspx?RestoreState=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                               </div>
                            </div>
                        </div> 
                    </div> 
                   
                </div>
            </div>
        </div>
        <%--Canal Special Water Popup - start--%>
        <div id="divAdd" class="modal fade" >
            <div class="modal-dialog table-responsive" style="max-height: 419px; max-width: 893.398px;">
                <div class="modal-content" style="width: 830px">
                    <div class="modal-body">
                        <div class="box">
                            <div class="box-title">
                                <h3>
                                    <asp:HiddenField ID="hdf_Mode" runat="server" Value="Add" />
                                    <asp:HiddenField ID="Hdf_Index" runat="server" Value="0" />
                                    <asp:Label ID="lblTitle" Text="Canal Special Waters" runat="server" />
                                </h3>
                            </div>
                            <div class="box-content ">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <div class="form-horizontal">
                                                <div class="row">
                                                    <div class="col-md-6 " runat="server" visible="false" id="divCSWDeAc">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label"> In-active </label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:CheckBox CssClass="radio-inline" ID="radioCSWDeActivate" runat="server" Text="" style="margin-left:-15px; margin-top:-5px;"/>
                                                             </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:DropDownList ID="CSW_ddlChnl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-control required" required="required"/>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Supply From</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="radioRD" runat="server" AutoPostBack="true" GroupName="ViewType" Text="RD" OnCheckedChanged="rb_CheckedChanged" Checked="true"/>
                                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="radionOutlet" runat="server" AutoPostBack="true" GroupName="ViewType" Text ="Outlet" OnCheckedChanged="rb_CheckedChanged"/>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                                <div id="divRD"  runat="server" class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">RD (ft)</asp:Label>
                                                            <div class="col-sm-3 col-lg-4 controls">
                                                                <asp:TextBox ID="CSW_txtRDL" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength form-control required"></asp:TextBox>
                                                            </div>
                                                            <div class="col-sm-1 col-lg-1 controls">
                                                                +
                                                            </div>
                                                            <div class="col-sm-3 col-lg-4 controls">
                                                                <asp:TextBox ID="CSW_txtRDR" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength form-control required"></asp:TextBox>
                                                            </div>
                                                        </div> 
                                                    </div> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Side </label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:DropDownList ID="CSW_ddlSdie" runat="server" AutoPostBack="true" CssClass="form-control">
                                                                    <asp:ListItem Text="Right" Value="R"></asp:ListItem>
                                                                    <asp:ListItem Text="Left" Value="L"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                                <div id="divOutlet" runat="server" class="row">
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Outlet</label>
                                                            <div class="col-sm-8 col-lg-9 controls"> 
                                                                <asp:DropDownList ID="CSW_ddlOutlet" runat="server" AutoPostBack="true" CssClass="form-control required" required="required"/>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Supply Source</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:DropDownList ID="CSW_ddlSupplySrc" runat="server" AutoPostBack="true" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Installation Date</label>
                                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id ="divSanctionDate">
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="CSW_txtInstlDate" TabIndex="4" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Installation Cost</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:TextBox ID="CSW_txtInstlCost" runat="server" CssClass="form-control decimalIntegerInput" MaxLength="10"> </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Agreement Signed</label>
                                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id ="div1">
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="CSW_txtAgrmntSignDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                                <div class="row"> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Agreement End</label>
                                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id ="div2">
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="CSW_txtAgrmntEndDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="col-md-6 ">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-3 control-label">Agreement Parties</label>
                                                            <div class="col-sm-8 col-lg-9 controls">
                                                                <asp:TextBox ID="CSW_txtAgrmntParties" runat="server" CssClass="form-control" MaxLength="50"> </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                </div>
                                            </div> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCSWSave" runat="server" formnovalidate="formnovalidate" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btn_Click" />
                            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>  
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript">  
        function openModal() { 
            $("#divAdd").modal("show"); 
        };

        function closeModal() {
            $("#divAdd").modal("hide");
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh(); 
                }
            });
        };

        function Reset() { 
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeDatePickerStateOnUpdatePanelRefresh();
                        ClearDateField();
                    }
                });
            };
        }
     </script>
</asp:Content>
