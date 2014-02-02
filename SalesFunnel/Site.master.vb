Imports System.Data


Partial Class Site
    Inherits System.Web.UI.MasterPage
    Dim sysUser As System.Security.Principal.IPrincipal


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'If Session("strMode") = "Unauthorized" Then
        'Me.NavigationMenu.Enabled = False
        'Else
        Me.NavigationMenu.Enabled = True
        'End If

        If Not IsNothing(Session("strTRUEUserName")) Then
            'Temporarily comment this out
            'If Session("strTRUEUserName") = "SSCOTT" Or Session("strTRUEUserName") = "SS185226" Or Session("strTRUEUserName") = "HL180002" Or Session("strTRUEUserName") = "KB185024" Then
            Me.ibtTeradata.Enabled = True
            'End If

            'Temporarily comment this out
            'If UCase(Session("strTRUEUserName")) = "SS185226" Or UCase(Session("strTRUEUserName")) = "SSCOTT" Then

            Dim mnuNavMenu As Menu = Me.NavigationMenu

            Dim mnuitmDeveloperQuery As New MenuItem
            mnuitmDeveloperQuery.NavigateUrl = "~/DeveloperQuery.aspx"
            mnuitmDeveloperQuery.Text = "Developer Query"
            mnuNavMenu.Items.Add(mnuitmDeveloperQuery)

            Dim mnuitmNewLink1 As New MenuItem
            mnuitmNewLink1.NavigateUrl = "~/ResourceList.aspx"
            mnuitmNewLink1.Text = "Resource"
            mnuNavMenu.Items.Add(mnuitmNewLink1)

            Dim mnuitmNewLink2 As New MenuItem
            mnuitmNewLink2.NavigateUrl = "~/Developer2.aspx"
            mnuitmNewLink2.Text = "Developer-2"
            mnuNavMenu.Items.Add(mnuitmNewLink2)

            Dim mnuitmNewLink3 As New MenuItem
            mnuitmNewLink3.NavigateUrl = "~/Developer3.aspx"
            mnuitmNewLink3.Text = "Developer-3"
            mnuNavMenu.Items.Add(mnuitmNewLink3)

            'End If
        End If

    End Sub

    Protected Sub ibtTeradata_Click(sender As Object, e As ImageClickEventArgs) Handles ibtTeradata.Click
        If Not IsNothing(Session("strTRUEUserName")) Then
            If Session("strTRUEUserName") = "SSCOTT" Or Session("strTRUEUserName") = "SS185226" Or Session("strTRUEUserName") = "HL180002" Or Session("strTRUEUserName") = "KB185024" Then
                Response.Redirect("~/TestPage.aspx")
            End If
        End If

    End Sub
End Class

