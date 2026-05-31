import { useEffect, useRef, useState, type ReactNode } from 'react'
import { cn } from '@/lib/cn'

interface RevealProps {
  children: ReactNode
  className?: string
  /** Delay before the reveal animation starts, in milliseconds. */
  delay?: number
  /** How far it travels in on the y axis before settling. */
  y?: number
}

// Reveals its children with a soft fade + rise once scrolled into view.
// Respects prefers-reduced-motion by showing content immediately.
export function Reveal({ children, className, delay = 0, y = 24 }: RevealProps) {
  const ref = useRef<HTMLDivElement>(null)
  const [shown, setShown] = useState(false)

  useEffect(() => {
    const el = ref.current
    if (!el) return

    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
      setShown(true)
      return
    }

    const observer = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (entry.isIntersecting) {
            setShown(true)
            observer.disconnect()
          }
        }
      },
      { threshold: 0.12, rootMargin: '0px 0px -40px 0px' },
    )

    observer.observe(el)
    return () => observer.disconnect()
  }, [])

  return (
    <div
      ref={ref}
      className={cn(
        'transition-all duration-[800ms] ease-[cubic-bezier(0.22,1,0.36,1)] will-change-transform',
        shown ? 'opacity-100 translate-y-0' : 'opacity-0',
        className,
      )}
      style={{ transitionDelay: `${delay}ms`, transform: shown ? undefined : `translateY(${y}px)` }}
    >
      {children}
    </div>
  )
}
