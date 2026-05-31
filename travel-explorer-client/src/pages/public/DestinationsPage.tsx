import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { categoriesApi } from '@/api/categories'
import { destinationsApi } from '@/api/destinations'
import { DestinationCard } from '@/components/DestinationCard'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import { Pagination } from '@/components/ui/Pagination'
import { Select } from '@/components/ui/Select'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import type { DestinationQuery } from '@/types/api'

export function DestinationsPage() {
  const [searchParams] = useSearchParams()
  const initialKeyword = searchParams.get('keyword')?.trim() ?? ''
  const initialCategory = searchParams.get('categoryId')?.trim() ?? ''
  const [query, setQuery] = useState<DestinationQuery>({
    pageNumber: 1,
    pageSize: 9,
    keyword: initialKeyword || undefined,
    categoryId: initialCategory ? Number(initialCategory) : undefined,
  })
  const [keyword, setKeyword] = useState(initialKeyword)
  const [location, setLocation] = useState('')
  const [categoryId, setCategoryId] = useState(initialCategory)
  const [maxPrice, setMaxPrice] = useState('')

  const categories = useQuery({
    queryKey: ['categories', 'all'],
    queryFn: () => categoriesApi.list({ pageNumber: 1, pageSize: 50 }),
  })
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['destinations', query],
    queryFn: () => destinationsApi.list(query),
  })

  function applyFilters() {
    setQuery({
      pageNumber: 1,
      pageSize: 9,
      keyword: keyword || undefined,
      location: location || undefined,
      categoryId: categoryId ? Number(categoryId) : undefined,
      maxPrice: maxPrice ? Number(maxPrice) : undefined,
    })
  }

  return (
    <div className="mx-auto max-w-7xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">Destinations</h1>
      <div className="mt-4 grid grid-cols-1 gap-3 rounded-xl border border-slate-200 bg-white p-4 sm:grid-cols-2 lg:grid-cols-5">
        <Input placeholder="Search..." value={keyword} onChange={(e) => setKeyword(e.target.value)} />
        <Input placeholder="Location" value={location} onChange={(e) => setLocation(e.target.value)} />
        <Select value={categoryId} onChange={(e) => setCategoryId(e.target.value)}>
          <option value="">All categories</option>
          {categories.data?.items.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </Select>
        <Input
          type="number"
          placeholder="Max price"
          value={maxPrice}
          onChange={(e) => setMaxPrice(e.target.value)}
        />
        <Button onClick={applyFilters}>Search</Button>
      </div>

      <div className="mt-6">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No destinations found" />
        ) : (
          <>
            <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3">
              {data.items.map((d) => (
                <DestinationCard key={d.id} d={d} />
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
