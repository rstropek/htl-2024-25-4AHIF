# Data Importer

## Description

In this exercise, you have to write an importer for a simple data format.

## Task 1: Create Console Application

1. Create a .NET console application.
2. Add the necessary NuGet packages for Entity Framework Core (with SQLite support).
3. Add the following data model classes:

    ```csharp
    public class Customer(int id, string companyName, string countryIsoCode, string region)
    {
        Customer() : this(0, "", "", "") { }

        public int ID { get; set; } = id;
        public string CompanyName { get; set; } = companyName;
        public string CountryIsoCode { get; set; } = countryIsoCode;
        public string Region { get; set; } = region;
        public List<OrderHeader> Orders { get; set; } = [];
        public int? ParentCustomerID { get; set; } = null;
        public Customer? ParentCustomer { get; set; } = null;
    }

    public class OrderHeader(int id, int customerID, Customer customer, DateOnly orderDate, string deliveryCountryIsoCode, string incoterm, string paymentTerms)
    {
        OrderHeader() : this(0, 0, null!, new(), "", "", "") { }

        public int ID { get; set; } = id;
        public int CustomerID { get; set; } = customerID;
        public Customer Customer { get; set; } = customer;
        public DateOnly OrderDate { get; set; } = orderDate;
        public string DeliveryCountryIsoCode { get; set; } = deliveryCountryIsoCode;
        public string Incoterm { get; set; } = incoterm;
        public string PaymentTerms { get; set; } = paymentTerms;
        public List<OrderLine> OrderLines { get; set; } = [];
    }

    public class OrderLine(int id, int orderID, OrderHeader order, string productCode, int quantity, decimal unitPrice)
    {
        OrderLine() : this(0, 0, null!, "", 0, 0) { }

        public int ID { get; set; } = id;
        public int OrderID { get; set; } = orderID;
        public OrderHeader Order { get; set; } = order;
        public string ProductCode { get; set; } = productCode;
        public int Quantity { get; set; } = quantity;
        public decimal UnitPrice { get; set; } = unitPrice;
    }
    ```

4. Add a `DbContext` class with the tables `Customers`, `OrderHeaders`, and `OrderLines`. Make sure to add the following code to the data context. It ensures that `CountryIsoCode` has a length of two.

    ```cs
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderLine>()
            .Property(ol => ol.UnitPrice)
            .HasConversion<double>();

        modelBuilder.Entity<Customer>()
            .ToTable(c => c.HasCheckConstraint("CK_CountryIsoCode", "length(CountryIsoCode) = 2"));
    }
    ``` 

5. Add an `IDesignTimeDbContextFactory` implementation for the `DbContext` class.
6. Setup the database connection in the `appsettings.json` file.
7. Add migrations and update the database.

Use the [_LinqEntityFramework_ project](https://github.com/rstropek/htl-2024-25-4AHIF/tree/main/homeworks/LinqEntityFramework) as a reference.

## Task 2: Write a Parser

The [import file](./data.txt) format is a custom, segment-based flat file designed to represent hierarchical data without explicit primary or foreign keys. Each line in the file represents a record, and fields within each record are separated by a vertical bar ("|"). The first field of every line is a segment identifier that indicates the type of record:

- **CUS**: Represents a customer record. This segment includes the customer’s company name, country ISO code, and region.
- **OH**: Represents an order header. This segment contains details such as the order date, delivery country ISO code, incoterm, and payment terms.
- **OL**: Represents an order line. This segment holds information about a specific product, including the product code, quantity, and unit price.

The hierarchical relationships between customers, orders, and order lines are implied by the order in which the records appear in the file. Each customer record is followed by its related order header records, and each order header record is immediately followed by its corresponding order line records. This sequential ordering allows for the association of orders and order lines with the correct customer, without the need for explicit keys.

Write a parser that reads the [import file](./data.txt) and creates objects in memory for customers, orders, and order lines.

## Task 3: Import Data

Write an importer that populates the database with the data read by the parser.

Before inserting any data, the importer must **delete all existing data** from the database.

**Importing data for each customer must be done in a single transaction.** If an error occurs during the import process of a certain customer, the transaction should be rolled back, but the import process should continue with the next customer.
