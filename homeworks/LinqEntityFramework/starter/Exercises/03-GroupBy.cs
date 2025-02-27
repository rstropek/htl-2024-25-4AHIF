namespace Exercises;

public class GroupBy
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
    public async Task<OrdersGroupedByMonthNumberOfOrdersResult[]> OrdersGroupedByMonthNumberOfOrders()
    {
        throw new NotImplementedException();
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
    public async Task<TotalOrderValueByCustomerResult[]> TotalOrderValueByCustomer()
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
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public async Task<OrderStatsByCustomerResult[]> OrderStatsByCustomer()
    {
        throw new NotImplementedException();
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
    public async Task<OrderStatsByCustomerResult[]> OrderStatsByAllCustomer()
    {
        throw new NotImplementedException();
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
    public async Task<OrderStatsByCustomerResult[]> OrderStatsByLargeCustomer()
    {
        throw new NotImplementedException();
    }
}