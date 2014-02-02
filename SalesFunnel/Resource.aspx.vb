Option Explicit On

Imports System.Data
Imports Teradata.Client.Provider

Partial Class Resource
    Inherits System.Web.UI.Page

    Dim _resourceID As String
    Dim _mode As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        _resourceID = Request.QueryString("ID")
        _mode = Request.QueryString("Mode")

        If _resourceID = "0" Then
            _mode = "Edit"
        End If

        If Not IsPostBack Then
            SetPageMode()
            BindData()
        End If
    End Sub

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
        If _resourceID <> "0" And _mode = "Edit" Then
            ReloadPage(_resourceID, "View")
        Else
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

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ReloadPage(_resourceID, "Edit")
    End Sub

    Private Sub ReloadPage(ByVal id As Integer, ByVal mode As String)
        Response.Redirect(String.Format("Resource.aspx?ID={0}&Mode={1}", id, mode))
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If QuickLookIDValidator.IsValid And QuickLookIDCustomValidator.IsValid And ResourceNBRValidator.IsValid Then
            _resourceID = SaveResource()

            ReloadPage(_resourceID, "View")
        Else
            ShowAlert("Unable to save. Please correct invalid items on the page")
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
End Class
