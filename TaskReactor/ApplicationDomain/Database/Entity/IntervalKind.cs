namespace ApplicationDomain.Database.Entity
{
    /// <summary>
    /// Repetition mode
    /// </summary>
    public enum IntervalKind : byte
    {
        YearByWeek,
        YearByDay,
        MonthByWeek,
        MonthByDay,
        ByWeek,
        ByDay
    }
}