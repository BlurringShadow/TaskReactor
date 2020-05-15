using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Models.Database.Entity
{
    [Owned]
    public class Interval
    {
        public IntervalKind Kind { get; set; }

        public byte Value { get; set; }
    }
}