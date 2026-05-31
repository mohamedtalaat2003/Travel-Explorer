import type { ReactNode } from 'react'
import { cn } from '@/lib/cn'
import type { BookingStatus } from '@/types/api'

type Tone = 'gray' | 'green' | 'red' | 'amber' | 'blue' | 'purple'

const tones: Record<Tone, string> = {
  gray: 'bg-slate-100 text-slate-700',
  green: 'bg-green-100 text-green-700',
  red: 'bg-red-100 text-red-700',
  amber: 'bg-amber-100 text-amber-700',
  blue: 'bg-brand-100 text-brand-700',
  purple: 'bg-purple-100 text-purple-700',
}

export function Badge({ children, tone = 'gray' }: { children: ReactNode; tone?: Tone }) {
  return (
    <span
      className={cn(
        'inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium',
        tones[tone],
      )}
    >
      {children}
    </span>
  )
}

const statusTones: Record<BookingStatus, Tone> = {
  Pending: 'amber',
  Confirmed: 'green',
  Cancelled: 'red',
  Completed: 'blue',
  Refunded: 'purple',
}

export function StatusBadge({ status }: { status: BookingStatus }) {
  return <Badge tone={statusTones[status] ?? 'gray'}>{status}</Badge>
}
