Public Class xss_encoder_jsfuck

    Private Sub xss_encoder_jsfuck_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gecko.LoadHtml(TextBox1.Text)
    End Sub
End Class