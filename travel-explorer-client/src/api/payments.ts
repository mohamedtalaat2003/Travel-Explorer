import { api } from '@/lib/apiClient'
import type { CheckoutResponse } from '@/types/api'

export const paymentsApi = {
  checkout: (bookingId: number, provider = 'Paymob') =>
    api
      .post<CheckoutResponse>(`/payments/checkout/${bookingId}`, null, { params: { provider } })
      .then((r) => r.data),
}
