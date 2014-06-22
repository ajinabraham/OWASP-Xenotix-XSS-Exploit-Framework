Public Class xenotix_gproxy
   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error Resume Next
        If Not addr.Text = "" And Not port.Text = "" Then
            If use_proxy.Checked = True Then
                My.Settings.proxy_enabled = True
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyServer", My.Settings.proxy_host & ":" & My.Settings.proxy_port, Microsoft.Win32.RegistryValueKind.String)
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", "1", Microsoft.Win32.RegistryValueKind.DWord)

            Else
                My.Settings.proxy_enabled = False
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", "0", Microsoft.Win32.RegistryValueKind.DWord)

            End If
            My.Settings.proxy_host = addr.Text
            My.Settings.proxy_port = port.Text
            My.Settings.proxy_us = us.Text
            My.Settings.proxy_ps = pass.Text
            My.Settings.Save()
          
            MsgBox("Proxy Settings has been saved.", vbInformation)
            Me.Close()
        Else
            MsgBox("Please provide valid Proxy Settings.", vbCritical)
            Me.Close()
        End If

    End Sub

 


    Private Sub xenotix_gproxy_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
   
        'Load Saved Settings
        If My.Settings.proxy_enabled = True Then
            use_proxy.Checked = True
        ElseIf My.Settings.proxy_enabled = False Then
            use_proxy.Checked = False
        End If
        Me.addr.Text = My.Settings.proxy_host
        Me.port.Text = My.Settings.proxy_port
        Me.us.Text = My.Settings.proxy_us
        Me.pass.Text = My.Settings.proxy_ps
    End Sub

    Private Sub use_proxy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles use_proxy.CheckedChanged
      
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub
End Class