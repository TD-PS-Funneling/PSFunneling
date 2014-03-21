<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="OpportunityFileList.aspx.vb" Inherits="OpportunityFileList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" Runat="Server" Tag="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server" Tag="MainContent">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

    <!--********************************************************************************-->
    <!--                                                                                -->
    <!-- File Panel                                                                     -->
    <!--                                                                                -->
    <!--********************************************************************************--> 
    <asp:Panel ID="pnlFile" runat="server" Width="100%" Height="500px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1">    
         <div class="divFile">
             <asp:Label ID="lblFile" runat="server" Text="Opportunity Explorer" ForeColor="White" BackColor="Blue" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="100%" Font-Bold="True"></asp:Label>
            <asp:TreeView ID="tvMain" runat="server" ImageSet ="XPFileExplorer" SelectedNodeStyle-HorizontalPadding="5" SelectedNodeStyle-BackColor="Silver" SelectedNodeStyle-Font-Bold="True" BorderStyle="None" Height="100%" Width="100%"></asp:TreeView>
        </div>
    </asp:Panel>
    <!--********************************************************************************-->
    <!--                                                                                -->
    <!-- File Unauthorized                                                              -->
    <!--                                                                                -->
    <!--********************************************************************************--> 
    <asp:Panel ID="pnlUnauthorized" runat="server" Width="100%" Height="500px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Visible="False" HorizontalAlign="Center">    
        <br />
        <br />
        <br />
        <asp:Label ID="lblUnauthorized1" runat="server" Text="You do not have permission to view this website." ForeColor="#FF3300" Font-Size="XX-Large" Font-Bold="True"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblUnauthorized2" runat="server" Text="If you feel that you have reached this page in error, please contact your system administrator." ForeColor="#FF3300" Font-Size="X-Large" Font-Bold="True"></asp:Label>
    </asp:Panel>
    <!--********************************************************************************-->
    <!--                                                                                -->
    <!-- Main Panel                                                                     -->
    <!--                                                                                -->
    <!--********************************************************************************--> 
    <asp:Panel ID="pnlMain" runat="server" BackColor="White" BorderColor="Black" Visible="False">
        <asp:Label ID="lblSearch" runat="server" Text="Search" Font-Bold="True" Font-Size="Large" BorderColor="Black" ForeColor="Black" />
        <br />
        <asp:Menu ID="mnuMain" runat="server" StaticEnableDefaultPopOutImage="false"  DynamiceEnableDefaultPopOutImage="True"  BackColor="#CCCCCC" DynamicEnableDefaultPopOutImage="False" ForeColor="Black" DynamicMenuItemStyle-HorizontalPadding="4" DynamicMenuItemStyle-VerticalPadding="2" DynamicMenuStyle-HorizontalPadding="4" DynamicMenuStyle-VerticalPadding="2" RenderingMode="List" DynamicHoverStyle-BackColor="#999999" DynamicMenuStyle-BackColor="#CCCCCC" StaticHoverStyle-BackColor="#999999" DynamicMenuItemStyle-BackColor="#CCCCCC" Orientation="Horizontal"></asp:Menu>
 
        <asp:Table ID="tblSearch" runat="server" EnableViewState="true">
            <asp:TableRow ID="trSearch" runat="server" ForeColor="Black" ItemStyle-HorizontalAlign="Center" BorderStyle="Solid" Width="100%"> 
                <asp:TableCell Width="700px"  HorizontalAlign="Left" ><asp:Label ID="lblHierarchy" runat="server" Text="Industry Hierachy" width="555px" Font-Bold="True" ViewStateMode="Inherit" /><br /><asp:TextBox ID="txtHierarchy" runat="server" BackColor="White" width="65%" Enabled="False" />&nbsp;&nbsp;<asp:Button ID="btnHierarchy" runat="server" Text="Change Hierarchy" />   </asp:TableCell> 
                <asp:TableCell Width="155px" HorizontalAlign="Center" ><asp:Label ID="lblFunnelPhase" runat="server" Text="Funnel Phase" width="150px" Font-Bold="True" /><br /><asp:DropDownList ID="ddlFunnelPhase" runat="server" width="150px"/></asp:TableCell> 
                <asp:TableCell Width="125px"><asp:Label ID="lblStatus" runat="server" Text="Status" width="90px" Font-Bold="True" /><br /><asp:DropDownList ID="ddlStatus" runat="server" width="90px"/></asp:TableCell> 
                <asp:TableCell HorizontalAlign="Center"><br /><asp:Button ID="btnSearch" runat="server" Text="Search" />&nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" /></asp:TableCell> 
            </asp:TableRow>  
        </asp:Table>

        <asp:GridView ID="gvMain" runat="Server" AllowPaging ="True" PagerSettings-Mode="NextPreviousFirstLast" PagerSettings-FirstPageText="First" PagerSettings-PreviousPageText="Previous" PagerSettings-NextPageText="Next" PagerSettings-LastPageText="Last"  allowsorting="true"  PageSize="15" autogeneratecolumns="False" datakeynames="opportunity_id"  emptydatatext="There are no data to display." showfooter="false" Width="100%" >
            <Columns> 
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnView" runat="server" Text = "View" CommandName="View" CommandArgument = '<%# Eval("opportunity_id")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="opportunity_id" HeaderText="opportunity_id" visible="false"/>   
                <asp:BoundField DataField="customer_name" HeaderText="Customer" SortExpression="customer_name" ItemStyle-HorizontalAlign="Left"/>     
                <asp:BoundField DataField="opportunity_desc" HeaderText="Opportunity" SortExpression="opportunity_desc" ItemStyle-HorizontalAlign="Left"/>       
                <asp:BoundField DataField="ps_value_nbr" HeaderText="PS Value" DataFormatString="{0:C0}" SortExpression="ps_value_nbr" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="close_date" HeaderText="Close Date" DataFormatString="{0:d}" SortExpression="close_date" ItemStyle-HorizontalAlign="Center"/>  
                <asp:BoundField DataField="funnel_phase_short_name" HeaderText="Funnel Phase" SortExpression="funnel_phase_short_name" ItemStyle-HorizontalAlign="Center"/>  
                <asp:BoundField DataField="win_percent" HeaderText="Win Percent" SortExpression="win_percent" ItemStyle-HorizontalAlign="Right"/>  
                <asp:BoundField DataField="current_outlook" HeaderText="Outlook Status" SortExpression="current_outlook"  ItemStyle-HorizontalAlign="Center"/>  
                
                
                <asp:BoundField DataField="revenue_upside" HeaderText="Revenue Upside" SortExpression="revenue_upside" ItemStyle-HorizontalAlign="Center"/>  
                <asp:BoundField DataField="customer_id" HeaderText="customer_id" visible="false"/>  
                <asp:BoundField DataField="L0ID" HeaderText="L0ID" visible="false"/>  
                <asp:BoundField DataField="L1ID" HeaderText="L1ID" visible="false"/>  
                <asp:BoundField DataField="L2ID" HeaderText="L2ID" visible="false"/>  
                <asp:BoundField DataField="L3ID" HeaderText="L3ID" visible="false"/>  
                <asp:BoundField DataField="L4ID" HeaderText="L4ID" visible="false"/>  
                <asp:BoundField DataField="L5ID" HeaderText="L5ID" visible="false"/> 
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="Ad" />
    </asp:Panel>
</asp:Content>
