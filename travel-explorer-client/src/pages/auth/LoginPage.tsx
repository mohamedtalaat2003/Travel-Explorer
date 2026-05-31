import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { toast } from 'sonner'
import { z } from 'zod'
import { useAuth } from '@/auth/AuthContext'
import { GoogleButton } from '@/components/GoogleButton'
import { Button } from '@/components/ui/Button'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { extractErrorMessage } from '@/lib/apiClient'
import { AuthShell, OrDivider } from './AuthShell'

const schema = z.object({
  userName: z.string().min(1, 'Username is required'),
  password: z.string().min(1, 'Password is required'),
})
type FormValues = z.infer<typeof schema>

export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation() as { state?: { from?: string } }
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  async function onSubmit(values: FormValues) {
    try {
      const user = await login(values)
      toast.success('Welcome back!')
      const from = location.state?.from
      if (from) navigate(from)
      else if (user?.role === 'Admin') navigate('/admin')
      else if (user?.role === 'Author') navigate('/author/blogs')
      else navigate('/')
    } catch (e) {
      toast.error(extractErrorMessage(e, 'Invalid username or password'))
    }
  }

  return (
    <AuthShell title="Welcome back" subtitle="Sign in to continue your journey">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <Field label="Username" error={errors.userName?.message} required>
          <Input {...register('userName')} placeholder="yourusername" autoComplete="username" />
        </Field>
        <Field label="Password" error={errors.password?.message} required>
          <Input
            type="password"
            {...register('password')}
            placeholder="********"
            autoComplete="current-password"
          />
        </Field>
        <Button type="submit" className="w-full" loading={isSubmitting}>
          Sign in
        </Button>
      </form>
      <OrDivider />
      <GoogleButton mode="login" />
      <p className="text-center text-sm text-slate-500">
        No account?{' '}
        <Link to="/register" className="font-medium text-brand-700">
          Create one
        </Link>
      </p>
    </AuthShell>
  )
}
