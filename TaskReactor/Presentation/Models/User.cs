using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Presentation.Models
{
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class User
    {
        public int Id { get; set; }

        [JetBrains.Annotations.NotNull, Required]
        public string Name { get; set; }

        [JetBrains.Annotations.NotNull, Required]
        public string Password { get; set; }

        [InverseProperty(nameof(Schedule.OwnerUser))]
        public IList<Schedule> Schedules { get; set; }
    }
}