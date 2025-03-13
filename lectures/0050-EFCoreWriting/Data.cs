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
