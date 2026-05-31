import { useMutation, useQueryClient } from '@tanstack/react-query'
import { Trash2 } from 'lucide-react'
import { useState } from 'react'
import { toast } from 'sonner'
import { reviewsApi } from '@/api/reviews'
import { useAuth } from '@/auth/AuthContext'
import { extractErrorMessage } from '@/lib/apiClient'
import { formatDate } from '@/lib/format'
import type { ReviewDto } from '@/types/api'
import { Button } from './ui/Button'
import { Card } from './ui/Card'
import { Rating } from './ui/Rating'
import { EmptyState } from './ui/States'
import { Textarea } from './ui/Textarea'

export function ReviewSection({
  destinationId,
  reviews,
}: {
  destinationId: number
  reviews: ReviewDto[]
}) {
  const { user, isAuthenticated, isTraveler, isAdmin } = useAuth()
  const qc = useQueryClient()
  const myReview = user ? reviews.find((r) => r.userId === user.id) : undefined
  const [rating, setRating] = useState(myReview?.rating ?? 5)
  const [comment, setComment] = useState(myReview?.comment ?? '')

  const invalidate = () => qc.invalidateQueries({ queryKey: ['destination', destinationId] })

  const save = useMutation({
    mutationFn: () =>
      myReview
        ? reviewsApi.update(myReview.id, { rating, comment })
        : reviewsApi.create({ rating, comment, destinationId }),
    onSuccess: () => {
      toast.success('Review saved')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  const remove = useMutation({
    mutationFn: (id: number) => reviewsApi.remove(id),
    onSuccess: () => {
      toast.success('Review deleted')
      invalidate()
    },
    onError: (e) => toast.error(extractErrorMessage(e)),
  })

  return (
    <div className="space-y-4">
      <h2 className="text-lg font-semibold">Reviews ({reviews.length})</h2>

      {isAuthenticated && isTraveler && (
        <Card className="space-y-3 p-4">
          <p className="text-sm font-medium text-slate-700">
            {myReview ? 'Edit your review' : 'Write a review'}
          </p>
          <Rating value={rating} onChange={setRating} size={22} />
          <Textarea
            value={comment}
            onChange={(e) => setComment(e.target.value)}
            placeholder="Share your experience"
          />
          <Button size="sm" loading={save.isPending} onClick={() => save.mutate()}>
            {myReview ? 'Update review' : 'Submit review'}
          </Button>
        </Card>
      )}

      {reviews.length === 0 ? (
        <EmptyState title="No reviews yet" description="Be the first to share your experience." />
      ) : (
        <div className="space-y-3">
          {reviews.map((r) => (
            <Card key={r.id} className="p-4">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <span className="font-medium text-slate-700">{r.userFullName || 'Traveler'}</span>
                  <Rating value={r.rating} readOnly size={14} />
                </div>
                <div className="flex items-center gap-3">
                  <span className="text-xs text-slate-400">{formatDate(r.createdAt)}</span>
                  {isAdmin && (
                    <button
                      onClick={() => remove.mutate(r.id)}
                      className="text-slate-400 hover:text-red-600"
                      aria-label="Delete review"
                    >
                      <Trash2 className="h-4 w-4" />
                    </button>
                  )}
                </div>
              </div>
              {r.comment && <p className="mt-2 text-sm text-slate-600">{r.comment}</p>}
            </Card>
          ))}
        </div>
      )}
    </div>
  )
}
