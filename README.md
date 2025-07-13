# **GHD Coding Test: C# .NET Core Product API to Manage Products**

This document outlines the decisions I took to implement the project.

## **Project Requirements**
##### Crud Operations:
- Implement CRUD operations for managing a Product. Product has following fields:
Id, Name, Brand, Price. All these fields are required.
- Include endpoints for creating, reading, updating, and deleting products.
- You are free to use any database in the backend; even an in-memory
implementation is ok.
- Check for duplication when creating a new product. Combination of Name and
Brand defines a unique product.
##### Validation:
- Validate input data for all API endpoints.
- Handle validation errors gracefully and return appropriate error messages.
##### Logging:
- Implement logging for API requests and errors.
- Log important events and errors to console.
##### Unit Tests
- Write appropriate unit tests.
##### Bonus (Optional):
- Implement pagination and filtering for GET endpoints.
- Use Swagger for API documentation.

## **Assumptions**
In a real project I would follow up any questions or assumption with project Stakeholders. However, as this is a technical test I am just highlighting them here for clarity:
- With pagination and filtering we do not require an option to return all data.
- I have created a 'GetAll' Products endpoint. However, I have assumed all other operations are for a single product units.
- I don't know the size of the data in the database, however for the 'GetAll' Endpoint I have implemented it as IQueryable for deferred execution. This will ensure that it is not possible to load all the records from the DB, causing potential performance issues.
- The delete functionality is soft delete rather than removing the record altogether.

## **Design Decisions**
- #### Crud Operations:
  - **Endpoints**:
    * I have included HATEOAS / self links on the endpoints, so when a JSON response is returned, the client has a link they can use to navigate to that resource.
  - **Business Layer**: 
    * I have used the 'MediatR' library to implement Command and Query Handlers:
    * I like this approach as it implements the Single Responsibility and Open Close Principles out of the box, which makes Unit Testing much easier.
    * There is more boiler plate code but it clearly lays out the structure and avoids fat service clients which are problematic.
    * I am aware there is now a commercial cost of using this library however, there are multiple workarounds available, where you can implement the framework yourself, [please see this example](https://www.milanjovanovic.tech/blog/cqrs-pattern-the-way-it-should-have-been-from-the-start?utm_source=newsletter&utm_medium=email&utm_campaign=tnw142).
    * This is the fist step towards CQRS, and while this may be overkill for such a small project, this is a coding test 🙂.
    * I have injected these Handlers into the Controller for each endpoint to use.
  - **Database**:
    * As per the project brief, I have created an in-memory DB in the 'ProductsDbContext' class.
    * I have created a unique index constraint to cover the Name and Brand fields. If this was a real implementation I would include a Non-Clustered index on these fields so any lookups and joins could be done efficiently.
    * I have created a generic 'IGenericRepository' for the basic CRUD operations, e.g. 'GetAll', 'GetByIdAsync', 'AddAsync', 'UpdateAsync' and 'DeleteAasync'.
    * This is implemented by the specific 'IProductRepository' and extended with the method 'ProductExistsAsync'.
    * The benefit of this approach is if the project is extended, you will not need to re-write the basic CRUD operations for each Repository class.
- #### Validation:
   * For validations I have used the FluentValidation library.
   * I have created individual Validators for each CRUD operation.
   * I have configured them in the 'Program.cs' and injected them into the individual Endpoints.
   * When a validation rule fails, a Bad Request Http Response (400) is returned with individual messages in the response.
- #### Logging:
    * For logging I have used the Serilog library, which has been configured in the 'Program.cs' Class.
    * I have injected this into individual classes where we require logging for Information, Warnings and Errors.
    * Additionally, I have created a 'RequestLoggingMiddleware' Class which logs requests and errors into the console as per the brief.
- #### Unit Tests:
    * All Command and Query Handlers have been Unit Tested as these act as the Business Layer logic.
    * Additionally, I have Unit Tested all the Validators for the incoming Commands and Queries.
- #### Pagination and Filtering:
    * I have implemented pagination on the 'GetAll' Endpoint with mandatory properties for 'Page' (default 1) and 'PageSize' (default 10).
    * I have included optional fields for filtering by 'Name' and 'Brand'.
- #### Swagger:
    * I have implemented Swagger to make testing easier without setting up tools like Postman.

## **Future Extension of the Project**
Below I have listed items that would be good items to extend the project in the future:
   
* Allow updating, deleting and creating of more than one Product.
* Caching, if the data for the GET Endpoints does not change very often then implementing caching would be way to reduce server load and improve performance. For example if the same Product Id was requested in quick succession, this would be a good candidate, providing the data doesn't change very often.
* We may want functionality to restore the soft deleted products, but this would likely be an Administrator function.
* Health Check Endpoint, for Observability.
* Versioning on the Endpoints e.g. v1.0, v1.1 etc. Swagger could help us to achieve this.
* I have used Try Catch Blocks with the 'ArgumentNullException' in a few places. Global Exception handling and logging would allow this to be managed in a central place, reducing boiler plate code.
* For the filtering on the 'GetAll' Endpoint we could implement wildcard filtering, where we would not have to include the whole word, this would help the API to be more resilient.
* We could also extend the filtering to the Price property.
* I have hard coded strings for errors and logging, moving them to a central constants file would be cleaner.
* Authentication and Authorisation could be implemented e.g. Login Endpoint and the use of JWT tokens. This would be a necessity if we were going to productionise the application.




