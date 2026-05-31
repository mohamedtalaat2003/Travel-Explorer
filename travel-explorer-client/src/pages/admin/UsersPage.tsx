import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { toast } from 'sonner'
import { adminApi } from '@/api/admin'
import { Badge } from '@/components/ui/Badge'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Input } from '@/components/ui/Input'
import { Modal } from '@/components/ui/Modal'
import { Pagination } from '@/components/ui/Pagination'
import { Select } from '@/components/ui/Select'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { APP_ROLES, type UserDto, type UserQuery } from '@/types/api'

export function UsersPage() {
  const qc = useQueryClient()
  const [query, setQuery] = useState<UserQuery>({ pageNumber: 1, pageSize: 10 })
  const [search, setSearch] = useState('')
  const [roleUser, setRoleUser] = useState<UserDto | null>(null)
  const [newRole, setNewRole] = useState('Traveler')

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-users', query],
    queryFn: () => adminApi.users(query),
  })
  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-users'] })

  const block = useMutation({
    mutationFn: (id: number) => adminApi.block(id),
    onSuccess: () => {
      toast.success('User blocked')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const unblock = useMutation({
    mutationFn: (id: number) => adminApi.unblock(id),
    onSuccess: () => {
      toast.success('User unblocked')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => adminApi.remove(id),
    onSuccess: () => {
      toast.success('User deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const changeRole = useMutation({
    mutationFn: () => adminApi.changeRole(roleUser!.id, newRole),
    onSuccess: () => {
      toast.success('Role updated')
      setRoleUser(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  function runSearch() {
    setQuery({ pageNumber: 1, pageSize: 10, searchTerm: search || undefined })
  }

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-900">Users</h1>
      <div className="mt-4 flex gap-2">
        <Input
          placeholder="Search name or email..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && runSearch()}
        />
        <Button onClick={runSearch}>Search</Button>
      </div>

      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No users found" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">User</th>
                <th className="p-3">Role</th>
                <th className="p-3">Status</th>
                <th className="p-3">Access</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((u) => (
                <tr key={u.id} className="border-b border-slate-100">
                  <td className="p-3">
                    <p className="font-medium text-slate-800">{u.userName}</p>
                    <p className="text-slate-400">{u.email}</p>
                  </td>
                  <td className="p-3">{u.role}</td>
                  <td className="p-3">{u.status}</td>
                  <td className="p-3">
                    {u.isBlocked ? <Badge tone="red">Blocked</Badge> : <Badge tone="green">Active</Badge>}
                  </td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => {
                          setRoleUser(u)
                          setNewRole(u.role)
                        }}
                      >
                        Role
                      </Button>
                      {u.isBlocked ? (
                        <Button size="sm" variant="outline" onClick={() => unblock.mutate(u.id)}>
                          Unblock
                        </Button>
                      ) : (
                        <Button size="sm" variant="outline" onClick={() => block.mutate(u.id)}>
                          Block
                        </Button>
                      )}
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => {
                          if (window.confirm('Soft-delete this user?')) del.mutate(u.id)
                        }}
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
          page={data.meta.pageNumber}
          pageSize={data.meta.pageSize}
          totalCount={data.meta.totalCount}
          onPageChange={(p) => setQuery((q) => ({ ...q, pageNumber: p }))}
        />
      )}

      <Modal
        open={!!roleUser}
        onClose={() => setRoleUser(null)}
        title={`Change role: ${roleUser?.userName ?? ''}`}
        footer={
          <>
            <Button variant="outline" onClick={() => setRoleUser(null)}>
              Cancel
            </Button>
            <Button loading={changeRole.isPending} onClick={() => changeRole.mutate()}>
              Save
            </Button>
          </>
        }
      >
        <Select value={newRole} onChange={(e) => setNewRole(e.target.value)}>
          {APP_ROLES.map((r) => (
            <option key={r} value={r}>
              {r}
            </option>
          ))}
        </Select>
      </Modal>
    </div>
  )
}
