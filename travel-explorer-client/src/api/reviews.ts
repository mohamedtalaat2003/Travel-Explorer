import { api } from '@/lib/apiClient'
import type { CreateReviewRequest, ReviewDto, UpdateReviewRequest } from '@/types/api'

export const reviewsApi = {
  get: (id: number) => api.get<ReviewDto>(`/Reviews/${id}`).then((r) => r.data),
  create: (body: CreateReviewRequest) => api.post<ReviewDto>('/Reviews', body).then((r) => r.data),
  update: (id: number, body: UpdateReviewRequest) =>
    api.put<ReviewDto>(`/Reviews/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Reviews/${id}`).then((r) => r.data),
}
