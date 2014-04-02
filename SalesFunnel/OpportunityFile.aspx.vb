Option Explicit On

Imports System.Data


Partial Class OpportunityFile
    Inherits System.Web.UI.Page
    Dim sysUser As System.Security.Principal.IPrincipal

    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = appService.GetOpportunityList.Tables(0)

    Dim dtTDResourcePosition As DataTable = Nothing
    Dim dtTDInHierarchy As DataTable = Nothing

    Dim dtTDHierarchy As DataTable = appService.GetOrganizationHierarchy.Tables(0)

    Dim dtStatusList As DataTable = appService.GetOpportunityStatusList.Tables(0)
    Dim dtFunnelPhaseList As DataTable = appService.GetFunnelPhaseList.Tables(0)
    Dim dtCustomerList As DataTable = appService.GetCustomerList.Tables(0)
    Dim dtAccountTeamList As DataTable = appService.GetAccountTeamList.Tables(0)

    Dim dtOutlookList As DataTable = appService.GetDetailValueList(11).Tables(0)
    Dim dtMPOverrideList As DataTable = appService.GetDetailValueList(12).Tables(0)
    Dim dtOrderFlowList As DataTable = appService.GetDetailValueList(13).Tables(0)

    Dim waSalesFunnel As New SalesFunnelApp

    Dim strAll As String = "- All -"
    Dim strChoose As String = "- Choose One - "
    Dim liAll As New ListItem(strAll, strAll)
    Dim liChoose As New ListItem(strChoose, -1)

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

            PageMode()

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



        PageMode()

    End Sub

    Protected Sub InHierarchy(intOrganizationStructID As Long, strUserName As String)

        dtTDInHierarchy = appService.InOrganizationHierarchy(intOrganizationStructID, strUserName).Tables(0)

        If dtTDInHierarchy.Rows.Count > 0 Then
            Session("InHierarchy") = 1
        Else
            Session("InHierarchy") = 0
        End If

        Session("selected_hrrchy_id") = appService.GetHierarchy(intOrganizationStructID)

        'dtTDInHierarchy = appService.InHierarchy(Session("organization_struct_id"), Session("OppFilter")).Tables(0)

        'If dtTDInHierarchy.Rows.Count > 0 Then
        '    Session("InHierarchy") = 1
        'Else
        '    Session("InHierarchy") = 0
        'End If
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        Me.ddlFunnelPhase.SelectedValue = Me.ddlFunnelPhase.Items.FindByText(strAll).Value
        Me.ddlStatus.SelectedValue = Me.ddlStatus.Items.FindByText(strAll).Value

        Session("Filter") = ""

        FilterData()
        BindData()

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        FilterData()

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

    Protected Sub gvMain_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMain.PageIndexChanging
        Session("PageIndex") = e.NewPageIndex.ToString
        BindData()
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

    Protected Sub gvMain_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "View" Then
            Session("strModeLast") = Session("strMode")
            Session("strMode") = "View"
            Session("intOpportunity") = e.CommandArgument
            PageMode()
        Else
            'Do Nothing
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Session("strModeLast") = Session("strMode")
        Session("strMode") = "Add"
        PageMode()
    End Sub

    Protected Sub btnAddCustomerProspect_Click(sender As Object, e As EventArgs) Handles btnAddCustomerProspect.Click
        Dim intresult As Integer = 1
        If Me.txtACCustomerName.Text.Length > 0 Then
            intresult = appService.AddCustomerProspect(Me.txtACCustomerName.Text, Me.ddlCustomerType.SelectedValue, Session("OppFilter"))

            dtCustomerList = appService.GetCustomerList.Tables(0)
            dtTDHierarchy = appService.GetOrganizationHierarchy.Tables(0)

            Session("strMode") = Session("strModeLast")
            PageMode()
        Else
            Me.lblTitle.Text = "Name Error"
            Me.txtMessage.Text = "Please enter a Customer or Prospect name."
            Me.lblOpportunityComment.Visible = False
            Me.txtOpportunityCommentEdit.Visible = False
            Me.pnlAddCustomer.Visible = False
            'Me.pnlDetail.Visible = False
            Me.pnlMsg.Visible = True
            Me.btnOpportunityOK.Enabled = False
            Me.btnOpportunityCancel.Enabled = False
        End If


    End Sub


    '*******************************************************************************
    '   Detail Stuff
    '
    '
    '*******************************************************************************

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        Session("strModeLast") = "Main"
        Session("strMode") = "Main"

        Session("intOpportunity") = 0
        Session("intOpportunityStateDetail") = 0

        PageMode()

    End Sub

    Protected Sub gvDetail_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetail.RowCommand
        If e.CommandName = "EditDetail" Then
            Session("strModeLast") = Session("strMode")
            Session("strMode") = "EditDetail"
            Session("intOpportunityStateDetail") = e.CommandArgument
            PageMode()
        Else
            'Do Nothing
        End If
    End Sub


    Protected Sub btnOpportunityDetailCancel_Click(sender As Object, e As EventArgs) Handles btnOpportunityDetailCancel.Click
        Session("strMode") = Session("strModeLast")
        PageMode()
    End Sub

    Protected Sub btnOpportunityDetailOK_Click(sender As Object, e As EventArgs) Handles btnOpportunityDetailOK.Click
        Dim intResult As Integer = -1
        Dim blnDelta As Boolean = True
        Dim rowFound As DataRow

        For Each rowFound In Session("dtOpportunityDetail").Rows
            If rowFound.Item("opportunity_state_detail_id") = Session("intOpportunityStateDetail") Then
                If rowFound.Item("detail_value_id") = Me.ddlOpportunityDetailTypeEdit.SelectedValue Then
                    blnDelta = False
                End If
                Exit For
            End If
        Next
        If blnDelta Then
            intResult = appService.SetOpportunityStateDetail(Session("intOpportunityStateDetail"), Session("intOpportunity"), Me.ddlOpportunityDetailTypeEdit.SelectedValue, Session("strUserName").ToString)
        End If

        Session("strModeLast") = Session("strMode")
        Session("strMode") = "Edit"
        PageMode()

    End Sub

    Protected Sub btnOpportunityCancel_Click(sender As Object, e As EventArgs) Handles btnOpportunityCancel.Click
        Select Case Session("strMode")

            Case "Add"
                Session("strModeLast") = "Main"
                Session("strMode") = "Main"
                PageMode()

            Case "Edit"
                Session("strModeLast") = "Main"
                Session("strMode") = "View"
                PageMode()

        End Select

    End Sub

    Protected Sub btnOpportunityOK_Click(sender As Object, e As EventArgs) Handles btnOpportunityOK.Click
        Dim intResult As Integer = -1
        Dim intOpportunityResult As Integer = -1
        Dim intOpportunityStateResult As Integer = -1
        Dim intOpportunityStateDetailResult As Integer = -1


        Select Case Session("strMode")

            Case "Add"
                If ValidateAdd() Then
                    intOpportunityResult = appService.AddOpportunityTemporal(Me.ddlCustomerEdit.SelectedValue, Me.txtOpportunityEdit.Text, Session("strUserName"))

                    If intOpportunityResult > 0 Then
                        Session("intOpportunity") = intOpportunityResult

                        'intOpportunityStateResult = appService.AddOpportunityDetailTemporal(intOpportunityResult,
                        '                                                                    Me.calCloseDateEdit.SelectedDate,
                        '                                                                    Me.txtPSValueEdit.Text,
                        '                                                                    Me.ddlFunnelPhaseEdit.SelectedValue,
                        '                                                                    Me.txtWinPercentEdit.Text,
                        '                                                                    Me.ddlOpportunityStatusEdit.Text,
                        '                                                                    Me.txtOpportunityCommentEdit.Text,
                        '                                                                    Me.txtRevenueUpsideEdit.Text,
                        '                                                                    Session("strUserName"))

                        intOpportunityStateResult = appService.SetOpportunityStateTemporalTable(intOpportunityResult,
                                                                                                Me.txtPSValueEdit.Text,
                                                                                                Me.calCloseDateEdit.SelectedDate.ToString("yyyy-MM-dd"),
                                                                                                Me.ddlFunnelPhaseEdit.SelectedValue,
                                                                                                Me.txtWinPercentEdit.Text,
                                                                                                Me.txtRevenueUpsideEdit.Text,
                                                                                                Me.ddlOpportunityStatusEdit.Text,
                                                                                                -1,
                                                                                                Me.txtOpportunityCommentEdit.Text,
                                                                                                Session("strUserName"))

                        If intOpportunityStateResult > 0 Then
                            Session("intOpportunityState") = intOpportunityStateResult
                            'intOpportunityStateDetailResult = appService.AddOpportunityStateDetailTemporal(Session("intOpportunityState"))

                            If Me.ddlOutlookStatusEdit.SelectedIndex <> 1 Then
                                intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(11, Session("intOpportunity"), Me.ddlOutlookStatusEdit.SelectedValue, Session("strUserName"))
                            End If
                            If Me.ddlMPOverrideEdit.SelectedIndex <> 1 Then
                                intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(12, Session("intOpportunity"), Me.ddlMPOverrideEdit.SelectedValue, Session("strUserName"))
                            End If
                            If Me.ddlOrderFlowEdit.SelectedIndex <> 1 Then
                                intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(13, Session("intOpportunity"), Me.ddlOrderFlowEdit.SelectedValue, Session("strUserName"))
                            End If

                            If intOpportunityStateDetailResult > 0 Then
                                Session("strModeLast") = Session("strMode")
                                Session("strMode") = "View"
                                PageMode()
                            Else
                                Session("strModeLast") = Session("strMode")
                                Session("strMode") = "Main"
                                PageMode()
                            End If

                        End If
                    End If

                    'intOpportunityResult = appService.AddOpportunity(Me.ddlCustomerEdit.SelectedValue, Me.txtOpportunityEdit.Text)
                    'If intOpportunityResult > 0 Then
                    '    Session("intOpportunity") = intOpportunityResult

                    '    intOpportunityStateResult = appService.AddOpportunityState(intOpportunityResult, Me.calCloseDateEdit.SelectedDate, Me.txtPSValueEdit.Text, Me.ddlFunnelPhaseEdit.SelectedValue, Me.txtWinPercentEdit.Text, Me.ddlOpportunityStatusEdit.Text, Me.txtOpportunityCommentEdit.Text, Me.txtRevenueUpsideEdit.Text, Session("strUserName").ToString)

                    '    If intOpportunityStateResult > 0 Then
                    '        Session("intOpportunityState") = intOpportunityStateResult

                    '        intOpportunityStateDetailResult = appService.AddOpportunityStateDetail(intOpportunityStateResult)

                    '        If Me.ddlOutlookStatusEdit.SelectedIndex <> 1 Then
                    '            intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(11, Session("intOpportunity"), Me.ddlOutlookStatusEdit.SelectedValue, Session("strUserName"))
                    '        End If
                    '        If Me.ddlMPOverrideEdit.SelectedIndex <> 1 Then
                    '            intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(12, Session("intOpportunity"), Me.ddlMPOverrideEdit.SelectedValue, Session("strUserName"))
                    '        End If
                    '        If Me.ddlOrderFlowEdit.SelectedIndex <> 1 Then
                    '            intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(13, Session("intOpportunity"), Me.ddlOrderFlowEdit.SelectedValue, Session("strUserName"))
                    '        End If

                    '        If intOpportunityStateDetailResult > 0 Then
                    '            Session("strModeLast") = Session("strMode")
                    '            Session("strMode") = "View"
                    '            PageMode()
                    '        End If
                    '    End If
                    'End If
                End If
            Case "Edit"
                    If ValidateAdd() Then
                        'Dim intOpportunityDelta As Integer = 0
                        'Dim intOpportunityStateDelta As Integer = 0
                        Dim dtOpportunity As DataTable = Session("dtOpportunity")
                        Dim bUpdateTable As Boolean = False

                        If Me.txtOpportunityEdit.Text <> dtOpportunity.Rows.Item(0).Item("opportunity_desc") Then
                            bUpdateTable = True
                        End If

                        If Me.calCloseDateEdit.SelectedDate <> dtOpportunity.Rows.Item(0).Item("close_date") Then
                            bUpdateTable = True
                        End If

                        If Me.txtPSValueEdit.Text <> dtOpportunity.Rows.Item(0).Item("ps_value_nbr") Then
                            bUpdateTable = True
                    End If

                        If Me.ddlFunnelPhaseEdit.SelectedValue <> dtOpportunity.Rows.Item(0).Item("funnel_phase_id") Then
                            bUpdateTable = True
                        End If

                        If Me.txtWinPercentEdit.Text <> dtOpportunity.Rows.Item(0).Item("win_percent") Then
                            bUpdateTable = True
                        End If

                        If Me.ddlOpportunityStatusEdit.SelectedValue <> dtOpportunity.Rows.Item(0).Item("opportunity_status_id") Then
                            bUpdateTable = True
                        End If

                        If Me.txtOpportunityCommentEdit.Text <> dtOpportunity.Rows.Item(0).Item("opportunity_comment_desc") Then
                            bUpdateTable = True
                        End If

                        If Me.txtRevenueUpsideEdit.Text <> dtOpportunity.Rows.Item(0).Item("revenue_upside") Then
                            bUpdateTable = True
                        End If

                        If bUpdateTable Then
                        intResult = appService.SetOpportunityStateTemporalTable(Session("intOpportunity"),
                                                                                Me.txtPSValueEdit.Text,
                                                                                Me.calCloseDateEdit.SelectedDate.ToString("yyyy-MM-dd"),
                                                                                Me.ddlFunnelPhaseEdit.SelectedValue,
                                                                                Me.txtWinPercentEdit.Text,
                                                                                Me.txtRevenueUpsideEdit.Text,
                                                                                Me.ddlOpportunityStatusEdit.SelectedValue,
                                                                                Session("intOpportunityState"),
                                                                                Me.txtOpportunityCommentEdit.Text,
                                                                                Session("strUserName"))

                        intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(11, Session("intOpportunity"), Me.ddlOutlookStatusEdit.SelectedValue, Session("strUserName"))

                        intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(12, Session("intOpportunity"), Me.ddlMPOverrideEdit.SelectedValue, Session("strUserName"))

                        intOpportunityStateDetailResult = appService.SetOpportunityStateDetail2(13, Session("intOpportunity"), Me.ddlOrderFlowEdit.SelectedValue, Session("strUserName"))

                    End If

                        Session("strModeLast") = Session("strMode")
                        Session("strMode") = "View"
                        PageMode()

                    End If
        End Select
    End Sub

    Protected Sub btnOpportunityEdit_Click(sender As Object, e As EventArgs) Handles btnOpportunityEdit.Click

        Session("strModeLast") = Session("strMode")
        Session("strMode") = "Edit"
        PageMode()

    End Sub


    Private Function ValidateAdd() As Boolean
        Dim blnValidate As Boolean = False

        Dim strInvalid As String = ""
        If Me.ddlCustomerEdit.SelectedValue = -1 Then strInvalid += "Please select a Customer!" & vbCrLf
        If Len(Me.txtOpportunityEdit.Text) = 0 Or Me.txtOpportunityEdit.Text = vbNullString Then strInvalid += "Please enter a Description!" & vbCrLf
        If Me.ddlFunnelPhaseEdit.SelectedValue = -1 Then strInvalid += "Please select a Phase!" & vbCrLf
        Try
            Dim intTest As Integer = CInt(Me.txtPSValueEdit.Text)
        Catch ex As Exception
            strInvalid += "Please enter a numeric value for PS Value!"
        End Try
        Try
            Dim bytTest As Byte = CByte(Me.txtWinPercentEdit.Text)
            If bytTest < 0 Or bytTest > 100 Then
                strInvalid += "Please enter a value between 0 and 100 for Win Percent!"
            End If
        Catch ex As Exception
            strInvalid += "Please enter a value between 0 and 100 for Win Percent!"
        End Try
        Try
            Dim intTest As Integer = CInt(Me.txtRevenueUpsideEdit.Text)
        Catch ex As Exception
            strInvalid += "Please enter a numeric value for Revenue Upside!"
        End Try

        If Len(strInvalid) > 0 Then
            blnValidate = False
            Me.lblTitle.Text = "Validation Error"
            Me.txtMessage.Text = strInvalid

            Me.lblOpportunityComment.Visible = False
            Me.txtOpportunityCommentEdit.Visible = False
            Me.pnlDetail.Visible = False
            Me.pnlMsg.Visible = True
            Me.btnOpportunityOK.Enabled = False
            Me.btnOpportunityCancel.Enabled = False

        Else
            blnValidate = True

        End If

        Return blnValidate

    End Function

    Protected Sub btnMessageOK_Click(sender As Object, e As EventArgs) Handles btnMessageOK.Click
        Me.lblTitle.Text = "Title"
        Me.txtMessage.Text = "Message"
        Select Case Session("strMode")
            Case "Add"
                Me.lblOpportunityComment.Visible = True
                Me.txtOpportunityCommentEdit.Visible = True
                Me.pnlDetail.Visible = False
                Me.pnlMsg.Visible = False
                Me.btnOpportunityOK.Enabled = True
                Me.btnOpportunityCancel.Enabled = True
            Case "Edit"
                Me.lblOpportunityComment.Visible = True
                Me.txtOpportunityCommentEdit.Visible = True
                Me.pnlDetail.Visible = False
                Me.pnlMsg.Visible = False
                Me.btnOpportunityOK.Enabled = True
                Me.btnOpportunityCancel.Enabled = True
            Case "EditDetail"
                Me.lblOpportunityComment.Visible = False
                Me.txtOpportunityCommentEdit.Visible = False
                Me.pnlDetail.Visible = True
                Me.pnlMsg.Visible = False
            Case "AddProspect"
                Me.lblOpportunityComment.Visible = False
                Me.txtOpportunityCommentEdit.Visible = False
                Me.pnlAddCustomer.Visible = True
                Me.pnlMsg.Visible = False
        End Select
    End Sub

    Private Sub PageMode()

        Select Case Session("strMode")
            Case "File"
                ModeFile()
            Case "Main"
                ModeMain()
            Case "Add"
                ModeAdd()
            Case "View"
                ModeView()
            Case "Edit"
                ModeEdit()
            Case "EditDetail"
                ModeEditDetail()
            Case "Unauthorized"
                ModeUnauthorized()
            Case "AddProspect"
                ModeAddProspect()
        End Select

    End Sub


#Region "Modes File, Main, Add, View, Edit, EditDetail"

    Private Sub ModeFile()

        Session("Filter") = ""
        Me.pnlFile.Visible = True
        Me.pnlMain.Visible = False
        Me.pnlView.Visible = False

    End Sub

    Private Sub ModeUnauthorized()

        Session("Filter") = ""

        Me.pnlUnauthorized.Visible = True
        Me.pnlFile.Visible = False
        Me.pnlMain.Visible = False
        Me.pnlView.Visible = False

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
        Me.pnlView.Visible = False

    End Sub

    Private Sub ModeAdd()
        Dim dtFoundRows() As DataRow

        ViewListPopulate()

        dtFoundRows = dtTDHierarchy.Select("organization_struct_id = " & Session("OppFilter"))
        Me.lblDsplyHierarchyEdit.Text = dtFoundRows(0).Item("organization_full_name").ToString()

        Me.lblAction.Text = "Add Opportunity"
        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = True
        Me.btnOpportunityOK.Visible = True
        Me.btnOpportunityCancel.Enabled = True
        Me.btnOpportunityCancel.Visible = True
        Me.btnCancelAddCustomer.Visible = False
        Me.btnAddProspect.Visible = True

        Me.pnlView.BackColor = Drawing.Color.White

        Me.pnlDetail.Visible = False

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = True
        Me.ddlCustomerEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityEdit.Enabled = True
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.calCloseDateEdit.Enabled = True
        Me.calCloseDateEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityCommentEdit.Enabled = True
        Me.txtOpportunityCommentEdit.BackColor = Drawing.Color.White
        Me.txtPSValueEdit.Enabled = True
        Me.txtPSValueEdit.BackColor = Drawing.Color.White
        Me.ddlFunnelPhaseEdit.Enabled = True
        Me.ddlFunnelPhaseEdit.BackColor = Drawing.Color.White
        Me.txtWinPercentEdit.Enabled = True
        Me.txtWinPercentEdit.BackColor = Drawing.Color.White
        Me.ddlOpportunityStatusEdit.Enabled = True
        Me.ddlOpportunityStatusEdit.BackColor = Drawing.Color.White
        Me.txtRevenueUpsideEdit.Enabled = True
        Me.txtRevenueUpsideEdit.BackColor = Drawing.Color.White
        Me.lblDsplyHierarchyEdit.BackColor = Drawing.Color.White

        Me.ddlOutlookStatusEdit.Enabled = True
        Me.ddlOutlookStatusEdit.BackColor = Drawing.Color.White
        If Session("organization_hrrchy_id") <= 2 And Session("organization_role_type_id") = 2 Then
            Me.ddlMPOverrideEdit.Enabled = True
            Me.ddlMPOverrideEdit.BackColor = Drawing.Color.White
        Else
            Me.ddlMPOverrideEdit.Enabled = False
            Me.ddlMPOverrideEdit.BackColor = Drawing.Color.LightGray
        End If
        Me.ddlOrderFlowEdit.Enabled = True
        Me.ddlOrderFlowEdit.BackColor = Drawing.Color.White

        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.LightGray

        Me.btnPSValueDown.Enabled = True
        Me.btnPSValueUp.Enabled = True
        Me.btnWinPercentDown.Enabled = True
        Me.btnWinPercentUp.Enabled = True
        Me.btnRevenueUpsideDown.Enabled = True
        Me.btnRevenueUpsideUP.Enabled = True

        Me.ddlCustomerEdit.SelectedValue = Me.ddlCustomerEdit.Items.FindByValue(-1).Value
        Me.txtOpportunityEdit.Text = ""
        Me.calCloseDateEdit.SelectedDate = Now.Date
        Me.calCloseDateEdit.VisibleDate = Now.Date
        Me.txtOpportunityCommentEdit.Text = ""
        Me.txtPSValueEdit.Text = 0
        Me.ddlFunnelPhaseEdit.SelectedValue = Me.ddlFunnelPhaseEdit.Items.FindByValue(-1).Value
        Me.txtWinPercentEdit.Text = 0
        Me.ddlOpportunityStatusEdit.SelectedValue = Me.ddlOpportunityStatusEdit.Items.FindByValue(1).Value
        Me.txtRevenueUpsideEdit.Text = 0

        Me.ddlOutlookStatusEdit.SelectedValue = dtOutlookList.Rows(0).Item("detail_value_id")
        Me.ddlMPOverrideEdit.SelectedValue = dtMPOverrideList.Rows(0).Item("detail_value_id")
        Me.ddlOrderFlowEdit.SelectedValue = dtOrderFlowList.Rows(0).Item("detail_value_id")

        Dim dtOpportunityDetail As DataTable = appService.GetOpportunityDetailBlank().Tables(0)
        Session("dtOpportunityDetail") = dtOpportunityDetail

        Dim dvDetail As New DataView(dtOpportunityDetail)

        Me.gvDetail.DataSource = dvDetail
        Me.gvDetail.DataBind()

        Me.pnlAddCustomer.Visible = False
        Me.pnlFile.Visible = False
        Me.pnlMain.Visible = False
        Me.pnlView.Visible = True
    End Sub

    Private Sub ModeView()
        Dim dtFoundRows() As DataRow

        ViewListPopulate()

        Dim dtOpportunity As DataTable = appService.GetOpportunity(Session("intOpportunity")).Tables(0)
        Dim dtOpportunityDetails As DataTable = appService.GetOpportunityDetails(Session("intOpportunity")).Tables(0)
        Dim dtOpportunityDetail As DataTable = appService.GetOpportunityDetail(dtOpportunity.Rows(0).Item("opportunity_state_id")).Tables(0)

        Session("intOpportunityState") = dtOpportunity.Rows.Item(0).Item("opportunity_state_id")
        Session("dtOpportunity") = dtOpportunity
        Session("dtOpportunityDetail") = dtOpportunityDetail

        dtFoundRows = dtTDHierarchy.Select("organization_struct_id = " & dtOpportunity.Rows(0).Item("customer_id") * -1)
        Me.lblDsplyHierarchyEdit.Text = dtFoundRows(0).Item("organization_full_name").ToString()

        InHierarchy(dtOpportunity.Rows(0).Item("customer_id") * -1, Session("strUserName"))

        Me.lblAction.Text = "View Opportunity"
        Me.btnBack.Enabled = True
        Me.btnBack.Visible = True

        If Session("InHierarchy") = 1 And Session("organization_role_type_id") = 2 Then
            Me.btnOpportunityEdit.Enabled = True
            Me.btnOpportunityEdit.Visible = True
        Else
            Me.btnOpportunityEdit.Enabled = False
            Me.btnOpportunityEdit.Visible = False
        End If
        Me.btnOpportunityOK.Enabled = False
        Me.btnOpportunityOK.Visible = False
        Me.btnOpportunityCancel.Enabled = False
        Me.btnOpportunityCancel.Visible = False
        Me.btnCancelAddCustomer.Visible = False
        Me.btnAddProspect.Visible = False


        Me.pnlView.BackColor = Drawing.Color.White

        Me.pnlDetail.Visible = False

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityEdit.Enabled = False
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.calCloseDateEdit.Enabled = False
        Me.calCloseDateEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityCommentEdit.Enabled = False
        Me.txtOpportunityCommentEdit.BackColor = Drawing.Color.White
        Me.txtPSValueEdit.Enabled = False
        Me.txtPSValueEdit.BackColor = Drawing.Color.White
        Me.ddlFunnelPhaseEdit.Enabled = False
        Me.ddlFunnelPhaseEdit.BackColor = Drawing.Color.White
        Me.txtWinPercentEdit.Enabled = False
        Me.txtWinPercentEdit.BackColor = Drawing.Color.White
        Me.ddlOpportunityStatusEdit.Enabled = False
        Me.ddlOpportunityStatusEdit.BackColor = Drawing.Color.White
        Me.txtRevenueUpsideEdit.Enabled = False
        Me.txtRevenueUpsideEdit.BackColor = Drawing.Color.White
        Me.lblDsplyHierarchyEdit.BackColor = Drawing.Color.White

        Me.ddlOutlookStatusEdit.Enabled = False
        Me.ddlOutlookStatusEdit.BackColor = Drawing.Color.White
        Me.ddlMPOverrideEdit.Enabled = False
        Me.ddlMPOverrideEdit.BackColor = Drawing.Color.White
        Me.ddlOrderFlowEdit.Enabled = False
        Me.ddlOrderFlowEdit.BackColor = Drawing.Color.White

        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.White

        Me.btnPSValueDown.Enabled = False
        Me.btnPSValueUp.Enabled = False
        Me.btnWinPercentDown.Enabled = False
        Me.btnWinPercentUp.Enabled = False
        Me.btnRevenueUpsideDown.Enabled = False
        Me.btnRevenueUpsideUP.Enabled = False




        Me.ddlCustomerEdit.SelectedValue = dtOpportunity.Rows(0).Item("customer_id")
        Me.txtOpportunityEdit.Text = dtOpportunity.Rows(0).Item("opportunity_desc")
        Me.calCloseDateEdit.SelectedDate = dtOpportunity.Rows(0).Item("close_date")
        Me.calCloseDateEdit.VisibleDate = dtOpportunity.Rows(0).Item("close_date")
        Me.txtOpportunityCommentEdit.Text = dtOpportunity.Rows(0).Item("opportunity_comment_desc")
        Me.txtPSValueEdit.Text = dtOpportunity.Rows(0).Item("ps_value_nbr")
        Me.ddlFunnelPhaseEdit.SelectedValue = dtOpportunity.Rows(0).Item("funnel_phase_id")
        Me.txtWinPercentEdit.Text = dtOpportunity.Rows(0).Item("win_percent")
        Me.ddlOpportunityStatusEdit.SelectedValue = dtOpportunity.Rows(0).Item("opportunity_status_id")

        If IsDBNull(dtOpportunity.Rows(0).Item("revenue_upside")) Then
            Me.txtRevenueUpsideEdit.Text = 0
        Else
            Me.txtRevenueUpsideEdit.Text = dtOpportunity.Rows(0).Item("revenue_upside")
        End If


        Me.ddlOutlookStatusEdit.SelectedValue = GetRow(dtOpportunityDetails, "detail_type_id", 11).Item("detail_value_id")
        Me.ddlMPOverrideEdit.SelectedValue = GetRow(dtOpportunityDetails, "detail_type_id", 12).Item("detail_value_id")
        Me.ddlOrderFlowEdit.SelectedValue = GetRow(dtOpportunityDetails, "detail_type_id", 13).Item("detail_value_id")

        Dim dvDetail As New DataView(dtOpportunityDetail)

        Me.gvDetail.DataSource = dvDetail
        Me.gvDetail.DataBind()

        Me.pnlAddCustomer.Visible = False
        Me.pnlFile.Visible = False
        Me.pnlMain.Visible = False
        Me.pnlView.Visible = True
    End Sub


    Private Sub ModeEdit()
        'Dim dtFoundRows() As DataRow

        'dtFoundRows = dtTDHierarchy.Select("organization_struct_id = " & Session("OppFilter"))
        'Me.lblDsplyHierarchyEdit.Text = dtFoundRows(0).Item("organization_full_name").ToString()

        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = True
        Me.btnOpportunityOK.Visible = True
        Me.btnOpportunityCancel.Enabled = True
        Me.btnOpportunityCancel.Visible = True
        Me.btnCancelAddCustomer.Visible = False

        Me.pnlView.BackColor = Drawing.Color.White

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityEdit.Enabled = True
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.calCloseDateEdit.Enabled = True
        Me.calCloseDateEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityCommentEdit.Enabled = True
        Me.txtOpportunityCommentEdit.BackColor = Drawing.Color.White
        Me.txtPSValueEdit.Enabled = True
        Me.txtPSValueEdit.BackColor = Drawing.Color.White
        Me.ddlFunnelPhaseEdit.Enabled = True
        Me.ddlFunnelPhaseEdit.BackColor = Drawing.Color.White
        Me.txtWinPercentEdit.Enabled = True
        Me.txtWinPercentEdit.BackColor = Drawing.Color.White
        Me.ddlOpportunityStatusEdit.Enabled = True
        Me.ddlOpportunityStatusEdit.BackColor = Drawing.Color.White
        Me.txtRevenueUpsideEdit.Enabled = True
        Me.txtRevenueUpsideEdit.BackColor = Drawing.Color.White
        Me.lblDsplyHierarchyEdit.BackColor = Drawing.Color.White

        Me.ddlOutlookStatusEdit.Enabled = True
        Me.ddlOutlookStatusEdit.BackColor = Drawing.Color.White
        If Session("organization_hrrchy_id") <= 2 And Session("organization_role_type_id") = 2 Then
            Me.ddlMPOverrideEdit.Enabled = True
            Me.ddlMPOverrideEdit.BackColor = Drawing.Color.White
        Else
            Me.ddlMPOverrideEdit.Enabled = False
            Me.ddlMPOverrideEdit.BackColor = Drawing.Color.LightGray
        End If
        Me.ddlOrderFlowEdit.Enabled = True
        Me.ddlOrderFlowEdit.BackColor = Drawing.Color.White


        Me.gvDetail.Enabled = True
        Me.gvDetail.BackColor = Drawing.Color.White

        Me.btnPSValueDown.Enabled = True
        Me.btnPSValueUp.Enabled = True
        Me.btnWinPercentDown.Enabled = True
        Me.btnWinPercentUp.Enabled = True
        Me.btnRevenueUpsideDown.Enabled = True
        Me.btnRevenueUpsideUP.Enabled = True

        Dim dtOpportunity As DataTable = appService.GetOpportunity(Session("intOpportunity")).Tables(0)
        Dim dtOpportunityDetail As DataTable = appService.GetOpportunityDetail(dtOpportunity.Rows(0).Item("opportunity_state_id")).Tables(0)

        Session("intOpportunityState") = dtOpportunity.Rows.Item(0).Item("opportunity_state_id")
        Session("dtOpportunity") = dtOpportunity
        Session("dtOpportunityDetail") = dtOpportunityDetail

        Dim dvDetail As New DataView(dtOpportunityDetail)

        Me.gvDetail.DataSource = dvDetail
        Me.gvDetail.DataBind()

        Me.pnlAddCustomer.Visible = False
        Me.pnlDetail.BackColor = Drawing.Color.White
        Me.pnlDetail.Visible = False

    End Sub

    Private Sub ModeEditDetail()

        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = False
        Me.btnOpportunityOK.Visible = False
        Me.btnOpportunityCancel.Enabled = False
        Me.btnOpportunityCancel.Visible = False
        Me.btnCancelAddCustomer.Visible = False

        Me.pnlView.BackColor = Drawing.Color.LightGray

        Me.lblOpportunityComment.Visible = False
        Me.txtOpportunityCommentEdit.Visible = False

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityEdit.Enabled = False
        Me.txtOpportunityEdit.BackColor = Drawing.Color.LightGray
        Me.calCloseDateEdit.Enabled = False
        Me.calCloseDateEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityCommentEdit.Enabled = False
        Me.txtOpportunityCommentEdit.BackColor = Drawing.Color.LightGray
        Me.txtPSValueEdit.Enabled = False
        Me.txtPSValueEdit.BackColor = Drawing.Color.LightGray
        Me.ddlFunnelPhaseEdit.Enabled = False
        Me.ddlFunnelPhaseEdit.BackColor = Drawing.Color.LightGray
        Me.txtWinPercentEdit.Enabled = False
        Me.txtWinPercentEdit.BackColor = Drawing.Color.LightGray
        Me.ddlOpportunityStatusEdit.Enabled = False
        Me.ddlOpportunityStatusEdit.BackColor = Drawing.Color.LightGray
        Me.txtRevenueUpsideEdit.Enabled = False
        Me.txtRevenueUpsideEdit.BackColor = Drawing.Color.LightGray
        Me.lblDsplyHierarchyEdit.BackColor = Drawing.Color.LightGray

        Me.ddlOutlookStatusEdit.Enabled = False
        Me.ddlOutlookStatusEdit.BackColor = Drawing.Color.LightGray
        Me.ddlMPOverrideEdit.Enabled = False
        Me.ddlMPOverrideEdit.BackColor = Drawing.Color.LightGray
        Me.ddlOrderFlowEdit.Enabled = False
        Me.ddlOrderFlowEdit.BackColor = Drawing.Color.LightGray

        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.LightGray

        Me.btnPSValueDown.Enabled = False
        Me.btnPSValueUp.Enabled = False
        Me.btnWinPercentDown.Enabled = False
        Me.btnWinPercentUp.Enabled = False
        Me.btnRevenueUpsideDown.Enabled = False
        Me.btnRevenueUpsideUP.Enabled = False

        Me.pnlAddCustomer.Visible = False
        Me.pnlDetail.BackColor = Drawing.Color.White
        Me.pnlDetail.Visible = True

        Dim dtOpportunityStateDetailList As DataTable = appService.GetOpportunityDetailList(Session("intOpportunityStateDetail")).Tables(0)

        Session("dtOpportunityStateDetailList") = dtOpportunityStateDetailList

        Me.lblOpportunityDetailTypeEdit.Text = dtOpportunityStateDetailList.Rows(0).Item("detail_type_desc")
        Me.ddlOpportunityDetailTypeEdit.Enabled = True
        Me.ddlOpportunityDetailTypeEdit.DataSource = dtOpportunityStateDetailList
        Me.ddlOpportunityDetailTypeEdit.DataValueField = "detail_value_id"
        Me.ddlOpportunityDetailTypeEdit.DataTextField = "detail_value_desc"
        Me.ddlOpportunityDetailTypeEdit.DataBind()

        Me.ddlOpportunityDetailTypeEdit.SelectedValue = dtOpportunityStateDetailList.Rows(0).Item("current_value_id")

    End Sub


    Private Sub ModeAddProspect()

        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = False
        Me.btnOpportunityOK.Visible = False
        Me.btnOpportunityCancel.Enabled = False
        Me.btnOpportunityCancel.Visible = False
        Me.btnCancelAddCustomer.Visible = True


        Me.pnlView.BackColor = Drawing.Color.LightGray

        Me.lblOpportunityComment.Visible = False
        Me.txtOpportunityCommentEdit.Visible = False

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityEdit.Enabled = False
        Me.txtOpportunityEdit.BackColor = Drawing.Color.LightGray
        Me.calCloseDateEdit.Enabled = False
        Me.calCloseDateEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityCommentEdit.Enabled = False
        Me.txtOpportunityCommentEdit.BackColor = Drawing.Color.LightGray
        Me.txtPSValueEdit.Enabled = False
        Me.txtPSValueEdit.BackColor = Drawing.Color.LightGray
        Me.ddlFunnelPhaseEdit.Enabled = False
        Me.ddlFunnelPhaseEdit.BackColor = Drawing.Color.LightGray
        Me.txtWinPercentEdit.Enabled = False
        Me.txtWinPercentEdit.BackColor = Drawing.Color.LightGray
        Me.ddlOpportunityStatusEdit.Enabled = False
        Me.ddlOpportunityStatusEdit.BackColor = Drawing.Color.LightGray
        Me.txtRevenueUpsideEdit.Enabled = False
        Me.txtRevenueUpsideEdit.BackColor = Drawing.Color.LightGray
        Me.lblDsplyHierarchyEdit.BackColor = Drawing.Color.LightGray

        Me.ddlOutlookStatusEdit.Enabled = False
        Me.ddlOutlookStatusEdit.BackColor = Drawing.Color.LightGray
        Me.ddlMPOverrideEdit.Enabled = False
        Me.ddlMPOverrideEdit.BackColor = Drawing.Color.LightGray
        Me.ddlOrderFlowEdit.Enabled = False
        Me.ddlOrderFlowEdit.BackColor = Drawing.Color.LightGray

        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.LightGray

        Me.btnPSValueDown.Enabled = False
        Me.btnPSValueUp.Enabled = False
        Me.btnWinPercentDown.Enabled = False
        Me.btnWinPercentUp.Enabled = False
        Me.btnRevenueUpsideDown.Enabled = False
        Me.btnRevenueUpsideUP.Enabled = False

        Me.pnlAddCustomer.BackColor = Drawing.Color.White
        Me.pnlAddCustomer.Visible = True

        Me.txtACCustomerName.Text = ""
        Me.ddlCustomerType.SelectedValue = "C"

        Me.pnlAddCustomer.Visible = True

    End Sub
#End Region

    Private Sub ViewListPopulate()

        Me.ddlCustomerEdit.DataSource = dtCustomerList
        Me.ddlCustomerEdit.DataValueField = "customer_id"
        Me.ddlCustomerEdit.DataTextField = "customer_name"
        Me.ddlCustomerEdit.DataBind()

        Me.ddlFunnelPhaseEdit.DataSource = dtFunnelPhaseList
        Me.ddlFunnelPhaseEdit.DataValueField = "funnel_phase_id"
        Me.ddlFunnelPhaseEdit.DataTextField = "funnel_phase_short_name"
        Me.ddlFunnelPhaseEdit.DataBind()

        Me.ddlOpportunityStatusEdit.DataSource = dtStatusList
        Me.ddlOpportunityStatusEdit.DataValueField = "opportunity_status_id"
        Me.ddlOpportunityStatusEdit.DataTextField = "opportunity_status_short_name"
        Me.ddlOpportunityStatusEdit.DataBind()

        Me.ddlOutlookStatusEdit.DataSource = dtOutlookList
        Me.ddlOutlookStatusEdit.DataValueField = "detail_value_id"
        Me.ddlOutlookStatusEdit.DataTextField = "detail_value_short_name"
        Me.ddlOutlookStatusEdit.DataBind()

        Me.ddlMPOverrideEdit.DataSource = dtMPOverrideList
        Me.ddlMPOverrideEdit.DataValueField = "detail_value_id"
        Me.ddlMPOverrideEdit.DataTextField = "detail_value_short_name"
        Me.ddlMPOverrideEdit.DataBind()

        Me.ddlOrderFlowEdit.DataSource = dtOrderFlowList
        Me.ddlOrderFlowEdit.DataValueField = "detail_value_id"
        Me.ddlOrderFlowEdit.DataTextField = "detail_value_short_name"
        Me.ddlOrderFlowEdit.DataBind()

        If Session("strMode") = "Add" Then
            Me.ddlCustomerEdit.Items.Insert(0, liChoose)
            Me.ddlFunnelPhaseEdit.Items.Insert(0, liChoose)
            Me.ddlOpportunityStatusEdit.Items.Insert(0, liChoose)
            Me.ddlOutlookStatusEdit.Items.Insert(0, liChoose)
            Me.ddlMPOverrideEdit.Items.Insert(0, liChoose)
            Me.ddlOrderFlowEdit.Items.Insert(0, liChoose)
        End If

        Me.ddlCustomerType.Items.Clear()
        Me.ddlCustomerType.Items.Insert(0, New ListItem("Prospect", "P"))
        Me.ddlCustomerType.Items.Insert(0, New ListItem("Customer", "C"))

    End Sub


    Protected Sub btnHierarchy_Click(sender As Object, e As EventArgs) Handles btnHierarchy.Click
        Session("strModeLast") = Session("strMode")
        Session("strMode") = "File"
        PageMode()
    End Sub

    Protected Sub btnAddProspect_Click(sender As Object, e As EventArgs) Handles btnAddProspect.Click

        If Session("OppFilter") > 0 Then
            Session("strModeLast") = Session("strMode")
            Session("strMode") = "AddProspect"
            PageMode()
        Else
            Me.lblTitle.Text = "Hierarchy Error"
            Me.txtMessage.Text = "Please Select a Hierarchy level above Customer."
            Me.lblOpportunityComment.Visible = False
            Me.txtOpportunityCommentEdit.Visible = False
            Me.pnlMsg.Visible = True
            Me.btnOpportunityOK.Enabled = False
            Me.btnOpportunityCancel.Enabled = False
        End If

    End Sub

    Protected Sub btnCancelAddCustomer_Click(sender As Object, e As EventArgs) Handles btnCancelAddCustomer.Click
        'Me.lblTitle.Text = "Title"
        'Me.txtMessage.Text = "Message"
        'Me.lblOpportunityComment.Visible = True
        'Me.txtOpportunityCommentEdit.Visible = True
        'Me.pnlMsg.Visible = False
        'Me.pnlAddCustomer.Visible = False
        'Me.btnOpportunityOK.Enabled = True
        'Me.btnOpportunityCancel.Enabled = True

        Session("strMode") = Session("strModeLast")
        PageMode()
    End Sub




    Private Function GetRow(ByVal dtSearchTable As DataTable, strField As String, lngValue As Long) As DataRow
        Dim drFoundRows() As DataRow

        drFoundRows = dtSearchTable.Select(strField & " = " & lngValue)

        Return drFoundRows(0)

    End Function

    Private Function GetRow(ByVal dtSearchTable As DataTable, strField As String, strValue As String) As DataRow
        Dim drFoundRows() As DataRow

        drFoundRows = dtSearchTable.Select(strField & " = '" & strValue & "'")

        Return drFoundRows(0)

    End Function

    Protected Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload



    End Sub

    Protected Sub gvMain_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMain.RowDataBound
        Dim strDate As String = ""

        For Each cell As TableCell In e.Row.Cells
            strDate = cell.Text

            Try
                strDate = Date.Parse(cell.Text)

                If strDate > Date.Now Then
                    cell.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception

            End Try

        Next

    End Sub
End Class
