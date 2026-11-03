# PizzaSolution

PizzaSolution is a .NET 8 ASP.NET Core Web API for an automated pizza place. The project demonstrates REST endpoints, dependency injection, JSON serialization, unit testing, different pizza ovens, PostgreSQL persistence with Entity Framework Core, and EF Core migrations.

## What the project can do

- Return a welcome response from `GET /api/welcome`.
- Return a standard or lunch menu from `GET /api/menu` based on UTC time.
- Accept pizza orders through `POST /api/order`.
- Look up recipes from PostgreSQL when processing orders.
- Check stock availability before preparing pizzas.
- Prepare pizzas using different oven implementations:
  - `NormalPizzaOven`
  - `AssemblyLinePizzaOven`
  - `GiantRevolvingPizzaOven`
- Add recipes through `POST /api/recipe`.
- Retrieve recipes required by the configured recipe request through `GET /api/recipe`.
- Restock ingredients through `POST /api/restocking`.
- Read calculated stock through `GET /api/restocking`.
- Persist recipes, recipe ingredients, and stock in PostgreSQL.
- Create and apply database schemas with EF Core migrations.
- Run automated MSTest tests using Moq and fake repositories.

## Project structure

```text
PizzaSolution/
├── PizzaSolution.sln
├── PizzaPlace/
│   ├── Controllers/       HTTP endpoints
│   ├── Data/              EF Core context, entities, seeding, and factory
│   ├── Factories/          Pizza oven implementations
│   ├── Models/             Domain models and DTOs
│   ├── Pizzas/             Pizza result types
│   ├── Repositories/       Database and fake repository implementations
│   ├── Services/           Application/business logic
│   ├── Migrations/         EF Core migrations
│   └── Program.cs          Application and dependency-injection setup
└── Tests/
	└── PizzaPlace.Test/   MSTest unit tests
```

## Requirements

- Windows 10 or Windows 11
- Visual Studio with the **ASP.NET and web development** workload
- .NET 8 SDK
- Git
- PostgreSQL running locally
- A PostgreSQL database named `PizzaDatabase`
- PostgreSQL credentials with permission to create and modify tables

The project currently uses:

- ASP.NET Core .NET 8
- Entity Framework Core 8
- Npgsql Entity Framework Core provider
- NSwag/Swagger
- MSTest
- Moq

## Clone the repository

From PowerShell:

```powershell
git clone https://github.com/MBVeilis/PizzaSolution.git
cd PizzaSolution
```

Alternatively, in Visual Studio:

1. Select **Clone a repository**.
2. Enter the repository URL.
3. Choose a local folder.
4. Open `PizzaSolution.sln`.

## Configure PostgreSQL

Create a local PostgreSQL database named `PizzaDatabase`. The default connection string is configured under `ConnectionStrings:WebApiDatabase` in `PizzaPlace/appsettings.json`.

Do not commit real passwords to source control. For local development, user secrets are preferable:

```powershell
dotnet user-secrets init --project PizzaPlace
dotnet user-secrets set "ConnectionStrings:WebApiDatabase" "Host=localhost;Port=5432;Database=PizzaDatabase;Username=postgres;Password=your-password" --project PizzaPlace
```

The connection string must contain the same key used by the application:

```text
ConnectionStrings:WebApiDatabase
```

## Set up the database and migrations

Open a terminal in the solution directory. Install the EF Core command-line tool if necessary:

```powershell
dotnet tool install --global dotnet-ef
```

Restore dependencies:

```powershell
dotnet restore
```

Create a migration only when the EF model changes:

```powershell
dotnet ef migrations add InitialCreate `
  --project PizzaPlace `
  --startup-project PizzaPlace
```

The project already contains an initial migration. Normally, after cloning, apply the existing migrations instead:

```powershell
dotnet ef database update `
  --project PizzaPlace `
  --startup-project PizzaPlace
```

The application also runs its database initialization and seeding during startup. The seed process applies pending migrations and adds the standard recipes and initial stock when the corresponding tables are empty.

## Run the project in Visual Studio

1. Open `PizzaSolution.sln` in Visual Studio.
2. Confirm that the `PizzaPlace` project is the startup project.
3. Confirm that PostgreSQL is running and the connection string is valid.
4. Build the solution with **Build > Build Solution**.
5. Start the project with the green **PizzaPlace** play button.
6. If Visual Studio asks about the development certificate, accept and install it.
7. Open Swagger at:

   ```text
   http://localhost:9009/swagger
   ```

The port is configured in `PizzaPlace/Properties/launchSettings.json`.

To stop the application, use the Stop button in Visual Studio or press `Shift+F5`.

## Run the project from PowerShell

```powershell
dotnet run --project PizzaPlace
```

Then open the Swagger URL shown by the application, normally:

```text
http://localhost:9009/swagger
```

## Try the endpoints

### Welcome

```http
GET /api/welcome
```

Returns a welcome message.

### Menu

```http
GET /api/menu
```

Returns the standard menu outside lunch hours and the lunch menu between 11:00 and 14:00 UTC.

### Add a recipe

```http
POST /api/recipe
Content-Type: application/json
```

Example:

```json
[
  {
	"recipeType": "StandardPizza",
	"ingredients": [
	  {
		"stockType": "Dough",
		"amount": 1
	  },
	  {
		"stockType": "Tomatoes",
		"amount": 2
	  }
	],
	"cookingTimeMinutes": 10
  }
]
```

Recipe IDs are generated by the database.

### Place an order

```http
POST /api/order
Content-Type: application/json
```

Example:

```json
{
  "id": "00000000-0000-0000-0000-000000000001",
  "requestedOrder": [
	{
	  "id": "00000000-0000-0000-0000-000000000002",
	  "pizzaOrderId": "00000000-0000-0000-0000-000000000003",
	  "pizzaType": "StandardPizza",
	  "amount": 1
	}
  ]
}
```

The current implementation waits for the oven to finish preparing the pizzas before returning the response. A standard pizza currently takes approximately 10 real minutes.

### Restock ingredients

```http
POST /api/restocking
Content-Type: application/json
```

Example:

```json
[
  {
	"stockType": "Dough",
	"amount": 50
  },
  {
	"stockType": "Tomatoes",
	"amount": 100
  }
]
```

Stock can be inspected with:

```http
GET /api/restocking
```

## Run tests

From Visual Studio:

1. Open **Test > Test Explorer**.
2. Build the solution.
3. Select **Run All Tests**.

From PowerShell:

```powershell
dotnet test Tests/PizzaPlace.Test/PizzaPlace.Test.csproj
```

The tests cover services, repositories, controllers, and the different pizza oven implementations. Most unit tests use the fake repositories so they do not require a live PostgreSQL database.

## How the application is structured

### Controllers

Controllers expose HTTP endpoints and translate requests into service calls. Examples include `OrderingController`, `RecipeController`, and `RestockingController`.

### Services

Services contain application behavior such as:

- Finding recipes required by an order.
- Checking whether sufficient stock exists.
- Calculating remaining stock.
- Grouping pizza orders for preparation.

### Repositories

The production dependency-injection configuration uses:

- `RecipeRepository`
- `StockRepository`

These use EF Core and PostgreSQL. The fake repositories remain useful for unit tests and isolated development.

### Entity Framework Core

`AppDbContext` maps database entities and relationships. Recipes and recipe ingredients are stored separately, with recipe ingredients linked to their recipe by a foreign key. Stock is stored by `StockType` with a unique constraint.

## Known limitations and possible improvements

- `GET /api/recipe` currently depends on the configured `PizzaOrder` and can return an empty list when that order has no requested pizzas. A separate `GET /api/recipe` implementation that returns all saved recipes would be clearer.
- The order endpoint waits synchronously for pizza production.
- Error handling is not yet centralized.
- The API accepts domain models directly instead of dedicated request and response DTOs.
- Stock updates could require transactions or concurrency handling when many orders arrive at the same time.
- The current seed data is intended for local development.
- Database credentials should be supplied through user secrets or environment variables.
- More integration tests should be added for the real EF Core repositories.
- Logging could be made more structured and less verbose outside development.

# Future development

The project guide continues with sections 6 and 7. The following work would complete those areas.

## Guide section 6: Improve the setup

### Return immediately when placing an order

The current order endpoint waits until every pizza is ready. This keeps the HTTP request open for several minutes. A better design would:

1. Validate the order immediately.
2. Calculate the estimated price and preparation time.
3. Create an order record with a status such as `Queued`.
4. Return an order ID, price, and estimated completion time.
5. Process the pizzas in a background service.
6. Mark the order as `Completed` or `Failed` when processing ends.
7. Log completion rather than keeping the HTTP request open.

Possible technologies include:

- `BackgroundService` or `IHostedService`
- A persistent order status table
- An in-memory channel for a simple implementation
- A message broker for a larger system
- SignalR or webhooks for completion notifications

### Improve order validation

Add validation for:

- Empty orders.
- Zero or negative amounts.
- Unsupported pizza types.
- Duplicate or conflicting IDs.
- Orders that exceed available stock.

Return consistent problem-details responses using `ProblemDetails` rather than raw exception messages.

### Improve error handling

Add centralized exception handling middleware and map errors to appropriate HTTP status codes:

- `400 Bad Request` for invalid input.
- `404 Not Found` for missing resources.
- `409 Conflict` for duplicate recipes or concurrent stock conflicts.
- `500 Internal Server Error` for unexpected failures.

Internal exception details should be logged but not exposed to API clients.

### Improve database usage

- Add repository integration tests using a test PostgreSQL database or Testcontainers.
- Add transactions around order validation and stock reservation.
- Prevent overselling stock under concurrent requests.
- Add indexes for frequently queried fields.
- Store order status and preparation results persistently.
- Move seed data into a controlled development-only initialization process.

## Guide section 7: Develop new functionality

Possible features include:

### Order tracking

Add an endpoint such as:

```http
GET /api/order/{id}
```

It could return:

- Order status.
- Requested pizzas.
- Current stock reservation.
- Estimated completion time.
- Completed pizzas.
- Failure details when applicable.

### Pizza inventory and sold-out menu items

Use current stock to determine whether menu items can be ordered. The menu could return a `soldOut` value based on ingredient availability rather than storing a manually maintained flag.

### Stock management

Add endpoints for:

- Current inventory levels.
- Low-stock warnings.
- Suggested restocking quantities.
- Stock movement history.
- Supplier deliveries.
- Ingredient expiration dates.

### Demand forecasting

Store historical orders and calculate:

- Most popular pizzas.
- Ingredient consumption trends.
- Expected demand by time of day.
- Recommended stock levels.
- Waste and shortage statistics.

### More oven implementations

Possible oven types include:

- An oven that prioritizes urgent orders.
- An oven that supports different temperature settings.
- An oven that can prepare multiple pizza sizes.
- An oven with maintenance downtime.
- An oven that reports progress and failures.

### Authentication and authorization

Add users and roles such as:

- Customer.
- Kitchen employee.
- Restaurant manager.
- Administrator.

Restrict recipe management, stock management, and reporting endpoints to appropriate roles.

### API quality improvements

- Use versioned API routes.
- Add OpenAPI descriptions and examples.
- Add request validation with FluentValidation or data annotations.
- Return consistent response DTOs.
- Add pagination and filtering for recipes, orders, and stock movements.
- Add contract and integration tests.

### Deployment

After local development is complete, the project could be deployed using:

- Docker and Docker Compose.
- A hosted PostgreSQL service.
- Azure App Service or Azure Container Apps.
- CI/CD with GitHub Actions.
- Automated migration and deployment steps.
- Environment-specific configuration and secrets.

## Suggested development workflow

1. Create a small issue describing one change.
2. Add or update tests first where practical.
3. Implement the smallest change that satisfies the tests.
4. Run the full test suite.
5. Run database migrations when the EF model changes.
6. Test the endpoint through Swagger or an HTTP client.
7. Commit the change with a focused message.

Example commit messages:

```text
Add PostgreSQL-backed stock repository
Add background order processing
Add low-stock warning endpoint
Add order status persistence
```
