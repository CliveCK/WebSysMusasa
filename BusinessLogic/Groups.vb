Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Groups

#region "Variables"

    Protected mGroupID As long
    Protected mWardID As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mGroupTypeID As long
    Protected mDescription As String
    Protected mGroupSize As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As String
    Protected mGroupName As String
    Protected mMales As Long
    Protected mFemales As Long

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long

    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#end region

#region "Properties"

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

    Public Property GroupID() As Long
        Get
            Return mGroupID
        End Get
        Set(ByVal value As Long)
            mGroupID = value
        End Set
    End Property

    Public Property Females() As Long
        Get
            Return mFemales
        End Get
        Set(ByVal value As Long)
            mFemales = value
        End Set
    End Property

    Public Property Males() As Long
        Get
            Return mMales
        End Get
        Set(ByVal value As Long)
            mMales = value
        End Set
    End Property

    Public  Property WardID() As long
        Get
		return mWardID
        End Get
        Set(ByVal value As long)
		mWardID = value
        End Set
    End Property

    Public Property ProvinceID() As Long
        Get
            Return mProvinceID
        End Get
        Set(ByVal value As Long)
            mProvinceID = value
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

    Public  Property GroupTypeID() As long
        Get
		return mGroupTypeID
        End Get
        Set(ByVal value As long)
		mGroupTypeID = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

    Public  Property GroupSize() As long
        Get
		return mGroupSize
        End Get
        Set(ByVal value As long)
		mGroupSize = value
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

    Public  Property GroupName() As string
        Get
		return mGroupName
        End Get
        Set(ByVal value As string)
		mGroupName = value
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

    GroupID = 0
    mWardID = 0
    mGroupTypeID = 0
    mDescription = 0
    mGroupSize = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
        mGroupName = ""
        mMales = 0
        mFemales = 0

    End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mGroupID) 

    End Function 

    Public Overridable Function Retrieve(ByVal GroupID As Long) As Boolean 

        Dim sql As String 

        If GroupID > 0 Then 
            sql = "SELECT C.*, D.DistrictID, P.ProvinceID FROM tblGroups C inner join tblWards W on W.WardID = C.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  WHERE GroupID = " & GroupID
        Else 
            sql = "SELECT * FROM tblGroups WHERE GroupID = " & mGroupID
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

                log.Warn("Groups not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function

    Public Function RetrieveAll() As DataSet

        Dim sql As String = "select G.*, GT.Description As GroupType, D.Name As District from tblGroups G left outer join luGroupTypes GT on G.GroupTypeID = GT.GroupTypeID "
        sql &= "left outer join tblWards W on W.WardID = G.WardID "
        sql &= "left outer join tblDistricts D on D.DistrictID = W.DistrictID "

        Return GetGroups(sql)

    End Function
    Public Overridable Function GetGroups() As System.Data.DataSet

        Return GetGroups(mGroupID)

    End Function

    Public Overridable Function GetGroups(ByVal GroupID As Long) As DataSet

        Dim sql As String

        If GroupID > 0 Then
            sql = "SELECT * FROM tblGroups WHERE GroupID = " & GroupID
        Else
            sql = "SELECT * FROM tblGroups WHERE GroupID = " & mGroupID
        End If

        Return GetGroups(sql)

    End Function

    Public Function GetGroupMembership(ByVal GroupID As Long) As DataSet

        Dim sql As String = "SELECT BeneficiaryID, MemberNo, Suffix, FirstName + ' ' + Surname As Name, DateOfBirth ,MaritalStatus FROM tblBeneficiaries B inner join tblHouseholdGroups HG on  "
        sql &= " B.BeneficiaryID = HouseholdID inner join tblGroups G on G.GroupID = HG.GroupID WHERE HG.GroupID = " & GroupID

        Return GetGroups(sql)

    End Function

    Protected Overridable Function GetGroups(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mGroupID = Catchnull(.Item("GroupID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mGroupTypeID = Catchnull(.Item("GroupTypeID"), 0)
            mDescription = Catchnull(.Item("Description"), "")
            mGroupName = Catchnull(.Item("GroupName"), "")
            mGroupSize = Catchnull(.Item("GroupSize"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mMales = Catchnull(.Item("NumberOfMales"), 0)
            mFemales = Catchnull(.Item("NumberOfFemales"), 0)

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@GroupID", DbType.Int32, mGroupID)
        db.AddInParameter(cmd, "@WardID", DbType.Int32, mWardID)
        db.AddInParameter(cmd, "@GroupTypeID", DbType.Int32, mGroupTypeID)
        db.AddInParameter(cmd, "@Description", DbType.String, mDescription)
        db.AddInParameter(cmd, "@GroupSize", DbType.Int32, mGroupSize)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@GroupName", DbType.String, mGroupName)
        db.AddInParameter(cmd, "@NumberOfMales", DbType.Int32, mMales)
        db.AddInParameter(cmd, "@NumberOfFemales", DbType.Int32, mFemales)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Groups")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mGroupID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblGroups SET Deleted = 1 WHERE GroupID = " & mGroupID) 
        Return Delete("DELETE FROM tblGroups WHERE GroupID = " & mGroupID)

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

#End Region

End Class