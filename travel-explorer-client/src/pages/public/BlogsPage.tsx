import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { blogsApi } from '@/api/blogs'
import { BlogCard } from '@/components/BlogCard'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import type { BlogQuery } from '@/types/api'

export function BlogsPage() {
  const [query, setQuery] = useState<BlogQuery>({ pageNumber: 1, pageSize: 9 })
  const [keyword, setKeyword] = useState('')

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['blogs', query],
    queryFn: () => blogsApi.list(query),
  })

  return (
    <div className="mx-auto max-w-7xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">Travel Blog</h1>
      <div className="mt-4 flex gap-2">
        <Input
          placeholder="Search articles..."
          value={keyword}
          onChange={(e) => setKeyword(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter')
              setQuery({ pageNumber: 1, pageSize: 9, keyword: keyword || undefined })
          }}
        />
        <Button onClick={() => setQuery({ pageNumber: 1, pageSize: 9, keyword: keyword || undefined })}>
          Search
        </Button>
      </div>

      <div className="mt-6">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No articles yet" />
        ) : (
          <>
            <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3">
              {data.items.map((b) => (
                <BlogCard key={b.id} b={b} />
              ))}
            </div>
            <Pagination
              page={data.pageNumber}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={(p) => setQuery((q) => ({ ...q, pageNumber: p }))}
            />
          </>
        )}
      </div>
    </div>
  )
}
