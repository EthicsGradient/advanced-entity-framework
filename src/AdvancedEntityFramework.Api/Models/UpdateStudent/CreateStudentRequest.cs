using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedEntityFramework.Api.Models.UpdateStudent
{
    public class UpdateStudentRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
    }
}
