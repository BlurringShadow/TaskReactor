using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace ApplicationDomain.Models.Database.Entity
{
    /// <summary>
    /// Item for task dependency
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class TaskDependency : DatabaseModel
    {
        [Required, NotNull] public UserTask Target { get; set; }

        [Required, NotNull] public UserTask Dependency { get; set; }
    }
}