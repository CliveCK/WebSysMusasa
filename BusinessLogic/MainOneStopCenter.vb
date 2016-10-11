﻿Imports Microsoft.Practices.EnterpriseLibrary.Data 
Imports Universal.CommonFunctions 

Public Class MainOneStopCenter
#region "Variables"

    Protected mMainOneStopCenterID As long
    Protected mCenterNameID As long
    Protected mYear As long
    Protected mMonth As long
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

    Public  Property MainOneStopCenterID() As long
        Get
		return mMainOneStopCenterID
        End Get
        Set(ByVal value As long)
		mMainOneStopCenterID = value
        End Set
    End Property

    Public  Property CenterNameID() As long
        Get
		return mCenterNameID
        End Get
        Set(ByVal value As long)
		mCenterNameID = value
        End Set
    End Property

    Public  Property Year() As long
        Get
		return mYear
        End Get
        Set(ByVal value As long)
		mYear = value
        End Set
    End Property

    Public  Property Month() As long
        Get
		return mMonth
        End Get
        Set(ByVal value As long)
		mMonth = value
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

    MainOneStopCenterID = 0
    mCenterNameID = 0
    mYear = 0
    mMonth = 0
    mCreatedBy = mObjectUserID
    mUpdatedBy = 0
    mCreatedDate = ""
    mUpdatedDate = ""

End Sub

#Region "Retrieve Overloads" 

    Public Overridable Function Retrieve() As Boolean 

        Return Me.Retrieve(mMainOneStopCenterID) 

    End Function 

    Public Overridable Function Retrieve(ByVal MainOneStopCenterID As Long) As Boolean 

        Dim sql As String 

        If MainOneStopCenterID > 0 Then 
            sql = "SELECT * FROM tblMainOneStopCenter WHERE MainOneStopCenterID = " & MainOneStopCenterID
        Else 
            sql = "SELECT * FROM tblMainOneStopCenter WHERE MainOneStopCenterID = " & mMainOneStopCenterID
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

                log.Error("MainOneStopCenter not found.")

                Return False

            End If

        Catch e As Exception

            log.Error(e)
            Return False

        End Try

    End Function

    Public Function GetAllMainOneStop() As DataSet

        Dim sql As String = "Select C.*, G.GroupName As CenterName, C.Year, C.Month, W.Name As Ward, D.Name as District, B.FirstName, B.Surname, B.NationlIDNo, "
        sql &= "B.Sex, B.DateOfBirth As DOB "
        sql &= "From tblMainOneStopCenter C  "
        sql &= "Left outer join tblMainOneStopCenterMember M1 on C.MainOneStopCenterID = M1.MainOneStopCenterID "
        sql &= "Left outer join tblOneStopCenterMemberNeedsServices S on S.MainOneStopCenterID = C.MainOneStopCenterID "
        sql &= "Left outer join tblBeneficiaries B on B.BeneficiaryID = M1.BeneficiaryID "
        sql &= "Left outer join tblGroups G on G.GroupID = C.CenterNameID "
        sql &= " Left outer join tblAddresses A on A.OwnerID = M1.BeneficiaryID "
        sql &= "Left outer join tblDistricts D on D.DistrictID = A.DistrictID "
        sql &= "Left outer join tblWards W on W.WardID = A.WardID "

        Return GetMainOneStopCenter(sql)

    End Function

    Public Overridable Function GetMainOneStopCenter() As System.Data.DataSet

        Return GetMainOneStopCenter(mMainOneStopCenterID)

    End Function

    Public Overridable Function GetMainOneStopCenter(ByVal MainOneStopCenterID As Long) As DataSet

        Dim sql As String

        If MainOneStopCenterID > 0 Then
            sql = "SELECT * FROM tblMainOneStopCenter WHERE MainOneStopCenterID = " & MainOneStopCenterID
        Else
            sql = "SELECT * FROM tblMainOneStopCenter WHERE MainOneStopCenterID = " & mMainOneStopCenterID
        End If

        Return GetMainOneStopCenter(sql)

    End Function

    Protected Overridable Function GetMainOneStopCenter(ByVal sql As String) As DataSet

        Return db.ExecuteDataSet(CommandType.Text, sql)

    End Function

#End Region

    Protected Friend Overridable Sub LoadDataRecord(ByRef Record As Object)

        With Record

            mMainOneStopCenterID = Catchnull(.Item("MainOneStopCenterID"), 0)
            mCenterNameID = Catchnull(.Item("CenterNameID"), 0)
            mYear = Catchnull(.Item("Year"), 0)
            mMonth = Catchnull(.Item("Month"), 0)
            mCreatedBy = Catchnull(.Item("CreatedBy"), 0)
            mUpdatedBy = Catchnull(.Item("UpdatedBy"), 0)
            mCreatedDate = Catchnull(.Item("CreatedDate"), "")
            mUpdatedDate = Catchnull(.Item("UpdatedDate"), "")

        End With

    End Sub

#Region "Save"

    Public Overridable Sub GenerateSaveParameters(ByRef db As Database, ByRef cmd As System.Data.Common.DbCommand)

        db.AddInParameter(cmd, "@MainOneStopCenterID", DbType.Int32, mMainOneStopCenterID)
        db.AddInParameter(cmd, "@CenterNameID", DbType.Int32, mCenterNameID)
        db.AddInParameter(cmd, "@Year", DbType.Int32, mYear)
        db.AddInParameter(cmd, "@Month", DbType.Int32, mMonth)
        db.AddInParameter(cmd, "@UpdatedBy", DbType.Int32, mObjectUserID)

    End Sub

    Public Overridable Function Save() As Boolean

        Dim cmd As System.Data.Common.DbCommand = db.GetStoredProcCommand("sp_Save_MainOneStopCenter")

        GenerateSaveParameters(db, cmd)

        Try

            Dim ds As DataSet = db.ExecuteDataSet(cmd)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                mMainOneStopCenterID = ds.Tables(0).Rows(0)(0)

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

        'Return Delete("UPDATE tblMainOneStopCenter SET Deleted = 1 WHERE MainOneStopCenterID = " & mMainOneStopCenterID) 
        Return Delete("DELETE FROM tblMainOneStopCenter WHERE MainOneStopCenterID = " & mMainOneStopCenterID)

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