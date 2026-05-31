import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  ActivityDto,
  CreateDestinationRequest,
  DestinationDto,
  DestinationQuery,
  PaginatedResult,
  ReviewDto,
  UpdateDestinationRequest,
} from '@/types/api'

export const destinationsApi = {
  list: (q: DestinationQuery) =>
    api
      .get<PaginatedResult<DestinationDto>>('/Destinations', { params: cleanParams(q) })
      .then((r) => r.data),
  get: (id: number) => api.get<DestinationDto>(`/Destinations/${id}`).then((r) => r.data),
  topRated: (count = 6) =>
    api.get<DestinationDto[]>('/Destinations/top-rated', { params: { count } }).then((r) => r.data),
  activities: (id: number) =>
    api.get<ActivityDto[]>(`/Destinations/${id}/activities`).then((r) => r.data),
  reviews: (id: number) => api.get<ReviewDto[]>(`/Destinations/${id}/reviews`).then((r) => r.data),
  create: (body: CreateDestinationRequest) =>
    api.post<DestinationDto>('/Destinations', body).then((r) => r.data),
  update: (id: number, body: UpdateDestinationRequest) =>
    api.put<DestinationDto>(`/Destinations/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Destinations/${id}`).then((r) => r.data),
}
