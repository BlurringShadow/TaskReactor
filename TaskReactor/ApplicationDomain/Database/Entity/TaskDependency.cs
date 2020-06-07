using System.ComponentModel.DataAnnotations;

namespace ApplicationDomain.Database.Entity
{
    /// <summary>
    /// Design for task dependency
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class TaskDependency : DatabaseModel
    {
        [Required] public int TargetId { get; set; }
        [Required] public UserTask Target { get; set; }

        [Required] public int DependencyId { get; set; }
        [Required] public UserTask Dependency { get; set; }
    }
}