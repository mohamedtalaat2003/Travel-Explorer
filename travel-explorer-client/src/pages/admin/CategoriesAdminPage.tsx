import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'
import { categoriesApi } from '@/api/categories'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Modal } from '@/components/ui/Modal'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'
import type { CategoryDto } from '@/types/api'

const schema = z.object({
  name: z.string().min(2, 'Name must be at least 2 characters'),
  description: z.string().optional(),
  iconUrl: z.string().optional(),
})
type FormValues = z.infer<typeof schema>

export function CategoriesAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [editing, setEditing] = useState<CategoryDto | 'new' | null>(null)
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-categories', page],
    queryFn: () => categoriesApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-categories'] })

  function openNew() {
    setEditing('new')
    reset({ name: '', description: '', iconUrl: '' })
  }
  function openEdit(c: CategoryDto) {
    setEditing(c)
    reset({ name: c.name, description: c.description ?? '', iconUrl: c.iconUrl ?? '' })
  }

  const save = useMutation({
    mutationFn: (v: FormValues) => {
      const body = { name: v.name, description: v.description || null, iconUrl: v.iconUrl || null }
      return editing && editing !== 'new'
        ? categoriesApi.update(editing.id, body)
        : categoriesApi.create(body)
    },
    onSuccess: () => {
      toast.success('Category saved')
      setEditing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => categoriesApi.remove(id),
    onSuccess: () => {
      toast.success('Category deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-slate-900">Categories</h1>
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
          <EmptyState title="No categories" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Name</th>
                <th className="p-3">Description</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((c) => (
                <tr key={c.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">{c.name}</td>
                  <td className="p-3 text-slate-500">{c.description}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button size="sm" variant="outline" onClick={() => openEdit(c)}>
                        Edit
                      </Button>
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this category?') && del.mutate(c.id)}
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
        title={editing && editing !== 'new' ? 'Edit category' : 'New category'}
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
          <Field label="Description">
            <Textarea {...register('description')} />
          </Field>
          <Field label="Icon URL">
            <Input {...register('iconUrl')} />
          </Field>
        </form>
      </Modal>
    </div>
  )
}
