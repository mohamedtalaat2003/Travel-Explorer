import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  BlogDto,
  BlogQuery,
  CreateBlogRequest,
  PaginatedResult,
  UpdateBlogRequest,
} from '@/types/api'

export const blogsApi = {
  list: (q: BlogQuery) =>
    api.get<PaginatedResult<BlogDto>>('/Blogs', { params: cleanParams(q) }).then((r) => r.data),
  get: (id: number) => api.get<BlogDto>(`/Blogs/${id}`).then((r) => r.data),
  create: (body: CreateBlogRequest) => api.post<BlogDto>('/Blogs', body).then((r) => r.data),
  update: (id: number, body: UpdateBlogRequest) =>
    api.put<BlogDto>(`/Blogs/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Blogs/${id}`).then((r) => r.data),
}
