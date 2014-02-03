<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ResourceList.aspx.vb" Inherits="ResourceList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<%--    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.1.custom.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/ResourceList.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    
<%--    <link rel="stylesheet" href="Styles/jquery-ui-1.10.1.custom.css"/>
    <link rel="stylesheet" href="Styles/jquery.ui.dialog.css" />--%>

    <div>&nbsp;</div>
    <table>
        <tr>
            <td>
             <a id="lnkadd" onclick="addResource()" style="cursor: pointer;">Add Resource</a>
            </td>
            <td>&nbsp;</td>
            <td>
                <a id="lnkdelete" onclick="deleteResource()" style="cursor: pointer;">Delete Resource</a>
            </td>
            <td>&nbsp;</td>
            <td>
                Display&nbsp;
                <asp:DropDownList AutoPostBack="true" ID="ddlDisplay" runat="server">
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div>
        <asp:GridView ID="gvResource" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" EmptyDataText="No Record Found" PageSize="15"
             OnPageIndexChanging="gvResource_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate><asp:CheckBox ID="chkSelectAll" runat="server" /></HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RESOURCE_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RESOURCE_ID" HeaderText="Resource ID" SortExpression="RESOURCE_ID" />
                <asp:BoundField DataField="QLOOKID" HeaderText="Quick Look ID" SortExpression="QLOOKID" />
                <asp:BoundField DataField="RESOURCE_NBR" HeaderText="Resource Number" SortExpression="RESOURCE_NBR" />
                <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FIRST_NAME" />
                <asp:BoundField DataField="MIDDLE_INITIAL" HeaderText="MI" SortExpression="MIDDLE_INITIAL" />
                <asp:BoundField DataField="LAST_NAME" HeaderText="Last Name" SortExpression="LAST_NAME" />
                <asp:BoundField DataField="TELEPHONE_NBR" HeaderText="Telephone Number" SortExpression="TELEPHONE_NBR" />
                <asp:BoundField DataField="RESOURCE_COUNTRY_CODE" HeaderText="Country Code" SortExpression="RESOURCE_COUNTRY_CODE" />
                <asp:TemplateField>                    
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkview" runat="server" Text="View"></asp:LinkButton>                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkedit" runat="server" Text="Edit" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    </div>

    <div id="confirm" title="Confirm Delete" style="display: none;">
        <table style="background: url('images/form-bg.jpg')">
            <tr>
                <td>Are you sure you want to delete these records?</td>
            </tr>
        </table>                
    </div>
    
   
</asp:Content>

