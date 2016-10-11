Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ExternalClientsService
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetTrainingBlocks() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Description FROM luBlock").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetTrainings() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Name FROM tblTrainings").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetGroupTypes() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Description FROM luHealthGroupTypes").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetDepartments() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Description FROM luDepartments").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetPeriods() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Description FROM luPeriod").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetStaffPositions() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Description FROM luStaffPosition").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetDistricts() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Name FROM tblDistricts").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetWards() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT DISTINCT Name FROM tblWards").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetVillages() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT DISTINCT Name FROM tblVillages").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetProvinces() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Name FROM tblProvinces").Tables(0)

    End Function

    <WebMethod()>
    Public Function GetHealthCenters() As DataTable

        Dim objData As New BusinessLogic.Training(CookiesWrapper.thisConnectionName, CookiesWrapper.thisUserID)

        Return objData.GetTraining("SELECT Name FROM tblHealthCenters").Tables(0)

    End Function

End Class