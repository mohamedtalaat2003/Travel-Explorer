import { Star } from 'lucide-react'
import { cn } from '@/lib/cn'

interface Props {
  value: number
  onChange?: (value: number) => void
  size?: number
  readOnly?: boolean
  showValue?: boolean
}

export function Rating({ value, onChange, size = 16, readOnly = false, showValue = false }: Props) {
  return (
    <div className="inline-flex items-center gap-1">
      <div className="inline-flex items-center gap-0.5">
        {[1, 2, 3, 4, 5].map((i) => (
          <button
            key={i}
            type="button"
            disabled={readOnly}
            onClick={() => onChange?.(i)}
            className={cn(!readOnly && 'cursor-pointer')}
            aria-label={`${i} star`}
          >
            <Star
              style={{ width: size, height: size }}
              className={cn(
                i <= Math.round(value) ? 'fill-amber-400 text-amber-400' : 'text-slate-300',
              )}
            />
          </button>
        ))}
      </div>
      {showValue && <span className="text-sm text-slate-500">{value.toFixed(1)}</span>}
    </div>
  )
}
