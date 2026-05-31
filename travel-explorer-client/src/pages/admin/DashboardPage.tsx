import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  ArrowRight,
  CalendarCheck,
  FileText,
  MapPin,
  Plane,
  Plus,
  UserCheck,
  UserPlus,
  Users as UsersIcon,
} from 'lucide-react'
import type { LucideIcon } from 'lucide-react'
import { Link } from 'react-router-dom'
import { toast } from 'sonner'
import { adminApi } from '@/api/admin'
import { blogsApi } from '@/api/blogs'
import { destinationsApi } from '@/api/destinations'
import { flightsApi } from '@/api/flights'
import { CountUp } from '@/components/CountUp'
import { Reveal } from '@/components/Reveal'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'

const quickActions = [
  { to: '/admin/destinations', label: 'Add destination', icon: MapPin },
  { to: '/admin/flights', label: 'Add flight', icon: Plane },
  { to: '/admin/blogs', label: 'Manage blogs', icon: FileText },
  { to: '/admin/users', label: 'Manage users', icon: UsersIcon },
]

export function DashboardPage() {
  const qc = useQueryClient()
  const stats = useQuery({ queryKey: ['admin-stats'], queryFn: () => adminApi.statistics() })
  const pending = useQuery({
    queryKey: ['admin-author-requests'],
    queryFn: () => adminApi.pendingAuthorRequests(),
  })
  const destinationCount = useQuery({
    queryKey: ['admin-count', 'destinations'],
    queryFn: () => destinationsApi.list({ pageNumber: 1, pageSize: 1 }).then((r) => r.totalCount),
  })
  const flightCount = useQuery({
    queryKey: ['admin-count', 'flights'],
    queryFn: () => flightsApi.list({ pageNumber: 1, pageSize: 1 }).then((r) => r.totalCount),
  })
  const blogCount = useQuery({
    queryKey: ['admin-count', 'blogs'],
    queryFn: () => blogsApi.list({ pageNumber: 1, pageSize: 1 }).then((r) => r.totalCount),
  })

  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-author-requests'] })
  const approve = useMutation({
    mutationFn: (id: number) => adminApi.approveAuthor(id),
    onSuccess: () => {
      toast.success('Author approved')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const reject = useMutation({
    mutationFn: (id: number) => adminApi.rejectAuthor(id),
    onSuccess: () => {
      toast.success('Request rejected')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const cards: { label: string; value: number; icon: LucideIcon; tint: string }[] = [
    { label: 'Total users', value: stats.data?.totalUsers ?? 0, icon: UsersIcon, tint: '#c0573a' },
    { label: 'Active users', value: stats.data?.activeUsers ?? 0, icon: UserCheck, tint: '#788c5d' },
    { label: 'Bookings', value: stats.data?.totalBookings ?? 0, icon: CalendarCheck, tint: '#6a9bcc' },
    { label: 'Destinations', value: destinationCount.data ?? 0, icon: MapPin, tint: '#c46686' },
    { label: 'Flights', value: flightCount.data ?? 0, icon: Plane, tint: '#b07d3f' },
    { label: 'Blogs', value: blogCount.data ?? 0, icon: FileText, tint: '#514f4a' },
  ]

  return (
    <div className="space-y-8">
      <div>
        <p className="font-mono text-xs uppercase tracking-eyebrow text-ink-300">Overview</p>
        <h1 className="mt-1 font-serif text-3xl font-medium text-ink-900">Dashboard</h1>
        <p className="mt-1 text-ink-400">Welcome back — here’s what’s happening across the platform.</p>
      </div>

      {/* Stat cards */}
      <div className="grid grid-cols-2 gap-4 lg:grid-cols-3 xl:grid-cols-6">
        {cards.map((c, i) => (
          <Reveal key={c.label} delay={i * 60}>
            <Card className="group h-full border-ink-100 p-5 transition duration-300 hover:-translate-y-1 hover:shadow-lift">
              <span
                className="flex h-11 w-11 items-center justify-center rounded-xl transition group-hover:scale-110"
                style={{ backgroundColor: `${c.tint}1a`, color: c.tint }}
              >
                <c.icon className="h-5 w-5" />
              </span>
              <p className="mt-4 font-serif text-3xl font-medium text-ink-900">
                <CountUp key={c.value} to={c.value} />
              </p>
              <p className="mt-0.5 text-sm text-ink-400">{c.label}</p>
            </Card>
          </Reveal>
        ))}
      </div>

      {/* Quick actions */}
      <div className="grid grid-cols-2 gap-3 sm:grid-cols-4">
        {quickActions.map((a) => (
          <Link
            key={a.to}
            to={a.to}
            className="group flex items-center gap-3 rounded-xl border border-ink-100 bg-white p-4 transition hover:-translate-y-0.5 hover:border-brand-300 hover:shadow-soft"
          >
            <span className="flex h-9 w-9 items-center justify-center rounded-lg bg-brand-50 text-brand-600">
              <a.icon className="h-4 w-4" />
            </span>
            <span className="text-sm font-medium text-ink-700">{a.label}</span>
            <Plus className="ml-auto h-4 w-4 text-ink-300 transition group-hover:text-brand-600" />
          </Link>
        ))}
      </div>

      {/* Pending author requests */}
      <Card className="border-ink-100 p-6">
        <div className="flex items-center gap-2">
          <UserPlus className="h-5 w-5 text-brand-600" />
          <h2 className="font-serif text-xl text-ink-900">Pending author requests</h2>
          {pending.data && pending.data.length > 0 && (
            <span className="rounded-full bg-brand-50 px-2 py-0.5 text-xs font-semibold text-brand-700">
              {pending.data.length}
            </span>
          )}
        </div>

        <div className="mt-4 space-y-2">
          {pending.isLoading ? (
            <LoadingState />
          ) : !pending.data || pending.data.length === 0 ? (
            <div className="rounded-xl border border-dashed border-ink-100 bg-cream-50 px-4 py-10 text-center">
              <p className="text-sm text-ink-400">No pending requests right now.</p>
            </div>
          ) : (
            pending.data.map((u) => (
              <div
                key={u.id}
                className="flex items-center justify-between gap-3 rounded-xl border border-ink-100 bg-white p-3 transition hover:border-brand-200"
              >
                <div className="flex min-w-0 items-center gap-3">
                  <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-brand-300 to-brand-600 font-semibold text-cream-50">
                    {u.userName.charAt(0).toUpperCase()}
                  </span>
                  <div className="min-w-0">
                    <p className="truncate font-medium text-ink-800">{u.userName}</p>
                    <p className="truncate text-sm text-ink-400">{u.email}</p>
                  </div>
                </div>
                <div className="flex shrink-0 gap-2">
                  <Button size="sm" loading={approve.isPending} onClick={() => approve.mutate(u.id)}>
                    Approve
                  </Button>
                  <Button
                    size="sm"
                    variant="outline"
                    loading={reject.isPending}
                    onClick={() => reject.mutate(u.id)}
                  >
                    Reject
                  </Button>
                </div>
              </div>
            ))
          )}
        </div>
      </Card>

      <Link
        to="/admin/users"
        className="inline-flex items-center gap-1.5 text-sm font-medium text-brand-700 transition hover:gap-2.5"
      >
        View all users <ArrowRight className="h-4 w-4" />
      </Link>
    </div>
  )
}
