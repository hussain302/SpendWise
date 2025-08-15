using Application.Services;

namespace Infrastructure.Services;
public class DateTimeService : IDateTimeService
{
    public DateTime DateTimeUtc => DateTime.UtcNow;
    public DateTime DateTimeLocal => DateTime.Now;
    public DateTime ConvertToTimeZone(DateTime dateTime, string timeZoneId)
    {
        try
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), timeZone);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new ArgumentException($"The timezone '{timeZoneId}' was not found.");
        }
        catch (InvalidTimeZoneException)
        {
            throw new ArgumentException($"The timezone '{timeZoneId}' is invalid.");
        }
    }
    public TimeSpan GetTimeDifference(DateTime fromDateTime, DateTime toDateTime)
    {
        return toDateTime - fromDateTime;
    }
    public bool IsWeekend(DateTime dateTime)
    {
        var dayOfWeek = dateTime.DayOfWeek;
        return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
    }
    public bool IsLeapYear(int year)
    {
        return DateTime.IsLeapYear(year);
    }
    public string FormatDateTime(DateTime dateTime, string format)
    {
        try
        {
            return dateTime.ToString(format);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid date format.");
        }
    }
    public DateTime AddDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(days);
    }
    public DateTime AddHours(DateTime dateTime, int hours)
    {
        return dateTime.AddHours(hours);
    }
    public DateTime SubtractDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(-days);
    }
    public bool IsHoliday(DateTime dateTime)
    {
        // Placeholder for a holiday-checking mechanism
        // In a real-world scenario, you could check against a list of predefined holidays.
        // This is just a placeholder that returns false.
        return false;
    }
}
