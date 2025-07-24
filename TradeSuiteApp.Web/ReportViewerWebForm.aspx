<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ReportViewerWebForm.aspx.cs" Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="margin: 0px; padding: 0px; background: white;">
    <div style="display: none">
        <% Response.Write(ReportViewer1.LocalReport.DisplayName
                                         + "<br>" + ReportViewer1.UniqueID
                                         + "<br/>" + ReportViewer1.ClientID
                                         ); %>
    </div>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="Smaller" BorderStyle="Solid" BorderWidth="1px">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
