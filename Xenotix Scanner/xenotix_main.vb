Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports CefSharp.WinForms
Imports CefSharp
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Text.RegularExpressions

Public Class xenotix_main

    Public webkit As WebView
    Dim index As Integer

    Public config_set As Integer = 0
    Public webkit_s As String

    Private Sub xenotix_main_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        On Error Resume Next
        Shell("taskkill /F /IM QuickPHP.exe")
        IO.File.Delete("xss.js")
        IO.File.Delete("xook.js")
        IO.File.Delete("index.html")
        Close()
        Dispose()
        End

    End Sub

    Private Sub xenotix_main_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed


    End Sub

    Private Sub xenotix_main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim content(final.Items.Count - 1) As String
        final.Items.CopyTo(content, 0)
        IO.File.WriteAllLines(Application.StartupPath & "\\scanhistory.txt", content)
    End Sub

    Public Shared Function ValidateRemoteCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        On Error Resume Next
        Dim aes_pass As String = "Xen0tix123"
        ''''''''''''''''''''''
        If My.Settings.first_run = True Then
            xenotix_whatsnew.Show()
        End If
        Shell("taskkill /F /IM QuickPHP.exe")
        IO.File.Delete("xss.js")
        IO.File.Delete("xook.js")
        IO.File.Delete("index.html")

        Dim load() As String = IO.File.ReadAllLines(Application.StartupPath & "\\scanhistory.txt")
        Array.Reverse(load)
        final.Items.AddRange(load)

        Button3.Enabled = False

        web.ScriptErrorsSuppressed = True
        bar.Enabled = False
        web.Navigate(Application.StartupPath & "\welcome\ie.html")
        webkit = New WebView(Application.StartupPath & "\welcome\chrome.html", New BrowserSettings())
        gecko.Navigate(Application.StartupPath & "\welcome\firefox.html")
        Me.Controls.Add(webkit)
        webkit.CreateControl()
        webkit.Location = New Point(390, 132)
        webkit.Width = 390
        webkit.Height = 512
        'SSL Error Fix
        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
        'Global Browser Proxy System Wide
        If My.Settings.proxy_enabled = True Then
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyServer", My.Settings.proxy_host & ":" & My.Settings.proxy_port, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", "1", Microsoft.Win32.RegistryValueKind.DWord)

        Else
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", "0", Microsoft.Win32.RegistryValueKind.DWord)
        End If
        'load and decrypt payload
        Dim aes As AES = New AES
        Dim payloads_encrypted As String = IO.File.ReadAllText(Application.StartupPath & "\\payloads_db")
        Dim raw_payloads As String = aes.AES_Decrypt(payloads_encrypted, aes_pass)
        RichTextBox1.Text = raw_payloads

        'mutate payload
        Dim match As Match
        For Each line In RichTextBox1.Lines
            If line.StartsWith("<") Then
                RichTextBox1.Text = RichTextBox1.Text.Replace(line, line + Environment.NewLine + """>" & line + Environment.NewLine + "'>" & line)
                match = Regex.Match(line, "^<[script]+>", RegexOptions.IgnoreCase)
                If match.Success Then
                    RichTextBox1.Text = RichTextBox1.Text.Replace(line, line + Environment.NewLine + "</ScrIpt>" & line)
                End If
            End If

        Next
        payloadcounter.Text = RichTextBox1.Lines.Length

    End Sub
    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click

        On Error Resume Next
        If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
            web.Refresh()
            webkit.Reload()
            gecko.Reload()

        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = False Then
            web.Refresh()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
            web.Refresh()

        ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
            web.Refresh()
            webkit.Reload()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
            web.Refresh()

            gecko.Reload()
        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then

            webkit.Reload()

        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then

            webkit.Reload()
            gecko.Reload()
        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then

            gecko.Reload()
        End If


    End Sub

    Private Sub ToolStripStatusLabel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next
        stat.Text = web.StatusText
        If web.IsBusy Then
            bar.Enabled = 1
            bar.Visible = 1
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        On Error Resume Next
        If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
            web.GoBack()
            webkit.Back()
            gecko.GoBack()

        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = False Then
            web.GoBack()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
            web.GoBack()

        ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
            web.GoBack()
            webkit.Back()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
            web.GoBack()

            gecko.GoBack()
        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then

            webkit.Back()

        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then

            webkit.Back()
            gecko.GoBack()
        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then

            gecko.GoBack()
        End If

    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click

        On Error Resume Next
        If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
            web.GoForward()
            webkit.Forward()
            gecko.GoForward()

        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = False Then
            web.GoForward()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
            web.GoForward()

        ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
            web.GoForward()
            webkit.Forward()

        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
            web.GoForward()

            gecko.GoForward()
        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then

            webkit.Forward()

        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then

            webkit.Forward()
            gecko.GoForward()
        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then

            gecko.GoForward()
        End If

    End Sub



    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles web.DocumentCompleted
        On Error Resume Next
        bar.Enabled = 0
        bar.Visible = 0


    End Sub

    Private Sub WebBrowser1_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles web.Navigated
        On Error Resume Next
        If (e.Url.ToString.Contains("http") Or e.Url.ToString.Contains("https")) Then

            bar.Enabled = 1
            bar.Visible = 1
        Else
            final.Text = ""
        End If


    End Sub





    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error Resume Next
        'SSL Error Fix
        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
        If final.Text = "" Or Not (final.Text.Contains("http://") Or final.Text.Contains("https://")) Then
            MsgBox("Please provide a valid URL", vbInformation)

        Else
            If (Not final.Items.Contains(final.Text)) Then
                final.Items.Add(final.Text)
            End If

            If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
                web.Url = New Uri(final.Text)
                gecko.Navigate(final.Text)
                webkit.Load(final.Text)
            ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
                web.Url = New Uri(final.Text)
                webkit.Load(final.Text)
            ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
                web.Url = New Uri(final.Text)
            ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
                web.Url = New Uri(final.Text)
                gecko.Navigate(final.Text)
            ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then
                gecko.Navigate(final.Text)
            ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then
                webkit.Load(final.Text)
            ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then
                gecko.Navigate(final.Text)
                webkit.Load(final.Text)
            End If

        End If

    End Sub

    Private Sub EncodersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EncodersToolStripMenuItem.Click

    End Sub

    Private Sub XSSKeyloggerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XSSKeyloggerToolStripMenuItem.Click


    End Sub

    Private Sub inbuilt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub XSSFileDrivebyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ToolStripTextBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub XSSReverseShellToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub






    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        xss_fuzzer_get.Show()

    End Sub

    Private Sub statusbar_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles statusbar.ItemClicked

    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub ToolStripStatusLabel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub ToolStripMenuItem3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_javascript.Show()
        End If
    End Sub

    Private Sub xenotix_main_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin

    End Sub

    Private Sub source_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub webkit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Function xss_javascript_injector() As Object
        Throw New NotImplementedException
    End Function


    Private Sub KeyloggerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KeyloggerToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_keylogger.Show()
        End If
    End Sub





    Private Sub DDoSerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDoSerToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ddos.Show()
        End If
    End Sub

    Private Sub CookieThiefToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieThiefToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_cookie_thief.Show()
        End If
    End Sub


    Private Sub TridentIEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TridentIEToolStripMenuItem.Click
        On Error Resume Next
        iesource.TextBox1.Text = web.DocumentText
        iesource.Show()

    End Sub

    Private Sub GeckoFFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeckoFFToolStripMenuItem.Click
        On Error Resume Next

        gecko.ViewSource()
    End Sub

    Private Sub WebkitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebkitToolStripMenuItem.Click
        On Error Resume Next
        xss_wekit_source.TextBox1.Text = webkit.EvaluateScript("document.documentElement.outerHTML;").ToString()
        xss_wekit_source.Show()
    End Sub

    Private Sub AllEnginesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllEnginesToolStripMenuItem.Click
        On Error Resume Next
        iesource.TextBox1.Text = web.DocumentText
        iesource.Show()
        gecko.ViewSource()
        xss_wekit_source.TextBox1.Text = webkit.EvaluateScript("document.documentElement.outerHTML;").ToString()
        xss_wekit_source.Show()
    End Sub

    Private Sub ConfigureServerToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureServerToolStripMenuItem.Click
        On Error Resume Next
        xss_server.Show()
    End Sub






    Private Sub AlertMessageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlertMessageToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_message.Show()
        End If
    End Sub


    Private Sub GmailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
        End If
    End Sub

    Private Sub TwitterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
        End If
    End Sub

    Private Sub LinkedInToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
        End If
    End Sub

    Private Sub GooglePlusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
        End If
    End Sub

    Private Sub LivecomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
        End If
    End Sub

    Private Sub XSSFuzzerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XSSFuzzerToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_fuzzer_get.Show()
        End If
    End Sub

    Private Sub EncoderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EncoderToolStripMenuItem.Click
        On Error Resume Next
        xss_encoder.Show()
    End Sub

    Private Sub XenotixXSSExploitFrameworkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XenotixXSSExploitFrameworkToolStripMenuItem.Click
        On Error Resume Next
        xenotix_about.Show()
    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        On Error Resume Next
        final.Items.Clear()
        IO.File.Delete(Application.StartupPath & "\\scanhistory.txt")
        web.DocumentText = ""
        webkit_s = ""
        webkit.LoadHtml("<html></html>")
        gecko.LoadHtml("<html></html>")
    End Sub



    Private Sub gc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gc.CheckedChanged
        On Error Resume Next
        engsel.Enabled = True
        If webkit.Address.ToString.Contains("/welcome/chrome.html") Then
            web.DocumentText = ""
            webkit_s = ""
            webkit.LoadHtml("<html></html>")
            gecko.LoadHtml("<html></html>")
        End If
    End Sub



    Private Sub ie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ie.CheckedChanged
        On Error Resume Next
        engsel.Enabled = True
        If web.Url.ToString.Contains("/welcome/ie.html") Then
            web.DocumentText = ""
            webkit_s = ""
            webkit.LoadHtml("<html></html>")
            gecko.LoadHtml("<html></html>")
        End If
    End Sub

    Private Sub ff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ff.CheckedChanged
        On Error Resume Next
        engsel.Enabled = True
        If gecko.Url.ToString.Contains("/welcome/firefox.html") Then
            web.DocumentText = ""
            webkit_s = ""
            webkit.LoadHtml("<html></html>")
            gecko.LoadHtml("<html></html>")
        End If
    End Sub

    Private Sub engsel_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles engsel.Tick
        'TODO neatify width
        If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
            web.Location = New Point(0, 134)
            webkit.Location = New Point(390, 132)
            gecko.Location = New Point(782, 132)
            web.Width = 390
            webkit.Width = 390
            gecko.Width = 381

            web.Visible = True
            gecko.Visible = True
            webkit.Visible = True
        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = False Then
            web.Location = New Point(0, 134)
            ie.Checked = True
            web.Visible = True
            gecko.Visible = False
            webkit.Visible = False
            web.Width = 1163
        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
            web.Location = New Point(0, 134)
            web.Visible = True
            gecko.Visible = False
            webkit.Visible = False
            web.Width = 1163
        ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
            web.Location = New Point(0, 134)
            webkit.Location = New Point(579, 132)
            web.Width = 577
            webkit.Width = 574
            web.Visible = True
            webkit.Visible = True
            gecko.Visible = False
        ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
            web.Location = New Point(0, 134)
            gecko.Location = New Point(579, 132)
            web.Width = 577
            gecko.Width = 574
            web.Visible = True
            webkit.Visible = False
            gecko.Visible = True
        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then
            webkit.Location = New Point(0, 134)
            webkit.Width = 1163
            web.Visible = False
            gecko.Visible = False
            webkit.Visible = True
        ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then
            webkit.Location = New Point(0, 134)
            gecko.Location = New Point(579, 132)
            webkit.Width = 577
            gecko.Width = 574
            webkit.Visible = True
            gecko.Visible = True
            web.Visible = False
        ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then

            gecko.Location = New Point(0, 134)
            gecko.Width = 1163
            web.Visible = False
            gecko.Visible = True
            webkit.Visible = False

        End If

    End Sub

    Private Sub CodeInspectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        xss_waf_detection.Show()
    End Sub


    Private Sub WebKitDeveloperToolsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebKitDeveloperToolsToolStripMenuItem.Click
        On Error Resume Next
        webkit.ShowDevTools()

    End Sub



    Private Sub POSTScannerToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles POSTScannerToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_fuzzer_post.Show()
        End If
    End Sub

    Private Sub ImportPayloadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportPayloadToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            OpenFileDialog1.ShowDialog()
        Else
            MsgBox("Stop the server and import the payloads", vbCritical)

        End If
    End Sub

    Private Sub HiddenParameterScannerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ProxySettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProxySettingsToolStripMenuItem.Click
        On Error Resume Next
        xenotix_gproxy.Show()
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        Try

            Dim tmp As String = RichTextBox1.Text
            Dim txtreader As New System.IO.StreamReader(OpenFileDialog1.FileName)
            RichTextBox1.Text = txtreader.ReadToEnd + tmp
            'Sync Payloads with Server
            payloadcounter.Text = RichTextBox1.Lines.Length
        Catch ex As Exception
            MsgBox(ex.Message.ToString, vbCritical)
        End Try
    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub HeaderInjectorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HeaderInjectorToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_fuzzer_request.Show()
        End If
    End Sub



    Private Sub ViewInjectedJavascriptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewInjectedJavascriptToolStripMenuItem.Click
        On Error Resume Next
        If File.Exists("xss.js") = True Then
            Process.Start("notepad.exe", "xss.js")

        End If
    End Sub



    Private Sub XssedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XssedToolStripMenuItem.Click
        Process.Start("http://www.xssed.com/")
    End Sub



    Private Sub XssVextorTwitterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XssVextorTwitterToolStripMenuItem.Click
        Process.Start("https://twitter.com/xenotix")
    End Sub

    Private Sub HttpshazzercoukvectorsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HttpshazzercoukvectorsToolStripMenuItem.Click
        Process.Start("http://shazzer.co.uk/vectors")
    End Sub


    Private Sub PacketStormToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PacketStormToolStripMenuItem.Click
        Process.Start("http://packetstormsecurity.com/news/tags/xss/")
    End Sub

    Private Sub OWASPProjectPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OWASPProjectPageToolStripMenuItem.Click
        Process.Start("https://www.owasp.org/index.php/OWASP_Xenotix_XSS_Exploit_Framework")
    End Sub


    Private Sub HackvectorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HackvectorToolStripMenuItem.Click
        Process.Start("https://hackvertor.co.uk/public")
    End Sub

    Private Sub OWASPXSSCheetSheetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OWASPXSSCheetSheetToolStripMenuItem.Click
        Process.Start("https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet")
    End Sub

    Private Sub DOMXSSCheatSheetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DOMXSSCheatSheetToolStripMenuItem.Click
        Process.Start("https://www.owasp.org/index.php/DOM_based_XSS_Prevention_Cheat_Sheet")
    End Sub

    Private Sub HiddenPArameterScannerToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HiddenPArameterScannerToolStripMenuItem1.Click
        On Error Resume Next
        xss_hidden_params.Show()
    End Sub


    Private Sub JavaScriptButifierToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JavaScriptButifierToolStripMenuItem.Click
        On Error Resume Next
        xenotix_jsbeautifier.Show()
    End Sub


    Private Sub TutorialsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TutorialsToolStripMenuItem.Click
        On Error Resume Next
        System.Diagnostics.Process.Start("https://www.youtube.com/playlist?list=PLX3EwmWe0cS80ls3TsNiukQD0hfZjLHnP")
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub WAFFingerprintingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WAFFingerprintingToolStripMenuItem.Click
        On Error Resume Next
        xss_waf_detection.Show()
    End Sub

    Private Sub LoadPDFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadPDFToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_load_file.Show()
        End If
    End Sub



    Private Sub HashDetectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HashDetectionToolStripMenuItem.Click
        On Error Resume Next
        Process.Start("hash_detection.exe")
    End Sub

    Private Sub HashCalcuLtorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HashCalcuLtorToolStripMenuItem.Click
        On Error Resume Next
        xss_hashcalculator.Show()
    End Sub




    Private Sub GrabPageScreenshotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrabPageScreenshotToolStripMenuItem.Click
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_screenshot_page.Show()
        End If
    End Sub


    Private Sub ScriptingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptingToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            System.Diagnostics.Process.Start(Application.StartupPath & "\\scripting_engine\\Xenotix Python Scripting Engine.exe")
        End If
    End Sub


    Private Sub ViewXSSPayloadsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewXSSPayloadsToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_payloads.Show()
        End If
    End Sub

    Private Sub final_KeyDown(sender As Object, e As KeyEventArgs) Handles final.KeyDown
        If e.KeyData = Keys.Enter Then
            If final.Text = "" Or Not (final.Text.Contains("http://") Or final.Text.Contains("https://")) Then
                MsgBox("Please provide a valid URL", vbInformation)

            Else
                If (Not final.Items.Contains(final.Text)) Then
                    final.Items.Add(final.Text)
                End If
                If ie.Checked = True And gc.Checked = True And ff.Checked = True Then
                    web.Url = New Uri(final.Text)
                    gecko.Navigate(final.Text)
                    webkit.Load(final.Text)
                ElseIf ie.Checked = True And gc.Checked = True And ff.Checked = False Then
                    web.Url = New Uri(final.Text)
                    webkit.Load(final.Text)
                ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = False Then
                    web.Url = New Uri(final.Text)
                ElseIf ie.Checked = True And gc.Checked = False And ff.Checked = True Then
                    web.Url = New Uri(final.Text)
                    gecko.Navigate(final.Text)
                ElseIf ie.Checked = False And gc.Checked = False And ff.Checked = True Then
                    gecko.Navigate(final.Text)
                ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = False Then
                    webkit.Load(final.Text)
                ElseIf ie.Checked = False And gc.Checked = True And ff.Checked = True Then
                    gecko.Navigate(final.Text)
                    webkit.Load(final.Text)
                End If

            End If


        End If

    End Sub



    Private Sub FeaturesdToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FeaturesdToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_pingscan.Show()
        End If
    End Sub

    Private Sub BrowserFingerprintingToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BrowserFingerprintingToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_browser_fing.Show()
        End If
    End Sub

    Private Sub FeaturesDetectorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FeaturesDetectorToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_browser_detector.Show()
        End If
    End Sub

    Private Sub FingerprintingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FingerprintingToolStripMenuItem.Click
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_networkip.Show()
        End If
    End Sub

    Private Sub PortScanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PortScanToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_portscan.Show()
        End If
    End Sub

    Private Sub InternalNetworkScanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InternalNetworkScanToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_internal_network.Show()
        End If
    End Sub

    Private Sub NetworkConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub PhisherToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PhisherToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_phish.url.Text = "http://www.facebook.com"
            xss_phish.Show()
        End If
    End Sub



    Private Sub TabnabbingToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TabnabbingToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_tabnabbing.Show()
        End If
    End Sub

    Private Sub ExecutableDriveByWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExecutableDriveByWindowsToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_java_driveby.Show()
        End If
    End Sub

    Private Sub WebCamScreenshotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WebCamScreenshotToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_live_webcam_screenshot.Show()
        End If
    End Sub

    Private Sub FirefoxAddonsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FirefoxAddonsToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_ddoser.Show()
        End If
    End Sub

    Private Sub KeyloggerToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles KeyloggerToolStripMenuItem2.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_keylogger.Show()
        End If
    End Sub

    Private Sub SessionStealerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SessionStealerToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_window_ssessionstealer.Show()
        End If
    End Sub

    Private Sub LinuxCredentialToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LinuxCredentialToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_linux.Show()
        End If
    End Sub

    Private Sub DropAndExecuteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DropAndExecuteToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_downloader.Show()
        End If
    End Sub

    Private Sub FirefoxAddonToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FirefoxAddonToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_reverse_shell_windows.Show()
        End If
    End Sub

    Private Sub FirefoxAddonReverseShellLinuxToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FirefoxAddonReverseShellLinuxToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_ff_reverse_shell_linux.Show()
        End If
    End Sub

    Private Sub DriToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DriToolStripMenuItem.Click

        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_java_reverse_shell.Show()
        End If
    End Sub

    Private Sub ShellAccessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShellAccessToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_javascript.Show()
        End If
    End Sub



    Private Sub ReverseHTTPWebShellToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ReverseHTTPWebShellToolStripMenuItem1.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_reversehttpwebshell.Show()
        End If
    End Sub

    Private Sub MetasploiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MetasploiToolStripMenuItem.Click
        On Error Resume Next

        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_metasploit_exploit.Show()
        End If
    End Sub


    Private Sub GelocationHTML5APIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GelocationHTML5APIToolStripMenuItem.Click
        On Error Resume Next

        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_geolocation.Show()
        End If
    End Sub

    Private Sub DownloadSpooferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadSpooferToolStripMenuItem.Click
        On Error Resume Next

        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_download_spoofer.Show()
        End If
    End Sub

    Private Sub final_SelectedIndexChanged(sender As Object, e As EventArgs) Handles final.SelectedIndexChanged

    End Sub

    Private Sub final_TextChanged(sender As Object, e As EventArgs) Handles final.TextChanged
        Button3.Enabled = True
    End Sub

    Private Sub LocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LocationToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_ip2location.Show()
        End If
    End Sub

    Private Sub IPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IPToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_ip2geolocation.Show()
        End If
    End Sub

    Private Sub ToolStripMenuItem3_Click_2(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_hta_reverseshell.Show()
        End If
    End Sub


    Private Sub HTADriveByWindowsIEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HTADriveByWindowsIEToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_hta_driveby.Show()
        End If
    End Sub

    Private Sub OAuth10aRequestScannerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OAuth10aRequestScannerToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_oauth_request.Show()
        End If
    End Sub

    Private Sub HTANetworkConfigurationWindowsIEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HTANetworkConfigurationWindowsIEToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_hta_network_adapter.Show()
        End If
    End Sub

    Private Sub XSSProtectionCheatSheetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XSSProtectionCheatSheetToolStripMenuItem.Click
        Process.Start("http://opensecurity.in/the-ultimate-xss-protection-cheat-sheet-for-developers/")
    End Sub

    Private Sub JSFuck6CharEncoderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles JSFuck6CharEncoderToolStripMenuItem1.Click
        On Error Resume Next
        xss_encoder_jsfuck.Show()
    End Sub

    Private Sub JjencodeEncoderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles JjencodeEncoderToolStripMenuItem1.Click
        On Error Resume Next
        xss_encoder_jjencode.Show()
    End Sub

    Private Sub AaencodeEncoderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AaencodeEncoderToolStripMenuItem.Click
        On Error Resume Next
        xss_encoder_aaencode.Show()
    End Sub

    Private Sub OTHERSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OTHERSToolStripMenuItem.Click

    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs)
        xss_underdev_websocketfuzzer.Show()

    End Sub

    Private Sub DOMXSSAnalyzerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DOMXSSAnalyzerToolStripMenuItem.Click
        On Error Resume Next

        xss_dom_scanner.Show()

    End Sub

    Private Sub LocalDOMXSSAnalyzerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LocalDOMXSSAnalyzerToolStripMenuItem.Click
        On Error Resume Next

        xss_dom_scanner_local.Show()
    End Sub

    Private Sub MSFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MSFToolStripMenuItem.Click

    End Sub

    Private Sub BrowserHistorySnifferHSTSCSPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BrowserHistorySnifferHSTSCSPToolStripMenuItem.Click
        On Error Resume Next
        If config_set = 0 Then
            MsgBox("Configure the Server First.", vbExclamation)
            xss_server.Show()
        Else
            xss_info_sites_visited.Show()
        End If


    End Sub
End Class
