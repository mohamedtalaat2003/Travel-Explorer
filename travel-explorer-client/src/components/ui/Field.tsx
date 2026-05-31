import type { ReactNode } from 'react'

interface Props {
  label?: string
  error?: string
  hint?: string
  required?: boolean
  children: ReactNode
}

export function Field({ label, error, hint, required, children }: Props) {
  return (
    <div className="space-y-1">
      {label && (
        <label className="block text-sm font-medium text-slate-700">
          {label}
          {required && <span className="text-red-500"> *</span>}
        </label>
      )}
      {children}
      {hint && !error && <p className="text-xs text-slate-400">{hint}</p>}
      {error && <p className="text-xs text-red-600">{error}</p>}
    </div>
  )
}
