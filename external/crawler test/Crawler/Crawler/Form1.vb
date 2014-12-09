Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim spider As New Chilkat.Spider()

        Dim seenDomains As New Chilkat.StringArray()
        Dim seedUrls As New Chilkat.StringArray()

        seenDomains.Unique = True
        seedUrls.Unique = True

        '  You will need to change the start URL to something else...
        seedUrls.Append("http://opensecurity.in/")

        '  Set outbound URL exclude patterns
        '  URLs matching any of these patterns will not be added to the
        '  collection of outbound links.
        spider.AddAvoidOutboundLinkPattern("*?id=*")
        spider.AddAvoidOutboundLinkPattern("*.mypages.*")
        spider.AddAvoidOutboundLinkPattern("*.personal.*")
        spider.AddAvoidOutboundLinkPattern("*.comcast.*")
        spider.AddAvoidOutboundLinkPattern("*.aol.*")
        spider.AddAvoidOutboundLinkPattern("*~*")

        '  Use a cache so we don't have to re-fetch URLs previously fetched.
        spider.CacheDir = "c:/spiderCache/"
        spider.FetchFromCache = True
        spider.UpdateCache = True

        While seedUrls.Count > 0

            Dim url As String
            url = seedUrls.Pop()
            spider.Initialize(url)

            '  Spider 5 URLs of this domain.
            '  but first, save the base domain in seenDomains
            Dim domain As String
            domain = spider.GetUrlDomain(url)
            seenDomains.Append(spider.GetBaseDomain(domain))

            Dim i As Long
            Dim success As Boolean
            For i = 0 To 4
                success = spider.CrawlNext()
                If (success <> True) Then
                    Exit For
                End If


                '  Display the URL we just crawled.
                TextBox1.Text = TextBox1.Text & spider.LastUrl & vbCrLf

                '  If the last URL was retrieved from cache,
                '  we won't wait.  Otherwise we'll wait 1 second
                '  before fetching the next URL.
                If (spider.LastFromCache <> True) Then
                    spider.SleepMs(1000)
                End If

            Next

            '  Add the outbound links to seedUrls, except
            '  for the domains we've already seen.
            For i = 0 To spider.NumOutboundLinks - 1

                url = spider.GetOutboundLink(i)

                domain = spider.GetUrlDomain(url)
                Dim baseDomain As String
                baseDomain = spider.GetBaseDomain(domain)
                If (Not seenDomains.Contains(baseDomain)) Then
                    seedUrls.Append(url)
                End If


                '  Don't let our list of seedUrls grow too large.
                If (seedUrls.Count > 1000) Then
                    Exit For
                End If

            Next

        End While
    End Sub
End Class
