import { useMutation, useQuery } from '@tanstack/react-query'
import { ArrowLeft, MapPin, Star } from 'lucide-react'
import { useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'
import { destinationBookingsApi } from '@/api/destinationBookings'
import { destinationsApi } from '@/api/destinations'
import { useAuth } from '@/auth/AuthContext'
import { ReviewSection } from '@/components/ReviewSection'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency } from '@/lib/format'

export function DestinationDetailPage() {
  const { id } = useParams()
  const destId = Number(id)
  const [activeImg, setActiveImg] = useState(0)

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['destination', destId],
    enabled: !Number.isNaN(destId),
    queryFn: async () => {
      const [destination, activities, reviews] = await Promise.all([
        destinationsApi.get(destId),
        destinationsApi.activities(destId),
        destinationsApi.reviews(destId),
      ])
      return { destination, activities, reviews }
    },
  })

  if (isLoading) return <LoadingState />
  if (isError || !data) return <ErrorState message="Destination not found" onRetry={refetch} />

  const { destination: d, activities, reviews } = data
  const images = d.imageUrls ?? []

  return (
    <div className="mx-auto max-w-6xl px-4 py-8">
      <Link to="/destinations" className="mb-4 inline-flex items-center gap-1 text-sm text-brand-700">
        <ArrowLeft className="h-4 w-4" /> Back
      </Link>
      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        <div className="space-y-6 lg:col-span-2">
          <div>
            <div className="aspect-[16/9] w-full overflow-hidden rounded-xl bg-slate-100">
              {images[activeImg] ? (
                <img src={images[activeImg]} alt={d.name} className="h-full w-full object-cover" />
              ) : (
                <div className="flex h-full items-center justify-center text-slate-300">
                  <MapPin className="h-12 w-12" />
                </div>
              )}
            </div>
            {images.length > 1 && (
              <div className="mt-2 flex gap-2 overflow-x-auto">
                {images.map((img, i) => (
                  <button
                    key={i}
                    onClick={() => setActiveImg(i)}
                    className={`h-16 w-24 shrink-0 overflow-hidden rounded-lg border-2 ${
                      i === activeImg ? 'border-brand-500' : 'border-transparent'
                    }`}
                  >
                    <img src={img} alt="" className="h-full w-full object-cover" />
                  </button>
                ))}
              </div>
            )}
          </div>

          <div>
            <div className="flex items-center justify-between gap-2">
              <h1 className="text-2xl font-bold text-slate-900">{d.name}</h1>
              <span className="flex items-center gap-1 text-amber-500">
                <Star className="h-5 w-5 fill-amber-400 text-amber-400" />
                {d.averageRating.toFixed(1)}
                <span className="text-sm text-slate-400">({d.reviewCount})</span>
              </span>
            </div>
            <p className="mt-1 flex items-center gap-1 text-slate-500">
              <MapPin className="h-4 w-4" />
              {d.location}
            </p>
            {d.categoryName && <p className="mt-1 text-sm text-brand-700">{d.categoryName}</p>}
            <p className="mt-4 whitespace-pre-wrap text-slate-700">{d.description}</p>
          </div>

          {activities.length > 0 && (
            <div>
              <h2 className="text-lg font-semibold">Activities</h2>
              <div className="mt-3 grid grid-cols-1 gap-3 sm:grid-cols-2">
                {activities.map((a) => (
                  <Card key={a.id} className="p-4">
                    <p className="font-medium text-slate-800">{a.name}</p>
                    {a.description && <p className="mt-1 text-sm text-slate-500">{a.description}</p>}
                  </Card>
                ))}
              </div>
            </div>
          )}

          <ReviewSection destinationId={destId} reviews={reviews} />
        </div>

        <div className="lg:col-span-1">
          <BookingWidget destinationId={destId} pricePerNight={d.pricePerNight} />
        </div>
      </div>
    </div>
  )
}

function BookingWidget({
  destinationId,
  pricePerNight,
}: {
  destinationId: number
  pricePerNight: number
}) {
  const { isAuthenticated, isTraveler } = useAuth()
  const navigate = useNavigate()
  const [checkIn, setCheckIn] = useState('')
  const [checkOut, setCheckOut] = useState('')
  const [guests, setGuests] = useState(1)

  const nights =
    checkIn && checkOut
      ? Math.max(0, Math.round((new Date(checkOut).getTime() - new Date(checkIn).getTime()) / 86400000))
      : 0
  const total = nights * pricePerNight

  const create = useMutation({
    mutationFn: () =>
      destinationBookingsApi.create({
        checkInDate: checkIn,
        checkOutDate: checkOut,
        numberOfGuests: guests,
        destinationId,
      }),
    onSuccess: () => {
      toast.success('Booking created! Complete payment from My Trips.')
      navigate('/my/bookings')
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <Card className="sticky top-20 space-y-3 p-5">
      <p className="text-lg font-semibold">
        {formatCurrency(pricePerNight)} <span className="text-sm font-normal text-slate-400">/ night</span>
      </p>
      {!isAuthenticated ? (
        <div className="space-y-2">
          <p className="text-sm text-slate-500">Sign in as a traveler to book this stay.</p>
          <Link to="/login">
            <Button className="w-full">Sign in</Button>
          </Link>
        </div>
      ) : !isTraveler ? (
        <p className="text-sm text-slate-500">Only travelers can make bookings.</p>
      ) : (
        <>
          <Field label="Check-in">
            <Input type="date" value={checkIn} onChange={(e) => setCheckIn(e.target.value)} />
          </Field>
          <Field label="Check-out">
            <Input type="date" value={checkOut} onChange={(e) => setCheckOut(e.target.value)} />
          </Field>
          <Field label="Guests">
            <Input
              type="number"
              min={1}
              max={100}
              value={guests}
              onChange={(e) => setGuests(Number(e.target.value))}
            />
          </Field>
          {nights > 0 && (
            <p className="text-sm text-slate-600">
              {nights} night(s) - <span className="font-semibold">{formatCurrency(total)}</span>
            </p>
          )}
          <Button
            className="w-full"
            disabled={nights <= 0}
            loading={create.isPending}
            onClick={() => create.mutate()}
          >
            Book now
          </Button>
        </>
      )}
    </Card>
  )
}
