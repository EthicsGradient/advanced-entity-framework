using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedEntityFramework.Api.Models.CreateStudent
{
    public class CreateStudentRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
    }
}
