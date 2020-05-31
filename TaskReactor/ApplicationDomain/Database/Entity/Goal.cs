using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace ApplicationDomain.Database.Entity
{
    /// <summary>
    /// Each task contains several goals to complete
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class Goal : Schedule, IIdentityKey
    {
        public int Id { get; set; }

        [Required, NotNull] public UserTask FromTask { get; set; }
    }
}