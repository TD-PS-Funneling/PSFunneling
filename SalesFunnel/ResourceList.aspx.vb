Option Explicit On

Imports System.Data
Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports Teradata.Client.Provider

Partial Class ResourceList
    Inherits System.Web.UI.Page
    Dim sysUser As System.Security.Principal.IPrincipal

    Dim appService As New FunnelAppSvc
    Dim dtGridView As DataTable = Nothing

    Dim _resourceID As String
    Dim _mode As String
    Dim _isShowList As Boolean



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
            dtGridView = appService.GetResourceList().Tables(0)
            BindList()
        Else
            SetPageMode()

            'only run when the previous page is the Resource List page. This can be checked if the _isShowList is false but the pnlResourceList is still visible.
            If (pnlResourceList.Visible) Then
                BindData()
            End If
        End If

        ShowHidePanel()
    End Sub
#End Region

#Region "GridView"

    Protected Sub gvResource_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvResource.PageIndexChanging
        Session("PageIndex") = e.NewPageIndex
        dtGridView = appService.GetResourceList().Tables(0)
        BindList()
    End Sub

    Protected Sub gvResource_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvResource.Sorting
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


        dtGridView = appService.GetResourceList().Tables(0)
        BindList()
    End Sub

    Protected Sub gvResource_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvResource.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim strId As String = e.Row.Cells(1).Text
            Dim lnkview As LinkButton = CType(e.Row.Cells(9).Controls(1), LinkButton)
            Dim lnkedit As LinkButton = CType(e.Row.Cells(10).Controls(1), LinkButton)

            lnkview.OnClientClick = String.Format("viewResource('{0}') ", strId)
            lnkedit.OnClientClick = String.Format("editResource('{0}')", strId)
        End If

        If e.Row.RowType = DataControlRowType.Header Then
            Dim chkSelectAll As CheckBox = CType(e.Row.Cells(0).Controls(0), CheckBox)
            chkSelectAll.Attributes.Add("onclick", "setAllElements(this.id);")
        End If
    End Sub
#End Region

#Region "Buttons"
    Protected Sub lnkdelete_Click(sender As Object, e As EventArgs)

    End Sub
#End Region


#Region "Drop Down List"
    Protected Sub ddlDisplay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDisplay.SelectedIndexChanged
        dtGridView = appService.GetResourceList().Tables(0)
        BindList()
    End Sub
#End Region

#Region "Private"
    'Data binding to Grid View
    Private Sub BindList()
        Dim dvSort As New DataView(dtGridView)
        Dim strfilter As String = String.Empty

        If (Not IsNothing(Session("SortDirection"))) And (Not IsNothing(Session("SortExpression"))) Then
            dvSort.Sort = Session("SortExpression").ToString() + " " + SortDirectionSql(Session("SortDirection").ToString())
        End If

        Me.gvResource.DataSource = dvSort
        Dim intIndex As Integer = 0

        If Not IsNothing(Session("PageIndex")) Then
            intIndex = CInt(Session("PageIndex"))
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


    Private Sub ShowHidePanel()
        If _isShowList Then
            pnlResourceList.Visible = True
            pnlResourceInfo.Visible = False
        Else
            pnlResourceList.Visible = False
            pnlResourceInfo.Visible = True
        End If
    End Sub
#End Region

#Region "Resource Information"

    Protected Sub BindData()
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()

        Dim strSQL As String = String.Format("SELECT * FROM FIHL_POC.RESOURCE WHERE resource_id = {0}", _resourceID)

        Dim dtTable As DataTable = objDataAccess.GetDataTable(strSQL)

        If dtTable.Rows.Count > 0 Then
            Dim dtRow As DataRow

            dtRow = dtTable.Rows.Item(0)

            txtQuicklookID.Text = dtRow("qlookid").ToString
            txtResourceNBR.Text = dtRow("RESOURCE_NBR").ToString
            txtFirstName.Text = dtRow("FIRST_NAME").ToString
            txtLastName.Text = dtRow("LAST_NAME").ToString
            txtMiddleInitial.Text = dtRow("MIDDLE_INITIAL").ToString

            txtTelephoneNumber.Text = dtRow("TELEPHONE_NBR").ToString
            txtCountryCode.Text = dtRow("resource_country_code").ToString


            lblQuickLookIDView.Text = txtQuicklookID.Text
            lblResourceNBRView.Text = txtResourceNBR.Text
            lblFirstNameView.Text = txtFirstName.Text
            lblLastNameView.Text = txtLastName.Text
            lblMiddleInitialView.Text = txtMiddleInitial.Text
            lblCountryCodeView.Text = txtCountryCode.Text
            lblTelephoneNumberView.Text = txtTelephoneNumber.Text

            'tcQuicklookID.Text = dtRow("qlookid").ToString
            'tcResourceNBR.Text = dtRow("RESOURCE_NBR").ToString
            'tcFirstName.Text = dtRow("FIRST_NAME").ToString
            'tcLastName.Text = dtRow("LAST_NAME").ToString
            'tcMiddleInitial.Text = dtRow("MIDDLE_INITIAL").ToString

            'tcTelephoneNumber.Text = dtRow("TELEPHONE_NBR").ToString
            'tcCountryCode.Text = dtRow("resource_country_code").ToString

        End If

    End Sub

    Protected Sub SetPageMode()

        Select Case _mode
            Case "View"

                txtQuicklookID.Visible = False

                txtResourceNBR.Visible = False
                txtFirstName.Visible = False
                txtMiddleInitial.Visible = False
                txtLastName.Visible = False

                txtTelephoneNumber.Visible = False
                txtCountryCode.Visible = False


                lblQuickLookIDView.Visible = True
                lblResourceNBRView.Visible = True
                lblFirstNameView.Visible = True
                lblLastNameView.Visible = True
                lblMiddleInitialView.Visible = True
                lblTelephoneNumberView.Visible = True
                lblCountryCodeView.Visible = True

                btnEdit.Visible = True
                btnSave.Visible = False
                btnDelete.Visible = True

            Case Else

                If _resourceID = "0" Then
                    txtQuicklookID.Visible = True

                    lblQuickLookIDView.Visible = False
                Else
                    txtQuicklookID.Visible = False

                    lblQuickLookIDView.Visible = True
                End If

                txtResourceNBR.Visible = True
                txtFirstName.Visible = True
                txtMiddleInitial.Visible = True
                txtLastName.Visible = True
                txtTelephoneNumber.Visible = True
                txtCountryCode.Visible = True


                lblResourceNBRView.Visible = False
                lblFirstNameView.Visible = False
                lblLastNameView.Visible = False
                lblMiddleInitialView.Visible = False
                lblCountryCodeView.Visible = False
                lblTelephoneNumberView.Visible = False


                btnEdit.Visible = False
                btnSave.Visible = True

                If _resourceID = "0" Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Visible = True
                End If

        End Select

    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _resourceID = "-1" Then
            Response.Redirect("~/ResourceList.aspx")
        End If
    End Sub

    Protected Function SaveResource() As Integer
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()
        Dim sb As New StringBuilder

        ' FIHL_POC.SP_RESOURCE_UPSERT(
        '								OUT oMESSAGE VARCHAR(10),
        '								IN iQLOOKID VARCHAR(10),
        '								IN iRESOURCE_NBR VARCHAR(25),
        '								IN iFIRST_NAME VARCHAR(25),
        '								IN iMIDDLE_INITIAL CHAR(1),
        '								IN iLAST_NAME VARCHAR(25),
        '								IN iTELEPHONE_NBR VARCHAR(25),
        '								IN iRESOURCE_COUNTRY_CODE CHAR(2))
        Dim cmd As TdCommand = New TdCommand()

        cmd.CommandText = "FIHL_POC.SP_RESOURCE_UPSERT"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("oMessage", TdType.Integer)
        cmd.Parameters.Add("QLID", TdType.VarChar)
        cmd.Parameters.Add("ResourceNo", TdType.VarChar)
        cmd.Parameters.Add("FirstName", TdType.VarChar)
        cmd.Parameters.Add("MiddleInitial", TdType.Char)
        cmd.Parameters.Add("LastName", TdType.VarChar)
        cmd.Parameters.Add("Tel", TdType.VarChar)
        cmd.Parameters.Add("Country", TdType.VarChar)

        cmd.Parameters("oMessage").Direction = ParameterDirection.Output

        cmd.Parameters("QLID").Value = txtQuicklookID.Text
        cmd.Parameters("ResourceNo").Value = txtResourceNBR.Text
        cmd.Parameters("FirstName").Value = txtFirstName.Text
        cmd.Parameters("MiddleInitial").Value = txtMiddleInitial.Text
        cmd.Parameters("LastName").Value = txtLastName.Text
        cmd.Parameters("Tel").Value = txtTelephoneNumber.Text
        cmd.Parameters("Country").Value = txtCountryCode.Text

        Try
            objDataAccess.ExecCmd(cmd)
        Catch ex As Exception
            ShowAlert(ex.Message)
        End Try

        Return cmd.Parameters("oMessage").Value

    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If QuickLookIDValidator.IsValid And QuickLookIDCustomValidator.IsValid And ResourceNBRValidator.IsValid And CountryCodeValidator.IsValid Then
            _resourceID = SaveResource()

            hdnResourceID.Value = _resourceID

            BindData()
        Else
            ShowAlert("Unable to save. Please correct invalid items on the page")

            _mode = "Edit"

            SetPageMode()

        End If
    End Sub

    Protected Sub QuickLookIDCustomValidator_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles QuickLookIDCustomValidator.ServerValidate
        Dim bValid As Boolean = True


        If _resourceID = "0" Then
            Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()
            Dim sb As New StringBuilder

            ' FIHL_POC.SP_RESOURCE_UPSERT(
            '								OUT oMESSAGE VARCHAR(10),
            '								IN iQLOOKID VARCHAR(10),
            '								IN iRESOURCE_NBR VARCHAR(25),
            '								IN iFIRST_NAME VARCHAR(25),
            '								IN iMIDDLE_INITIAL CHAR(1),
            '								IN iLAST_NAME VARCHAR(25),
            '								IN iTELEPHONE_NBR VARCHAR(25),
            '								IN iRESOURCE_COUNTRY_CODE CHAR(2))
            Dim cmd As TdCommand = New TdCommand()

            sb.Append(String.Format("SELECT count(1) c FROM FIHL_POC.RESOURCE WHERE QLOOKID = '{0}'", args.Value.ToString.Replace("'", "''")))

            Dim ds As DataTable = objDataAccess.GetDataTable(sb.ToString)

            If Integer.Parse(ds.Rows(0)(0)) > 0 Then
                bValid = False
            End If
        End If


        args.IsValid = bValid



    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim objDataAccess As Teradata_Access_Layer = New Teradata_Access_Layer()
        Dim sb As New StringBuilder

        'FIHL_POC.SP_RESOURCE_DELETE(
        '		OUT oMESSAGE INTEGER,
        '		IN  iRESOURCE_ID INTEGER)
        Dim cmd As TdCommand = New TdCommand()

        cmd.CommandText = "FIHL_POC.SP_RESOURCE_DELETE"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("oMessage", TdType.Integer)
        cmd.Parameters.Add("ResourceID", TdType.Integer)

        cmd.Parameters("oMessage").Direction = ParameterDirection.Output

        cmd.Parameters("ResourceID").Value = _resourceID

        Try
            objDataAccess.ExecCmd(cmd)
        Catch ex As Exception
            ShowAlert(ex.Message)
        End Try

        Response.Redirect("~/ResourceList.aspx")
    End Sub

    Private Sub ShowAlert(ByVal strMsg As String)
        Dim CSM As ClientScriptManager = Page.ClientScript

        Dim strconfirm As String = String.Format("<script>alert('{0}');</script>", strMsg)
        CSM.RegisterClientScriptBlock(Me.GetType(), "Alert", strconfirm, False)

    End Sub
#End Region





End Class
