import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { contactApi } from '@/api/contactMessages'
import { Badge } from '@/components/ui/Badge'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Modal } from '@/components/ui/Modal'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatDateTime } from '@/lib/format'
import type { ContactMessageDto } from '@/types/api'

export function MessagesAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [viewing, setViewing] = useState<ContactMessageDto | null>(null)
  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-messages', page],
    queryFn: () => contactApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-messages'] })

  const markRead = useMutation({
    mutationFn: (id: number) => contactApi.markRead(id),
    onSuccess: () => {
      toast.success('Marked as read')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => contactApi.remove(id),
    onSuccess: () => {
      toast.success('Message deleted')
      setViewing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-900">Messages</h1>
      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No messages" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">From</th>
                <th className="p-3">Subject</th>
                <th className="p-3">Received</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((m) => (
                <tr key={m.id} className={`border-b border-slate-100 ${m.isRead ? '' : 'bg-brand-50/40'}`}>
                  <td className="p-3">
                    <p className="font-medium text-slate-800">{m.fullName}</p>
                    <p className="text-slate-400">{m.email}</p>
                  </td>
                  <td className="p-3">
                    <span className="text-slate-700">{m.subject}</span>{' '}
                    {!m.isRead && <Badge tone="blue">New</Badge>}
                  </td>
                  <td className="p-3 text-slate-500">{formatDateTime(m.createdAt)}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button size="sm" variant="outline" onClick={() => setViewing(m)}>
                        View
                      </Button>
                      {!m.isRead && (
                        <Button size="sm" variant="outline" onClick={() => markRead.mutate(m.id)}>
                          Mark read
                        </Button>
                      )}
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this message?') && del.mutate(m.id)}
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

      <Modal open={!!viewing} onClose={() => setViewing(null)} title={viewing?.subject}>
        {viewing && (
          <div className="space-y-2 text-sm">
            <p>
              <span className="text-slate-400">From:</span> {viewing.fullName} ({viewing.email})
            </p>
            <p className="text-slate-400">{formatDateTime(viewing.createdAt)}</p>
            <p className="whitespace-pre-wrap pt-2 text-slate-700">{viewing.message}</p>
          </div>
        )}
      </Modal>
    </div>
  )
}
