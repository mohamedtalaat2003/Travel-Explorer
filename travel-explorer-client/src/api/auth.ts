import { api } from '@/lib/apiClient'
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  TokenResponse,
} from '@/types/api'

export const authApi = {
  login: (body: LoginRequest) => api.post<LoginResponse>('/Auth/login', body).then((r) => r.data),
  register: (body: RegisterRequest) =>
    api.post<RegisterResponse>('/Auth/register', body).then((r) => r.data),
  refresh: (refreshToken: string) =>
    api.post<TokenResponse>('/Auth/refresh-token', { refreshToken }).then((r) => r.data),
  logout: (refreshToken: string) =>
    api.post('/Auth/logout', { refreshToken }).then((r) => r.data),
}
