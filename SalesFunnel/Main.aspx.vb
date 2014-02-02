Option Explicit On

Imports System.Data


Partial Class Main
    Inherits System.Web.UI.Page

    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = appService.GetOpportunityList.Tables(0)
    Dim dtCustomerFilter As DataTable = appService.GetCustomerFilter.Tables(0)
    Dim dtPSPartnerFilter As DataTable = appService.GetPSPartnerFilter.Tables(0)
    Dim dtAreaFilter As DataTable = appService.GetAreaFilter.Tables(0)
    Dim dtMPartnerFilter As DataTable = appService.GetMPartnerFilter.Tables(0)

    'Dim dsTDHierarchy As DataSet = appService.GetTDHierarchy
    Dim dtTDHierarchy As DataTable = appService.GetTDHierarchy.Tables(0)

    Dim dtStatusList As DataTable = appService.GetOpportunityStatusList.Tables(0)
    Dim dtFunnelPhaseList As DataTable = appService.GetFunnelPhaseList.Tables(0)
    Dim dtCustomerList As DataTable = appService.GetCustomerList.Tables(0)
    Dim dtAccountTeamList As DataTable = appService.GetAccountTeamList.Tables(0)

    Dim strAll As String = "- All -"
    Dim strChoose As String = "- Choose One - "
    Dim liAll As New ListItem(strAll, strAll)
    Dim liChoose As New ListItem(strChoose, -1)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Me.gvMain.AllowSorting = True
            Session("SortOrder") = "customer_name ASC"
            Session("PageIndex") = "0"
            Session("Filter") = ""
            Session("strMode") = "Main"
            Session("strModeLast") = "Main"
            Session("intOpportunity") = 0
            Session("intOpportunityState") = 0
            Session("intOpportunityStateDetail") = 0

            'DoSomething()

            DoSomethingElse()

            BindData()

            PageMode()

        End If
    End Sub

    'Private Sub DoSomething(Optional lngParent As Long = 0, Optional tnNode As TreeNode = Nothing)

    '    If dtTDHierarchy.Rows.Count > 0 Then
    '        Dim drRow As DataRow
    '        For Each drRow In dtTDHierarchy.Rows
    '            If drRow("organization_struct_parent_id") = lngParent Then
    '                Dim tnNewNode As TreeNode
    '                tnNewNode = New TreeNode(drRow("organization_struct_name").ToString, drRow("organization_struct_id").ToString())
    '                tnNewNode.SelectAction = TreeNodeSelectAction.Select
    '                tnNewNode.PopulateOnDemand = False
    '                tnNewNode.CollapseAll()
    '                If IsNothing(tnNode) Then
    '                    Me.tvMain.Nodes.Add(tnNewNode)
    '                Else
    '                    tnNode.ChildNodes.Add(tnNewNode)
    '                End If
    '                DoSomething(drRow("organization_struct_id"), tnNewNode)
    '            End If
    '        Next
    '    End If
    'End Sub

    Private Sub DoSomethingElse(Optional lngParent As Long = 0, Optional mnuItem As MenuItem = Nothing)

        If dtTDHierarchy.Rows.Count > 0 Then
            Dim drRow As DataRow
            For Each drRow In dtTDHierarchy.Rows
                If drRow("organization_struct_parent_id") = lngParent Then
                    Dim mnuNewItem As MenuItem
                    mnuNewItem = New MenuItem(drRow("organization_struct_name").ToString, drRow("organization_struct_id").ToString())
                    mnuNewItem.Selectable = True
                    'tnNewNode.SelectAction = TreeNodeSelectAction.Select
                    'tnNewNode.PopulateOnDemand = False
                    'tnNewNode.CollapseAll()
                    If IsNothing(mnuItem) Then
                        Me.mnuMain.Items.Add(mnuNewItem)
                    Else
                        mnuItem.ChildItems.Add(mnuNewItem)
                    End If
                    DoSomethingElse(drRow("organization_struct_id"), mnuNewItem)
                End If
            Next
        End If
    End Sub



    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        Me.ddlCustomer.SelectedValue = Me.ddlCustomer.Items.FindByText(strAll).Value
        Me.ddlPSPartner.SelectedValue = Me.ddlPSPartner.Items.FindByText(strAll).Value
        Me.ddlFunnelPhase.SelectedValue = Me.ddlFunnelPhase.Items.FindByText(strAll).Value
        Me.ddlMPartner.SelectedValue = Me.ddlMPartner.Items.FindByText(strAll).Value
        Me.ddlOrgStruct.SelectedValue = Me.ddlOrgStruct.Items.FindByText(strAll).Value
        Me.ddlStatus.SelectedValue = Me.ddlStatus.Items.FindByText(strAll).Value

        Session("Filter") = ""

        BindData()

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        FilterData()

    End Sub
    Private Sub FilterData()
        Dim strFilter As String = ""
        Dim intFilterCount As Integer = 0

        If Me.ddlCustomer.SelectedValue <> strAll Then
            strFilter += "customer_name= '" & Me.ddlCustomer.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        If Me.ddlPSPartner.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "psp_last_name= '" & Me.ddlPSPartner.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        If Me.ddlFunnelPhase.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "funnel_phase_short_name= '" & Me.ddlFunnelPhase.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        If Me.ddlMPartner.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "mpr_last_name= '" & Me.ddlMPartner.SelectedValue.ToString & "'"
            intFilterCount += 1
        End If

        If Me.ddlOrgStruct.SelectedValue <> strAll Then
            If intFilterCount > 0 Then
                strFilter += " AND "
            End If
            strFilter += "organization_struct_name= '" & Me.ddlOrgStruct.SelectedValue.ToString & "'"
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
                    intOpportunityResult = appService.AddOpportunity(Me.ddlCustomerEdit.SelectedValue, Me.txtOpportunityEdit.Text)
                    If intOpportunityResult > 0 Then
                        Session("intOpportunity") = intOpportunityResult

                        intOpportunityStateResult = appService.AddOpportunityState(intOpportunityResult, Me.calCloseDateEdit.SelectedDate, Me.txtPSValueEdit.Text, Me.ddlFunnelPhaseEdit.SelectedValue, Me.txtWinPercentEdit.Text, Me.ddlOpportunityStatusEdit.Text, Me.txtOpportunityCommentEdit.Text, Me.txtRevenueUpsideEdit.Text, Session("strUserName").ToString)

                        If intOpportunityStateResult > 0 Then
                            Session("intOpportunityState") = intOpportunityStateResult

                            intOpportunityStateDetailResult = appService.AddOpportunityStateDetail(intOpportunityStateResult)

                            If intOpportunityStateDetailResult > 0 Then
                                Session("strModeLast") = Session("strMode")
                                Session("strMode") = "View"
                                PageMode()
                            End If
                        End If
                    End If
                End If
            Case "Edit"
                If ValidateAdd() Then
                    Dim intOpportunityDelta As Integer = 0
                    Dim intOpportunityStateDelta As Integer = 0
                    Dim dtOpportunity As DataTable = Session("dtOpportunity")

                    If Me.txtOpportunityEdit.Text <> dtOpportunity.Rows.Item(0).Item("opportunity_desc") Then
                        intOpportunityDelta += 1
                    End If
                    If Me.ddlAccountTeamEdit.SelectedValue <> dtOpportunity.Rows.Item(0).Item("account_team_id") Then
                        intOpportunityDelta += 1
                    End If
                    If intOpportunityDelta > 0 Then
                        intResult = appService.SetOpportunity(Session("intOpportunity"), Me.ddlCustomerEdit.SelectedValue, Me.ddlAccountTeamEdit.SelectedValue, Me.txtOpportunityEdit.Text)
                    End If
                    If Me.calCloseDateEdit.SelectedDate <> dtOpportunity.Rows.Item(0).Item("close_date") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.txtPSValueEdit.Text <> dtOpportunity.Rows.Item(0).Item("ps_value_nbr") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.ddlFunnelPhaseEdit.SelectedValue <> dtOpportunity.Rows.Item(0).Item("funnel_phase_id") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.txtWinPercentEdit.Text <> dtOpportunity.Rows.Item(0).Item("win_percent") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.ddlOpportunityStatusEdit.SelectedValue <> dtOpportunity.Rows.Item(0).Item("opportunity_status_id") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.txtOpportunityCommentEdit.Text <> dtOpportunity.Rows.Item(0).Item("opportunity_comment_desc") Then
                        intOpportunityStateDelta += 1
                    End If
                    If Me.txtRevenueUpsideEdit.Text <> dtOpportunity.Rows.Item(0).Item("revenue_upside") Then
                        intOpportunityStateDelta += 1
                    End If
                    If intOpportunityStateDelta > 0 Then
                        intResult = appService.SetOpportunityState(Session("intOpportunityState"), Session("intOpportunity"), Me.calCloseDateEdit.SelectedDate, Me.txtPSValueEdit.Text, Me.ddlFunnelPhaseEdit.SelectedValue, Me.txtWinPercentEdit.Text, Me.ddlOpportunityStatusEdit.SelectedValue, Me.txtOpportunityCommentEdit.Text, Me.txtRevenueUpsideEdit.Text, Session("strUserName"))
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
        If Me.ddlAccountTeamEdit.SelectedValue = -1 Then strInvalid += "Please select an Account Team!" & vbCrLf
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
        End Select
    End Sub

    Private Sub PageMode()

        Select Case Session("strMode")
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
        End Select

    End Sub


#Region "Modes Main, Add, View, Edit, EditDetail"

    Private Sub ModeMain()

        dtGridView = appService.GetOpportunityList.Tables(0)
        Session("Filter") = ""
        BindData()

        Me.ddlCustomer.DataSource = dtCustomerFilter
        Me.ddlCustomer.DataValueField = "customer_name"
        Me.ddlCustomer.DataTextField = "customer_name"
        Me.ddlCustomer.DataBind()
        Me.ddlCustomer.Items.Insert(0, liAll)

        Me.ddlPSPartner.DataSource = dtPSPartnerFilter
        Me.ddlPSPartner.DataValueField = "psp_last_name"
        Me.ddlPSPartner.DataTextField = "psp_last_name"
        Me.ddlPSPartner.DataBind()
        Me.ddlPSPartner.Items.Insert(0, liAll)

        Me.ddlFunnelPhase.DataSource = dtFunnelPhaseList
        Me.ddlFunnelPhase.DataValueField = "funnel_phase_short_name"
        Me.ddlFunnelPhase.DataTextField = "funnel_phase_short_name"
        Me.ddlFunnelPhase.DataBind()
        Me.ddlFunnelPhase.Items.Insert(0, liAll)

        Me.ddlMPartner.DataSource = dtMPartnerFilter
        Me.ddlMPartner.DataValueField = "mpr_last_name"
        Me.ddlMPartner.DataTextField = "mpr_last_name"
        Me.ddlMPartner.DataBind()
        Me.ddlMPartner.Items.Insert(0, liAll)

        Me.ddlOrgStruct.DataSource = dtAreaFilter
        Me.ddlOrgStruct.DataValueField = "organization_struct_name"
        Me.ddlOrgStruct.DataTextField = "organization_struct_name"
        Me.ddlOrgStruct.DataBind()
        Me.ddlOrgStruct.Items.Insert(0, liAll)

        Me.ddlStatus.DataSource = dtStatusList
        Me.ddlStatus.DataValueField = "opportunity_status_short_name"
        Me.ddlStatus.DataTextField = "opportunity_status_short_name"
        Me.ddlStatus.DataBind()
        Me.ddlStatus.Items.Insert(0, liAll)

        Me.ddlCustomer.SelectedValue = Me.ddlCustomer.Items.FindByText(strAll).Value
        Me.ddlPSPartner.SelectedValue = Me.ddlPSPartner.Items.FindByText(strAll).Value
        Me.ddlFunnelPhase.SelectedValue = Me.ddlFunnelPhase.Items.FindByText(strAll).Value
        Me.ddlMPartner.SelectedValue = Me.ddlMPartner.Items.FindByText(strAll).Value
        Me.ddlOrgStruct.SelectedValue = Me.ddlOrgStruct.Items.FindByText(strAll).Value
        Me.ddlStatus.SelectedValue = Me.ddlStatus.Items.FindByText(strAll).Value

        Me.pnlMain.Visible = True
        Me.pnlView.Visible = False

    End Sub

    Private Sub ModeAdd()

        ViewListPopulate()

        Me.lblAction.Text = "Add Opportunity"
        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = True
        Me.btnOpportunityOK.Visible = True
        Me.btnOpportunityCancel.Enabled = True
        Me.btnOpportunityCancel.Visible = True

        Me.pnlView.BackColor = Drawing.Color.White

        Me.pnlDetail.Visible = False

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = True
        Me.ddlCustomerEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityEdit.Enabled = True
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.ddlAccountTeamEdit.Enabled = True
        Me.ddlAccountTeamEdit.BackColor = Drawing.Color.White
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
        Me.ddlAccountTeamEdit.SelectedValue = Me.ddlAccountTeamEdit.Items.FindByValue(-1).Value
        Me.calCloseDateEdit.SelectedDate = Now.Date
        Me.calCloseDateEdit.VisibleDate = Now.Date
        Me.txtOpportunityCommentEdit.Text = ""
        Me.txtPSValueEdit.Text = 0
        Me.ddlFunnelPhaseEdit.SelectedValue = Me.ddlFunnelPhaseEdit.Items.FindByValue(-1).Value
        Me.txtWinPercentEdit.Text = 0
        Me.ddlOpportunityStatusEdit.SelectedValue = Me.ddlOpportunityStatusEdit.Items.FindByValue(1).Value
        Me.txtRevenueUpsideEdit.Text = 0

        Dim dtOpportunityDetail As DataTable = appService.GetOpportunityDetailBlank().Tables(0)
        Session("dtOpportunityDetail") = dtOpportunityDetail

        Dim dvDetail As New DataView(dtOpportunityDetail)

        Me.gvDetail.DataSource = dvDetail
        Me.gvDetail.DataBind()

        Me.pnlMain.Visible = False
        Me.pnlView.Visible = True
    End Sub

    Private Sub ModeView()

        ViewListPopulate()

        Me.lblAction.Text = "View Opportunity"
        Me.btnBack.Enabled = True
        Me.btnBack.Visible = True
        Me.btnOpportunityEdit.Enabled = True
        Me.btnOpportunityEdit.Visible = True
        Me.btnOpportunityOK.Enabled = False
        Me.btnOpportunityOK.Visible = False
        Me.btnOpportunityCancel.Enabled = False
        Me.btnOpportunityCancel.Visible = False

        Me.pnlView.BackColor = Drawing.Color.White

        Me.pnlDetail.Visible = False

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.White
        Me.txtOpportunityEdit.Enabled = False
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.ddlAccountTeamEdit.Enabled = False
        Me.ddlAccountTeamEdit.BackColor = Drawing.Color.White
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
        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.White

        Me.btnPSValueDown.Enabled = False
        Me.btnPSValueUp.Enabled = False
        Me.btnWinPercentDown.Enabled = False
        Me.btnWinPercentUp.Enabled = False
        Me.btnRevenueUpsideDown.Enabled = False
        Me.btnRevenueUpsideUP.Enabled = False


        Dim dtOpportunity As DataTable = appService.GetOpportunity(Session("intOpportunity")).Tables(0)
        Dim dtOpportunityDetail As DataTable = appService.GetOpportunityDetail(dtOpportunity.Rows(0).Item("opportunity_state_id")).Tables(0)

        Session("intOpportunityState") = dtOpportunity.Rows.Item(0).Item("opportunity_state_id")
        Session("dtOpportunity") = dtOpportunity
        Session("dtOpportunityDetail") = dtOpportunityDetail

        Me.ddlCustomerEdit.SelectedValue = dtOpportunity.Rows(0).Item("customer_id")
        Me.txtOpportunityEdit.Text = dtOpportunity.Rows(0).Item("opportunity_desc")
        Me.ddlAccountTeamEdit.SelectedValue = dtOpportunity.Rows(0).Item("account_team_id")
        Me.calCloseDateEdit.SelectedDate = dtOpportunity.Rows(0).Item("close_date")
        Me.calCloseDateEdit.VisibleDate = dtOpportunity.Rows(0).Item("close_date")
        Me.txtOpportunityCommentEdit.Text = dtOpportunity.Rows(0).Item("opportunity_comment_desc")
        Me.txtPSValueEdit.Text = dtOpportunity.Rows(0).Item("ps_value_nbr")
        Me.ddlFunnelPhaseEdit.SelectedValue = dtOpportunity.Rows(0).Item("funnel_phase_id")
        Me.txtWinPercentEdit.Text = dtOpportunity.Rows(0).Item("win_percent")
        Me.ddlOpportunityStatusEdit.SelectedValue = dtOpportunity.Rows(0).Item("opportunity_status_id")
        Me.txtRevenueUpsideEdit.Text = dtOpportunity.Rows(0).Item("revenue_upside")

        Dim dvDetail As New DataView(dtOpportunityDetail)

        Me.gvDetail.DataSource = dvDetail
        Me.gvDetail.DataBind()

        Me.pnlMain.Visible = False
        Me.pnlView.Visible = True
    End Sub

    Private Sub ModeEdit()

        Me.btnBack.Enabled = False
        Me.btnBack.Visible = False
        Me.btnOpportunityEdit.Enabled = False
        Me.btnOpportunityEdit.Visible = False
        Me.btnOpportunityOK.Enabled = True
        Me.btnOpportunityOK.Visible = True
        Me.btnOpportunityCancel.Enabled = True
        Me.btnOpportunityCancel.Visible = True

        Me.pnlView.BackColor = Drawing.Color.White

        Me.lblOpportunityComment.Visible = True
        Me.txtOpportunityCommentEdit.Visible = True

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityEdit.Enabled = True
        Me.txtOpportunityEdit.BackColor = Drawing.Color.White
        Me.ddlAccountTeamEdit.Enabled = True
        Me.ddlAccountTeamEdit.BackColor = Drawing.Color.White
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

        Me.pnlView.BackColor = Drawing.Color.LightGray

        Me.lblOpportunityComment.Visible = False
        Me.txtOpportunityCommentEdit.Visible = False

        Me.ddlCustomerEdit.Enabled = False
        Me.ddlCustomerEdit.BackColor = Drawing.Color.LightGray
        Me.txtOpportunityEdit.Enabled = False
        Me.txtOpportunityEdit.BackColor = Drawing.Color.LightGray
        Me.ddlAccountTeamEdit.Enabled = False
        Me.ddlAccountTeamEdit.BackColor = Drawing.Color.LightGray
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
        Me.gvDetail.Enabled = False
        Me.gvDetail.BackColor = Drawing.Color.LightGray

        Me.btnPSValueDown.Enabled = False
        Me.btnPSValueUp.Enabled = False
        Me.btnWinPercentDown.Enabled = False
        Me.btnWinPercentUp.Enabled = False
        Me.btnRevenueUpsideDown.Enabled = False
        Me.btnRevenueUpsideUP.Enabled = False

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
#End Region

    Private Sub ViewListPopulate()

        Me.ddlCustomerEdit.DataSource = dtCustomerList
        Me.ddlCustomerEdit.DataValueField = "customer_id"
        Me.ddlCustomerEdit.DataTextField = "customer_name"
        Me.ddlCustomerEdit.DataBind()

        Me.ddlAccountTeamEdit.DataSource = dtAccountTeamList
        Me.ddlAccountTeamEdit.DataValueField = "account_team_id"
        Me.ddlAccountTeamEdit.DataTextField = "account_team_desc"
        Me.ddlAccountTeamEdit.DataBind()

        Me.ddlFunnelPhaseEdit.DataSource = dtFunnelPhaseList
        Me.ddlFunnelPhaseEdit.DataValueField = "funnel_phase_id"
        Me.ddlFunnelPhaseEdit.DataTextField = "funnel_phase_short_name"
        Me.ddlFunnelPhaseEdit.DataBind()

        Me.ddlOpportunityStatusEdit.DataSource = dtStatusList
        Me.ddlOpportunityStatusEdit.DataValueField = "opportunity_status_id"
        Me.ddlOpportunityStatusEdit.DataTextField = "opportunity_status_short_name"
        Me.ddlOpportunityStatusEdit.DataBind()

        If Session("strMode") = "Add" Then
            Me.ddlCustomerEdit.Items.Insert(0, liChoose)
            Me.ddlAccountTeamEdit.Items.Insert(0, liChoose)
            Me.ddlFunnelPhaseEdit.Items.Insert(0, liChoose)
            Me.ddlOpportunityStatusEdit.Items.Insert(0, liChoose)
        End If

    End Sub


End Class


