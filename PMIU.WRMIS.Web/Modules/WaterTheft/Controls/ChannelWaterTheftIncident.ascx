<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChannelWaterTheftIncident.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.Controls.ChannelWaterTheftIncident" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/SBESDOWorking.ascx" TagPrefix="ucSBESDOWorking" TagName="SBESDOWorking" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="ucSBESDOWorking" TagName="FileUploadControl" %>

<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Table ID="tblCaseNo" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>
            Case ID: &nbsp;&nbsp;&nbsp;         
            <asp:Label ID="lblCaseNo" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Case Status: &nbsp;&nbsp;&nbsp;         
            <asp:Label ID="lblCaseStatus" runat="server"></asp:Label>
        </asp:TableHeaderCell>
    </asp:TableRow>
</asp:Table>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Table ID="tblChannelWaterTheftIncident" runat="server" CssClass="table tbl-info">
            <asp:TableRow>
                <asp:TableHeaderCell Width="33.3%">Channel Name</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="33.3%">Type of Offence</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="33.3%">Condition of Cut</asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lblChannelName" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblOffencesType" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCutCondition" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnChannelTotalRDs" Value="0" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableHeaderCell>Offence Side</asp:TableHeaderCell>
                <asp:TableHeaderCell>Date Time of Checking</asp:TableHeaderCell>
                <asp:TableHeaderCell>Theft Site RD</asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lblOffenceSide" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblDateTimeOfChecking" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblTheftSiteRD" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
               <%-- <asp:TableHeaderCell>SMS Date</asp:TableHeaderCell>--%>
                <asp:TableHeaderCell>
                    <asp:Label ID="lblViewRemarks" runat="server" Text="Remarks History"></asp:Label>
                </asp:TableHeaderCell>
                <asp:TableHeaderCell>Proof</asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableRow>
           <%--     <asp:TableCell>
                    <asp:Label ID="lblSMSDate" runat="server"></asp:Label>
                </asp:TableCell>--%>
                <asp:TableCell>
                    <asp:LinkButton ID="lnkViewRemarks" OnClick="lnkViewRemarks_Click" runat="server">View Remarks</asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:LinkButton ID="lnkViewAttachments" OnClick="lnkViewAttachments_Click" runat="server">View Attachments</asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
            <%--<asp:TableRow>
                <asp:TableHeaderCell ColumnSpan="3">Remarks</asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3">
                    <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>--%>
        </asp:Table>
    </ContentTemplate>
</asp:UpdatePanel>
<ucSBESDOWorking:SBESDOWorking runat="server" ID="SBESDOWorking" />

<br />

<!-- Start Of view remarks -->

<div id="viewRemarks" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                <asp:UpdatePanel runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="gvViewRemarks" runat="server"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="false"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnPageIndexChanging="gvViewRemarks_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="225px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Log Date & Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLogDateTime" runat="server" Text='<%# Eval("LogDateTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Logged By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLoggedBy" runat="server" Text='<%# Eval("LoggedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
            </div>
        </div>
    </div>
</div>
<!-- End Of view remarks -->

<!-- Start Of view images -->
<div id="viewAttachment" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                <asp:UpdatePanel runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="gvViewAttachment" runat="server"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="false"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnPageIndexChanging="gvViewAttachment_PageIndexChanging"     OnRowDataBound="gvViewAttachment_RowDataBound" 
                            DataKeyNames="AttachmentPath">
                            <Columns>
                                <asp:TemplateField HeaderText="File name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploadedBy" runat="server" Text='<%# Eval("UploadedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                            <ucSBESDOWorking:FileUploadControl runat="server" ID="FileUploadControl4" Size="0" />
                                       <%-- <asp:HyperLink ID="hlImage" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.WaterTheft , Convert.ToString(Eval("AttachmentPath"))) %>' CssClass="btn btn-primary btn_24 viewimg" runat="server" />--%>
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
