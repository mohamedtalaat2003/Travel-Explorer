import { Loader2, Upload, X } from 'lucide-react'
import { useState } from 'react'
import { toast } from 'sonner'
import { uploadApi } from '@/api/upload'
import { extractErrorMessage } from '@/lib/apiClient'
import { Button } from './ui/Button'
import { Input } from './ui/Input'

interface Props {
  value: string[]
  onChange: (urls: string[]) => void
  multiple?: boolean
}

export function ImageUpload({ value, onChange, multiple = true }: Props) {
  const [uploading, setUploading] = useState(false)
  const [urlInput, setUrlInput] = useState('')

  async function handleFiles(files: FileList | null) {
    if (!files || files.length === 0) return
    setUploading(true)
    try {
      const arr = Array.from(files)
      const results = multiple ? await uploadApi.images(arr) : [await uploadApi.image(arr[0])]
      const urls = results.map((r) => r.url)
      onChange(multiple ? [...value, ...urls] : urls.slice(0, 1))
    } catch (e) {
      toast.error(extractErrorMessage(e, 'Upload failed. Paste an image URL instead.'))
    } finally {
      setUploading(false)
    }
  }

  function addUrl() {
    const url = urlInput.trim()
    if (!url) return
    onChange(multiple ? [...value, url] : [url])
    setUrlInput('')
  }

  function removeAt(index: number) {
    onChange(value.filter((_, i) => i !== index))
  }

  return (
    <div className="space-y-2">
      {value.length > 0 && (
        <div className="flex flex-wrap gap-2">
          {value.map((url, i) => (
            <div
              key={`${url}-${i}`}
              className="relative h-20 w-20 overflow-hidden rounded-lg border border-slate-200"
            >
              <img src={url} alt="" className="h-full w-full object-cover" />
              <button
                type="button"
                onClick={() => removeAt(i)}
                className="absolute right-0 top-0 bg-black/50 p-0.5 text-white"
              >
                <X className="h-3.5 w-3.5" />
              </button>
            </div>
          ))}
        </div>
      )}
      <div className="flex flex-wrap items-center gap-2">
        <label className="inline-flex cursor-pointer items-center gap-2 rounded-lg border border-dashed border-slate-300 px-3 py-2 text-sm text-slate-600 hover:bg-slate-50">
          {uploading ? <Loader2 className="h-4 w-4 animate-spin" /> : <Upload className="h-4 w-4" />}
          <span>Upload</span>
          <input
            type="file"
            accept="image/*"
            multiple={multiple}
            className="hidden"
            onChange={(e) => handleFiles(e.target.files)}
            disabled={uploading}
          />
        </label>
        <div className="flex items-center gap-2">
          <Input
            placeholder="or paste an image URL"
            value={urlInput}
            onChange={(e) => setUrlInput(e.target.value)}
            className="w-52"
          />
          <Button type="button" variant="outline" size="sm" onClick={addUrl}>
            Add
          </Button>
        </div>
      </div>
    </div>
  )
}
