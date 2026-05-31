import { api } from '@/lib/apiClient'
import type { UpdateUserProfileRequest, UserProfileDto } from '@/types/api'

export const profileApi = {
  get: () => api.get<UserProfileDto>('/UserProfile').then((r) => r.data),
  update: (body: UpdateUserProfileRequest) =>
    api.put<UserProfileDto>('/UserProfile', body).then((r) => r.data),
  requestAuthor: () =>
    api.post<{ message: string }>('/UserProfile/request-author').then((r) => r.data),
}
