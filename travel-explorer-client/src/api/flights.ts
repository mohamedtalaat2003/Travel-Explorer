import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  CreateFlightScheduleRequest,
  FlightQuery,
  FlightScheduleDto,
  PaginatedResult,
  UpdateFlightScheduleRequest,
} from '@/types/api'

export const flightsApi = {
  list: (q: FlightQuery) =>
    api
      .get<PaginatedResult<FlightScheduleDto>>('/Flights', { params: cleanParams(q) })
      .then((r) => r.data),
  get: (id: number) => api.get<FlightScheduleDto>(`/Flights/${id}`).then((r) => r.data),
  create: (body: CreateFlightScheduleRequest) =>
    api.post<FlightScheduleDto>('/Flights', body).then((r) => r.data),
  update: (id: number, body: UpdateFlightScheduleRequest) =>
    api.put<FlightScheduleDto>(`/Flights/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Flights/${id}`).then((r) => r.data),
}
