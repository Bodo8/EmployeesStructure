Aplication .NET Framework 4.8 Web API.

It uses Entity Framework 6 with a Code First approach.

MSSql database, SQL Server SQLEXPRESS. The database connection string in Web.config must be updated before creating the application.

Database Name: EmployeesStructure


Solution 1: allows you to determine whether a given employee is a superior of any rank to another employee and stores the number of that rank.
The Composite design pattern was used.
In the API tab, you can see Employee endpoints and URL parameters.

The application has a DBSeeder and several employees and their relationships are uploaded.

The application logic is in the EmployeeHierarchyService class, and its tests are in the EmployeeHierarchyServiceTests class in the test project.

(If updating Employee, you must prevent employee C from being set as the superior of employee X when C has employee X as the superior.)

You can test the solution using Postman:

Sample request:
https://localhost:44367/api/GetSuperiorRowOfEmployee?employeeId=3&superiorId=1

should return:
employeeId	3
superiorId	1
row	1
isSuperior	true

(In a solution for a company with more than 1,000 employees, the design pattern should be changed to the Materialized Path Pattern.

Performance optimizations should be added, and the Employee entity should include:

a string HierarchyPath field, which would store the entire hierarchy path in a single column (e.g. "/1/2/4/"), and
an int HierarchyLevel field, which would store the level in the hierarchy (0 = CEO, 1 = first level, etc.).

You could also use MemoryCache, recursion handled in the database (not in C#), and indexes on the HierarchyPath and HierarchyLevel fields.

For very large companies (over 50,000 employees):
use Redis Cache instead of MemoryCache, Read Replicas (a separate database for read operations), and partitioning (dividing tables by departments).)