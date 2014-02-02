<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="TestPage.aspx.vb" Inherits="TestPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <br />
    <br />
    <asp:Label ID="lblTrue" runat="server" Text="True User:" />
    <br />
    <asp:TextBox ID="txtTrueUser" runat="server" Enabled="False"/>
    <br />
    <br />
    <asp:Label ID="lblImpersonate" runat="server" Text="Impersonating User:" />
    <br />
    <asp:DropDownList ID="ddlImpersonateUser" runat="server"/>
    <br />
    <br />
    <asp:Button ID="btnImpersonate" runat="server" Text="Impersonate" />
</asp:Content>



