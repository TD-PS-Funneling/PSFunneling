Imports Microsoft.VisualBasic

Public Class SalesFunnelApp

    Dim _strUserName As String
    Dim _strPageMode As String




    Public Property UserName As String
        Get
            Return _strUserName
        End Get
        Set(ByVal value As String)
            _strUserName = value
        End Set
    End Property


    Public Property PageMode As String
        Get
            Return _strPageMode
        End Get
        Set(ByVal value As String)
            _strPageMode = value
        End Set
    End Property



End Class
