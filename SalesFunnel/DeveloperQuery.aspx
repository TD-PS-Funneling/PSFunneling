<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DeveloperQuery.aspx.vb" Inherits="DeveloperQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="conMain" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="lblSQL" runat="server" Text="SQL" Font-Bold="True" ForeColor="Black" Font-Size="Large"></asp:Label>
    <br />
    <asp:TextBox ID="txtSQL" runat="server" Width="800px" Height="100px" BorderColor="Black" BorderStyle="Solid" BorderWidth="2" TextMode="MultiLine"></asp:TextBox>
    <br />
    <asp:Button ID="btnExplain" runat="server" Text="Explain" />
    &nbsp;
    <asp:Button ID="btnExecute" runat="server" Text="Execute" />
    <br />
    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="True" AlternatingRowStyle-BackColor="#66FFFF" AlternatingRowStyle-BorderColor="Black" AlternatingRowStyle-BorderStyle="Solid" AlternatingRowStyle-BorderWidth="1" AlternatingRowStyle-ForeColor="Black" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" EmptyDataText="?" FooterStyle-BorderColor="Black" FooterStyle-BorderStyle="Solid" FooterStyle-BorderWidth="1" FooterStyle-ForeColor="Black" ForeColor="Black" Visible="true" Enabled="true" AllowSorting="False"></asp:GridView>
    
</asp:Content>

