
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Public Class Product

    Private Shared _connectionString As String

    Private _id As Integer

    Private _title As String

    Private _director As String

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Director As String
        Get
            Return _director
        End Get
        Set(value As String)
            _director = value
        End Set
    End Property

    Public Sub Update(ByVal id As Integer, ByVal title As String, ByVal director As String)
        Dim con As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("ProductUpdate", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Id", id)
        cmd.Parameters.AddWithValue("@Title", title)
        cmd.Parameters.AddWithValue("@Director", director)
        'con()
        con.Open()
        cmd.ExecuteNonQuery()
    End Sub

    Public Function GetAll() As List(Of Product)
        Dim results As List(Of Product) = New List(Of Product)
        Dim con As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("ProductSelect", con)
        cmd.CommandType = CommandType.StoredProcedure
        'con()
        con.Open()
        Dim reader As SqlDataReader = cmd.ExecuteReader

        While reader.Read
            Dim newProduct As Product = New Product
            newProduct.Id = CType(reader("Id"), Integer)
            newProduct.Title = CType(reader("Title"), String)
            newProduct.Director = CType(reader("Director"), String)
            results.Add(newProduct)

        End While
        Return results
    End Function

    Shared Sub New()
        _connectionString = WebConfigurationManager.ConnectionStrings("Products").ConnectionString
    End Sub
End Class







