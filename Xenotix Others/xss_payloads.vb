Public Class xss_payloads

    Private Sub xss_payloads_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.ShowItemToolTips = True
        ListView1.View = View.Details
        ' Set column header
        ListView1.Columns.Clear()
        ListView1.Columns.Add("No", 50)
        ListView1.Columns.Add("XSS Payload", 800)
        'Load Payloads


        Dim i As Integer = xenotix_main.RichTextBox1.Lines.Length
        Me.Text = "Xenotix Injection Payloads - " & i
        Dim j As Integer = 0
        Dim TempStr(1) As String
        Dim TempNode As ListViewItem

        While j < i
            TempStr(0) = j + 1
            TempStr(1) = xenotix_main.RichTextBox1.Lines(j)
            TempNode = New ListViewItem(TempStr)
            ListView1.Items.Add(TempNode)
            j = j + 1

        End While

    End Sub

    Private Sub xss_payloads_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ListView1.Height = Me.Height - 50
        ListView1.Width = Me.Width - 30
    End Sub
End Class