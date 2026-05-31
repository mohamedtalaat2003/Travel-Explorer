import { api } from '@/lib/apiClient'
import { cleanParams } from '@/lib/params'
import type {
  CategoryDto,
  CategoryQuery,
  CreateCategoryRequest,
  PaginatedResult,
  UpdateCategoryRequest,
} from '@/types/api'

export const categoriesApi = {
  list: (q: CategoryQuery) =>
    api
      .get<PaginatedResult<CategoryDto>>('/Categories', { params: cleanParams(q) })
      .then((r) => r.data),
  get: (id: number) => api.get<CategoryDto>(`/Categories/${id}`).then((r) => r.data),
  create: (body: CreateCategoryRequest) =>
    api.post<CategoryDto>('/Categories', body).then((r) => r.data),
  update: (id: number, body: UpdateCategoryRequest) =>
    api.put<CategoryDto>(`/Categories/${id}`, body).then((r) => r.data),
  remove: (id: number) => api.delete(`/Categories/${id}`).then((r) => r.data),
}
