import { useQuery } from '@tanstack/react-query'
import { ArrowLeft } from 'lucide-react'
import { Link, useParams } from 'react-router-dom'
import { blogsApi } from '@/api/blogs'
import { formatDate } from '@/lib/format'
import { ErrorState, LoadingState } from '@/components/ui/States'

export function BlogDetailPage() {
  const { id } = useParams()
  const blogId = Number(id)
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['blog', blogId],
    queryFn: () => blogsApi.get(blogId),
    enabled: !Number.isNaN(blogId),
  })

  if (isLoading) return <LoadingState />
  if (isError || !data) return <ErrorState message="Article not found" onRetry={refetch} />

  return (
    <article className="mx-auto max-w-3xl px-4 py-8">
      <Link to="/blogs" className="mb-4 inline-flex items-center gap-1 text-sm text-brand-700">
        <ArrowLeft className="h-4 w-4" /> Back to blog
      </Link>
      <h1 className="text-3xl font-bold text-slate-900">{data.title}</h1>
      <p className="mt-2 text-sm text-slate-400">{formatDate(data.createdAt)}</p>
      {data.imageUrl && (
        <img
          src={data.imageUrl}
          alt={data.title}
          className="mt-6 aspect-[16/9] w-full rounded-xl object-cover"
        />
      )}
      <div className="prose mt-6 max-w-none whitespace-pre-wrap text-slate-700">{data.content}</div>
    </article>
  )
}
