Imports System.Text

Public Class xss_encoder

    Public Shared Function ConvertStringToHex(ByVal input As [String], ByVal encoding As System.Text.Encoding) As String
        On Error Resume Next
        Dim stringBytes As [Byte]() = encoding.GetBytes(input)
        Dim sbBytes As New StringBuilder(stringBytes.Length * 2)
        For Each b As Byte In stringBytes
            sbBytes.AppendFormat("{0:X2}", b)
        Next
        Return sbBytes.ToString()
    End Function

    Public Shared Function ConvertHexToString(ByVal hexInput As [String], ByVal encoding As System.Text.Encoding) As String
        On Error Resume Next
        Dim numberChars As Integer = hexInput.Length
        Dim bytes As Byte() = New Byte(numberChars \ 2 - 1) {}
        For i As Integer = 0 To numberChars - 1 Step 2
            bytes(i \ 2) = Convert.ToByte(hexInput.Substring(i, 2), 16)
        Next
        Return encoding.GetString(bytes)
    End Function

    Private Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WebBrowser1.AllowWebBrowserDrop = False
        WebBrowser1.ScriptErrorsSuppressed = True
        WebBrowser1.DocumentText = p.Text
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted

    End Sub

    Private Sub p_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles p.TextChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub base64_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        If TextBox1.Text <> " " Then
            TextBox2.Text = Uri.EscapeDataString(TextBox1.Text)
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        On Error Resume Next
        If TextBox2.Text <> " " Then
            TextBox1.Text = Uri.UnescapeDataString(TextBox2.Text)
        End If
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error Resume Next
        TextBox4.Text = ConvertStringToHex(TextBox3.Text, System.Text.Encoding.ASCII)
        Dim hexx As String = ""
        Dim parts(TextBox4.Text.Length \ 2 - 1) As String
        For x As Integer = 0 To TextBox4.Text.Length - 1 Step 2
            parts(x \ 2) = TextBox4.Text.Substring(x, 2)
        Next


        For value As Integer = 0 To parts.Length

            'If (value > 3) Then
            '    Exit For
            'End If

            hexx = hexx + "\x" + parts(value)
        Next
        TextBox5.Text = hexx
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim tmp As String
        TextBox3.Text = ConvertHexToString(TextBox4.Text, System.Text.Encoding.ASCII)
        tmp = TextBox5.Text.Replace("\x", "")
        TextBox3.Text = ConvertHexToString(tmp, System.Text.Encoding.ASCII)
    End Sub
End Class