<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ResourceList.aspx.vb" Inherits="ResourceList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <%--    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.10.1.custom.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/ResourceList.js?version=5"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>


    <script type="text/javascript">
        hiddenFieldResourceID = '<%= hdnResourceID.ClientID %>';
        hiddenFieldMode = '<%= hdnMode.ClientID%>';

    </script>
    
<%--    <link rel="stylesheet" href="Styles/jquery-ui-1.10.1.custom.css"/>
    <link rel="stylesheet" href="Styles/jquery.ui.dialog.css" />--%>

        <asp:Panel ID="pnlResourceList" runat="server">
            <div class="Table">
        <div class="Row">
            <div class="LeftCell">
             <a id="lnkadd" onclick="addResource()" style="cursor: pointer;">Add Resource</a>
            </div>
            <div class="LeftCell">
                <a id="lnkdelete" onclick="deleteResource()" style="cursor: pointer;">Delete Resource</a>
  
            </div>
            <div class="LeftCell">
                <asp:Label ID="Label1" runat="server" Text="Display"></asp:Label>
                <asp:DropDownList AutoPostBack="true" ID="ddlDisplay" runat="server">
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <br />
    <div style="clear:both">
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

        </asp:Panel>
    
        <asp:Panel ID="pnlResourceInfo" runat="server">
            <div class="Table">
        <div class="Row">
            <div class="ActionButtonCell">
                 <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this resource?")'/>
                <asp:Button ID="btnEdit" runat="server" Text="Edit" UseSubmitBehavior="True" CausesValidation="False" OnClientClick="editClick()" />
                <asp:Button ID="btnSave" runat="server" Text="Save"  OnClientClick='return saveResource()' />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" OnClientClick="cancelClick()"/>
            </div>
        </div>
    </div>
            <div class ="Table">
                <div class ="Row">
                    <div class="LeftCell">
                        <asp:Label ID="lblQuickLookID" runat="server" Text="Quicklook ID"></asp:Label>
                    </div>
                    <div class="RightCell">
                        <asp:TextBox ID="txtQuicklookID" runat="server" ControlToValidate="txtQuicklookID" MaxLength="10" ></asp:TextBox>
                        <asp:Label ID="lblQuickLookIDView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                        <asp:RequiredFieldValidator ID="QuickLookIDValidator" runat="server" ErrorMessage="Required!" SetFocusOnError="True" ControlToValidate="txtQuicklookID"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="QuickLookIDCustomValidator" runat="server" ErrorMessage="Existing Quicklook ID" ControlToValidate="txtQuicklookID"></asp:CustomValidator>
                    </div>
                </div>
                <div class ="Row">
                    <div class="LeftCell">
                        <asp:Label ID="lblResourceNBR" runat="server" Text="Resource No"></asp:Label>
                    </div>
                    <div class="RightCell">
                        <asp:TextBox ID="txtResourceNBR" runat="server" ClientIDMode="Static" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblResourceNBRView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                 <asp:RequiredFieldValidator ID="ResourceNBRValidator" runat="server" ErrorMessage="Required!" SetFocusOnError="True" ControlToValidate="txtResourceNBR"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class ="Row">
                    <div class="LeftCell">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                    </div>
                    <div class="RightCell">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="25"></asp:TextBox>
                 <asp:Label ID="lblFirstNameView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                    </div>
                </div>
                <div class ="Row">
                    <div class="LeftCell">
                                        <asp:Label ID="lblMiddleInitial" runat="server" Text="Middle Initial"></asp:Label>
                    </div>
                    <div class="RightCell">
                 <asp:TextBox ID="txtMiddleInitial" runat="server" MaxLength="1"></asp:TextBox>
                 <asp:Label ID="lblMiddleInitialView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                    </div>
                </div>
                <div class ="Row">
                    <div class="LeftCell">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                        </div>
                     <div class="RightCell">
                         <asp:TextBox ID="txtLastName" runat="server" MaxLength="25"></asp:TextBox>
                         <asp:Label ID="lblLastNameView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                        </div>
                </div>
                <div class ="Row">
                        <div class="LeftCell">
                            <asp:Label ID="lblTelephoneNumber" runat="server" Text="Tel No"></asp:Label>
                        </div>
                        <div class="RightCell">
                            <asp:TextBox ID="txtTelephoneNumber" runat="server" MaxLength="25"></asp:TextBox>
                            <asp:Label ID="lblTelephoneNumberView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                        </div>
                </div>
                <div class ="Row">
                    <div class="LeftCell">
                        <asp:Label ID="lblCountryCode" runat="server" Text="Country Code"></asp:Label>
                    </div>
                    <div class="RightCell">
                        <asp:TextBox ID="txtCountryCode" runat="server" MaxLength="2"></asp:TextBox>
                        <asp:Label ID="lblCountryCodeView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                        <asp:RequiredFieldValidator ID="CountryCodeValidator" runat="server" ErrorMessage="Required!" SetFocusOnError="True" ControlToValidate="txtCountryCode"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        
        </asp:Panel>
    
   <asp:HiddenField ID="hdnResourceID"  runat="server" value="-1"/>
    <asp:HiddenField ID="hdnMode"  runat="server" value="View"/>
</asp:Content>

