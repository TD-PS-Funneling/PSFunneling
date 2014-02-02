<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="OpportunityFile.aspx.vb" Inherits="OpportunityFile" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" Runat="Server" Tag="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server" Tag="MainContent">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
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
    <!--********************************************************************************-->
    <!--                                                                                -->
    <!-- View Panel                                                                     -->
    <!--                                                                                -->
    <!--********************************************************************************--> 
    <asp:Panel ID="pnlView" runat="server" BackColor="White" BorderColor="Black" Visible="false" Width="1200px">
        <asp:Table id="tblOpportunity" runat="server" Width="100%">
            <asp:TableRow>
            <asp:TableCell ColumnSpan="1" Width="200px"><asp:Label ID="lblAction" runat="server" Text="View Opportunity" Font-Size="Large" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" Width="250px"></asp:TableCell>
            <asp:TableCell ColumnSpan="1" Width="225px"></asp:TableCell>
            <asp:TableCell ColumnSpan="1" Width="225px"></asp:TableCell>
                <asp:TableCell ColumnSpan="1" Width="325px" HorizontalAlign="Right">
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" enabled="False" Visible="True"/>
                    <asp:Button ID="btnOpportunityOK" runat="server" Text="OK" enabled="False" Visible="false"/>
                    <asp:Button ID="btnOpportunityCancel" runat="server" Text="Cancel" enabled="false" Visible="false"/>
                    <asp:Button ID="btnOpportunityEdit" runat="server" Text="Edit" />
                    <asp:Button ID="btnBack" runat="server" Text="Back" enabled="true" Visible="true" />
                </asp:TableCell>
            </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" VerticalAlign="Top"><asp:Label ID="lblHierarchyEdit" runat="server" Text="Industry Hierarchy: " Font-Bold="True" ForeColor="Black" Width="325px" /></asp:TableCell>
            <asp:TableCell ColumnSpan="2" VerticalAlign="Top"><asp:Label ID="lblCustomerEdit" runat="server" Text="Customer: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblOpportunityEdit" runat="server" Text="Opportunity: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" VerticalAlign="Top"><asp:TextBox ID="lblDsplyHierarchyEdit" runat="server" BackColor="White" width="98%" Enabled="False"></asp:TextBox></asp:TableCell>
            <asp:TableCell ColumnSpan="2" VerticalAlign="Top"><asp:DropDownList ID="ddlCustomerEdit" runat="server" width="400px" Enabled="False" /><asp:Button ID="btnAddProspect" runat="server" Text="Add" Visible="False" /> </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:TextBox ID="txtOpportunityEdit" runat="server" width="445px" Enabled="False" Tag="X" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblFunnelPhaseEdit" runat="server" Text="Funnel Phase: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblCloseDateEdit" runat="server" Text="Close Date:" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="2" VerticalAlign="Top"><asp:Label ID="lblOpportunityComment" runat="server" Text="Opportunity Comment:" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblOpportunityDetail" runat="server" Text="Opportunity Detail: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" RowSpan="1"  VerticalAlign="Top"><asp:DropDownList ID="ddlFunnelPhaseEdit" runat="server" width="150px" Enabled="False" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" RowSpan="7"  VerticalAlign="Top" HorizontalAlign="Left"><asp:Calendar ID="calCloseDateEdit" runat="server"> </asp:Calendar>  </asp:TableCell>
            <asp:TableCell ColumnSpan="2" RowSpan="7"  VerticalAlign="Top">
                <asp:TextBox ID="txtOpportunityCommentEdit" runat="server" width="450px" height= "150px" Enabled="False" TextMode="MultiLine" />
                        <!--********************************************************************************-->
                        <!--                                                                                -->
                        <!-- Detail Panel                                                                   -->
                        <!--                                                                                -->
                        <!--********************************************************************************-->
                    <asp:Panel ID="pnlDetail" runat="server" Visible="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="5"  width="450px" height= "150px">
                        <asp:Table id="tblOpportunityDetailEdit" runat="server">
                             <asp:TableRow>
                                 <asp:TableCell ColumnSpan="2" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Label ID="lblOpportunityDetailEdit" runat="server" Text="Edit Opportunity Detail" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
                             </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Width="440px" VerticalAlign="Top" HorizontalAlign="Left">&nbsp;</asp:TableCell>
                            </asp:TableRow >
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="220px" VerticalAlign="Top" HorizontalAlign="Right"><asp:Label ID="lblOpportunityDetailTypeEdit" runat="server" Text="OpportunityDetailType"></asp:Label></asp:TableCell>
                                <asp:TableCell ColumnSpan="1" Width="220px" VerticalAlign="Top" HorizontalAlign="Left"><asp:DropDownList ID="ddlOpportunityDetailTypeEdit" runat="server" Enabled="False" /></asp:TableCell>
                             </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Width="440px" VerticalAlign="Top" HorizontalAlign="Left">&nbsp;</asp:TableCell>
                            </asp:TableRow >
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Width="440px" VerticalAlign="Top" HorizontalAlign="Left">&nbsp;</asp:TableCell>
                            </asp:TableRow >
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="220px" VerticalAlign="Top" HorizontalAlign="Right"><asp:Button ID="btnOpportunityDetailOK" runat="server" Text="Ok" /></asp:TableCell>
                                <asp:TableCell ColumnSpan="1" Width="220px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Button ID="btnOpportunityDetailCancel" runat="server" Text="Cancel" /> </asp:TableCell>
                             </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                    <!--********************************************************************************-->
                    <!--                                                                                -->
                    <!-- Message Panel                                                                  -->
                    <!--                                                                                -->
                    <!--********************************************************************************-->
                    <asp:Panel ID="pnlMsg" runat="server" Visible="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="2"  width="440px" height= "160px">
                        <asp:Table id="tblMSG" runat="server">
                             <asp:TableRow>
                                 <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Label ID="lblTitle" runat="server" Text="Title" Font-Bold="True" ForeColor="White" BackColor="Red" Width="430px"/></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Text="Message" Width="430px" Height="100px" BorderStyle="None" /></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Center"><asp:Button ID="btnMessageOK" runat="server" Text="Ok" /></asp:TableCell>
                             </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                    <!--********************************************************************************-->
                    <!--                                                                                -->
                    <!-- Add Customer                                                                  -->
                    <!--                                                                                -->
                    <!--********************************************************************************-->
                    <asp:Panel ID="pnlAddCustomer" runat="server" Visible="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="2"  width="440px" height= "160px">
                        <asp:Table id="tblAddCustomer" runat="server">
                             <asp:TableRow>
                                 <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Label ID="lblACP" runat="server" Text="Add Customer / Prospect" Font-Bold="True" ForeColor="White" BackColor="Blue" Width="430px"/></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Label ID="lblACCustomer" runat="server" Text="Customer Name:" Font-Bold="True" /></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:TextBox ID="txtACCustomerName" runat="server" Text="" Width="430px"/></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:Label ID="lblCustomerType" runat="server" Text="Customer/Prospect:" Font-Bold="True" /></asp:TableCell>
                             </asp:TableRow>
                             <asp:TableRow>
                                 <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Left"><asp:DropDownList ID="ddlCustomerType" runat="server" /></asp:TableCell>  
                             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="1" Width="440px" VerticalAlign="Top" HorizontalAlign="Center"><asp:Button ID="btnAddCustomerProspect" runat="server" Text="Ok" /><asp:Button ID="btnCancelAddCustomer" runat="server" Text="Cancel" Visible="True" /></asp:TableCell>
                             </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
            </asp:TableCell>
            <asp:TableCell ColumnSpan="1" RowSpan="12" VerticalAlign="Top">
                <asp:GridView ID="gvDetail" runat="Server" allowpaging="False" allowsorting="False"  PageSize="10" autogeneratecolumns="False" datakeynames="opportunity_state_detail_id"  emptydatatext="There are no data to display."  showfooter="false" enabled="false" >
                    <Columns> 
                        <asp:TemplateField ControlStyle-Width="50px" >
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetail" runat="server" Text = "Edit" CommandName="EditDetail" CommandArgument = '<%# Eval("opportunity_state_detail_id")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="opportunity_state_detail_id" HeaderText="opportunity_state_detail_id" visible="false"  />
                        <asp:BoundField DataField="opportunity_detail_id" HeaderText="opportunity_detail_id" visible="false"/>
                        <asp:BoundField DataField="detail_type_id" HeaderText="detail_type_id" visible="false"/>
                        <asp:BoundField DataField="detail_value_id" HeaderText="detail_type_id" visible="false"/>
                        <asp:BoundField DataField="detail_type_desc" HeaderText="Detail" HeaderStyle-ForeColor="Black" HeaderStyle-Width="138px" ItemStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Black"  ItemStyle-Width="125px" />   
                        <asp:BoundField DataField="detail_value_desc" HeaderText="Value" HeaderStyle-ForeColor="Black" HeaderStyle-Width="137px" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" ItemStyle-Width="125px" />   
                    </Columns>
                </asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblPSValueEdit" runat="server" Text="PS Value: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">
                <ajaxToolkit:NumericUpDownExtender ID="nudPSValue" runat="server" TargetControlID="txtPSValueEdit" Width="75" TargetButtonDownID="btnPSValueDown" TargetButtonUpID="btnPSValueUp"  Step="100" Tag="1" Maximum="1000000" Minimum="0" />
                <asp:TextBox ID="txtPSValueEdit" runat="server" width="75px" Enabled="False"  VerticalAlign="Top" /><asp:Button ID="btnPSValueUp" runat="server" Text="+" /><asp:Button ID="btnPSValueDown" runat="server" Text="-" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblWinPercentEdit" runat="server" Text="Win Percent: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">
                <ajaxToolkit:NumericUpDownExtender ID="nudWinPercent" runat="server" TargetControlID="txtWinPercentEdit" Width="75" TargetButtonDownID="btnWinPercentDown" TargetButtonUpID="btnWinPercentUp"  Step="5" Tag="2" Maximum="100" Minimum="0" />
                <asp:TextBox ID="txtWinPercentEdit" runat="server" width="75px" Enabled="False" /><asp:Button ID="btnWinPercentUp" runat="server" Text="+" /><asp:Button ID="btnWinPercentDown" runat="server" Text="-" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblOutlookStatus" runat="server" Text="Outlook Status:" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:DropDownList ID="ddlOutlookStatusEdit" runat="server" width="150px" Enabled="False" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblMPOverrideEdit" runat="server" Text="MP Override:" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblOrderFlowEdit" runat="server" Text="Order Flow:" Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>

        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:DropDownList ID="ddlMPOverrideEdit" runat="server" width="150px" Enabled="False" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:DropDownList ID="ddlOrderFlowEdit" runat="server" width="150px" Enabled="False" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblRevenueUpsideEdit" runat="server" Text="Revenue Upside: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:Label ID="lblOpportunityStatusEdit" runat="server" Text="Opportunity Status: " Font-Bold="True" ForeColor="Black" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">
                <ajaxToolkit:NumericUpDownExtender ID="nudRevenueUpside" runat="server" TargetControlID="txtRevenueUpsideEdit" Width="75" TargetButtonDownID="btnRevenueUpsideDown" TargetButtonUpID="btnRevenueUpsideUP"  Step="1" Tag="3" Maximum="1000000" Minimum="0" />
                <asp:TextBox ID="txtRevenueUpsideEdit" runat="server" width="75px" Enabled="False" /><asp:Button ID="btnRevenueUpsideUP" runat="server" Text="+" /><asp:Button ID="btnRevenueUpsideDown" runat="server" Text="-" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top"><asp:DropDownList ID="ddlOpportunityStatusEdit" runat="server" width="150px" Enabled="False" /></asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
            <asp:TableCell ColumnSpan="1" VerticalAlign="Top">&nbsp; </asp:TableCell>
        </asp:TableRow>
        </asp:Table>
    </asp:Panel>

</asp:Content>

