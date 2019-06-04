using System.Linq;

namespace AdvancedEntityFramework.Shared.Entities.Students
{
    public static class StudentEntityExtensions
    {
        public static IQueryable<StudentEntity> WhereActive(this IQueryable<StudentEntity> extended)
        {
            return extended.Where(x => x.Active);
        }
    }
}