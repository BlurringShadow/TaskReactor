using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;

namespace ApplicationDomain.Database.Entity
{
    /// <summary>
    /// Design for repeated event.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public interface ISchedule : IItem
    {
        /// <summary>
        /// The time it cost to finish
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        TimeSpan DurationOfOneTime { get; set; }

        [Required, NotNull] Interval Interval { get; set; }
    }
}