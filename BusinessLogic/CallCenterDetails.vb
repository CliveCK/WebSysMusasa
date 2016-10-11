Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class CallCenterDetails

#region "Variables"

    Protected mCallCenterDetailID As Long
    Protected mDistrictID As Long
    Protected mWardID As Long
    Protected mTypeOfIssueID As long
    Protected mReferredFromID As long
    Protected mReferredToID As long
    Protected mDOB As string
    Protected mCallCode As string
    Protected mCellNumber As string
    Protected mFirstName As string
    Protected mSurname As string
    Protected mNationalIDNum As string
    Protected mSex As string
    Protected mAddress As string
    Protected mDetails As string
    Protected mActionTaken As String
    Protected mNotes As String
    Protected mCallDate As String

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

    Public  Property CallCenterDetailID() As long
        Get
		return mCallCenterDetailID
        End Get
        Set(ByVal value As long)
		mCallCenterDetailID = value
        End Set
    End Property

    Public Property DistrictID() As Long
        Get
            Return mDistrictID
        End Get
        Set(ByVal value As Long)
            mDistrictID = value
        End Set
    End Property

    Public Property WardID() As Long
        Get
            Return mWardID
        End Get
        Set(ByVal value As Long)
            mWardID = value
        End Set
    End Property

    Public  Property TypeOfIssueID() As long
        Get
		return mTypeOfIssueID
        End Get
        Set(ByVal value As long)
		mTypeOfIssueID = value
        End Set
    End Property

    Public  Property ReferredFromID() As long
        Get
		return mReferredFromID
        End Get
        Set(ByVal value As long)
		mReferredFromID = value
        End Set
    End Property

    Public  Property ReferredToID() As long
        Get
		return mReferredToID
        End Get
        Set(ByVal value As long)
		mReferredToID = value
        End Set
    End Property

    Public  Property DOB() As string
        Get
		return mDOB
        End Get
        Set(ByVal value As string)
		mDOB = value
        End Set
    End Property

    Public Property CallCode() As String
        Get
            Return mCallCode
        End Get
        Set(ByVal value As String)
            mCallCode = value
        End Set
    End Property

    Public Property CallDate() As String
        Get
            Return mCallDate
        End Get
        Set(ByVal value As String)
            mCallDate = value
        End Set
    End Property

    Public  Property CellNumber() As string
        Get
		return mCellNumber
        End Get
        Set(ByVal value As string)
		mCellNumber = value
        End Set
    End Property

    Public  Property FirstName() As string
        Get
		return mFirstName
        End Get
        Set(ByVal value As string)
		mFirstName = value
        End Set
    End Property

    Public  Property Surname() As string
        Get
		return mSurname
        End Get
        Set(ByVal value As string)
		mSurname = value
        End Set
    End Property

    Public  Property NationalIDNum() As string
        Get
		return mNationalIDNum
        End Get
        Set(ByVal value As string)
		mNationalIDNum = value
        End Set
    End Property

    Public  Property Sex() As string
        Get
		return mSex
        End Get
        Set(ByVal value As string)
		mSex = value
        End Set
    End Property

    Public  Property Address() As string
        Get
		return mAddress
        End Get
        Set(ByVal value As string)
		mAddress = value
        End Set
    End Property

    Public  Property Details() As string
        Get
		return mDetails
        End Get
        Set(ByVal value As string)
		mDetails = value
        End Set
    End Property

    Public  Property ActionTaken() As string
        Get
		return mActionTaken
        End Get
        Set(ByVal value As string)
		mActionTaken = value
        End Set
    End Property

    Public  Property Notes() As string
        Get
		return mNotes
        End Get
        Set(ByVal value As string)
		mNotes = value
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

    CallCenterDetailID = 0
        mDistrictID = 0
        mWardID = 0
        mTypeOfIssueID = 0
        mCallDate = ""
        mReferredFromID = 0
    mReferredToID = 0
    mDOB = ""
    mCallCode = ""
    mCellNumber = ""
    mFirstName = ""
    mSurname = ""
    mNationalIDNum = ""
    mSex = ""
    mAddress = ""
    mDetails = ""
    mActionTaken = ""
    mNotes = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCallCenterDetailID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CallCenterDetailID As Long) As Boolean 

        Dim sql As String 

        If CallCenterDetailID > 0 Then 
            sql = "SELECT * FROM tblCallCenterDetails WHERE CallCenterDetailID = " & CallCenterDetailID
        Else 
            sql = "SELECT * FROM tblCallCenterDetails WHERE CallCenterDetailID = " & mCallCenterDetailID
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

                Log.Error("CallCenterDetails not found.")

                Return False

            End If

        Catch e As Exception

            Log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetCallCenterDetails() As System.Data.DataSet

        Return GetCallCenterDetails(mCallCenterDetailID)

    End Function

    Public Overridable Function GetAllCallCenterDetails() As DataSet

        Dim sql As String

        sql = "  Select *, D.Name As District, N.Description As TypeOfIssue, RF.Description As ReferredFrom, RT.Description as ReferredTo "
        sql &= "From tblCallCenterDetails C left outer Join tblDistricts D On C.DistrictID = D.DistrictID "
        sql &= "Left outer join luNatureOfProblems N On N.NatureOfProblemID = TypeOfIssueID "
        sql &= "Left outer join luReferralCentreTypes RF On RF.ReferralCentreTypeID = C.ReferredFromID "
        sql &= "Left outer join luReferralCentreTypes RT On RT.ReferralCentreTypeID = C.ReferredToID"

        Return GetCallCenterDetails(sql)

    End Function

    Public Overridable Function GetCallCenterDetails(ByVal CallCenterDetailID As Long) As DataSet

        Dim sql As String

        If CallCenterDetailID > 0 Then
            sql = "SELECT * FROM tblCallCenterDetails WHERE CallCenterDetailID = " & CallCenterDetailID
        Else
            sql = "SELECT * FROM tblCallCenterDetails WHERE CallCenterDetailID = " & mCallCenterDetailID
        End If

        Return GetCallCenterDetails(sql)

    End Function

    Protected Overridable Function GetCallCenterDetails(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mCallCenterDetailID = Catchnull(.Item("CallCenterDetailID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mTypeOfIssueID = Catchnull(.Item("TypeOfIssueID"), 0)
            mReferredFromID = Catchnull(.Item("ReferredFromID"), 0)
            mReferredToID = Catchnull(.Item("ReferredToID"), 0)
            mDOB = Catchnull(.Item("DOB"), "")
            mCallCode = Catchnull(.Item("CallCode"), "")
            mCellNumber = Catchnull(.Item("CellNumber"), "")
            mFirstName = Catchnull(.Item("FirstName"), "")
            mSurname = Catchnull(.Item("Surname"), "")
            mNationalIDNum = Catchnull(.Item("NationalIDNum"), "")
            mSex = Catchnull(.Item("Sex"), "")
            mAddress = Catchnull(.Item("Address"), "")
            mDetails = Catchnull(.Item("Details"), "")
            mActionTaken = Catchnull(.Item("ActionTaken"), "")
            mNotes = Catchnull(.Item("Notes"), "")
            mCallDate = Catchnull(.Item("CallDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@CallCenterDetailID", DbType.Int32, mCallCenterDetailID)
        db.AddInParameter(cmd, "@DistrictID", DbType.Int32, mDistrictID)
        db.AddInParameter(cmd, "@WardID", DbType.Int32, mWardID)
        db.AddInParameter(cmd, "@TypeOfIssueID", DbType.Int32, mTypeOfIssueID)
        db.AddInParameter(cmd, "@ReferredFromID", DbType.Int32, mReferredFromID)
        db.AddInParameter(cmd, "@ReferredToID", DbType.Int32, mReferredToID)
        db.AddInParameter(cmd, "@DOB", DbType.String, mDOB)
        db.AddInParameter(cmd, "@CallCode", DbType.String, mCallCode)
        db.AddInParameter(cmd, "@CellNumber", DbType.String, mCellNumber)
        db.AddInParameter(cmd, "@FirstName", DbType.String, mFirstName)
        db.AddInParameter(cmd, "@Surname", DbType.String, mSurname)
        db.AddInParameter(cmd, "@NationalIDNum", DbType.String, mNationalIDNum)
        db.AddInParameter(cmd, "@Sex", DbType.String, mSex)
        db.AddInParameter(cmd, "@Address", DbType.String, mAddress)
        db.AddInParameter(cmd, "@Details", DbType.String, mDetails)
        db.AddInParameter(cmd, "@ActionTaken", DbType.String, mActionTaken)
        db.AddInParameter(cmd, "@Notes", DbType.String, mNotes)
        db.AddInParameter(cmd, "@CallDate", DbType.String, mCallDate)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_CallCenterDetails")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mCallCenterDetailID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            Log.Error(ex)
            Return False

        End Try

    End Function

#End Region

#Region "Delete"

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblCallCenterDetails SET Deleted = 1 WHERE CallCenterDetailID = " & mCallCenterDetailID) 
        Return Delete("DELETE FROM tblCallCenterDetails WHERE CallCenterDetailID = " & mCallCenterDetailID)

    End Function

    Protected Overridable Function Delete(ByVal DeleteSQL As String) As Boolean

        Try

            db.ExecuteNonQuery(CommandType.Text, DeleteSQL)
            Return True

        Catch e As Exception

            Log.Error(e)
            Return False 

        End Try 

    End Function 

#End Region 

#end region

End Class