<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptCadetBankSummary.aspx.cs" Inherits="GAFPAY.Reports.rptCadetBankSummary" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="800px" style="margin-right: 679px" Width="1200px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <ServerReport ReportPath="/GAFPAYReports/rptCadetBankMain" />
        </rsweb:ReportViewer>
    </form>
</body>
</html>
