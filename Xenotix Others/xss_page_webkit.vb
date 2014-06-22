Public Class xss_wekit_source

    Private Sub xss_wekit_source_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next
        TextBox1.Width = Me.Width - 35
        TextBox1.Height = Me.Height - 80
    End Sub
    Private TargetPosition As Integer

    Private Sub FindText(ByVal start_at As Integer)
        Dim pos As Integer
        Dim target As String

        target = TextBox2.Text
        pos = InStr(start_at, TextBox1.Text, target)
        If pos > 0 Then
            TargetPosition = pos
            TextBox1.SelectionStart = TargetPosition - 1
            TextBox1.SelectionLength = Len(target)
            TextBox1.Focus()
        Else
            If TargetPosition = 0 Then
                MsgBox("Not found", vbCritical)
            Else
                MsgBox("Search Completed!", vbInformation)

            End If
            TextBox1.Focus()
        End If
    End Sub

  

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TargetPosition = 0
        FindText(1)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FindText(TargetPosition + 1)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub xss_wekit_source_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        TextBox1.Width = Me.Width - 35
        TextBox1.Height = Me.Height - 80
    End Sub
End Class