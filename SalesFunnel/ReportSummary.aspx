<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportSummary.aspx.vb" Inherits="ReportSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmReportSummary" runat="server">
    <div>
    
        <asp:Table ID="tblReport" runat="server">
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="2" Width="50%" HorizontalAlign="Center" ><asp:Label ID="lblTitle" runat="server" Text="Summary Report" Font-Size="Large" ForeColor="Black" Font-Bold="True" Font-Underline="True" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="2" Width="50%" HorizontalAlign="Center" >&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="2" Width="50%" HorizontalAlign="Center" >&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="1" Width="50%" HorizontalAlign="Center" VerticalAlign="Top">
                    <asp:GridView ID="gvPhase" runat="Server" allowpaging="False" allowsorting="False"  autogeneratecolumns="False" emptydatatext="There are no data to display."  showfooter="True" enabled="true" BorderStyle="Solid" BorderWidth="4" BorderColor="Black">
                        <Columns> 
                            <asp:BoundField DataField="funnel_phase_short_name" HeaderText="Phase" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Black"  />   
                            <asp:BoundField DataField="ps_value" HeaderText="PS Value" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:c}"/>   
                            <asp:BoundField DataField="ps_value_percent" HeaderText="Percent of Total" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:P2}" />   
                        </Columns>
                    </asp:GridView>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" Width="50%" HorizontalAlign="Center" VerticalAlign="Top" >
                    <asp:GridView ID="gvArea" runat="Server" allowpaging="False" allowsorting="False"  autogeneratecolumns="False" emptydatatext="There are no data to display."  showfooter="True" enabled="true" BorderStyle="Solid" BorderWidth="4" BorderColor="Black">
                        <Columns> 
                            <asp:BoundField DataField="organization_struct_name" HeaderText="Area" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Black"  />   
                            <asp:BoundField DataField="ps_value" HeaderText="PS Value" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:c}"/>   
                            <asp:BoundField DataField="ps_value_percent" HeaderText="Percent of Total" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:P2}" />   
                        </Columns>
                    </asp:GridView>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="2" Width="50%" HorizontalAlign="Center" >&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="1" Width="50%" HorizontalAlign="Center" VerticalAlign="Top" >
                    <asp:GridView ID="gvAccount" runat="Server" allowpaging="False" allowsorting="False"  autogeneratecolumns="False" emptydatatext="There are no data to display."  showfooter="True" enabled="true" BorderStyle="Solid" BorderWidth="4" BorderColor="Black">
                        <Columns> 
                            <asp:BoundField DataField="account_team_desc" HeaderText="Account Team" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Black"  />   
                            <asp:BoundField DataField="ps_value" HeaderText="PS Value" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:c}" />   
                            <asp:BoundField DataField="ps_value_percent" HeaderText="Percent of Total" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:P2}" />   
                        </Columns>
                    </asp:GridView>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" Width="50%" HorizontalAlign="Center" VerticalAlign="Top" >
                    <asp:GridView ID="gvWinPercent" runat="Server" allowpaging="False" allowsorting="False"  autogeneratecolumns="False" emptydatatext="There are no data to display."  showfooter="True" enabled="true" BorderStyle="Solid" BorderWidth="4" BorderColor="Black">
                        <Columns> 
                            <asp:BoundField DataField="win_percent" HeaderText="Probability" HeaderStyle-ForeColor="Black"  ItemStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Black"  />   
                            <asp:BoundField DataField="ps_value" HeaderText="PS Value" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:c}"/>   
                            <asp:BoundField DataField="ps_value_percent" HeaderText="Percent of Total" HeaderStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Black" DataFormatString="{0:P2}" />   
                        </Columns>
                    </asp:GridView>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow BorderStyle="Solid" BorderColor="Black" BorderWidth="2">
                <asp:TableCell ColumnSpan="1" Width="50%" ></asp:TableCell>
                <asp:TableCell ColumnSpan="1" Width="50%" ></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

    </div>
    </form>
</body>
</html>
