import { Compass } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Button } from '@/components/ui/Button'

export function NotFoundPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-4 p-4 text-center">
      <Compass className="h-12 w-12 text-brand-500" />
      <h1 className="text-3xl font-bold text-slate-800">404</h1>
      <p className="max-w-sm text-slate-500">This page wandered off the map.</p>
      <Link to="/">
        <Button>Back to home</Button>
      </Link>
    </div>
  )
}
