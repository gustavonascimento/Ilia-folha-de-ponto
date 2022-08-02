using System;
namespace WebApi.Helpers
{
    public static class DateTimeExtensions
    {
        public static bool IsValidTimeFormat(this string input)
        {
            return TimeSpan.TryParse(input, out var dummyOutput);
        }
    }
}

