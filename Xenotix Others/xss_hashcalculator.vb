Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Threading

Public Class xss_hashcalculator

    Private trd As Thread
    Public Function getCookie(ByVal url As String) As CookieContainer

        Dim tempCookies As New CookieContainer
        Try

            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36"
            request.Headers.Add("Pragma", "no-cache")
            request.CookieContainer = tempCookies
            'Proxy support
            If My.Settings.proxy_enabled = True Then

                Try
                    Dim myProxy As New WebProxy()
                    Dim newUri As New Uri("http://" & My.Settings.proxy_host & ":" & My.Settings.proxy_port)
                    myProxy.Address = newUri
                    myProxy.Credentials = New NetworkCredential(My.Settings.proxy_us, My.Settings.proxy_ps)
                    request.Proxy = myProxy
                Catch ex As Exception
                    MsgBox(ex.Message.ToString, vbCritical)
                End Try

            End If
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            tempCookies.Add(response.Cookies)
            response.Close()
            Return tempCookies
        Catch wb As WebException
            MsgBox(wb.Message.ToString, vbCritical)
        Catch xxx As Exception
            MsgBox(xxx.Message.ToString, vbCritical)
        End Try
        Return tempCookies
    End Function
    Function getHash(ByVal param As String, ByVal hashname As String) As String
        Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create("https://www.tools4noobs.com"), HttpWebRequest)
        Dim encoding As New UTF8Encoding
        Dim postData As String = param
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
        postReq.Method = "POST"
        postReq.KeepAlive = True
        postReq.CookieContainer = getCookie("https://www.tools4noobs.com")
        postReq.Referer = "https://www.tools4noobs.com/"
        postReq.ContentType = "application/x-www-form-urlencoded"
        postReq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36"
        postReq.ContentLength = byteData.Length
        postReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        postReq.Headers.Add("Keep-Alive", "115")
        Dim req, absp As String
        Dim myUri As New Uri("https://www.tools4noobs.com/")
        absp = myUri.AbsolutePath
        req = postReq.Method & " " & absp & " HTTP/" & postReq.ProtocolVersion.ToString & vbCrLf
        Try
            Dim postreqstream As Stream = postReq.GetRequestStream()
            postreqstream.Write(byteData, 0, byteData.Length)
            postreqstream.Close()
        Catch xx As Exception

            MsgBox(xx.Message.ToString, vbCritical)
            Me.Close()
        End Try
        'RESPONSE
        Dim x As String = ""
        Try
            Dim myHttpWebResponse As HttpWebResponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
            tempCookies.Add(myHttpWebResponse.Cookies)

            Dim postreqreader As New StreamReader(myHttpWebResponse.GetResponseStream, encoding)
            x = postreqreader.ReadToEnd
            x = x.Replace("Result", hashname)
            myHttpWebResponse.Close()
        Catch xxx As Exception
            MsgBox(xxx.Message.ToString, vbCritical)
            Me.Close()
        End Try
        Return x
    End Function
    Private Sub ThreadTask()
        Try


            Dim op As String
            op = "<font face='arial'>"
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=md2", "MD2")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=md4", "MD4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=md5", "MD5")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=sha1", "SHA1")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=sha224", "SHA224")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=sha256", "SHA256")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=sha384", "SHA384")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=sha512", "SHA512")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=ripemd128", "RIPEMD128")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=ripemd160", "RIPEMD160")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=ripemd256", "RIPEMD256")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=ripemd320", "RIPEMD320")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=whirlpool", "WHIRLPOOL")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger128,3", "TIGER128,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger160,3", "TIGER160,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger192,3", "TIGER192,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger128,4", "TIGER128,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger160,4", "TIGER160,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=tiger192,4", "TIGER192,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=snefru", "SNEFRU")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=snefru256", "SNEFRU256")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=gost", "GOST")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=adler32", "ADLER32")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=crc32", "CRC32")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=crc32b", "CRC32b")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=fnv132", "FNV132")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=fnv164", "FNV164")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=joaat", "JOAAT")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval128,3", "HAVAL128,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval160,3", "HAVAL160,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval192,3", "HAVAL192,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval224,3", "HAVAL224,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval256,3", "HAVAL256,3")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval128,4", "HAVAL128,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval160,4", "HAVAL160,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval192,4", "HAVAL192,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval224,4", "HAVAL224,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval256,4", "HAVAL256,4")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval128,5", "HAVAL128,5")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval160,5", "HAVAL160,5")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval192,5", "HAVAL192,5")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval224,5", "HAVAL224,5")
            op += getHash("action=ajax_hash&text=" & hash.Text & "&algo=haval256,5", "HAVAL256,5")

            WebBrowser1.DocumentText = op


        Catch

            trd.Abort()
            Close()
        End Try
        Thread.Sleep(1)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        On Error Resume Next
        Button1.Enabled = False
        MsgBox("Obtaining hashes. Please Wait...", vbInformation)
        trd = New Thread(AddressOf ThreadTask)
        trd.IsBackground = True
        trd.Start()
        Button1.Enabled = True
    End Sub


    Private Sub xss_hash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub xss_hash_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        WebBrowser1.Width = Me.Width - 10
        WebBrowser1.Height = Me.Height - 50
    End Sub
End Class