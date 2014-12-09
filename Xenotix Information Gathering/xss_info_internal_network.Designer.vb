<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class xss_info_internal_network
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(xss_info_internal_network))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.startip = New System.Windows.Forms.TextBox()
        Me.code1 = New System.Windows.Forms.TextBox()
        Me.code2 = New System.Windows.Forms.TextBox()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.code3 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.endip = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Tag = "IP: "
        Me.Label1.Text = "Start IP:"
        '
        'startip
        '
        Me.startip.Location = New System.Drawing.Point(72, 9)
        Me.startip.Name = "startip"
        Me.startip.Size = New System.Drawing.Size(95, 20)
        Me.startip.TabIndex = 20
        Me.startip.Text = "192.168.1.1"
        '
        'code1
        '
        Me.code1.Location = New System.Drawing.Point(522, 9)
        Me.code1.Multiline = True
        Me.code1.Name = "code1"
        Me.code1.Size = New System.Drawing.Size(13, 14)
        Me.code1.TabIndex = 19
        Me.code1.Text = resources.GetString("code1.Text")
        Me.code1.Visible = False
        '
        'code2
        '
        Me.code2.Location = New System.Drawing.Point(541, 9)
        Me.code2.Multiline = True
        Me.code2.Name = "code2"
        Me.code2.Size = New System.Drawing.Size(13, 14)
        Me.code2.TabIndex = 18
        Me.code2.Text = resources.GetString("code2.Text")
        Me.code2.Visible = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(12, 36)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(558, 434)
        Me.WebBrowser1.TabIndex = 17
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(405, 7)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(60, 23)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "Close"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(338, 7)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(61, 23)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "Scan"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'code3
        '
        Me.code3.Location = New System.Drawing.Point(557, 7)
        Me.code3.Multiline = True
        Me.code3.Name = "code3"
        Me.code3.Size = New System.Drawing.Size(13, 14)
        Me.code3.TabIndex = 22
        Me.code3.Text = resources.GetString("code3.Text")
        Me.code3.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(176, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 24
        Me.Label2.Tag = "IP: "
        Me.Label2.Text = "End IP:"
        '
        'endip
        '
        Me.endip.Location = New System.Drawing.Point(224, 9)
        Me.endip.Name = "endip"
        Me.endip.Size = New System.Drawing.Size(95, 20)
        Me.endip.TabIndex = 23
        Me.endip.Text = "192.168.1.25"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'xss_info_internal_network
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(580, 467)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.endip)
        Me.Controls.Add(Me.code3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.startip)
        Me.Controls.Add(Me.code1)
        Me.Controls.Add(Me.code2)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "xss_info_internal_network"
        Me.Text = "Internal Network Scanner"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents startip As System.Windows.Forms.TextBox
    Friend WithEvents code1 As System.Windows.Forms.TextBox
    Friend WithEvents code2 As System.Windows.Forms.TextBox
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents code3 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents endip As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
