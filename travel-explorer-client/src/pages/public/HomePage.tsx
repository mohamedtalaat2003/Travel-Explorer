import { useQuery } from '@tanstack/react-query'
import {
  ArrowRight,
  ArrowUpRight,
  BookOpen,
  Building2,
  Check,
  Compass,
  Globe2,
  Landmark,
  Mountain,
  Palmtree,
  Plane,
  Quote,
  Search,
  ShieldCheck,
  Sparkles,
  Star,
  Sun,
  Waves,
  type LucideIcon,
} from 'lucide-react'
import { useState, type FormEvent, type ReactNode } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { blogsApi } from '@/api/blogs'
import { categoriesApi } from '@/api/categories'
import { destinationsApi } from '@/api/destinations'
import { BlogCard } from '@/components/BlogCard'
import { CountUp } from '@/components/CountUp'
import { DestinationCard } from '@/components/DestinationCard'
import { Reveal } from '@/components/Reveal'
import { Button } from '@/components/ui/Button'
import { cn } from '@/lib/cn'

const POPULAR = ['Tokyo', 'Santorini', 'Bali', 'Paris', 'Reykjavík']
const MARQUEE = [
  'Tokyo',
  'Santorini',
  'Bali',
  'Paris',
  'Reykjavík',
  'Marrakesh',
  'Kyoto',
  'Lisbon',
  'Cape Town',
  'Patagonia',
  'Hanoi',
  'Amalfi',
]

const FEATURES = [
  {
    icon: Plane,
    title: 'Stays & flights',
    desc: 'Book handpicked places to stay and the flights to get there — all in one calm, uncluttered flow.',
  },
  {
    icon: ShieldCheck,
    title: 'Secure checkout',
    desc: 'Pay online with confidence. Manage, edit, and cancel your trips from a single dashboard.',
  },
  {
    icon: BookOpen,
    title: 'Stories & reviews',
    desc: 'Read honest reviews and long-form travel journals from people who actually went.',
  },
]

const STEPS = [
  { title: 'Discover', desc: 'Search destinations and flights, then filter by vibe, price, and rating.' },
  { title: 'Book in minutes', desc: 'Reserve stays and flights through a fast, secure online checkout.' },
  { title: 'Travel & share', desc: 'Manage everything from your trips dashboard, then review and write about it.' },
]

const STATS: { value: number; decimals?: number; suffix?: string; label: string }[] = [
  { value: 12, suffix: '+', label: 'Destinations' },
  { value: 36, suffix: '+', label: 'Curated activities' },
  { value: 4.8, decimals: 1, label: 'Average rating' },
  { value: 10, suffix: 'k+', label: 'Happy travelers' },
]

const CATEGORY_META: Record<string, { icon: LucideIcon; tint: string }> = {
  Beach: { icon: Waves, tint: '#6a9bcc' },
  Island: { icon: Palmtree, tint: '#788c5d' },
  Mountain: { icon: Mountain, tint: '#6c6a64' },
  City: { icon: Building2, tint: '#c46686' },
  Historical: { icon: Landmark, tint: '#b07d3f' },
  Desert: { icon: Sun, tint: '#d97757' },
  Adventure: { icon: Compass, tint: '#5c6e44' },
}

const DEFAULT_CATEGORY_META = { icon: Compass, tint: '#c0573a' }

function Eyebrow({ children, className }: { children: ReactNode; className?: string }) {
  return (
    <p className={cn('font-mono text-xs font-medium uppercase tracking-eyebrow text-brand-700', className)}>
      {children}
    </p>
  )
}

function DestinationSkeleton() {
  return (
    <div className="overflow-hidden rounded-xl border border-ink-100 bg-white">
      <div className="aspect-[4/3] w-full animate-pulse bg-cream-200" />
      <div className="space-y-3 p-4">
        <div className="h-4 w-2/3 animate-pulse rounded bg-cream-200" />
        <div className="h-3 w-1/2 animate-pulse rounded bg-cream-200" />
        <div className="h-3 w-1/3 animate-pulse rounded bg-cream-200" />
      </div>
    </div>
  )
}

function HomeEmpty({ icon: Icon, title, desc, to, cta }: {
  icon: typeof Globe2
  title: string
  desc: string
  to: string
  cta: string
}) {
  return (
    <div className="flex flex-col items-center justify-center rounded-3xl border border-dashed border-ink-100 bg-white/60 px-6 py-16 text-center">
      <span className="flex h-14 w-14 items-center justify-center rounded-2xl bg-brand-50 text-brand-600">
        <Icon className="h-7 w-7" />
      </span>
      <p className="mt-5 font-serif text-2xl text-ink-800">{title}</p>
      <p className="mt-2 max-w-md text-ink-400">{desc}</p>
      <Link to={to} className="mt-6">
        <Button variant="ink" className="rounded-full">
          {cta}
          <ArrowRight className="h-4 w-4" />
        </Button>
      </Link>
    </div>
  )
}

export function HomePage() {
  const navigate = useNavigate()
  const [q, setQ] = useState('')
  const [tilt, setTilt] = useState({ x: 0, y: 0 })

  const topRated = useQuery({
    queryKey: ['destinations', 'top-rated'],
    queryFn: () => destinationsApi.topRated(6),
  })
  const blogs = useQuery({
    queryKey: ['blogs', 'home'],
    queryFn: () => blogsApi.list({ pageNumber: 1, pageSize: 3 }),
  })
  const categories = useQuery({
    queryKey: ['categories', 'home'],
    queryFn: () => categoriesApi.list({ pageNumber: 1, pageSize: 12 }),
  })

  function onSearch(e: FormEvent) {
    e.preventDefault()
    const term = q.trim()
    navigate(term ? `/destinations?keyword=${encodeURIComponent(term)}` : '/destinations')
  }

  function onHeroMove(e: React.MouseEvent<HTMLDivElement>) {
    const rect = e.currentTarget.getBoundingClientRect()
    setTilt({
      x: (e.clientX - rect.left) / rect.width - 0.5,
      y: (e.clientY - rect.top) / rect.height - 0.5,
    })
  }

  // Pick a different real destination for the hero "Featured stay" card on every page load.
  const [featuredSeed] = useState(() => Math.random())
  const featuredPool = topRated.data ?? []
  const featured = featuredPool.length
    ? featuredPool[Math.floor(featuredSeed * featuredPool.length) % featuredPool.length]
    : null
  const featuredImage =
    featured?.imageUrls?.[0] ??
    'https://images.unsplash.com/photo-1761755889797-5e709dc9ce6e?auto=format&fit=crop&w=900&h=1100&q=80'
  const featuredName = featured?.name ?? 'Santorini'
  const featuredLocation = featured?.location ?? 'Cyclades · Greece'
  const featuredHref = featured ? `/destinations/${featured.id}` : '/destinations'

  return (
    <div className="overflow-x-hidden">
      {/* ───────────────────────── Hero ───────────────────────── */}
      <section className="relative overflow-hidden bg-cream-50">
        <div aria-hidden className="pointer-events-none absolute inset-0 bg-dotgrid opacity-70" />
        <div
          aria-hidden
          className="pointer-events-none absolute -left-24 top-6 h-72 w-72 rounded-full bg-brand-300/40 blur-3xl animate-float-slow"
        />
        <div
          aria-hidden
          className="pointer-events-none absolute right-[-4rem] top-44 h-80 w-80 rounded-full bg-[#6a9bcc]/25 blur-3xl animate-float"
        />

        <div
          onMouseMove={onHeroMove}
          onMouseLeave={() => setTilt({ x: 0, y: 0 })}
          className="relative mx-auto grid max-w-7xl items-center gap-14 px-4 py-20 lg:grid-cols-[1.05fr_0.95fr] lg:py-28"
        >
          <div className="animate-fade-up">
            <Eyebrow className="flex items-center gap-2">
              <Sparkles className="h-3.5 w-3.5" /> Explore · Stay · Fly
            </Eyebrow>

            <h1 className="mt-5 font-serif text-5xl font-medium leading-[1.04] tracking-tight text-ink-900 sm:text-6xl lg:text-[4.3rem]">
              Go somewhere worth{' '}
              <span className="italic text-gradient-clay">writing about.</span>
            </h1>

            <p className="mt-6 max-w-xl text-lg leading-relaxed text-ink-400">
              Discover handpicked destinations, book stays and flights in minutes, and read stories
              from travelers who have actually been there.
            </p>

            <form
              onSubmit={onSearch}
              className="mt-8 flex max-w-xl items-center gap-2 rounded-full border border-ink-100 bg-white p-2 shadow-soft transition focus-within:border-brand-300 focus-within:shadow-lift"
            >
              <div className="flex flex-1 items-center gap-2 pl-3">
                <Search className="h-5 w-5 shrink-0 text-ink-300" />
                <input
                  value={q}
                  onChange={(e) => setQ(e.target.value)}
                  placeholder="Where do you want to go?"
                  aria-label="Search destinations"
                  className="h-10 w-full bg-transparent text-ink-700 placeholder:text-ink-300 focus:outline-none"
                />
              </div>
              <button
                type="submit"
                className="inline-flex h-11 shrink-0 items-center gap-2 rounded-full bg-ink-900 px-5 font-medium text-cream-50 transition hover:bg-ink-800 active:scale-95"
              >
                Search
                <ArrowRight className="h-4 w-4" />
              </button>
            </form>

            <div className="mt-5 flex flex-wrap items-center gap-2 text-sm">
              <span className="text-ink-300">Popular</span>
              {POPULAR.map((p) => (
                <Link
                  key={p}
                  to={`/destinations?keyword=${encodeURIComponent(p)}`}
                  className="rounded-full border border-ink-100 bg-white/70 px-3 py-1 text-ink-600 transition hover:border-brand-300 hover:text-brand-700"
                >
                  {p}
                </Link>
              ))}
            </div>

            <div className="mt-10 flex items-center gap-4">
              <div className="flex -space-x-3">
                {[
                  'https://randomuser.me/api/portraits/women/68.jpg',
                  'https://randomuser.me/api/portraits/men/32.jpg',
                  'https://randomuser.me/api/portraits/women/44.jpg',
                  'https://randomuser.me/api/portraits/men/75.jpg',
                ].map((src) => (
                  <img
                    key={src}
                    src={src}
                    alt="Traveler"
                    loading="lazy"
                    className="h-10 w-10 rounded-full border-2 border-cream-50 object-cover"
                  />
                ))}
              </div>
              <div>
                <div className="flex items-center gap-0.5 text-brand-500">
                  {Array.from({ length: 5 }).map((_, i) => (
                    <Star key={i} className="h-4 w-4 fill-brand-500" />
                  ))}
                </div>
                <p className="text-sm text-ink-400">Loved by 12,000+ travelers</p>
              </div>
            </div>
          </div>

          {/* Visual composition */}
          <div
            className="relative mx-auto hidden h-[480px] w-full max-w-md lg:block"
            style={{
              transform: `translate(${tilt.x * 22}px, ${tilt.y * 22}px)`,
              transition: 'transform 0.25s ease-out',
            }}
          >
            <Link
              to={featuredHref}
              className="group absolute inset-x-6 top-0 block animate-float-slow rounded-[2rem] border border-ink-100 bg-white p-3 shadow-lift"
            >
              <div className="relative aspect-[4/5] overflow-hidden rounded-[1.5rem] bg-ink-800">
                <img
                  key={featuredImage}
                  src={featuredImage}
                  alt={featuredName}
                  loading="lazy"
                  className="absolute inset-0 h-full w-full object-cover transition-transform duration-700 ease-out animate-fade-in group-hover:scale-105"
                />
                <div
                  aria-hidden
                  className="absolute inset-0 bg-gradient-to-t from-ink-900/85 via-ink-900/10 to-ink-900/45"
                />
                <div className="absolute inset-0 flex flex-col justify-between p-6 text-cream-50">
                  <span className="font-mono text-[0.7rem] uppercase tracking-eyebrow text-cream-50/90">
                    Featured stay
                  </span>
                  <div>
                    <p className="font-serif text-3xl leading-tight drop-shadow-lg">{featuredName}</p>
                    <p className="mt-1 text-sm text-cream-100/90">{featuredLocation}</p>
                  </div>
                </div>
              </div>
            </Link>

            <div className="absolute -left-2 top-40 flex animate-float items-center gap-3 rounded-2xl border border-ink-100 bg-white/95 p-3 pr-4 shadow-lift backdrop-blur">
              <span className="flex h-10 w-10 items-center justify-center rounded-xl bg-brand-50 text-brand-600">
                <Plane className="h-5 w-5" />
              </span>
              <div className="leading-tight">
                <p className="font-mono text-xs text-ink-300">CAI → JTR</p>
                <p className="text-sm font-semibold text-ink-800">$420 round trip</p>
              </div>
            </div>

            <div
              className="absolute -right-3 bottom-6 flex animate-float-slow items-center gap-3 rounded-2xl border border-ink-100 bg-white/95 p-3 pr-4 shadow-lift backdrop-blur"
              style={{ animationDelay: '1.2s' }}
            >
              <span className="flex h-10 w-10 items-center justify-center rounded-xl bg-[#788c5d]/15 text-[#5c6e44]">
                <Check className="h-5 w-5" />
              </span>
              <div className="leading-tight">
                <p className="text-sm font-semibold text-ink-800">Stay booked</p>
                <p className="text-xs text-ink-400">3 nights · confirmed</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ─────────────────────── Marquee ─────────────────────── */}
      <section className="border-y border-ink-100 bg-cream-100/60 py-5">
        <div className="overflow-hidden [mask-image:linear-gradient(to_right,transparent,black_8%,black_92%,transparent)]">
          <div className="marquee-track flex w-max animate-marquee items-center gap-10">
            {[...MARQUEE, ...MARQUEE].map((place, i) => (
              <span key={`${place}-${i}`} className="flex items-center gap-10">
                <span className="font-serif text-xl text-ink-300">{place}</span>
                <span className="h-1.5 w-1.5 rounded-full bg-brand-400" />
              </span>
            ))}
          </div>
        </div>
      </section>

      {/* ─────────────────────── Stats ─────────────────────── */}
      <section className="bg-cream-50">
        <div className="mx-auto grid max-w-6xl grid-cols-2 gap-8 px-4 py-16 md:grid-cols-4">
          {STATS.map((s, i) => (
            <Reveal key={s.label} delay={i * 80} className="text-center">
              <p className="font-serif text-4xl font-medium tracking-tight text-ink-900 sm:text-5xl">
                <CountUp to={s.value} decimals={s.decimals ?? 0} suffix={s.suffix ?? ''} />
              </p>
              <p className="mt-2 font-mono text-xs uppercase tracking-eyebrow text-ink-300">{s.label}</p>
            </Reveal>
          ))}
        </div>
      </section>

      {/* ─────────────── Top rated destinations ─────────────── */}
      <section className="mx-auto max-w-7xl px-4 py-20 sm:py-24">
        <Reveal className="mb-10 flex flex-col gap-5 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <Eyebrow>Curated stays</Eyebrow>
            <h2 className="mt-3 font-serif text-4xl font-medium tracking-tight text-ink-900 sm:text-5xl">
              Top rated destinations
            </h2>
            <p className="mt-3 max-w-lg text-ink-400">
              The places travelers rate highest right now — chosen for character, not crowds.
            </p>
          </div>
          <Link
            to="/destinations"
            className="group inline-flex items-center gap-1.5 self-start rounded-full border border-ink-100 bg-white px-4 py-2 text-sm font-medium text-ink-700 transition hover:border-brand-300 hover:text-brand-700 sm:self-auto"
          >
            View all
            <ArrowUpRight className="h-4 w-4 transition-transform group-hover:translate-x-0.5 group-hover:-translate-y-0.5" />
          </Link>
        </Reveal>

        {topRated.isLoading ? (
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {Array.from({ length: 3 }).map((_, i) => (
              <DestinationSkeleton key={i} />
            ))}
          </div>
        ) : !topRated.data || topRated.data.length === 0 ? (
          <HomeEmpty
            icon={Globe2}
            title="New destinations are landing soon"
            desc="We're curating standout stays right now. Be the first to explore them."
            to="/destinations"
            cta="Browse all destinations"
          />
        ) : (
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {topRated.data.map((d, i) => (
              <Reveal key={d.id} delay={i * 70}>
                <DestinationCard d={d} />
              </Reveal>
            ))}
          </div>
        )}
      </section>

      {/* ──────────────── Browse by category ──────────────── */}
      {(categories.data?.items?.length ?? 0) > 0 && (
        <section className="border-t border-ink-100 bg-cream-100/40">
          <div className="mx-auto max-w-7xl px-4 py-20 sm:py-24">
            <Reveal className="mb-10 text-center">
              <Eyebrow className="flex justify-center">Find your vibe</Eyebrow>
              <h2 className="mt-3 font-serif text-4xl font-medium tracking-tight text-ink-900 sm:text-5xl">
                Browse by category
              </h2>
            </Reveal>
            <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
              {(categories.data?.items ?? []).map((c, i) => {
                const meta = CATEGORY_META[c.name] ?? DEFAULT_CATEGORY_META
                const Icon = meta.icon
                return (
                  <Reveal key={c.id} delay={i * 60}>
                    <Link
                      to={`/destinations?categoryId=${c.id}`}
                      className="group flex h-full flex-col rounded-2xl border border-ink-100 bg-white p-6 transition duration-300 hover:-translate-y-1 hover:border-brand-300 hover:shadow-lift"
                    >
                      <span
                        className="flex h-12 w-12 items-center justify-center rounded-xl transition group-hover:scale-110"
                        style={{ backgroundColor: `${meta.tint}1a`, color: meta.tint }}
                      >
                        <Icon className="h-6 w-6" />
                      </span>
                      <h3 className="mt-4 font-serif text-xl text-ink-900">{c.name}</h3>
                      {c.description && (
                        <p className="mt-1 line-clamp-2 text-sm text-ink-400">{c.description}</p>
                      )}
                      <span className="mt-3 inline-flex items-center gap-1 text-sm font-medium text-brand-700 opacity-0 transition group-hover:opacity-100">
                        Explore <ArrowRight className="h-4 w-4" />
                      </span>
                    </Link>
                  </Reveal>
                )
              })}
            </div>
          </div>
        </section>
      )}

      {/* ──────────────── Dark feature band ──────────────── */}
      <section className="relative overflow-hidden bg-ink-900 text-cream-50">
        <div aria-hidden className="pointer-events-none absolute -right-24 -top-24 h-80 w-80 rounded-full bg-brand-500/20 blur-3xl" />
        <div className="relative mx-auto max-w-7xl px-4 py-20 sm:py-28">
          <Reveal className="max-w-2xl">
            <Eyebrow className="text-brand-400">Why Travel Explorer</Eyebrow>
            <h2 className="mt-3 font-serif text-4xl font-medium tracking-tight sm:text-5xl">
              Everything the trip needs, nothing it doesn’t.
            </h2>
            <p className="mt-4 text-lg text-ink-200">
              One considered place to find, book, and remember your travels — designed to feel calm
              instead of cluttered.
            </p>
          </Reveal>

          <div className="mt-14 grid gap-6 md:grid-cols-3">
            {FEATURES.map((f, i) => (
              <Reveal key={f.title} delay={i * 90}>
                <div className="group h-full rounded-2xl border border-white/10 bg-white/[0.03] p-7 transition duration-300 hover:-translate-y-1 hover:border-brand-500/40 hover:bg-white/[0.06]">
                  <span className="flex h-12 w-12 items-center justify-center rounded-xl bg-brand-500/15 text-brand-400 transition group-hover:bg-brand-500 group-hover:text-cream-50">
                    <f.icon className="h-6 w-6" />
                  </span>
                  <h3 className="mt-5 font-serif text-2xl text-cream-50">{f.title}</h3>
                  <p className="mt-2 text-ink-200">{f.desc}</p>
                </div>
              </Reveal>
            ))}
          </div>
        </div>
      </section>

      {/* ──────────────────── How it works ──────────────────── */}
      <section className="mx-auto max-w-7xl px-4 py-20 sm:py-24">
        <Reveal className="text-center">
          <Eyebrow className="flex justify-center">How it works</Eyebrow>
          <h2 className="mt-3 font-serif text-4xl font-medium tracking-tight text-ink-900 sm:text-5xl">
            From daydream to departure
          </h2>
        </Reveal>

        <div className="mt-14 grid gap-10 md:grid-cols-3">
          {STEPS.map((s, i) => (
            <Reveal key={s.title} delay={i * 100}>
              <div className="relative">
                <span className="font-serif text-6xl font-medium text-brand-200">
                  0{i + 1}
                </span>
                <h3 className="mt-3 font-serif text-2xl text-ink-900">{s.title}</h3>
                <p className="mt-2 text-ink-400">{s.desc}</p>
              </div>
            </Reveal>
          ))}
        </div>
      </section>

      {/* ───────────────────── From the blog ───────────────────── */}
      <section className="border-y border-ink-100 bg-cream-100/50">
        <div className="mx-auto max-w-7xl px-4 py-20 sm:py-24">
          <Reveal className="mb-10 flex flex-col gap-5 sm:flex-row sm:items-end sm:justify-between">
            <div>
              <Eyebrow>The journal</Eyebrow>
              <h2 className="mt-3 font-serif text-4xl font-medium tracking-tight text-ink-900 sm:text-5xl">
                Stories from the road
              </h2>
              <p className="mt-3 max-w-lg text-ink-400">
                Field notes, guides, and long reads to fuel your next escape.
              </p>
            </div>
            <Link
              to="/blogs"
              className="group inline-flex items-center gap-1.5 self-start rounded-full border border-ink-100 bg-white px-4 py-2 text-sm font-medium text-ink-700 transition hover:border-brand-300 hover:text-brand-700 sm:self-auto"
            >
              Read more
              <ArrowUpRight className="h-4 w-4 transition-transform group-hover:translate-x-0.5 group-hover:-translate-y-0.5" />
            </Link>
          </Reveal>

          {blogs.isLoading ? (
            <div className="grid grid-cols-1 gap-6 sm:grid-cols-3">
              {Array.from({ length: 3 }).map((_, i) => (
                <DestinationSkeleton key={i} />
              ))}
            </div>
          ) : !blogs.data || blogs.data.items.length === 0 ? (
            <HomeEmpty
              icon={BookOpen}
              title="The first stories are being written"
              desc="Travel journals from our authors will appear here soon."
              to="/blogs"
              cta="Visit the journal"
            />
          ) : (
            <div className="grid grid-cols-1 gap-6 sm:grid-cols-3">
              {blogs.data.items.map((b, i) => (
                <Reveal key={b.id} delay={i * 70}>
                  <BlogCard b={b} />
                </Reveal>
              ))}
            </div>
          )}
        </div>
      </section>

      {/* ───────────────────── Testimonial ───────────────────── */}
      <section className="mx-auto max-w-4xl px-4 py-24 text-center">
        <Reveal>
          <Quote className="mx-auto h-10 w-10 text-brand-400" />
          <blockquote className="mt-6 font-serif text-3xl font-medium leading-snug tracking-tight text-ink-800 sm:text-[2.5rem] sm:leading-[1.2]">
            “It’s the first travel app that feels like it was made by people who actually love to
            travel. I booked a week in Kyoto before my coffee went cold.”
          </blockquote>
          <figcaption className="mt-8 flex items-center justify-center gap-3">
            <img
              src="https://randomuser.me/api/portraits/women/65.jpg"
              alt="Lina Farouk"
              loading="lazy"
              className="h-11 w-11 rounded-full object-cover ring-2 ring-cream-200"
            />
            <div className="text-left">
              <p className="font-semibold text-ink-800">Lina Farouk</p>
              <p className="text-sm text-ink-400">Frequent traveler</p>
            </div>
          </figcaption>
        </Reveal>
      </section>

      {/* ───────────────────── Final CTA ───────────────────── */}
      <section className="mx-auto max-w-7xl px-4 pb-24">
        <Reveal>
          <div className="relative overflow-hidden rounded-[2rem] bg-ink-900 px-6 py-16 text-center text-cream-50 shadow-lift sm:px-16 sm:py-20">
            <div aria-hidden className="pointer-events-none absolute -left-20 -top-20 h-72 w-72 rounded-full bg-brand-500/25 blur-3xl" />
            <div aria-hidden className="pointer-events-none absolute -bottom-24 -right-16 h-72 w-72 rounded-full bg-[#6a9bcc]/20 blur-3xl" />
            <div className="relative">
              <Eyebrow className="flex justify-center text-brand-400">Start exploring</Eyebrow>
              <h2 className="mt-4 font-serif text-4xl font-medium tracking-tight sm:text-5xl">
                Your next story starts here
              </h2>
              <p className="mx-auto mt-4 max-w-xl text-lg text-ink-200">
                Create a free account and start planning the trip you’ll be talking about for years.
              </p>
              <div className="mt-9 flex flex-wrap items-center justify-center gap-3">
                <Link to="/register">
                  <Button variant="primary" size="lg" className="rounded-full px-7">
                    Create free account
                    <ArrowRight className="h-4 w-4" />
                  </Button>
                </Link>
                <Link
                  to="/destinations"
                  className="inline-flex h-12 items-center justify-center gap-2 rounded-full border border-cream-50/25 px-6 font-medium text-cream-50 transition hover:bg-cream-50/10"
                >
                  Browse destinations
                </Link>
              </div>
            </div>
          </div>
        </Reveal>
      </section>
    </div>
  )
}
