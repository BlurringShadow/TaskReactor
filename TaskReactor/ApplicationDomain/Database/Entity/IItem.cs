using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;

namespace ApplicationDomain.Database.Entity
{
    /// <summary>
    /// Single time event
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public interface IItem : IDatabaseModel
    {
        [Required, NotNull] string Title { get; set; }

        string Description { get; set; }

        /// <summary>
        /// Time when it first start
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        DateTime StartTime { get; set; }

        /// <summary>
        /// Time when it end
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        DateTime EndTime { get; set; }
    }
}