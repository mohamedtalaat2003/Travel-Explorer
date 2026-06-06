import axios, {
  AxiosError,
  type AxiosRequestConfig,
  type InternalAxiosRequestConfig,
} from 'axios'
import type { TokenResponse } from '@/types/api'

// In dev VITE_API_BASE_URL is empty, so requests go to /api and are proxied by Vite.
export const API_ORIGIN = (import.meta.env.VITE_API_BASE_URL ?? '') as string
const API_BASE = `${API_ORIGIN}/api`

const ACCESS_KEY = 'te_access_token'
const REFRESH_KEY = 'te_refresh_token'

export const tokenStore = {
  get access(): string | null {
    return localStorage.getItem(ACCESS_KEY)
  },
  get refresh(): string | null {
    return localStorage.getItem(REFRESH_KEY)
  },
  set(tokens: TokenResponse) {
    localStorage.setItem(ACCESS_KEY, tokens.accessToken)
    localStorage.setItem(REFRESH_KEY, tokens.refreshToken)
  },
  clear() {
    localStorage.removeItem(ACCESS_KEY)
    localStorage.removeItem(REFRESH_KEY)
  },
}

// AuthContext registers a callback so it can react when the session ends.
let onAuthFailure: (() => void) | null = null
export function setOnAuthFailure(cb: (() => void) | null) {
  onAuthFailure = cb
}

export const api = axios.create({ baseURL: API_BASE })

api.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = tokenStore.access
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// Single-flight refresh so concurrent 401s trigger only one refresh call.
let refreshPromise: Promise<string | null> | null = null

async function refreshAccessToken(): Promise<string | null> {
  const refresh = tokenStore.refresh
  if (!refresh) return null
  try {
    // Bare axios call to avoid interceptor recursion.
    const res = await axios.post<TokenResponse>(`${API_BASE}/Auth/refresh-token`, {
      refreshToken: refresh,
    })
    tokenStore.set(res.data)
    return res.data.accessToken
  } catch {
    return null
  }
}

api.interceptors.response.use(
  (res) => res,
  async (error: AxiosError) => {
    const original = error.config as (AxiosRequestConfig & { _retry?: boolean }) | undefined
    const status = error.response?.status
    const url = original?.url ?? ''
    const isAuthCall =
      url.includes('/Auth/login') ||
      url.includes('/Auth/refresh-token') ||
      url.includes('/Auth/register')

    if (status === 401 && original && !original._retry && !isAuthCall && tokenStore.refresh) {
      original._retry = true
      if (!refreshPromise) {
        refreshPromise = refreshAccessToken().finally(() => {
          refreshPromise = null
        })
      }
      const newToken = await refreshPromise
      if (newToken) {
        original.headers = original.headers ?? {}
        ;(original.headers as Record<string, string>).Authorization = `Bearer ${newToken}`
        return api(original)
      }
      tokenStore.clear()
      onAuthFailure?.()
    }
    return Promise.reject(error)
  },
)

interface ApiProblem {
  status?: number
  title?: string
  Title?: string
  detail?: string
  Detail?: string
  message?: string
  Message?: string
  errors?: Record<string, string[]>
  Errors?: Record<string, string[]>
}

export function extractErrorMessage(error: unknown, fallback = 'Something went wrong'): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as ApiProblem | string | undefined
    if (typeof data === 'string' && data.trim()) return data
    if (data && typeof data === 'object') {
      const errors = data.errors ?? data.Errors
      if (errors) {
        const msgs = Object.values(errors).flat()
        if (msgs.length) return msgs.join(' ')
      }
      const message = data.message ?? data.Message ?? data.title ?? data.Title ?? data.detail ?? data.Detail
      if (message) return message
    }
    if (error.response?.status === 401) return 'You need to sign in to continue.'
    if (error.response?.status === 403) return 'You do not have permission to do that.'
    if (error.message) return error.message
  }
  return fallback
}
