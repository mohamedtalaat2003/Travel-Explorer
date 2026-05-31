import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import { authApi } from '@/api/auth'
import { setOnAuthFailure, tokenStore } from '@/lib/apiClient'
import { decodeUser, type DecodedUser } from '@/lib/jwt'
import type { LoginRequest, RegisterRequest, TokenResponse } from '@/types/api'

interface AuthContextValue {
  user: DecodedUser | null
  isAuthenticated: boolean
  isAdmin: boolean
  isAuthor: boolean
  isTraveler: boolean
  login: (body: LoginRequest) => Promise<DecodedUser | null>
  register: (body: RegisterRequest) => Promise<void>
  setSession: (tokens: TokenResponse) => DecodedUser | null
  logout: () => Promise<void>
  refreshUser: () => void
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<DecodedUser | null>(() => {
    const token = tokenStore.access
    return token ? decodeUser(token) : null
  })

  const applyToken = useCallback((tokens: TokenResponse) => {
    tokenStore.set(tokens)
    const decoded = decodeUser(tokens.accessToken)
    setUser(decoded)
    return decoded
  }, [])

  const login = useCallback(
    async (body: LoginRequest) => {
      const res = await authApi.login(body)
      return applyToken(res.token)
    },
    [applyToken],
  )

  const register = useCallback(async (body: RegisterRequest) => {
    await authApi.register(body)
  }, [])

  const logout = useCallback(async () => {
    const refresh = tokenStore.refresh
    try {
      if (refresh) await authApi.logout(refresh)
    } catch {
      /* ignore network/logout failures */
    }
    tokenStore.clear()
    setUser(null)
  }, [])

  const refreshUser = useCallback(() => {
    const token = tokenStore.access
    setUser(token ? decodeUser(token) : null)
  }, [])

  // When the API client gives up refreshing, drop the user.
  useEffect(() => {
    setOnAuthFailure(() => setUser(null))
    return () => setOnAuthFailure(null)
  }, [])

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: !!user,
      isAdmin: user?.role === 'Admin',
      isAuthor: user?.role === 'Author',
      isTraveler: user?.role === 'Traveler',
      login,
      register,
      setSession: applyToken,
      logout,
      refreshUser,
    }),
    [user, login, register, applyToken, logout, refreshUser],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
