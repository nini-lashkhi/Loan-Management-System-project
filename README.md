[README.md](https://github.com/user-attachments/files/28718228/README.md)
# Loan Management API

ASP.NET Core Web API პროექტი სესხების მართვისთვის

## ტექნოლოგიები

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (LocalDB)
- JWT Authentication
- FluentValidation
- Swagger / OpenAPI

## პროექტის სტრუქტურა

- `LoanManagement.API` – API Controllers და კონფიგურაცია
- `LoanManagement.Application` – Services, DTOs, Interfaces, Validators
- `LoanManagement.Infrastructure` – Repositories და Data Access Layer

## მოთხოვნები

- .NET SDK 8.0 
- SQL Server LocalDB

## გაშვების ინსტრუქცია

### 1. Repository-ის კლონირება

```bash
git clone <repository-url>
cd LoanManagement
```

### 2. დამოკიდებულებების აღდგენა

```bash
dotnet restore
```

### 3. მონაცემთა ბაზის კონფიგურაცია

ფაილში `LoanManagement.API/appsettings.json` მითითებულია Connection String.

### 4. Migration-ების გაშვება

```bash
dotnet ef database update
```

### 5. პროექტის გაშვება

```bash
cd LoanManagement.API
dotnet run
```

### 6. Swagger-ის გახსნა

```text
https://localhost:<port>/swagger
```

## ავტორი

Nini Lashkhi
