<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataPaging.ascx.cs" Inherits="PMIU.WRMIS.Web.Controls.DataPaging" %>
<table runat="server" cellpadding="0" cellspacing="0" width="100%" border="0" class="PagingContainer" id="tblPager">
    <tr class="GridFooter" style="height: 25px;">
        <td align="left" style="width: 25%" class="PagingShowingRecord">
            <asp:Label runat="server" ID="lblTotalRecord">Showing 00 of 00 record(s)</asp:Label>
        </td>
        <td align="center" style="width: 50%">
            <table cellpadding="0" cellspacing="0" border="0" class="PagingNavigation">
                <tr>
                    <td>
                        <asp:Button runat="server" ToolTip="Move First" ID="btnMoveFirst" CssClass="PagingFirst"
                            CommandArgument="First" OnCommand="btnMove_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ToolTip="Move Previous" ID="btnMovePrevious" CssClass="PagingPrevious"
                            CommandArgument="Previous" OnCommand="btnMove_Click" />
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" AutoPostBack="true" CssClass="CurrentPage" ID="txtPage"
                                        OnTextChanged="txtPage_TextChanged" Width="40"></asp:TextBox>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="CurrentPage" ID="ddlPage" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged" Width="40"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblTotalPage">Showing 00 of 00 record(s)</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Button runat="server" ToolTip="Move Next" ID="btnMoveNext" CssClass="PagingNext"
                            CommandArgument="Next" OnCommand="btnMove_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ToolTip="Move Last" ID="btnMoveLast" CssClass="PagingLast"
                            CommandArgument="Last" OnCommand="btnMove_Click" />
                    </td>
                </tr>
            </table>
        </td>
        <td align="right" style="width: 25%">
            <table cellpadding="0" cellspacing="0" class="PagingRecords">
                <tr>
                    <td>
                        Display records
                    </td>
                    <td>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlRecords" OnSelectedIndexChanged="ddlRecords_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>