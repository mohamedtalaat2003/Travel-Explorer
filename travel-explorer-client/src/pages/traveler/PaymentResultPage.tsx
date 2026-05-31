import { CheckCircle2, XCircle } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Button } from '@/components/ui/Button'

export function PaymentResultPage({ success }: { success: boolean }) {
  return (
    <div className="mx-auto flex max-w-md flex-col items-center px-4 py-20 text-center">
      {success ? (
        <CheckCircle2 className="h-14 w-14 text-green-500" />
      ) : (
        <XCircle className="h-14 w-14 text-red-500" />
      )}
      <h1 className="mt-4 text-2xl font-bold text-slate-900">
        {success ? 'Payment successful' : 'Payment failed'}
      </h1>
      <p className="mt-2 text-slate-500">
        {success
          ? 'Your booking is being confirmed. Check My Trips for the latest status.'
          : 'Your payment did not complete. You can try again from My Trips.'}
      </p>
      <Link to="/my/bookings" className="mt-6">
        <Button>Go to My Trips</Button>
      </Link>
    </div>
  )
}
