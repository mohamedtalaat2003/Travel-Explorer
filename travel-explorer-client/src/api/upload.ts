import { api } from '@/lib/apiClient'
import type { UploadResult } from '@/types/api'

export const uploadApi = {
  image: (file: File) => {
    const fd = new FormData()
    fd.append('file', file)
    return api.post<UploadResult>('/Upload/image', fd).then((r) => r.data)
  },
  images: (files: File[]) => {
    const fd = new FormData()
    files.forEach((f) => fd.append('files', f))
    return api.post<UploadResult[]>('/Upload/images', fd).then((r) => r.data)
  },
}
