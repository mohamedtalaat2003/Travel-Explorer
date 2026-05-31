import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'
import { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'
import { profileApi } from '@/api/profile'
import { useAuth } from '@/auth/AuthContext'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { LoadingState } from '@/components/ui/States'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'
import { toDateInput } from '@/lib/format'

const schema = z.object({
  fullName: z.string().min(1, 'Full name is required'),
  email: z.string().email('Enter a valid email'),
  phoneNumber: z.string().min(1, 'Phone number is required'),
  passportNumber: z.string().min(1, 'Passport number is required'),
  bio: z.string().optional(),
  avatarUrl: z.string().optional(),
  country: z.string().optional(),
  dateOfBirth: z.string().optional(),
})
type FormValues = z.infer<typeof schema>

export function ProfilePage() {
  const qc = useQueryClient()
  const { isTraveler } = useAuth()

  const { data, isLoading } = useQuery({
    queryKey: ['profile'],
    queryFn: async () => {
      try {
        return await profileApi.get()
      } catch (e) {
        if (axios.isAxiosError(e) && e.response?.status === 404) return null
        throw e
      }
    },
  })

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  useEffect(() => {
    if (data) {
      reset({
        fullName: data.fullName ?? '',
        email: data.email ?? '',
        phoneNumber: data.phoneNumber ?? '',
        passportNumber: data.passportNumber ?? '',
        bio: data.bio ?? '',
        avatarUrl: data.avatarUrl ?? '',
        country: data.country ?? '',
        dateOfBirth: toDateInput(data.dateOfBirth),
      })
    }
  }, [data, reset])

  const save = useMutation({
    mutationFn: (v: FormValues) =>
      profileApi.update({
        fullName: v.fullName,
        email: v.email,
        phoneNumber: v.phoneNumber,
        passportNumber: v.passportNumber,
        bio: v.bio || null,
        avatarUrl: v.avatarUrl || null,
        country: v.country || null,
        dateOfBirth: v.dateOfBirth || null,
      }),
    onSuccess: () => {
      toast.success('Profile saved')
      qc.invalidateQueries({ queryKey: ['profile'] })
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const requestAuthor = useMutation({
    mutationFn: () => profileApi.requestAuthor(),
    onSuccess: () => toast.success('Author request submitted. An admin will review it.'),
    onError: (e) => toast.error(extractErrorMessage(e, 'Could not submit request')),
  })

  if (isLoading) return <LoadingState />

  return (
    <div className="mx-auto max-w-2xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">My profile</h1>
      {!data && (
        <p className="mt-1 text-sm text-slate-500">
          Complete your profile to speed up bookings.
        </p>
      )}
      <Card className="mt-6 p-6">
        <form onSubmit={handleSubmit((v) => save.mutate(v))} className="space-y-4">
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
            <Field label="Full name" error={errors.fullName?.message} required>
              <Input {...register('fullName')} />
            </Field>
            <Field label="Email" error={errors.email?.message} required>
              <Input type="email" {...register('email')} />
            </Field>
            <Field label="Phone number" error={errors.phoneNumber?.message} required>
              <Input {...register('phoneNumber')} />
            </Field>
            <Field label="Passport number" error={errors.passportNumber?.message} required>
              <Input {...register('passportNumber')} />
            </Field>
            <Field label="Country" error={errors.country?.message}>
              <Input {...register('country')} />
            </Field>
            <Field label="Date of birth" error={errors.dateOfBirth?.message}>
              <Input type="date" {...register('dateOfBirth')} />
            </Field>
          </div>
          <Field label="Avatar URL" error={errors.avatarUrl?.message}>
            <Input {...register('avatarUrl')} placeholder="https://..." />
          </Field>
          <Field label="Bio" error={errors.bio?.message}>
            <Textarea {...register('bio')} />
          </Field>
          <Button type="submit" loading={isSubmitting || save.isPending}>
            Save profile
          </Button>
        </form>
      </Card>

      {isTraveler && (
        <Card className="mt-6 p-6">
          <h2 className="font-semibold text-slate-800">Become an Author</h2>
          <p className="mt-1 text-sm text-slate-500">
            Request the Author role to publish blog posts. An admin will review your request.
          </p>
          <Button
            className="mt-3"
            variant="outline"
            loading={requestAuthor.isPending}
            onClick={() => requestAuthor.mutate()}
          >
            Request Author role
          </Button>
        </Card>
      )}
    </div>
  )
}
