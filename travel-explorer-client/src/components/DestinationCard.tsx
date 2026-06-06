import { MapPin, Star } from 'lucide-react'
import { Link } from 'react-router-dom'
import { formatCurrency } from '@/lib/format'
import type { DestinationDto } from '@/types/api'
import { Card } from './ui/Card'

export function DestinationCard({ d }: { d: DestinationDto }) {
  const img = d.imageUrls?.[0]
  return (
    <Link to={`/destinations/${d.id}`}>
      <Card className="group h-full overflow-hidden transition hover:shadow-md">
        <div className="aspect-[4/3] w-full overflow-hidden bg-slate-100">
          {img ? (
            <img
              src={img}
              alt={d.name}
              className="h-full w-full object-cover transition group-hover:scale-105"
            />
          ) : (
            <div className="flex h-full items-center justify-center text-slate-300">
              <MapPin className="h-10 w-10" />
            </div>
          )}
        </div>
        <div className="space-y-1 p-4">
          <div className="flex items-center justify-between gap-2">
            <h3 className="truncate font-semibold text-slate-800">{d.name}</h3>
            <span className="flex shrink-0 items-center gap-1 text-sm text-amber-500">
              <Star className="h-4 w-4 fill-amber-400 text-amber-400" />
              {d.averageRating.toFixed(1)}
            </span>
          </div>
          <p className="flex items-center gap-1 text-sm text-slate-500">
            <MapPin className="h-3.5 w-3.5" />
            {d.location}
          </p>
          <p className="pt-1 text-sm">
            <span className="font-semibold text-brand-700">{formatCurrency(d.pricePerNight)}</span>
            <span className="text-slate-400"> / night</span>
          </p>
        </div>
      </Card>
    </Link>
  )
}
