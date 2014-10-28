Imports System.IO

Public Class xss_dom_scanner

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Shell("taskkill /F /IM dom_scanner.exe")
        IO.File.Delete("res.txt")
        Me.Close()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("Enter a valid URL", vbCritical)
        Else
            Button1.Enabled = False
            TextBox2.Text = ""
            Dim para As String = ""
            Dim agent As String
            If Not TextBox1.Text = "" And Not cook.Text = "" And Not prox.Text = "" Then
                If usr.Text = "" Then
                    agent = " --random-agent"
                Else
                    agent = " --user-agent " & usr.Text
                End If
                para = " -u " & TextBox1.Text & " --dom" & agent & " --cookie " & cook.Text & " --http-proxy " & prox.Text
            ElseIf Not TextBox1.Text = "" And Not cook.Text = "" And prox.Text = "" Then
                If usr.Text = "" Then
                    agent = " --random-agent"
                Else
                    agent = " --user-agent " & usr.Text
                End If
                para = " -u " & TextBox1.Text & " --dom --cookie " & cook.Text & agent

            ElseIf Not TextBox1.Text = "" And cook.Text = "" And Not prox.Text = "" Then
                If usr.Text = "" Then
                    agent = " --random-agent"
                Else
                    agent = " --user-agent " & usr.Text
                End If
                para = " -u " & TextBox1.Text & " --dom --http-proxy " & prox.Text & agent

            ElseIf Not TextBox1.Text = "" And cook.Text = "" And prox.Text = "" Then
                If usr.Text = "" Then
                    agent = " --random-agent"
                Else
                    agent = " --user-agent " & usr.Text
                End If
                para = " -u " & TextBox1.Text & " --dom" & agent


            End If

            Process.Start("dom_scanner.exe", para)
            Timer1.Enabled = True
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Dim q = """"
            Dim line As String
            Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\res.txt")
            line = readfile.ReadToEnd()
            TextBox2.Text = line
            readfile.Close()
            readfile = Nothing

        Catch

        End Try
    End Sub

    Private Sub xss_dom_scanner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        TextBox1.Focus()
        Shell("taskkill /F /IM dom_scanner.exe")
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("DOM XSS Scanner Module is Hacked from Gianluca Brindisi's xsssniper", vbInformation)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error Resume Next
        Shell("taskkill /F /IM dom_scanner.exe")
        IO.File.Delete("res.txt")
        Button1.Enabled = True
    End Sub


End Class