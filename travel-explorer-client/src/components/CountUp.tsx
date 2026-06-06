import { useEffect, useRef, useState } from 'react'

interface CountUpProps {
  to: number
  duration?: number
  decimals?: number
  suffix?: string
  prefix?: string
}

// Counts up from 0 to `to` once it scrolls into view (eased). Respects reduced motion.
export function CountUp({ to, duration = 1600, decimals = 0, suffix = '', prefix = '' }: CountUpProps) {
  const ref = useRef<HTMLSpanElement>(null)
  const started = useRef(false)
  const [value, setValue] = useState(0)

  useEffect(() => {
    const el = ref.current
    if (!el) return

    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
      setValue(to)
      return
    }

    const observer = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (entry.isIntersecting && !started.current) {
            started.current = true
            const start = performance.now()
            const step = (nowT: number) => {
              const progress = Math.min((nowT - start) / duration, 1)
              const eased = 1 - Math.pow(1 - progress, 3)
              setValue(to * eased)
              if (progress < 1) requestAnimationFrame(step)
              else setValue(to)
            }
            requestAnimationFrame(step)
            observer.disconnect()
          }
        }
      },
      { threshold: 0.4 },
    )

    observer.observe(el)
    return () => observer.disconnect()
  }, [to, duration])

  return (
    <span ref={ref}>
      {prefix}
      {value.toFixed(decimals)}
      {suffix}
    </span>
  )
}
