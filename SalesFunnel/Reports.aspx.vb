
Partial Class Reports
    Inherits System.Web.UI.Page

    Protected Sub btnReportSummary_Click(sender As Object, e As EventArgs) Handles btnReportSummary.Click
        Response.Redirect("~/ReportSummary.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Form.Target = "_blank"
    End Sub

End Class
