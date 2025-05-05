# 📘 API ENDPOINTS DOCUMENTATION

All API endpoints used throughout the application are detailed below. Each endpoint is protected with role-based access control, validation, and error management appropriate to its purpose.

---

## 🔐 Authentication

### POST `/api/Auth/Login`

Allows the user to log in to the system with their email and password. Returns Access and Refresh Tokens upon successful login.

**Request**

```json
{
  "email": "admin@expense.com",
  "password": "Admin123."
}
```

**Response**

```json
{
  "accessToken": "...",
  "refreshToken": "..."
}
```

**Status Codes:**

* 200 OK
* 400 Bad Request
* 401 Unauthorized

### POST `/api/Auth/RefreshToken`

Allows obtaining a new Access Token using a valid Refresh Token.

**Request**

```json
{
  "refreshToken": "..."
}
```

**Response**

```json
{
  "accessToken": "...",
  "refreshToken": "..."
}
```

**Status Codes:**

* 200 OK
* 400 Bad Request
* 401 Unauthorized

---

## 👤 Users (Admin)

### GET `/api/Users`

Lists all users.

### GET `/api/Users/{id}`

Retrieves a specific user by their ID.

### POST `/api/Users`

Creates a new user. A password is generated and sent via email using RabbitMQ.

### PUT `/api/Users/{id}`

Updates an existing user.

### DELETE `/api/Users/{id}`

Soft deletes a user from the system.

**General Status Codes:**

* 200 OK
* 404 Not Found
* 400 Bad Request

---

## 📁 Expense Categories (Admin)

### GET `/api/ExpenseCategories`

Lists all expense categories defined in the system. Accelerated with Redis cache.

### GET `/api/ExpenseCategories/{id}`

Retrieves a specific expense category by its ID.

### POST `/api/ExpenseCategories`

Defines a new category.

### PUT `/api/ExpenseCategories/{id}`

Allows updating a category.

### DELETE `/api/ExpenseCategories/{id}`

Soft deletes a specific category.

---

## 💸 Expense Claims (Personnel)

### GET `/api/ExpenseClaims`

Lists expense claims filtered by date, status, category, and amount.

**Query Params:**

* `startDate`
* `endDate`
* `status`
* `categoryId`
* `minAmount`
* `maxAmount`

### GET `/api/ExpenseClaims/{id}`

Retrieves the details of a specific expense claim.

### POST `/api/ExpenseClaims`

Creates a new expense claim. A receipt/invoice file can be attached.

### PUT `/api/ExpenseClaims/{id}`

Updates an existing claim.

### DELETE `/api/ExpenseClaims/{id}`

Soft deletes an expense claim.

---

## 🧾 File Upload (Personnel)

### POST `/api/Files/upload`

Allows uploading files such as receipts or documents. Uploaded files are saved in the `/wwwroot/uploads` directory on the server.

**Request:** Multipart/Form-Data (max 5MB)

---

## 🏦 EFT Simulation (Admin)

### POST `/api/EftSimulations`

Simulates a payment using IBAN information when an expense claim is approved. The simulation result is logged in the system.

**Status Codes:**

* 200 OK
* 400 Bad Request
* 404 Not Found

---

## 📊 Reports

### GET `/api/Reports/PersonnelExpenseSummary`

Provides a summary of the total expenses (amount, approved, rejected counts) for the logged-in personnel.

### GET `/api/Reports/UserExpenseList`

Provides a detailed list of all expense claims made by the logged-in personnel.

### GET `/api/Reports/CompanyExpenseSummary?startDate=...&endDate=...`

Provides a summary of the company's total expenses within a specified date range.

### GET `/api/Reports/CompanyExpenseStatusSummary?startDate=...&endDate=...`

Provides summaries of expense statuses (approved, rejected, pending) within a specified date range.

---

## 🔁 Refresh Token

### POST `/api/Auth/RefreshToken`

Generates a new Access Token using a Refresh Token. Extends the token's validity period.

---

## ℹ️ General Notes

* All endpoints require JWT-based authentication.
* Role-based access control is implemented. (Admin / Personnel)
* Can be tested via Swagger UI. Endpoint groups are separated by roles.
* Validation and error messages are structured to inform the user.
* Advanced error handling and customized ApiResponse models are used.
* **Redis** is used for caching operations.
* **RabbitMQ** is used for password delivery with message queue infrastructure.
* **Dapper** is used for custom SQL reporting.
* **SMTP (Gmail)** is used for automatic password notification emails.
* Detailed Swagger test screenshots and usage scenarios are available in `docs/swagger-guide.pdf`.
