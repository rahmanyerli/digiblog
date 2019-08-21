# DigiBlog

> Demo Blog Application - Backend

## Installation

### 1st Step - Create Database
Execute all scripts from Database.sql file on your local MSSQL database. Database.sql file contains all database objects and demo records such as demo roles and demo users.

### 2nd Step - Run Project
Open your Terminal or Command Prompt and run enter this dotnet CLI commands `dotnet restore` and `dotnet run`
Your application will be hosted at this endpoint: [https://localhost:5001](https://localhost:5001)

### 3rd Step - Call APIs
You can use Postman or Insomnia to make API calls. All services work with a role-based session, for this reason you must to get a JWT token before calling any API. To do this, you need to call the api/users/authenticate method first. 

You can find the full list of APIs and sample authentication requests below.

#### APIs ####

1. Roles

    [https://localhost:5001/api/roles](https://localhost:5001/api/roles)
2. Users

    [https://localhost:5001/api/users](https://localhost:5001/api/users)
3. Categories

    [https://localhost:5001/api/categories](https://localhost:5001/api/categories)
4. Articles

    [https://localhost:5001/api/articles](https://localhost:5001/api/articles)
5. Comments

    [https://localhost:5001/api/comments](https://localhost:5001/api/comments)

##### Authentication(JWT) #####

POST [https://localhost:5001/api/users/authenticate](https://localhost:5001/api/users/authenticate)

Get authentication token for admin role.

BODY

```JSON
{
	"Username": "demoadmin",
	"Password": "demoadmin"
}
```

Get authentication token for author role.

BODY

```JSON
{
	"Username": "demoauthor",
	"Password": "demoauthor"
}
```

Get authentication token for member role.

BODY

```JSON
{
	"Username": "demomember",
	"Password": "demomember"
}
```

### Used Patterns ###

- Dependency Injection
- Repository Pattern
- Fluent API
- Controller - Service - Repository architecture

> I have used these patterns in the Zurich Internet Branch project before.

### What more can be done! ###

- Unit test scripts with mock objects.
- Swagger for detailed API documentation.
- More validation and encoding for data consistency and security.
- More controller methods such as paging, sorting, change password etc.
- More detailed exception messages.
- Redis for better caching.
- 3rd party logging packages such as NLog.