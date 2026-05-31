import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'
import { activitiesApi } from '@/api/activities'
import { destinationsApi } from '@/api/destinations'
import { ImageUpload } from '@/components/ImageUpload'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Modal } from '@/components/ui/Modal'
import { Pagination } from '@/components/ui/Pagination'
import { Select } from '@/components/ui/Select'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'
import type { ActivityDto } from '@/types/api'

const schema = z.object({
  name: z.string().min(2, 'Name must be at least 2 characters'),
  description: z.string().optional(),
  icon: z.string().optional(),
  destinationId: z.coerce.number().min(1, 'Select a destination'),
})
type FormValues = z.infer<typeof schema>

export function ActivitiesAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [editing, setEditing] = useState<ActivityDto | 'new' | null>(null)
  const [imageUrls, setImageUrls] = useState<string[]>([])

  const destinations = useQuery({
    queryKey: ['destinations', 'all'],
    queryFn: () => destinationsApi.list({ pageNumber: 1, pageSize: 100 }),
  })
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-activities', page],
    queryFn: () => activitiesApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-activities'] })

  function openNew() {
    setEditing('new')
    setImageUrls([])
    reset({ name: '', description: '', icon: '', destinationId: 0 })
  }
  function openEdit(a: ActivityDto) {
    setEditing(a)
    setImageUrls(a.imageUrls ?? [])
    reset({ name: a.name, description: a.description ?? '', icon: a.icon ?? '', destinationId: a.destinationId })
  }

  const save = useMutation({
    mutationFn: (v: FormValues) => {
      const body = {
        name: v.name,
        description: v.description ?? '',
        icon: v.icon ?? '',
        destinationId: v.destinationId,
        imageUrls,
      }
      return editing && editing !== 'new'
        ? activitiesApi.update(editing.id, body)
        : activitiesApi.create(body)
    },
    onSuccess: () => {
      toast.success('Activity saved')
      setEditing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => activitiesApi.remove(id),
    onSuccess: () => {
      toast.success('Activity deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-slate-900">Activities</h1>
        <Button onClick={openNew}>
          <Plus className="h-4 w-4" /> New
        </Button>
      </div>
      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No activities" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Name</th>
                <th className="p-3">Destination</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((a) => (
                <tr key={a.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">{a.name}</td>
                  <td className="p-3 text-slate-500">{a.destinationName}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button size="sm" variant="outline" onClick={() => openEdit(a)}>
                        Edit
                      </Button>
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this activity?') && del.mutate(a.id)}
                      >
                        Delete
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      {data && (
        <Pagination
          page={data.pageNumber}
          pageSize={data.pageSize}
          totalCount={data.totalCount}
          onPageChange={setPage}
        />
      )}

      <Modal
        open={!!editing}
        onClose={() => setEditing(null)}
        title={editing && editing !== 'new' ? 'Edit activity' : 'New activity'}
        footer={
          <>
            <Button variant="outline" onClick={() => setEditing(null)}>
              Cancel
            </Button>
            <Button loading={save.isPending} onClick={handleSubmit((v) => save.mutate(v))}>
              Save
            </Button>
          </>
        }
      >
        <form className="space-y-3" onSubmit={handleSubmit((v) => save.mutate(v))}>
          <Field label="Name" error={errors.name?.message} required>
            <Input {...register('name')} />
          </Field>
          <Field label="Destination" error={errors.destinationId?.message} required>
            <Select {...register('destinationId')}>
              <option value={0}>Select...</option>
              {destinations.data?.items.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.name}
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Icon (name or URL)">
            <Input {...register('icon')} />
          </Field>
          <Field label="Description">
            <Textarea {...register('description')} />
          </Field>
          <Field label="Images">
            <ImageUpload value={imageUrls} onChange={setImageUrls} />
          </Field>
        </form>
      </Modal>
    </div>
  )
}
