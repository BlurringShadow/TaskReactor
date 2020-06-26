using Microsoft.EntityFrameworkCore;

namespace Data.Database.Entity
{
    /// <summary>
    /// <para> Design for schedule repetition. </para>
    /// <example>
    /// Such as
    /// <code>Interval{Kind = MonthByWeek, Value = 2}</code>
    /// Define a schedule which happened the 2nd week every month
    /// </example>
    /// </summary>
    [Owned]
    public class Interval
    {
        public IntervalKind Kind { get; set; }

        public byte Value { get; set; }
    }
}