Option Explicit On

Imports System.Data

Partial Class OpportunityFileList
    Inherits System.Web.UI.Page
    Dim sysUser As System.Security.Principal.IPrincipal
    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = appService.GetOpportunityList.Tables(0)
    Dim dtTDHierarchy As DataTable = appService.GetOrganizationHierarchy.Tables(0)
    Dim dtFunnelPhaseList As DataTable = appService.GetFunnelPhaseList.Tables(0)
    Dim dtStatusList As DataTable = appService.GetOpportunityStatusList.Tables(0)

    Dim dtTDResourcePosition As DataTable = Nothing
    Dim dtTDInHierarchy As DataTable = Nothing

    Dim waSalesFunnel As New SalesFunnelApp

    Dim strAll As String = "- All -"
    Dim liAll As New ListItem(strAll, strAll)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        sysUser = System.Web.HttpContext.Current.User
        Dim strFullUserName As String = sysUser.Identity.Name
        Dim strComponents() As String = strFullUserName.Split("\")
        Dim strDomain As String = strComponents(0)
        Dim strUserName As String = strComponents(1)
        If IsNothing(Session("strUserName")) Or IsNothing(Session("strTRUEUserName")) Then
            Session("strUserName") = "BB125453" 'strUserName.ToUpper
            Session("strTRUEUserName") = "BB125453" 'strUserName.ToUpper
        End If

        If Not Page.IsPostBack Then
            Me.gvMain.AllowSorting = True
            Session("SortOrder") = "customer_name ASC"
            Session("PageIndex") = "0"

            Session("OppFilter") = ""

            Session("Filter") = ""
            Session("strMode") = "Main"
            Session("strModeLast") = "Main"

            Session("intOpportunity") = 0
            Session("intOpportunityState") = 0
            Session("intOpportunityStateDetail") = 0

            Session("InHierarchy") = 0

            If Not IsNothing(Session("strUserName")) And IsNothing(dtTDResourcePosition) Then
                If IsNothing(waSalesFunnel) And Not IsNothing(Session("WebApp")) Then

                    waSalesFunnel = Session("WebApp")
                ElseIf IsNothing(waSalesFunnel) And IsNothing(Session("WebApp")) Then
                    waSalesFunnel.UserName = Session("strUserName")
                    waSalesFunnel.PageMode = Session("strMode") = "Main"
                End If



                dtTDResourcePosition = appService.GetResourcePosition(Session("strUserName")).Tables(0)
                If dtTDResourcePosition.Rows.Count > 0 Then
                    Session("OppFilter") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
                    Session("organization_struct_id") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
                    Session("organization_id") = dtTDResourcePosition.Rows(0).Item("organization_id")
                    Session("organization_hrrchy_id") = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")
                    Session("organization_role_type_name") = dtTDResourcePosition.Rows(0).Item("organization_role_type_name")
                    Session("organization_role_type_id") = dtTDResourcePosition.Rows(0).Item("organization_role_type_id")
                    Session("InHierarchy") = 1

                    Session("selected_hrrchy_id") = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")

                    DoSomething()

                    FilterData()

                    BindData()

                Else
                    Session("strMode") = "Unauthorized"
                    Session("strModeLast") = "Unauthorized"
                End If
            End If

            ModeMain()
        End If
    End Sub
    Private Sub DoSomething(Optional lngParent As Long = 0, Optional tnNode As TreeNode = Nothing)

        If dtTDHierarchy.Rows.Count > 0 Then
            Dim drRow As DataRow
            For Each drRow In dtTDHierarchy.Rows
                If drRow("organization_struct_parent_id") = lngParent Then
                    Dim tnNewNode As TreeNode
                    tnNewNode = New TreeNode(drRow("organization_struct_name").ToString, drRow("organization_struct_id").ToString())
                    tnNewNode.SelectAction = TreeNodeSelectAction.Select
                    tnNewNode.PopulateOnDemand = False
                    tnNewNode.CollapseAll()
                    If IsNothing(tnNode) Then
                        Me.tvMain.Nodes.Add(tnNewNode)
                    Else
                        tnNode.ChildNodes.Add(tnNewNode)
                    End If
                    DoSomething(drRow("organization_struct_id"), tnNewNode)
                End If
            Next
        End If
    End Sub
    Private Sub FilterData()
        Dim strFilter As String = ""
        Dim intFilterCount As Integer = 0
        Dim intRowCount As Integer = 0

        If Not IsNothing(Session("OppFilter")) Then
            If Session("OppFilter") < 1 Then
                strFilter += " customer_id = " & (Session("OppFilter") * -1)
                intFilterCount += 1
            Else
                For intRowCount = 0 To dtGridView.Rows.Count - 1
                    If dtGridView.Rows(intRowCount).Item("L0ID") = Session("OppFilter") Then
                        strFilter += " L0ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                    If dtGridView.Rows(intRowCount).Item("L1ID") = Session("OppFilter") Then
                        strFilter += " L1ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                    If dtGridView.Rows(intRowCount).Item("L2ID") = Session("OppFilter") Then
                        strFilter += " L2ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                    If dtGridView.Rows(intRowCount).Item("L3ID") = Session("OppFilter") Then
                        strFilter += " L3ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                    If dtGridView.Rows(intRowCount).Item("L4ID") = Session("OppFilter") Then
                        strFilter += " L4ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                    If dtGridView.Rows(intRowCount).Item("L5ID") = Session("OppFilter") Then
                        strFilter += " L5ID= " & Session("OppFilter")
                        intFilterCount += 1
                        Exit For
                    End If
                Next
            End If
        End If

        If Me.ddlFunnelPhase.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "funnel_phase_short_name= '" & Me.ddlFunnelPhase.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        If Me.ddlStatus.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "opportunity_status_short_name= '" & Me.ddlStatus.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        Session("Filter") = strFilter

        BindData()
    End Sub
    Private Sub BindData()
        Dim dvSort As New DataView(dtGridView)
        Dim strfilter As String = ""

        If Not IsNothing(Session("SortOrder")) Then
            dvSort.Sort = Session("SortOrder")
        End If

        If Not IsNothing(Session("Filter")) Then
            strfilter = Session("Filter")

        End If

        dvSort.RowFilter = strfilter

        Me.gvMain.DataSource = dvSort
        Me.gvMain.PageIndex = CInt(Session("PageIndex"))
        Me.gvMain.DataBind()
    End Sub
    Protected Sub InHierarchy(intOrganizationStructID As Long, strUserName As String)
        dtTDInHierarchy = appService.InOrganizationHierarchy(intOrganizationStructID, strUserName).Tables(0)

        If dtTDInHierarchy.Rows.Count > 0 Then
            Session("InHierarchy") = 1
        Else
            Session("InHierarchy") = 0
        End If

        Session("selected_hrrchy_id") = appService.GetHierarchy(intOrganizationStructID)
    End Sub
    Private Sub ModeMain()
        Dim dtFoundRows() As DataRow
        dtGridView = appService.GetOpportunityList.Tables(0)

        dtFoundRows = dtTDHierarchy.Select("organization_struct_id = " & Session("OppFilter"))
        Me.txtHierarchy.Text = dtFoundRows(0).Item("organization_full_name").ToString()

        Dim O As Integer = Session("OppFilter")
        Dim S As String = Session("strUserName")

        InHierarchy(Session("OppFilter"), Session("strUserName"))


        If Session("InHierarchy") = 1 And Session("organization_role_type_id") = 2 And Session("selected_hrrchy_id") > 2 Then
            Me.btnAdd.Visible = True
            Me.btnAdd.Enabled = True
        Else
            Me.btnAdd.Visible = False
            Me.btnAdd.Enabled = False
        End If

        Me.ddlFunnelPhase.DataSource = dtFunnelPhaseList
        Me.ddlFunnelPhase.DataValueField = "funnel_phase_short_name"
        Me.ddlFunnelPhase.DataTextField = "funnel_phase_short_name"
        Me.ddlFunnelPhase.DataBind()
        Me.ddlFunnelPhase.Items.Insert(0, liAll)

        Me.ddlStatus.DataSource = dtStatusList
        Me.ddlStatus.DataValueField = "opportunity_status_short_name"
        Me.ddlStatus.DataTextField = "opportunity_status_short_name"
        Me.ddlStatus.DataBind()
        Me.ddlStatus.Items.Insert(0, liAll)

        Me.ddlFunnelPhase.SelectedValue = Me.ddlFunnelPhase.Items.FindByText(strAll).Value
        Me.ddlStatus.SelectedValue = Me.ddlStatus.Items.FindByText(strAll).Value

        FilterData()
        BindData()

        Me.pnlFile.Visible = False
        Me.pnlMain.Visible = True

    End Sub
    Protected Sub gvMain_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMain.Sorting
        If Session("SortOrder") <> Convert.ToString(e.SortExpression) & " " & ConvertSortDirectionToSql(e.SortDirection) Then
            Session("SortOrder") = Convert.ToString(e.SortExpression) & " " & ConvertSortDirectionToSql(e.SortDirection)
            Session("PageIndex") = "0"
            BindData()
        End If
    End Sub
    Private Function ConvertSortDirectionToSql(sortDirection__1 As SortDirection) As String
        Dim newSortDirection As String = [String].Empty
        Select Case sortDirection__1
            Case SortDirection.Ascending
                newSortDirection = "ASC"
                Exit Select
            Case SortDirection.Descending
                newSortDirection = "DESC"
                Exit Select
        End Select
        Return newSortDirection
    End Function
    Protected Sub btnHierarchy_Click(sender As Object, e As EventArgs) Handles btnHierarchy.Click
        Session("strModeLast") = Session("strMode")
        Session("strMode") = "File"
        ModeFile()
    End Sub
    Private Sub ModeFile()

        Session("Filter") = ""
        Me.pnlFile.Visible = True
        Me.pnlMain.Visible = False

    End Sub
    Protected Sub tvMain_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tvMain.SelectedNodeChanged
        Session("strMode") = "Main"
        Session("OppFilter") = tvMain.SelectedNode.Value

        InHierarchy(Session("OppFilter"), Session("strUserName"))


        'If Not IsNothing(Session("strUserName")) And IsNothing(dtTDResourcePosition) Then
        '    dtTDResourcePosition = appService.GetResourcePosition(Session("strUserName")).Tables(0)
        '    If dtTDResourcePosition.Rows.Count > 0 Then
        '        Session("OppFilter") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
        '        Session("organization_struct_id") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
        '        Session("organization_id") = dtTDResourcePosition.Rows(0).Item("organization_id")
        '        Session("organization_hrrchy_id") = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")
        '        Session("organization_role_type_name") = dtTDResourcePosition.Rows(0).Item("organization_role_type_name")
        '        Session("organization_role_type_id") = dtTDResourcePosition.Rows(0).Item("organization_role_type_id")
        '        Session("InHierarchy") = 1

        '        DoSomething()

        '        FilterData()

        '        BindData()

        '    Else
        '        Session("strMode") = "Unauthorized"
        '        Session("strModeLast") = "Unauthorized"
        '    End If
        'End If



        ModeMain()

    End Sub
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        FilterData()

    End Sub
    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        Me.ddlFunnelPhase.SelectedValue = Me.ddlFunnelPhase.Items.FindByText(strAll).Value
        Me.ddlStatus.SelectedValue = Me.ddlStatus.Items.FindByText(strAll).Value

        Session("Filter") = ""

        FilterData()
        BindData()

    End Sub
    Protected Sub gvMain_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "View" Then
            Session("strModeLast") = Session("strMode")
            Session("strMode") = "View"
            Session("intOpportunity") = e.CommandArgument
            Session("dtTDHierarchy") = dtTDHierarchy
            Response.Redirect("~/OpportunityFileView.aspx")
            'PageMode()
        Else
            'Do Nothing
        End If
    End Sub
End Class
