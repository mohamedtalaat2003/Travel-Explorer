import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'
import { z } from 'zod'
import { blogsApi } from '@/api/blogs'
import { categoriesApi } from '@/api/categories'
import { ImageUpload } from '@/components/ImageUpload'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Select } from '@/components/ui/Select'
import { LoadingState } from '@/components/ui/States'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'

const schema = z.object({
  title: z.string().min(5, 'Title must be at least 5 characters'),
  content: z.string().min(1, 'Content is required'),
  categoryId: z.string().optional(),
  isPublished: z.boolean(),
})
type FormValues = z.infer<typeof schema>

export function BlogEditorPage() {
  const { id } = useParams()
  const isEdit = !!id
  const blogId = Number(id)
  const navigate = useNavigate()
  const qc = useQueryClient()
  const [imageUrls, setImageUrls] = useState<string[]>([])

  const categories = useQuery({
    queryKey: ['categories', 'all'],
    queryFn: () => categoriesApi.list({ pageNumber: 1, pageSize: 50 }),
  })
  const existing = useQuery({
    queryKey: ['blog', blogId],
    queryFn: () => blogsApi.get(blogId),
    enabled: isEdit && !Number.isNaN(blogId),
  })

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema), defaultValues: { isPublished: true } })

  useEffect(() => {
    if (existing.data) {
      reset({
        title: existing.data.title,
        content: existing.data.content,
        categoryId: existing.data.categoryId ? String(existing.data.categoryId) : '',
        isPublished: existing.data.isPublished,
      })
      setImageUrls(existing.data.imageUrl ? [existing.data.imageUrl] : [])
    }
  }, [existing.data, reset])

  const save = useMutation({
    mutationFn: (v: FormValues) => {
      const body = {
        title: v.title,
        content: v.content,
        imageUrl: imageUrls[0] ?? '',
        isPublished: v.isPublished,
        categoryId: v.categoryId ? Number(v.categoryId) : null,
      }
      return isEdit ? blogsApi.update(blogId, body) : blogsApi.create(body)
    },
    onSuccess: () => {
      toast.success(isEdit ? 'Blog updated' : 'Blog created')
      qc.invalidateQueries({ queryKey: ['my-blogs'] })
      navigate('/author/blogs')
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  if (isEdit && existing.isLoading) return <LoadingState />

  return (
    <div className="mx-auto max-w-2xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">{isEdit ? 'Edit blog' : 'New blog'}</h1>
      <Card className="mt-6 p-6">
        <form onSubmit={handleSubmit((v) => save.mutate(v))} className="space-y-4">
          <Field label="Title" error={errors.title?.message} required>
            <Input {...register('title')} />
          </Field>
          <Field label="Cover image">
            <ImageUpload value={imageUrls} onChange={setImageUrls} multiple={false} />
          </Field>
          <Field label="Category">
            <Select {...register('categoryId')}>
              <option value="">None</option>
              {categories.data?.items.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Content" error={errors.content?.message} required>
            <Textarea rows={10} {...register('content')} />
          </Field>
          <label className="flex items-center gap-2 text-sm text-slate-600">
            <input type="checkbox" {...register('isPublished')} className="h-4 w-4" /> Publish
            immediately
          </label>
          <div className="flex gap-2">
            <Button type="submit" loading={save.isPending}>
              {isEdit ? 'Save changes' : 'Create blog'}
            </Button>
            <Button type="button" variant="outline" onClick={() => navigate('/author/blogs')}>
              Cancel
            </Button>
          </div>
        </form>
      </Card>
    </div>
  )
}
