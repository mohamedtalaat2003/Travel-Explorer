import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { flightBookingsApi } from '@/api/flightBookings'
import { StatusBadge } from '@/components/ui/Badge'
import { Card } from '@/components/ui/Card'
import { Pagination } from '@/components/ui/Pagination'
import { Select } from '@/components/ui/Select'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency, formatDateTime } from '@/lib/format'
import { BOOKING_STATUSES, type BookingStatus } from '@/types/api'

export function FlightBookingsAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [status, setStatus] = useState<BookingStatus | ''>('')
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-flight-bookings', page, status],
    queryFn: () =>
      flightBookingsApi.all({ pageNumber: page, pageSize: 10, status: status || undefined }),
  })
  const update = useMutation({
    mutationFn: ({ id, s }: { id: number; s: BookingStatus }) => flightBookingsApi.updateStatus(id, s),
    onSuccess: () => {
      toast.success('Status updated')
      qc.invalidateQueries({ queryKey: ['admin-flight-bookings'] })
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex flex-wrap items-center justify-between gap-3">
        <h1 className="text-2xl font-bold text-slate-900">Flight bookings</h1>
        <Select
          value={status}
          onChange={(e) => {
            setStatus(e.target.value as BookingStatus | '')
            setPage(1)
          }}
          className="w-44"
        >
          <option value="">All statuses</option>
          {BOOKING_STATUSES.map((s) => (
            <option key={s} value={s}>
              {s}
            </option>
          ))}
        </Select>
      </div>
      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No flight bookings" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Flight</th>
                <th className="p-3">Passenger</th>
                <th className="p-3">Class</th>
                <th className="p-3">Total</th>
                <th className="p-3">Status</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((b) => (
                <tr key={b.id} className="border-b border-slate-100">
                  <td className="p-3">
                    <p className="font-medium text-slate-800">
                      {b.airline} {b.flightNumber}
                    </p>
                    <p className="text-slate-400">{formatDateTime(b.departureTime)}</p>
                  </td>
                  <td className="p-3 text-slate-500">{b.userFullName}</td>
                  <td className="p-3 text-slate-500">
                    {b.class} ({b.numberOfPassengers})
                  </td>
                  <td className="p-3">{formatCurrency(b.totalPrice)}</td>
                  <td className="p-3">
                    <div className="flex items-center gap-2">
                      <StatusBadge status={b.status} />
                      <Select
                        value={b.status}
                        onChange={(e) => update.mutate({ id: b.id, s: e.target.value as BookingStatus })}
                        className="h-8 w-32 text-xs"
                      >
                        {BOOKING_STATUSES.map((s) => (
                          <option key={s} value={s}>
                            {s}
                          </option>
                        ))}
                      </Select>
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
