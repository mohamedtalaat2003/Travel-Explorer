import { useQuery } from '@tanstack/react-query'
import { Plane } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { flightsApi } from '@/api/flights'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Input } from '@/components/ui/Input'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { formatCurrency, formatDateTime } from '@/lib/format'
import type { FlightQuery } from '@/types/api'

export function FlightsPage() {
  const [query, setQuery] = useState<FlightQuery>({ pageNumber: 1, pageSize: 10 })
  const [from, setFrom] = useState('')
  const [to, setTo] = useState('')
  const [date, setDate] = useState('')

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['flights', query],
    queryFn: () => flightsApi.list(query),
  })

  function applyFilters() {
    setQuery({
      pageNumber: 1,
      pageSize: 10,
      departureCity: from || undefined,
      arrivalCity: to || undefined,
      departureDate: date || undefined,
    })
  }

  return (
    <div className="mx-auto max-w-5xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">Flights</h1>
      <div className="mt-4 grid grid-cols-1 gap-3 rounded-xl border border-slate-200 bg-white p-4 sm:grid-cols-4">
        <Input placeholder="From" value={from} onChange={(e) => setFrom(e.target.value)} />
        <Input placeholder="To" value={to} onChange={(e) => setTo(e.target.value)} />
        <Input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
        <Button onClick={applyFilters}>Search flights</Button>
      </div>

      <div className="mt-6 space-y-3">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No flights found" />
        ) : (
          <>
            {data.items.map((f) => (
              <Card key={f.id} className="flex flex-col gap-3 p-4 sm:flex-row sm:items-center sm:justify-between">
                <div className="flex items-center gap-3">
                  <div className="rounded-lg bg-brand-50 p-2 text-brand-700">
                    <Plane className="h-5 w-5" />
                  </div>
                  <div>
                    <p className="font-semibold text-slate-800">
                      {f.departureCity} - {f.arrivalCity}
                    </p>
                    <p className="text-sm text-slate-500">
                      {f.airline} {f.flightNumber} - {formatDateTime(f.departureTime)}
                    </p>
                  </div>
                </div>
                <div className="flex items-center justify-between gap-4 sm:justify-end">
                  <p className="text-sm">
                    from{' '}
                    <span className="font-semibold text-brand-700">
                      {formatCurrency(f.economyPrice)}
                    </span>
                  </p>
                  <Link to={`/flights/${f.id}`}>
                    <Button size="sm">View</Button>
                  </Link>
                </div>
              </Card>
            ))}
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
