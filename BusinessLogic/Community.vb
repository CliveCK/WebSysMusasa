Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Community

#region "Variables"

    Protected mCommunityID As long
    Protected mWardID As Long
    Protected mProvinceID As Long
    Protected mDistrictID As Long
    Protected mNoOfHouseholds As long
    Protected mNoOfIndividualAdultMales As long
    Protected mNoOfIndividualAdultFemales As long
    Protected mNoOfMaleYouths As long
    Protected mNoOfFemaleYouth As long
    Protected mNoOfChildren As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mName As string
    Protected mDescription As string

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

    Public  Property CommunityID() As long
        Get
		return mCommunityID
        End Get
        Set(ByVal value As long)
		mCommunityID = value
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


    Public  Property NoOfHouseholds() As long
        Get
		return mNoOfHouseholds
        End Get
        Set(ByVal value As long)
		mNoOfHouseholds = value
        End Set
    End Property

    Public  Property NoOfIndividualAdultMales() As long
        Get
		return mNoOfIndividualAdultMales
        End Get
        Set(ByVal value As long)
		mNoOfIndividualAdultMales = value
        End Set
    End Property

    Public  Property NoOfIndividualAdultFemales() As long
        Get
		return mNoOfIndividualAdultFemales
        End Get
        Set(ByVal value As long)
		mNoOfIndividualAdultFemales = value
        End Set
    End Property

    Public  Property NoOfMaleYouths() As long
        Get
		return mNoOfMaleYouths
        End Get
        Set(ByVal value As long)
		mNoOfMaleYouths = value
        End Set
    End Property

    Public  Property NoOfFemaleYouth() As long
        Get
		return mNoOfFemaleYouth
        End Get
        Set(ByVal value As long)
		mNoOfFemaleYouth = value
        End Set
    End Property

    Public  Property NoOfChildren() As long
        Get
		return mNoOfChildren
        End Get
        Set(ByVal value As long)
		mNoOfChildren = value
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

    Public  Property Name() As string
        Get
		return mName
        End Get
        Set(ByVal value As string)
		mName = value
        End Set
    End Property

    Public  Property Description() As string
        Get
		return mDescription
        End Get
        Set(ByVal value As string)
		mDescription = value
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

    CommunityID = 0
    mWardID = 0
    mNoOfHouseholds = 0
    mNoOfIndividualAdultMales = 0
    mNoOfIndividualAdultFemales = 0
    mNoOfMaleYouths = 0
    mNoOfFemaleYouth = 0
    mNoOfChildren = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""
    mName = ""
    mDescription = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mCommunityID) 

    End Function 

    Public Overridable Function Retrieve(ByVal CommunityID As Long) As Boolean 

        Dim sql As String 

        If CommunityID > 0 Then
            sql = "SELECT C.*, D.DistrictID, P.ProvinceID FROM tblCommunities C inner join tblWards W on W.WardID = C.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  WHERE CommunityID = " & CommunityID
        Else
            sql = "SELECT * FROM tblCommunities WHERE CommunityID = " & mCommunityID
        End If

        Return Retrieve(sql) 

    End Function

    Public Function RetrieveAll(ByVal Criteria As String) As DataSet

        Dim sql As String = "SELECT C.*, D.DistrictID, P.ProvinceID, W.WardID FROM tblCommunities C inner join tblWards W on W.WardID = C.WardID "
        sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
        sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID " & Criteria

        Return GetCommunity(sql)

    End Function

    Protected Overridable Function Retrieve(ByVal sql As String) As Boolean 

        Try 

            Dim dsRetrieve As DataSet = db.ExecuteDataSet(CommandType.Text, sql) 

            If dsRetrieve IsNot Nothing AndAlso dsRetrieve.Tables.Count > 0 AndAlso dsRetrieve.Tables(0).Rows.Count > 0 Then 

                LoadDataRecord(dsRetrieve.Tables(0).Rows(0)) 

                dsRetrieve = Nothing 
                Return True 

            Else 

                log.Warn("Community not found.")

                Return False 

            End If 

        Catch e As Exception 

            log.Error(e)
            Return False 

        End Try 

    End Function 

    Public Overridable Function GetCommunity() As System.Data.DataSet

        Return GetCommunity(mCommunityID) 

    End Function 

    Public Overridable Function GetCommunity(ByVal CommunityID As Long) As DataSet 

        Dim sql As String 

        If CommunityID > 0 Then 
            sql = "SELECT C.*, D.DistrictID, P.ProvinceID FROM tblCommunities C inner join tblWards W on W.WardID = C.WardID "
            sql &= "inner join tblDistricts D on D.DistrictID = W.DistrictID "
            sql &= "inner join tblProvinces P on P.ProvinceID = D.ProvinceID  WHERE CommunityID = " & CommunityID
        Else 
            sql = "SELECT * FROM tblCommunities WHERE CommunityID = " & mCommunityID
        End If 

        Return GetCommunity(sql) 

    End Function 

    Protected Overridable Function GetCommunity(ByVal sql As String) As DataSet 

        Return db.ExecuteDataSet(CommandType.Text, sql) 

    End Function 

#End Region 

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object) 

        With Record 

            mCommunityID = Catchnull(.Item("CommunityID"), 0)
            mWardID = Catchnull(.Item("WardID"), 0)
            mProvinceID = Catchnull(.Item("ProvinceID"), 0)
            mDistrictID = Catchnull(.Item("DistrictID"), 0)
            mNoOfHouseholds = Catchnull(.Item("NoOfHouseholds"), 0)
            mNoOfIndividualAdultMales = Catchnull(.Item("NoOfIndividualAdultMales"), 0)
            mNoOfIndividualAdultFemales = Catchnull(.Item("NoOfIndividualAdultFemales"), 0)
            mNoOfMaleYouths = Catchnull(.Item("NoOfMaleYouths"), 0)
            mNoOfFemaleYouth = Catchnull(.Item("NoOfFemaleYouth"), 0)
            mNoOfChildren = Catchnull(.Item("NoOfChildren"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mName = Catchnull(.Item("Name"), "")
            mDescription = Catchnull(.Item("Description"), "")

        End With 

    End Sub 

#region "Save" 

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand) 

        db.AddInParameter(cmd, "@CommunityID", DBType.Int32, mCommunityID) 
        db.AddInParameter(cmd, "@WardID", DBType.Int32, mWardID) 
        db.AddInParameter(cmd, "@NoOfHouseholds", DBType.Int32, mNoOfHouseholds) 
        db.AddInParameter(cmd, "@NoOfIndividualAdultMales", DBType.Int32, mNoOfIndividualAdultMales) 
        db.AddInParameter(cmd, "@NoOfIndividualAdultFemales", DBType.Int32, mNoOfIndividualAdultFemales) 
        db.AddInParameter(cmd, "@NoOfMaleYouths", DBType.Int32, mNoOfMaleYouths) 
        db.AddInParameter(cmd, "@NoOfFemaleYouth", DBType.Int32, mNoOfFemaleYouth) 
        db.AddInParameter(cmd, "@NoOfChildren", DBType.Int32, mNoOfChildren) 
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID) 
        db.AddInParameter(cmd, "@Name", DBType.String, mName) 
        db.AddInParameter(cmd, "@Description", DBType.String, mDescription) 

    End Sub 

Public Overridable Function Save() As Boolean 

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Community") 

        GenerateSaveParameters(db, cmd)

        Try 

            Dim ds As DataSet = db.ExecuteDataSet(cmd) 

            If ds isnot nothing andalso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then 

                mCommunityID = ds.Tables(0).Rows(0)(0) 

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

        'Return Delete("UPDATE tblCommunities SET Deleted = 1 WHERE CommunityID = " & mCommunityID) 
        Return Delete("DELETE FROM tblCommunities WHERE CommunityID = " & mCommunityID) 

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