Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports Microsoft.VisualBasic
Imports Teradata.Client.Provider

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://teradata.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class FunnelAppSvc
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloFunnelAppSvc() As String
        Return "Hello Funnel Application Service"
    End Function

    <WebMethod()> _
    Public Function GetCustomerFilter() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT customer_name FROM FIHL_POC.Funnel_vw group by 1 order by 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetMPartnerFilter() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT mpr_last_name FROM FIHL_POC.Funnel_vw group by 1 order by 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetPSPartnerFilter() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT psp_last_name FROM FIHL_POC.Funnel_vw group by 1 order by 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetAreaFilter() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT organization_struct_name FROM FIHL_POC.Funnel_vw group by 1 order by 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetFunnelFilter() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT funnel_phase_short_name FROM FIHL_POC.Funnel_vw group by 1 order by 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetOpportunityList() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT * FROM FIHL_POC.Funnel_vw ORDER BY 1;")
        Return dtReturn
    End Function


    <WebMethod()> _
    Public Function GetCustomerList() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT customer_id, customer_name || ' - (' || customer_type_cd || ')' customer_name FROM FIHL_POC.customer WHERE end_date=DATE '9999-12-31'ORDER BY customer_name;")
        Return dtReturn
    End Function


    <WebMethod()> _
    Public Function GetOpportunityStatusList() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT opportunity_status_id, opportunity_status_short_name FROM FIHL_POC.util_opportunity_status ORDER BY 1;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetFunnelPhaseList() As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT funnel_phase_id, funnel_phase_short_name FROM FIHL_POC.util_funnel_phase order by funnel_phase_sort_nbr;")
        Return dtReturn
    End Function

    <WebMethod()> _
    Public Function GetDetailValueList(intDetailTYpeID As Long) As DataSet
        Dim dtReturn As New DataSet
        Dim dService As New FunnelDataSvc
        dtReturn = dService.GetDataSetSQL("SELECT detail_value_id, detail_value_short_name FROM FIHL_POC.util_opportunity_detail_value WHERE detail_type_id= " & intDetailTYpeID & " ORDER BY detail_value_sort_nbr;")
        Return dtReturn
    End Function



    <WebMethod()> _
    Public Function GetOpportunity(intOpportunityID As Integer) As DataSet
        Dim talTest As New Teradata_Access_Layer
        If intOpportunityID <> vbNull And intOpportunityID > 0 Then
            Return talTest.GetDataSet("SELECT * FROM FIHL_POC.opportunity_vw WHERE opportunity_id=" & intOpportunityID.ToString & ";")
        Else
            Return talTest.GetDataSet("")
        End If
    End Function


    <WebMethod()> _
    Public Function GetOpportunityDetails(intOpportunityID As Integer) As DataSet
        Dim talTest As New Teradata_Access_Layer
        If intOpportunityID <> Microsoft.VisualBasic.vbNull And intOpportunityID > 0 Then
            Return talTest.GetDataSet("SELECT * FROM FIHL_POC.opportunity_state_detail WHERE opportunity_id=" & intOpportunityID.ToString & ";")
        Else
            Return talTest.GetDataSet("")
        End If
    End Function


    <WebMethod()> _
    Public Function GetOpportunityDetail(intOpportunityStateID As Integer) As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.opportunity_state_detail_vw WHERE opportunity_state_id=" & intOpportunityStateID.ToString & " ORDER BY detail_type_sort_nbr;")
    End Function

    <WebMethod()> _
    Public Function GetAccountTeamList() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT account_team_id, account_team_desc FROM FIHL_POC.Account_Team ORDER BY account_team_desc;")
    End Function

    <WebMethod()> _
    Public Function GetCloseDateList() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT day_of_calendar, CAST( calendar_date AS DATE FORMAT 'mm/dd/yyyy')AS calendar_date FROM sys_calendar.CALBASICS WHERE calendar_date BETWEEN ADD_MONTHS(DATE, -3) AND ADD_MONTHS(DATE, 18) ORDER BY day_of_calendar;")
    End Function

    <WebMethod()> _
    Public Function GetOutlookListList() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT day_of_calendar, CAST( calendar_date AS DATE FORMAT 'mm/dd/yyyy')AS calendar_date FROM sys_calendar.CALBASICS WHERE calendar_date BETWEEN ADD_MONTHS(DATE, -3) AND ADD_MONTHS(DATE, 18) ORDER BY day_of_calendar;")
    End Function



    <WebMethod()> _
    Public Function GetOpportunityDetailList(intOpportunityStateDetailID As Integer) As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.opportunity_state_value_vw WHERE opportunity_state_detail_id=" & intOpportunityStateDetailID.ToString & "  ORDER BY detail_value_sort_nbr;")
    End Function

    <WebMethod()> _
    Public Function GetOpportunityDetailBlank() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("select 0 opportunity_state_detail_id, 0 opportunity_state_id, 0 opportunity_id, A.* from  FIHL_POC.opportunity_detail_value_vw A where detail_value_id<0 order by detail_type_sort_nbr;")
    End Function



    <WebMethod()> _
    Public Function GetResourcePosition(strLogin As String) As DataSet
        Dim talTest As New Teradata_Access_Layer
        strLogin = strLogin.ToUpper
        'If strLogin = "SSCOTT" Or strLogin = "SS185226" Then strLogin   = "HL180002"
        'If strLogin = "SSCOTT" Or strLogin = "SS185226" Then strLogin = "BG128062"
        'If strLogin = "SSCOTT" Or strLogin = "SS185226" Then strLogin = "KB185024"
        'If strLogin = "SSCOTT" Or strLogin = "SS185226" Then strLogin = "JK185071"
        'Session("strUserName") = strLogin
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.resource_position_vw WHERE qlookid='" & Trim(strLogin.ToUpper) & "';")
    End Function

    <WebMethod()>
    Public Function GetResourceList() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.resource ORDER BY resource_id;")
    End Function

    <WebMethod()>
    Public Function DelResource(ResourceID As String) As Integer
        Dim intResult As Integer = -1
        Dim cmd As New TdCommand
        Dim talTest As New Teradata_Access_Layer
        Dim intResourceID As Long

        Long.TryParse(ResourceID, intResourceID)
        cmd.CommandText = "FIHL_POC.SP_RESOURCE_DELETE"
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add("oMessage", TdType.Integer)
        cmd.Parameters.Add("iRESOURCE_ID", TdType.Integer)

        cmd.Parameters("oMessage").Direction = ParameterDirection.Output
        cmd.Parameters("iRESOURCE_ID").Value = intResourceID

        intResult = intResult + talTest.ExecCmd(cmd)

        Return intResult

    End Function

    <WebMethod()> _
    Public Function InHierarchy(intOrganizationStructID As Long, intCustomerID As Long) As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("EXEC FIHL_POC.inhierarchy(" & intOrganizationStructID & "," & intCustomerID & ");")
    End Function

    <WebMethod()> _
    Public Function InOrganizationHierarchy(intOrganizationStructID As Long, strUserName As String) As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet(" EXEC FIHL_POC.organization_inhierarchy(" & intOrganizationStructID & ",'" & strUserName & "');")
    End Function

    <WebMethod()> _
    Public Function GetHierarchy(intOrganizationStructID As Long) As Integer
        Dim intReturn As Integer = -1
        Dim talTest As New Teradata_Access_Layer
        Dim dsCheck As DataSet = talTest.GetDataSet("EXEC FIHL_POC.Get_hrrchy_id(" & intOrganizationStructID & ");")
        If dsCheck.Tables(0).Rows.Count > 0 Then
            intReturn = dsCheck.Tables(0).Rows(0).Item(0)
        End If
        Return intReturn
    End Function




    <WebMethod()> _
    Public Function GetExpandedHierarchy(intOpportunityStructID As Long) As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.resource_position_vw WHERE qlookid='" & Trim(intOpportunityStructID) & "';")
    End Function

    <WebMethod()> _
    Public Function GetNextOpportunityID() As Integer
        Dim intOpportunityID As Integer = 0
        Dim talTest As New Teradata_Access_Layer
        Dim dsNextOpportunityID As DataSet = talTest.GetDataSet("SELECT MAX(COALESCE(opportunity_id,0))+1 next_opportunity_id FROM FIHL_POC.opportunity;")
        If Not IsNothing(dsNextOpportunityID) Then
            intOpportunityID = dsNextOpportunityID.Tables(0).Rows(0).Item(0)
        End If
        Return intOpportunityID
    End Function
    <WebMethod()> _
    Public Function AddOpportunity(intCustomerID As Integer, strOpportunityDesc As String) As Integer
        Dim intResult As Integer = -1
        Dim intOpportunityID As Integer = GetNextOpportunityID()
        Dim strSQL As String = "INSERT INTO FIHL_POC.opportunity VALUES(" & intOpportunityID.ToString & ", " & intCustomerID.ToString & ", -1, '" & strOpportunityDesc & "', PERIOD(DATE, UNTIL_CHANGED));"
        Dim talTest As New Teradata_Access_Layer

        intResult = talTest.ExecSQL(strSQL)

        Return (intResult * intOpportunityID)
    End Function

    <WebMethod()> _
    Public Function GetNextOpportunityStateID() As Integer
        Dim intOpportunityStateID As Integer = 0
        Dim talTest As New Teradata_Access_Layer
        Dim dsNextOpportunityStateID As DataSet = talTest.GetDataSet("SELECT MAX(COALESCE(opportunity_state_id,0))+1 next_opportunity_state_id FROM FIHL_POC.opportunity_state;")
        If Not IsNothing(dsNextOpportunityStateID) Then
            intOpportunityStateID = dsNextOpportunityStateID.Tables(0).Rows(0).Item(0)
        End If
        Return intOpportunityStateID
    End Function

    <WebMethod()> _
    Public Function AddCustomerProspect(strCustomerName As String, strCustomerType As String, intHierachy As Integer) As Integer
        Dim intResult As Integer = -1
        Dim strSQL As String = "INSERT INTO FIHL_POC.customer SELECT MAX(customer_id) +1, '0', '" & strCustomerType & "', '0', 'US', '" & strCustomerName & "', '0' , 0, CURRENT_DATE, MAX(end_date) FROM FIHL_POC.customer;"
        Dim talTest As New Teradata_Access_Layer
        intResult = talTest.ExecSQL(strSQL)

        If intResult > 0 Then
            strSQL = "INSERT INTO FIHL_POC.customer_organization SELECT MAX(customer_organization_id) +1, MAX(customer_id) +1, " & intHierachy & ", CURRENT_DATE, MAX(end_date) FROM FIHL_POC.customer_organization;"
            intResult = talTest.ExecSQL(strSQL)
        End If

        Return intResult
    End Function




    <WebMethod()> _
    Public Function AddOpportunityState(intOpportunityID As Integer, dtClose As Date, intPSValue As Integer, intFunnelPhaseID As Integer, bytWinPercent As Byte, bytOpportunityStatusID As Byte, strOpportunityComment As String, intRevenueUpside As Integer, strQuicklookID As String) As Integer
        Dim intResult As Integer = -1
        Dim intOpportunityStateID As Integer = GetNextOpportunityStateID()
        Dim strSQL As String = "INSERT INTO FIHL_POC.opportunity_state VALUES(" & intOpportunityStateID.ToString & ", " & intOpportunityID.ToString & ", DATE'" & Format(dtClose, "yyyy-MM-dd") & "', " & intPSValue.ToString & ", " & intFunnelPhaseID.ToString & ", " & bytWinPercent.ToString & ", " & bytOpportunityStatusID.ToString & ", '" & strOpportunityComment & "'," & intRevenueUpside.ToString & ", PERIOD(DATE, UNTIL_CHANGED), '" & Format(Now, "HH:mm:ss").ToString & "', '" & strQuicklookID & "');"
        Dim talTest As New Teradata_Access_Layer

        intResult = talTest.ExecSQL(strSQL)

        Return (intResult * intOpportunityStateID)
    End Function


    <WebMethod()> _
    Public Function AddOpportunityStateDetail(intOpportunityStateID As Integer) As Integer
        Dim talTest As New Teradata_Access_Layer
        Dim intResult As Integer = -1
        Dim strSQL As String = "INSERT INTO FIHL_POC.opportunity_state_detail "
        strSQL += " SELECT ROW_NUMBER() OVER(PARTITION BY 1 ORDER BY ODT.detail_type_id) + NXT.opportunity_state_detail_id, OST.opportunity_state_id, OST.opportunity_id, "
        strSQL += " ODT.detail_type_id, ODT.detail_type_id * -1, NULL, CURRENT_DATE, DATE'9999-12-31', OST.update_time, OST.update_qlookid "
        strSQL += " FROM FIHL_POC.opportunity_state OST "
        strSQL += " INNER JOIN FIHL_POC.util_opportunity_detail_type ODT ON 1=1 "
        strSQL += " INNER JOIN (SELECT MAX(COALESCE(opportunity_state_detail_id,0)) opportunity_state_detail_id FROM FIHL_POC.opportunity_state_detail) NXT ON 1=1 "
        strSQL += " WHERE OST.opportunity_state_id=" & intOpportunityStateID.ToString & ";"

        intResult = talTest.ExecSQL(strSQL)

        Return intResult
    End Function

    <WebMethod()> _
    Public Function SetOpportunity(intOpportunityID As Integer, intCustomerID As Integer, intAccountTeamID As Integer, strOpportunityDesc As String) As Integer
        Dim talTest As New Teradata_Access_Layer
        Dim intResult As Integer = -1
        Dim strSQL As String = "UPDATE FIHL_POC.opportunity "
        strSQL += " SET account_team_id=" & intAccountTeamID.ToString & ", "
        strSQL += " opportunity_desc='" & strOpportunityDesc & "' "
        strSQL += " WHERE opportunity_id=" & intOpportunityID.ToString & " AND customer_id=" & intCustomerID.ToString & ";"""

        intResult = talTest.ExecSQL(strSQL)

        Return intResult
    End Function

    <WebMethod()> _
    Public Function SetOpportunityState(intOpportunityStateID As Integer, intOpportunityID As Integer, dtClose As Date, intPSValue As Integer, intFunnelPhaseID As Integer, bytWinPercent As Byte, intOpportunityStatusID As Integer, strOpportunityDesc As String, intRevenueUpside As Integer, strQuicklookID As String) As Integer
        Dim talTest As New Teradata_Access_Layer
        Dim intResult As Integer = -1
        Dim strSQL As String = "UPDATE FIHL_POC.opportunity_state "
        strSQL += " SET close_date= DATE'" & Format(dtClose, "yyyy-MM-dd") & "', "
        strSQL += " ps_value_nbr=" & intPSValue.ToString & ", "
        strSQL += " funnel_phase_id=" & intFunnelPhaseID.ToString & ", "
        strSQL += " win_percent=" & bytWinPercent.ToString & ", "
        strSQL += " opportunity_status_id=" & intOpportunityStatusID.ToString & ", "
        strSQL += " opportunity_desc='" & strOpportunityDesc & "', "
        strSQL += " revenue_upside=" & intRevenueUpside.ToString & ", "
        strSQL += " update_time= '" & Format(Now, "HH:mm:ss").ToString & "', "
        strSQL += " update_qlookid='" & strQuicklookID & "' "
        strSQL += " WHERE opportunity_state_id=" & intOpportunityStateID.ToString & " AND opportunity_id=" & intOpportunityID.ToString & ";"

        intResult = talTest.ExecSQL(strSQL)

        Return intResult
    End Function



    <WebMethod()> _
    Public Function SetOpportunityStateDetail(intOpportunityStateDetailID As Integer, intOpportunityID As Integer, intValue As Integer, strQuicklookID As String) As Integer
        Dim talTest As New Teradata_Access_Layer
        Dim intResult As Integer = -1
        Dim strSQL As String = "UPDATE FIHL_POC.opportunity_state_detail "
        strSQL += " SET detail_value_id=" & intValue.ToString & ", "
        strSQL += " update_time= '" & Format(Now, "HH:mm:ss").ToString & "', "
        strSQL += " update_qlookid='" & strQuicklookID & "' "
        strSQL += " WHERE opportunity_state_detail_id=" & intOpportunityStateDetailID.ToString & " AND opportunity_id=" & intOpportunityID.ToString & ";"

        intResult = talTest.ExecSQL(strSQL)

        Return intResult
    End Function

    <WebMethod()> _
    Public Function SetOpportunityStateDetail2(intDetailTypeID As Integer, intOpportunityID As Integer, intValue As Integer, strQuicklookID As String) As Integer
        Dim talTest As New Teradata_Access_Layer
        Dim intResult As Integer = -1
        Dim strSQL As String = "UPDATE FIHL_POC.opportunity_state_detail "
        strSQL += " SET detail_value_id=" & intValue.ToString & ", "
        strSQL += " update_time= '" & Format(Now, "HH:mm:ss").ToString & "', "
        strSQL += " update_qlookid='" & strQuicklookID & "' "
        strSQL += " WHERE detail_type_id=" & intDetailTypeID.ToString & " AND opportunity_id=" & intOpportunityID.ToString & " AND end_date= DATE '9999-12-31';"

        intResult = talTest.ExecSQL(strSQL)

        Return intResult
    End Function


    <WebMethod()> _
    Public Function GetReportSummaryPhase() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.rpt_phase_summary ORDER BY 1;")
    End Function
    <WebMethod()> _
    Public Function GetReportSummaryAccountTeam() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.rpt_account_team_summary ORDER BY 1;")
    End Function
    <WebMethod()> _
    Public Function GetReportSummaryArea() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.rpt_area_summary ORDER BY 1;")
    End Function
    <WebMethod()> _
    Public Function GetReportSummaryWinPercent() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.rpt_win_percent_summary ORDER BY 1 DESC;")
    End Function


    <WebMethod()> _
    Public Function GetTDHierarchy() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.td_hier_vw ORDER BY organization_hrrchy_id, organization_struct_parent_id, organization_struct_name;")
    End Function

    <WebMethod()> _
    Public Function GetTDFullHierarchy() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.td_hier_full_vw ORDER BY organization_hrrchy_id, organization_struct_parent_id, organization_struct_name;")
    End Function


    <WebMethod()> _
    Public Function GetOrganizationHierarchy() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT * FROM FIHL_POC.organization_hrrchy_full_vw ORDER BY organization_full_name;")
    End Function

    <WebMethod()> _
    Public Function GetImpersonationList() As DataSet
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("SELECT qlookid, last_name || ', ' || first_name || '  (' ||  qlookid || ')'  full_name  FROM FIHL_POC.resource_position_vw ORDER BY last_name, first_name;")
    End Function

End Class