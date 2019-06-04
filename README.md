# Advanced Entity Framework

## Configuring DbContext

- `Min Pool Size=5`
  - Opening and maintaining connections is expensive
  - If you ALWAYS need multiple connections, set Min Pool Size
- `Application Name=AdvancedEntityFrameworkApi`
  - Application Name can be used to identify clients that are draining SQL Server resources (assuming there is more than one client)
- `.AddDbContextPool()`
  - Each request has a single `DbContext` instance
  - Use `AddDbContextPool` instead of `AddDbContext` to maintain a pool of `DbContext` instances, which are reset at the end of each request
- `.EnableRetryOnFailure()`
  - Use EnableRetryOnFailure to retry when transient exceptions are thrown (SqlException with certain SQL error codes or TimeoutException)
- `.ConfigureWarnings()`
  - Use ConfigureWarnings to throw exceptions when queries are evaluated in-memory instead of in SQL instead of logging warnings (e.g. Where clause before a GroupBy)
- `.EnableSensitiveDataLogging()`
  - Use EnabledSensitiveDataLogging to log application data with exception messages and logs (e.g. entity values, command parameter values)
- `.UseQueryTrackingBehavior()`
  - Use UseQueryTrackingBehaviour to disable tracking of in-memory entities and improve performance, especially for large data sets
  - Call `.UpdateAsync` when editing an entity

## Entity Type Configurations

- Move to a separate file (fluent syntax vs. attributes)
- Couldn't get `.ToTable(...)` to work with latest NuGets so reverted back to `[Table(...)]`
- Automatically add them by overriding `OnModelCreating` and finding all entity type configurations for a given assembly

## Auditing

- `IEntity`
  - `CreatedAt`
  - `UpdatedAt`
- `AuditEntity`
  - Could be extended to include information about the authenticated user
- `DbContext` changes
  - `OnBeforeSaveChanges`
  - `OnAfterSaveChanges`
  - `SaveChangesAsync`
- SQL query using JSON_VALUE
```
select		*
from		Audit
where		JSON_VALUE(NewValues, '$.FirstName') = '...'
```

## Profiling

- Stackify Prefix
  - Trace each web request
  - Identify slow performing SQL queries
  - Installs and runs locally
  - No config/code changes required
  - FREE

## Query optimisations

- *** List students ***
  - Don't use `.Include()`
```
var students = await _schoolDbContext.Students
	.Select(x => new Student
	(
		x.StudentId,
		x.FirstName,
		x.LastName,
		x.DateOfBirth
	))
	.ToListAsync();

	return new ListStudentsResponse(students);
```
- Find student
  - Only include the fields that are required by calling `.Select()` after `.Where()` - reduce bandwidth
  - Always project outside of the query itself - avoid in-memory evaluation
```
return await _schoolDbContext.Students
    .Where(x => x.StudentId == studentId)
    .Select(x => new FindStudentResponse
    (
        x.StudentId,
        x.FirstName,
        x.LastName,
        x.DateOfBirth
    ))
    .SingleOrDefaultAsync();
```
- *** In-memory evaluation ***
  - `GroupBy()` before `.Where`
- *** PredicateBuilder***

## Don't abstract an abstraction

- Entity Framework provides a really good abstraction to the database
- The Repository Pattern adds unnecessary complexity and hides essential features - inject `DbContext` instance into the controller directly (KISS) or the business layer
- `DbContext` and `DbSet<T>` can be mocked (https://medium.com/@metse/entity-framework-core-unit-testing-3c412a0a997c)
  - Beware of using the in-memory provider because it doesn't act like a true relational database so some of your constraints could be broken
- Common where clauses are a good example for extension methods to keep it DRY

## "Entity Framework is slow"

- Developers have the potential to write inefficient queries, but this is also true for raw SQL - make sure they're profiled and optimised
- Start with Entity Framework to get a head start - it's always better to release a product earlier with potential performance problems as opposed to wait until everything is perfect
- Queries that can't be optimised using Entity Framework can be replaced by raw SQL (potentially with Dapper)

## References

- [Optimally Configuring Entity Framework Core](https://rehansaeed.com/optimally-configuring-entity-framework-core/) by Muhammad Rehan Saeed
- [Entity Framework Core: History / Audit table](https://www.meziantou.net/2017/08/14/entity-framework-core-history-audit-table) by Gérald Barré