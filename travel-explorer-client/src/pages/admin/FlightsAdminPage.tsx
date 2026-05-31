import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'
import { flightsApi } from '@/api/flights'
import { Button } from '@/components/ui/Button'
import { Card } from '@/components/ui/Card'
import { Field } from '@/components/ui/Field'
import { Input } from '@/components/ui/Input'
import { Modal } from '@/components/ui/Modal'
import { Pagination } from '@/components/ui/Pagination'
import { EmptyState, ErrorState, LoadingState } from '@/components/ui/States'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatDateTime } from '@/lib/format'
import type { FlightScheduleDto } from '@/types/api'

const schema = z.object({
  airline: z.string().min(1, 'Airline is required'),
  flightNumber: z.string().min(1, 'Flight number is required'),
  departureCity: z.string().min(1, 'Departure city is required'),
  arrivalCity: z.string().min(1, 'Arrival city is required'),
  departureTime: z.string().min(1, 'Departure time is required'),
  arrivalTime: z.string().min(1, 'Arrival time is required'),
  economyPrice: z.coerce.number().min(0),
  businessPrice: z.coerce.number().min(0),
  firstClassPrice: z.coerce.number().min(0),
  availableEconomySeats: z.coerce.number().min(0),
  availableBusinessSeats: z.coerce.number().min(0),
  availableFirstClassSeats: z.coerce.number().min(0),
})
type FormValues = z.infer<typeof schema>

const toLocalInput = (iso: string) => {
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return ''
  const pad = (n: number) => String(n).padStart(2, '0')
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(
    d.getMinutes(),
  )}`
}

export function FlightsAdminPage() {
  const qc = useQueryClient()
  const [page, setPage] = useState(1)
  const [editing, setEditing] = useState<FlightScheduleDto | 'new' | null>(null)

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['admin-flights', page],
    queryFn: () => flightsApi.list({ pageNumber: page, pageSize: 10 }),
  })
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) })

  const invalidate = () => qc.invalidateQueries({ queryKey: ['admin-flights'] })

  function openNew() {
    setEditing('new')
    reset({
      airline: '',
      flightNumber: '',
      departureCity: '',
      arrivalCity: '',
      departureTime: '',
      arrivalTime: '',
      economyPrice: 0,
      businessPrice: 0,
      firstClassPrice: 0,
      availableEconomySeats: 0,
      availableBusinessSeats: 0,
      availableFirstClassSeats: 0,
    })
  }
  function openEdit(f: FlightScheduleDto) {
    setEditing(f)
    reset({
      airline: f.airline,
      flightNumber: f.flightNumber,
      departureCity: f.departureCity,
      arrivalCity: f.arrivalCity,
      departureTime: toLocalInput(f.departureTime),
      arrivalTime: toLocalInput(f.arrivalTime),
      economyPrice: f.economyPrice,
      businessPrice: f.businessPrice,
      firstClassPrice: f.firstClassPrice,
      availableEconomySeats: f.availableEconomySeats,
      availableBusinessSeats: f.availableBusinessSeats,
      availableFirstClassSeats: f.availableFirstClassSeats,
    })
  }

  const save = useMutation({
    mutationFn: (v: FormValues) =>
      editing && editing !== 'new' ? flightsApi.update(editing.id, v) : flightsApi.create(v),
    onSuccess: () => {
      toast.success('Flight saved')
      setEditing(null)
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })
  const del = useMutation({
    mutationFn: (id: number) => flightsApi.remove(id),
    onSuccess: () => {
      toast.success('Flight deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-slate-900">Flights</h1>
        <Button onClick={openNew}>
          <Plus className="h-4 w-4" /> New
        </Button>
      </div>
      <Card className="mt-4 overflow-x-auto">
        {isLoading ? (
          <LoadingState />
        ) : isError ? (
          <ErrorState onRetry={refetch} />
        ) : !data || data.items.length === 0 ? (
          <EmptyState title="No flights" />
        ) : (
          <table className="w-full text-sm">
            <thead className="border-b border-slate-200 text-left text-slate-500">
              <tr>
                <th className="p-3">Flight</th>
                <th className="p-3">Route</th>
                <th className="p-3">Departs</th>
                <th className="p-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((f) => (
                <tr key={f.id} className="border-b border-slate-100">
                  <td className="p-3 font-medium text-slate-800">
                    {f.airline} {f.flightNumber}
                  </td>
                  <td className="p-3 text-slate-500">
                    {f.departureCity} - {f.arrivalCity}
                  </td>
                  <td className="p-3 text-slate-500">{formatDateTime(f.departureTime)}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-2">
                      <Button size="sm" variant="outline" onClick={() => openEdit(f)}>
                        Edit
                      </Button>
                      <Button
                        size="sm"
                        variant="danger"
                        onClick={() => window.confirm('Delete this flight?') && del.mutate(f.id)}
                      >
                        Delete
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      {data && (
        <Pagination
          page={data.pageNumber}
          pageSize={data.pageSize}
          totalCount={data.totalCount}
          onPageChange={setPage}
        />
      )}

      <Modal
        open={!!editing}
        onClose={() => setEditing(null)}
        title={editing && editing !== 'new' ? 'Edit flight' : 'New flight'}
        footer={
          <>
            <Button variant="outline" onClick={() => setEditing(null)}>
              Cancel
            </Button>
            <Button loading={save.isPending} onClick={handleSubmit((v) => save.mutate(v))}>
              Save
            </Button>
          </>
        }
      >
        <form className="space-y-3" onSubmit={handleSubmit((v) => save.mutate(v))}>
          <div className="grid grid-cols-2 gap-3">
            <Field label="Airline" error={errors.airline?.message} required>
              <Input {...register('airline')} />
            </Field>
            <Field label="Flight number" error={errors.flightNumber?.message} required>
              <Input {...register('flightNumber')} />
            </Field>
            <Field label="From" error={errors.departureCity?.message} required>
              <Input {...register('departureCity')} />
            </Field>
            <Field label="To" error={errors.arrivalCity?.message} required>
              <Input {...register('arrivalCity')} />
            </Field>
            <Field label="Departure" error={errors.departureTime?.message} required>
              <Input type="datetime-local" {...register('departureTime')} />
            </Field>
            <Field label="Arrival" error={errors.arrivalTime?.message} required>
              <Input type="datetime-local" {...register('arrivalTime')} />
            </Field>
            <Field label="Economy price">
              <Input type="number" step="0.01" {...register('economyPrice')} />
            </Field>
            <Field label="Economy seats">
              <Input type="number" {...register('availableEconomySeats')} />
            </Field>
            <Field label="Business price">
              <Input type="number" step="0.01" {...register('businessPrice')} />
            </Field>
            <Field label="Business seats">
              <Input type="number" {...register('availableBusinessSeats')} />
            </Field>
            <Field label="First class price">
              <Input type="number" step="0.01" {...register('firstClassPrice')} />
            </Field>
            <Field label="First class seats">
              <Input type="number" {...register('availableFirstClassSeats')} />
            </Field>
          </div>
        </form>
      </Modal>
    </div>
  )
}
