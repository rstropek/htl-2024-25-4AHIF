using Microsoft.AspNetCore.Mvc;

Customer[] customers = [
    new(1, "Alice"),
    new(2, "Bob"),
    new(3, "Charlie")
];

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Calculator>();
var app = builder.Build();
app.MapGet("/ping", () => "pong");
app.MapGet("/king", () => "kong");
app.MapPost("/add", ([FromBody] AddDto values, Calculator calc) => calc.Add(values.A, values.B));
app.MapGet("/sub", Sub);
app.MapGet("/customers", () => customers);
app.MapGet("/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    return customer is not null
        ? Results.Ok(customer)
        : Results.NotFound();
});
app.Run();

static int Sub(int a, int b)
{
    return a - b;
}

record AddDto(int A, int B);

record Customer(int Id, string Name);

// Business logic
class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Sub(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}
