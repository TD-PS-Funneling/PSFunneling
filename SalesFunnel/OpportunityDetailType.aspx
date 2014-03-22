<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="OpportunityDetailType.aspx.vb" Inherits="OpportunityDetailType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%-- HEADER --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<%-- CONTENT --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/OpportunityDetailTypeList.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>

    <script type="text/javascript">
        hiddenFieldResourceID = '<%= hdnResourceID.ClientID %>';
        hiddenFieldMode = '<%= hdnMode.ClientID%>';
    </script>
    <asp:Panel ID="pnlOpportunityTypeList" runat="server">
        <div class="Table">
            <div class="Row">
                <div class="LeftCell">
                    <a id="lnkadd" onclick="addResource()" style="cursor: pointer;">Add Resource</a>
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
        <div style="clear:both">
            <asp:GridView ID="gvOpportunity" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" EmptyDataText="No Record Found" PageSize="15"
                 OnPageIndexChanging="gvOpportunity_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate><asp:CheckBox ID="chkSelectAll" runat="server" /></HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DETAIL_TYPE_ID")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DETAIL_TYPE_ID" HeaderText="Detail Type ID" SortExpression="DETAIL_TYPE_ID" />
                    <asp:BoundField DataField="DETAIL_TYPE_SHORT_NAME" HeaderText="Detail Name" SortExpression="DETAIL_TYPE_SHORT_NAME" />
                    <asp:BoundField DataField="DETAIL_TYPE_DESC" HeaderText="Detail Type Description" SortExpression="DETAIL_TYPE_DESC" />
                    <asp:BoundField DataField="DETAIL_TYPE_SORT_NBR" HeaderText="Detail Type Sort Number" SortExpression="DETAIL_TYPE_SORT_NBR" />
                    <asp:BoundField DataField="DETAIL_TYPE_VISIBLE_IND" HeaderText="Detail Type Visible" SortExpression="DETAIL_TYPE_VISIBLE_IND" />
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

    <asp:Panel ID="pnlOpportunityTypeDetail" runat="server">
        <div class="Table">
            <div class="Row">
                <div class="ActionButtonCell">
                    <!-- BUTTONS -->
                     <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this resource?")'/>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" UseSubmitBehavior="True" CausesValidation="False" OnClientClick="editClick()" />
                    <asp:Button ID="btnSave" runat="server" Text="Save"  OnClientClick='return saveResource()' />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" OnClientClick="cancelClick()"/>
                </div>
            </div>
        </div>

        <div class ="Table">
            <div class ="Row">
                <!-- Detail Type ID -->
                <div class="LeftCell">
                    <asp:Label ID="lblDetailTypeID" runat="server" Text="Detail tYpe ID"></asp:Label>
                </div>
                <div class="RightCell">
                    <!-- <asp:TextBox ID="txtDetailTypeID" runat="server" ControlToValidate="txtDetailTypeID" MaxLength="10" ></asp:TextBox> -->
                    <asp:Label ID="lblDetailTypeIDView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </div>
            </div>
            <div class ="Row">
                <!-- Detail Name -->
                <div class="LeftCell">
                    <asp:Label ID="lblDetailName" runat="server" Text="Detail Name"></asp:Label>
                </div>
                <div class="RightCell">
                    <asp:TextBox ID="txtDetailName" runat="server" ControlToValidate="txtDetailName" MaxLength="100" ></asp:TextBox>
                    <asp:Label ID="lblDetailNameView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </div>
            </div>
            <div class ="Row">
                <!-- Detail Description -->
                <div class="LeftCell">
                    <asp:Label ID="lblDetailDesc" runat="server" Text="Detail Type Description"></asp:Label>
                </div>
                <div class="RightCell">
                    <asp:TextBox ID="txtDetailDesc" runat="server" ControlToValidate="txtDetailDesc" MaxLength="100" ></asp:TextBox>
                    <asp:Label ID="lblDetailDescView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </div>
            </div>
            <div class ="Row">
                <!-- Detail Sort Number -->
                <div class="LeftCell">
                    <asp:Label ID="lblDetailSortNum" runat="server" Text="Detail Sort Number"></asp:Label>
                </div>
                <div class="RightCell">
                    <asp:TextBox ID="txtDetailSortNum" runat="server" ControlToValidate="txtDetailSortNum" MaxLength="100" ></asp:TextBox>
                    <asp:Label ID="lblDetailSortNumView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </div>
            </div>
            <div class ="Row">
                <!-- Detail Visible -->
                <div class="LeftCell">
                    <asp:Label ID="lblDetailVis" runat="server" Text="Detail Visible"></asp:Label>
                </div>
                <div class="RightCell">
                    <asp:TextBox ID="txtDetailVis" runat="server" ControlToValidate="txtDetailVis" MaxLength="100" ></asp:TextBox>
                    <asp:Label ID="lblDetailVisView" runat="server" Text="" CssClass="labelViewMode"></asp:Label>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hdnResourceID"  runat="server" value="-1"/>
    <asp:HiddenField ID="hdnMode"  runat="server" value="View"/>
</asp:Content>
