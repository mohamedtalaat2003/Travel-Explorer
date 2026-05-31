import { Compass } from 'lucide-react'
import { Link } from 'react-router-dom'

const columns = [
  {
    title: 'Explore',
    links: [
      { to: '/destinations', label: 'Destinations' },
      { to: '/flights', label: 'Flights' },
      { to: '/blogs', label: 'Journal' },
    ],
  },
  {
    title: 'Company',
    links: [
      { to: '/contact', label: 'Contact' },
      { to: '/login', label: 'Sign in' },
      { to: '/register', label: 'Create account' },
    ],
  },
]

export function Footer() {
  return (
    <footer className="border-t border-ink-100 bg-cream-100/60">
      <div className="mx-auto max-w-7xl px-4 py-14">
        <div className="grid gap-10 sm:grid-cols-2 lg:grid-cols-4">
          <div className="lg:col-span-2">
            <Link to="/" className="flex items-center gap-2 font-serif text-xl font-medium text-ink-900">
              <Compass className="h-5 w-5 text-brand-600" /> Travel Explorer
            </Link>
            <p className="mt-4 max-w-sm text-ink-400">
              Discover destinations, book stays and flights, and collect stories worth telling — all
              in one calm place.
            </p>
          </div>

          {columns.map((col) => (
            <div key={col.title}>
              <p className="font-mono text-xs uppercase tracking-eyebrow text-ink-300">{col.title}</p>
              <ul className="mt-4 space-y-2.5">
                {col.links.map((l) => (
                  <li key={l.to}>
                    <Link to={l.to} className="text-ink-500 transition hover:text-brand-700">
                      {l.label}
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="mt-12 flex flex-col items-center justify-between gap-3 border-t border-ink-100 pt-6 text-sm text-ink-400 sm:flex-row">
          <p>&copy; {new Date().getFullYear()} Travel Explorer. All rights reserved.</p>
          <p className="font-mono text-xs uppercase tracking-eyebrow text-ink-300">
            Made for people who love to travel
          </p>
        </div>
      </div>
    </footer>
  )
}
