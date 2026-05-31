// Types mirroring the Travel Explorer API. Enums are serialized as strings by the backend
// (JsonStringEnumConverter), so they are modeled as string unions.

export type BookingStatus = 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed' | 'Refunded'
export const BOOKING_STATUSES: BookingStatus[] = ['Pending', 'Confirmed', 'Cancelled', 'Completed', 'Refunded']

export type PaymentStatus = 'Unpaid' | 'Partial' | 'Paid' | 'Failed' | 'Refunded'

export type FlightClass = 'Economy' | 'Business' | 'FirstClass'
export const FLIGHT_CLASSES: FlightClass[] = ['Economy', 'Business', 'FirstClass']

export type Gender = 'Male' | 'Female' | 'Other'
export const GENDERS: Gender[] = ['Male', 'Female', 'Other']

export type AccountStatus = 'Pending' | 'Approved' | 'Rejected'

export type RequestToBeAuthor = 'Pending' | 'Approved' | 'Rejected'

export type AppRole = 'Admin' | 'Traveler' | 'Author'
export const APP_ROLES: AppRole[] = ['Admin', 'Traveler', 'Author']

export interface PaginatedResult<T> {
  items: T[]
  pageNumber: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

// ---- Auth ----
export interface TokenResponse {
  accessToken: string
  refreshToken: string
}

export interface LoginResponse {
  token: TokenResponse
}

export interface RegisterResponse {
  id: number
  userName: string
  email: string
  fullName: string
  role: string
}

export interface LoginRequest {
  userName: string
  password: string
}

export interface RegisterRequest {
  fullName: string
  userName: string
  email: string
  password: string
  confirmPassword: string
  iWantToBeAuthor: boolean
}

export interface RefreshTokenRequest {
  refreshToken: string
}

// ---- Catalog ----
export interface DestinationDto {
  id: number
  name: string
  description: string
  location: string
  pricePerNight: number
  averageRating: number
  reviewCount: number
  imageUrls: string[]
  categoryId: number
  categoryName: string
}

export interface CreateDestinationRequest {
  name: string
  description: string
  location: string
  pricePerNight: number
  imageUrls: string[]
  categoryId: number
}
export interface UpdateDestinationRequest extends CreateDestinationRequest {}

export interface ActivityDto {
  id: number
  name: string
  description: string
  icon: string
  imageUrls: string[]
  destinationId: number
  destinationName: string
}

export interface CreateActivityRequest {
  name: string
  description: string
  icon: string
  imageUrls: string[]
  destinationId: number
}
export interface UpdateActivityRequest extends CreateActivityRequest {}

export interface CategoryDto {
  id: number
  name: string
  description?: string | null
  iconUrl?: string | null
}

export interface CreateCategoryRequest {
  name: string
  description?: string | null
  iconUrl?: string | null
}
export interface UpdateCategoryRequest extends CreateCategoryRequest {}

// ---- Blogs ----
export interface BlogDto {
  id: number
  title: string
  content: string
  imageUrl: string
  isPublished: boolean
  authorId: number
  categoryId?: number | null
  createdAt: string
}

export interface CreateBlogRequest {
  title: string
  content: string
  imageUrl: string
  isPublished: boolean
  categoryId?: number | null
}
export interface UpdateBlogRequest extends CreateBlogRequest {}

// ---- Reviews ----
export interface ReviewDto {
  id: number
  rating: number
  comment?: string | null
  userId: number
  userFullName: string
  destinationId: number
  createdAt: string
  imageUrls: string[]
}

export interface CreateReviewRequest {
  rating: number
  comment: string
  destinationId: number
  imageUrls?: string[]
}

export interface UpdateReviewRequest {
  rating: number
  comment?: string | null
  imageUrls?: string[]
}

// ---- Destination bookings ----
export interface DestinationBookingDto {
  id: number
  checkInDate: string
  checkOutDate: string
  numberOfGuests: number
  totalPrice: number
  status: BookingStatus
  notes?: string | null
  userId: number
  userFullName: string
  destinationId: number
  destinationName: string
  createdAt: string
}

export interface CreateBookingRequest {
  checkInDate: string
  checkOutDate: string
  numberOfGuests: number
  destinationId: number
  notes?: string | null
}

// ---- Flights ----
export interface FlightScheduleDto {
  id: number
  airline: string
  flightNumber: string
  departureCity: string
  arrivalCity: string
  departureTime: string
  arrivalTime: string
  economyPrice: number
  businessPrice: number
  firstClassPrice: number
  availableEconomySeats: number
  availableBusinessSeats: number
  availableFirstClassSeats: number
}

export interface CreateFlightScheduleRequest {
  airline: string
  flightNumber: string
  departureCity: string
  arrivalCity: string
  departureTime: string
  arrivalTime: string
  economyPrice: number
  businessPrice: number
  firstClassPrice: number
  availableEconomySeats: number
  availableBusinessSeats: number
  availableFirstClassSeats: number
}
export interface UpdateFlightScheduleRequest extends CreateFlightScheduleRequest {}

export interface FlightBookingDto {
  id: number
  class: FlightClass
  numberOfPassengers: number
  totalPrice: number
  status: BookingStatus
  seatPreference?: string | null
  gender: Gender
  nationality?: string | null
  userId: number
  userFullName: string
  flightScheduleId: number
  airline: string
  flightNumber: string
  departureTime: string
}

export interface CreateFlightBookingRequest {
  class: FlightClass
  numberOfPassengers: number
  flightScheduleId: number
  seatPreference?: string | null
  gender: Gender
  nationality?: string | null
}

// ---- Payments ----
export interface CheckoutResponse {
  checkoutUrl: string
}

// ---- Contact ----
export interface ContactMessageDto {
  id: number
  fullName: string
  email: string
  subject: string
  message: string
  isRead: boolean
  userId?: number | null
  createdAt: string
}

export interface CreateContactMessageRequest {
  fullName: string
  email: string
  subject: string
  message: string
}

// ---- Profile ----
export interface UserProfileDto {
  userId: number
  fullName: string
  email: string
  bio?: string | null
  avatarUrl?: string | null
  phoneNumber?: string | null
  country?: string | null
  dateOfBirth?: string | null
  passportNumber?: string | null
}

export interface UpdateUserProfileRequest {
  fullName: string
  email: string
  phoneNumber: string
  passportNumber: string
  bio?: string | null
  avatarUrl?: string | null
  country?: string | null
  dateOfBirth?: string | null
}

// ---- Admin / Users ----
export interface UserDto {
  id: number
  userName: string
  email?: string | null
  gender?: Gender | null
  isBlocked: boolean
  role: string
  status: string
  requestToBeAuthor: string
}

export interface UserDetailsDto extends UserDto {
  createdAt: string
}

export interface AdminStatisticsDto {
  totalUsers: number
  activeUsers: number
  totalBookings: number
}

export interface ChangeRoleRequest {
  newRole: string
}

// ---- Upload ----
export interface UploadResult {
  url: string
  publicId: string
}

// ---- Query params ----
export interface PaginationParams {
  pageNumber?: number
  pageSize?: number
}

export interface DestinationQuery extends PaginationParams {
  keyword?: string
  location?: string
  minPrice?: number
  maxPrice?: number
  categoryId?: number
}

export interface BlogQuery extends PaginationParams {
  keyword?: string
  authorId?: number
  categoryId?: number
}

export interface FlightQuery extends PaginationParams {
  departureCity?: string
  arrivalCity?: string
  departureDate?: string
}

export interface CategoryQuery extends PaginationParams {
  name?: string
}

export interface FlightBookingQuery extends PaginationParams {
  status?: BookingStatus
}

export interface ContactMessageQuery extends PaginationParams {
  isRead?: boolean
  keyword?: string
}

export interface UserQuery extends PaginationParams {
  searchTerm?: string
  gender?: Gender
  isBlocked?: boolean
  status?: AccountStatus
  authorRequestStatus?: RequestToBeAuthor
}
