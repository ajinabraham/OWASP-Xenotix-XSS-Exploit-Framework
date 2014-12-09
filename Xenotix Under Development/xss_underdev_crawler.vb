'Imports System.Threading
'Imports System.Text.RegularExpressions
'Imports System.Net
'Imports System.IO
'Imports HtmlAgilityPack

'Public Class xss_underdev_crawler
'    'Dim hostz As String

'    ' Private trd As Thread
'    'Private Sub ThreadTask()
'    '    Dim CrawlOnly As Array
'    '    If CheckBox1.Checked = True Then
'    '        CrawlOnly = TextBox2.Text.Split(",")
'    '    Else
'    '        CrawlOnly = {""}
'    '    End If
'    '    Spider(scanURL.Text, HScrollBar1.Value, CrawlOnly)


'    'End Sub

'    Private Sub AddToList(ByVal url As String)
'        If Me.crawledurl.InvokeRequired Then
'            Me.crawledurl.Invoke(New Action(Of String)(AddressOf AddToList), url)
'        Else
'            crawledurl.Items.Add(url)
'        End If
'    End Sub


'    'Private Function Spider(ByVal url As String, ByVal depth As Integer, ByVal CrawlOnly As Array)
'    '    'aReturn is used to hold the list of urls
'    '    Dim aReturn As New ArrayList
'    '    'aStart is used to hold the new urls to be checked
'    '    Dim aStart As ArrayList = GrabUrls(url, CrawlOnly)
'    '    'temp array to hold data being passed to new arrays
'    '    Dim aTemp As ArrayList
'    '    'aNew is used to hold new urls before being passed to aStart
'    '    Dim aNew As New ArrayList
'    '    'add the first batch of urls
'    '    aReturn.AddRange(aStart)
'    '    'loops through the levels of urls
'    '    For i = 1 To depth
'    '        'grabs the urls from each url in aStart
'    '        For Each tUrl As String In aStart
'    '            'grabs the urls and returns non-duplicates
'    '            aTemp = GrabUrls(tUrl, aReturn, aNew, CrawlOnly)
'    '            'add the urls to be check to aNew
'    '            aNew.AddRange(aTemp)
'    '        Next
'    '        'swap urls to aStart to be checked
'    '        aStart = aNew
'    '        'add the urls to the main list
'    '        aReturn.AddRange(aNew)
'    '        'clear the temp array
'    '        aNew = New ArrayList
'    '    Next
'    '    Return 0 'Remove From Final Code, If implemented
'    'End Function
'    'Private Overloads Function GrabUrls(ByVal url As String, ByVal CrawlOnly As Array) As ArrayList
'    '    'will hold the urls to be returned
'    '    Dim aReturn As New ArrayList
'    '    Try
'    '        'regex string used: thanks google
'    '        Dim strRegex As String = "<a.*?href=""(.*?)"".*?>(.*?)</a>"
'    '        'i used a webclient to get the source
'    '        'web requests might be faster
'    '        Dim wc As New WebClient
'    '        'put the source into a string
'    '        Dim strSource As String = wc.DownloadString(url)
'    '        Dim HrefRegex As New Regex(strRegex, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
'    '        'parse the urls from the source
'    '        Dim HrefMatch As Match = HrefRegex.Match(strSource)
'    '        'used later to get the base domain without subdirectories or pages
'    '        Dim BaseUrl As New Uri(url)
'    '        'while there are urls
'    '        While HrefMatch.Success = True
'    '            'loop through the matches
'    '            Dim sUrl As String = HrefMatch.Groups(1).Value
'    '            'if it's a page or sub directory with no base url (domain)
'    '            If Not (sUrl.Contains("http://") Or sUrl.Contains("https://")) AndAlso Not sUrl.Contains("www") Then
'    '                'add the domain plus the page
'    '                Dim tURi As New Uri(BaseUrl, sUrl)
'    '                sUrl = tURi.ToString
'    '            End If
'    '            If sUrl.StartsWith("//") Then
'    '                If url.Contains("https://") Then sUrl = "https:" + sUrl
'    '                If url.Contains("http://") Then sUrl = "http:" + sUrl

'    '            End If
'    '            'if it's not already in the list then add it

'    '            If Not aReturn.Contains(sUrl) Then
'    '                If Not (crawledurl.Items.Contains(sUrl)) And Not (sUrl.StartsWith("./")) And Not (sUrl.StartsWith("javascript:")) And Not (sUrl.StartsWith("data:")) Then
'    '                    For Each x In CrawlOnly
'    '                        If sUrl.EndsWith(x) Then
'    '                            AddToList(sUrl)
'    '                        End If
'    '                    Next
'    '                End If
'    '                aReturn.Add(sUrl)

'    '            End If
'    '            'go to the next url
'    '            HrefMatch = HrefMatch.NextMatch
'    '        End While
'    '    Catch ex As Exception
'    '        'catch ex here. I left it blank while debugging
'    '    End Try

'    '    Return aReturn
'    'End Function
'    'Private Overloads Function GrabUrls(ByVal url As String, ByRef aReturn As ArrayList, ByRef aNew As ArrayList, ByVal CrawlOnly As Array) As ArrayList
'    '    'overloads function to check duplicates in aNew and aReturn
'    '    'temp url arraylist
'    '    Dim tUrls As ArrayList = GrabUrls(url, CrawlOnly)
'    '    'used to return the list
'    '    Dim tReturn As New ArrayList
'    '    'check each item to see if it exists, so not to grab the urls again
'    '    For Each item As String In tUrls
'    '        If Not aReturn.Contains(item) AndAlso Not aNew.Contains(item) Then
'    '            If Not (crawledurl.Items.Contains(item)) And Not (item.StartsWith("./")) And Not (item.StartsWith("javascript:")) And Not (item.StartsWith("data:")) Then
'    '                For Each x In CrawlOnly
'    '                    If item.EndsWith(x) Then
'    '                        AddToList(item)
'    '                    End If
'    '                Next
'    '            End If
'    '            tReturn.Add(item)
'    '        End If
'    '    Next
'    '    Return tReturn
'    'End Function

'    Private Sub HScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
'        crawlDepth.Text = HScrollBar1.Value
'    End Sub

'    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
'        crawledurl.Items.Clear()

'        MsgBox("Spidering the URL")
'        trd = New Thread(AddressOf ThreadTask)
'        trd.IsBackground = True
'        trd.Start()

'    End Sub

'    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

'    End Sub

'    Private Sub xss_crawler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        HScrollBar1.Value = 1
'        crawlDepth.Text = HScrollBar1.Value
'    End Sub

'    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
'        trd.Abort()

'    End Sub

'    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
'        'Dim htmlDoc As New HtmlDocument()
'        'Using webClient = New System.Net.WebClient()
'        '    Using stream = webClient.OpenRead("http://opensecurity.in/contact-us/")
'        '        htmlDoc.Load(stream)

'        '    End Using
'        'End Using
'        ''   Dim divTags As HtmlAgilityPack.HtmlNodeCollection = htmlDoc.DocumentNode.SelectNodes("//input | //select | //textarea")

'        'Dim forms As HtmlAgilityPack.HtmlNodeCollection = htmlDoc.DocumentNode.SelectNodes("//form")
'        'Dim faction, fmethod, fid, fname As String
'        'For Each Form As HtmlNode In forms

'        '    faction = Form.GetAttributeValue("action", "none")
'        '    fmethod = Form.GetAttributeValue("method", "none")

'        '    fid = Form.GetAttributeValue("id", "none")
'        '    fname = Form.GetAttributeValue("name", "none")
'        '    MsgBox(" Action: " + faction + " Method: " + fmethod + " Name: " + fname + " ID : " + fid)
'        '    For Each InputNode As HtmlNode In Form.ChildNodes
'        '        Dim Name As String = ""
'        '        Dim Value As String = ""
'        '        For Each Attr As HtmlAttribute In InputNode.Attributes
'        '            Select Case Attr.Name
'        '                Case ("name")
'        '                    Name = Attr.Value
'        '                    MsgBox(Name)
'        '                    Exit Select
'        '                Case ("type")
'        '                    If Attr.Value.Equals("submit") Then
'        '                        Name = ""
'        '                    End If
'        '                    Exit Select
'        '                Case ("value")
'        '                    Value = Attr.Value
'        '                    MsgBox(Value)
'        '                    Exit Select
'        '            End Select
'        '        Next
'        '    Next
'        'Next
'        ''  Dim value = htmlDoc.DocumentNode.SelectSingleNode("//form")
'        '' MsgBox(value.OuterHtml)

'        ''Dim signupFormIdElement = htmlDoc.DocumentNode.SelectSingleNode("//input")

'        ''Dim signupFormId = signupFormIdElement.GetAttributeValue("name", "")

'        '' MsgBox(signupFormId)

'    End Sub
'End Class