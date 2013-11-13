Imports ColorCode
Imports System.Net
Imports System.IO

Public Class xenotix_jsbeautifier

    Private Sub xenotix_jsbeautifier_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Shell("taskkill /F /IM js_beautify.exe")
        IO.File.Delete("tmp.js")
        IO.File.Delete("beautified.js")
      
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If code_chk.Checked = True Then
            Try
                If js.Text = "" Then
                    MsgBox("Input the JavaScript to Beautify", vbCritical)
                Else

                    IO.File.WriteAllText(Application.StartupPath & "\tmp.js", js.Text)
                    Process.Start("js_beautify.exe")
                    Timer1.Enabled = True
                    TabControl1.SelectTab(1)
                End If
Catch ex As Exception
                MsgBox(ex.Message.ToString, vbCritical)
            End Try
        ElseIf url_check.Checked = True Then
            If (jsurl.Text.Contains("http://")) Or (jsurl.Text.Contains("https://")) Then

                Try
                    Dim request As WebRequest = WebRequest.Create(jsurl.Text)
                    If My.Settings.proxy_enabled = True Then

                        Try
                            Dim myProxy As New WebProxy()
                            Dim newUri As New Uri("http://" & My.Settings.proxy_host & ":" & My.Settings.proxy_port)
                            myProxy.Address = newUri
                            myProxy.Credentials = New NetworkCredential(My.Settings.proxy_us, My.Settings.proxy_ps)
                            request.Proxy = myProxy
                        Catch ex As Exception
                            ' MsgBox(ex.Message.ToString, vbCritical)
                        End Try

                    End If
                    request.Method = "GET"
                    DirectCast(request, System.Net.HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.38) Gecko/20101203 Firefox/3.6.13 (.NET CLR 3.5.26529)"
                    Dim response As WebResponse = request.GetResponse()
                    Dim dataStream As Stream = response.GetResponseStream()
                    Dim reader As New StreamReader(dataStream)
                    Dim responseFromServer As String = reader.ReadToEnd()
                    IO.File.WriteAllText(Application.StartupPath & "\tmp.js", responseFromServer)
                    Process.Start("js_beautify.exe")
                    Timer1.Enabled = True
                    TabControl1.SelectTab(1)
                Catch ex2 As Exception
                    MsgBox(ex2.Message.ToString, vbCritical)
                End Try

            Else
                MsgBox("Invalid URL", vbCritical)
            End If
        End If
    End Sub

 

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next
        If IO.File.Exists(Application.StartupPath & "\beautified.js") Then
            Dim js_beauty As String = IO.File.ReadAllText(Application.StartupPath & "\beautified.js")
            ''coloring
            Dim colorizer As ColorCode.CodeColorizer = New ColorCode.CodeColorizer()
            WebBrowser1.DocumentText = colorizer.Colorize(js_beauty, Languages.JavaScript)
            Timer1.Enabled = False
            IO.File.Delete("beautified.js")
            Shell("taskkill /F /IM js_beautify.exe")
        End If
    End Sub

    
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Shell("taskkill /F /IM js_beautify.exe")
        IO.File.Delete("tmp.js")
        IO.File.Delete("beautified.js")
        Me.Close()
    End Sub
End Class