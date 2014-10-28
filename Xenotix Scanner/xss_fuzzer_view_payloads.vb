Public Class xss_fuzzer_view_payloads
    Dim request_repeater = False
    Dim xss_fuzzer_intelli As Boolean = False
    Dim xss_fuzzer_context As Boolean = False
    Dim xss_fuzzer_intelli_post As Boolean = False
    Dim xss_fuzzer_context_post As Boolean = False
    Dim xss_fuzzer_intelli_request As Boolean = False
    Dim xss_fuzzer_context_request As Boolean = False
    Private _ID As Integer
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value

            If _ID = 1 Then
                xss_fuzzer_intelli = True
            ElseIf _ID = 2 Then
                xss_fuzzer_context = True
            ElseIf _ID = 3 Then
                xss_fuzzer_intelli_post = True
            ElseIf _ID = 4 Then
                xss_fuzzer_context_post = True
            ElseIf _ID = 5 Then
                xss_fuzzer_intelli_request = True
            ElseIf _ID = 6 Then
                xss_fuzzer_context_request = True
            End If
        End Set
    End Property
    Private Sub xss_intelli_payloads_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ListView1.ShowItemToolTips = True
        ListView1.View = View.Details
        ' Set column header
        ListView1.Columns.Clear()
        ListView1.Columns.Add("No", 100)
        ListView1.Columns.Add("Payload", 500)
        Dim i As Integer = 0
        If request_repeater Then

        ElseIf xss_fuzzer_intelli Then
            LoadPayloads(xss_fuzzer_get.intellipayloadlist)
        ElseIf xss_fuzzer_context Then
            LoadPayloads(xss_fuzzer_get.payloadlist)
        ElseIf xss_fuzzer_intelli_post Then
            LoadPayloads(xss_fuzzer_post.intellipayloadlist)
        ElseIf xss_fuzzer_context_post Then
            LoadPayloads(xss_fuzzer_post.payloadlist)
        ElseIf xss_fuzzer_intelli_request Then
            LoadPayloads(xss_fuzzer_request.intellipayloadlist)
        ElseIf xss_fuzzer_context_request Then
            LoadPayloads(xss_fuzzer_request.payloadlist)
        End If
    End Sub
    Function LoadPayloads(ByVal o As Object)
        For Each itm In o
            Dim i As Integer = 0
            Dim TempStr(1) As String
            Dim TempNode As ListViewItem
            TempStr(0) = i + 1
            TempStr(1) = itm
            TempNode = New ListViewItem(TempStr)
            ListView1.Items.Add(TempNode)
            i = i + 1
        Next
        Return 0
    End Function
    Private Sub xss_intelli_payloads_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ListView1.Height = Me.Height - 53
        ListView1.Width = Me.Width - 35
    End Sub
End Class