import { useMutation } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { toast } from 'sonner'
import { z } from 'zod'
import { contactApi } from '@/api/contactMessages'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'

const schema = z.object({
  fullName: z.string().min(3, 'Name must be at least 3 characters'),
  email: z.string().email('Enter a valid email'),
  subject: z.string().min(1, 'Subject is required'),
  message: z.string().min(1, 'Message is required'),
})
type FormValues = z.infer<typeof schema>

export function ContactPage() {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  const send = useMutation({
    mutationFn: (body: FormValues) => contactApi.create(body),
    onSuccess: () => {
      toast.success('Message sent! We will get back to you soon.')
      reset()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div className="mx-auto max-w-2xl px-4 py-10">
      <h1 className="text-2xl font-bold text-slate-900">Contact us</h1>
      <p className="mt-1 text-slate-500">Have a question? Send us a message.</p>
      <Card className="mt-6 p-6">
        <form onSubmit={handleSubmit((v) => send.mutate(v))} className="space-y-4">
          <Field label="Full name" error={errors.fullName?.message} required>
            <Input {...register('fullName')} />
          </Field>
          <Field label="Email" error={errors.email?.message} required>
            <Input type="email" {...register('email')} />
          </Field>
          <Field label="Subject" error={errors.subject?.message} required>
            <Input {...register('subject')} />
          </Field>
          <Field label="Message" error={errors.message?.message} required>
            <Textarea rows={5} {...register('message')} />
          </Field>
          <Button type="submit" loading={send.isPending}>
            Send message
          </Button>
        </form>
      </Card>
    </div>
  )
}
