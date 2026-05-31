# Travel Explorer - Frontend

A React + Vite + TypeScript + Tailwind single-page app for the Travel Explorer API. It covers
public browsing plus the Traveler, Author, and Admin roles, with JWT access/refresh auth and
role-gated routing. The UI uses a warm, editorial design language (cream canvas, clay accent,
serif display headings via Fraunces) with subtle scroll-reveal and hover animations.

## Prerequisites

- Node.js 18+ and npm
- The Travel Explorer .NET API running (see the solution at the repo root)
- A PostgreSQL database configured for the API

## Quick start

```bash
cd travel-explorer-client
npm install
npm run dev
```

Then open http://localhost:5173.

By default the dev server proxies all `/api` requests to the backend at `https://localhost:7133`
(see `vite.config.ts`), so you do not need to deal with CORS or the self-signed dev certificate.

### Environment variables (`.env`)

| Variable | Purpose |
| --- | --- |
| `VITE_API_BASE_URL` | Leave empty in dev to use the proxy. In production set it to the API origin, e.g. `https://api.example.com`. |
| `VITE_DEV_API_TARGET` | The backend URL the dev proxy forwards `/api` to. Default `https://localhost:7133`. |

If your backend runs on a different port, set `VITE_DEV_API_TARGET` accordingly (e.g.
`http://localhost:5015`).

## Default admin login

The backend seeds a default admin on startup (configurable in the API's `appsettings.json`):

- Username: `admin`
- Password: `Admin@123`

Use it to access the Admin dashboard at `/admin`. On a fresh database the backend also seeds demo
content (destinations, flights, blogs, reviews) and demo accounts—an author (`maya.lawson`) and
travelers like `liam.carter`—all with the password `Password@123`.

## Scripts

- `npm run dev` - start the dev server
- `npm run build` - type-check and build for production (output in `dist/`)
- `npm run preview` - preview the production build
- `npm run typecheck` - type-check only

## Features by role

- Public: home, destinations (search/filter), destination details with activities and reviews,
  flights (search) and details, blog list and articles, contact form.
- Traveler: create stay and flight bookings, My Trips (cancel, edit notes, pay), write/edit reviews,
  manage profile, request the Author role.
- Author: create/edit/publish own blog posts (with image upload).
- Admin: dashboard stats, user management (block/unblock/role/delete), approve/reject author
  requests, full CRUD for destinations/activities/categories/flights, manage bookings and flight
  bookings, delete blogs, and a contact-message inbox.

## Architecture

- `src/types/api.ts` - types mirroring the API DTOs/enums
- `src/lib/apiClient.ts` - axios instance with JWT attach + single-flight refresh-token interceptor
- `src/api/*` - one typed module per endpoint area
- `src/auth/*` - auth context (JWT decode) and role-based route guard
- `src/components/*` - reusable UI primitives and widgets
- `src/layouts/*` - public and dashboard layouts
- `src/pages/*` - pages grouped by `public`, `auth`, `traveler`, `author`, `admin`

## External services (needed for full end-to-end)

Some flows require credentials configured on the backend:

- Image upload uses Cloudinary - set Cloudinary config in the API. The upload widgets also accept a
  pasted image URL, so content management works without Cloudinary.
- Google sign-in needs Google OAuth client id/secret on the backend, plus the backend
  `JwtSettings:GoogleFrontend*RedirectUrl` values pointing at this app (already set to
  `http://localhost:5173/auth/google/...` for dev). Until those are configured, the
  "Continue with Google" button shows a "coming soon" notice instead of starting the flow.
- Online payments use Paymob - real keys and a return URL are required. The checkout UI is wired up
  and redirects to the gateway; with mock keys the payment will not complete.

## Notes

- For a production cross-origin deployment (not using the dev proxy), enable CORS on the API for the
  app origin and expose the `X-Pagination` response header so the admin user list can read totals.
