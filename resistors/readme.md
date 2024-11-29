# Resistor Value Calculator

![Hero image](./hero.jpg)

Your task is to create an [ASP.NET Core Minimal Web API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview) that can calculate the value of a resistor based on the color bands.

The functionality of the web API has been specified using *Open API Specification* (OAS, aks *Swagger*): [https://app.swaggerhub.com/apis/RAINER/Resistor-Calculator/1.0.0#/](https://app.swaggerhub.com/apis/RAINER/Resistor-Calculator/1.0.0#/)

The logic to calculate the resistor value from the color bands is demonstrated in the console app [*ResistorValues.Console*](./ResistorValues.Console/).

## Advanced Requirements

* Add logic for unit of measure (ohms, kilo-ohms, mega-ohms).
* Add endpoint for calculating the color bands from a given resistor value.
