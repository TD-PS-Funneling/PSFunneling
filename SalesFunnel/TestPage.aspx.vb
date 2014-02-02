Option Explicit On

Imports System.Data

Partial Class TestPage
    Inherits System.Web.UI.Page

    Dim appService As New FunnelAppSvc
    Dim dtTDImpersonationList As DataTable = appService.GetImpersonationList.Tables(0)


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsNothing(Session("strTRUEUserName")) Then
            If Session("strTRUEUserName") <> "SSCOTT" And Session("strTRUEUserName") <> "SS185226" And Session("strTRUEUserName") <> "HL180002" And Session("strTRUEUserName") <> "KB185024" Then
                Response.Redirect("~/OpportunityFile.aspx")
            End If
        Else
            Response.Redirect("~/OpportunityFile.aspx")
        End If


        If Not Page.IsPostBack Then
            Me.ddlImpersonateUser.DataSource = dtTDImpersonationList
            Me.ddlImpersonateUser.DataValueField = "qlookid"
            Me.ddlImpersonateUser.DataTextField = "full_name"
            Me.ddlImpersonateUser.DataBind()

            Me.ddlImpersonateUser.Items.Insert(0, New ListItem("Sam Scott (SS185226)", "SS185226"))

            Me.txtTrueUser.Text = Session("strTRUEUserName")

            Dim x As String = Session("strUserName")

            Me.ddlImpersonateUser.SelectedValue = Session("strUserName")
        End If

    End Sub

    Protected Sub btnImpersonate_Click(sender As Object, e As EventArgs) Handles btnImpersonate.Click

        If Not IsNothing(Session("strTRUEUserName")) Then
            If Session("strTRUEUserName") <> "SSCOTT" And Session("strTRUEUserName") <> "SS185226" And Session("strTRUEUserName") <> "HL180002" And Session("strTRUEUserName") <> "KB185024" Then
                Response.Redirect("~/OpportunityFile.aspx")
            End If
        Else
            Response.Redirect("~/OpportunityFile.aspx")
        End If

        If Me.ddlImpersonateUser.SelectedValue <> Session("strUserName") Then

            'Dim A As String = Me.ddlImpersonateUser.SelectedValue
            Session("strUserName") = Me.ddlImpersonateUser.SelectedValue
            Dim dtTDResourcePosition As DataTable = appService.GetResourcePosition(Session("strUserName")).Tables(0)
            If dtTDResourcePosition.Rows.Count > 0 Then
                Session("OppFilter") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
                Session("organization_struct_id") = dtTDResourcePosition.Rows(0).Item("organization_struct_id")
                Session("organization_id") = dtTDResourcePosition.Rows(0).Item("organization_id")
                Session("organization_hrrchy_id") = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")
                Session("organization_role_type_name") = dtTDResourcePosition.Rows(0).Item("organization_role_type_name")
                Session("organization_role_type_id") = dtTDResourcePosition.Rows(0).Item("organization_role_type_id")
                Session("InHierarchy") = 1

                Session("selected_hrrchy_id") = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")

                Dim A As Integer = dtTDResourcePosition.Rows(0).Item("organization_hrrchy_id")
                Dim B As Integer = Session("selected_hrrchy_id")
                Session("strMode") = "Main"
                Response.Redirect("~/OpportunityFile.aspx")
            End If
        End If

    End Sub
End Class
