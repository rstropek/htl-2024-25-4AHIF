namespace Exercises;

public static class GroupBy
{
    public record OrdersGroupedByMonthNumberOfOrdersResult(DateOnly FirstOfMonth, int NumberOfOrders);

    /// <summary>
    /// Returns the number of orders placed each month.
    /// </summary>
    /// <remarks>
    /// The result must only contain months with orders.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// </remarks>
    public static IEnumerable<OrdersGroupedByMonthNumberOfOrdersResult> OrdersGroupedByMonthNumberOfOrders()
    {
        return SampleData.OrderHeaders
            .GroupBy(oh => new DateOnly(oh.OrderDate.Year, oh.OrderDate.Month, 1))
            .Select(g => new OrdersGroupedByMonthNumberOfOrdersResult(g.Key, g.Count()));
    }

    public record TotalOrderValueByCustomerResult(int CompanyID, string CompanyName, decimal TotalOrderValue);

    /// <summary>
    /// Returns the total order value for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with orders.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// </remarks>
    public static IEnumerable<TotalOrderValueByCustomerResult> TotalOrderValueByCustomer()
    {
        return SampleData.OrderHeaders
            .GroupBy(oh => oh.Customer)
            .Select(g => new TotalOrderValueByCustomerResult(
                g.Key.ID,
                g.Key.CompanyName,
                g.Sum(oh => oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice))));
    }

    public record OrderStatsCustomer(int CompanyID, string CompanyName);
    public record OrderStatsByCustomerResult(OrderStatsCustomer Customer, decimal AvgOrderValue, decimal TotalOrderValue, int NumberOfOrders);

    /// <summary>
    /// Returns the average order value, total order value, and number of orders for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with orders.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByCustomer()
    {
        return SampleData.OrderHeaders
            .GroupBy(oh => new OrderStatsCustomer(oh.Customer.ID, oh.Customer.CompanyName))
            .Select(g => new OrderStatsByCustomerResult(
                g.Key,
                g.Average(oh => oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)),
                g.Sum(oh => oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)),
                g.Count()));
    }

    /// <summary>
    /// Same as <see cref="OrderStatsByCustomer"/>, but includes **all** customers 
    /// </summary>
    /// <remarks>
    /// Note: You can solve this exercise with or without grouping.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.defaultifempty
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByAllCustomer()
    {
        return SampleData.Customers
            .Select(c => new OrderStatsByCustomerResult
            (
                new OrderStatsCustomer(c.ID, c.CompanyName),
                c.Orders.Select(o => o.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)).DefaultIfEmpty().Average(),
                c.Orders.Select(o => o.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)).Sum(),
                c.Orders.Count
            ));
    }

    /// <summary>
    /// Returns the average order value, total order value, and number of orders for each customer.
    /// </summary>
    /// <remarks>
    /// The result must only contain customers with a total order value of $60,000 or more.
    /// Tip: Do NOT call the exising <see cref="OrderStatsByCustomer"/> method. Practice LINQ by rewriting the query from scratch.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public static IEnumerable<OrderStatsByCustomerResult> OrderStatsByLargeCustomer()
    {
        return SampleData.OrderHeaders
            .GroupBy(oh => new OrderStatsCustomer(oh.Customer.ID, oh.Customer.CompanyName))
            .Select(g => new OrderStatsByCustomerResult(
                g.Key,
                g.Average(oh => oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)),
                g.Sum(oh => oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)),
                g.Count()))
            .Where(r => r.TotalOrderValue >= 60000);
    }
}