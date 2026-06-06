import {
  Activity,
  CalendarCheck,
  FileText,
  LayoutDashboard,
  Mail,
  MapPin,
  Plane,
  Tags,
  Ticket,
  Users,
} from 'lucide-react'
import { NavLink } from 'react-router-dom'
import { cn } from '@/lib/cn'

const links = [
  { to: '/admin', label: 'Dashboard', icon: LayoutDashboard, end: true },
  { to: '/admin/users', label: 'Users', icon: Users },
  { to: '/admin/destinations', label: 'Destinations', icon: MapPin },
  { to: '/admin/activities', label: 'Activities', icon: Activity },
  { to: '/admin/categories', label: 'Categories', icon: Tags },
  { to: '/admin/flights', label: 'Flights', icon: Plane },
  { to: '/admin/bookings', label: 'Stay Bookings', icon: CalendarCheck },
  { to: '/admin/flight-bookings', label: 'Flight Bookings', icon: Ticket },
  { to: '/admin/blogs', label: 'Blogs', icon: FileText },
  { to: '/admin/messages', label: 'Messages', icon: Mail },
]

export function Sidebar() {
  return (
    <aside className="hidden w-60 shrink-0 border-r border-ink-100 bg-cream-50 lg:block">
      <nav className="flex flex-col gap-1 p-3">
        <p className="px-3 pb-2 pt-1 font-mono text-[0.65rem] uppercase tracking-eyebrow text-ink-300">
          Admin
        </p>
        {links.map(({ to, label, icon: Icon, end }) => (
          <NavLink
            key={to}
            to={to}
            end={end}
            className={({ isActive }) =>
              cn(
                'flex items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium text-ink-500 transition hover:bg-cream-200 hover:text-ink-900',
                isActive && 'bg-brand-50 text-brand-700 hover:bg-brand-50',
              )
            }
          >
            <Icon className="h-4 w-4" /> {label}
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}
