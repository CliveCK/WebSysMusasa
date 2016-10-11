Imports System.Web.Routing
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure()
        'BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub
End Class