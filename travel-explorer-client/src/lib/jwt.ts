import { jwtDecode } from 'jwt-decode'

// The backend issues tokens with the classic XML-schema claim URIs.
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
const NAMEID_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
const NAME_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'

export interface DecodedUser {
  id: number
  userName: string
  role: string
  exp?: number
}

export function decodeUser(token: string): DecodedUser | null {
  try {
    const payload = jwtDecode<Record<string, unknown>>(token)
    const idRaw = payload[NAMEID_CLAIM] ?? payload['nameid'] ?? payload['sub']
    const role = payload[ROLE_CLAIM] ?? payload['role'] ?? ''
    const name = payload[NAME_CLAIM] ?? payload['unique_name'] ?? payload['name'] ?? ''
    const id = Number(idRaw)
    if (!id || Number.isNaN(id)) return null
    return {
      id,
      userName: String(name ?? ''),
      role: String(role ?? ''),
      exp: typeof payload.exp === 'number' ? payload.exp : undefined,
    }
  } catch {
    return null
  }
}

export function isExpired(token: string): boolean {
  try {
    const { exp } = jwtDecode<{ exp?: number }>(token)
    if (!exp) return false
    return exp * 1000 <= Date.now()
  } catch {
    return true
  }
}
