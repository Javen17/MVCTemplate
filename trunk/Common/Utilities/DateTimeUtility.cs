using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Utilities
{
	/// <summary>
	/// Represents a set of <see cref="DateTime"/> utility methods.
	/// </summary>
	public static class DateTimeUtility
	{
		/// <summary>
		/// Creates a new <see cref="DateTime"/> object that represents the same time in Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static DateTime ToUniversalTime(DateTime dateTime)
		{
			return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
		}

		/// <summary>
		/// Creates a new <see cref="DateTime"/> object that represents the same time in Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static DateTime? ToUniversalTime(DateTime? dateTime)
		{
			if (null == dateTime)
			{
				return null;
			}

			return ToUniversalTime(dateTime.Value);
		}

		/// <summary>
		/// Creates a new <see cref="DateTime"/> object that represents the same time in local time using the specified time zone offset.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <param name="utcOffset">The UTC offset.</param>
		/// <param name="isDaylightSavingsTime">if set to <c>true</c> the the computation should consider daylight savings time.</param>
		/// <returns></returns>
		public static DateTime ToLocalTime(DateTime dateTime, TimeSpan utcOffset, bool isDaylightSavingsTime)
		{
			switch (dateTime.Kind)
			{
				case DateTimeKind.Local:
					return dateTime;
				case DateTimeKind.Utc:
				case DateTimeKind.Unspecified:
				default:
					return ToLocalTime(dateTime.Ticks, utcOffset, isDaylightSavingsTime);
			}
		}

		/// <summary>
		/// Creates a new <see cref="DateTime"/> object that represents the same time in local time using the specified time zone offset.
		/// </summary>
		/// <param name="ticks">The ticks.</param>
		/// <param name="utcOffset">The UTC offset.</param>
		/// <param name="isDaylightSavingsTime">if set to <c>true</c> the the computation should consider daylight savings time.</param>
		/// <returns></returns>
		public static DateTime ToLocalTime(long ticks, TimeSpan utcOffset, bool isDaylightSavingsTime)
		{
			return new DateTime(ticks + ComputeUtcOffset(utcOffset, isDaylightSavingsTime).Ticks, DateTimeKind.Local);
		}

		/// <summary>
		/// Computes the UTC offset.
		/// </summary>
		/// <param name="utcOffset">The UTC offset.</param>
		/// <param name="isDaylightSavingsTime">if set to <c>true</c> [is daylight savings time].</param>
		/// <returns></returns>
		public static TimeSpan ComputeUtcOffset(TimeSpan utcOffset, bool isDaylightSavingsTime)
		{
			DaylightTime daylight = TimeZone.CurrentTimeZone.GetDaylightChanges(DateTime.UtcNow.Year);
			if (DateTime.UtcNow >= daylight.Start && DateTime.UtcNow <= daylight.End && isDaylightSavingsTime)
			{
				return utcOffset - daylight.Delta;
			}
			return utcOffset;
		}

		/// <summary>
		/// Sets the time for the specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <param name="hour">The hour.</param>
		/// <param name="minute">The minute.</param>
		/// <param name="second">The second.</param>
		/// <param name="milisecond">The milisecond.</param>
		/// <param name="kind">The kind.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static DateTime SetTime(DateTime dateTime, int hour, int minute, int second, int milisecond, DateTimeKind kind)
		{
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second, milisecond, kind);
		}

		/// <summary>
		/// Truncates the specified date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="resolution">The resolution.  e.g. TimeSpan.TicksPerSecond</param>
		/// <returns></returns>
		public static DateTime Truncate(this DateTime date, long resolution)
		{
			return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
		}


		/// <summary>
		/// Used to get the list of localized days of the week
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static List<string> GetLocalizedDayOfWeekValues(CultureInfo culture)
		{
			return GetLocalizedDayOfWeekValues(culture, culture.DateTimeFormat.FirstDayOfWeek);
		}

		/// <summary>
		/// Used to get a localized day of the week
		/// </summary>
		/// <param name="culture"></param>
		/// <param name="startDay"></param>
		/// <returns></returns>
		public static List<string> GetLocalizedDayOfWeekValues(CultureInfo culture, DayOfWeek startDay)
		{
			string[] dayNames = culture.DateTimeFormat.DayNames;
			IEnumerable<string> query = dayNames
				.Skip((int)startDay)
				.Concat(
					dayNames.Take((int)startDay)
				);

			return query.ToList();
		}


		/// <summary>
		/// This will return the culture approperate first day of the week.
		/// </summary>
		/// <param name="now">the date to find the first day of that date's week</param>
		/// <param name="firstDayOfWeek"></param>
		/// <returns></returns>
		public static DateTime GetFirstDayOfWeek(DateTime now, int firstDayOfWeek)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			now = now.AddDays(isBeforeStartOfDay ? -1 : 0);
			return now.AddDays(-(((int)now.DayOfWeek - firstDayOfWeek + 7) % 7)).Date;
		}

		/// <summary>
		/// This is used to translate a .Net ticks since epoch to java ticks since epoch
		/// </summary>
		/// <remarks>
		/// .Net works off of dates from 01/01/0001 00:00:00 and Java works off of 01/01/1970 00:00:00 GMT
		/// Microsoft.Net: 
		///   A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond.
		///   The value of this property represents the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001, which represents DateTime.MinValue. It does not include the number of ticks that are attributable to leap seconds.
		/// Java:
		///   Ticks are the number of milliseconds since January 1, 1970, 00:00:00 GMT
		/// </remarks>
		/// <param name="cSharpSource">a .Net <see cref="DateTime"/> object to convert</param>
		/// <returns>Java friendly ticks since UNIX epoch</returns>
		public static long AsJavaFriendlyTicks(this DateTime cSharpSource)
		{
			return ((cSharpSource.Ticks - 621355968000000000L) / 10000);
		}

		/// <summary>
		/// Used to take a java ticks and make them .net ticks
		/// </summary>
		/// <param name="javaSource"></param>
		/// <returns></returns>
		public static long AsDotNetFriendlyTicks(this long javaSource)
		{
			return ((javaSource * 10000) + 621355968000000000L);
		}

		/// <summary>
		/// Used to take a java ticks and turn it into a date
		/// </summary>
		/// <param name="javaSource"></param>
		/// <returns></returns>
		public static DateTime AsDotNetDate(this long javaSource)
		{
			return DateTime.FromFileTime(javaSource.AsDotNetFriendlyTicks());
		}


		static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), System.DateTimeKind.Utc);


		/// <summary>
		/// This gets linux ticks
		/// </summary>
		/// <returns></returns>
		public static long CurrentTimeMillis()
		{
			return (DateTime.UtcNow.Ticks - Epoch.Ticks) / TimeSpan.TicksPerMillisecond;
		}

		public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
		{
			// Java timestamp is millisecods past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(Math.Round(javaTimeStamp / 1000)).ToLocalTime();
			return dtDateTime;
		}


		/// <summary>
		/// Used to get a date string that is used to help interact with java
		/// </summary>
		/// <param name="sourceHour"></param>
		/// <returns></returns>
		public static string GetHourlyDateKey(DateTime sourceHour)
		{
			return string.Format(
				"{0}{1}{2}{3}",
				sourceHour.Year,
				sourceHour.Month.ToString().PadLeft(2, '0'),
				sourceHour.Day.ToString().PadLeft(2, '0'),
				sourceHour.Hour.ToString().PadLeft(2, '0'));
		}

		/// <summary>
		/// Used to get the first day of a given month
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime FirstDayOfMonth(this DateTime now)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			now = now.AddDays(isBeforeStartOfDay ? -1 : 0);
			return new DateTime(now.Year, now.Month, 1);
		}

		/// <summary>
		/// Used to get the last day of a given month 
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime LastDayOfMonth(this DateTime now)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			now = now.AddDays(isBeforeStartOfDay ? -1 : 0);
			return now.AddDays(1 - (now.Day)).AddMonths(1).AddDays(-1);
		}

		private const int DayStartHour = 6;

		/// <summary>
		/// Used to get the start[0] and end[1] of of the supplied date
		/// </summary>
		/// <param name="now"></param>
		/// <returns></returns>
		public static DateTime[] AsStartEndOfDate(this DateTime now)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			return new[]
			{
				isBeforeStartOfDay ? now.Date.AddDays(-1).AddHours(DayStartHour) : now.Date.AddHours(DayStartHour),
				isBeforeStartOfDay ? now.Date.AddHours(DayStartHour - 1).AddMinutes(59).AddSeconds(59).AddMilliseconds(999) : now.AddDays(1).Date.AddHours(DayStartHour - 1).AddMinutes(59).AddSeconds(59).AddMilliseconds(999)

			};
		}

		/// <summary>
		/// Used to get the first day of the year
		/// </summary>
		/// <param name="now">date that you want to find the first day of the year for</param>
		/// <returns>1/1/{year from supplied date}</returns>
		public static DateTime FirstDayOfYear(this DateTime now)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			now = now.AddDays(isBeforeStartOfDay ? -1 : 0);
			return new DateTime(now.Year, 1, 1);
		}

		/// <summary>
		/// Used to get the last day of the year 
		/// </summary>
		/// <param name="now">date that you want to find the last day of the year for</param>
		/// <returns>12/31/{year from supplied date}</returns>
		public static DateTime LastDayOfYear(this DateTime now)
		{
			bool isBeforeStartOfDay = now.Hour < DayStartHour;
			now = now.AddDays(isBeforeStartOfDay ? -1 : 0);
			return new DateTime(now.Year, 12, 31);
		}
	}
}