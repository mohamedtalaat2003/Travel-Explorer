import { Compass, LogOut, Menu, User as UserIcon, X } from 'lucide-react'
import { useState } from 'react'
import { Link, NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '@/auth/AuthContext'
import { Button } from '@/components/ui/Button'
import { cn } from '@/lib/cn'

const navItems = [
  { to: '/destinations', label: 'Destinations' },
  { to: '/flights', label: 'Flights' },
  { to: '/blogs', label: 'Blog' },
  { to: '/contact', label: 'Contact' },
]

export function Navbar() {
  const { isAuthenticated, user, logout, isAdmin, isAuthor, isTraveler } = useAuth()
  const navigate = useNavigate()
  const [open, setOpen] = useState(false)

  async function handleLogout() {
    await logout()
    setOpen(false)
    navigate('/')
  }

  return (
    <header className="sticky top-0 z-40 border-b border-ink-100 bg-cream-50/85 backdrop-blur">
      <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4">
        <Link to="/" className="flex items-center gap-2 font-serif text-xl font-medium text-ink-900">
          <Compass className="h-5 w-5 text-brand-600" /> Travel Explorer
        </Link>

        <nav className="hidden items-center gap-1 md:flex">
          {navItems.map((i) => (
            <NavLink
              key={i.to}
              to={i.to}
              className={({ isActive }) =>
                cn(
                  'rounded-lg px-3 py-2 text-sm font-medium text-ink-400 transition hover:bg-cream-200 hover:text-ink-900',
                  isActive && 'text-brand-700',
                )
              }
            >
              {i.label}
            </NavLink>
          ))}
        </nav>

        <div className="hidden items-center gap-2 md:flex">
          {isAuthenticated ? (
            <>
              {isAdmin && (
                <Link to="/admin">
                  <Button variant="outline" size="sm">
                    Admin
                  </Button>
                </Link>
              )}
              {isAuthor && (
                <Link to="/author/blogs">
                  <Button variant="outline" size="sm">
                    My Blogs
                  </Button>
                </Link>
              )}
              {isTraveler && (
                <Link to="/my/bookings">
                  <Button variant="outline" size="sm">
                    My Trips
                  </Button>
                </Link>
              )}
              <Link
                to="/profile"
                className="inline-flex items-center gap-1 rounded-lg px-2 py-1 text-sm text-ink-500 transition hover:bg-cream-200 hover:text-ink-900"
              >
                <UserIcon className="h-4 w-4" />
                {user?.userName || 'Account'}
              </Link>
              <Button variant="ghost" size="sm" onClick={handleLogout} aria-label="Log out">
                <LogOut className="h-4 w-4" />
              </Button>
            </>
          ) : (
            <>
              <Link to="/login">
                <Button variant="ghost" size="sm">
                  Sign in
                </Button>
              </Link>
              <Link to="/register">
                <Button size="sm">Sign up</Button>
              </Link>
            </>
          )}
        </div>

        <button className="text-ink-700 md:hidden" onClick={() => setOpen(!open)} aria-label="Menu">
          {open ? <X /> : <Menu />}
        </button>
      </div>

      {open && (
        <div className="border-t border-ink-100 bg-cream-50 px-4 py-3 md:hidden">
          <div className="flex flex-col gap-1">
            {navItems.map((i) => (
              <NavLink
                key={i.to}
                to={i.to}
                onClick={() => setOpen(false)}
                className="rounded-lg px-3 py-2 text-sm font-medium text-ink-500 transition hover:bg-cream-200 hover:text-ink-900"
              >
                {i.label}
              </NavLink>
            ))}
            <div className="my-2 h-px bg-ink-100" />
            {isAuthenticated ? (
              <>
                {isAdmin && (
                  <Link to="/admin" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                    Admin
                  </Link>
                )}
                {isAuthor && (
                  <Link to="/author/blogs" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                    My Blogs
                  </Link>
                )}
                {isTraveler && (
                  <Link to="/my/bookings" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                    My Trips
                  </Link>
                )}
                <Link to="/profile" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                  Profile
                </Link>
                <button onClick={handleLogout} className="px-3 py-2 text-left text-sm text-red-600">
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link to="/login" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                  Sign in
                </Link>
                <Link to="/register" onClick={() => setOpen(false)} className="px-3 py-2 text-sm">
                  Sign up
                </Link>
              </>
            )}
          </div>
        </div>
      )}
    </header>
  )
}
