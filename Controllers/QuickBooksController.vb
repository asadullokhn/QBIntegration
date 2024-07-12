Imports Microsoft.AspNetCore.Mvc
Imports QBIntegration.QBIntegration.Models
Imports QBIntegration.QBIntegration.Services

Namespace QBIntegration.Controllers
    Public Class QuickBooksController
        Inherits Controller

        Private ReadOnly _quickBooksService As New QuickBooksService()

        Public Async Function Index() As Task(Of ActionResult)
            Dim company As Company = Await _quickBooksService.GetCompanyAsync()

            Return View(company)
        End Function

    End Class
End Namespace