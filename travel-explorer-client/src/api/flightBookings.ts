import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  BookingStatus,
  CreateFlightBookingRequest,
  FlightBookingDto,
  FlightBookingQuery,
  PaginatedResult,
} from '@/types/api'

export const flightBookingsApi = {
  all: (q: FlightBookingQuery) =>
    api
      .get<PaginatedResult<FlightBookingDto>>('/FlightBookings', { params: cleanParams(q) })
      .then((r) => r.data),
  mine: (q: FlightBookingQuery) =>
    api
      .get<PaginatedResult<FlightBookingDto>>('/FlightBookings/my', { params: cleanParams(q) })
      .then((r) => r.data),
  get: (id: number) => api.get<FlightBookingDto>(`/FlightBookings/${id}`).then((r) => r.data),
  create: (body: CreateFlightBookingRequest) =>
    api.post<FlightBookingDto>('/FlightBookings', body).then((r) => r.data),
  // The endpoint binds [FromBody] BookingStatus, so the body is a raw JSON string ("Confirmed").
  updateStatus: (id: number, status: BookingStatus) =>
    api
      .patch<FlightBookingDto>(`/FlightBookings/${id}/status`, JSON.stringify(status), {
        headers: { 'Content-Type': 'application/json' },
      })
      .then((r) => r.data),
  cancel: (id: number) => api.patch<boolean>(`/FlightBookings/${id}/cancel`).then((r) => r.data),
}
