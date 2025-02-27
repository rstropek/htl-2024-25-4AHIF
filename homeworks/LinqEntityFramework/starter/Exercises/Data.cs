namespace Exercises;

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

public static class SampleData
{
    public static readonly Customer[] Customers;
    public static readonly List<OrderHeader> OrderHeaders = [];
    public static readonly List<OrderLine> OrderLines = [];

    static SampleData()
    {
        var acme = new Customer(1, "Acme Corp", "US", "California");
        var globex = new Customer(2, "Globex Corp", "DE", "Nordrhein-Westfalen");
        var initech = new Customer(3, "Initech", "MEX", "Yucatán");
        var soylent = new Customer(4, "Soylent Corp", "JP", "東京都");
        var umbrella = new Customer(5, "Umbrella Corp", "CHE", "Zürich");
        Customers = [acme, globex, initech, soylent, umbrella];

        globex.ParentCustomer = acme;
        initech.ParentCustomer = acme;
        acme.ParentCustomer = umbrella;

        var orderHeaderID = 1;
        var orderLineID = 1;

        // acme orders ==================================================================================
        var oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 1, 10), "US", "EXW", "Due on receipt");
        var line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732", 1, 4990);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732-A", 10, 34.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732-B", 5, 199m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 2, 15), "CAN", "FOB", "Net 30");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733", 2, 3499.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-X", 15, 89.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 4, 5), "US", "CIF", "Net 45");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734", 3, 2999.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-PRO", 8, 449.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-LITE", 20, 149.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 6, 22), "US", "DDP", "Due on receipt");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3735", 1, 7999.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3735-ACC", 5, 299.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 9, 8), "MEX", "EXW", "Net 60");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3736", 4, 1999.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3736-KIT", 12, 179.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 11, 30), "CAN", "FCA", "Net 30");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3737", 2, 5499.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3737-BUNDLE", 3, 899.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3737-BASIC", 10, 249.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, acme.ID, acme, new DateOnly(2024, 12, 15), "US", "", "");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734", 1, -2999.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        acme.Orders.Add(oh);
        OrderHeaders.Add(oh);

        // globex orders ==================================================================================
        oh = new OrderHeader(orderHeaderID++, globex.ID, globex, new DateOnly(2024, 1, 5), "DE", "CIF", "Net 30");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732", 5, 4890);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732-A", 3, 33.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        globex.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, globex.ID, globex, new DateOnly(2024, 3, 15), "DE", "DDP", "Net 45");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733", 10, 3350);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-X", 20, 89.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        globex.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, globex.ID, globex, new DateOnly(2024, 7, 1), "AT", "FOB", "Due on receipt");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734", 2, 2750);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-PRO", 5, 499.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-LITE", 15, 139);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        globex.Orders.Add(oh);
        OrderHeaders.Add(oh);

        // initech orders ==================================================================================
        oh = new OrderHeader(orderHeaderID++, initech.ID, initech, new DateOnly(2024, 2, 20), "MEX", "EXW", "Net 30");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732", 3, 4795);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732-A", 6, 44.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        initech.Orders.Add(oh);
        OrderHeaders.Add(oh);   

        oh = new OrderHeader(orderHeaderID++, initech.ID, initech, new DateOnly(2024, 5, 10), "US", "FCA", "Net 60");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733", 8, 4499.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-X", 4, 89.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-Y", 12, 19.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        initech.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, initech.ID, initech, new DateOnly(2024, 10, 15), "MEX", "CIF", "Due on receipt");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734", 1, 2999.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-PRO", 3, 449.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        initech.Orders.Add(oh);
        OrderHeaders.Add(oh);

        // soylent orders ==================================================================================
        oh = new OrderHeader(orderHeaderID++, soylent.ID, soylent, new DateOnly(2024, 4, 1), "JP", "DDP", "Net 45");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732", 4, 4999);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3732-A", 8, 34.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        soylent.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, soylent.ID, soylent, new DateOnly(2024, 8, 20), "JP", "FOB", "Net 30");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733", 2, 3999);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-X", 5, 88.50m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3733-Y", 10, 21.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        soylent.Orders.Add(oh);
        OrderHeaders.Add(oh);

        oh = new OrderHeader(orderHeaderID++, soylent.ID, soylent, new DateOnly(2024, 12, 1), "JP", "EXW", "Due on receipt");
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734", 1, 2875);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        line = new OrderLine(orderLineID++, oh.ID, oh, "VW3734-PRO", 4, 489.99m);
        oh.OrderLines.Add(line);
        OrderLines.Add(line);
        soylent.Orders.Add(oh);
        OrderHeaders.Add(oh);
    }
}
