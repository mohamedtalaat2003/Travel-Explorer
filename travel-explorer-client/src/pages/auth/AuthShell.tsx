import { Compass } from 'lucide-react'
import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'

export function AuthShell({
  title,
  subtitle,
  children,
}: {
  title: string
  subtitle?: string
  children: ReactNode
}) {
  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-brand-50 to-slate-100 p-4">
      <div className="w-full max-w-md">
        <Link
          to="/"
          className="mb-6 flex items-center justify-center gap-2 text-xl font-bold text-brand-700"
        >
          <Compass className="h-7 w-7" /> Travel Explorer
        </Link>
        <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-lg sm:p-8">
          <h1 className="text-xl font-semibold text-slate-800">{title}</h1>
          {subtitle && <p className="mt-1 text-sm text-slate-500">{subtitle}</p>}
          <div className="mt-6 space-y-4">{children}</div>
        </div>
      </div>
    </div>
  )
}

export function OrDivider() {
  return (
    <div className="flex items-center gap-3">
      <div className="h-px flex-1 bg-slate-200" />
      <span className="text-xs uppercase text-slate-400">or</span>
      <div className="h-px flex-1 bg-slate-200" />
    </div>
  )
}
