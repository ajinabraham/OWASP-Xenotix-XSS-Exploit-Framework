Imports System.IO

Public Class xss_dom_scanner_local

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Then
            MsgBox("Select a Folder", vbCritical)
        Else
            Button2.Enabled = False
            TextBox2.AppendText(Environment.NewLine + "Analysis Started")
            Dim p As Process = New Process
            p.StartInfo.FileName = "dom_xss_scanner_local.exe"
            Dim q = """"
            p.StartInfo.Arguments = " -d " + q + TextBox1.Text + q
            p.StartInfo.UseShellExecute = False
            p.StartInfo.RedirectStandardOutput = True
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            p.Start()
            Dim output As String = p.StandardOutput.ReadToEnd
            TextBox2.AppendText(Environment.NewLine + output)
            p.WaitForExit()

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        On Error Resume Next
        Shell("taskkill /F /IM dom_xss_scanner_local.exe")
        If Not TextBox1.Text = "" Then
            Button2.Enabled = True
        End If
    End Sub
End Class