using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ApplicationDomain.Database.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class UserTask : Item, IIdentityKey
    {
        public int Id { get; set; }

        [Required, NotNull] public User OwnerUser { get; set; }

        [InverseProperty(nameof(Goal.FromTask))]
        public IList<Goal> Goals { get; set; }
    }
}