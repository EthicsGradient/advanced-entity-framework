using System;

namespace AdvancedEntityFramework.Api.Extensions
{
    public static class DateTimeExtensions
    {
        // https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c#answer-1404
        public static int ToAge(this DateTime extended)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - extended.Year;
            
            // Go back to the year the person was born in case of a leap year
            if (extended.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
