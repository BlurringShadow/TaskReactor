using Microsoft.EntityFrameworkCore;

namespace Presentation.Models
{
    [Owned]
    public class Interval
    {
        public IntervalKind Kind { get; set; }

        public byte Value { get; set; }
    }
}