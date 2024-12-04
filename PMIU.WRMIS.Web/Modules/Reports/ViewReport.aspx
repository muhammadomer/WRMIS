<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.ViewReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        td[id*='oReportCell'] {
            width: 100% !important;
        }
    </style>
</head>
<body style="margin-top: 0px auto;">
    <form id="frm" runat="server">
        <asp:ScriptManager ID="ScriptManagerMaster" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <table width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td width="100%" align="center">
                    <rsweb:ReportViewer ID="rvReport" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="480px" ProcessingMode="Remote" SizeToReportContent="True" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Style="width: 100%">
                        <ServerReport ReportPath="/PMIU.WRMIS.Reports/WT_CaseInformationChannel" ReportServerUrl="http://172.16.7.181/reportserver" />
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
