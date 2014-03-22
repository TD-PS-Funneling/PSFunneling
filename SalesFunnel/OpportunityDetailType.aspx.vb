Option Explicit On

Imports System.Data
Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports Teradata.Client.Provider

Partial Class OpportunityDetailType
    Inherits System.Web.UI.Page

#Region "Class properties"
    Dim sysUser As System.Security.Principal.IPrincipal

    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = Nothing

    Dim _resourceID As String
    Dim _mode As String
    Dim _isShowList As Boolean

#End Region

#Region "Page Events"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        _resourceID = hdnResourceID.Value
        _mode = hdnMode.Value

        If _resourceID = "-1" Then
            _isShowList = True
        Else
            _isShowList = False
        End If

        If (_isShowList) Then
            dtGridView = appService.GetOpportunityTypeList.Tables(0)
            Bindlist()
        Else
            SetPageMode()

            If (pnlOpportunityTypeList.Visible) Then
                BindData()
            End If
        End If

        ShowHidePanel()

    End Sub

#End Region

#Region "PRIVATE METHODS"
    Private Sub Bindlist()
        Dim dvSort As New DataView(dtGridView)
        Dim strfilter As String = String.Empty

        If (Not IsNothing(Session("SortDirection"))) And (Not IsNothing(Session("SortExpression"))) Then
            dvSort.Sort = Session("SortExpression").ToString() + " " + SortDirectionSql(Session("SortDirection").ToString())
        End If

        Me.gvOpportunity.DataSource = dvSort
        Dim intIndex As Integer = 0

        If Not IsNothing(Session("PageIndex")) Then
            intIndex = CInt(Session("PageIndex"))
        End If

        Me.gvOpportunity.PageSize = CInt(ddlDisplay.SelectedValue)
        Me.gvOpportunity.PageIndex = intIndex
        Me.gvOpportunity.DataBind()
    End Sub
    Private Function GetNextID() As Integer
        Dim nNextID As Integer
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()

        Dim strSQL As String = String.Format("SELECT MAX(detail_type_id)+1 NEWKEY from FIHL_POC.util_opportunity_detail_type NEWID")

        Dim dtTable As DataTable = objDataAccess.GetDataTable(strSQL)

        If dtTable.Rows.Count > 0 Then
            Dim dtRow As DataRow

            dtRow = dtTable.Rows.Item(0)
            nNextID = Convert.ToInt32(dtRow("NEWKEY").ToString)
        End If

        Return nNextID
    End Function
    Private Sub ShowHidePanel()
        If _isShowList Then
            pnlOpportunityTypeList.Visible = True
            pnlOpportunityTypeDetail.Visible = False
        Else
            pnlOpportunityTypeList.Visible = False
            pnlOpportunityTypeDetail.Visible = True
        End If
    End Sub
#End Region

    Protected Sub gvOpportunity_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvOpportunity.PageIndexChanging
        Session("PageIndex") = e.NewPageIndex
        dtGridView = appService.GetOpportunityTypeList().Tables(0)
        Bindlist()
    End Sub
    Protected Sub BindData()
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()

        Dim strSQL As String = String.Format("SELECT * FROM FIHL_POC.util_opportunity_detail_type WHERE detail_type_id = {0}", _resourceID)

        Dim dtTable As DataTable = objDataAccess.GetDataTable(strSQL)

        If dtTable.Rows.Count > 0 Then
            Dim dtRow As DataRow

            dtRow = dtTable.Rows.Item(0)

            txtDetailTypeID.Text = dtRow("detail_type_id").ToString
            txtDetailName.Text = dtRow("detail_type_short_name").ToString
            txtDetailDesc.Text = dtRow("detail_type_desc").ToString
            txtDetailSortNum.Text = dtRow("detail_type_sort_nbr").ToString
            txtDetailVis.Text = dtRow("DETAIL_TYPE_VISIBLE_IND").ToString

            lblDetailTypeIDView.Text = txtDetailTypeID.Text
            lblDetailNameView.Text = txtDetailName.Text
            lblDetailDescView.Text = txtDetailDesc.Text
            lblDetailSortNumView.Text = txtDetailSortNum.Text
            lblDetailVisView.Text = txtDetailVis.Text
        End If
    End Sub
    Protected Sub SetPageMode()
        Select Case _mode
            Case "View"
                txtDetailTypeID.Visible = False
                txtDetailName.Visible = False
                txtDetailDesc.Visible = False
                txtDetailSortNum.Visible = False
                txtDetailVis.Visible = False

                lblDetailTypeIDView.Visible = True
                lblDetailNameView.Visible = True
                lblDetailDescView.Visible = True
                lblDetailSortNumView.Visible = True
                lblDetailVisView.Visible = True

                btnEdit.Visible = True
                btnSave.Visible = False
                btnDelete.Visible = True
            Case "Add"
                lblDetailTypeIDView.Visible = True
                lblDetailTypeIDView.Text = GetNextID()
                txtDetailTypeID.Visible = False
                txtDetailTypeID.Text = lblDetailTypeIDView.Text

                
                txtDetailName.Visible = True
                txtDetailDesc.Visible = True
                txtDetailSortNum.Visible = True
                txtDetailVis.Visible = True

                lblDetailNameView.Visible = False
                lblDetailDescView.Visible = False
                lblDetailSortNumView.Visible = False
                lblDetailVisView.Visible = False

                btnEdit.Visible = False
                btnSave.Visible = True

                If _resourceID = "0" Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Visible = True
                End If
            Case Else
                If _resourceID = "0" Then
                    txtDetailTypeID.Visible = True

                    lblDetailTypeIDView.Visible = False
                Else
                    txtDetailTypeID.Visible = False

                    lblDetailTypeIDView.Visible = True
                End If

                txtDetailTypeID.Visible = False
                txtDetailName.Visible = True
                txtDetailDesc.Visible = True
                txtDetailSortNum.Visible = True
                txtDetailVis.Visible = False

                lblDetailTypeIDView.Visible = True
                lblDetailNameView.Visible = False
                lblDetailDescView.Visible = False
                lblDetailSortNumView.Visible = False
                lblDetailVisView.Visible = True

                btnEdit.Visible = False
                btnSave.Visible = True

                If _resourceID = "0" Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Visible = True
                End If
        End Select

    End Sub
    Protected Sub gvOpportunity_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvOpportunity.Sorting
        Session("SortExpression") = e.SortExpression
        If Not IsNothing(Session("SortDirection")) Then
            If (e.SortDirection = SortDirection.Ascending) Then
                Session("SortDirection") = SortDirection.Descending.ToString()
            Else
                Session("SortDirection") = SortDirection.Ascending.ToString()
            End If

        Else
            Session("SortDirection") = e.SortDirection
        End If

        dtGridView = appService.GetOpportunityTypeList().Tables(0)
        Bindlist()
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

    Protected Sub gvOpportunity_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvOpportunity.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim strId As String = e.Row.Cells(1).Text
            Dim lnkview As LinkButton = CType(e.Row.Cells(6).Controls(1), LinkButton)
            Dim lnkedit As LinkButton = CType(e.Row.Cells(7).Controls(1), LinkButton)

            lnkview.OnClientClick = String.Format("viewResource('{0}') ", strId)
            lnkedit.OnClientClick = String.Format("editResource('{0}')", strId)
        End If

        If e.Row.RowType = DataControlRowType.Header Then
            Dim chkSelectAll As CheckBox = CType(e.Row.Cells(0).Controls(0), CheckBox)
            chkSelectAll.Attributes.Add("onclick", "setAllElements(this.id);")
        End If
    End Sub
    Protected Function SaveResource() As Integer
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()
        Dim sb As New StringBuilder

        Dim cmd As TdCommand = New TdCommand()

        cmd.CommandText = "FIHL_POC.SP_UTIL_OPP_DETAIL_TYPE_UPSERT"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("oMessage", TdType.Integer)
        cmd.Parameters.Add("detailTypeID", TdType.Integer)
        cmd.Parameters.Add("detailTypeShortName", TdType.VarChar)
        cmd.Parameters.Add("detailTypeDesc", TdType.VarChar)
        cmd.Parameters.Add("detailTypeSortNbr", TdType.ByteInt)
        cmd.Parameters.Add("detailTypeVisInd", TdType.ByteInt)
        cmd.Parameters.Add("detailTypeStartDate", TdType.VarChar)
        cmd.Parameters.Add("detailTypeEndDate", TdType.VarChar)

        cmd.Parameters("oMessage").Direction = ParameterDirection.Output
        cmd.Parameters("detailTypeID").Value = txtDetailTypeID.Text
        cmd.Parameters("detailTypeShortName").Value = txtDetailName.Text
        cmd.Parameters("detailTypeDesc").Value = txtDetailDesc.Text
        cmd.Parameters("detailTypeSortNbr").Value = txtDetailSortNum.Text
        cmd.Parameters("detailTypeVisInd").Value = txtDetailVis.Text
        cmd.Parameters("detailTypeStartDate").Value = ""
        cmd.Parameters("detailTypeEndDate").Value = ""

        Try
            objDataAccess.ExecCmd(cmd)
        Catch ex As Exception
            ShowAlert(ex.Message)
        End Try

        Return cmd.Parameters("oMessage").Value

    End Function

    Private Sub ShowAlert(ByVal strMsg As String)
        Dim CSM As ClientScriptManager = Page.ClientScript

        Dim strconfirm As String = String.Format("<script>alert('{0}');</script>", strMsg)
        CSM.RegisterClientScriptBlock(Me.GetType(), "Alert", strconfirm, False)

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'SaveResource()
        ShowAlert(SaveResource())
        hdnResourceID.Value = _resourceID

        BindData()
        RefreshPage()

    End Sub
    Private Sub RefreshPage()
        Response.Redirect("/OpportunityDetailType.aspx")
    End Sub

End Class

