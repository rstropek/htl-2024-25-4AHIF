List<Customer> customers = [
    new("John", "Doe"),
    new("Jane", "Doe")
];

foreach (var customer in customers)
{
    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
}

foreach (var n in GetNumbers()
    .Skip(5)
    .Take(10)
    .Select(i => i * 2))
{
    Console.WriteLine(n);
}

static IEnumerable<int> GetNumbers()
{
    var i = 0;
    while (true)
    {
        yield return i++;
    }
}

record Customer(string FirstName, string LastName);