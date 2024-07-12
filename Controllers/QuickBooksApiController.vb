Imports Microsoft.AspNetCore.Mvc
Imports QBIntegration.QBIntegration.Models
Imports QBIntegration.QBIntegration.Services


Namespace QBIntegration.Controllers
    <ApiController>
    <Route("api/quickbooks")>
    Public Class QuickBooksApiController
        Inherits ControllerBase

        Private ReadOnly _quickBooksService As New QuickBooksService()

        <HttpGet>
        <Route("about-company")>
        Public Async Function GetAboutCompany() As Task(Of IActionResult)
            Try
                Dim companyData As Company = Await _quickBooksService.GetCompanyAsync()
                Return Ok(companyData)
            Catch ex As Exception
                Return StatusCode(500, ex.Message)
            End Try
        End Function

        <HttpGet>
        <Route("invoices")>
        Public Async Function GetInvoices() As Task(Of IActionResult)
            Try
                Dim invoiceData As List(Of Invoice) = Await _quickBooksService.GetInvoicesAsync()
                Return Ok(invoiceData)
            Catch ex As Exception
                Return StatusCode(500, ex.Message)
            End Try
        End Function

        <HttpGet>
        <Route("item-sales")>
        Public Async Function GetItemSales() As Task(Of IActionResult)
            Try
                Dim itemSalesData As List(Of ItemSales) = Await _quickBooksService.GetItemSalesAsync()
                Return Ok(itemSalesData)
            Catch ex As Exception
                Return StatusCode(500, ex.Message)
            End Try
        End Function

        <HttpGet>
        <Route("bills")>
        Public Async Function GetBills() As Task(Of IActionResult)
            Try
                Dim billData As List(Of Bill) = Await _quickBooksService.GetBillsAsync()
                Return Ok(billData)
            Catch ex As Exception
                Return StatusCode(500, ex.Message)
            End Try
        End Function

        <HttpGet>
        <Route("checks")>
        Public Async Function GetChecks() As Task(Of IActionResult)
            Try
                Dim checkData As List(Of Check) = Await _quickBooksService.GetChecksAsync()
                Return Ok(checkData)
            Catch ex As Exception
                Return StatusCode(500, ex.Message)
            End Try
        End Function
    End Class
End Namespace