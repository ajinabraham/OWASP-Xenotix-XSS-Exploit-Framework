Imports System.IO

Public Class xss_underdev_gecko_report

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ListBox1.Items.Clear()
        Try
            For Each line As String In File.ReadLines(Application.StartupPath & "\report.txt")
                ListBox1.Items.Add(line)
            Next



        Catch

        End Try



    End Sub

    Private Sub xss_gecko_report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub xss_gecko_report_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ListBox1.Height = Me.Height - 50
        ListBox1.Width = Me.Width - 35
    End Sub
End Class