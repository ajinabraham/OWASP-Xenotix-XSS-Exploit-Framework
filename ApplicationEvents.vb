Imports Gecko
Imports System.IO

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Protected Overrides Function OnStartup(ByVal eventArgs As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) As Boolean

            '   set the path to the temp files
            Dim ProfileDirectory As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Xenotix\DefaultProfile"
            If Not Directory.Exists(ProfileDirectory) Then
                Directory.CreateDirectory(ProfileDirectory)
            End If

            '   set the temp-directory for the gecko
            Xpcom.ProfileDirectory = ProfileDirectory

            '   set the path of the directory where the xulrunner files are located
            Dim xrPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location
            xrPath = xrPath.Substring(0, xrPath.LastIndexOf("\") + 1) & "\xulrunner"

            '   initialize the path
            Xpcom.Initialize(xrPath)
            Return True
        End Function

    End Class
End Namespace