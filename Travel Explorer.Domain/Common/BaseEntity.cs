namespace Travel_Explorer.Domain.Common
{
    /// <summary>
    /// Base class for all domain entities.
    /// Provides common audit properties shared across every entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>Unique primary key — auto-incremented by the database.</summary>
        public int Id { get; set; }

        /// <summary>Timestamp of when this record was first created.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Timestamp of the last modification. Null if never updated.</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Soft-delete flag. True means logically deleted, not physically removed.</summary>
        public bool IsDeleted { get; set; } = false;
    }

}
