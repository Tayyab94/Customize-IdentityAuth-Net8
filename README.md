# Identity_Net8 Web Application and API

## Overview

This project is a comprehensive web application and API built with ASP.NET Core 8. It leverages ASP.NET Core Identity for user authentication and authorization, customizing the user and role tables. The project also includes global exception handling, route definitions, and seed data creation on startup.

## Features

- **User and Role Customization:** Customizes the default Identity user and role tables.
- **Global Exception Handling:** Implements global exception handling for both API and web requests.
- **Routing:** Defines routes for both web and API controllers.
- **Data Seeding:** Adds initial data (users and roles) when the project starts.
- **Swagger Integration:** Includes Swagger for API documentation and testing.
- **Authentication and Authorization:** Uses JWT for API authentication and custom policies for role-based authorization.

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- ASP.NET Core Identity
- JWT Authentication
- Swagger
- SQL Server

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio or Visual Studio Code

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/Tayyab94/Customize-IdentityAuth-Net8.git
    cd Identity_Net8
    ```

2. **Set up the database:**

    Update the connection string in `appsettings.json` to point to your SQL Server instance:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
    }
    ```

3. **Apply migrations:**

    Open a terminal or command prompt and run the following commands:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. **Run the application:**

    ```bash
    dotnet run
    ```

### Usage

- **Web Application:**

  Access the web application by navigating to `https://localhost:5001` in your browser.

- **API:**

  Access the API by navigating to `https://localhost:5001/api/{controller}/{action}`.

- **Swagger UI:**

  Explore and test the API using Swagger UI at `https://localhost:5001/swagger`.

## Project Structure

- `Models`: Contains the custom user (`ApplicationUser`) and role (`ApplicationRole`) classes.
- `HelperFunctions`: Includes helper functions and custom authorization handlers.
- `Controllers`: Contains the MVC controllers for handling web and API requests.
- `Data`: Contains the `ApplicationDbContext` class for database context.
- `Startup.cs`: Configures services and the HTTP request pipeline.

## Key Components

### Custom User and Role Classes

- `ApplicationUser`: Extends `IdentityUser` to include custom properties.
- `ApplicationRole`: Extends `IdentityRole`.

### Global Exception Handling

Configures global exception handling to differentiate between API and web requests.

### Route Definitions

Defines separate routes for web and API controllers.

### Data Seeding

Adds initial data (users and roles) during application startup.

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit them (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Open a Pull Request.
