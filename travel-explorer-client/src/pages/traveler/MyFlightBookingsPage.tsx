import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { flightBookingsApi } from '@/api/flightBookings'
import { StatusBadge } from '@/components/ui/Badge'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency, formatDateTime } from '@/lib/format'
import type { BookingStatus } from '@/types/api'

export function MyFlightBookingsPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['my-flight-bookings', page],
    queryFn: () => flightBookingsApi.mine({ pageNumber: page, pageSize: 10 }),
  })

  const cancel = useMutation({
    mutationFn: (id: number) => flightBookingsApi.cancel(id),
    onSuccess: () => {
      toast.success('Flight booking cancelled')
      qc.invalidateQueries({ queryKey: ['my-flight-bookings'] })
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const isClosed = (s: BookingStatus) => s === 'Cancelled' || s === 'Completed' || s === 'Refunded'

  return (
    <div className="mx-auto max-w-4xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">My flight bookings</h1>
      <div className="mt-6 space-y-3">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No flight bookings yet" />
        ) : (
          <>
            {data.items.map((b) => (
              <Card key={b.id} className="flex flex-wrap items-start justify-between gap-3 p-4">
                <div>
                  <p className="font-semibold text-slate-800">
                    {b.airline} {b.flightNumber}
                  </p>
                  <p className="text-sm text-slate-500">
                    {formatDateTime(b.departureTime)} - {b.class} - {b.numberOfPassengers} pax
                  </p>
                  <p className="mt-1 font-medium text-brand-700">{formatCurrency(b.totalPrice)}</p>
                </div>
                <div className="flex flex-col items-end gap-2">
                  <StatusBadge status={b.status} />
                  {!isClosed(b.status) && (
                    <Button
                      size="sm"
                      variant="danger"
                      loading={cancel.isPending}
                      onClick={() => cancel.mutate(b.id)}
                    >
                      Cancel
                    </Button>
                  )}
                </div>
              </Card>
            ))}
            <Pagination
              page={data.pageNumber}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
            />
          </>
        )}
      </div>
    </div>
  )
}
