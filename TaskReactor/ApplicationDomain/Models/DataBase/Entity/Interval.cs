using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Models.DataBase.Entity
{
    [Owned]
    public class Interval
    {
        public IntervalKind Kind { get; set; }

        public byte Value { get; set; }
    }
}