import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type { BookingStatus, CreateBookingRequest, DestinationBookingDto } from '@/types/api'

export const destinationBookingsApi = {
  create: (body: CreateBookingRequest) =>
    api.post<DestinationBookingDto>('/DestinationBookings', body).then((r) => r.data),
  get: (id: number) =>
    api.get<DestinationBookingDto>(`/DestinationBookings/${id}`).then((r) => r.data),
  mine: (status?: BookingStatus) =>
    api
      .get<DestinationBookingDto[]>('/DestinationBookings/my', { params: cleanParams({ status }) })
      .then((r) => r.data),
  all: (status?: BookingStatus) =>
    api
      .get<DestinationBookingDto[]>('/DestinationBookings', { params: cleanParams({ status }) })
      .then((r) => r.data),
  updateStatus: (id: number, status: BookingStatus) =>
    api
      .patch<DestinationBookingDto>(`/DestinationBookings/${id}/status`, { status })
      .then((r) => r.data),
  updateNotes: (id: number, notes: string) =>
    api.patch<DestinationBookingDto>(`/DestinationBookings/${id}`, { notes }).then((r) => r.data),
  cancel: (id: number) =>
    api.patch<boolean>(`/DestinationBookings/${id}/cancel`).then((r) => r.data),
  remove: (id: number) => api.delete(`/DestinationBookings/${id}`).then((r) => r.data),
}
