namespace Gerald.Core.Entities;

/// <summary>
/// Base entity class for all domain entities.
/// Provides common properties like Id and timestamps.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
