Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Text
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class xss_oauth_request
    Dim ourl, oparams As String
    Public flag As Integer
    Dim i As Integer = 0
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim header As String = InputBox("Enter the Header Name and Value", "Add Header")
        If (header.Contains(":") And header.Length > 2) Then
            headers.Items.Add(header)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If headers.SelectedIndex >= 0 Then
            Dim eheader As String = InputBox("Enter the Header", "Edit Header")
            If (eheader.Contains(":") And eheader.Length > 2) Then
                headers.Items(headers.SelectedIndex) = eheader
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If headers.SelectedIndex >= 0 Then
            headers.Items.RemoveAt(headers.SelectedIndex)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        headers.Items.Clear()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim namevalue As String = InputBox("Enter the Cookie Name and Value", "Add Cookie")
        If (namevalue.Contains(":") And namevalue.Length > 2) Then
            cookies.Items.Add(namevalue)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If cookies.SelectedIndex >= 0 Then
            Dim cook As String = InputBox("Enter the Cookie", "Edit Cookie")
            If (cook.Contains(":") And cook.Length > 2) Then
                cookies.Items(cookies.SelectedIndex) = cook
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cookies.SelectedIndex >= 0 Then
            cookies.Items.RemoveAt(cookies.SelectedIndex)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        cookies.Items.Clear()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim html As String = ""

        If urlz.Text = "" Or method.Text = "" Or version.Text = "" Or signaturemethod.Text = "" Or csecret.Text = "" Or ckey.Text = "" Then
            MsgBox("Missing OAuth Fields", vbCritical)
        Else
            'OAuth in Header
            If (CheckBox2.Checked = True) Then
                html = oauth_auth_header.Text.Replace("XURL", urlz.Text).Replace("XPARAMS", parameters.Text).Replace("XMETHOD", method.Text).Replace("XVERSION", version.Text).Replace("XSIG_METHOD", signaturemethod.Text).Replace("XCONSUMER_KEY", ckey.Text).Replace("XCONSUMER_SECRET", csecret.Text).Replace("XOTOKEN", token.Text).Replace("XTOKEN_SECRET", tokensecret.Text)
                browserparams.DocumentText = html
                flag = 0
            Else
                'OAuth in URL
                html = TextBox1.Text.Replace("XURL", urlz.Text).Replace("XPARAMS", parameters.Text).Replace("XMETHOD", method.Text).Replace("XVERSION", version.Text).Replace("XSIG_METHOD", signaturemethod.Text).Replace("XCONSUMER_KEY", ckey.Text).Replace("XCONSUMER_SECRET", csecret.Text).Replace("XOTOKEN", token.Text).Replace("XTOKEN_SECRET", tokensecret.Text)
                browserparams.DocumentText = html
                flag = 1
            End If
        End If
    End Sub

    Public Shared Function ValidateRemoteCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Private Sub browserparams_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles browserparams.Navigated
        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        If (e.Url.ToString().Contains("http://")) Then
            Dim oauthparams As String = e.Url.AbsolutePath.Remove(0, 1)

            If (flag = 1) Then
                If (method.Text = "POST") Or (method.Text = "PUT") Then
                    Dim para() = parameters.Text.Split("&")
                    For Each p As String In para
                        oauthparams = oauthparams.Replace("&" + p, "")
                    Next

                End If
            Else
                oauthparams = oauthparams.Replace("%22", """").Replace("%20", " ")
            End If
            '  MsgBox(oauthparams)
            If method.Text.Contains("POST") Or method.Text.Contains("PUT") Then
                responseheaders.Text = ""

                RTB.Text = ""
                Try

                    Dim postReq As HttpWebRequest
                    If (flag = 1) Then
                        postReq = DirectCast(WebRequest.Create(urlz.Text + "?" + oauthparams), HttpWebRequest)
                    Else
                        postReq = DirectCast(WebRequest.Create(urlz.Text), HttpWebRequest)
                    End If
                    Dim encoding As New UTF8Encoding
                    Dim postData As String = parameters.Text
                    Dim byteData As Byte() = encoding.GetBytes(postData)
                    Dim tempCookies As New CookieContainer
                    'Proxy support
                    If My.Settings.proxy_enabled = True Then
                        Try
                            Dim myProxy As New WebProxy()
                            Dim newUri As New Uri("http://" & My.Settings.proxy_host & ":" & My.Settings.proxy_port)
                            myProxy.Address = newUri
                            myProxy.Credentials = New NetworkCredential(My.Settings.proxy_us, My.Settings.proxy_ps)
                            postReq.Proxy = myProxy
                        Catch ex As Exception
                            MsgBox(ex.Message.ToString, vbCritical)
                        End Try

                    End If
                    postReq.Method = method.Text
                    If (flag = 0) Then
                        postReq.Headers.Add("Authorization", oauthparams)

                    End If
                    postReq.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    postReq.KeepAlive = True
                    'ADD HEADERS
                    Dim cnt As Integer = 0
                    Dim posSep, hname, hvalue As String
                    While cnt < headers.Items.Count
                        posSep = headers.Items(cnt).IndexOf(":")
                        hname = headers.Items(cnt).Substring(0, posSep)
                        hvalue = headers.Items(cnt).Substring(posSep + 1)
                        If hname.Contains("User-Agent") Then
                            postReq.UserAgent = hvalue
                        ElseIf hname.Contains("Connection") Then
                            postReq.Connection = hvalue
                        ElseIf hname.Contains("Content-Type") Then
                            postReq.ContentType = hvalue
                        ElseIf hname.Equals("Accept") Then
                            postReq.Accept = hvalue
                        ElseIf hname.Contains("Referer") Then
                            postReq.Referer = hvalue
                        ElseIf hname.Contains("Expect") Then
                            postReq.Expect = hvalue
                        Else
                            postReq.Headers.Add(hname, hvalue)
                        End If
                        cnt = cnt + 1
                    End While
                    'Cookie
                    For Each cok As String In cookies.Items
                        posSep = cok.IndexOf(":")
                        hname = cok.Substring(0, posSep)
                        hvalue = cok.Substring(posSep + 1)
                        tempCookies.Add(New Uri(urlz.Text), New Cookie(hname, hvalue))
                    Next

                    postReq.CookieContainer = tempCookies
                    postReq.ContentLength = byteData.Length
                    Dim postreqstream As Stream = postReq.GetRequestStream()
                    postreqstream.Write(byteData, 0, byteData.Length)
                    postreqstream.Close()

                    '   RESPONSE


                    Dim myHttpWebResponse As HttpWebResponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
                    tempCookies.Add(myHttpWebResponse.Cookies)
                    Dim postreqreader As New StreamReader(myHttpWebResponse.GetResponseStream, encoding)


                    RTB.Text = postreqreader.ReadToEnd

                    responseheaders.Text = "HTTP/" + myHttpWebResponse.ProtocolVersion.ToString + " " + Str(myHttpWebResponse.StatusCode) + " " + myHttpWebResponse.StatusDescription.ToString + ControlChars.CrLf

                    Dim i As Integer
                    While i < myHttpWebResponse.Headers.Count
                        responseheaders.Text += myHttpWebResponse.Headers.Keys(i) + ":" + myHttpWebResponse.Headers(i) + ControlChars.CrLf
                        i = i + 1
                    End While

                    myHttpWebResponse.Close()
                Catch wbr As WebException
                    responseheaders.Text = wbr.Message.ToString
                    Using response As WebResponse = wbr.Response
                        Dim httpResponse As HttpWebResponse = DirectCast(response, HttpWebResponse)

                        Using data As Stream = response.GetResponseStream()
                            Using reader = New StreamReader(data)
                                Dim text As String = reader.ReadToEnd()

                                RTB.Text = text
                            End Using
                        End Using
                    End Using

                Catch xxx As Exception
                    responseheaders.Text = xxx.Message.ToString
                End Try

            ElseIf method.Text.Contains("GET") Or method.Text.Contains("DELETE") Then
                responseheaders.Text = ""

                RTB.Text = ""
                Try
                    Dim tempCookies As New CookieContainer
                    Dim postReq As HttpWebRequest
                    If (flag = 1) Then
                        postReq = CType(WebRequest.Create(urlz.Text + "?" + oauthparams), HttpWebRequest)
                    Else
                        If parameters.Text.Length > 0 Then
                            postReq = CType(WebRequest.Create(urlz.Text + "?" + parameters.Text), HttpWebRequest)
                        Else
                            postReq = CType(WebRequest.Create(urlz.Text), HttpWebRequest)
                        End If
                    End If

                    'Proxy support

                    If My.Settings.proxy_enabled = True Then

                        Try
                            Dim myProxy As New WebProxy()
                            Dim newUri As New Uri("http://" & My.Settings.proxy_host & ":" & My.Settings.proxy_port)
                            myProxy.Address = newUri
                            myProxy.Credentials = New NetworkCredential(My.Settings.proxy_us, My.Settings.proxy_ps)
                            postReq.Proxy = myProxy
                        Catch ex As Exception
                            MsgBox(ex.Message.ToString, vbCritical)
                        End Try

                    End If

                    postReq.Method = method.Text
                    'ADD HEADER
                    If (flag = 0) Then
                        postReq.Headers.Add("Authorization", oauthparams)

                    End If
                    postReq.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    Dim cnt As Integer = 0
                    Dim posSep, hname, hvalue As String
                    While cnt < headers.Items.Count
                        posSep = headers.Items(cnt).IndexOf(":")
                        hname = headers.Items(cnt).Substring(0, posSep)
                        hvalue = headers.Items(cnt).Substring(posSep + 1)
                        If hname.Contains("User-Agent") Then
                            postReq.UserAgent = hvalue
                        ElseIf hname.Contains("Connection") Then
                            postReq.Connection = hvalue
                        ElseIf hname.Contains("Content-Type") Then
                            postReq.ContentType = hvalue
                        ElseIf hname.Equals("Accept") Then
                            postReq.Accept = hvalue
                        ElseIf hname.Contains("Referer") Then
                            postReq.Referer = hvalue
                        ElseIf hname.Contains("Expect") Then
                            postReq.Expect = hvalue
                        Else
                            postReq.Headers.Add(hname, hvalue)
                        End If
                        cnt = cnt + 1
                    End While

                    'Cookie
                    For Each cok As String In cookies.Items
                        posSep = cok.IndexOf(":")
                        hname = cok.Substring(0, posSep)
                        hvalue = cok.Substring(posSep + 1)
                        tempCookies.Add(New Uri(urlz.Text), New Cookie(hname, hvalue))
                    Next
                    postReq.CookieContainer = tempCookies

                    'RESPONSE
                    Dim myHttpWebResponse As HttpWebResponse = CType(postReq.GetResponse(), HttpWebResponse)

                    Dim postreqreader As New StreamReader(myHttpWebResponse.GetResponseStream)

                    RTB.Text = postreqreader.ReadToEnd

                    responseheaders.Text += "HTTP/" + myHttpWebResponse.ProtocolVersion.ToString + " " + Str(myHttpWebResponse.StatusCode) + " " + myHttpWebResponse.StatusDescription.ToString + ControlChars.CrLf
                    Dim i As Integer
                    While i < myHttpWebResponse.Headers.Count
                        responseheaders.Text += myHttpWebResponse.Headers.Keys(i) + ":" + myHttpWebResponse.Headers(i) + ControlChars.CrLf
                        i = i + 1
                    End While

                    myHttpWebResponse.Close()
                Catch wb As WebException
                    responseheaders.Text = wb.Message.ToString
                    Using response As WebResponse = wb.Response
                        Dim httpResponse As HttpWebResponse = DirectCast(response, HttpWebResponse)

                        Using data As Stream = response.GetResponseStream()
                            Using reader = New StreamReader(data)
                                Dim text As String = reader.ReadToEnd()

                                RTB.Text = text
                            End Using
                        End Using
                    End Using

                Catch xxy As Exception
                    responseheaders.Text = xxy.Message.ToString

                End Try
            Else
                MsgBox("Unsupported Method!", vbCritical)

            End If
            If xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then
                xenotix_main.web.DocumentText = RTB.Text
                xenotix_main.webkit.LoadHtml(RTB.Text)
                xenotix_main.gecko.LoadHtml(RTB.Text)
            ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then
                xenotix_main.web.DocumentText = RTB.Text
                xenotix_main.webkit.LoadHtml(RTB.Text)

            ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = False Then
                xenotix_main.web.DocumentText = RTB.Text

            ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then
                xenotix_main.web.DocumentText = RTB.Text
                xenotix_main.gecko.LoadHtml(RTB.Text)
            ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then

                xenotix_main.gecko.LoadHtml(RTB.Text)
            ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then

                xenotix_main.webkit.LoadHtml(RTB.Text)

            ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then

                xenotix_main.webkit.LoadHtml(RTB.Text)
                xenotix_main.gecko.LoadHtml(RTB.Text)
            End If

        End If
    End Sub

    Private Sub browserparams_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles browserparams.DocumentCompleted

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Me.Close()
    End Sub

    Private Sub xss_oauth_request_repeater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next
        browserparams.ScriptErrorsSuppressed = True
        tcount.Text = xenotix_main.RichTextBox1.Lines.Length
        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
        Me.ActiveControl = urlz
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        On Error Resume Next

        Dim flg As Integer = 0
        If TextBox3.Text < 1 Then
            MsgBox("Time Interval should not be less than 1 sec", vbInformation)
            flg = 1
        End If
        If Not (urlz.Text.Contains("[X]") Or parameters.Text.Contains("[X]")) Then
            MsgBox("Replace the URL or Parameters with [X] to start testing.", vbInformation)
            flg = 1
        End If

        If flg = 0 Then


            oparams = parameters.Text
            ourl = urlz.Text
            Dim t As Integer = Convert.ToInt32(TextBox3.Text)
            t = t * 1000
            Timer1.Interval = t
            Timer1.Enabled = True

        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim t As Integer = 1000
            Try
                t = Convert.ToInt32(TextBox3.Text)
                t = t * 1000
            Catch ex As Exception
                t = 1000
            End Try
            Timer1.Interval = t
            Dim ran = New Random()
            Dim random As String = Str(ran.Next(2300, 2400))
            Dim turl, tparams
            If (i < xenotix_main.RichTextBox1.Lines.Length) Then
                If (i = xenotix_main.RichTextBox1.Lines.Length) Then
                    cnt.Text = xenotix_main.RichTextBox1.Lines.Length
                Else
                    cnt.Text = i + 1
                End If

                xenotix_main.textxss.Text = xenotix_main.RichTextBox1.Lines(i)
                headers.Text = ""
                RTB.Text = ""

                tparams = oparams.Replace("[X]", xenotix_main.textxss.Text).Replace("[R]", random)
                parameters.Text = tparams
                turl = ourl.Replace("[X]", xenotix_main.textxss.Text).Replace("[R]", random)
                urlz.Text = turl
                Dim html As String = ""
                'OAuth in Header
                If (CheckBox2.Checked = True) Then
                    html = oauth_auth_header.Text.Replace("XURL", urlz.Text).Replace("XPARAMS", parameters.Text).Replace("XMETHOD", method.Text).Replace("XVERSION", version.Text).Replace("XSIG_METHOD", signaturemethod.Text).Replace("XCONSUMER_KEY", ckey.Text).Replace("XCONSUMER_SECRET", csecret.Text).Replace("XOTOKEN", token.Text).Replace("XTOKEN_SECRET", tokensecret.Text)
                    browserparams.DocumentText = html
                    flag = 0
                Else
                    'OAuth in URL
                    html = TextBox1.Text.Replace("XURL", urlz.Text).Replace("XPARAMS", parameters.Text).Replace("XMETHOD", method.Text).Replace("XVERSION", version.Text).Replace("XSIG_METHOD", signaturemethod.Text).Replace("XCONSUMER_KEY", ckey.Text).Replace("XCONSUMER_SECRET", csecret.Text).Replace("XOTOKEN", token.Text).Replace("XTOKEN_SECRET", tokensecret.Text)
                    browserparams.DocumentText = html
                    flag = 1
                End If
                i = i + 1
                'Time Remaining

                Dim iSecond As Double = Convert.ToInt32(TextBox3.Text) * (Convert.ToInt32(tcount.Text) - Convert.ToInt32(cnt.Text))
                Dim iSpan As TimeSpan = TimeSpan.FromSeconds(iSecond)

                Label21.Text = iSpan.Hours.ToString.PadLeft(2, "0"c) & ":" & _
                                        iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" & _
                                        iSpan.Seconds.ToString.PadLeft(2, "0"c)

            Else
                MsgBox("Scan Finished!", vbInformation)
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, vbCritical)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        On Error Resume Next
        Timer1.Enabled = False
        Button9.Visible = False
        Button15.Visible = True
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Timer1.Enabled = True
        Button9.Visible = True
        Button15.Visible = False
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        On Error Resume Next
        If i <= tcount.Text Then
            i = i + 1
            cnt.Text = i + 1
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        On Error Resume Next
        Dim x As Integer = InputBox("Enter the Payload position index", "Skip to Payload")
        If x <= tcount.Text Then
            i = x
            cnt.Text = x
        Else
            MsgBox("Invalid Payload index!", vbCritical)
        End If
    End Sub
End Class