using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;

namespace ApplicationDomain.Models.Database.Entity
{
    /// <summary>
    /// Design for repeated event.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class Schedule : Item
    {
        /// <summary>
        /// The time it cost to finish
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        public TimeSpan DurationOfOneTime { get; set; }

        [Required, NotNull] public Interval Interval { get; set; }
    }
}