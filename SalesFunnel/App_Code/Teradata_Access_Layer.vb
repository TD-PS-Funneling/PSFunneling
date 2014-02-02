Option Explicit On

Imports Microsoft.VisualBasic
Imports System.Data
Imports Teradata.Client.Provider

Public Class Teradata_Access_Layer
    Dim cnTD As New Teradata.Client.Provider.TdConnection
    Private blnConnected As Boolean = False
    'Private td As Teradata.Client.Provider.TdConnection

    Private strDataSource As String
    Private strUserID As String
    Private strPassword As String

    Public Enum Environment
        Development = 1
        SystemTest = 2
        UserTest = 3
        Production = 4
    End Enum


    Public Sub New()
        Me.InitConn(ConfigurationManager.AppSettings("DevCon"))
    End Sub

    'Public Sub New(Optional env As Environment = 1)
    '    Dim strconnstring As String
    '    Select Case env
    '        Case 1
    '            strconnstring = configurationmanager.appsettings("devcon")
    '        Case 2
    '            strconnstring = configurationmanager.appsettings("syscon")
    '        Case 3
    '            strconnstring = configurationmanager.appsettings("testcon")
    '        Case 4
    '            strconnstring = configurationmanager.appsettings("prodcon")
    '    End Select
    '    call new(strconnstring)
    'End Sub

    Public Sub New(strConnString As String)
        Me.InitConn(strConnString)
    End Sub

    Public Sub InitConn(strConnString As String)
        'cnTD.ConnectionString='Data Source=myServerAddress;User ID=myUsername;Password=myPassword;'
        ' cnTD.ConnectionString = "Data Source=t00drecop2.td.teradata.com;User ID=FIHL_USER;Password=fihluser;"
        cnTD.ConnectionString = strConnString

        Try


        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub

    Public Function OpenConn() As Boolean
        If blnConnected = False Then
            Try
                cnTD.Open()
                blnConnected = True
            Catch ex As Exception
                MsgBox(ex.Message.ToString)
            End Try
        End If
        Return blnConnected
    End Function
    Public Function CloseConn() As Boolean
        If blnConnected = True Then
            Try
                cnTD.Close()
                blnConnected = False
            Catch ex As Exception
                MsgBox(ex.Message.ToString)
            End Try
        End If
        Return blnConnected
    End Function



    Public Function TestGetDataSet(Optional strSQL As String = "SELECT COUNT(*) RC FROM FIHL_POC.organization;") As Data.DataSet
        'strSQL = "SELECT COUNT(*) RC FROM FIHL_POC.organization;"
        Return GetDataSet(strSQL)
    End Function



    Public Function GetDataSet(ByVal strSQL As String) As Data.DataSet

        Dim dsTD As New DataSet()
        If GetCharacterCount(strSQL, ";") <= 1 Then
            'Dim input As String = "Hello there. My name is John. I work very hard. Hello there!"
            'Dim phrase As String = "Hello there"
            'Dim Occurrences As Integer = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length


            Dim daTD As TdDataAdapter = New TdDataAdapter(strSQL, cnTD)
            Try
                OpenConn()
                daTD.Fill(dsTD)
                daTD.Dispose()
                CloseConn()
            Catch ex As Exception

            End Try
        End If
        Return dsTD
    End Function



    Public Function ExecSQL(ByVal strSQL As String) As Integer
        Dim intResult As Integer = -1

        If GetCharacterCount(strSQL, ";") <= 1 Then
            Dim cmdTD As New TdCommand(strSQL, cnTD)
            Try
                OpenConn()
                intResult = cmdTD.ExecuteNonQuery()
                CloseConn()
            Catch ex As Exception
            End Try
        End If
        Return intResult
    End Function

    ''' <summary>
    ''' Executes a command that doesn't return any data. i.e. stored procedured
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Author: Jericho Segubre (JS186060)
    ''' Date: 01/28/2014
    ''' </remarks>
    Public Function ExecCmd(ByVal command As TdCommand) As Integer
        Dim intResult As Integer = -1

        Try
            OpenConn()
            command.Connection = cnTD

            intResult = command.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            CloseConn()
        End Try

        Return intResult
    End Function

    Public Function GetDataTable(ByVal strSQL As String) As DataTable
        Dim dtReturn As New DataTable
        If GetCharacterCount(strSQL, ";") <= 1 Then
            'Dim input As String = "Hello there. My name is John. I work very hard. Hello there!"
            'Dim phrase As String = "Hello there"
            'Dim Occurrences As Integer = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length


            Dim daTD As TdDataAdapter = New TdDataAdapter(strSQL, cnTD)
            Try
                OpenConn()
                daTD.Fill(dtReturn)
                daTD.Dispose()
                CloseConn()
            Catch ex As Exception

            End Try
        End If
        Return dtReturn
    End Function




    Public Function TestItAll() As Integer
        Dim intReturn As Integer = -314159265




        Return intReturn
    End Function




    Public Function GetData(strSQL As String) As Integer
        Dim intVal As Integer = 0
        Dim cmdData As TdCommand = New TdCommand("SELECT COUNT(*) RC FROM FIHL_POC.organization;", cnTD)
        Try
            'cnTD.ConnectionString = "Data Source=t00drecop2.td.teradata.com;User ID=FIHL_USER;Password=fihluser;"
            cnTD.Open()

            Dim drData As TdDataReader = cmdData.ExecuteReader()

            If drData.HasRows Then
                drData.Read()
                intVal = drData.Item(0)
            Else
                MsgBox("No rows found.")
            End If

            drData.Close()
            cnTD.Close()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
        Return intVal
    End Function

    Private Function GetCharacterCount(ByVal strVal As String, ByVal chVal As Char) As Integer
        Dim intCount As Integer = 0
        For Each c As Char In strVal
            If c = chVal Then intCount += 1
        Next
        Return intCount
    End Function
End Class


