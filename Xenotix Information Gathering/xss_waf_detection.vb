

Public Class xss_waf_detection

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next
        If IO.File.Exists(Application.StartupPath & "\waf_detection_res.txt") Then
            Dim waf_dec_res As String = IO.File.ReadAllText(Application.StartupPath & "\waf_detection_res.txt")
                text_op.Text = waf_dec_res
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not (TextBox1.Text.Contains("http://") Or TextBox1.Text.Contains("https://")) Or TextBox1.Text = "" Then
            MsgBox("Invalid URL!", vbCritical)
        Else
            IO.File.Delete("waf_detection_res.txt")
            Dim para As String
            para = " " & TextBox1.Text
            Process.Start("waf_detection.exe", para)
            Timer1.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Shell("taskkill /F /IM waf_detection.exe")
        IO.File.Delete("waf_detection_res.txt")
        Timer1.Enabled = False
        Me.Close()
    End Sub

    Private Sub xss_waf_detection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Shell("taskkill /F /IM waf_detection.exe")
        IO.File.Delete("waf_detection_res.txt")
    End Sub
End Class