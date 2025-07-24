<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerForm.aspx.cs" Inherits="TradeSuiteApp.Web.ReportViewerForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body id="report-body" style="margin: 0px; padding: 0px; background: white; border-color: white;">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Message1" runat="server"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="Smaller" BorderStyle="Solid" BorderWidth="1px" BorderColor="White" Width="99.99%" Height="99.99%">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
