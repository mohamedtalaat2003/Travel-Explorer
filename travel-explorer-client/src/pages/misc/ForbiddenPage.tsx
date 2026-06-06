import { ShieldX } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Button } from '@/components/ui/Button'

export function ForbiddenPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-4 p-4 text-center">
      <ShieldX className="h-12 w-12 text-amber-500" />
      <h1 className="text-2xl font-bold text-slate-800">Access denied</h1>
      <p className="max-w-sm text-slate-500">
        You do not have permission to view this page with your current role.
      </p>
      <Link to="/">
        <Button>Back to home</Button>
      </Link>
    </div>
  )
}
