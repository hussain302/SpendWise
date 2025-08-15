namespace Application.Services;
public interface IDateTimeService
{
    DateTime DateTimeUtc { get; }
    DateTime DateTimeLocal { get; }
    DateTime ConvertToTimeZone(DateTime dateTime, string timeZoneId);
    TimeSpan GetTimeDifference(DateTime fromDateTime, DateTime toDateTime);
    bool IsWeekend(DateTime dateTime);
    bool IsLeapYear(int year);
    string FormatDateTime(DateTime dateTime, string format);
    DateTime AddDays(DateTime dateTime, int days);
    DateTime AddHours(DateTime dateTime, int hours);
    DateTime SubtractDays(DateTime dateTime, int days);
    bool IsHoliday(DateTime dateTime);
}