import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { toast } from 'sonner'
import { blogsApi } from '@/api/blogs'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatDate } from '@/lib/format'

export function BlogsAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-blogs', page],
    queryFn: () => blogsApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const del = useMutation({
    mutationFn: (id: number) => blogsApi.remove(id),
    onSuccess: () => {
      toast.success('Blog deleted')
      qc.invalidateQueries({ queryKey: ['admin-blogs'] })
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-900">Blogs</h1>
      <p className="mt-1 text-sm text-slate-400">Published posts are listed here.</p>
      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No blogs" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Title</th>
                <th className="p-3">Created</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((b) => (
                <tr key={b.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">{b.title}</td>
                  <td className="p-3 text-slate-500">{formatDate(b.createdAt)}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Link to={`/blogs/${b.id}`}>
                        <Button size="sm" variant="outline">
                          View
                        </Button>
                      </Link>
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this blog?') && del.mutate(b.id)}
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
    </div>
  )
}
