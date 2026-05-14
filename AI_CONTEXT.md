# Arab River Backend Complete Architecture & Implementation Guide

# Project Overview

Arab River is a production-style ASP.NET Core Web API backend built using:

- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- 3-Tier Architecture
- JWT Authentication
- AutoMapper
- FluentValidation
- Repository Pattern
- Service Layer
- Rate Limiting
- Global Exception Handling
- Brevo SMTP Email Integration
- CSV Export
- Geolocation

The backend powers:

- Public catalog browsing
- Lead capture & catalog download system
- Contact form workflow
- Admin dashboard
- Authentication system
- CSV export system

---

# Main Business Flow

## Public Website

Users browse catalogs.

If user wants to open/download a catalog:

1. User enters:
   - First Name (required)
   - Phone Number (required)
   - Clinic/Hospital Name (optional)

2. Backend detects user country using IP.

3. Lead is saved.

4. Backend returns Google Drive catalog URL.

5. Frontend redirects user to download URL.

---

## Egypt Logic

- Egyptian users browse normally.
- Non-Egyptian users may enter clinic/hospital name.

Geolocation is handled using IP address.

---

# Final Architecture

```txt
Controllers
    ↓
Services
    ↓
Repositories
    ↓
DbContext
    ↓
SQL Server
```

---

# Solution Structure

```txt
ArabRiver.sln
│
├── ArabRiver.Api
├── ArabRiver.Service
└── ArabRiver.Repository
```

---

# Repository Layer Structure

```txt
ArabRiver.Repository
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Interfaces
│   ├── IAdminRepository.cs
│   ├── ICatalogRepository.cs
│   ├── ILeadRepository.cs
│   └── IContactMessageRepository.cs
│
├── Repositories
│   ├── AdminRepository.cs
│   ├── CatalogRepository.cs
│   ├── LeadRepository.cs
│   └── ContactMessageRepository.cs
│
└── Models
    ├── Admin.cs
    ├── Catalog.cs
    ├── Lead.cs
    └── ContactMessage.cs
```

---

# Service Layer Structure

```txt
ArabRiver.Service
│
├── DTOs
├── Helpers
├── Interfaces
├── Mapping
├── Pagination
├── Responses
├── Services
└── Validations
```

---

# API Layer Structure

```txt
ArabRiver.Api
│
├── Controllers
│   ├── AuthController.cs
│   ├── CatalogController.cs
│   ├── LeadController.cs
│   ├── ContactController.cs
│   │
│   └── Admin
│       ├── AdminCatalogController.cs
│       ├── AdminLeadController.cs
│       └── AdminContactController.cs
│
├── Middleware
│   └── ExceptionMiddleware.cs
│
├── Seed
│   └── AdminSeeder.cs
│
├── appsettings.json
└── Program.cs
```

---

# Database Models

# Admin

Purpose:
- single admin authentication
- dashboard access

Fields:

```txt
Id
Name
Email
PasswordHash
Role
CreatedAt
```

Passwords use BCrypt hashing.

---

# Catalog

Purpose:
- store downloadable catalogs

Fields:

```txt
Id
Name
Description
GoogleDriveLink
ThumbnailUrl
Category
DisplayOrder
IsActive
CreatedAt
UpdatedAt
```

Catalogs are deactivated instead of deleted.

---

# Lead

Purpose:
- store download leads

Fields:

```txt
Id
FirstName
PhoneNumber
OrganizationName
Country
CountryCode
IsEgypt
IpAddress
CatalogId
CatalogNameSnapshot
CreatedAt
```

Important:
- CatalogNameSnapshot preserves catalog name even if catalog changes later.

---

# ContactMessage

Purpose:
- store website contact messages

Fields:

```txt
Id
Name
Email
Message
Status
ReadAt
IpAddress
CreatedAt
```

Status values:

```txt
New
Read
```

---

# Repository Pattern

Repositories handle:
- database access only
- Entity Framework operations
- CRUD operations

Repositories DO NOT contain:
- business logic
- validation
- HTTP logic

---

# Service Layer Responsibilities

Services contain:
- business logic
- workflows
- integrations
- pagination
- validation flow
- email flow
- geolocation flow

Services DO NOT:
- access HttpContext directly
- return EF entities directly

---

# DTO Structure

Separate DTOs were created for:

```txt
Admin
Auth
Catalog
Lead
Contact
```

Purpose:
- never expose EF entities directly
- clean API contracts
- frontend-safe responses

---

# Unified API Response Structure

All endpoints use unified response models.

# Success Response

```json
{
  "success": true,
  "message": "Success message",
  "data": {}
}
```

---

# Validation Error Response

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": []
}
```

---

# Exception Response

```json
{
  "success": false,
  "message": "Something went wrong. Please try again later."
}
```

---

# Pagination System

PaginationParameters:

```txt
PageNumber
PageSize
```

PagedResponse returns:

```txt
Items
PageNumber
PageSize
TotalCount
TotalPages
```

Pagination is handled in Service layer.

---

# AutoMapper

AutoMapper is used for:

```txt
DTO <-> Entity Mapping
```

Main profile:

```txt
MappingProfile.cs
```

Mappings include:

```txt
CreateLeadDto -> Lead
Lead -> LeadResponseDto
CreateCatalogDto -> Catalog
Catalog -> CatalogResponseDto
ContactMessage -> ContactMessageResponseDto
Admin -> AdminResponseDto
```

---

# FluentValidation

Validation implemented using:

```txt
FluentValidation
```

Validators exist for:

```txt
Catalog DTOs
Lead DTOs
Contact DTOs
Auth DTOs
```

Validation registered using:

```csharp
builder.Services
    .AddValidatorsFromAssemblyContaining<
        CreateCatalogDtoValidator>();
```

---

# JWT Authentication

JWT system implemented using:

```txt
IJwtService
JwtService
JwtSettings
```

Claims included:

```txt
Admin Id
Email
Role
Name
```

JWT configuration stored in:

```txt
appsettings.json
```

Authentication middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

# Auth Flow

## Login

Endpoint:

```http
POST /api/admin/auth/login
```

Flow:

```txt
Validate credentials
→ Verify BCrypt password
→ Generate JWT
→ Return token
```

---

# Get Current Admin

Endpoint:

```http
GET /api/admin/auth/me
```

Uses JWT token claims.

---

# Swagger Authentication

Swagger configured with Bearer authentication.

Usage:

```txt
Bearer JWT_TOKEN
```

---

# Geolocation System

Service:

```txt
GeolocationService
```

Uses:

urlip-api.comhttps://ip-api.com/

Purpose:
- detect user country
- determine Egypt/non-Egypt

Lead saving still works even if geolocation fails.

Fallback:

```txt
Unknown
```

---

# Email System

Email provider:

urlBrevohttps://www.brevo.com/

Implementation uses:

```txt
MailKit
```

EmailService responsibilities:

```txt
SendContactNotificationAsync
SendAutoReplyAsync
```

SMTP configuration stored in:

```txt
EmailSettings
```

---

# Contact Workflow

Endpoint:

```http
POST /api/contact
```

Flow:

```txt
Save message in DB
→ Send company email
→ Send auto reply email
→ Return success
```

Important:
- DB save happens BEFORE email sending.

---

# Lead Download Workflow

Endpoint:

```http
POST /api/leads
```

Flow:

```txt
Validate catalog
→ Detect IP location
→ Save lead
→ Return Google Drive URL
```

Frontend redirects user to:

```txt
downloadUrl
```

---

# CSV Export System

Service:

```txt
CsvExportService
```

Purpose:
- export all leads as CSV

Endpoint:

```http
GET /api/admin/leads/export
```

CSV includes:

```txt
FirstName
PhoneNumber
OrganizationName
Country
CountryCode
IsEgypt
CatalogName
CreatedAt
```

UTF8 encoding used for Arabic support.

---

# Exception Middleware

Global exception middleware implemented.

Purpose:
- unified error responses
- production-safe exception handling
- no stack traces exposed

Middleware:

```txt
ExceptionMiddleware
```

Registered before other middleware.

---

# Rate Limiting

Built-in ASP.NET rate limiting implemented.

Policies:

```txt
LoginPolicy
ContactPolicy
LeadPolicy
```

Limits:

```txt
Login    -> 5/min
Contact  -> 10/min
Lead     -> 15/min
```

429 returned when limit exceeded.

---

# Seeder System

AdminSeeder implemented.

Purpose:
- automatically create first admin

Default credentials:

```txt
Email:
admin@arabriver.com

Password:
Admin@123
```

Password is hashed using BCrypt.

---

# CORS Configuration

CORS uses:

```txt
FrontendSettings:BaseUrl
```

Configured in appsettings.

Purpose:
- allow only frontend domain
- secure production APIs

---

# Program.cs Responsibilities

Program.cs configures:

```txt
DbContext
JWT
Swagger
Repositories
Services
AutoMapper
Validation
CORS
Rate Limiting
Exception Middleware
Authentication
Authorization
Seeder
```

---

# Dependency Injection Registrations

Repositories:

```txt
ICatalogRepository
ILeadRepository
IContactMessageRepository
IAdminRepository
```

Services:

```txt
IAuthService
IJwtService
ICatalogService
ILeadService
IContactService
IEmailService
IGeolocationService
ICsvExportService
```

---

# Public Controllers

# AuthController

Routes:

```txt
POST /api/admin/auth/login
GET  /api/admin/auth/me
```

---

# CatalogController

Routes:

```txt
GET /api/catalogs
GET /api/catalogs/{id}
```

Public browsing only.

---

# LeadController

Routes:

```txt
POST /api/leads
```

Handles lead capture and download flow.

---

# ContactController

Routes:

```txt
POST /api/contact
```

Handles contact form workflow.

---

# Admin Controllers

All protected using:

```csharp
[Authorize]
```

---

# AdminCatalogController

Routes:

```txt
GET    /api/admin/catalogs
POST   /api/admin/catalogs
PUT    /api/admin/catalogs/{id}
PATCH  /api/admin/catalogs/{id}/status
```

Responsibilities:

```txt
Catalog CRUD
Activate/Deactivate
Pagination
```

---

# AdminLeadController

Routes:

```txt
GET /api/admin/leads
GET /api/admin/leads/export
```

Responsibilities:

```txt
Lead listing
CSV export
Pagination
```

---

# AdminContactController

Routes:

```txt
GET   /api/admin/contact-messages
PATCH /api/admin/contact-messages/{id}/read
```

Responsibilities:

```txt
Message listing
Mark as read
```

---

# Security Decisions

Implemented:

```txt
JWT Authentication
Password Hashing
Rate Limiting
Protected Admin APIs
CORS Restriction
HTTPS Redirection
Exception Handling
```

---

# Important Architectural Decisions

# No Entity Exposure

Entities are never returned directly.

DTOs are always used.

---

# No Business Logic In Controllers

Controllers remain thin.

Business logic belongs in Services.

---

# No Hard Delete

Catalogs are deactivated instead of deleted.

---

# Save Before Email

Contact messages save to DB before sending emails.

---

# Snapshot Strategy

Lead stores CatalogNameSnapshot.

Protects historical data.

---

# Future Improvements

Recommended future improvements:

```txt
Refresh Tokens
Docker
Serilog
Redis Caching
Unit Testing
Audit Logging
Search & Filtering
Excel Export
CI/CD
Background Jobs
Cloud Storage
```

---

# Current Backend Status

The backend is now a production-style MVP backend supporting:

```txt
Authentication
Admin Dashboard
Catalog Management
Lead System
Contact Workflow
CSV Export
Geolocation
Email Integration
Pagination
Validation
Rate Limiting
Global Exception Handling
```

---

# Important Notes For Future Development

# When Adding Features

Follow this order:

```txt
DTO
Validator
Interface
Service
Repository
Controller
```

---

# Controllers Should

```txt
Receive request
Call service
Return response
```

No business logic.

---

# Services Should

```txt
Contain workflows
Contain business logic
Coordinate repositories
```

---

# Repositories Should

```txt
Only access database
Only use EF Core logic
```

---

# Recommended Frontend Stack

```txt
Next.js
Axios
Tailwind CSS
ShadCN UI
React Query
```

---

# Recommended Hosting

Backend:

```txt
Azure App Service
Render
Railway
IIS VPS
```

Frontend:

```txt
Vercel
```

Database:

```txt
SQL Server
Azure SQL
```

---

# Final Notes

This backend was intentionally designed as:

```txt
Simple
Scalable
Production-oriented
Cleanly layered
Frontend-friendly
Portfolio-level
```

The project uses a practical 3-tier architecture instead of full Clean Architecture to keep development speed high while maintaining strong separation of concerns.

The backend is now ready for:

```txt
Frontend integration
Deployment
Portfolio presentation
Client delivery
Future scaling
```

