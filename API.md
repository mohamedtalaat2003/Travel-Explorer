# Travel Explorer API reference

Base URL: `/api` (dev: `https://localhost:7133/api`). All bodies/responses are JSON,
camelCase, with enums serialized as strings (e.g. `"Pending"`, `"Economy"`).

## Conventions

- Auth: send `Authorization: Bearer <accessToken>`. Roles are `Admin`, `Traveler`, `Author`.
- Paginated list endpoints return:
  ```json
  { "items": [], "pageNumber": 1, "pageSize": 10, "totalCount": 0, "totalPages": 0,
    "hasPreviousPage": false, "hasNextPage": false }
  ```
  Common query params: `pageNumber` (default 1), `pageSize` (default 10, max 50).
- Error responses are JSON problem details (PascalCase from the exception middleware):
  ```json
  { "Status": 400, "Type": "ValidationFailure",
    "Title": "One or more validation errors occurred.",
    "Instance": "/api/Reviews", "Errors": { "Rating": ["Rating must be between 1 and 5."] } }
  ```
  Status codes used: 200, 201, 204, 400, 401, 403, 404, 409, 500.

---

## Auth - `/api/Auth`

### POST /api/Auth/register  (anonymous)
Request:
```json
{ "fullName": "Jane Traveler", "userName": "janet", "email": "jane@example.com",
  "password": "Passw0rd!", "confirmPassword": "Passw0rd!", "iWantToBeAuthor": false }
```
Success `200`:
```json
{ "id": 5, "userName": "janet", "email": "jane@example.com", "fullName": "Jane Traveler", "role": "Traveler" }
```
Error `400`: `"User already exists."`

### POST /api/Auth/login  (anonymous)
Request: `{ "userName": "janet", "password": "Passw0rd!" }`
Success `200`:
```json
{ "token": { "accessToken": "<jwt>", "refreshToken": "<token>" } }
```
Error `401`: `"Invalid username or password."`

### POST /api/Auth/refresh-token  (anonymous)
Request: `{ "refreshToken": "<token>" }`
Success `200`: `{ "accessToken": "<jwt>", "refreshToken": "<token>" }`
Error `401`: `"Invalid or expired refresh token"`

### POST /api/Auth/logout  (authenticated)
Request: `{ "refreshToken": "<token>" }` -> `200 "Logged out successfully."`

### POST /api/Auth/assign-role?IWantToBeAuthor=false  (Admin)
Request: `{ "userId": 5, "newRole": "Author" }` -> `200` token response.

### GET /api/Auth/google-login | google-register  (anonymous)
Browser redirect to Google; the callback redirects back to the SPA URLs configured in
`JwtSettings:GoogleFrontend*RedirectUrl` (login callback carries `accessToken` &
`refreshToken` query params). Requires Google client id/secret.

---

## Destinations - `/api/Destinations`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/` | anon | query: `keyword, location, minPrice, maxPrice, categoryId, pageNumber, pageSize` -> `PaginatedResult<DestinationDto>` |
| GET | `/{id}` | anon | `DestinationDto` |
| GET | `/top-rated?count=6` | anon | `DestinationDto[]` |
| GET | `/{id}/activities` | anon | `ActivityDto[]` |
| GET | `/{id}/reviews` | anon | `ReviewDto[]` |
| POST | `/` | Admin | -> `201 DestinationDto` |
| PUT | `/{id}` | Admin | -> `200 DestinationDto` |
| DELETE | `/{id}` | Admin | -> `204` |

`DestinationDto`:
```json
{ "id": 1, "name": "Sharm Resort", "description": "...", "location": "South Sinai",
  "pricePerNight": 120.0, "averageRating": 4.5, "reviewCount": 12,
  "imageUrls": ["https://..."], "categoryId": 2, "categoryName": "Beach" }
```
Create/Update body:
```json
{ "name": "Sharm Resort", "description": "...", "location": "South Sinai",
  "pricePerNight": 120.0, "imageUrls": ["https://..."], "categoryId": 2 }
```

---

## Activities - `/api/Activities`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/?destinationId=&pageNumber=&pageSize=` | anon | `PaginatedResult<ActivityDto>` |
| GET | `/{id}` | anon | `ActivityDto` |
| POST | `/` | Admin | `201 ActivityDto` |
| PUT | `/{id}` | Admin | `200 ActivityDto` |
| DELETE | `/{id}` | Admin | `204` |

Create/Update body:
```json
{ "name": "Scuba diving", "description": "...", "icon": "diving",
  "imageUrls": ["https://..."], "destinationId": 1 }
```

---

## Categories - `/api/Categories`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/?name=&pageNumber=&pageSize=` | anon | `PaginatedResult<CategoryDto>` |
| GET | `/{id}` | anon | `CategoryDto` |
| POST | `/` | Admin | `201 CategoryDto` |
| PUT | `/{id}` | Admin | `200 CategoryDto` |
| DELETE | `/{id}` | Admin | `204` |

Create/Update body: `{ "name": "Beach", "description": "...", "iconUrl": "https://..." }`

---

## Blogs - `/api/Blogs`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/?keyword=&authorId=&categoryId=&pageNumber=&pageSize=` | anon | published only -> `PaginatedResult<BlogDto>` |
| GET | `/{id}` | anon | published only -> `BlogDto` |
| POST | `/` | Author | `201 BlogDto` (author taken from token) |
| PUT | `/{id}` | Author | `200 BlogDto` (own posts) |
| DELETE | `/{id}` | Admin | `204` |

Create/Update body:
```json
{ "title": "Top 10 beaches", "content": "...", "imageUrl": "https://...",
  "isPublished": true, "categoryId": 2 }
```

---

## Reviews - `/api/Reviews`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/{id}` | anon | `ReviewDto` |
| POST | `/` | Traveler | `201 ReviewDto`; `409` if you already reviewed the destination |
| PUT | `/{id}` | Traveler | `200 ReviewDto` (own) |
| DELETE | `/{id}` | Admin | `204` |

Create body: `{ "rating": 5, "comment": "Great!", "destinationId": 1, "imageUrls": [] }`
Update body: `{ "rating": 4, "comment": "Good", "imageUrls": [] }`

---

## Destination bookings - `/api/DestinationBookings`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| POST | `/` | Traveler | create -> `201 DestinationBookingDto` |
| GET | `/{id}` | authenticated | owner or Admin |
| GET | `/my?status=` | Traveler | `DestinationBookingDto[]` |
| GET | `/?status=` | Admin | `DestinationBookingDto[]` |
| PATCH | `/{id}/status` | Admin | body `{ "status": "Confirmed" }` |
| PATCH | `/{id}` | Traveler | body `{ "notes": "late check-in" }` |
| PATCH | `/{id}/cancel` | authenticated | owner or Admin -> `true` |
| DELETE | `/{id}` | Admin | `204` |

Create body:
```json
{ "checkInDate": "2026-07-01", "checkOutDate": "2026-07-05",
  "numberOfGuests": 2, "destinationId": 1, "notes": "" }
```

---

## Flights - `/api/Flights`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/?departureCity=&arrivalCity=&departureDate=&pageNumber=&pageSize=` | anon | `PaginatedResult<FlightScheduleDto>` |
| GET | `/{id}` | anon | `FlightScheduleDto` |
| POST | `/` | Admin | `201 FlightScheduleDto` |
| PUT | `/{id}` | Admin | `200 FlightScheduleDto` |
| DELETE | `/{id}` | Admin | `204` |

Create/Update body:
```json
{ "airline": "EgyptAir", "flightNumber": "MS800", "departureCity": "Cairo",
  "arrivalCity": "Dubai", "departureTime": "2026-07-01T10:00:00",
  "arrivalTime": "2026-07-01T14:00:00", "economyPrice": 200, "businessPrice": 500,
  "firstClassPrice": 900, "availableEconomySeats": 120,
  "availableBusinessSeats": 20, "availableFirstClassSeats": 8 }
```

---

## Flight bookings - `/api/FlightBookings`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/?status=&pageNumber=&pageSize=` | Admin | `PaginatedResult<FlightBookingDto>` |
| GET | `/my?status=&pageNumber=&pageSize=` | Traveler | `PaginatedResult<FlightBookingDto>` |
| GET | `/{id}` | authenticated | owner or Admin |
| POST | `/` | Traveler | `201 FlightBookingDto` |
| PATCH | `/{id}/status` | Admin | body is a raw JSON enum string, e.g. `"Confirmed"` |
| PATCH | `/{id}/cancel` | authenticated | owner or Admin -> `true` |

Create body:
```json
{ "class": "Economy", "numberOfPassengers": 1, "flightScheduleId": 3,
  "seatPreference": "window", "gender": "Female", "nationality": "Egyptian" }
```

---

## Payments - `/api/payments`

### POST /api/payments/checkout/{bookingId}?provider=Paymob  (Traveler)
Creates a payment session for a **destination** booking. Success `200`:
```json
{ "checkoutUrl": "https://accept.paymob.com/unifiedcheckout/?..." }
```
Error `400`: `ProblemDetails` with `Title: "Checkout Failed"`. Requires real Paymob keys
to complete.

### POST /api/payments/webhook/{provider}  (anonymous, server-to-server)
Called by the payment provider. HMAC-verified by middleware; not called by the SPA.

---

## Contact messages - `/api/ContactMessages`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| POST | `/` | anon | `201 ContactMessageDto` |
| GET | `/?isRead=&keyword=&pageNumber=&pageSize=` | Admin | `PaginatedResult<ContactMessageDto>` |
| GET | `/{id}` | authenticated | owner or Admin |
| PATCH | `/{id}` | Admin | mark read -> `true` |
| DELETE | `/{id}` | Admin | `204` |

Create body: `{ "fullName": "Jane", "email": "jane@example.com", "subject": "Hi", "message": "..." }`

---

## User profile - `/api/UserProfile`

| Method | Path | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/` | authenticated | `UserProfileDto`; `404` if not created yet |
| PUT | `/` | authenticated | upsert -> `200 UserProfileDto` |
| POST | `/request-author` | authenticated | `200 { "message": "..." }` |

PUT body:
```json
{ "fullName": "Jane", "email": "jane@example.com", "phoneNumber": "+20100...",
  "passportNumber": "A1234567", "bio": "", "avatarUrl": "", "country": "Egypt",
  "dateOfBirth": "1995-05-20" }
```

---

## Admin - `/api/Admin`  (all Admin)

| Method | Path | Notes |
| --- | --- | --- |
| GET | `/?searchTerm=&gender=&isBlocked=&status=&authorRequestStatus=&pageNumber=&pageSize=` | `UserDto[]` + `X-Pagination` response header `{ totalCount, PageNumber, PageSize }` |
| GET | `/{id}` | `UserDetailsDto` |
| PUT | `/{id}/block` | `200 { message }` |
| PUT | `/{id}/unblock` | `200 { message }` |
| PUT | `/{id}/role` | body `{ "newRole": "Author" }` -> `200 { message }` |
| DELETE | `/{id}` | soft delete -> `200 { message }` |
| GET | `/statistics` | `{ "totalUsers": 0, "activeUsers": 0, "totalBookings": 0 }` |
| GET | `/pending-author-requests` | `UserDto[]` |
| PUT | `/{id}/approve-author` | `200 { message }` |
| PUT | `/{id}/reject-author` | `200 { message }` |

---

## Upload - `/api/Upload`  (authenticated)

| Method | Path | Body | Response |
| --- | --- | --- | --- |
| POST | `/image` | multipart, field `file` | `{ "url": "https://...", "publicId": "..." }` |
| POST | `/images` | multipart, field `files` (multiple) | `[{ "url": "...", "publicId": "..." }]` |

Max 5 MB; `.jpg .jpeg .png .webp`. Requires Cloudinary credentials on the backend.
