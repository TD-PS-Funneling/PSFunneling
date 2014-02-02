Option Explicit On

Imports System.Data




Partial Class ReportSummary
    Inherits System.Web.UI.Page


    Dim appService As New FunnelAppSvc
    Dim dtReportSummaryPhase As DataTable = appService.GetReportSummaryPhase.Tables(0)
    Dim dtReportSummaryAccountTeam As DataTable = appService.GetReportSummaryAccountTeam.Tables(0)
    Dim dtReportSummaryArea As DataTable = appService.GetReportSummaryArea.Tables(0)
    Dim dtReportSummaryWinPercent As DataTable = appService.GetReportSummaryWinPercent.Tables(0)



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Me.lblTitle.Text = Session("strUserName").ToString
        BindData()

    End Sub



    Private Sub BindData()
        Dim dvReportSummaryPhase As New DataView(dtReportSummaryPhase)
        Me.gvPhase.DataSource = dvReportSummaryPhase
        Me.gvPhase.DataBind()

        Dim dvReportSummaryAccountTeam As New DataView(dtReportSummaryAccountTeam)
        Me.gvAccount.DataSource = dvReportSummaryAccountTeam
        Me.gvAccount.DataBind()

        Dim dvReportSummaryArea As New DataView(dtReportSummaryArea)
        Me.gvArea.DataSource = dvReportSummaryArea
        Me.gvArea.DataBind()

        Dim dvReportSummaryWinPercent As New DataView(dtReportSummaryWinPercent)
        Me.gvWinPercent.DataSource = dvReportSummaryWinPercent
        Me.gvWinPercent.DataBind()

    End Sub

    Protected Sub gvPhase_DataBound(sender As Object, e As EventArgs) Handles gvPhase.DataBound
        gvPhase.FooterRow.Cells(0).ForeColor = Drawing.Color.Black
        gvPhase.FooterRow.Cells(0).Font.Bold = True
        gvPhase.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Center
        gvPhase.FooterRow.Cells(0).Text = "Total"
        gvPhase.FooterRow.Cells(1).Text = String.Format("{0:c}", dtReportSummaryPhase.Compute("sum(ps_value)", ""))
    End Sub
    Protected Sub gvAccount_DataBound(sender As Object, e As EventArgs) Handles gvAccount.DataBound
        gvAccount.FooterRow.Cells(0).ForeColor = Drawing.Color.Black
        gvAccount.FooterRow.Cells(0).Font.Bold = True
        gvAccount.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Center
        gvAccount.FooterRow.Cells(0).Text = "Total"
        gvAccount.FooterRow.Cells(1).Text = String.Format("{0:c}", dtReportSummaryAccountTeam.Compute("sum(ps_value)", ""))
    End Sub

    Protected Sub gvArea_DataBound(sender As Object, e As EventArgs) Handles gvArea.DataBound
        gvArea.FooterRow.Cells(0).ForeColor = Drawing.Color.Black
        gvArea.FooterRow.Cells(0).Font.Bold = True
        gvArea.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Center
        gvArea.FooterRow.Cells(0).Text = "Total"
        gvArea.FooterRow.Cells(1).Text = String.Format("{0:c}", dtReportSummaryArea.Compute("sum(ps_value)", ""))
    End Sub
    Protected Sub gvWinPercent_DataBound(sender As Object, e As EventArgs) Handles gvWinPercent.DataBound
        gvWinPercent.FooterRow.Cells(0).ForeColor = Drawing.Color.Black
        gvWinPercent.FooterRow.Cells(0).Font.Bold = True
        gvWinPercent.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Center
        gvWinPercent.FooterRow.Cells(0).Text = "Total"
        gvWinPercent.FooterRow.Cells(1).Text = String.Format("{0:c}", dtReportSummaryWinPercent.Compute("sum(ps_value)", ""))
    End Sub
End Class
