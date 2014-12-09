Public Class xenotix_whatsnew

    Private Sub xenotix_whatsnew_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        WebBrowser1.DocumentText = "<iframe width='910' height='482' src='http://www.youtube.com/embed/RhGVuus_NJw' frameborder='0' allowfullscreen></iframe>"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox1.Checked = True Then
            My.Settings.first_run = False
            My.Settings.Save()
        End If
        Me.Close()
    End Sub
End Class