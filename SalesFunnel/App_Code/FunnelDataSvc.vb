Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://teradata.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class FunnelDataSvc
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloFunnelDataSvc() As String
        Return "Hello Funnel Data Service"
    End Function

    <WebMethod()> _
    Public Function GetDataSetSQL(strSQL As String) As DataSet
        Dim talReturn As New Teradata_Access_Layer
        Return talReturn.GetDataSet(strSQL)
    End Function
    <WebMethod()> _
    Public Function GetDatTableSQL(strSQL As String) As DataTable
        Dim talReturn As New Teradata_Access_Layer
        Return talReturn.GetDataTable(strSQL)
    End Function

    <WebMethod()> _
    Public Function GetDataSetCommand(cmdSQL As Object) As DataSet 'as TdCommand
        Dim talTest As New Teradata_Access_Layer
        Return talTest.GetDataSet("")
    End Function

    <WebMethod()> _
    Public Function ExecuteSQL(strSQL As String) As Integer
        Dim talTest As New Teradata_Access_Layer
        Return talTest.ExecSQL(strSQL)
    End Function

    <WebMethod()> _
    Public Function ExecuteCommand(cmdSQL As Object) As Integer 'as TdCommand

        Return 0
    End Function

End Class