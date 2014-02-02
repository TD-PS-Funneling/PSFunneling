<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Resource.aspx.vb" Inherits="Resource"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="MainContent">

    <asp:Table ID="tblButtons" runat="server" Width="100%">
        <asp:TableRow ID="TableRow7" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell14" runat="server" HorizontalAlign="Right">
                 <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this resource?")'/>
                &nbsp;
                <asp:Button ID="btnEdit" runat="server" Text="Edit" UseSubmitBehavior="True" CausesValidation="False" />
                &nbsp;               
                <asp:Button ID="btnSave" runat="server" Text="Save"  OnClientClick='return confirm("Save Resource Information?")' />
                &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="tblResource" runat="server" Width="100%">
        <asp:TableRow runat="server" Height="100%" Width="100%">
            <asp:TableCell runat="server" Width="15%">
                <asp:Label ID="lblQuickLookID" runat="server" Text="Quicklook ID"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcQuicklookID" runat="server" >
                 <asp:TextBox ID="txtQuicklookID" runat="server" ControlToValidate="txtQuicklookID" MaxLength="10" ></asp:TextBox>
                 <asp:Label ID="lblQuickLookIDView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                 <asp:RequiredFieldValidator ID="QuickLookIDValidator" runat="server" ErrorMessage="Required!" SetFocusOnError="True" ControlToValidate="txtQuicklookID"></asp:RequiredFieldValidator>
                 <asp:CustomValidator ID="QuickLookIDCustomValidator" runat="server" ErrorMessage="Existing Quicklook ID" ControlToValidate="txtQuicklookID"></asp:CustomValidator>
             </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow ID="TableRow1" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell2" runat="server">
                <asp:Label ID="lblResourceNBR" runat="server" Text="Resource No"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcResourceNBR" runat="server" >
                 <asp:TextBox ID="txtResourceNBR" runat="server" ClientIDMode="Static" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblResourceNBRView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                 <asp:RequiredFieldValidator ID="ResourceNBRValidator" runat="server" ErrorMessage="Required!" SetFocusOnError="True" ControlToValidate="txtResourceNBR"></asp:RequiredFieldValidator>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell4" runat="server">
                <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcFirstName" runat="server" >
                 <asp:TextBox ID="txtFirstName" runat="server" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblFirstNameView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell6" runat="server">
                <asp:Label ID="lblMiddleInitial" runat="server" Text="Middle Initial"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcMiddleInitial" runat="server" >
                 <asp:TextBox ID="txtMiddleInitial" runat="server" MaxLength="1"></asp:TextBox>
                 <asp:Label ID="lblMiddleInitialView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow4" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell8" runat="server">
                <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcLastName" runat="server" >
                 <asp:TextBox ID="txtLastName" runat="server" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblLastNameView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow5" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell10" runat="server">
                <asp:Label ID="lblTelephoneNumber" runat="server" Text="Tel No"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcTelephoneNumber" runat="server" >
                 <asp:TextBox ID="txtTelephoneNumber" runat="server" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblTelephoneNumberView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow6" runat="server" Height="100%" Width="100%">
            <asp:TableCell ID="TableCell12" runat="server">
                <asp:Label ID="lblCountryCode" runat="server" Text="Country Code"></asp:Label></asp:TableCell>
             <asp:TableCell ID="tcCountryCode" runat="server" >
                 <asp:TextBox ID="txtCountryCode" runat="server" MaxLength="2"></asp:TextBox>
                  <asp:Label ID="lblCountryCodeView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </asp:TableCell>
        </asp:TableRow>
        
    </asp:Table>
</asp:Content>


