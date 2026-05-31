import { useMutation, useQuery } from '@tanstack/react-query'
import { ArrowLeft, Plane } from 'lucide-react'
import { useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'
import { flightBookingsApi } from '@/api/flightBookings'
import { flightsApi } from '@/api/flights'
import { useAuth } from '@/auth/AuthContext'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Select } from '@/components/ui/Select'
import { ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatCurrency, formatDateTime } from '@/lib/format'
import { FLIGHT_CLASSES, GENDERS, type FlightClass, type Gender } from '@/types/api'

export function FlightDetailPage() {
  const { id } = useParams()
  const flightId = Number(id)
  const { data: f, isLoading, isError, refetch } = useQuery({
    queryKey: ['flight', flightId],
    queryFn: () => flightsApi.get(flightId),
    enabled: !Number.isNaN(flightId),
  })

  if (isLoading) return <LoadingState />
  if (isError || !f) return <ErrorState message="Flight not found" onRetry={refetch} />

  const priceByClass: Record<FlightClass, number> = {
    Economy: f.economyPrice,
    Business: f.businessPrice,
    FirstClass: f.firstClassPrice,
  }
  const seatsByClass: Record<FlightClass, number> = {
    Economy: f.availableEconomySeats,
    Business: f.availableBusinessSeats,
    FirstClass: f.availableFirstClassSeats,
  }

  return (
    <div className="mx-auto max-w-5xl px-4 py-8">
      <Link to="/flights" className="mb-4 inline-flex items-center gap-1 text-sm text-brand-700">
        <ArrowLeft className="h-4 w-4" /> Back to flights
      </Link>
      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <Card className="p-6">
            <div className="flex items-center gap-3">
              <div className="rounded-lg bg-brand-50 p-3 text-brand-700">
                <Plane className="h-6 w-6" />
              </div>
              <div>
                <h1 className="text-xl font-bold text-slate-900">
                  {f.departureCity} - {f.arrivalCity}
                </h1>
                <p className="text-sm text-slate-500">
                  {f.airline} - {f.flightNumber}
                </p>
              </div>
            </div>
            <div className="mt-6 grid grid-cols-2 gap-4 text-sm">
              <div>
                <p className="text-slate-400">Departure</p>
                <p className="font-medium">{formatDateTime(f.departureTime)}</p>
              </div>
              <div>
                <p className="text-slate-400">Arrival</p>
                <p className="font-medium">{formatDateTime(f.arrivalTime)}</p>
              </div>
            </div>
            <div className="mt-6 grid grid-cols-3 gap-3 text-center text-sm">
              {FLIGHT_CLASSES.map((c) => (
                <div key={c} className="rounded-lg border border-slate-200 p-3">
                  <p className="font-medium text-slate-700">{c}</p>
                  <p className="text-brand-700">{formatCurrency(priceByClass[c])}</p>
                  <p className="text-xs text-slate-400">{seatsByClass[c]} seats left</p>
                </div>
              ))}
            </div>
          </Card>
        </div>
        <div className="lg:col-span-1">
          <FlightBookingWidget
            flightScheduleId={flightId}
            priceByClass={priceByClass}
            seatsByClass={seatsByClass}
          />
        </div>
      </div>
    </div>
  )
}

function FlightBookingWidget({
  flightScheduleId,
  priceByClass,
  seatsByClass,
}: {
  flightScheduleId: number
  priceByClass: Record<FlightClass, number>
  seatsByClass: Record<FlightClass, number>
}) {
  const { isAuthenticated, isTraveler } = useAuth()
  const navigate = useNavigate()
  const [flightClass, setFlightClass] = useState<FlightClass>('Economy')
  const [passengers, setPassengers] = useState(1)
  const [gender, setGender] = useState<Gender>('Male')
  const [nationality, setNationality] = useState('')
  const [seatPreference, setSeatPreference] = useState('')

  const total = priceByClass[flightClass] * passengers

  const create = useMutation({
    mutationFn: () =>
      flightBookingsApi.create({
        class: flightClass,
        numberOfPassengers: passengers,
        flightScheduleId,
        gender,
        nationality: nationality || undefined,
        seatPreference: seatPreference || undefined,
      }),
    onSuccess: () => {
      toast.success('Flight booked! View it under My Trips.')
      navigate('/my/flight-bookings')
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <Card className="sticky top-20 space-y-3 p-5">
      <p className="font-semibold text-slate-800">Book this flight</p>
      {!isAuthenticated ? (
        <div className="space-y-2">
          <p className="text-sm text-slate-500">Sign in as a traveler to book.</p>
          <Link to="/login">
            <Button className="w-full">Sign in</Button>
          </Link>
        </div>
      ) : !isTraveler ? (
        <p className="text-sm text-slate-500">Only travelers can book flights.</p>
      ) : (
        <>
          <Field label="Class">
            <Select value={flightClass} onChange={(e) => setFlightClass(e.target.value as FlightClass)}>
              {FLIGHT_CLASSES.map((c) => (
                <option key={c} value={c} disabled={seatsByClass[c] <= 0}>
                  {c} ({seatsByClass[c]} left)
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Passengers">
            <Input
              type="number"
              min={1}
              max={10}
              value={passengers}
              onChange={(e) => setPassengers(Number(e.target.value))}
            />
          </Field>
          <Field label="Gender">
            <Select value={gender} onChange={(e) => setGender(e.target.value as Gender)}>
              {GENDERS.map((g) => (
                <option key={g} value={g}>
                  {g}
                </option>
              ))}
            </Select>
          </Field>
          <Field label="Nationality">
            <Input value={nationality} onChange={(e) => setNationality(e.target.value)} />
          </Field>
          <Field label="Seat preference">
            <Input
              value={seatPreference}
              onChange={(e) => setSeatPreference(e.target.value)}
              placeholder="e.g. window"
            />
          </Field>
          <p className="text-sm text-slate-600">
            Total: <span className="font-semibold">{formatCurrency(total)}</span>
          </p>
          <Button
            className="w-full"
            loading={create.isPending}
            disabled={seatsByClass[flightClass] <= 0}
            onClick={() => create.mutate()}
          >
            Book flight
          </Button>
        </>
      )}
    </Card>
  )
}
