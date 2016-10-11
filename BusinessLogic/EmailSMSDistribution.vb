Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports SpectrumITS.Communications.CampaignManager.ReportsDistribution
Imports SpectrumITS.Communications.CampaignManager.ReportsDistribution.Exporting
Imports SpectrumITS.Communications.CampaignManager.ReportsDistribution.OutputFormats
Imports SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportDefinitions
Imports CampaignManager.Integration.WebServices.ReportsDistribution
Imports System.Configuration
Imports System.Web.Services

Public Class EmailSMSDistribution

    Protected Shared db As Database
    Private mObjectUserID As Long
    Private mConnectionName As String

    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long)

        mObjectUserID = ObjectUserID
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Function CreateCampaign(ByVal OwnEmail As String, ByVal CellNo() As String, ByVal EmailAddress() As String, ByVal Message As String, ByVal Subject As String, ByVal ParamArray args() As String) As Boolean

        Try

            Dim ReplyTo As String = OwnEmail, FromAddress As String = OwnEmail
            Dim NotificationAddress As String = OwnEmail

            Dim wsCrystal As New CampaignManager.Integration.WebServices.ReportsDistribution.ReportDistributionCampaign
            Dim objCampaign As New SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportDistributionCampaign

            Dim con As String = ConfigurationManager.ConnectionStrings(mConnectionName).ConnectionString
            Dim builder As New SqlClient.SqlConnectionStringBuilder(con)

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            'This method creates a campaign and returns the Campaign ID. 
            'Use this Campaign ID for all interactions with Campaign Manager regarding this campagin
            Dim CampaignID As String = String.Empty

            Try

                CampaignID = wsCrystal.CreateCampaign()
                If String.IsNullOrWhiteSpace(CampaignID) Then
                    log.Error("Failed to get CampaignID. Please check Campaign Manager service.")
                    Return False
                End If

            Catch ex As Exception
                log.Error(ex)
                Return False
            End Try

            objCampaign.CampaignID = CampaignID


            CreateEmailReportsPackage(
                        objCampaign,
                        builder, mConnectionName, "", mObjectUserID, "", EmailAddress, CellNo, Message, Subject
                    )

            wsCrystal.UploadCampaign(objCampaign)

            wsCrystal.SetDefaults(FromAddress, Subject, Message.Replace(vbCrLf, "<br />"), objCampaign.CampaignID, ReplyTo, NotificationAddress)
            wsCrystal.SendToQueue(objCampaign.CampaignID, Subject)

            Return True

        Catch ex As Exception
            log.Error(ex)
            Return False
        End Try

    End Function

    Public Function CreateSMSCampaign(ByVal OwnEmail As String, ByVal CellNo() As String, ByVal EmailAddress() As String, ByVal Message As String, ByVal Subject As String, ByVal ParamArray args() As String) As Boolean

        Try

            Dim ReplyTo As String = OwnEmail, FromAddress As String = OwnEmail
            Dim NotificationAddress As String = OwnEmail

            Dim wsCrystal As New CampaignManager.Integration.WebServices.ReportsDistribution.ReportDistributionCampaign
            Dim objCampaign As New SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportDistributionCampaign

            Dim con As String = ConfigurationManager.ConnectionStrings(mConnectionName).ConnectionString
            Dim builder As New SqlClient.SqlConnectionStringBuilder(con)

            builder.ConnectionString = con
            builder.AsynchronousProcessing = True

            'This method creates a campaign and returns the Campaign ID. 
            'Use this Campaign ID for all interactions with Campaign Manager regarding this campagin
            Dim CampaignID As String = String.Empty

            Try

                CampaignID = wsCrystal.CreateCampaign()
                If String.IsNullOrWhiteSpace(CampaignID) Then
                    log.Error("Failed to get CampaignID. Please check Campaign Manager service.")
                    Return False
                End If

            Catch ex As Exception
                log.Error(ex)
                Return False
            End Try

            objCampaign.CampaignID = CampaignID


            CreateSMSReportsPackage(
                        objCampaign,
                        builder, "", mObjectUserID, "", EmailAddress, CellNo, Message
                    )

            wsCrystal.UploadCampaign(objCampaign)

            wsCrystal.SetDefaults(FromAddress, Subject, Message.Replace(vbCrLf, "<br />"), objCampaign.CampaignID, ReplyTo, NotificationAddress)
            wsCrystal.SendToQueue(objCampaign.CampaignID, Subject)

            Return True

        Catch ex As Exception
            log.Error(ex)
            Return False
        End Try

    End Function


    Public Sub CreateEmailReportsPackage(
                    ByVal objCampaign As SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportDistributionCampaign,
                    ByVal cnn As SqlConnectionStringBuilder,
                    ByVal ConnectionName As String,
                    ByVal SettingsPath As String,
                    ByVal UserID As Long,
                    ByVal UserName As String,
                    ByVal EmailAddress() As String,
                    ByVal CellPhoneNo() As String,
                    ByVal Message As String,
                    ByVal Subject As String
                )

        With objCampaign

            .ServerName = cnn.DataSource  'SQL Server Instance Name
            .DatabaseName = cnn.InitialCatalog 'SQL Server Database Name
            .UserName = cnn.UserID 'SQL Server User
            .Password = cnn.Password 'SQL Server User Password

            With .CampaignData

                .Application = "PrimaSys"
                .CreatedBy = UserID
                .CreatedDate = Now
                .Name = "Primasys Email Distribution: " & Subject

            End With

            Dim package As New SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportsPackage
            With package

                .EmailSendingOptions.AllowBlankEmailBody = False
                .EmailSendingOptions.AllowEmailsWithoutAttachments = True
                .ProcessingMode = ReportPackageProcessingModes.PerRecipient

                With .Reports 'Use the reports collection to define the set of reports

                    Dim objRpt As ReportDefinition

                    objRpt = New ReportDefinitions.EmailReportDefinition With {
                    .CampaignID = objCampaign.CampaignID,
                    .HtmlView = Message,
                    .PlainTextView = Message
                    }

                    objRpt.ParameterFields.AddRange(New String() {"@Name", "@Company", "@CompanyEmail"})
                    objRpt.LoadReportData()

                    .Add(objRpt)

                End With

            End With

            .ReportsPackages.Add(package)

            Try

                Dim ReceipientsSQL As String = ""

                'ensure that emails are part of the receipients list
                For Each email As String In EmailAddress

                    If email IsNot "" Then

                        If ReceipientsSQL = "" Then

                            ReceipientsSQL &= String.Format("SELECT '{0}' As  ID,	'{1}' As  Name,	'{2}' As  Address,	'{3}' As  Mobile,	'{4}' As  Message,	'{5}' As  DefaultPassword",
                                                                                                UserID, UserName, email, CellPhoneNo, Message, "")

                        ElseIf ReceipientsSQL.Contains("SELECT") Then

                            ReceipientsSQL &= " UNION " & vbCrLf
                            ReceipientsSQL &= String.Format("SELECT '{0}' As  ID,	'{1}' As  Name,	'{2}' As  Address,	'{3}' As  Mobile,	'{4}' As  Message,	'{5}' As  DefaultPassword",
                                                                                                UserID, UserName, email, CellPhoneNo, Message, "")
                        End If

                    End If

                Next

                .ReportsPackages.Add(package)
                .PackageRecipients.Query = ReceipientsSQL

                Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(.PackageRecipients.Query)
                .PackageRecipients.Data = db.ExecuteDataSet(cmd)

            Catch ex As Exception

                log.Warn("Failed to get package recipients: " & vbCrLf & .PackageRecipients.Query)
                log.Error(ex)

            End Try
        End With

    End Sub

    Public Shared Sub CreateSMSReportsPackage(
            ByVal objCampaign As SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportDistributionCampaign,
            ByVal cnn As SqlConnectionStringBuilder,
            ByVal SettingsPath As String,
            ByVal UserID As Long,
            ByVal UserName As String,
            ByVal EmailAddress() As String,
            ByVal CellPhoneNo() As String,
            ByVal SMSTemplate As String
        )


        With objCampaign

            .ServerName = cnn.DataSource  'SQL Server Instance Name
            .DatabaseName = cnn.InitialCatalog 'SQL Server Database Name
            .UserName = cnn.UserID 'SQL Server User
            .Password = cnn.Password 'SQL Server User Password

            With .CampaignData

                .Application = "PrimaSys"
                .CreatedBy = UserID
                .CreatedDate = Now
                .Name = "PrimaSys"

            End With

            Dim package As New SpectrumITS.Communications.CampaignManager.ReportsDistribution.ReportsPackage
            With package

                .EmailSendingOptions.AllowBlankEmailBody = False
                .EmailSendingOptions.AllowEmailsWithoutAttachments = True
                .ProcessingMode = ReportPackageProcessingModes.PerRecipient

                With .Reports 'Use the reports collection to define the set of reports

                    Dim objRpt As ReportDefinition

                    objRpt = New SQLQueryReportDefinition With {
                                .CampaignID = objCampaign.CampaignID,
                                .ReportPath = "SMSMessage.sql",
                                .OutputFormat = New TXTFileOutputFormat,
                                .ExportDestination = New SMSExportDestination()
                            }
                    objRpt.ParameterFields.AddRange(New String() {"@MemberID", "@TransactionDate", "@Amount"})
                    objRpt.FileData = System.Text.ASCIIEncoding.ASCII.GetBytes(SMSTemplate)

                    .Add(objRpt)

                End With

            End With

            .ReportsPackages.Add(package)
            Dim ReceipientsSQL As String = ""

            'ensure that emails are part of the receipients list
            For Each Cell As String In CellPhoneNo

                If Cell IsNot "" Then

                    If ReceipientsSQL = "" Then

                        ReceipientsSQL &= String.Format("SELECT '{0}' As  ID,	'{1}' As  Name,	'{2}' As  Address,	'{3}' As  Mobile,	'{4}' As  Message,	'{5}' As  DefaultPassword",
                                                                                                UserID, UserName, EmailAddress, CellPhoneNo, SMSTemplate, "")

                    ElseIf ReceipientsSQL.Contains("SELECT") Then

                        ReceipientsSQL &= " UNION " & vbCrLf
                        ReceipientsSQL &= String.Format("SELECT '{0}' As  ID,	'{1}' As  Name,	'{2}' As  Address,	'{3}' As  Mobile,	'{4}' As  Message,	'{5}' As  DefaultPassword",
                                                                                                UserID, UserName, EmailAddress, CellPhoneNo, SMSTemplate, "")
                    End If

                End If

            Next

            Try
                .PackageRecipients.Query = ReceipientsSQL
                Dim cmd As System.Data.Common.DbCommand = db.GetSqlStringCommand(.PackageRecipients.Query)
                .PackageRecipients.Data = db.ExecuteDataSet(cmd)

            Catch ex As Exception

                log.Warn("Failed to get package recipients: " & vbCrLf & .PackageRecipients.Query)
                log.Error(ex)

            End Try
        End With

        'Serialize object to a text file.
        'Dim objStreamWriter As New StreamWriter("C:\RDP\objCampaign.xml")
        'Dim x As New XmlSerializer(objCampaign.GetType)
        'x.Serialize(objStreamWriter, objCampaign)
        'objStreamWriter.Close()

    End Sub

End Class
