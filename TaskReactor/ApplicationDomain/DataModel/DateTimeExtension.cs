﻿using System;

namespace ApplicationDomain.DataModel
{
    static class DateTimeExtension
    {
        public static DateTime AddWeeks(this DateTime dateTime, double value) => dateTime.AddDays(7 * value);

        public static int WeekOfMonth(this DateTime dateTime) =>
            (int)MathF.Ceiling(dateTime.AddDays(-(int)dateTime.DayOfWeek).Day / 7f);
    }
}