Imports HtmlAgilityPack.HtmlWeb
Imports System.Net

Public Class xss_hidden_params
   
    Private Sub xss_hidden_params_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        WebBrowser1.ScriptErrorsSuppressed = True
        Button1.Enabled = False
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        ListBox1.Items.Clear()
        Dim doc As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
        doc.Load(WebBrowser1.DocumentStream)




        Dim hid As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//input")
        Dim q As String = """"
        Dim flag As Integer = 0
        For c As Integer = 0 To hid.Count - 1
            If hid.Item(c).OuterHtml.Contains("type=" & q & "hidden" & q) Then
                ListBox1.Items.Add(hid.Item(c).OuterHtml)
                flag = 1
            End If
        Next
        If flag = 1 Then
            MsgBox("Scanning Finished.", vbInformation)
        Else
        End If

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Me.Close()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error Resume Next
        If TextBox1.Text.Contains("http://") Or TextBox1.Text.Contains("https://") Then
            WebBrowser1.Url = New Uri(TextBox1.Text)
            Button1.Enabled = True
        Else
            MsgBox("Invalid URL", vbCritical)
        End If


    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        On Error Resume Next
        WebBrowser1.Refresh()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        On Error Resume Next
        WebBrowser1.GoForward()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        On Error Resume Next
        WebBrowser1.GoBack()
    End Sub

    Private Sub WebBrowser1_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        On Error Resume Next
        If (e.Url.ToString.Contains("http") Or e.Url.ToString.Contains("https")) Then
            TextBox1.Text = e.Url.ToString
           
        Else
            TextBox1.Text = ""
        End If

    End Sub
End Class