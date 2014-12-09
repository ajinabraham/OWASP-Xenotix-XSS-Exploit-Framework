<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class xenotix_jsbeautifier
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(xenotix_jsbeautifier))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.code_chk = New System.Windows.Forms.RadioButton()
        Me.url_check = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.js = New System.Windows.Forms.TextBox()
        Me.jsurl = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(0, 21)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(886, 507)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.js)
        Me.TabPage1.Controls.Add(Me.jsurl)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(878, 481)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Input"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.code_chk)
        Me.GroupBox1.Controls.Add(Me.url_check)
        Me.GroupBox1.Location = New System.Drawing.Point(584, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(116, 44)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Source"
        '
        'code_chk
        '
        Me.code_chk.AutoSize = True
        Me.code_chk.Checked = True
        Me.code_chk.Location = New System.Drawing.Point(59, 21)
        Me.code_chk.Name = "code_chk"
        Me.code_chk.Size = New System.Drawing.Size(50, 17)
        Me.code_chk.TabIndex = 1
        Me.code_chk.TabStop = True
        Me.code_chk.Text = "Code"
        Me.code_chk.UseVisualStyleBackColor = True
        '
        'url_check
        '
        Me.url_check.AutoSize = True
        Me.url_check.Location = New System.Drawing.Point(6, 21)
        Me.url_check.Name = "url_check"
        Me.url_check.Size = New System.Drawing.Size(47, 17)
        Me.url_check.TabIndex = 0
        Me.url_check.Text = "URL"
        Me.url_check.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(706, 14)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(60, 36)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Beautify"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "URL:"
        '
        'js
        '
        Me.js.Location = New System.Drawing.Point(0, 56)
        Me.js.Multiline = True
        Me.js.Name = "js"
        Me.js.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.js.Size = New System.Drawing.Size(875, 429)
        Me.js.TabIndex = 3
        '
        'jsurl
        '
        Me.jsurl.Location = New System.Drawing.Point(60, 22)
        Me.jsurl.Name = "jsurl"
        Me.jsurl.Size = New System.Drawing.Size(518, 20)
        Me.jsurl.TabIndex = 2
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.WebBrowser1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(878, 481)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Beautified JS"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(875, 485)
        Me.WebBrowser1.TabIndex = 0
        Me.WebBrowser1.WebBrowserShortcutsEnabled = False
        '
        'Timer1
        '
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(835, 4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(44, 33)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Close"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'xenotix_jsbeautifier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 528)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "xenotix_jsbeautifier"
        Me.Text = "JavaScript Beautify"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents code_chk As System.Windows.Forms.RadioButton
    Friend WithEvents url_check As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents js As System.Windows.Forms.TextBox
    Friend WithEvents jsurl As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
