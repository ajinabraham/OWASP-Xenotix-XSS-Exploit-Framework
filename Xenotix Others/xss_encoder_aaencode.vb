Imports CefSharp.WinForms
Imports CefSharp

Public Class xss_encoder_aaencode
    Public webkit As WebView
    Private Sub xss_encoder_aaencode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        webkit = New WebView(Application.StartupPath & "\components\index.html", New BrowserSettings())
        Me.Controls.Add(webkit)
        webkit.CreateControl()
        webkit.Location = New Point(0, 0)
        webkit.Width = 833
        webkit.Height = 599
    End Sub
End Class