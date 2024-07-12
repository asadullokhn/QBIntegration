Imports System.IO
Imports QBFC16Lib
Imports QBIntegration.QBIntegration.Models

Namespace QBIntegration.Services
    Public Class QuickBooksService
        Private ReadOnly QueriesDirectory As String = "C:\Users\ASUS\source\repos\QBIntegration\Queries"
        Private ReadOnly SessionManager As New QBSessionManager()

        Public Async Function GetCompanyAsync() As Task(Of Company)
            Dim response As IResponse = Await ExecuteQueryAsync("CompanyQueryRq.xml")
            Dim companyRet As ICompanyRet = CType(response.Detail, ICompanyRet)
            Dim company As New Company With {
            .CompanyName = If(companyRet.CompanyName IsNot Nothing, companyRet.CompanyName.GetValue(), String.Empty),
            .LegalName = If(companyRet.LegalCompanyName IsNot Nothing, companyRet.LegalCompanyName.GetValue(), String.Empty),
            .Address = If(companyRet.Address IsNot Nothing AndAlso companyRet.Address.Addr1 IsNot Nothing, companyRet.Address.Addr1.GetValue(), String.Empty),
            .City = If(companyRet.Address IsNot Nothing AndAlso companyRet.Address.City IsNot Nothing, companyRet.Address.City.GetValue(), String.Empty),
            .State = If(companyRet.Address IsNot Nothing AndAlso companyRet.Address.State IsNot Nothing, companyRet.Address.State.GetValue(), String.Empty),
            .PostalCode = If(companyRet.Address IsNot Nothing AndAlso companyRet.Address.PostalCode IsNot Nothing, companyRet.Address.PostalCode.GetValue(), String.Empty),
            .Country = If(companyRet.Address IsNot Nothing AndAlso companyRet.Address.Country IsNot Nothing, companyRet.Address.Country.GetValue(), String.Empty),
            .PhoneNumber = If(companyRet.Phone IsNot Nothing AndAlso companyRet.Phone IsNot Nothing, companyRet.Phone.GetValue(), String.Empty)
        }
            Return company
        End Function

        Public Async Function GetInvoicesAsync() As Task(Of List(Of Invoice))
            Dim response As IResponse = Await ExecuteQueryAsync("InvoiceQueryRq.xml")
            Dim invoiceRetList As IInvoiceRetList = CType(response.Detail, IInvoiceRetList)
            Dim invoices As New List(Of Invoice)

            If invoiceRetList IsNot Nothing Then
                For i As Integer = 0 To invoiceRetList.Count - 1
                    Dim invoiceRet As IInvoiceRet = invoiceRetList.GetAt(i)
                    Dim invoice As New Invoice With {
                    .TxnID = If(invoiceRet.TxnID IsNot Nothing, invoiceRet.TxnID.GetValue(), String.Empty),
                    .Date = If(invoiceRet.TxnDate IsNot Nothing, invoiceRet.TxnDate.GetValue(), Date.MinValue),
                    .CustomerName = If(invoiceRet.CustomerRef IsNot Nothing AndAlso invoiceRet.CustomerRef.FullName IsNot Nothing, invoiceRet.CustomerRef.FullName.GetValue(), String.Empty)
                }
                    invoices.Add(invoice)
                Next
            End If

            Return invoices
        End Function

        Public Async Function GetItemSalesAsync() As Task(Of List(Of ItemSales))
            Dim response As IResponse = Await ExecuteQueryAsync("ItemSalesTaxQueryRq.xml")
            Dim itemSalesRetList As IItemSalesTaxRetList = CType(response.Detail, IItemSalesTaxRetList)
            Dim itemSales As New List(Of ItemSales)

            If itemSalesRetList IsNot Nothing Then
                For i As Integer = 0 To itemSalesRetList.Count - 1
                    Dim itemSalesRet As IItemSalesTaxRet = itemSalesRetList.GetAt(i)
                    Dim item As New ItemSales With {
                    .ItemName = If(itemSalesRet.Name IsNot Nothing, itemSalesRet.Name.GetValue(), String.Empty),
                    .ItemType = If(itemSalesRet.Type IsNot Nothing, itemSalesRet.Type.GetValue().ToString(), String.Empty),
                    .ItemDesciption = If(itemSalesRet.ItemDesc IsNot Nothing, itemSalesRet.ItemDesc.GetValue(), String.Empty)
                }
                    itemSales.Add(item)
                Next
            End If

            Return itemSales
        End Function

        Public Async Function GetBillsAsync() As Task(Of List(Of Bill))
            Dim response As IResponse = Await ExecuteQueryAsync("BillQueryRq.xml")
            Dim billRetList As IBillRetList = CType(response.Detail, IBillRetList)
            Dim bills As New List(Of Bill)

            If billRetList IsNot Nothing Then
                For i As Integer = 0 To billRetList.Count - 1
                    Dim billRet As IBillRet = billRetList.GetAt(i)
                    Dim bill As New Bill With {
                    .TxnID = If(billRet.TxnID IsNot Nothing, billRet.TxnID.GetValue(), String.Empty),
                    .Date = If(billRet.TxnDate IsNot Nothing, billRet.TxnDate.GetValue(), Date.MinValue),
                    .VendorName = If(billRet.VendorRef IsNot Nothing AndAlso billRet.VendorRef.FullName IsNot Nothing, billRet.VendorRef.FullName.GetValue(), String.Empty),
                    .AmountDue = If(billRet.AmountDue IsNot Nothing, billRet.AmountDue.GetValue(), 0)
                }
                    bills.Add(bill)
                Next
            End If

            Return bills
        End Function

        Public Async Function GetChecksAsync() As Task(Of List(Of Check))
            Dim response As IResponse = Await ExecuteQueryAsync("CheckQueryRq.xml")
            Dim checkRetList As ICheckRetList = CType(response.Detail, ICheckRetList)
            Dim checks As New List(Of Check)

            If checkRetList IsNot Nothing Then
                For i As Integer = 0 To checkRetList.Count - 1
                    Dim checkRet As ICheckRet = checkRetList.GetAt(i)
                    Dim check As New Check With {
                    .TxnID = If(checkRet.TxnID IsNot Nothing, checkRet.TxnID.GetValue(), String.Empty),
                    .Date = If(checkRet.TxnDate IsNot Nothing, checkRet.TxnDate.GetValue(), Date.MinValue),
                    .PayeeName = If(checkRet.PayeeEntityRef IsNot Nothing AndAlso checkRet.PayeeEntityRef.FullName IsNot Nothing, checkRet.PayeeEntityRef.FullName.GetValue(), String.Empty),
                    .Amount = If(checkRet.Amount IsNot Nothing, checkRet.Amount.GetValue(), 0)
                }
                    checks.Add(check)
                Next
            End If

            Return checks
        End Function

        Private Async Function ExecuteQueryAsync(queryFileName As String) As Task(Of IResponse)
            Dim response As IResponse

            Try
                Await OpenConnectionAsync()

                Dim queryXml As String = Await File.ReadAllTextAsync(Path.Combine(QueriesDirectory, queryFileName))
                Dim responseMsgSet As IMsgSetResponse = SessionManager.DoRequestsFromXMLString(queryXml)
                response = responseMsgSet.ResponseList.GetAt(0)
            Catch ex As Exception
                Throw
            Finally
                CloseConnection()
            End Try

            Return response
        End Function

        Private Function OpenConnectionAsync() As Task
            Return Task.Run(Sub()
                                SessionManager.OpenConnection2("QBIntegrationApp", "QBIntegrationApp", ENConnectionType.ctLocalQBD)
                                SessionManager.BeginSession(String.Empty, ENOpenMode.omDontCare)
                            End Sub)
        End Function

        Private Sub CloseConnection()
            SessionManager.EndSession()
            SessionManager.CloseConnection()
        End Sub

    End Class

End Namespace