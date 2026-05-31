import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  ContactMessageDto,
  ContactMessageQuery,
  CreateContactMessageRequest,
  PaginatedResult,
} from '@/types/api'

export const contactApi = {
  create: (body: CreateContactMessageRequest) =>
    api.post<ContactMessageDto>('/ContactMessages', body).then((r) => r.data),
  list: (q: ContactMessageQuery) =>
    api
      .get<PaginatedResult<ContactMessageDto>>('/ContactMessages', { params: cleanParams(q) })
      .then((r) => r.data),
  get: (id: number) => api.get<ContactMessageDto>(`/ContactMessages/${id}`).then((r) => r.data),
  markRead: (id: number) => api.patch<boolean>(`/ContactMessages/${id}`).then((r) => r.data),
  remove: (id: number) => api.delete(`/ContactMessages/${id}`).then((r) => r.data),
}
