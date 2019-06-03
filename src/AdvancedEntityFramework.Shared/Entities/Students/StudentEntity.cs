using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedEntityFramework.Shared.Entities.Students
{
    [Table("Student")]
    public class StudentEntity : IEntity
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
