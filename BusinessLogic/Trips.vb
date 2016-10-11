Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class Trips

#region "Variables"

    Protected mTripID As Long
    Protected mProjectID As Long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mFromDate As string
    Protected mToDateDate As string
    Protected mCreatedDate As string
    Protected mUpdatedDate As string
    Protected mPurpose As string
    Protected mTravellingFrom As string
    Protected mTravellingTo As string

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

    Public  Property TripID() As long
        Get
		return mTripID
        End Get
        Set(ByVal value As long)
		mTripID = value
        End Set
    End Property

    Public Property ProjectID() As Long
        Get
            Return mProjectID
        End Get
        Set(ByVal value As Long)
            mProjectID = value
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

    Public  Property FromDate() As string
        Get
		return mFromDate
        End Get
        Set(ByVal value As string)
		mFromDate = value
        End Set
    End Property

    Public  Property ToDateDate() As string
        Get
		return mToDateDate
        End Get
        Set(ByVal value As string)
		mToDateDate = value
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

    Public  Property Purpose() As string
        Get
		return mPurpose
        End Get
        Set(ByVal value As string)
		mPurpose = value
        End Set
    End Property

    Public  Property TravellingFrom() As string
        Get
		return mTravellingFrom
        End Get
        Set(ByVal value As string)
		mTravellingFrom = value
        End Set
    End Property

    Public  Property TravellingTo() As string
        Get
		return mTravellingTo
        End Get
        Set(ByVal value As string)
		mTravellingTo = value
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

        TripID = 0
        mProjectID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mFromDate = ""
    mToDateDate = ""
    mCreatedDate = ""
    mUpdatedDate = ""
    mPurpose = ""
    mTravellingFrom = ""
    mTravellingTo = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mTripID) 

    End Function 

    Public Overridable Function Retrieve(ByVal TripID As Long) As Boolean 

        Dim sql As String 

        If TripID > 0 Then 
            sql = "SELECT * FROM tblTrips WHERE TripID = " & TripID
        Else 
            sql = "SELECT * FROM tblTrips WHERE TripID = " & mTripID
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

                log.Error("Trips not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetTrips() As System.Data.DataSet

        Return GetTrips(mTripID)

    End Function

    Public Overridable Function GetTrips(ByVal TripID As Long) As DataSet

        Dim sql As String

        If TripID > 0 Then
            sql = "SELECT * FROM tblTrips WHERE TripID = " & TripID
        Else
            sql = "SELECT * FROM tblTrips WHERE TripID = " & mTripID
        End If

        Return GetTrips(sql)

    End Function

    Public Overridable Function GetAllTrips(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Overridable Function GetTrips(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mTripID = Catchnull(.Item("TripID"), 0)
            mProjectID = Catchnull(.Item("ProjectID"), 0)
            mCreatedBy = Catchnull(.Item("Createdy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mFromDate = Catchnull(.Item("FromDate"), "")
            mToDateDate = Catchnull(.Item("ToDate"), "")
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")
            mPurpose = Catchnull(.Item("Purpose"), "")
            mTravellingFrom = Catchnull(.Item("TravellingFrom"), "")
            mTravellingTo = Catchnull(.Item("TravellingTo"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@TripID", DbType.Int32, mTripID)
        db.AddInParameter(cmd, "@ProjectID", DbType.Int32, mProjectID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)
        db.AddInParameter(cmd, "@FromDate", DBType.String, mFromDate)
        db.AddInParameter(cmd, "@ToDate", DbType.String, mToDateDate)
        db.AddInParameter(cmd, "@Purpose", DBType.String, mPurpose)
        db.AddInParameter(cmd, "@TravellingFrom", DBType.String, mTravellingFrom)
        db.AddInParameter(cmd, "@TravellingTo", DBType.String, mTravellingTo)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_Trips")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mTripID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblTrips SET Deleted = 1 WHERE TripID = " & mTripID) 
        Return Delete("DELETE FROM tblTrips WHERE TripID = " & mTripID)

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