Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class TripTravellers

#region "Variables"

    Protected mTripTravellerID As long
    Protected mTripID As long
    Protected mStaffID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

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

    Public  Property TripTravellerID() As long
        Get
		return mTripTravellerID
        End Get
        Set(ByVal value As long)
		mTripTravellerID = value
        End Set
    End Property

    Public  Property TripID() As long
        Get
		return mTripID
        End Get
        Set(ByVal value As long)
		mTripID = value
        End Set
    End Property

    Public  Property StaffID() As long
        Get
		return mStaffID
        End Get
        Set(ByVal value As long)
		mStaffID = value
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

    TripTravellerID = 0
    mTripID = 0
    mStaffID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTripTravellerID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TripTravellerID As Long) As Boolean 

        Dim sql As String 

        If TripTravellerID > 0 Then 
            sql = "SELECT * FROM tblTripTravellers WHERE TripTravellerID = " & TripTravellerID
        Else 
            sql = "SELECT * FROM tblTripTravellers WHERE TripTravellerID = " & mTripTravellerID
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

                log.Error("TripTravellers not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTripTravellers() As System.Data.DataSet

        Return GetTripTravellers(mTripTravellerID)

    End Function

    Public Overridable Function GetTripTravellers(ByVal TripTravellerID As Long) As DataSet

        Dim sql As String

        If TripTravellerID > 0 Then
            sql = "SELECT * FROM tblTripTravellers WHERE TripTravellerID = " & TripTravellerID
        Else
            sql = "SELECT * FROM tblTripTravellers WHERE TripTravellerID = " & mTripTravellerID
        End If

        Return GetTripTravellers(sql)

    End Function

    Protected Overridable Function GetTripTravellers(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTripTravellerID = Catchnull(.Item("TripTravellerID"), 0)
            mTripID = Catchnull(.Item("TripID"), 0)
            mStaffID = Catchnull(.Item("StaffID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TripTravellerID", DBType.Int32, mTripTravellerID)
        db.AddInParameter(cmd, "@TripID", DBType.Int32, mTripID)
        db.AddInParameter(cmd, "@StaffID", DBType.Int32, mStaffID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_TripTravellers")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTripTravellerID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTripTravellers SET Deleted = 1 WHERE TripTravellerID = " & mTripTravellerID) 
        Return Delete("DELETE FROM tblTripTravellers WHERE TripTravellerID = " & mTripTravellerID)

    End Function

    Public Function DeleteEntries() As Boolean

        Return Delete("DELETE FROM tblTripTravellers WHERE TripID = " & mTripID & " AND StaffID = " & mStaffID)

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