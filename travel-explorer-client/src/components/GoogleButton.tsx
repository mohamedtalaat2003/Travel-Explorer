import { toast } from 'sonner'
import { Button } from './ui/Button'

// Google sign-in is not configured yet (no OAuth credentials), so the button
// shows a "coming soon" notice instead of starting the broken OAuth flow.
export function GoogleButton(_: { mode: 'login' | 'register' }) {
  return (
    <Button
      type="button"
      variant="outline"
      className="w-full"
      onClick={() => toast('Google sign-in coming soon')}
    >
      Continue with Google
    </Button>
  )
}
