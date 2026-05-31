import { useQuery } from '@tanstack/react-query'
import { Plus } from 'lucide-react'
import { Link } from 'react-router-dom'
import { blogsApi } from '@/api/blogs'
import { useAuth } from '@/auth/AuthContext'
import { Badge } from '@/components/ui/Badge'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { formatDate } from '@/lib/format'

export function MyBlogsPage() {
  const { user } = useAuth()
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['my-blogs', user?.id],
    queryFn: () => blogsApi.list({ authorId: user?.id, pageNumber: 1, pageSize: 50 }),
    enabled: !!user,
  })

  return (
    <div className="mx-auto max-w-4xl px-4 py-8">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-slate-900">My blogs</h1>
        <Link to="/author/blogs/new">
          <Button>
            <Plus className="h-4 w-4" /> New blog
          </Button>
        </Link>
      </div>
      <p className="mt-1 text-sm text-slate-400">Note: only published posts appear in this list.</p>
      <div className="mt-6 space-y-3">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No published blogs yet" description="Create your first post." />
        ) : (
          data.items.map((b) => (
            <Card key={b.id} className="flex items-center justify-between gap-3 p-4">
              <div>
                <p className="font-semibold text-slate-800">{b.title}</p>
                <p className="text-sm text-slate-500">{formatDate(b.createdAt)}</p>
              </div>
              <div className="flex items-center gap-3">
                <Badge tone={b.isPublished ? 'green' : 'gray'}>
                  {b.isPublished ? 'Published' : 'Draft'}
                </Badge>
                <Link to={`/author/blogs/${b.id}`}>
                  <Button size="sm" variant="outline">
                    Edit
                  </Button>
                </Link>
              </div>
            </Card>
          ))
        )}
      </div>
    </div>
  )
}
