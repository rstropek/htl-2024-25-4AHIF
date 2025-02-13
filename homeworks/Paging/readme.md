# API Exercise

## Task 1: Project Setup

* Create a new ASP.NET Core Minimal API Application
* Add a _ping_ endpoint that returns _pong_ when sent a GET request
* Test your application using Postman or any other API testing tool

## Task 2: Research Exercise

* Read the documentation of [Bogus](https://github.com/bchavez/Bogus) and try to understand its working
* Create Model classes:

    ```csharp
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly Birthday { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }

    class Products
    {
        public int Id { get; set; }
        public string Code { get; set; } // Random 10 digit/letter code
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; } // Random price between 100 and 1000
    }
    ```

* Create a new application service (i.e. C# class) called `RandomDataRepository`
* In the constructor of `RandomDataRepository`, generate 1000 random customers and 1000 random products using Bogus. Store the result in private `List<T>` fields.
* For testing purposes, make a read-only property for each list and return the list of customers and products
* Publish the application service using _Dependency Injection_
* Add two endpoints to your API:
    * `/api/customers` - Returns a list of customers
    * `/api/products` - Returns a list of products
* Test your application using Postman or any other API testing tool

## Task 3: Paging

* Add _optional_ paging support to both data endpoint. Paging means that the user can specify:
  * The page number (0 is default and means the first page)
  * The number of items per page (10 is default)
* The endpoints skip the first `page number * itemsPerPage` items and return the next `itemsPerPage` items
* How do you implement the paging function using HTTP mechanisms?
* How can you implement the paging logic so that you can reuse at least parts of it for both endpoints?

## Task 4: Sorting

* The user can optionally specify one sort column and a sort direction (ascending or descending)
* The response must be sortable by all columns
* The paging functionality must still work
* Advanced feature: Allow sorting by multiple columns
