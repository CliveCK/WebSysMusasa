Public Class PolicyObjects

    Public Class Security

        Public Shared Property PasswordExpires() As Boolean
            Get
                Return Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordExpires", True)
            End Get
            Set(ByVal value As Boolean)
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordExpires", value)
            End Set
        End Property

        Public Shared Property MaxInvalidPasswordAttempts() As Long
            Get
                Return Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "MaxInvalidPasswordAttempts", 3)
            End Get
            Set(ByVal value As Long)
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "MaxInvalidPasswordAttempts", CLng(value))
            End Set
        End Property

        Public Shared Property PasswordAttemptWindow() As Long
            Get
                Return Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordAttemptWindow", 5) 'minutes
            End Get
            Set(ByVal value As Long)
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordAttemptWindow", CLng(value))
            End Set
        End Property

        Public Shared Property PasswordExpiryPeriod() As Long
            Get
                Return Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordExpiryPeriod", 3)
            End Get
            Set(ByVal value As Long)
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordExpiryPeriod", CLng(value))
            End Set
        End Property

        Public Shared Property PasswordAttemptsBeforeLockout() As Byte
            Get
                Return Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordAttemptsBeforeLockout", 3)
            End Get
            Set(ByVal value As Byte)
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WebSys\ApplicationServices\Policy\Security", "PasswordAttemptsBeforeLockout", value)
            End Set
        End Property

    End Class

End Class
