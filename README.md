# Expense Management System

The Expense Management System is a modular and layered .NET 9 Web API project developed as part of the **Papara Women in Tech Bootcamp**. It is designed for personnel to manage expense requests and for managers to approve, reject, and report these requests.

---

## 🌐 Technologies Used

* .NET 9 Web API
* PostgreSQL + EF Core
* Redis (caching)
* RabbitMQ (event-driven architecture)
* Dapper (for reporting performance)
* AutoMapper, FluentValidation, JWT, Swagger
* Docker & Docker Compose

---

## 📊 Key Features

* User registration is handled by the admin, and passwords are sent via RabbitMQ email
* Users log in with the password sent to them
* Role-based access (Admin / Personnel)
* Expense request operations (add, update, delete, list)
* Document upload (receipt/invoice), saved in the `wwwroot/uploads` folder
* Caching category data with Redis
* Sending password information via RabbitMQ email
* Filtered reports by date, status, category, and amount
* High-performance queries using PostgreSQL functions and Dapper
* Endpoint testing and organized grouping via Swagger
* Two default users (admin and personnel) are automatically added during system setup

---

## 🧪 Seed (Default) Users

When the database is first created, the system automatically adds the following users:

| Role      | Email                                         | Password  |
| --------- | --------------------------------------------- | --------- |
| Admin     | [admin@expense.com](mailto:admin@expense.com) | Admin123. |
| Personnel | [user@expense.com](mailto:user@expense.com)   | User123.  |

> After passwords are added to the database, new user passwords are sent via RabbitMQ email. These default users are added during system setup.

---

## 🚀 How to Run

### Run with Docker Compose (Recommended)

> Docker and Docker Compose must be installed.

1. Clone the project:

```bash
git clone https://github.com/tugcegenc/ExpenseManagementSystem.git
cd ExpenseManagementSystem
```

2. Run app_settings.sh to place the required appsettings files, then customize Expense.EmailConsumer/appsettings.Local.json to configure the email sender account:

```bash
./app_settings.sh
```

3. Start Docker services:

```bash
docker-compose up -d
```

This will start PostgreSQL, Redis, and RabbitMQ containers.

4. Run the migration to create the database:

```bash
dotnet ef database update --project Expense.Infrastructure --startup-project Expense.Api
```

If there are no migrations, create the initial one:

```bash
dotnet ef migrations add InitialCreate --project Expense.Infrastructure --startup-project Expense.Api
```

5. Run the API and Email Consumer projects from within the Expense.Api and Expense.EmailConsumer directories:

```bash
dotnet run
```

6. Access the Swagger interface:
   👉 `http://localhost:5220/index.html`

---

## 📖 Additional Information

* Users log in with the password sent to them via email; password changes are not supported.
* The system adds 2 default users (admin & personnel) as seed data during the initial setup.
* Passwords, email, SMTP, and queue information are stored in `appsettings.Local.json` and protected by `.gitignore`.
* Endpoints are grouped and role-based in the Swagger interface.
* PostgreSQL functions and Dapper are used together for reporting.

---

## 📘 Additional Documentation and Previews

### 📄 [docs/api-endpoints.md](docs/api-endpoints.md)

Descriptions, parameters, authorization details, and functions of all API endpoints are listed here.

### 📸 [docs/swagger-screenshots.pdf](docs/swagger-screenshots.pdf)

Example images of the Swagger interface can be found in the following folder.
