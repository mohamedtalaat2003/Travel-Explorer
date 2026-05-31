import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type { AdminStatisticsDto, UserDetailsDto, UserDto, UserQuery } from '@/types/api'

export interface PaginationMeta {
  totalCount: number
  pageNumber: number
  pageSize: number
}

export const adminApi = {
  users: async (q: UserQuery): Promise<{ items: UserDto[]; meta: PaginationMeta }> => {
    const res = await api.get<UserDto[]>('/Admin', { params: cleanParams(q) })
    let meta: PaginationMeta = {
      totalCount: res.data.length,
      pageNumber: q.pageNumber ?? 1,
      pageSize: q.pageSize ?? 10,
    }
    const header = res.headers['x-pagination']
    if (header) {
      try {
        const parsed = JSON.parse(header)
        meta = {
          totalCount: parsed.totalCount ?? parsed.TotalCount ?? res.data.length,
          pageNumber: parsed.pageNumber ?? parsed.PageNumber ?? meta.pageNumber,
          pageSize: parsed.pageSize ?? parsed.PageSize ?? meta.pageSize,
        }
      } catch {
        /* keep fallback meta */
      }
    }
    return { items: res.data, meta }
  },
  user: (id: number) => api.get<UserDetailsDto>(`/Admin/${id}`).then((r) => r.data),
  block: (id: number) => api.put(`/Admin/${id}/block`).then((r) => r.data),
  unblock: (id: number) => api.put(`/Admin/${id}/unblock`).then((r) => r.data),
  changeRole: (id: number, newRole: string) =>
    api.put(`/Admin/${id}/role`, { newRole }).then((r) => r.data),
  remove: (id: number) => api.delete(`/Admin/${id}`).then((r) => r.data),
  statistics: () => api.get<AdminStatisticsDto>('/Admin/statistics').then((r) => r.data),
  pendingAuthorRequests: () =>
    api.get<UserDto[]>('/Admin/pending-author-requests').then((r) => r.data),
  approveAuthor: (id: number) => api.put(`/Admin/${id}/approve-author`).then((r) => r.data),
  rejectAuthor: (id: number) => api.put(`/Admin/${id}/reject-author`).then((r) => r.data),
}
