using System.Collections.Generic;

namespace AdvancedEntityFramework.Api.Models.ListStudents
{
    public class ListStudentsResponse
    {
        public ListStudentsResponse(IReadOnlyCollection<Student> students)
        {
            Students = students;
        }

        public IReadOnlyCollection<Student> Students { get; }
    }
}