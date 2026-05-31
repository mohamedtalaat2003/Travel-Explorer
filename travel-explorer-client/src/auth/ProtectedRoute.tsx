import type { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import type { AppRole } from '@/types/api'
import { useAuth } from './AuthContext'

interface Props {
  children: ReactNode
  allowedRoles?: AppRole[]
}

export function ProtectedRoute({ children, allowedRoles }: Props) {
  const { isAuthenticated, user } = useAuth()
  const location = useLocation()

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location.pathname + location.search }} replace />
  }

  if (allowedRoles && user && !allowedRoles.includes(user.role as AppRole)) {
    return <Navigate to="/forbidden" replace />
  }

  return <>{children}</>
}
