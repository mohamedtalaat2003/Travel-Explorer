import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { destinationBookingsApi } from '@/api/destinationBookings'
import { paymentsApi } from '@/api/payments'
import { StatusBadge } from '@/components/ui/Badge'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Modal } from '@/components/ui/Modal'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { Textarea } from '@/components/ui/Textarea'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency, formatDate } from '@/lib/format'
import type { DestinationBookingDto } from '@/types/api'

export function MyBookingsPage() {
  const qc = useQueryClient()
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['my-bookings'],
    queryFn: () => destinationBookingsApi.mine(),
  })
  const [editing, setEditing] = useState<DestinationBookingDto | null>(null)
  const [notes, setNotes] = useState('')

  const invalidate = () => qc.invalidateQueries({ queryKey: ['my-bookings'] })

  const pay = useMutation({
    mutationFn: (id: number) => paymentsApi.checkout(id),
    onSuccess: (res) => {
      if (res.checkoutUrl) window.location.href = res.checkoutUrl
      else toast.error('No checkout URL was returned by the gateway.')
    },
    onError: (e) => toast.error(extractErrorMessage(e, 'Could not start payment')),
  })

  const cancel = useMutation({
    mutationFn: (id: number) => destinationBookingsApi.cancel(id),
    onSuccess: () => {
      toast.success('Booking cancelled')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const saveNotes = useMutation({
    mutationFn: () => destinationBookingsApi.updateNotes(editing!.id, notes),
    onSuccess: () => {
      toast.success('Notes updated')
      setEditing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const isClosed = (s: DestinationBookingDto['status']) =>
    s === 'Cancelled' || s === 'Completed' || s === 'Refunded'

  return (
    <div className="mx-auto max-w-4xl px-4 py-8">
      <h1 className="text-2xl font-bold text-slate-900">My stay bookings</h1>
      <div className="mt-6 space-y-3">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.length === 0 ? (
          <EmptyState title="No bookings yet" description="Browse destinations to plan your first trip." />
        ) : (
          data.map((b) => (
            <Card key={b.id} className="p-4">
              <div className="flex flex-wrap items-start justify-between gap-3">
                <div>
                  <p className="font-semibold text-slate-800">{b.destinationName}</p>
                  <p className="text-sm text-slate-500">
                    {formatDate(b.checkInDate)} - {formatDate(b.checkOutDate)} - {b.numberOfGuests}{' '}
                    guest(s)
                  </p>
                  {b.notes && <p className="mt-1 text-sm text-slate-500">Notes: {b.notes}</p>}
                  <p className="mt-1 font-medium text-brand-700">{formatCurrency(b.totalPrice)}</p>
                </div>
                <div className="flex flex-col items-end gap-2">
                  <StatusBadge status={b.status} />
                  <div className="flex flex-wrap justify-end gap-2">
                    {b.status === 'Pending' && (
                      <Button size="sm" loading={pay.isPending} onClick={() => pay.mutate(b.id)}>
                        Pay now
                      </Button>
                    )}
                    {!isClosed(b.status) && (
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => {
                          setEditing(b)
                          setNotes(b.notes ?? '')
                        }}
                      >
                        Notes
                      </Button>
                    )}
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
                </div>
              </div>
            </Card>
          ))
        )}
      </div>

      <Modal
        open={!!editing}
        onClose={() => setEditing(null)}
        title="Edit booking notes"
        footer={
          <>
            <Button variant="outline" onClick={() => setEditing(null)}>
              Cancel
            </Button>
            <Button loading={saveNotes.isPending} onClick={() => saveNotes.mutate()}>
              Save
            </Button>
          </>
        }
      >
        <Textarea
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          placeholder="Special requests..."
        />
      </Modal>
    </div>
  )
}
