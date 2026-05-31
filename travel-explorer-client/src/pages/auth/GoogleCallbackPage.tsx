import { useEffect, useRef } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { toast } from 'sonner'
import { useAuth } from '@/auth/AuthContext'
import { Button } from '@/components/ui/Button'
import { AuthShell } from './AuthShell'

export function GoogleCallbackPage() {
  const { setSession } = useAuth()
  const navigate = useNavigate()
  const [params] = useSearchParams()
  const handled = useRef(false)

  const accessToken = params.get('accessToken')
  const refreshToken = params.get('refreshToken')
  const error = params.get('error')
  const success = params.get('success')

  useEffect(() => {
    if (handled.current) return
    handled.current = true
    if (accessToken && refreshToken) {
      setSession({ accessToken, refreshToken })
      toast.success('Signed in with Google')
      navigate('/', { replace: true })
    }
  }, [accessToken, refreshToken, setSession, navigate])

  if (accessToken && refreshToken) {
    return <AuthShell title="Signing you in...">{null}</AuthShell>
  }

  if (success) {
    return (
      <AuthShell
        title="Account created"
        subtitle="Your Google account is registered. Please sign in to continue."
      >
        <Link to="/login">
          <Button className="w-full">Go to sign in</Button>
        </Link>
      </AuthShell>
    )
  }

  return (
    <AuthShell
      title="Google sign-in failed"
      subtitle={error ? `Error: ${error}` : 'Something went wrong during sign-in.'}
    >
      <Link to="/login">
        <Button variant="outline" className="w-full">
          Back to sign in
        </Button>
      </Link>
    </AuthShell>
  )
}
