import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { destinationBookingsApi } from '@/api/destinationBookings'
import { StatusBadge } from '@/components/ui/Badge'
import { Card } from '@/components/ui/Card'
import { Select } from '@/components/ui/Select'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency, formatDate } from '@/lib/format'
import { BOOKING_STATUSES, type BookingStatus } from '@/types/api'

export function BookingsAdminPage() {
  const qc = useQueryClient()
  const [status, setStatus] = useState<BookingStatus | ''>('')
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-bookings', status],
    queryFn: () => destinationBookingsApi.all(status || undefined),
  })
  const update = useMutation({
    mutationFn: ({ id, s }: { id: number; s: BookingStatus }) =>
      destinationBookingsApi.updateStatus(id, s),
    onSuccess: () => {
      toast.success('Status updated')
      qc.invalidateQueries({ queryKey: ['admin-bookings'] })
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex flex-wrap items-center justify-between gap-3">
        <h1 className="text-2xl font-bold text-slate-900">Stay bookings</h1>
        <Select
          value={status}
          onChange={(e) => setStatus(e.target.value as BookingStatus | '')}
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
        ) : !data || data.length === 0 ? (
          <EmptyState title="No bookings" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Destination</th>
                <th className="p-3">Guest</th>
                <th className="p-3">Dates</th>
                <th className="p-3">Total</th>
                <th className="p-3">Status</th>
              </tr>
            </thead>
            <tbody>
              {data.map((b) => (
                <tr key={b.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">{b.destinationName}</td>
                  <td className="p-3 text-slate-500">{b.userFullName}</td>
                  <td className="p-3 text-slate-500">
                    {formatDate(b.checkInDate)} - {formatDate(b.checkOutDate)}
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
    </div>
  )
}
