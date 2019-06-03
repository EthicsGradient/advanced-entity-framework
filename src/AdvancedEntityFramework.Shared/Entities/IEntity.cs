using System;

namespace AdvancedEntityFramework.Shared.Entities
{
    public interface IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}