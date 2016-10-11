<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFilter.aspx.vb" Inherits="WebSysME.ReportFilter" %>

<%@ Register Src="~/Reporting/ReportsFilterControl.ascx" TagName="ReportsFilterControl"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report Filter</title>
    <link href="ApplicationSkins/default/styles/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmReportFilter" runat="server">
        <uc1:ReportsFilterControl id="ReportsFilterControl1" runat="server">
        </uc1:ReportsFilterControl>
    </form>
</body>
</html>
