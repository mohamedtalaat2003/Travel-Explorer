import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'
import { categoriesApi } from '@/api/categories'
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
import { formatCurrency } from '@/lib/format'
import type { DestinationDto } from '@/types/api'

const schema = z.object({
  name: z.string().min(3, 'Name must be at least 3 characters'),
  description: z.string().min(1, 'Description is required'),
  location: z.string().min(1, 'Location is required'),
  pricePerNight: z.coerce.number().min(0, 'Price must be 0 or more'),
  categoryId: z.coerce.number().min(1, 'Select a category'),
})
type FormValues = z.infer<typeof schema>

export function DestinationsAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [editing, setEditing] = useState<DestinationDto | 'new' | null>(null)
  const [imageUrls, setImageUrls] = useState<string[]>([])

  const categories = useQuery({
    queryKey: ['categories', 'all'],
    queryFn: () => categoriesApi.list({ pageNumber: 1, pageSize: 50 }),
  })
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-destinations', page],
    queryFn: () => destinationsApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-destinations'] })

  function openNew() {
    setEditing('new')
    setImageUrls([])
    reset({ name: '', description: '', location: '', pricePerNight: 0, categoryId: 0 })
  }
  function openEdit(d: DestinationDto) {
    setEditing(d)
    setImageUrls(d.imageUrls ?? [])
    reset({
      name: d.name,
      description: d.description,
      location: d.location,
      pricePerNight: d.pricePerNight,
      categoryId: d.categoryId,
    })
  }

  const save = useMutation({
    mutationFn: (v: FormValues) => {
      const body = { ...v, imageUrls }
      return editing && editing !== 'new'
        ? destinationsApi.update(editing.id, body)
        : destinationsApi.create(body)
    },
    onSuccess: () => {
      toast.success('Destination saved')
      setEditing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => destinationsApi.remove(id),
    onSuccess: () => {
      toast.success('Destination deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-slate-900">Destinations</h1>
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
          <EmptyState title="No destinations" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Name</th>
                <th className="p-3">Location</th>
                <th className="p-3">Category</th>
                <th className="p-3">Price</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((d) => (
                <tr key={d.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">{d.name}</td>
                  <td className="p-3 text-slate-500">{d.location}</td>
                  <td className="p-3 text-slate-500">{d.categoryName}</td>
                  <td className="p-3">{formatCurrency(d.pricePerNight)}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button size="sm" variant="outline" onClick={() => openEdit(d)}>
                        Edit
                      </Button>
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this destination?') && del.mutate(d.id)}
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
        title={editing && editing !== 'new' ? 'Edit destination' : 'New destination'}
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
          <div className="grid grid-cols-2 gap-3">
            <Field label="Location" error={errors.location?.message} required>
              <Input {...register('location')} />
            </Field>
            <Field label="Price / night" error={errors.pricePerNight?.message} required>
              <Input type="number" step="0.01" {...register('pricePerNight')} />
            </Field>
          </div>
          <Field label="Category" error={errors.categoryId?.message} required>
            <Select {...register('categoryId')}>
              <option value={0}>Select...</option>
              {categories.data?.items.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Description" error={errors.description?.message} required>
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
