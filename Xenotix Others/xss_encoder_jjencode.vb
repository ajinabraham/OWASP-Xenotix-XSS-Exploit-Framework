Imports CefSharp.WinForms
Imports CefSharp

Public Class xss_encoder_jjencode
    Public webkit As WebView
    Private Sub xss_encoder_jjencode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        webkit = New WebView(Application.StartupPath & "\components\jj.html", New BrowserSettings())
        Me.Controls.Add(webkit)
        webkit.CreateControl()
        webkit.Location = New Point(0, 0)
        webkit.Width = 828
        webkit.Height = 599
    End Sub
End Class