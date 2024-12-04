<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddComplaintComments.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.AddComplaintComments" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc" TagName="FileUploadControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Complaint Comments Activity</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">


                        <asp:Table ID="tblChannelWaterTheftIncident" runat="server" CssClass="table tbl-info">
                            <asp:TableRow>
                                <asp:TableHeaderCell Width="33.3%">Complaint ID </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="33.3%">Complaint Source</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="33.3%">Domain</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblComplaintNumber" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblComplaintSource" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblDomain" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableHeaderCell>Division</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Complaint Date</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Complainant Name</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblComplaintDate" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblComplainantName" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableHeaderCell>Complaint Type</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Mobile No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Address</asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblComplaintType" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblCell" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                            <asp:TableHeaderCell>Complaint Details</asp:TableHeaderCell>
                            <asp:TableHeaderCell>PMIU File No.</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Response Duration</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblComplaintDetails" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblPMIUFileNo" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblResponseDuration" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Attachment</asp:TableHeaderCell>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                          <%--      <asp:HyperLink ID="hlAttachment" runat="server" Visible="false" CssClass="btn btn-primary btn_24 viewimg"></asp:HyperLink>--%>
                                   <uc:FileUploadControl runat="server" ID="FileUploadControl2" Size="0" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthers1" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthers2" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        </asp:Table>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvComplaintComments" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                        DataKeyNames="ID,Attachment" OnPageIndexChanging="gvSearchComplaints_PageIndexChanging" OnRowDataBound="gvComplaintComments_RowDataBound">

                                        <%--OnRowDataBound="gvComplaintComments_RowDataBound"--%>
                                        <%--OnPageIndexChanging="gvComplaintComments_PageIndexChanging"--%>

                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComplaintID" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ComplaintID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComplaintNumber" runat="server" CssClass="control-label" Text='<%# Eval("ComplaintNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComplaintComments" runat="server" CssClass="control-label" Text='<%# Eval("Comments") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Logged By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date/Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComplaintDate" runat="server" CssClass="control-label" Text='<%# Eval("Date") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Attachment">
                                                <ItemTemplate>
                                                     <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("Attachment") %>' Visible="false"></asp:Label>
                                                    <%-- <asp:LinkButton ID="lnkViewAttachments" OnClick="lnkViewAttachments_Click" runat="server">View Attachments</asp:LinkButton>--%>
                                                  <%--  <asp:HyperLink ID="hlImage" NavigateUrl='<%# PMIU.WRMIS.Common.Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.Complaints , Convert.ToString(Eval("Attachment"))) %>' CssClass="btn btn-primary btn_24 viewimg" runat="server"  />--%>
                                                        <uc:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                                </ItemTemplate>
                                            </asp:TemplateField>





                                            <%--              <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Action" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlQuickAction" runat="server" CssClass="btn QuickComplaint channel" ToolTip="QuickAction" OnClick="hlQuickAction_Click" Visible="false" />
                                <asp:HyperLink ID="hlComplaintID" runat="server" CssClass="btn btn-primary btn_24 view" data-id='<%# Eval("ID") %>' ToolTip="Favorite" Visible="true"></asp:HyperLink>
                                <asp:LinkButton ID="lbtnPrint" runat="server" CssClass="btn btn-primary btn_24 channel" ToolTip="Print" OnClick="lbtnPrint_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <%--          <div class="row">
                            <div class="col-md-12">
                                <div>
                                    <asp:LinkButton runat="server" ID="btnAddComment" value="Add Comments" CssClass="btn btn-primary" Text="Add Comments" OnClick="btnAddComments_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>--%>
                        <div id="commentsdiv" runat="server">

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="box">
                                        <div class="box-title">
                                            <h3>Comments Area</h3>
                                        </div>
                                        <div class="box-content">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-lg-2 control-label" style="text-align: left">Comments</label>
                                                <div class="col-sm-8 col-lg-10 controls">
                                                    <asp:TextBox runat="server" ID="txtComments"  TabIndex="1" required="true" type="text" TextMode="multiline" class="form-control multiline-no-resize required" Rows="3"> </asp:TextBox>
                                                </div>

                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label class="col-sm-2 col-lg-2 control-label" style="text-align: left">Attachment</label>
                                                <uc:FileUploadControl runat="server"  ID="FileUploadControl" Size="1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="NotifyArea" runat="server">
                                <div class="col-md-6">
                                    <div class="box">
                                        <div class="box-title">
                                            <h3>Notify Area</h3>
                                        </div>
                                        <div class="box-content">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblNotify" class="col-sm-1 col-lg-1 control-label" Style="text-align: left">SDO</asp:Label>
                                                <%-- <label runat="server" class="col-sm-4 col-lg-3 control-label">Notify</label>--%>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control" ID="ddlNotify" runat="server" TabIndex="5">
                                                        <%-- OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged"--%>
                                                        <asp:ListItem Text="Select" Value="" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div>
                                                <asp:LinkButton runat="server" TabIndex="6" ID="btnNotifySDO" value="Notify" CssClass="btn btn-primary" Text="Notify" OnClick="btnNotifySDO_Click">

                                                </asp:LinkButton>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                    </div>
                            </div>
                        </div>





                        <div  class="row" style="padding-left: 16px;">


                            <asp:Button runat="server" TabIndex="2" ID="btnSave" value="Save" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveComments_Click" />




                            <asp:Button runat="server" TabIndex="2" ID="btnSaveForward" value="Forward" CssClass="btn btn-primary" Text="Forward" OnClick="btnForward_Click" />




                            <asp:Button runat="server" TabIndex="2" ID="btnAssignADM" value="Assign To ADM" CssClass="btn btn-primary" Text="Assign To ADM" OnClick="btnAssignADM_Click" />




                            <asp:Button runat="server" TabIndex="2" ID="btnAssignXEN" value="Assign To XEN" CssClass="btn btn-primary" Text="Assign To XEN" OnClick="btnAssignToXEN_Click" />



                            <asp:Button runat="server" TabIndex="3" ID="btnResolved" value="Resolved" CssClass="btn btn-primary" Text="Resolved" OnClick="btnResolved_Click" />

                                <asp:HyperLink ID="hlBack" runat="server" TabIndex="4" CssClass="btn" Text="Back"/>

                        </div>
                    
                        <asp:HiddenField ID="hdnComplaintID" runat="server" />
                        <asp:HiddenField ID="hdnDivisionID" runat="server" />
                           <asp:HiddenField ID="hdnComplaintStatus" runat="server" />
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->

    <!-- Start Of view images -->
    <div id="viewAttachment" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:GridView ID="gvViewAttachment" runat="server"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="false"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="File name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("Attachment") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlImage" NavigateUrl='<%# PMIU.WRMIS.Common.Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.Complaints , Convert.ToString(Eval("Attachment"))) %>' CssClass="btn btn-primary btn_24 viewimg" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="75px" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button id="btnCloseAttachment" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- End Of view images -->

    <script type="text/javascript">
        $(document).ready(function () {
            $('.CtrlClass0').blur();
            $('.CtrlClass0').removeAttr('required');
        });
    </script>
</asp:Content>
