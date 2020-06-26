using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Data.Database.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class User : KeyedEntity<int>
    {
        [Required, NotNull] public string Name { get; set; }

        [Required, NotNull] public string Password { get; set; }

        [InverseProperty(nameof(UserTask.OwnerUser)), ItemNotNull]
        public IList<UserTask> Tasks { get; set; }
    }
}