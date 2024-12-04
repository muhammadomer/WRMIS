<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchComplaints.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.SearchComplaints" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Search Complaints</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Complaint ID</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtComplaintID" type="text" class="form-control " placeholder="CMXXXXXX" TabIndex="1"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Complainant</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtComplainantName" type="text" class="form-control " TabIndex="2"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Complainant Cell</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtComplainantCell" type="text" class="form-control " TabIndex="3"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Domain</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDomain" runat="server" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged">
                                            <%-- OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged"--%>
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDivision" TabIndex="5" runat="server">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Complaint Source</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlComplaintSource" TabIndex="6" runat="server">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server" TabIndex="7" >
                                            <%-- OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged"--%>
                                            <asp:ListItem Text="All" Value="" />

                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Action</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlAction" TabIndex="8" runat="server">
                                            <asp:ListItem Text="All" Value="" />
                                            <asp:ListItem Text="Important" Value="1" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="9" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="10" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Complaint Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlComplaintType" TabIndex="11" runat="server">
                                            <asp:ListItem Text="All" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <asp:CheckBox ID="chkboxOtherThenPMIUStaff" CssClass="checkbox-inline" TabIndex="12" Text="Show Complaints other than PMIU Staff" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <%--      <asp:Button runat="server" ID="btnBreachSearch" value=" Search " CssClass="btn btn-primary" Text="&nbsp;Search" OnClick="btnBreachSearch_Click" />--%>
                                    <asp:LinkButton TabIndex="13" ID="btnComplaintsSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnComplaintsSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                                    <asp:HyperLink ID="hlAddComplaint" runat="server" Visible="false" CssClass="btn btn-success" Text="Quick Add" TabIndex="15" />
                                    <%--OnClick="btnBreachSearch_Click"--%>
                                    <%--<asp:Button runat="server" ID="btnAddNewIncident" value=" Add New " CssClass="btn btn-success" Text="&nbsp;Add New" OnClick="btnAddNewIncident_Click" />--%>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:Label ID="lblTotalComplaints" Visible="false" runat="server"></asp:Label>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">            
                                <asp:GridView ID="gvSearchComplaints" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    OnPageIndexChanging="gvSearchComplaints_PageIndexChanging" OnRowDataBound="gvSearchComplaints_RowDataBound"
                                    DataKeyNames="ID">


                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" TabIndex="16" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" TabIndex="17" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Complaint ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComplaintNo" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblComplaintType" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Complaint Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComplaintDate" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Complainant">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComplainantName" runat="server" CssClass="control-label" Text='<%# Eval("ComplainantName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Domain">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDomain" runat="server" CssClass="control-label" Text='<%# Eval("DomainName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannel" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division" HeaderStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--            <asp:TemplateField HeaderText="Zone">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZone" runat="server" CssClass="control-label" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Favorite" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFavorite" runat="server" CssClass="control-label" Text='<%# Eval("Starred") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                             <asp:TemplateField HeaderText="ComplaintSource" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComplaintSourceID" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintSourceID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                        <%--        <asp:TemplateField HeaderText="ComplaintID" ItemStyle-HorizontalAlign="Right" Visible="false" >
                                            <ItemTemplate>
                                                 <asp:HyperLink ID="hlComplaintNumber" runat="server" CssClass="btn btn-primary btn_24 add-feedback"  NavigateUrl='<%# String.Format("~/Modules/ComplaintsManagement/AddComplaint.aspx?ComplaintSearchID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Add Complaint" Visible="true"></asp:HyperLink>
                                                 
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="14%">
                                            <ItemTemplate>
                                                <asp:HyperLink TabIndex="18" ID="hlFavorite" runat="server" CssClass="btn btn-primary btn_24 StarComplaint" data-id='<%# Eval("ID") %>' ToolTip="Important" Visible="true"></asp:HyperLink>
                                                <asp:HyperLink TabIndex="19" ID="hlComplaintEdit" runat="server" CssClass="btn btn-primary btn_24 add-feedback" NavigateUrl='<%# String.Format("~/Modules/ComplaintsManagement/AddComplaint.aspx?ComplaintSearchID={0}&ComplaintSource={1}", Convert.ToString(Eval("ID")),Convert.ToString(Eval("ComplaintSourceID"))) %>' ToolTip="Edit/View"></asp:HyperLink>
                                                <asp:HyperLink TabIndex="20" ID="hlComplaintActivity" runat="server" CssClass="btn btn-primary btn_24 history-feedback" NavigateUrl='<%# String.Format("~/Modules/ComplaintsManagement/AddComplaintComments.aspx?ComplaintSearchID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Comments Activity" ></asp:HyperLink>
                                                <asp:LinkButton TabIndex="21" ID="hlQuickAction" runat="server" CssClass="btn btn-primary btn_24 QuickComplaint " ToolTip="QuickAction" OnClick="hlQuickAction_Click" Visible="false" />
                                                <asp:LinkButton TabIndex="22" ID="lbtnPrint" runat="server" CssClass="btn btn-primary btn_24 channel" ToolTip="Print" OnClick="lbtnPrint_Click" Visible="<%# base.CanPrint %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                     
                            <asp:LinkButton TabIndex="23" ID="btnBulkActivity" runat="server" CssClass="btn btn-primary" Text="Bulk Activity" OnClick="btnBulkActivity_Click" ></asp:LinkButton>
                            <br />
                            <br />
                            <div id="BulkActivity" class="modal fade">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-body">
                                            <div class="form-horizontal">
                                                <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-11">
                                                                <div class="form-group">
                                                                    <label class="col-sm-4 col-lg-3 control-label">Complaints List</label>
                                                                    <div id="BulActivity" runat="server" class="col-sm-8 col-lg-9 controls">
                                                                        <asp:ListBox runat="server" ID="lstBoxComplaints" ClientIDMode="Static" SelectionMode="Multiple" class="selected form-control" Style="height: 200px; width: 40%;" />
                                                                        <div style="padding-top:10px"></div>
                                                                        <asp:LinkButton  ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-primary" OnClick="btnRemoveComplaints_Click"></asp:LinkButton>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>


                                                        <div class="row">
                                                            <div class="col-md-11" style="padding-left: 68px;">
                                                                <div class="form-group">
                                                                    <label class="col-sm-2 col-lg-2 control-label">Comments</label>
                                                                    <div class="col-sm-8 col-lg-9 controls" style="padding-left: 38px;">
                                                                        <asp:TextBox runat="server" TabIndex="1" ID="txtComments" type="text" TextMode="multiline" class="form-control " Rows="3"> </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="row" >
                                                <div class="col-md-11" style="padding-left: 84px;">
                                                    <div class="form-group">
                                                        <label class="col-sm-2 col-lg-2 control-label">Attachment</label>
                                                        <div class="col-sm-8 col-lg-9 controls" tabindex="3">
                                                            <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1"  />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-11" style="padding-left: 202px;padding-top: 10px;">
                                                    <div class="col-sm-4 col-lg-6 control-label">
                                                        <asp:CheckBox CssClass="col-sm-10 col-lg-10 controls checkbox-inline" TabIndex="4" ID="chkBoxResolved" Text="Mark All Complaints Resolved" runat="server" />

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <button id="btnClose" class="btn btn-info" tabindex="5" data-dismiss="modal" aria-hidden="true">Close</button>
                                            <asp:LinkButton TabIndex="6" ID="btnSave"  runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="QuickActivity" class="modal fade">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-body">

                                            <div class="form-horizontal">


                                                <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-11">
                                                                <div class="form-group">
                                                                    <label class="col-sm-4 col-lg-3 control-label">Complaint ID</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:Label runat="server" TabIndex="1" ID="lblQComplaintID" class="control-label"></asp:Label>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-11">
                                                                <div class="form-group">
                                                                    <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:TextBox runat="server"  TabIndex="2" ID="txtQuickComments" type="text" TextMode="multiline" class="form-control multiline-no-resize" Rows="3"> </asp:TextBox>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="row">
                                                    <div class="col-md-11" style="padding-right: 181px;">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 col-lg-4 control-label">Attachments</label>
                                                            <uc1:FileUploadControl runat="server"  TabIndex="3" ID="FileUploadControl1" Size="1" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-11" style="padding-right: 26px;">
                                                        <div class="col-sm-4 col-lg-6 control-label">
                                                            <asp:CheckBox ID="chkboxQResolve" CssClass="checkbox-inline"  TabIndex="4" Text="Mark Complaint Resolved" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button id="btnQClose" class="btn btn-info" data-dismiss="modal"  TabIndex="5" aria-hidden="true">Close</button>
                                                <asp:LinkButton TabIndex="6" ID="btnQuickSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnQuickSave_Click"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdnComplaintID" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
    <script type="text/javascript">
        // console.log(window.sessionStorage.getItem("ToDate"));
        $("a[data-id]").click(function () {
            var rowid = $(this).attr("data-id");
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'GET',
                data: { "rowid": rowid },
                url: '<%= ResolveUrl("SearchComplaints.aspx/AddComplaintAsFavorite") %>',
                success: function (data) {
                    if (data.d == true) {
                        //$('#lblMsgs').html('Complaint Add Favorite Successfully');
                        //$('#lblMsgs').addClass('SuccessMsg').show();
                        setTimeout(function () {// wait for 5 secs(2)
                            location.reload(); // then reload the page.(3)
                        }, 50);
                    }
                    else {
                        $('#lblMsgs').html('Complaint Not Added');
                        $('#lblMsgs').addClass('ErrorMsg').show();

                    }
                },
                error: function (xhr, err) {
                    $('#ajaxResponse').html(xhr.responseText);
                }
            });


        });

        // for check all checkbox  
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvSearchComplaints.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

    </script>
</asp:Content>
