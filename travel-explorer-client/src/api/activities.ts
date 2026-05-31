import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  ActivityDto,
  CreateActivityRequest,
  PaginatedResult,
  PaginationParams,
  UpdateActivityRequest,
} from '@/types/api'

export const activitiesApi = {
  list: (params: PaginationParams & { destinationId?: number }) =>
    api
      .get<PaginatedResult<ActivityDto>>('/Activities', { params: cleanParams(params) })
      .then((r) => r.data),
  get: (id: number) => api.get<ActivityDto>(`/Activities/${id}`).then((r) => r.data),
  create: (body: CreateActivityRequest) =>
    api.post<ActivityDto>('/Activities', body).then((r) => r.data),
  update: (id: number, body: UpdateActivityRequest) =>
    api.put<ActivityDto>(`/Activities/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Activities/${id}`).then((r) => r.data),
}
