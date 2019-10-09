<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<%@ Register Assembly="ListBoxControl" Namespace="ListBoxControl" TagPrefix="cc1" %>

<%@ Register Assembly="DropDownControl" Namespace="DropDownControl" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%-- XML datasources definition --%>
        <asp:XmlDataSource ID="MySource" DataFile="XmlData.xml" runat="server">
        </asp:XmlDataSource>
        
        <asp:XmlDataSource ID="MySource2" DataFile="XmlData2.xml" runat="server">
        </asp:XmlDataSource>
    
        <%-- Examples of custom DateDropDownList --%>
        <cc2:DateDropDownList ID="DateDropDownList1" runat="server">
            <asp:ListItem>Entry 1</asp:ListItem>
            <asp:ListItem Selected="True">Entry 2</asp:ListItem>
            <asp:ListItem>Another val</asp:ListItem>
        </cc2:DateDropDownList>
        
        <cc2:DateDropDownList ID="DateDropDownList2" runat="server" 
        DataSourceID="MySource2" DataMember="ListItem" DataTextField="Text">
        </cc2:DateDropDownList>
        <br /><br />

        <%-- Examples of custom DateListBox --%>
        <cc1:DateListBox ID="DateListBox1" runat="server">
            <asp:ListItem Selected="True">Entry my</asp:ListItem>
            <asp:ListItem>Simple data</asp:ListItem>
        </cc1:DateListBox>
       
        <cc1:DateListBox ID="DateListBox2" runat="server"  
        DataSourceID="MySource" DataMember="ListItem" DataTextField="Text">
        </cc1:DateListBox>
        
    </div>
    </form>
</body>
</html>
