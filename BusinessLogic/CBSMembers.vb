﻿Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CBSMembers

#region "Variables"

    Protected mCBSMemberID As long
    Protected mBeneficiaryID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Properties"

    Public ReadOnly Property Database() As Database 
        Get 
            Return db 
        End Get 
    End Property 
     
    Public ReadOnly Property OwnerType() As String 
        Get 
            Return Me.GetType.Name 
        End Get 
    End Property 

    Public ReadOnly Property ConnectionName() As String 
        Get 
            Return mConnectionName 
        End Get 
    End Property 

    Public  Property CBSMemberID() As long
        Get
		return mCBSMemberID
        End Get
        Set(ByVal value As long)
		mCBSMemberID = value
        End Set
    End Property

    Public  Property BeneficiaryID() As long
        Get
		return mBeneficiaryID
        End Get
        Set(ByVal value As long)
		mBeneficiaryID = value
        End Set
    End Property

    Public  Property CreatedBy() As long
        Get
		return mCreatedBy
        End Get
        Set(ByVal value As long)
		mCreatedBy = value
        End Set
    End Property

    Public  Property UpdatedBy() As long
        Get
		return mUpdatedBy
        End Get
        Set(ByVal value As long)
		mUpdatedBy = value
        End Set
    End Property

    Public  Property CreatedDate() As string
        Get
		return mCreatedDate
        End Get
        Set(ByVal value As string)
		mCreatedDate = value
        End Set
    End Property

    Public  Property UpdatedDate() As string
        Get
		return mUpdatedDate
        End Get
        Set(ByVal value As string)
		mUpdatedDate = value
        End Set
    End Property

#end region

#region "Methods"

#Region "Constructors" 
 
    Public Sub New(ByVal ConnectionName As String, ByVal ObjectUserID As Long) 

        mObjectUserID = ObjectUserID 
        mConnectionName = ConnectionName
        db = New DatabaseProviderFactory().Create(ConnectionName)

    End Sub 

#End Region 

Public Sub Clear()  

    CBSMemberID = 0
    mBeneficiaryID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCBSMemberID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CBSMemberID As Long) As Boolean 

        Dim sql As String 

        If CBSMemberID > 0 Then 
            sql = "SELECT * FROM tblCBSMembers WHERE CBSMemberID = " & CBSMemberID
        Else 
            sql = "SELECT * FROM tblCBSMembers WHERE CBSMemberID = " & mCBSMemberID
        End If 

        Return Retrieve(sql) 

    End Function 

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else

                log.Error("CBSMembers not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCBSMembers() As System.Data.DataSet

        Return GetCBSMembers(mCBSMemberID)

    End Function

    Public Overridable Function GetCBSMembers(ByVal CBSMemberID As Long) As DataSet

        Dim sql As String

        If CBSMemberID > 0 Then
            sql = "SELECT * FROM tblCBSMembers WHERE CBSMemberID = " & CBSMemberID
        Else
            sql = "SELECT * FROM tblCBSMembers WHERE CBSMemberID = " & mCBSMemberID
        End If

        Return GetCBSMembers(sql)

    End Function

    Public Function GetAllCBSMembersByCBSMemberReportingID(ByVal CBSMemberReportingID As Long) As DataSet

        Dim sql As String

        sql = "Select B.*, A.Description As AssistanceProvided, NP.Description As Problem, R.Description As ReferredTo from tblBeneficiaries B "
        sql &= " inner Join tblCBSMemberNeeds N on N.BeneficiaryID = B.BeneficiaryID "
        sql &= "inner join tblBeneficiaryCBSMemberReportingID D on D.BeneficiaryID = B.BeneficiaryID "
        sql &= "inner Join luAssistenceAndServicesProvided A on A.AssistenceAndServicesID = N.AssistanceID "
        sql &= "inner join luNatureOfProblems NP on NP.NatureOfProblemID = N.NeedID "
        sql &= "Left outer join luReferralCentreTypes R on R.ReferralCentreTypeID = ReferredToID "
        sql &= "where CBSMemberReportingID = " & CBSMemberReportingID

        Return GetCBSMembers(sql)

    End Function

    Public Overridable Function GetAllCBSMembers(ByVal BeneficiaryID As Long) As DataSet

        Dim sql As String


        sql = "Select NP.NatureOfProblemID, A.Description As AssistanceProvided, NP.Description As Problem, R.Description As ReferredTo, Comments from tblBeneficiaries B "
        sql &= "inner join tblCBSMemberNeeds N On N.BeneficiaryID = B.BeneficiaryID "
        sql &= "inner join tblBeneficiaryCBSMemberReportingID D on D.BeneficiaryID = B.BeneficiaryID "
        sql &= "inner join luAssistenceAndServicesProvided A On A.AssistenceAndServicesID = N.AssistanceID "
        sql &= "inner join luNatureOfProblems NP on NP.NatureOfProblemID = N.NeedID "
        sql &= "left outer join luReferralCentreTypes R On R.ReferralCentreTypeID = ReferredToID WHERE B.BeneficiaryID = " & BeneficiaryID


        Return GetCBSMembers(sql)

    End Function

    Protected Overridable Function GetCBSMembers(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCBSMemberID = Catchnull(.Item("CBSMemberID"), 0)
            mBeneficiaryID = Catchnull(.Item("BeneficiaryID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CBSMemberID", DbType.Int32, mCBSMemberID)
        db.AddInParameter(cmd, "@BeneficiaryID", DbType.Int32, mBeneficiaryID)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CBSMembers")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCBSMemberID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblCBSMembers Set Deleted = 1 WHERE CBSMemberID = " & mCBSMemberID) 
        Return Delete("DELETE FROM tblCBSMembers WHERE CBSMemberID = " & mCBSMemberID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            log.Error(e)
            Return False 

        End Try 

    End Function 

#End Region 

#end region

End Class