namespace Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToColombiaTime(this DateTime dateTime)
        {
            var colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, colombiaTimeZone);
        }
    }
}
