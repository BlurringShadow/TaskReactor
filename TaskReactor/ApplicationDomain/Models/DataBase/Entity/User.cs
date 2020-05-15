using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Utilities;

namespace ApplicationDomain.Models.Database.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class User : DataBaseModel
    {
        public int Id { get; set; }

        [NotNull, Required] public string Name { get; set; }

        [NotNull, Required] public string Password { get; set; }

        [InverseProperty(nameof(Schedule.OwnerUser))]
        public IList<Schedule> Schedules { get; set; }
    }
}