import { FileText } from 'lucide-react'
import { Link } from 'react-router-dom'
import { formatDate } from '@/lib/format'
import type { BlogDto } from '@/types/api'
import { Card } from './ui/Card'

export function BlogCard({ b }: { b: BlogDto }) {
  return (
    <Link to={`/blogs/${b.id}`}>
      <Card className="group h-full overflow-hidden transition hover:shadow-md">
        <div className="aspect-[16/9] w-full overflow-hidden bg-slate-100">
          {b.imageUrl ? (
            <img
              src={b.imageUrl}
              alt={b.title}
              className="h-full w-full object-cover transition group-hover:scale-105"
            />
          ) : (
            <div className="flex h-full items-center justify-center text-slate-300">
              <FileText className="h-10 w-10" />
            </div>
          )}
        </div>
        <div className="space-y-1 p-4">
          <h3 className="line-clamp-1 font-semibold text-slate-800">{b.title}</h3>
          <p className="line-clamp-2 text-sm text-slate-500">{b.content}</p>
          <p className="pt-1 text-xs text-slate-400">{formatDate(b.createdAt)}</p>
        </div>
      </Card>
    </Link>
  )
}
