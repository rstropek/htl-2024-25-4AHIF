namespace Exercises;

public static class GroupBy
{
    public record OrdersGroupedByMonthNumberOfOrdersResult(DateOnly FirstOfMonth, int NumberOfOrders);

    /// <summary>
    /// Returns the number of orders placed each month.
    /// </summary>
    /// <remarks>
    /// The result must only contain months with orders.
    /// </remarks>
    public static IEnumerable<OrdersGroupedByMonthNumberOfOrdersResult> OrdersGroupedByMonthNumberOfOrders()
    {
        throw new NotImplementedException();
    }

    public record TotalOrderValueByCustomerResult(int CompanyID, string CompanyName, decimal TotalOrderValue);

    /// <summary>
    /// Returns the total order value for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with orders.
    /// </remarks>
    public static IEnumerable<TotalOrderValueByCustomerResult> TotalOrderValueByCustomer()
    {
        throw new NotImplementedException();
    }

    public record OrderStatsCustomer(int CompanyID, string CompanyName);
    public record OrderStatsByCustomerResult(OrderStatsCustomer Customer, decimal AvgOrderValue, decimal TotalOrderValue, int NumberOfOrders);

    /// <summary>
    /// Returns the average order value, total order value, and number of orders for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with orders.
    /// </remarks>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByCustomer()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Same as <see cref="OrderStatsByCustomer"/>, but includes **all** customers 
    /// </summary>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByAllCustomer()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the average order value, total order value, and number of orders for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with a total order value of $60,000 or more.
    /// Tip: Do NOT call the exising OrderStatsByCustomer method. Practice LINQ by rewriting the query from scratch.
    /// </remarks>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByLargeCustomer()
    {
        throw new NotImplementedException();
    }
}