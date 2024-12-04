<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddClosureWork.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.AddClosureWork" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
 <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
       .decimalInput , .integerInput {
            text-align: left;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3> 
                        <asp:Label ID="lblPageTitle" Text ="Add Closure Work" runat="server"></asp:Label> 
                    </h3>
                </div>
                <div class="box-content">
                     <div class="form-horizontal">
                         <asp:HiddenField ID="hdnF_ID" runat="server" Value="0"/>
                         <asp:HiddenField ID="hdnF_CWP_ID" runat="server" />  
                         <asp:HiddenField ID="hdF_Mode" runat="server" Value ="A" />
                         <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Work Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkName" runat="server" placeholder="" MaxLength="250" required="required" CssClass="required form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Work Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                         <asp:DropDownList ID="ddlWorkType" runat="server" AutoPostBack="true" CssClass="required form-control" required="required" OnSelectedIndexChanged="ddlWorkType_SelectedIndexChanged">
                                             <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                    </div>
                                </div>
                            </div> 
                            <%--<div class="col-md-6" id="divOtherWorkType" runat="server" visible="false">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Sub Work Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                         <asp:DropDownList ID="ddlOthrWork" runat="server" CssClass="form-control" AutoPostBack="true" required="required" OnSelectedIndexChanged="ddlOthrWork_SelectedIndexChanged">
                                              <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                    </div>
                                </div>
                            </div> --%>
                        </div>
                        <div  id="div_D" runat="server"  > 
                            <hr />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddl_D_Chnl" runat="server" CssClass="required form-control" required="required">
                                             </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Silt to be Removed (cft)</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:TextBox ID="txtD_Silt" runat="server" placeholder="" required="required" MaxLength="15"  pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput form-control required"></asp:TextBox> 
                                        </div>
                                    </div>
                                </div> 
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">From RD (ft)</asp:Label>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtFromRDLeft" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength form-control required"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1 col-lg-1 controls">
                                            +
                                        </div>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtFromRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength form-control required"></asp:TextBox>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">To RD (ft)</asp:Label>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtToRDLeft" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength form-control required"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1 col-lg-1 controls">
                                            +
                                        </div>
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtToRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength form-control required"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">District</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddl_D_Dstrct" runat="server" CssClass="form-control"
                                                  OnSelectedIndexChanged="ddl_D_Dstrct_SelectedIndexChanged" AutoPostBack="true">
                                              <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Tehsil</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddl_D_tehsil" runat="server" CssClass="form-control" >
                                              <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </div>
                        <div  id="div_EM" runat="server" visible="false" > 
                            <hr />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Structure</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddl_EM_Struct" AutoPostBack="true" runat="server" CssClass="form-control required" required="required" OnSelectedIndexChanged="ddl_EM_Struct_SelectedIndexChanged">
                                                <asp:ListItem Text="Headwork/Barrage" Value="HB"/>
                                                <asp:ListItem Text="Channel" Value="chnl"/>
                                             </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Name</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddl_EM_Name" runat="server" CssClass="form-control required" required="required">
                                             </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </div>
                        <div  id="div_BW" runat="server" visible="false" >
                            <hr /> 
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group"> 
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:CheckBox CssClass="radio-inline" ID="cb_BW_Res" runat="server" Text="Residences" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cb_BW_Ofc" runat="server" Text ="Offices" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cb_BW_RHs" runat="server" Text="Rest House" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cb_BW_GRH" runat="server" Text="Gauge Reader Hut" /> 
                                            <asp:CheckBox CssClass="radio-inline"  ID="cb_BW_Othrz" runat="server" Text="Others" /> 
                                        </div>
                                    </div> 
                                </div>   
                            </div>
                        </div>
                        <div  id="div_OGP" runat="server" visible="false" > 
                            <hr />
                            
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sub-division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlOGPSubDiv" AutoPostBack="true" runat="server" CssClass="form-control required" 
                                                 required="required" OnSelectedIndexChanged="ddlOGPSubDiv_SelectedIndexChanged">
                                             </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Section</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlOGPSec" runat="server" CssClass="form-control"> <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group"> 
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:CheckBox CssClass="radio-inline" ID="cbGP" runat="server" Text="Gauge Painting" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cbGF" runat="server" Text ="Gauge Fixing" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cbGOGP" runat="server" Text="Gate Oiling/Greasing/Painting" />
                                            <asp:CheckBox CssClass="radio-inline"  ID="cbOthrz" runat="server" Text="Others" /> 
                                        </div>
                                    </div> 
                                </div>  
                            </div>
                        </div>
                        <div  id="div_OR" runat="server" visible="false" > 
                            <hr/>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sub-division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlORSubDiv" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlORSubDiv_SelectedIndexChanged">
                                             </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Section</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlORSec" runat="server" AutoPostBack="true" CssClass="form-control"  OnSelectedIndexChanged="ddlORSec_SelectedIndexChanged">
                                              <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlORChannels" runat="server" CssClass="form-control">
                                             <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div>  
                            </div>
                        </div>
                        <div  id="div_CSW" runat="server" visible="false" > 
                            <hr />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlCSWChannels" runat="server" CssClass="required form-control" required="required">
                                              <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div>  
                            </div>
                        </div>
                        <div  id="div_Other" runat="server" visible="false" > 
                            <hr />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sub-division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                             <asp:DropDownList ID="ddlOtherSubDiv" runat="server" CssClass="required form-control" AutoPostBack="true"
                                                 required="required" OnSelectedIndexChanged="ddlOtherSubDiv_SelectedIndexChanged">
                                             </asp:DropDownList>
                                        </div>
                                    </div> 
                                </div> 
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Section</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlOtherSec" runat="server" CssClass="form-control"  >
                                             <asp:ListItem Value="" Text="Select" />
                                         </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </div>
                 
                        <asp:Panel ID="pnlEstDtl" runat="server" GroupingText="Estimation Details">
                            <div class="row"> 
                               <div class="col-md-6" runat="server" visible="false">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Funding Source</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlFundingSrc" runat="server" CssClass="required form-control" required="required" > 
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Closure Period</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                             <asp:CheckBox CssClass="radio-inline" ID="cbClsurPrd" runat="server" style ="margin-left:-20px;" /> 
                                        </div>
                                    </div>
                                </div> 
                            </div>
                            <div class="row"> 
                               <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Cost (Rs)</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtCost" runat="server"  placeholder="" required="required" MaxLength="15" CssClass="required integerInput form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Completion Period</label>  
                                            <div class="col-sm-4 col-lg-5 controls"> 
                                                <asp:TextBox ID="txtCompPrd" runat="server"  placeholder=""  MaxLength="3"  CssClass="decimalInput form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-lg-4 controls"> 
                                                <asp:DropDownList ID="ddlCompPrdType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Day (s)" Value="Days"/>
                                                    <asp:ListItem Text="Week (s)" Value="Weeks"/>
                                                    <asp:ListItem Text="Month (s)" Value="Months"/>
                                                </asp:DropDownList>
                                            </div> 
                                    </div>
                                </div> 
                            </div>

                            <div class="row"> 
                               <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Start Date</label>
                                        <div class="col-sm-8 col-lg-9 control" runat="server" id ="divStartDate">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>  
                                                <asp:TextBox ID="txtStartDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div> 
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">End Date</label>
                                        <div class="col-sm-8 col-lg-9 controls" runat="server" id ="divEndDate">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtEndDate" TabIndex="4" runat="server" CssClass="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </asp:Panel>
               
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Technical Sanction and Tendering Details ">
                            <div class="row"> 
                               <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sanction No</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtSanctionNo" runat="server" placeholder="" required="required" MaxLength="30" CssClass="required form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sanction Date</label>
                                        <div class="col-sm-8 col-lg-9 controls" runat="server" id ="divSanctionDate">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="TxtSnctnDate" TabIndex="4" required="required" runat="server" CssClass="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>
                               
                                    </div>
                                </div> 
                            </div>

                            <div class="row"> 
                               <div class="col-md-6 ">
                                    <div class="form-group">
                                         <label class="col-sm-4 col-lg-3 control-label">Earnest Money (Rs)</label >
                                            <div class="col-sm-4 col-lg-5 controls">
                                                <asp:TextBox ID="txtErnsMny"  required="required" runat="server" placeholder=""  MaxLength="15" CssClass="decimalInput form-control required"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-lg-4 controls">
                                                <asp:DropDownList ID="ddlErnsMnyType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Lumpsum" Value="Lumpsum"/>
                                                    <asp:ListItem Text="% of Financial Bid" Value="% of Financial Bid"/> 
                                                </asp:DropDownList>
                                            </div> 
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Tender Fee (Rs)</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtndrFee" runat="server" placeholder=""  required="required" MaxLength="15" CssClass="decimalInput form-control required"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </asp:Panel>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Description</label> 
                                    <div class="col-sm-7 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control commentsMaxLengthRow multiline-no-resize" 
                                            minlength="3" MaxLength="250"  TextMode="MultiLine" Rows="5" Columns="50" 
                                            ID="txtDesc" runat="server" placeholder=""  ></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                        </div> 

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click"/>
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>

            </div>
                </div>
            </div>
        </div> 
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript"> 
        $(document).ready(function () {
            var title = $('#<%=lblPageTitle.ClientID%>').text();
            console.log('title :- ' + title);
            if (title.toLowerCase() === 'closure work')
            {
                $('input').removeClass('required');
                $('select').removeClass('required');
            }
           
            $('#<%=cbClsurPrd.ClientID %>').click(function () {
                console.log('check box clicked :- ' );
                if (!$(this).is(':checked')) {//disable fields 

                    $('#<%=txtCompPrd.ClientID %>').removeAttr('disabled');
                    $('#<%=ddlCompPrdType.ClientID %>').removeAttr('disabled');
                }
                else
                { 
                    $('#<%=txtCompPrd.ClientID %>').attr('disabled', 'disabled');
                    $('#<%=ddlCompPrdType.ClientID %>').attr('disabled', 'disabled');
                }
            });
            if (!$('#<%=cbClsurPrd.ClientID %>').is(':checked')) {//disable fields 
                $('#<%=txtCompPrd.ClientID %>').removeAttr('disabled');
                $('#<%=ddlCompPrdType.ClientID %>').removeAttr('disabled');
            }
            else {
                $('#<%=txtCompPrd.ClientID %>').attr('disabled', 'disabled');
                $('#<%=ddlCompPrdType.ClientID %>').attr('disabled', 'disabled');
            } 
			
			if ($('#<%=hdF_Mode.ClientID %>').val() == "V") {
                $('#<%=txtCompPrd.ClientID %>').attr('disabled', 'disabled');
                $('#<%=ddlCompPrdType.ClientID %>').attr('disabled', 'disabled');
			} 
        });

    </script>
    <style type="text/css">

    .pnlEstDtl legend
    {
    color:green;
    }

    </style>

 </asp:Content>