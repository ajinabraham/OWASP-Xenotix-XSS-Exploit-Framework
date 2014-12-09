Option Strict Off
Imports ICSharpCode.TextEditor.Document
Imports System.IO
Imports IronPython.Hosting
Imports IronPython
Imports System.Text
Imports Microsoft.Scripting
Imports Microsoft.Scripting.Hosting
Imports System.Threading

Public Class main
    Private trd As Thread


    Dim textEditorControl1 As New ICSharpCode.TextEditor.TextEditorControl()
    Dim m_searchPaths As New List(Of String)()
    Dim filepath As String
    'IronPython Engine
    Public engine As ScriptEngine
    Public scope As ScriptScope
    Public source As ScriptSource
    Private Sub SetError(ByVal s As String)
        If Me.RTB.InvokeRequired Then
            Me.RTB.Invoke(New Action(Of String)(AddressOf SetError), s)
        Else
            Me.RTB.Text += s
        End If
    End Sub
    
    Private Sub SelectTab(ByVal no As Integer)
        If Me.TabControl1.InvokeRequired Then
            Me.TabControl1.Invoke(New Action(Of Integer)(AddressOf SelectTab), no)
        Else
            Me.TabControl1.SelectTab(no)
        End If
    End Sub

    Private Sub AsyncNavigate(url As String)
        Dim starter As ThreadStart = Sub() AsyncNavigateThread(url)
        Dim thread As New Thread(starter)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Private Delegate Sub StringDelegate(url As String)
    Private Sub AsyncNavigateThread(url As String)
        If Me Is Nothing OrElse Me.IsDisposed Then
            Return
        End If
        If Me.InvokeRequired OrElse GeckoWebBrowser1.InvokeRequired Then
            Me.BeginInvoke(New StringDelegate(AddressOf AsyncNavigateThread), New Object() {url})
            Return
        End If
        GeckoWebBrowser1.Navigate(url)
    End Sub
    Private Sub GeckoRender(html As String)
        Dim starter As ThreadStart = Sub() GeckoRenderThread(html)
        Dim thread As New Thread(starter)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Private Delegate Sub HTMLDelegate(html As String)
    Private Sub GeckoRenderThread(html As String)
        If Me Is Nothing OrElse Me.IsDisposed Then
            Return
        End If
        If Me.InvokeRequired OrElse GeckoWebBrowser1.InvokeRequired Then
            Me.BeginInvoke(New HTMLDelegate(AddressOf GeckoRenderThread), New Object() {html})
            Return
        End If
        GeckoWebBrowser1.LoadHtml(html)
    End Sub
    Private Sub Run()
    


        Try
            SelectTab(1)

            scope = engine.CreateScope()
            engine.Runtime.IO.RedirectToConsole()

            System.IO.File.WriteAllText(Application.StartupPath & "\\Modules\\TMP_XEN_IP_EXEC.py", textEditorControl1.Text, System.Text.Encoding.UTF8)
            If textEditorControl1.Text <> "" Then
                engine.ExecuteFile(Application.StartupPath & "\\Modules\\TMP_XEN_IP_EXEC.py", scope)
                Try
                    Dim web = scope.GetVariable("IBROWSER_RENDER")
                    WebBrowser1.DocumentText = web.ToString()
                    SelectTab(3)
                Catch
                End Try
                Try

                    Dim web = scope.GetVariable("GBROWSER_RENDER")
                    GeckoRender(web.ToString())
                    SelectTab(3)
                Catch
                End Try
                Try
                    Dim nav = scope.GetVariable("IBROWSER_NAVIGATE")
                    WebBrowser1.Navigate(nav.ToString())
                    SelectTab(3)
                Catch
                End Try
                Try
                    Dim nav = scope.GetVariable("GBROWSER_NAVIGATE")
                    AsyncNavigate(nav.ToString())
                    SelectTab(3)
                Catch
                End Try


            End If
        Catch ee As System.Data.SyntaxErrorException

            Dim eo As ExceptionOperations = engine.GetService(Of ExceptionOperations)()
            Dim errorz As String
            errorz = eo.FormatException(ee)
            Dim msg As String = "Syntax error in " & filepath
            MessageBox.Show(errorz, msg, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch e1 As Exception
            SetError(vbNewLine & e1.Message.ToString())
            SelectTab(2)


        End Try
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If (Not System.IO.File.Exists(Application.StartupPath & "\\Modules\\ext.dat")) Or (Not System.IO.File.Exists(Application.StartupPath & "\\Modules\\payload.dat")) Then
                MsgBox("Xenotix Server is not running!", vbCritical)
                End
            End If

            Me.textEditorControl1.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.textEditorControl1.IsReadOnly = False
            Me.textEditorControl1.Location = New System.Drawing.Point(0, 25)
            Me.textEditorControl1.Name = "textEditorControl1"
            Me.textEditorControl1.Height = Me.Height - 200
            Me.textEditorControl1.Width = Me.Width - 10
            Me.textEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.textEditorControl1.TabIndex = 0
            Me.textEditorControl1.ActiveTextAreaControl.TextArea.AllowDrop = True
            Me.TabPage1.Controls.Add(Me.textEditorControl1)
            WebBrowser1.ScriptErrorsSuppressed = True
            TabControl1.Height = Me.Height - 84
            TabControl1.Width = Me.Width - 10

            Dim dir As String = Application.StartupPath
            Dim fsmProvider As FileSyntaxModeProvider
            If System.IO.Directory.Exists(dir) Then
                fsmProvider = New FileSyntaxModeProvider(dir)
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider)
                textEditorControl1.SetHighlighting("Python")
            End If
            Dim payload As String = Application.StartupPath + "\Modules\payload.dat"
            payload = payload.Replace("\", "\\")
            Dim ext As String = Application.StartupPath + "\Modules\ext.dat"
            Dim content As String = System.IO.File.ReadAllText(Application.StartupPath + "\\Modules\Xenotix.py")
            ext = ext.Replace("\", "\\")
            content = content.Replace("XENPATHP", payload).Replace("XENPATHE", ext)
            System.IO.File.WriteAllText(Application.StartupPath + "\\Modules\Xenotix.py", content)
            'PATH
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory)
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory + "Lib")
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory + "Modules")
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory + "DLLs")
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory + "Lib\site-packages")
            m_searchPaths.Add(System.AppDomain.CurrentDomain.BaseDirectory + "Lib\site-packages\urllib3-1.7.1-py2.7.egg")
            m_searchPaths.Add("C:\Python27\Lib")
            m_searchPaths.Add("C:\Python27\DLLs")
            m_searchPaths.Add("C:\Python27\Lib\site-packages")
            m_searchPaths.Add("c:\Program Files (x86)\IronPython 2.7\DLLs")
            m_searchPaths.Add("c:\Program Files (x86)\IronPython 2.7\Lib")
            m_searchPaths.Add("c:\Program Files (x86)\IronPython 2.7\Lib\site-packages")
            m_searchPaths.Add("c:\Program Files\IronPython 2.7\DLLs")
            m_searchPaths.Add("c:\Program Files\IronPython 2.7\Lib")
            m_searchPaths.Add("c:\Program Files\IronPython 2.7\Lib\site-packages")
            Dim options = New Dictionary(Of String, Object)()
            options("Frames") = True
            options("FullFrames") = True
            options("Debug") = False
            engine = Python.CreateEngine(options)
            engine.SetSearchPaths(m_searchPaths)
            If (System.IO.File.Exists(Application.StartupPath & "\\Modules\\TMP_XEN_IP_EXEC.py")) Then
                textEditorControl1.Text = System.IO.File.ReadAllText(Application.StartupPath & "\\Modules\\TMP_XEN_IP_EXEC.py", System.Text.Encoding.UTF8)
            End If
            Console.SetOut(New TextBoxWriter(OutputTextBox, System.Text.Encoding.UTF8))
        Catch ee As Exception
            MsgBox("Error: " + ee.Message.ToString())
        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            Dim sr As New System.IO.StreamReader(OpenFileDialog1.FileName)
            textEditorControl1.Text = sr.ReadToEnd
            sr.Close()
            Me.Text = ("Xenotix Python Scripting Engine - " & OpenFileDialog1.FileName)
            filepath = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub main_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        TabControl1.Height = Me.Height - 84
        TabControl1.Width = Me.Width - 10
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        textEditorControl1.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(Me, e)
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        textEditorControl1.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(Me, e)
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        textEditorControl1.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(Me, e)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        textEditorControl1.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(Me, e)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

            Dim sw As StreamWriter = New StreamWriter(SaveFileDialog1.OpenFile())
            If (sw IsNot Nothing) Then

                sw.WriteLine(textEditorControl1.Text)
                Me.Text = ("Xenotix Python Scripting Engine - " & SaveFileDialog1.FileName)
                filepath = SaveFileDialog1.FileName

                sw.Close()
            End If
        End If
    End Sub

    Private Sub CustomToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'user modules
    End Sub

    Private Sub RunModuleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunModuleToolStripMenuItem.Click
        trd = New Thread(AddressOf Run)
        trd.IsBackground = True
        trd.Start()

    End Sub

    Private Sub AboutXenotixPythonScriptingEngineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutXenotixPythonScriptingEngineToolStripMenuItem.Click
        MsgBox("Xenotix Python Scripting Engine is a part of OWASP Xenotix XSS Exploit Framework.", vbInformation)
    End Sub

    Private Sub ToolStripStatusLabel1_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel1.Click
        WebBrowser1.DocumentText = OutputTextBox.Text
        GeckoWebBrowser1.LoadHtml(OutputTextBox.Text)
        TabControl1.SelectTab(3)
    End Sub

    Private Sub ToolStripStatusLabel2_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel2.Click
        If TabControl1.SelectedTab Is TabPage2 Then
            OutputTextBox.Text = ""
        ElseIf TabControl1.SelectedTab Is TabPage3 Then
            RTB.Text = ""
        ElseIf TabControl1.SelectedTab Is TabPage4 Then
            WebBrowser1.DocumentText = ""
            GeckoWebBrowser1.LoadHtml("")
        End If

    End Sub

    Private Sub StatusStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles StatusStrip1.ItemClicked

    End Sub
End Class

Public Class TextBoxWriter

    Inherits TextWriter
    Private ReadOnly m_encoding As Encoding
    Private ReadOnly textBox As TextBox
    Private Sub SetOp(ByVal value As String)
        If textBox.InvokeRequired Then
            textBox.Invoke(New Action(Of String)(AddressOf SetOp), value)
        Else
            textBox.AppendText(value)
        End If
    End Sub

    Public Sub New(textBox As TextBox)
        Me.New(textBox, System.Text.Encoding.UTF8)
    End Sub

    Public Sub New(textBox As TextBox, encoding As Encoding)
        Me.textBox = textBox
        Me.m_encoding = encoding
    End Sub

    Public Overrides Sub WriteLine(value As String)
        SetOp(value)
    End Sub

    Public Overrides Sub Write(value As Char)
        SetOp(value.ToString())
    End Sub

    Public Overrides Sub Write(value As String)
        SetOp(value)
    End Sub

    Public Overrides Sub Write(buffer As Char(), index As Integer, count As Integer)
        SetOp(New String(buffer))
    End Sub

    Public Overrides ReadOnly Property Encoding() As Encoding
        Get
            Return Me.m_encoding
        End Get
    End Property
End Class