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

            Dim ProfileDirectory As String = My.Application.Info.DirectoryPath
            Dim parent = Directory.GetParent(ProfileDirectory)
            ProfileDirectory = parent.ToString & "\xulrunner\"

           If Not Directory.Exists(ProfileDirectory) Then
                Directory.CreateDirectory(ProfileDirectory)
            End If
            Xpcom.ProfileDirectory = ProfileDirectory

            Dim xrPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location
            xrPath = ProfileDirectory
            Xpcom.Initialize(xrPath)

            Return True
        End Function
    End Class


End Namespace

