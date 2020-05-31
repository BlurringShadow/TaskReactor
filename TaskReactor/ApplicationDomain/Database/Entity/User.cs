using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ApplicationDomain.Database.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class User : DatabaseModel, IIdentityKey
    {
        public int Id { get; set; }

        [Required, NotNull] public string Name { get; set; }

        [Required, NotNull] public string Password { get; set; }

        [InverseProperty(nameof(UserTask.OwnerUser))]
        public IList<UserTask> Tasks { get; set; }
    }
}