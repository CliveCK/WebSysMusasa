Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class DocumentObjects

#region "Variables"

    Protected mDocumentObjectID As long
    Protected mDocumentID As long
    Protected mObjectID As long
    Protected mObjectTypeID As long
    Protected mCreatedBy As long
    Protected mUpdatedBy As long
    Protected mCreatedDate As string
    Protected mUpdatedDate As string

    Protected db As Database 
    Protected mConnectionName As String 
    Protected mObjectUserID As Long
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

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

    Public  Property DocumentObjectID() As long
        Get
		return mDocumentObjectID
        End Get
        Set(ByVal value As long)
		mDocumentObjectID = value
        End Set
    End Property

    Public  Property DocumentID() As long
        Get
		return mDocumentID
        End Get
        Set(ByVal value As long)
		mDocumentID = value
        End Set
    End Property

    Public  Property ObjectID() As long
        Get
		return mObjectID
        End Get
        Set(ByVal value As long)
		mObjectID = value
        End Set
    End Property

    Public  Property ObjectTypeID() As long
        Get
		return mObjectTypeID
        End Get
        Set(ByVal value As long)
		mObjectTypeID = value
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

    DocumentObjectID = 0
    mDocumentID = 0
    mObjectID = 0
    mObjectTypeID = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mDocumentObjectID) 

    End Function 

    Public Overridable Function Retrieve(ByVal DocumentObjectID As Long) As Boolean 

        Dim sql As String 

        If DocumentObjectID > 0 Then 
            sql = "SELECT * FROM tblDocumentObjects WHERE DocumentObjectID = " & DocumentObjectID
        Else 
            sql = "SELECT * FROM tblDocumentObjects WHERE DocumentObjectID = " & mDocumentObjectID
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

                log.Warn("DocumentObjects not found.")

                Return False 

            End If 

        Catch e As Exception 

            Log.Error(e)
            Return False

        End Try

    End Function

    Public Overridable Function GetDocumentObjects() As System.Data.DataSet

        Return GetDocumentObjects(mDocumentObjectID)

    End Function

    Public Overridable Function GetDocumentObjects(ByVal DocumentObjectID As Long) As DataSet

        Dim sql As String

        If DocumentObjectID > 0 Then
            sql = "SELECT * FROM tblDocumentObjects WHERE DocumentObjectID = " & DocumentObjectID
        Else
            sql = "SELECT * FROM tblDocumentObjects WHERE DocumentObjectID = " & mDocumentObjectID
        End If

        Return GetDocumentObjects(sql)

    End Function

    Public Overridable Function GetDocumentObjects(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

    Public Function GetObjectTypeIDByDescription(ByVal Descr As String) As Long

        Dim sql As String

        sql = "SELECT ObjectTypeID FROM luObjectTypes WHERE Description = '" & Descr & "'"

        Return Catchnull(GetDocumentObjects(sql).Tables(0).Rows(0)(0), 0)

    End Function

    Public Function GetAvailableObjects(ByVal sql As String, ByVal ObjectTypeID As Long, ByVal FileID As Long)

        Dim sqlExempt As String = "Select ObjectID FROM tblFiles F inner join tblDocumentObjects O On F.FileID = O.DocumentID inner join luObjectTypes OT "
        sqlExempt &= " On OT.ObjectTypeID = O.ObjectTypeID where O.ObjectTypeID = " & ObjectTypeID & " And O.DocumentID = '" & FileID & "'"

        Dim ExecSql As String = "WITH Temp AS ( " & sql & ") SELECT * FROM Temp WHERE ObjectID not in ( " & sqlExempt & ")"

        Return db.ExecuteDataSet(CommandType.Text, ExecSql)

    End Function

    Public Function GetMappedObjects(ByVal sql As String, ByVal ObjectTypeID As Long, ByVal FileID As Long)

        Dim sqlExempt As String = "SELECT ObjectID FROM tblFiles F inner join tblDocumentObjects O on F.FileID = O.DocumentID inner join luObjectTypes OT "
        sqlExempt &= " on OT.ObjectTypeID = O.ObjectTypeID where O.ObjectTypeID = " & ObjectTypeID & " AND O.DocumentID = '" & FileID & "'"

        Dim ExecSql As String = "WITH Temp AS ( " & sql & ") SELECT * FROM Temp WHERE ObjectID in ( " & sqlExempt & ")"

        Return db.ExecuteDataSet(CommandType.Text, ExecSql)

    End Function

    Public Function CheckIsMapped(ByVal FileID As Long) As Boolean

        Try

            Dim sql As String = "SELECT ObjectID FROM tblFiles F inner join tblDocumentObjects O on F.FileID = O.DocumentID inner join luObjectTypes OT "
            sql &= " on OT.ObjectTypeID = O.ObjectTypeID where O.DocumentID = '" & FileID & "'"

            Dim ds As DataSet = db.ExecuteDataSet(CommandType.Text, sql)

            If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                Return True

            Else

                Return False

            End If

        Catch ex As Exception

            Return False

        End Try

    End Function


#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mDocumentObjectID = Catchnull(.Item("DocumentObjectID"), 0)
            mDocumentID = Catchnull(.Item("DocumentID"), 0)
            mObjectID = Catchnull(.Item("ObjectID"), 0)
            mObjectTypeID = Catchnull(.Item("ObjectTypeID"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@DocumentObjectID", DBType.Int32, mDocumentObjectID)
        db.AddInParameter(cmd, "@DocumentID", DBType.Int32, mDocumentID)
        db.AddInParameter(cmd, "@ObjectID", DBType.Int32, mObjectID)
        db.AddInParameter(cmd, "@ObjectTypeID", DBType.Int32, mObjectTypeID)
        db.AddInParameter(cmd, "@UpdatedBy", DBType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_DocumentObjects")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mDocumentObjectID = ds.Tables(0).Rows(0)(0)

            End If

            Return True

        Catch ex As Exception

            Log.Error(ex)
            Return False

        End Try

    End Function

    Public Function SaveDetail() As Boolean

        Dim InsertSQL As String = "INSERT INTO tblDocumentObjects ([ObjectID], [DocumentID],[ObjectTypeID],[CreatedDate], [CreatedBy]) VALUES (" & mObjectID & "," & mDocumentID & "," & mObjectTypeID & "," & Now & "," & mObjectUserID & ")"

        db.ExecuteNonQuery(CommandType.Text, InsertSQL)

        Return True

    End Function

#End Region

#Region "Delete"

    Public Function DeleteEntries() As Boolean

        Dim sql As String = " DELETE FROM tblDocumentObjects WHERE ObjectID = " & mObjectID & " AND ObjectTypeID = " & mObjectTypeID & " AND DocumentID = " & mDocumentID

        Return Delete(sql)

    End Function

    Public Function Revoke() As Boolean

        Dim sql As String = " DELETE FROM tblDocumentObjects WHERE ObjectID = " & mObjectID & " AND ObjectTypeID = " & mObjectTypeID & " AND DocumentID = " & mDocumentID

        Return Delete(sql)

    End Function

    Public Overridable Function Delete() As Boolean

        'Return Delete("UPDATE tblDocumentObjects SET Deleted = 1 WHERE DocumentObjectID = " & mDocumentObjectID) 
        Return Delete("DELETE FROM tblDocumentObjects WHERE DocumentObjectID = " & mDocumentObjectID)

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