# Arab River API Guide (Business + Frontend Integration)

This guide explains every endpoint, its business purpose, and how the frontend should use it.

## Quick Start

- Base URL (HTTP): `http://localhost:5233`
- Base URL (HTTPS): `https://localhost:7136`
- Swagger UI (dev): `https://localhost:7136/swagger`

## Shared Conventions

### Unified Success Response

```json
{
  "success": true,
  "message": "Success message",
  "data": {}
}
```

### Validation Error Response

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": []
}
```

### Pagination Response (Admin list endpoints)

```json
{
  "items": [],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 0,
  "totalPages": 0
}
```

### Auth Header (Admin endpoints)

```
Authorization: Bearer <JWT_TOKEN>
```

### Rate Limits

- `LoginPolicy`: 5 requests/min
- `ContactPolicy`: 10 requests/min
- `LeadPolicy`: 15 requests/min

---

# Public Endpoints

## 1) Admin Login

**POST** `/api/admin/auth/login`

**Business purpose**: Authenticate the admin and return a JWT token for protected APIs.

**Frontend use**:
- Call on admin login form submit.
- Store returned JWT in a secure storage (memory, cookie, or secure storage).

**Rate limit:** `LoginPolicy`

**Request Body**

```json
{
  "email": "admin@arabriver.com",
  "password": "Admin@123"
}
```

**Success Response**

```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "<jwt>",
    "expiration": "2025-01-01T12:00:00Z"
  }
}
```

---

## 2) Get Current Admin

**GET** `/api/admin/auth/me`

**Business purpose**: Resolve the current admin profile from the JWT.

**Frontend use**:
- Call on admin app boot to show admin name/email.
- Use to verify a stored token is still valid.

**Auth:** Required

**Success Response**

```json
{
  "success": true,
  "message": "Admin retrieved successfully",
  "data": {
    "id": "<guid>",
    "name": "Admin Name",
    "email": "admin@arabriver.com",
    "role": "Admin"
  }
}
```

---

## 3) Get Active Catalogs

**GET** `/api/catalogs`

**Business purpose**: Public catalog listing for site browsing.

**Frontend use**:
- Call on catalog listing page.
- Display only active catalogs.

**Success Response**

```json
{
  "success": true,
  "message": "Catalogs retrieved successfully",
  "data": [
    {
      "id": "<guid>",
      "name": "Catalog A",
      "description": "...",
      "googleDriveLink": "https://...",
      "thumbnailUrl": "https://...",
      "category": "Dental",
      "displayOrder": 1,
      "isActive": true
    }
  ]
}
```

---

## 4) Get Catalog By Id

**GET** `/api/catalogs/{id}`

**Business purpose**: Show catalog details by id.

**Frontend use**:
- Call on catalog details page.

**Success Response** (same catalog schema as above)

---

## 5) Get Lead Location

**GET** `/api/lead/location`

**Business purpose**: Detect visitor location before any lead/download actions.

**Frontend use**:
- Call when user lands on the site.
- If `isEgypt` is `false`, show the outside‑Egypt modal.

**Success Response**

```json
{
  "success": true,
  "message": "Location retrieved successfully",
  "data": {
    "ipAddress": "<ip>",
    "country": "Egypt",
    "countryCode": "EG",
    "isEgypt": true
  }
}
```

---

## 6) Create Outside‑Egypt Visitor

**POST** `/api/lead/outside-egypt`

**Business purpose**: Capture outside‑Egypt visitors (name + optional clinic/hospital) and notify admin by email.

**Frontend use**:
- Trigger only if `Get Lead Location` returns `isEgypt = false`.
- Submit modal data (name required, clinic/hospital optional).

**Rate limit:** `LeadPolicy`

**Request Body**

```json
{
  "name": "John Doe",
  "organizationName": "Clinic Name"
}
```

**Success Response**

```json
{
  "success": true,
  "message": "Outside Egypt visitor sent successfully",
  "data": null
}
```

---

## 7) Create Lead + Get Download URL

**POST** `/api/leads`

**Business purpose**: Capture download leads and return the catalog URL.

**Frontend use**:
- Use on download button flow.
- After success, redirect the user to `downloadUrl`.

**Rate limit:** `LeadPolicy`

**Request Body**

```json
{
  "firstName": "Mina",
  "phoneNumber": "+201234567890",
  "organizationName": "Clinic Name",
  "catalogId": "<guid>"
}
```

**Success Response**

```json
{
  "success": true,
  "message": "Lead created successfully",
  "data": {
    "downloadUrl": "https://drive.google.com/..."
  }
}
```

---

## 8) Contact Form

**POST** `/api/contact`

**Business purpose**: Submit contact messages and trigger email notifications.

**Frontend use**:
- Use on the contact form.
- Show success toast on completion.

**Rate limit:** `ContactPolicy`

**Request Body**

```json
{
  "name": "User Name",
  "email": "user@email.com",
  "message": "Hello"
}
```

**Success Response**

```json
{
  "success": true,
  "message": "Message sent successfully",
  "data": null
}
```

---

# Admin Endpoints (JWT Required)

## 9) Get All Catalogs (Paged)

**GET** `/api/admin/catalogs?pageNumber=1&pageSize=10`

**Business purpose**: Admin catalog list and management view.

**Frontend use**:
- Use in admin catalog table with pagination controls.

---

## 10) Create Catalog

**POST** `/api/admin/catalogs`

**Business purpose**: Admin creates new catalog.

**Frontend use**:
- Use on admin catalog creation form.

**Request Body**

```json
{
  "name": "New Catalog",
  "description": "Optional",
  "googleDriveLink": "https://...",
  "thumbnailUrl": "https://...",
  "category": "Optional",
  "displayOrder": 1
}
```

**Success Response**

```json
{
  "success": true,
  "message": "Catalog created successfully",
  "data": null
}
```

---

## 11) Update Catalog

**PUT** `/api/admin/catalogs/{id}`

**Business purpose**: Admin edits catalog content.

**Frontend use**:
- Use on admin edit form.

**Request Body** (same shape as create)

---

## 12) Activate / Deactivate Catalog

**PATCH** `/api/admin/catalogs/{id}/status`

**Business purpose**: Soft deactivate catalogs without deleting them.

**Frontend use**:
- Use as a toggle in admin catalog list.

**Request Body**

```json
{
  "isActive": true
}
```

---

## 13) Get All Leads (Paged)

**GET** `/api/admin/leads?pageNumber=1&pageSize=10`

**Business purpose**: Admin view for download leads.

**Frontend use**:
- Use in admin leads table with pagination.

**Response Item Fields**

```json
{
  "id": "<guid>",
  "firstName": "Mina",
  "phoneNumber": "+201234567890",
  "organizationName": "Clinic Name",
  "country": "Egypt",
  "isEgypt": true,
  "catalogNameSnapshot": "Catalog A",
  "createdAt": "2025-01-01T12:00:00Z"
}
```

---

## 14) Export Leads CSV

**GET** `/api/admin/leads/export`

**Business purpose**: Export all leads for offline analysis.

**Frontend use**:
- Trigger download action in admin leads page.

**Response:** file download (`text/csv`) with UTF-8 encoding.

---

## 15) Get All Contact Messages (Paged)

**GET** `/api/admin/contact-messages?pageNumber=1&pageSize=10`

**Business purpose**: Admin inbox for contact messages.

**Frontend use**:
- Use in admin contact inbox page.

**Response Item Fields**

```json
{
  "id": "<guid>",
  "name": "User",
  "email": "user@email.com",
  "message": "Hello",
  "status": "New",
  "createdAt": "2025-01-01T12:00:00Z"
}
```

---

## 16) Mark Contact Message As Read

**PATCH** `/api/admin/contact-messages/{id}/read`

**Business purpose**: Mark a contact message as read.

**Frontend use**:
- Use on message open or explicit "Mark as read" action.

---

## 17) Get All Outside‑Egypt Visitors (Paged)

**GET** `/api/admin/outside-egypt-visitors?pageNumber=1&pageSize=10`

**Business purpose**: Admin list for outside‑Egypt visitors from the pre‑browse modal.

**Frontend use**:
- Use in a dedicated admin page for outside‑Egypt visitors.

**Response Item Fields**

```json
{
  "id": "<guid>",
  "name": "John Doe",
  "organizationName": "Clinic Name",
  "country": "USA",
  "countryCode": "US",
  "ipAddress": "<ip>",
  "createdAt": "2025-01-01T12:00:00Z"
}
```

---

# Frontend Integration Notes

- Always send JWT in `Authorization` header for admin endpoints.
- Use camelCase JSON when posting data.
- Use `GET /api/lead/location` on site load to decide whether to show the outside‑Egypt modal.
- Trigger `POST /api/lead/outside-egypt` only for non‑Egypt visitors.
- When creating a lead, use the returned `downloadUrl` to redirect the user.
- For paged endpoints, handle `pageNumber`, `pageSize`, `totalCount`, and `totalPages`.
- Catalogs are deactivated instead of deleted.

# Testing Tips

- Use Swagger for quick manual testing.
- For frontend integration, test public endpoints first, then log in and store the token to access admin APIs.
- Respect rate limits to avoid `429` responses.
