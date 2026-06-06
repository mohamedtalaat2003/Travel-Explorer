import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'sonner'
import { z } from 'zod'
import { useAuth } from '@/auth/AuthContext'
import { GoogleButton } from '@/components/GoogleButton'
import { Button } from '@/components/ui/Button'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { extractErrorMessage } from '@/lib/apiClient'
import { AuthShell, OrDivider } from './AuthShell'

const schema = z
  .object({
    fullName: z.string().min(3, 'Full name must be at least 3 characters'),
    userName: z
      .string()
      .min(3, 'Username must be at least 3 characters')
      .max(50, 'Username cannot exceed 50 characters'),
    email: z.string().email('Enter a valid email'),
    password: z
      .string()
      .min(8, 'Password must be at least 8 characters')
      .regex(
        /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/,
        'Include upper, lower, number and special character',
      ),
    confirmPassword: z.string(),
    iWantToBeAuthor: z.boolean(),
  })
  .refine((d) => d.password === d.confirmPassword, {
    path: ['confirmPassword'],
    message: 'Passwords do not match',
  })

type FormValues = z.infer<typeof schema>

export function RegisterPage() {
  const { register: registerUser, login } = useAuth()
  const navigate = useNavigate()
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { iWantToBeAuthor: false },
  })

  async function onSubmit(values: FormValues) {
    try {
      await registerUser(values)
      await login({ userName: values.userName, password: values.password })
      toast.success('Account created. Welcome aboard!')
      navigate('/')
    } catch (e) {
      toast.error(extractErrorMessage(e, 'Registration failed'))
    }
  }

  return (
    <AuthShell title="Create your account" subtitle="Join Travel Explorer and start booking">
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <Field label="Full name" error={errors.fullName?.message} required>
          <Input {...register('fullName')} placeholder="Jane Traveler" />
        </Field>
        <Field label="Username" error={errors.userName?.message} required>
          <Input {...register('userName')} placeholder="janet" autoComplete="username" />
        </Field>
        <Field label="Email" error={errors.email?.message} required>
          <Input type="email" {...register('email')} placeholder="jane@example.com" />
        </Field>
        <Field label="Password" error={errors.password?.message} required>
          <Input type="password" {...register('password')} placeholder="********" />
        </Field>
        <Field label="Confirm password" error={errors.confirmPassword?.message} required>
          <Input type="password" {...register('confirmPassword')} placeholder="********" />
        </Field>
        <label className="flex items-center gap-2 text-sm text-slate-600">
          <input type="checkbox" {...register('iWantToBeAuthor')} className="h-4 w-4 rounded border-slate-300" />
          I want to become an Author (requires admin approval)
        </label>
        <Button type="submit" className="w-full" loading={isSubmitting}>
          Create account
        </Button>
      </form>
      <OrDivider />
      <GoogleButton mode="register" />
      <p className="text-center text-sm text-slate-500">
        Already have an account?{' '}
        <Link to="/login" className="font-medium text-brand-700">
          Sign in
        </Link>
      </p>
    </AuthShell>
  )
}
