using System;

namespace AdvancedEntityFramework.Api.Models.FindStudent
{
    public class FindStudentResponse
    {
        public FindStudentResponse(Guid id, string firstName, string lastName,
            DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
    }
}
