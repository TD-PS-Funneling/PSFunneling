Option Explicit On

Imports System.Data
Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services

Partial Class ResourceList
    Inherits System.Web.UI.Page
    Dim sysUser As System.Security.Principal.IPrincipal

    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = Nothing



#Region "Page Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            DeleteResource()
            dtGridView = appService.GetResourceList().Tables(0)
            Me.BindData()

        End If
    End Sub
#End Region

#Region "GridView"

    Protected Sub gvResource_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvResource.PageIndexChanging
        ViewState("PageIndex") = e.NewPageIndex
        dtGridView = appService.GetResourceList().Tables(0)
        BindData()
    End Sub

    Protected Sub gvResource_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvResource.Sorting
        ViewState("SortExpression") = e.SortExpression
        If Not IsNothing(ViewState("SortDirection")) Then
            If (e.SortDirection = SortDirection.Ascending) Then
                ViewState("SortDirection") = SortDirection.Descending.ToString()
            Else
                ViewState("SortDirection") = SortDirection.Ascending.ToString()
            End If

        Else
            ViewState("SortDirection") = e.SortDirection
        End If


        dtGridView = appService.GetResourceList().Tables(0)
        BindData()
    End Sub

    Protected Sub gvResource_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvResource.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim strId As String = e.Row.Cells(1).Text
            Dim lnkview As LinkButton = CType(e.Row.Cells(9).Controls(1), LinkButton)
            Dim lnkedit As LinkButton = CType(e.Row.Cells(10).Controls(1), LinkButton)

            lnkview.PostBackUrl = "Resource.aspx?ID=" & strId & "&mode=View"
            lnkedit.PostBackUrl = "Resource.aspx?ID=" & strId & "&mode=Edit"

        End If

        'If e.Row.RowType = DataControlRowType.Header Then
        '    Dim chkSelectAll As CheckBox = CType(e.Row.Cells(0).Controls(0), CheckBox)
        '    chkSelectAll.Attributes.Add("onclick", "setAllElements(this.id);")
        'End If
    End Sub
#End Region

#Region "Drop Down List"
    Protected Sub ddlDisplay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDisplay.SelectedIndexChanged
        dtGridView = appService.GetResourceList().Tables(0)
        BindData()
    End Sub
#End Region

#Region "Private"
    'Data binding to Grid View
    Private Sub BindData()
        Dim dvSort As New DataView(dtGridView)
        Dim strfilter As String = String.Empty

        If (Not IsNothing(ViewState("SortDirection"))) And (Not IsNothing(ViewState("SortExpression"))) Then
            dvSort.Sort = ViewState("SortExpression").ToString() + " " + SortDirectionSql(ViewState("SortDirection").ToString())
        End If

        Me.gvResource.DataSource = dvSort
        Dim intIndex As Integer = 0

        If Not IsNothing(ViewState("PageIndex")) Then
            intIndex = CInt(ViewState("PageIndex"))
        End If

        Me.gvResource.PageSize = CInt(ddlDisplay.SelectedValue)
        Me.gvResource.PageIndex = intIndex
        Me.gvResource.DataBind()

    End Sub

    Private Function SortDirectionSql(sortDirection As String) As String
        Dim strSort As String = "ASC"
        Select Case (sortDirection.ToLower())
            Case "ascending"
                strSort = "ASC"
            Case "descending"
                strSort = "DESC"
        End Select

        Return strSort
    End Function

    Private Sub DeleteResource()
        If IsNothing(Request.QueryString("items")) = False Then

            Dim arrItem As String()
            Dim strItem As String
            arrItem = Request.QueryString("items").ToString().Split("::")
            Dim recordCount As Integer

            For Each strItem In arrItem
                Dim lngID As Long = -1
                Long.TryParse(strItem, lngID)
                recordCount = recordCount + appService.DelResource(lngID)
            Next

            Response.Redirect("ResourceList.aspx")
        End If
    End Sub

#End Region




End Class
