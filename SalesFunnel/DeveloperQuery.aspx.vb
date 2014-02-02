Imports System.Data


Partial Class DeveloperQuery
    Inherits System.Web.UI.Page

    Dim dtReturn As New DataTable

    Protected Sub btnExplain_Click(sender As Object, e As EventArgs) Handles btnExplain.Click
        Dim talSQL As New Teradata_Access_Layer
        dtReturn = talSQL.GetDataSet("EXPLAIN " & Trim(Me.txtSQL.Text)).Tables(0)
        BindData(dtReturn)
    End Sub

    Protected Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        Dim talSQL As New Teradata_Access_Layer
        dtReturn = talSQL.GetDataSet(Trim(Me.txtSQL.Text)).Tables(0)
        BindData(dtReturn)
    End Sub

    'Protected Sub gvResult_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvResult.Sorting
    '    If Session("DevSort") <> Convert.ToString(e.SortExpression) & " " & ConvertSortDirectionToSql(e.SortDirection) Then
    '        Session("DevSort") = Convert.ToString(e.SortExpression) & " " & ConvertSortDirectionToSql(e.SortDirection)
    '        BindData(dtReturn)
    '    End If
    'End Sub

    'Private Function ConvertSortDirectionToSql(sortDirection__1 As SortDirection) As String
    '    Dim newSortDirection As String = [String].Empty
    '    Select Case sortDirection__1
    '        Case SortDirection.Ascending
    '            newSortDirection = "ASC"
    '            Exit Select
    '        Case SortDirection.Descending
    '            newSortDirection = "DESC"
    '            Exit Select
    '    End Select
    '    Return newSortDirection
    'End Function

    Private Sub BindData(dtDataTable As DataTable)
        Dim dvgvSort As New DataView(dtDataTable)

        'If Not IsNothing(Session("DevSort")) Then
        '    dvgvSort.Sort = Session("DevSort")
        'End If

        Me.gvResult.DataSource = dvgvSort
        'Me.gvResult.PageIndex = CInt(Session("PageIndex"))
        Me.gvResult.DataBind()

    End Sub

End Class
