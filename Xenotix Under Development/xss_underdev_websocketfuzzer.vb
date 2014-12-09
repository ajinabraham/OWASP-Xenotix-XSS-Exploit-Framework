Imports WebSocket4Net

Public Class xss_underdev_websocketfuzzer

    Dim wb As WebSocket
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        wb = New WebSocket(TextBox1.Text)
     
        AddHandler wb.Opened, AddressOf Opened
        AddHandler wb.Error, AddressOf Errors
        AddHandler wb.Closed, AddressOf ClosedConn
        AddHandler wb.MessageReceived, AddressOf MessageRecv
        wb.Open()

    End Sub
    Private Sub Opened(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("Opened")
        wb.Send("Ajin")
    End Sub
    Private Sub Errors(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("Error")
    End Sub
    Private Sub ClosedConn(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("Closed")
    End Sub
    Private Sub MessageRecv(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("Message Received")
    End Sub
End Class



'Private Shared consoleLock As New Object()
'Private Const sendChunkSize As Integer = 256
'Private Const receiveChunkSize As Integer = 256
'Private Const verbose As Boolean = True
'Private Shared ReadOnly delay As TimeSpan = TimeSpan.FromMilliseconds(30000)



'Public Shared Async Function Connect(uri As String) As Task
'    Dim webSocket As ClientWebSocket = Nothing

'    Try
'        webSocket = New ClientWebSocket()
'        Await webSocket.ConnectAsync(New Uri(uri), CancellationToken.None)
'        Await Task.WhenAll(Receive(webSocket), Send(webSocket))
'    Catch ex As Exception
'        Console.WriteLine("Exception: {0}", ex)
'    Finally
'        If webSocket IsNot Nothing Then
'            webSocket.Dispose()
'        End If
'        Console.WriteLine()

'        SyncLock consoleLock
'            Console.ForegroundColor = ConsoleColor.Red
'            Console.WriteLine("WebSocket closed.")
'            Console.ResetColor()
'        End SyncLock
'    End Try
'End Function
'Shared encoder As New UTF8Encoding()

'Private Shared Async Function Send(webSocket As ClientWebSocket) As Task

'    'byte[] buffer = encoder.GetBytes("{\"op\":\"blocks_sub\"}"); //"{\"op\":\"unconfirmed_sub\"}");
'    Dim buffer As Byte() = encoder.GetBytes("{""op"":""unconfirmed_sub""}")
'    Await webSocket.SendAsync(New ArraySegment(Of Byte)(buffer), WebSocketMessageType.Text, True, CancellationToken.None)

'    While webSocket.State = WebSocketState.Open
'        LogStatus(False, buffer, buffer.Length)
'        Await Task.Delay(delay)
'    End While
'End Function

'Private Shared Async Function Receive(webSocket As ClientWebSocket) As Task
'    Dim buffer As Byte() = New Byte(receiveChunkSize - 1) {}
'    While webSocket.State = WebSocketState.Open
'        Dim result = Await webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), CancellationToken.None)
'        If result.MessageType = WebSocketMessageType.Close Then
'            Await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None)
'        Else
'            LogStatus(True, buffer, result.Count)
'        End If
'    End While
'End Function

'Private Shared Sub LogStatus(receiving As Boolean, buffer As Byte(), length As Integer)
'    SyncLock consoleLock
'        Console.ForegroundColor = If(receiving, ConsoleColor.Green, ConsoleColor.Gray)
'        'Console.WriteLine("{0} ", receiving ? "Received" : "Sent");

'        If verbose Then
'            Console.WriteLine(encoder.GetString(buffer))
'        End If

'        Console.ResetColor()
'    End SyncLock
'End Sub


'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'    Thread.Sleep(1000)
'    Connect(TextBox1.Text).Wait()

'End Sub