// Drops undefined / null / empty-string values so they aren't sent as query params.
export function cleanParams<T extends object>(params: T): Partial<T> {
  const out: Record<string, unknown> = {}
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue
    out[key] = value
  }
  return out as Partial<T>
}
