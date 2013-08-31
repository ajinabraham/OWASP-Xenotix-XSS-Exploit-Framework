Public Class Spash

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next

        xenotix_main.Show()

        Timer1.Enabled = False
        Me.Hide()

    End Sub

    Private Sub Spash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Shell("taskkill /F /IM QuickPHP.exe")
        Timer1.Enabled = True
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class