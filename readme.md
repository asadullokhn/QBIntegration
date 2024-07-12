# QuickBooks Integration

## Description

This application integrates with QuickBooks to retrieve and display data about Company, Invoice, ItemSales, Bill, Check, and Customers as a REST API.

## Installation

1. **Unzip the `QBIntegration.zip` file**.
2. **Install QuickBooks SDK** following the instructions in the SDK documentation.
3. **Open the solution in Visual Studio**.

## Setup

1. **Add references to QuickBooks SDK libraries**:
   - Right-click on the project in Solution Explorer and select `Add > Reference`.
   - Browse to the QuickBooks SDK installation folder and add the necessary DLL files.

2. **Copy the XML query files** to a folder in your project:
   - `CompanyQueryRq.xml`
   - `InvoiceQueryRq.xml`
   - `ItemSalesTaxQueryRq.xml`
   - `BillQueryRq.xml`
   - `CheckQueryRq.xml`
   
   Place these files in a folder such as `C:\Users\User\QBIntegration\Queries`.

## Configuration

1. **Ensure the path in the `QuickBooksService` class matches the location of your XML query files**:
   
   ```vb
    Private ReadOnly QueriesDirectory As String = "C:\Users\User\QBIntegration\Queries"
   ```


`Do not forget to open QuickBooks and give permission to the App.`