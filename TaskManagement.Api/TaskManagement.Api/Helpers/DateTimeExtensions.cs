namespace TaskManagement.Api.Helpers
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo VietnamTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        public static DateTime ToVietnamTime(this DateTime utcDateTime)
        {
            // Nếu DateTime chưa phải UTC thì convert về UTC trước
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, VietnamTimeZone);
        }

        public static DateTime? ToVietnamTime(this DateTime? utcDateTime)
        {
            if (utcDateTime == null)
                return null;

            return utcDateTime.Value.ToVietnamTime();
        }
    }
}
