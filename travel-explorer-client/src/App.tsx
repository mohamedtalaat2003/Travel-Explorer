import { Route, Routes } from 'react-router-dom'
import { ProtectedRoute } from './auth/ProtectedRoute'
import { DashboardLayout } from './layouts/DashboardLayout'
import { PublicLayout } from './layouts/PublicLayout'
import { ActivitiesAdminPage } from './pages/admin/ActivitiesAdminPage'
import { BlogsAdminPage } from './pages/admin/BlogsAdminPage'
import { BookingsAdminPage } from './pages/admin/BookingsAdminPage'
import { CategoriesAdminPage } from './pages/admin/CategoriesAdminPage'
import { DashboardPage } from './pages/admin/DashboardPage'
import { DestinationsAdminPage } from './pages/admin/DestinationsAdminPage'
import { FlightBookingsAdminPage } from './pages/admin/FlightBookingsAdminPage'
import { FlightsAdminPage } from './pages/admin/FlightsAdminPage'
import { MessagesAdminPage } from './pages/admin/MessagesAdminPage'
import { UsersPage } from './pages/admin/UsersPage'
import { GoogleCallbackPage } from './pages/auth/GoogleCallbackPage'
import { LoginPage } from './pages/auth/LoginPage'
import { RegisterPage } from './pages/auth/RegisterPage'
import { ForbiddenPage } from './pages/misc/ForbiddenPage'
import { NotFoundPage } from './pages/misc/NotFoundPage'
import { BlogDetailPage } from './pages/public/BlogDetailPage'
import { BlogsPage } from './pages/public/BlogsPage'
import { ContactPage } from './pages/public/ContactPage'
import { DestinationDetailPage } from './pages/public/DestinationDetailPage'
import { DestinationsPage } from './pages/public/DestinationsPage'
import { FlightDetailPage } from './pages/public/FlightDetailPage'
import { FlightsPage } from './pages/public/FlightsPage'
import { HomePage } from './pages/public/HomePage'
import { BlogEditorPage } from './pages/author/BlogEditorPage'
import { MyBlogsPage } from './pages/author/MyBlogsPage'
import { MyBookingsPage } from './pages/traveler/MyBookingsPage'
import { MyFlightBookingsPage } from './pages/traveler/MyFlightBookingsPage'
import { PaymentResultPage } from './pages/traveler/PaymentResultPage'
import { ProfilePage } from './pages/traveler/ProfilePage'

export default function App() {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route index element={<HomePage />} />
        <Route path="/destinations" element={<DestinationsPage />} />
        <Route path="/destinations/:id" element={<DestinationDetailPage />} />
        <Route path="/flights" element={<FlightsPage />} />
        <Route path="/flights/:id" element={<FlightDetailPage />} />
        <Route path="/blogs" element={<BlogsPage />} />
        <Route path="/blogs/:id" element={<BlogDetailPage />} />
        <Route path="/contact" element={<ContactPage />} />

        <Route
          path="/profile"
          element={
            <ProtectedRoute>
              <ProfilePage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/my/bookings"
          element={
            <ProtectedRoute allowedRoles={['Traveler']}>
              <MyBookingsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/my/flight-bookings"
          element={
            <ProtectedRoute allowedRoles={['Traveler']}>
              <MyFlightBookingsPage />
            </ProtectedRoute>
          }
        />
        <Route path="/payment/success" element={<PaymentResultPage success />} />
        <Route path="/payment/failed" element={<PaymentResultPage success={false} />} />

        <Route
          path="/author/blogs"
          element={
            <ProtectedRoute allowedRoles={['Author']}>
              <MyBlogsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/author/blogs/new"
          element={
            <ProtectedRoute allowedRoles={['Author']}>
              <BlogEditorPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/author/blogs/:id"
          element={
            <ProtectedRoute allowedRoles={['Author']}>
              <BlogEditorPage />
            </ProtectedRoute>
          }
        />
      </Route>

      <Route
        path="/admin"
        element={
          <ProtectedRoute allowedRoles={['Admin']}>
            <DashboardLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<DashboardPage />} />
        <Route path="users" element={<UsersPage />} />
        <Route path="destinations" element={<DestinationsAdminPage />} />
        <Route path="activities" element={<ActivitiesAdminPage />} />
        <Route path="categories" element={<CategoriesAdminPage />} />
        <Route path="flights" element={<FlightsAdminPage />} />
        <Route path="bookings" element={<BookingsAdminPage />} />
        <Route path="flight-bookings" element={<FlightBookingsAdminPage />} />
        <Route path="blogs" element={<BlogsAdminPage />} />
        <Route path="messages" element={<MessagesAdminPage />} />
      </Route>

      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/auth/google/login-callback" element={<GoogleCallbackPage />} />
      <Route path="/auth/google/register-callback" element={<GoogleCallbackPage />} />

      <Route path="/forbidden" element={<ForbiddenPage />} />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  )
}
