# Registration API Example

## Introduction

This example demonstrates some good practices for building a web API with .NET 9.

## _.editorconfig_

_.editorconfig_ is a configuration file used to standardize coding styles across different editors and IDEs. It allows developers to define consistent settings, such as indentation size, line endings, and character encodings, within a project. By placing a `.editorconfig` file in the root of a project, contributors can ensure that their code adheres to the same formatting rules, regardless of their preferred editor. The file is simple, consisting of sections with key-value pairs that specify the desired settings for different file types. This helps streamline collaboration and maintain a clean, uniform codebase.

C#-specific formatting options can be found at [https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/csharp-formatting-options](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/csharp-formatting-options).

**TODO:** This project contains a sample [_.editorconfig_](./.editorconfig) file. Try to change one of the settings, auto-format your code, and see how it affects the formatting of your code.

## General Throughts on Structuring Your Project

### API Structure

As long as your API contains only a few endpoints and their business logic is simple, you can place all of your code in _Program.cs_. However, in real life, APIs are larger and more complex. In this case, putting everything in a single file is not good practice.

**Structure your API by features or domain areas instead.** That means organizing your codebase around specific business functionalities or areas of concern rather than by technical layers (e.g., controllers, services, repositories). This structure helps ensure that related components (handlers, models, validators, etc.) are colocated, making the code easier to navigate, maintain, and extend.

This approach is recommended particularly because of the following reasons:

1. **Cohesion:** All files related to a particular feature or domain are located together.
2. **Scalability:** Easier to add or modify features without impacting unrelated parts of the system.
3. **Maintainability:** Developers can quickly find all the code relevant to a specific functionality.
4. **Collaboration:** Teams working on different features can work independently without conflicts.

In this project, the API is structured by business domains:

* [_Admin_](./Registration.Api/Apis/Admin/) deals with administrative tasks (e.g. campaign management)
* [_Public_](./Registration.Api/Apis/Public/) deals with public tasks (e.g. registration)

### Cross-Cutting Concerns

For [cross-cutting concerns](https://en.wikipedia.org/wiki/Cross-cutting_concern) like authentication, logging, validation, exception handling, or caching, you should avoid duplicating functionality across features and **centralize their implementation** to ensure consistency and reusability.

### Business and Data Access Logic

Business and data access logic should be **separated** from the API layer (e.g. separate folder, dedicated project). Example for business and data access logic include:

* Business rules (e.g. validations, calculations, conditional logic)
* Data access logic (e.g. database operations, database queries, map domain model to DB entities)
* Orchestration logic (e.g. orchestrate workflows across different entities)

The API handlers should delegate all logic to the business and/or data access layer. They get reference to the business and/or data access layer classes via **dependency injection**.

The business and data access classes should not have dependencies to ASP.NET Core or any other web API-related libraries. Internally, the business and data access layer should be structured by features and domain areas (similiar to the API structure mentioned above).

## DTOs (Data Transfer Objects)

### Classes vs. Records

Records are a good choice for DTOs because they are immutable, easy to write, and easy to use.

### Input vs. Output DTOs

It is considered good practice to have **separate DTOs for input and output**. This is because:

- Input DTOs only expose fields that users are allowed to set
- Output DTOs can include additional data not present in input
- Prevents over-posting attacks where users could set sensitive fields
- Allows you to hide internal implementation details
- Input and output models can evolve independently
- You can add new response fields without breaking client input
- Easier to maintain backward compatibility
- Can use different annotations for input vs output (e.g., [Required] only makes sense for input)
- API documentation is clearer when it shows exactly what's expected vs what's returned
- If generated, Open API Specification (aka Swagger) becomes more accurate and useful

In this project, we have e.g. [`CreateCampaignRequest` and `CreateCampaignResponse`](./Registration.Api/Apis/Admin/CreateCampaign.cs). The first one is an input DTO and the second one is an output DTO.

### DTO Validation

DTOs provided by callers of an API must be validated before they are processed. For that, you have multiple options:

* Use a validation framework like [FluentValidation](https://docs.fluentvalidation.net/)
* Implement your own validation logic
* Use a combination of both

Do not add validation logic to the DTOs themselves. Instead, implement dedicated validation classes (_Separation of Concerns_ principle).

In this project, we want to focus on the very fundamentals of ASP.NET Core and therefore we try to avoid external dependencies. However, take a look at _FluentValidation_ as it is widely used and a good choice for many projects. In this project, we implement some fundamental helper classes for DTO validation (see [_DtoValidation.cs_](./Registration.Api/CrossCuttingConcerns/DtoValidation.cs)). Examples for how to use them can be found in [_CreateCampaign.cs_](./Registration.Api/Apis/Admin/CreateCampaign.cs).

## How To Design APIs

Web API design shares many similarities with UI design, as both require a focus on the user’s needs. In API design, the "user" is not a person directly interacting with a graphical interface but rather the developer and the software making calls to the API. Just as a well-designed UI anticipates user behavior and provides an intuitive, seamless interaction, a well-structured API ensures clear, efficient, and predictable communication between systems. This involves carefully considering common use cases, minimizing complexity, providing documentation, and error handling.

A critical aspect of modern API design is the concept of Backend-for-Frontend (BFF). This pattern tailors backend services specifically to the needs of a given frontend, ensuring that the API delivers exactly the data and functionality required. By optimizing interactions for specific user experiences, BFFs reduce [over-fetching and under-fetching](https://nordicapis.com/what-are-over-fetching-and-under-fetching/) of data, improving performance and usability.

**Designing APIs with both the "programmatic user" and the end-user experience in mind leads to more robust, developer-friendly systems.**

In this project, we assume that the end user will create a campaign, its dates, and department assigments in one go. The end user will enter all this data and store it with a single click. Therefore, we do not offer separate endpoints for creating a campaign, its dates, and department assigments. Instead, we offer a single endpoint that creates all of them at once ([`CreateCampaign`](./Registration.Api/Apis/Admin/CreateCampaign.cs)).


